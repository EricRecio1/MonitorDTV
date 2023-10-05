using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLogOcasa.Models
{
    public class Log
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("id_aplicacion")]
        public int id_aplicacion { get; set; }

        [JsonProperty("fecha")]
        public DateTime fecha { get; set; }

        [JsonProperty("id_tipo_log")]
        public int id_tipo_log { get; set; }

        [JsonProperty("descripcion")]
        public string descripcion { get; set; }

        [JsonProperty("procedencia", NullValueHandling = NullValueHandling.Ignore)]
        public string procedencia { get; set; }

        [JsonProperty("descripcion_paquete", NullValueHandling = NullValueHandling.Ignore)]
        public string descripcion_paquete { get; set; }


        [JsonProperty("descripcion_error", NullValueHandling = NullValueHandling.Ignore)]
        public string descripcion_error { get; set; }


        [JsonProperty("descripcion_respuesta", NullValueHandling = NullValueHandling.Ignore)]
        public string descripcion_respuesta { get; set; }


        [JsonProperty("codigo_agrupador", NullValueHandling = NullValueHandling.Ignore)]
        public string codigo_agrupador { get; set; }

        
    }
}