using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443,https://monitordelogs.ocasa.com:443,https://monitordelogs.ocasa.com", headers: "*", methods: "*")]
    public class UpdateLogStateController : ApiController
    {

        [Route("api/UpdateLogState")]
        [HttpPatch]
        public HttpResponseMessage UpdateLogState(UpdateLogStateRequest param)
        {
            HttpRequestMessage request = this.ActionContext.Request;
            GenericResponse ret = new GenericResponse();

            try
            {
                if (param == null) return (request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ret));

                dbServices db = new dbServices();
                ret = db.Save(new StoredProcedure()
                {
                    name = "Monitor_ActualizarEstadoLog",
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

        private List<StoredProcedureParameters> AddParamenters(UpdateLogStateRequest param)
        {
            try
            {
                return (new List<StoredProcedureParameters>()
                     {
                         new StoredProcedureParameters()
                         {
                             name="@id_log",
                              type=System.Data.SqlDbType.BigInt,
                               value=param.id_log.ToString()
                         },
                          new StoredProcedureParameters()
                         {
                             name="@clave_estado",
                              type=System.Data.SqlDbType.VarChar,
                               value=param.clave_estado.ToString()
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