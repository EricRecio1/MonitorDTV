using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443,https://monitordelogs.ocasa.com:443,https://monitordelogs.ocasa.com", headers: "*", methods: "*")]
    public class ListErrorsController : ApiController
    {
        
        [HttpPost]
        [Route("api/ListErrors")]
        public HttpResponseMessage ListErrors(LogListRequest param)
        {
            HttpRequestMessage request = this.ActionContext.Request;
            Records<DTV_Tabla_Error> records=new Records<DTV_Tabla_Error>();
            GenericResponse bad_ret = new GenericResponse();
            try
            {
                dbServices db = new dbServices();
                
                records = db.ListErrors(new StoredProcedure()
                {
                    name = "sp_dtv_FindErrors",
                    parameters = AddParameters(param)
                });
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

        private List<StoredProcedureParameters> AddParameters(LogListRequest log)
        {
            return (new List<StoredProcedureParameters>()
            {
                new StoredProcedureParameters()
                {
                    name="@nroDoc",
                    type=System.Data.SqlDbType.Decimal,
                    value= (log.nroDoc.ToString() ??  DBNull.Value.ToString())
                },
                new StoredProcedureParameters()
                {
                    name="@estado",
                    type=System.Data.SqlDbType.NVarChar,
                    value= (log.estado.ToString() ?? DBNull.Value.ToString())
                },
                new StoredProcedureParameters()
                {
                    name="@fechaDesde",
                    type=System.Data.SqlDbType.VarChar,
                    value=(log.fechaDesde.ToString())
                },
                new StoredProcedureParameters()

                {
                    name="@fechaHasta",
                    type=System.Data.SqlDbType.VarChar,
                    value=(log.fechaHasta.ToString())
                },
                new StoredProcedureParameters()
                {
                    name="@pais",
                    type=System.Data.SqlDbType.NVarChar,
                    value=(log.pais.ToString() ?? DBNull.Value.ToString())
                },
                new StoredProcedureParameters()
                {
                    name="@integracion",
                    type=System.Data.SqlDbType.Int,
                    value=(log.integracion.ToString() ?? DBNull.Value.ToString())
                }
            });
        }
    }
}