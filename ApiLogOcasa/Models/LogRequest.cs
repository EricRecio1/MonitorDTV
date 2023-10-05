using System;
using Newtonsoft.Json;

namespace ApiLogOcasa.Models
{
    public class LogRequest
    {
        [JsonProperty("id_aplicacion")]
        public int id_aplicacion { get; set; }

        [JsonProperty("id_tipo_log")]
        public int id_tipo_log { get; set; }

        [JsonProperty("descripcion_general")]
        public string descripcion_general { get; set; }

        [JsonProperty("procedencia", NullValueHandling = NullValueHandling.Ignore)]
        public string procedencia { get; set; }

        [JsonProperty("id_cliente", NullValueHandling = NullValueHandling.Ignore)]
        public string id_cliente { get; set; }


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