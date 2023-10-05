using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLogOcasa.Models
{
    public class ServerRequest
    {

        [JsonProperty("nombre_dominio")]
        public string nombre_dominio { get; set; }
        [JsonProperty("ip_v4")]
        public string ip_v4 { get; set; }
        [JsonProperty("ip_v6")]
        public string ip_v6 { get; set; }
        [JsonProperty("activo")]
        public bool activo { get; set; }
        [JsonProperty("productivo")]
        public bool productivo { get; set; }
        [JsonProperty("descripcion")]
        public string descripcion { get; set; }
    }
}