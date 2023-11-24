using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443,https://monitordelogs.ocasa.com:443,https://monitordelogs.ocasa.com", headers: "*", methods: "*")]
    public class AddApplicationController : ApiController
    {
        [Route("api/AddApplication")]
        public HttpResponseMessage AddApplication(ApplicationRequest param)
        {
            HttpRequestMessage request = this.ActionContext.Request;
            GenericResponse ret;

            try
            {
                // 1. Verificar estructura de param


                // 2. Insertar 
                dbServices db = new dbServices();
                ret = db.Save(new StoredProcedure()
                {
                    name = "Monitor_AgregarApp",
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


        private List<StoredProcedureParameters> AddParamenters(ApplicationRequest param)
        {
            return (new List<StoredProcedureParameters>()
                     {                        
                         new StoredProcedureParameters()
                         {
                             name="@Nombre",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.nombre.ToString()
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Activo",
                              type=System.Data.SqlDbType.Bit,
                               value=(param.activo?1:0)
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Descripcion",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.descripcion
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Especificaciones",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.especificiaciones
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Url_Git",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.url_git
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Id_Servidor",
                              type=System.Data.SqlDbType.BigInt,
                               value=param.id_servidor
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Url_Documentos",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.url_documentos
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Max_Mensajes_Error",
                              type=System.Data.SqlDbType.Int,
                               value=param.max_mensajes_error
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Id_Cliente",
                              type=System.Data.SqlDbType.Int,
                               value=param.id_cliente
                         }
                     });

        }
    }
}