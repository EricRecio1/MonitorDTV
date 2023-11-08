using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLogOcasa.Models
{
    public class UpdateLogStateRequest
    {
        [JsonProperty("id_log")]
        public long id_log { get; set; }
        [JsonProperty("clave_estado")]
        public string clave_estado { get; set; }

    }
}