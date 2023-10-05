using System;
using Newtonsoft.Json;
namespace ApiLogOcasa.Models
{
    public class SummaryLogsResponse
    {

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("nombre")]
        public string nombre { get; set; }

        [JsonProperty("activo")]
        public bool activo { get; set; }

        [JsonProperty("descripcion")]
        public string descripcion { get; set; }
        
        [JsonProperty("max_mensajes_error")]
        public int max_mensajes_error { get; set; }

        [JsonProperty("id_tipo_log")]
        public int id_tipo_log { get; set; }

        [JsonProperty("cantidad_log")]
        public int cantidad_log { get; set; }

        [JsonProperty("criticidad")]
        public string criticidad { get; set; }

        [JsonProperty("nivel")]
        public int nivel { get; set; }

        [JsonProperty("servidor")]
        public string servidor { get; set; }

        [JsonProperty("tipo_log")]
        public string tipo_log { get; set; }
    }
}