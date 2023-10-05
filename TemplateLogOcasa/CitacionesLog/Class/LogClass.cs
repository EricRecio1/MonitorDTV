using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GenericProcessLog.Class
{
    public class LogClass
    {
        [JsonProperty("id_aplicacion")]
        public int id_aplicacion { get; set; }

        [JsonProperty("fecha")]
        public DateTime fecha { get; set; }

        [JsonProperty("id_tipo_log")]
        public int id_tipo_log { get; set; }

        [JsonProperty("descripcion_general")]
        public string descripcion_general { get; set; }

        [JsonProperty("id_cliente")]
        public string id_cliente { get; set; }

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
