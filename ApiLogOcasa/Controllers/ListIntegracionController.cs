using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    //[EnableCors(origins: "https://localhost:44351,https://localhost:443,https://monitor-dtv.ocasa.com:443,https://monitor-dtv.ocasa.com", headers: "*", methods: "*")]
    public class ListIntegracionController : ApiController
    {
        [HttpGet]
        [Route("api/ListIntegracion")]
        public HttpResponseMessage ListIntegracion()
        {
            HttpRequestMessage request = this.ActionContext.Request;
            Records<DTV_Moni_Integ> records = new Records<DTV_Moni_Integ>();
            GenericResponse ret = new GenericResponse();

            try
            {
                dbServices db = new dbServices();
                records = db.ListIntegracion("DTV_Moni_Integ");

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
