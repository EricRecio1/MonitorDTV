/****** Object:  StoredProcedure [dbo].[Monitor]    Script Date: 24/11/2023 09:17:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- OCASA 2023
-- Muestra la cantidad de mensajes recibidos por aplicacion en el día
-- Ejemplo
--			exec [Monitor] @id_tipo_log=null, @fecha='2023-11-17',@id_aplicacion=null,@id_estado=2
--			exec [Monitor] @id_tipo_log=0, @fecha='1900-01-01'
--			exec [Monitor] @id_tipo_log=null,@fecha='2023-10-17',@id_aplicacion=6
CREATE procedure [dbo].[Monitor]
@id_tipo_log int=null,
@fecha datetime=null,
@id_aplicacion bigint=null,
@id_estado int=null
as
begin
	
	if @fecha is null or @fecha <= '1900-01-01'
		set @fecha = CONVERT(DATETIME, CONVERT(VARCHAR(10),GETDATE(),120))
 	
	--set @fecha = '2022-09-19'

	-- Lista los registros del día
	
	select * from 
		(
		select	app.id,
				app.nombre,
				app.activo,
				app.descripcion,
				app.max_mensajes_error,
				cant.id_tipo_log,
				cant.cantidad_log,
				case 
					when cant.Cantidad_Log>=app.max_mensajes_error and cant.Id_tipo_log=3 then 'Critico' 
					when cant.Cantidad_Log<app.max_mensajes_error and cant.Cantidad_Log > 2 then 'Atención'
					when app.max_mensajes_error is null and cant.Cantidad_Log > 2 then 'Atención'
					else 'Normal'
				end 'criticidad',
				case 
					when cant.Cantidad_Log>=app.max_mensajes_error and cant.Id_tipo_log=3 then 3 
					when cant.Cantidad_Log<app.max_mensajes_error and cant.Cantidad_Log > 2 then 2
					when cant.id_tipo_log = 1 then 1
					when cant.id_tipo_log = 2 then 2
					when app.max_mensajes_error is null and cant.Cantidad_Log > 2 then 2
					when app.max_mensajes_error is null then 1
					else 0
				end 'nivel'
				,ser.nombre_dominio as 'servidor'
				,tip.descripcion 'tipo_log'
		from aplicacion app
		
		left join (
			select 
				mon.id_aplicacion, mon.id_tipo_log,count(1) as 'cantidad_log'
				
			from monitor_log mon			
			where 
				(mon.Fecha >= @fecha and mon.Fecha < dateadd(dd,1,@fecha))
				and (mon.id_tipo_log=@id_tipo_log or @id_tipo_log is null or @id_tipo_log = 0)
				and (mon.id_estado_log=@id_estado or @id_estado is null)
 			group by mon.Id_Aplicacion, mon.Id_tipo_log
			) cant on cant.Id_Aplicacion = app.Id
		left join servidor ser on ser.id=app.id_servidor
		inner join tipo_log tip on tip.id=cant.id_tipo_log		
		where (app.id=@id_aplicacion or @id_aplicacion is null or @id_aplicacion=0)
		) res
	--where res.id
	order by res.Nivel desc, res.Cantidad_Log desc

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_ActualizarEstadoLog]    Script Date: 24/11/2023 09:17:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Actualiza el estado del log
-- Ejemplo
--			exec Monitor_ActualizarEstadoLog 10676,'REVI'
-- Estados posibles: PEND - REVI - SOLU
CREATE procedure [dbo].[Monitor_ActualizarEstadoLog]
@id_log	bigint,
@clave_estado varchar(20)
as
begin
	declare @id_estado int

	select @id_estado=id from estado_log where clave=@clave_estado
	if @id_estado>0
	begin

		update monitor_log 
		set id_estado_log=@id_estado where id=@id_log

	end
	if @@ROWCOUNT <> 0	
		select @id_log as 'Id','Actualizacion correcta' as 'Descripcion','OK' as 'Respuesta'
	else
		select 0 as 'Id','No actualizado' as 'Descripcion', 'ERROR' as 'Respuesta'
	

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_AgregarApp]    Script Date: 24/11/2023 09:17:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Inserta un registro en la tabla de aplicacion
-- Ejemplo
--			
CREATE procedure [dbo].[Monitor_AgregarApp]
@Nombre			varchar(200),
@Activo			bit,
@Descripcion	varchar(2000),
@Especificaciones varchar(2000)=null,
@Url_Git		varchar(2000)=null,
@Id_Servidor	bigint=null,
@Url_Documentos	varchar(2000)=null,
@Max_Mensajes_Error	int=null,
@Id_Cliente		int=null

as
begin

	declare @respuesta varchar(10)
	declare @respuesta_descripcion varchar(max)
	declare @validaciones bit
	declare @Id_insertado bigint

	set @respuesta=''
	set @respuesta_descripcion='No fue posible ingresar la apliación'
	set @validaciones=1
	set @Id_insertado=0

	begin try
	
		if not exists(select * from [aplicacion] where nombre=@nombre and
			url_git=@url_git)
		begin
			-- Validacion de servidor
			if not exists(select * from servidor where id=@Id_Servidor) and 
				( not @Id_Servidor is null or @Id_Servidor > 0)
			begin
				set @validaciones=0
				set @respuesta_descripcion='El Id de servidor ['+convert(varchar(10),@Id_Servidor) +'] no existe en la BD'
			end
			if @validaciones=1
			begin

				insert into [aplicacion] 
						(nombre,fecha_alta,fecha_creacion,activo,descripcion,especificaciones,url_git,
						id_servidor,url_documentos,max_mensajes_error,id_cliente)
						values(
						@Nombre,getdate(),getdate(),@Activo,@Descripcion,@Especificaciones,@Url_Git,
						@Id_Servidor,@Url_Documentos,@Max_Mensajes_Error,@Id_Cliente
						)
				if @@ERROR=0
				begin
					set @Id_insertado=convert(bigint, SCOPE_IDENTITY())
					set @respuesta_descripcion='Aplicacion '+@nombre +' ingresada correctamente'
					set @respuesta='OK'
				end
				
			end
			
		end
		else
		begin
			set @respuesta_descripcion='La aplicacion '+@nombre +' ya existe en la BD'
			set @respuesta='ERROR'
		end
	end try
	begin catch
		set @respuesta='ERROR'
		select @respuesta_descripcion = ERROR_MESSAGE() + ' - linea SP - ' + convert(varchar(20),ERROR_LINE())

	end catch
		

	
	select @Id_insertado as 'Id',@respuesta_descripcion as 'Descripcion',@respuesta as 'Respuesta'

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_AgregarLog]    Script Date: 24/11/2023 09:17:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Inserta un registro en la tabla de log
-- Ejemplo
--			Monitor_AgregarLog 1,1,'Prueba con agrupador','STORED PROCEDURE',null,null,null,'A1'
CREATE procedure [dbo].[Monitor_AgregarLog]
@Id_Aplicacion	int,
@Id_Tipo_Log	int,
@Descripcion	text,
@Procedencia	varchar(50)=null,
@Id_Cliente		int,
@Descripcion_Paquete	text=null,
@Descripcion_Error		text=null,
@Descripcion_Respuesta	text=null,
@Codigo_Agrupador		varchar(50)=null
as
begin

	declare @respuesta varchar(10)
	declare @respuesta_descripcion varchar(max)
	declare @respuesta_app varchar(100)
	declare @respuesta_tipo varchar(100)
	declare @existe_app bit
	declare @existe_tipo bit
	declare @estado_log int

	set @respuesta_app=''
	set @respuesta_tipo=''
	set @existe_app=0
	set @existe_tipo=0
	set @estado_log=1 -- Pendiente


	set @respuesta='OK'
	set @respuesta_descripcion='Log insertado correctamente'

	if exists(select 1 from aplicacion where id=@Id_Aplicacion or @Id_Aplicacion=0) set @existe_app=1
	else set @respuesta_app = @respuesta_app + '[El id aplicacion no existe]'

	if exists(select 1 from tipo_log where id=@Id_Tipo_Log) set @existe_tipo=1
	else set @respuesta_tipo = @respuesta_tipo + '[El tipo de log no existe]'

	if @existe_app=1 and
	   @existe_tipo=1 
	begin

		begin try

		select @estado_log=id from estado_log where clave='PEND'
		if @estado_log is null set @estado_log=1

		insert into [monitor_log] 
							(id_aplicacion,fecha,id_tipo_log,id_cliente,descripcion,procedencia,descripcion_paquete,
							descripcion_error,descripcion_respuesta,codigo_agrupador,id_estado_log)
		values		(@Id_Aplicacion,getdate(),@Id_Tipo_Log,@Id_Cliente,@Descripcion,@Procedencia,@Descripcion_Paquete,
					@Descripcion_Error,@Descripcion_Respuesta,@Codigo_Agrupador,1)
		end try
		begin catch
			set @respuesta='ERROR'
			select @respuesta_descripcion = ERROR_MESSAGE()

		end catch

	end
	else
	begin
		set @respuesta='ERROR'
		set @respuesta_descripcion = 'Se produjo el siguiente error,' 
			+ @respuesta_app 
			+ ' ' 
			+ @respuesta_tipo + ', verifique integridad referencial de las tablas'
	end

	if @@ERROR<>0
	begin
		set @respuesta='ERROR'
		set @respuesta_descripcion = 'No fue posible insertar el log : ' + ERROR_MESSAGE()
	end
	
	select @respuesta as 'Respuesta',@respuesta_descripcion as 'Descripcion'

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_AgregarServidor]    Script Date: 24/11/2023 09:17:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Inserta un registro en la tabla de servidores
-- Ejemplo
--			
CREATE procedure [dbo].[Monitor_AgregarServidor]
@Nombre_Dominio	varchar(200),
@Ip_V4			varchar(50),
@Ip_V6			varchar(50),
@Activo			bit,
@Productivo		bit,
@Descripcion	varchar(2000)
as
begin

	declare @respuesta varchar(10)
	declare @respuesta_descripcion varchar(max)
	
	declare @Id_insertado bigint

	set @respuesta=''
	set @respuesta_descripcion='No fue posible ingresar el servidor'
	
	set @Id_insertado=0

	begin try
	
		if not exists(select * from [servidor] where upper(nombre_dominio)=upper(@Nombre_Dominio) 
			or
			(ip_v4=@Ip_V4 and not @Ip_V4 is null and @Ip_V4<>'') 
			or 
			(ip_v6=@Ip_V6 and not @Ip_V6 is null and @Ip_V6<>''))
		begin
			
			if @Nombre_Dominio<>'' and @Ip_V4<>''
			begin 
				insert into [servidor] 
						(fecha_alta, nombre_dominio,ip_v4,ip_v6,activo,productivo,descripcion)
						values(	getdate(),@Nombre_Dominio,@Ip_V4,@Ip_V6,@Activo,@Productivo,@Descripcion )
				if @@ERROR=0
				begin
					set @Id_insertado=convert(bigint, SCOPE_IDENTITY())
					set @respuesta_descripcion='Servidor '+@Nombre_Dominio +' ingresado correctamente'
					set @respuesta='OK'
				end
			end
				
		end
		else
		begin
			set @respuesta_descripcion='El servidor '+@Nombre_Dominio +' ya existe en la BD'
			set @respuesta='ERROR'
		end
	end try
	begin catch
		set @respuesta='ERROR'
		select @respuesta_descripcion = ERROR_MESSAGE() + ' - linea SP - ' + convert(varchar(20),ERROR_LINE())

	end catch
		
	select @Id_insertado as 'Id',@respuesta_descripcion as 'Descripcion',@respuesta as 'Respuesta'

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_CrearConfiguracion]    Script Date: 24/11/2023 09:17:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Inserta un registro en la tabla de configuracion
-- Ejemplo
--		exec Monitor_CrearConfiguracion 1,'ultima_hora_procesada','08:00:00'
CREATE procedure [dbo].[Monitor_CrearConfiguracion]
@id_aplicacion int,
@clave varchar(50),
@valor varchar(1000)
as
begin

	if not exists(select * from configuracion where id_aplicacion=@id_aplicacion
					and clave=@clave)
	begin
		insert into configuracion(id_aplicacion,clave,valor)
			values(@id_aplicacion,@clave,@valor)
		if @@ERROR=0
			select SCOPE_IDENTITY() as 'id',@id_aplicacion as 'id_aplicacion',@clave as 'clave',@valor as 'valor'	
	end
	else
		select * from configuracion where id_aplicacion=@id_aplicacion and clave=@clave
end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_LeerConfigJob]    Script Date: 24/11/2023 09:17:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Lee los registros de la configuracion para el log
-- Ejemplo
--		exec Monitor_LeerConfigLog
CREATE procedure [dbo].[Monitor_LeerConfigJob]
as
begin

	select * from configuracion_job_log

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_LimpiarLogs]    Script Date: 24/11/2023 09:17:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Limpia los logs
-- Ejemplo
--			Monitor_CleanLog
create procedure [dbo].[Monitor_LimpiarLogs]
@datefrom as datetime=null
as
begin

	-- Si no se especifica una fecha de corte
	-- elimina los registros de 90 días para atras
	delete from monitor_log 
	where ( fecha <= @datefrom and fecha < getdate()-90) or
		  ( fecha < getdate()-90) 
	
end


GO

/****** Object:  StoredProcedure [dbo].[Monitor_ListarAplicacion]    Script Date: 24/11/2023 09:17:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- OCASA 2023 
-- Lista los tipos de logs
-- Ejemplo Monitor_ListarAplicacion
--		   Monitor_ListarAplicacion 1
create procedure [dbo].[Monitor_ListarAplicacion]
@id bigint=null
as
begin
	select * from aplicacion
	where @id is null or id=@id
end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_ListarConfiguracion]    Script Date: 24/11/2023 09:17:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Lista todas las configuraciones de los jobs
-- Ejemplo
--		exec Monitor_ListarConfiguracion
create procedure [dbo].[Monitor_ListarConfiguracion]
@id_aplicacion int=null
as
begin

select cf.id_aplicacion, ap.nombre, cf.clave, cf.valor
  FROM [AmbientesIT].[configuracion] cf
  inner join [AmbientesIT].[aplicacion] ap on ap.id=cf.id_aplicacion
  where id_aplicacion=@id_aplicacion or @id_aplicacion is null
  order by id_aplicacion asc, clave asc

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_ListarLog]    Script Date: 24/11/2023 09:17:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- OCASA 2023 
-- Listado de registro en la tabla de log
-- Ejemplo
--			Monitor_ListarLog null,'2023-10-05','1'
--			Monitor_ListarLog 1,'2023-07-06',1
CREATE procedure [dbo].[Monitor_ListarLog]
@id_aplicacion int=null,
@fecha datetime=null,
@id_tipo_log int=null,
@buscar	varchar(max)=null
as
begin

declare @estado_log_descripcion varchar(100)
declare @estado_clave varchar(20)
set @estado_log_descripcion='estado no definido'
set @estado_clave='PEND'

	if @fecha is null or @fecha <= '1900-01-01' or @fecha =''
		set @fecha = CONVERT(DATETIME, CONVERT(VARCHAR(10),GETDATE(),120))

	select 
		ml.id,
		ml.id_aplicacion,
		ml.fecha,
		ml.id_tipo_log,
		ml.id_cliente,
		ml.descripcion,
		ml.procedencia,
		ml.descripcion_paquete,
		ml.descripcion_error,
		ml.descripcion_respuesta,
		ml.codigo_agrupador,
		coalesce(ml.id_estado_log,0) as 'id_estado',
		coalesce(el.clave,@estado_clave) as 'clave_estado',
		coalesce(el.descripcion,@estado_log_descripcion) as 'descripcion_estado'
	from monitor_log ml
	left join estado_log el on el.id=ml.id_estado_log
	where 
		(id_aplicacion = @id_aplicacion or @id_aplicacion is null or @id_aplicacion=0) 
		and
		((fecha >= @fecha and fecha < dateadd(d,1,@fecha)) or @fecha is null or @fecha='1900-01-01 00:00:00.000')
		and 
		(id_tipo_log = @id_tipo_log or @id_tipo_log is null)
	/*	and 
		(
		(Descripcion like '%' + @buscar + '%') or
		(Procedencia like '%' + @buscar + '%') or
		(Descripcion_Paquete like '%' + @buscar + '%' ) or
		(Descripcion_Error like '%' + @buscar + '%' ) or
		(Descripcion_Respuesta like '%' + @buscar + '%' ) or
		(codigo_agrupador like '%' + @buscar + '%' ) or @buscar is null)
		*/
	
end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_ListarTipoLog]    Script Date: 24/11/2023 09:17:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- OCASA 2023 
-- Lista los tipos de logs
-- Ejemplo Monitor_ListarTipoLog
--		   Monitor_ListarTipoLog 'ADV'
CREATE procedure [dbo].[Monitor_ListarTipoLog]
@clave varchar(20)=null
as
begin
	select * from tipo_log
	where @clave is null or clave=@clave
end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_ObtenerConfiguracionJob]    Script Date: 24/11/2023 09:17:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Muestra la cantidad de mensajes recibidos por aplicacion en el día
-- Ejemplo
--			exec dbo.Monitor_ObtenerConfiguracionJob 1
CREATE procedure [dbo].[Monitor_ObtenerConfiguracionJob]
@id_aplicacion int=null
as
begin

	--select 
	--id, id_aplicacion,[path],extension,
	--clasifica_subcarpetas,
	----case when ultima_fecha_procesada is null then getdate()-1 else ultima_fecha_procesada end as 'ultima_fecha_procesada',
	--ultima_fecha_procesada,
	--case when ultima_linea_procesada is null then 0 else ultima_linea_procesada end as 'ultima_linea_procesada',
	--case when palabra_clave_busqueda is null then '' else palabra_clave_busqueda end as 'palabra_clave_busqueda',
	--parametros
	--from configuracion_job_log where id_aplicacion=@id_aplicacion

	select id,id_aplicacion,clave,valor from configuracion 
	where id_aplicacion=@id_aplicacion or @id_aplicacion is null
	order by id_aplicacion asc, clave desc

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_ObtenerJobs]    Script Date: 24/11/2023 09:17:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Obtiene los jobs configurados
-- Ejemplo
--			exec Monitor_ObtenerJobs
CREATE procedure [dbo].[Monitor_ObtenerJobs]
as
begin

	select * from [jobs] where activo = 1
	
end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_Reindex_Identity]    Script Date: 24/11/2023 09:17:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Re-indexa el identity de una tabla
-- Ejemplo
-- exec Monitor_Reindex_Identity 'configuracion',24

create procedure [dbo].[Monitor_Reindex_Identity]
@tabla as varchar(200),
@value as int
as
begin

	DBCC CHECKIDENT (@tabla, RESEED, @value);

end
GO

/****** Object:  StoredProcedure [dbo].[Monitor_UpdateConfiguration]    Script Date: 24/11/2023 09:17:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- OCASA 2023 
-- Actualiza un valor de configuracion para los jobs
-- Ejemplo
--		
create procedure [dbo].[Monitor_UpdateConfiguration]
@id_applicacion int,
@clave varchar(50),
@valor varchar(1000)
as
begin
	update [configuracion]
	set valor=@valor
	where id_aplicacion=@id_applicacion and clave=@clave

end
GO

