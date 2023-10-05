using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443", headers: "*", methods: "*")]
    public class ListLogsController : ApiController
    {
        
        [HttpPost]
        [Route("api/ListLogs")]
        public HttpResponseMessage ListLogs(LogListRequest param)
        {
            HttpRequestMessage request = this.ActionContext.Request;
            Records<SummaryLogsResponse> records=new Records<SummaryLogsResponse>();
            GenericResponse bad_ret = new GenericResponse();
            try
            {
                if (param == null) return (request.CreateResponse(System.Net.HttpStatusCode.BadRequest, bad_ret));

                dbServices db = new dbServices();
                records = db.ListLogs(new StoredProcedure()
                {
                    name = "Monitor",
                    parameters = AddParamenters(param)
                });
                if (db.error_message != "")
                {
                    bad_ret = new GenericResponse()
                    {
                        response = "Error",
                        description = db.error_message
                    };
                    return (request.CreateResponse(System.Net.HttpStatusCode.OK, bad_ret));
                }else                  
                    return (request.CreateResponse(System.Net.HttpStatusCode.OK, records));
            }
            catch(System.Exception ex)
            {
                bad_ret = new GenericResponse()
                {
                    response = "Error",
                    description = ex.Message
                };
                return (request.CreateResponse(System.Net.HttpStatusCode.BadRequest, bad_ret));
            }
        }

        private List<StoredProcedureParameters> AddParamenters(LogListRequest log)
        {
            
            return (new List<StoredProcedureParameters>()
                     {
                         new StoredProcedureParameters()
                         {
                             name="@id_tipo_log",
                              type=System.Data.SqlDbType.Int,
                               value= (log.id_tipo_log.ToString() ?? DBNull.Value.ToString())
                         },
                         new StoredProcedureParameters()
                         {
                             name="@fecha",
                              type=System.Data.SqlDbType.DateTime,
                              value= (log.fecha =="" ? DateTime.Parse("1900-01-01").ToString(): log.fecha)
                         }
                     });

        }
    }
}