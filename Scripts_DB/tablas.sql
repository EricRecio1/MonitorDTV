/****** Object:  Table [dbo].[aplicacion]    Script Date: 24/11/2023 09:14:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aplicacion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](200) NOT NULL,
	[fecha_alta] [datetime] NOT NULL,
	[fecha_creacion] [datetime] NOT NULL,
	[activo] [bit] NOT NULL,
	[descripcion] [varchar](2000) NOT NULL,
	[especificaciones] [varchar](2000) NULL,
	[url_git] [varchar](2000) NULL,
	[id_servidor] [bigint] NULL,
	[url_documentos] [varchar](2000) NULL,
	[max_mensajes_error] [int] NULL,
	[id_cliente] [int] NULL,
	[id_equipo] [int] NULL,
 CONSTRAINT [PK_aplicacion] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[configuracion]    Script Date: 24/11/2023 09:14:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[configuracion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_aplicacion] [int] NOT NULL,
	[clave] [varchar](50) NOT NULL,
	[valor] [varchar](1000) NOT NULL,
 CONSTRAINT [PK_configuracion] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[estado_log]    Script Date: 24/11/2023 09:14:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[estado_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[clave] [varchar](20) NOT NULL,
	[descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_estado_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[jobs]    Script Date: 24/11/2023 09:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[jobs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[path] [varchar](500) NOT NULL,
	[activo] [bit] NULL,
	[parametros] [varchar](100) NULL,
	[id_aplicacion] [int] NULL,
 CONSTRAINT [PK_jobs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[monitor_log]    Script Date: 24/11/2023 09:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[monitor_log](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[id_aplicacion] [int] NOT NULL,
	[fecha] [datetime] NOT NULL,
	[id_tipo_log] [int] NOT NULL,
	[id_cliente] [int] NULL,
	[descripcion] [text] NOT NULL,
	[procedencia] [varchar](50) NULL,
	[descripcion_paquete] [text] NULL,
	[descripcion_error] [text] NULL,
	[descripcion_respuesta] [text] NULL,
	[codigo_agrupador] [varchar](50) NULL,
	[id_estado_log] [int] NULL,
 CONSTRAINT [PK_monitor_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[servidor]    Script Date: 24/11/2023 09:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[servidor](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[fecha_alta] [datetime] NOT NULL,
	[nombre_dominio] [varchar](100) NOT NULL,
	[ip_v4] [varchar](15) NOT NULL,
	[ip_v6] [varchar](50) NULL,
	[activo] [bit] NOT NULL,
	[productivo] [bit] NOT NULL,
	[descripcion] [varchar](5000) NOT NULL,
 CONSTRAINT [PK_Servidor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[software]    Script Date: 24/11/2023 09:14:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[software](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](200) NOT NULL,
	[fecha_alta] [datetime] NOT NULL,
	[descripcion] [varchar](2000) NOT NULL,
	[id_servidor] [bigint] NOT NULL,
	[version] [varchar](50) NOT NULL,
 CONSTRAINT [PK_software] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[tipo_log]    Script Date: 24/11/2023 09:14:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tipo_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
	[clave] [varchar](20) NULL,
 CONSTRAINT [PK_monitor_tipoLog] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[aplicacion] ADD  CONSTRAINT [DF_aplicacion_esta_activa]  DEFAULT ((1)) FOR [activo]
GO

ALTER TABLE [dbo].[servidor] ADD  CONSTRAINT [DF_servidor_es_activo]  DEFAULT ((1)) FOR [activo]
GO

ALTER TABLE [dbo].[servidor] ADD  CONSTRAINT [DF_servidor_productivo]  DEFAULT ((1)) FOR [productivo]
GO

ALTER TABLE [dbo].[aplicacion]  WITH CHECK ADD  CONSTRAINT [FK_aplicacion_servidor] FOREIGN KEY([id_servidor])
REFERENCES [dbo].[servidor] ([id])
GO

ALTER TABLE [dbo].[aplicacion] CHECK CONSTRAINT [FK_aplicacion_servidor]
GO

ALTER TABLE [dbo].[configuracion]  WITH CHECK ADD  CONSTRAINT [FK_configuracion_aplicacion] FOREIGN KEY([id_aplicacion])
REFERENCES [dbo].[aplicacion] ([id])
GO

ALTER TABLE [dbo].[configuracion] CHECK CONSTRAINT [FK_configuracion_aplicacion]
GO

ALTER TABLE [dbo].[jobs]  WITH CHECK ADD  CONSTRAINT [FK_jobs_aplicacion] FOREIGN KEY([id_aplicacion])
REFERENCES [dbo].[aplicacion] ([id])
GO

ALTER TABLE [dbo].[jobs] CHECK CONSTRAINT [FK_jobs_aplicacion]
GO

ALTER TABLE [dbo].[monitor_log]  WITH CHECK ADD  CONSTRAINT [FK_monitor_log_aplicacion] FOREIGN KEY([id_aplicacion])
REFERENCES [dbo].[aplicacion] ([id])
GO

ALTER TABLE [dbo].[monitor_log] CHECK CONSTRAINT [FK_monitor_log_aplicacion]
GO

ALTER TABLE [dbo].[monitor_log]  WITH CHECK ADD  CONSTRAINT [FK_monitor_log_estado] FOREIGN KEY([id_estado_log])
REFERENCES [dbo].[estado_log] ([id])
GO

ALTER TABLE [dbo].[monitor_log] CHECK CONSTRAINT [FK_monitor_log_estado]
GO

ALTER TABLE [dbo].[monitor_log]  WITH CHECK ADD  CONSTRAINT [FK_monitor_log_tipo_log] FOREIGN KEY([id_tipo_log])
REFERENCES [dbo].[tipo_log] ([id])
GO

ALTER TABLE [dbo].[monitor_log] CHECK CONSTRAINT [FK_monitor_log_tipo_log]
GO

ALTER TABLE [dbo].[software]  WITH CHECK ADD  CONSTRAINT [FK_software_servidor] FOREIGN KEY([id_servidor])
REFERENCES [dbo].[servidor] ([id])
GO

ALTER TABLE [dbo].[software] CHECK CONSTRAINT [FK_software_servidor]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id de registro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id de la aplicacion que esta notificando' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'id_aplicacion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha del log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'fecha'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador de tipo de log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'id_tipo_log'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción general del log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'descripcion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción de la procedencia del log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'procedencia'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contenido del paquete que origino el log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'descripcion_paquete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripcion del error notificado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'descripcion_error'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción de la respuesta obtenida/otorgada por parte de la app que originó el log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'descripcion_respuesta'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Codigo agrupador de mensajes de log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'monitor_log', @level2type=N'COLUMN',@level2name=N'codigo_agrupador'
GO

