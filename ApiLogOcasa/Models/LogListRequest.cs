using Newtonsoft.Json;
using System;


namespace ApiLogOcasa.Models
{
    public class LogListRequest
    {
        [JsonProperty("nroDoc")]
        public Int64 nroDoc { get; set; }

        [JsonProperty("estado")]
        public string estado { get; set; }

        [JsonProperty("fechaDesde")]
        public string fechaDesde { get; set; }

        [JsonProperty("fechaHasta")]
        public string fechaHasta { get; set; }
        
        [JsonProperty("pais")]
        public string pais { get; set; }

        [JsonProperty("integracion")]
        public string integracion { get; set; }
    }
}