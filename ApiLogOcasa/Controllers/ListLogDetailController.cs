using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443,https://monitordelogs.ocasa.com:443,https://monitordelogs.ocasa.com", headers: "*", methods: "*")]
    public class ListLogDetailController : ApiController
    {


        [Route("api/ListLogDetail")]
        public HttpResponseMessage ListLogDetail(LogDetailRequest param)
        {
            HttpRequestMessage request = this.ActionContext.Request;
            Records<Log> records = new Records<Log>();
            GenericResponse ret=new GenericResponse();

            try {

                dbServices db = new dbServices();
                //records = db.ListLogsDetail(new StoredProcedure()
                //{
                //    name = "Monitor_ListarLog",
                //    parameters = AddParamenters(param)
                //});

                return request.CreateResponse(System.Net.HttpStatusCode.OK, records);
            }
            catch (System.Exception ex){
                ret = new GenericResponse()
                {
                    response = "Error",
                    description = ex.Message
                };
                return request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ret);
            }

            
        }

        private List<StoredProcedureParameters> AddParamenters(LogDetailRequest log)
        {

            return (new List<StoredProcedureParameters>()
                     {
                         new StoredProcedureParameters()
                         {
                             name="@id_aplicacion",
                              type=System.Data.SqlDbType.Int,
                               value= (log.id_aplicacion.ToString() ?? DBNull.Value.ToString())
                         },
                         new StoredProcedureParameters()
                         {
                             name="@fecha",
                              type=System.Data.SqlDbType.DateTime,
                              value= (!IsDateFormat(log.fecha,"dd/MM/yyyy") && !IsDateFormat(log.fecha,"yyyy-MM-dd") ? "1900-01-01": log.fecha)
                         },
                         new StoredProcedureParameters()
                         {
                             name="@id_tipo_log",
                              type=System.Data.SqlDbType.Int,
                               value= (log.id_tipo_log.ToString() ?? DBNull.Value.ToString())
                         },
                         new StoredProcedureParameters()
                         {
                             name="@buscar",
                              type=System.Data.SqlDbType.VarChar,
                              value= (log.buscar ?? DBNull.Value.ToString())
                         }
                     });

        }

        private bool IsDateFormat(string value, string format)
        {
            DateTime result;            
            return DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            
        }
    }
}