using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443,https://monitordelogs.ocasa.com:443,https://monitordelogs.ocasa.com", headers: "*", methods: "*")]
    public class ListTypesLogController : ApiController
    {
        [HttpGet]
        [Route("api/ListTypesLog")]
        public HttpResponseMessage ListTypesLog()
        {
            HttpRequestMessage request = this.ActionContext.Request;
            Records<LogType> records = new Records<LogType>();
            GenericResponse ret = new GenericResponse();

            try
            {
                dbServices db = new dbServices();
                records=db.ListLogTypes(new StoredProcedure()
                {
                    name = "Monitor_ListarTipoLog",
                    parameters = null
                });

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
    }
}
