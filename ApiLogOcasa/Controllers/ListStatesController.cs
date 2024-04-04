using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443,https://monitordelogs.ocasa.com:443,https://monitordelogs.ocasa.com", headers: "*", methods: "*")]

    public class ListStatesController : ApiController
    {
        [HttpGet]
        [Route("api/ListStates")]
        public HttpResponseMessage ListStates()
        {
            HttpRequestMessage request = this.ActionContext.Request;
            Records<DTV_Moni_Estado> records = new Records<DTV_Moni_Estado>();
            GenericResponse ret = new GenericResponse();

            try
            {
                dbServices db = new dbServices();
                records = db.ListStates("DTV_Moni_Estado");

                return (request.CreateResponse(System.Net.HttpStatusCode.OK, records));

            }
            catch (Exception ex)
            {
                ret = new GenericResponse()
                {
                    response = "Error",
                    description = ex.Message
                };
                return (request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ret));
            }

        }

        private List<StoredProcedureParameters> AddParamenters(ServerRequest param)
        {
            try
            {
                return (new List<StoredProcedureParameters>()
                     {
                         new StoredProcedureParameters()
                         {
                             name="@Nombre_Dominio",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.nombre_dominio.ToString()
                         },
                          new StoredProcedureParameters()
                         {
                             name="@Ip_V4",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.ip_v4.ToString()
                         },
                           new StoredProcedureParameters()
                         {
                             name="@Ip_V6",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.ip_v6.ToString()
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Activo",
                              type=System.Data.SqlDbType.Bit,
                               value=(param.activo?1:0)
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Productivo",
                              type=System.Data.SqlDbType.Bit,
                               value=(param.productivo?1:0)
                         },
                         new StoredProcedureParameters()
                         {
                             name="@Descripcion",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.descripcion
                         }
                     });
            }
            catch (Exception)
            {
                throw new Exception("Error en la estructura de parámetros");
            }
        }
    }
}