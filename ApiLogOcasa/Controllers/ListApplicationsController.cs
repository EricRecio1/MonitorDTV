using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443", headers: "*", methods: "*")]

    public class ListApplicationsController : ApiController
    {

        [HttpGet]
        [Route("api/ListApplication")]
        public HttpResponseMessage ListApplication()
        {
            HttpRequestMessage request = this.ActionContext.Request;
            Records<Application> records = new Records<Application>();
            GenericResponse ret = new GenericResponse();

            try
            {
                dbServices db = new dbServices();
                records = db.ListApplications(new StoredProcedure()
                {
                    name = "Monitor_ListarAplicacion",
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