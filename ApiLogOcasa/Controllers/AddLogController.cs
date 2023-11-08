using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443", headers: "*", methods: "*")]
    public class AddLogController : ApiController
    {

        [Route("api/AddLog")]
        public HttpResponseMessage AddLog(LogRequest param)
        {
            HttpRequestMessage request = this.ActionContext.Request;            
            GenericResponse ret;

            try
            {
                // 1. Verificar estructura de param


                // 2. Insertar 
                dbServices db = new dbServices();
                ret=db.SaveMessageLog(new StoredProcedure()
                {
                    name = "Monitor_AgregarLog",
                    parameters = AddParamenters(param)
                });

                
                return request.CreateResponse(System.Net.HttpStatusCode.OK, ret);
                
            }
            catch (Exception ex)
            {
                ret = new GenericResponse()
                {
                    response = "Error",
                    description = ex.Message,
                    operation = false
                };
                return request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ret);
            }
            
        }


        private List<StoredProcedureParameters> AddParamenters(LogRequest param)
        {
            return(new List<StoredProcedureParameters>()
                     {
                         new StoredProcedureParameters()
                         {
                             name="@Id_Aplicacion",
                              type=System.Data.SqlDbType.Int,
                               value=param.id_aplicacion.ToString()
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Id_Tipo_Log",
                              type=System.Data.SqlDbType.Int,
                               value=param.id_tipo_log.ToString()
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Descripcion",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.descripcion_general
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Procedencia",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.procedencia
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Id_Cliente",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.id_cliente
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Descripcion_Paquete",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.descripcion_paquete
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Descripcion_Error",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.descripcion_error
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Descripcion_Respuesta",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.descripcion_respuesta
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Codigo_Agrupador",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.codigo_agrupador
                         }
                     });

        }
    }
}