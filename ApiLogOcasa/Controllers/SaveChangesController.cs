using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ApiLogOcasa.Models;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443,https://monitor-dtv.ocasa.com:443,https://monitor-dtv.ocasa.com", headers: "*", methods: "*")]
    public class SaveChangesController : ApiController
    {
        [HttpPost]
        [Route("api/SaveChanges")]
        public HttpResponseMessage SaveChanges(AddChange param)
        {
            HttpRequestMessage request = this.ActionContext.Request;
            Records<DTV_Moni_Error> records = new Records<DTV_Moni_Error>();
            GenericResponse ret = new GenericResponse();

            try
            {
                dbServices db = new dbServices();
                db.SaveChanges("DTV_Moni_Error", param);

                return request.CreateResponse(System.Net.HttpStatusCode.OK, records);
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
    }
}
