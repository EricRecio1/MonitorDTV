using ApiLogOcasa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogOcasa.Controllers
{
    [EnableCors(origins: "https://localhost:44351,https://localhost:443", headers: "*", methods: "*")]
    public class AddServerController : ApiController
    {
        [Route("api/AddServer")]
        public HttpResponseMessage AddServer(ServerRequest param)
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
                    name = "Monitor_AgregarServidor",
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