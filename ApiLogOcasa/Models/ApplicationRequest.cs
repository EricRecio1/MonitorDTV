using System;
using Newtonsoft.Json;

namespace ApiLogOcasa.Models
{
    public class ApplicationRequest
    {
        
        [JsonProperty("nombre")]
        public string nombre { get; set; }
        [JsonProperty("fecha_alta")]
        public DateTime fecha_alta { get; set; }
        [JsonProperty("fecha_creacion")]
        public DateTime fecha_creacion { get; set; }
        [JsonProperty("activo")]
        public bool activo { get; set; }
        [JsonProperty("descripcion")]
        public string descripcion { get; set; }
        [JsonProperty("especificiaciones")]
        public string especificiaciones { get; set; }
        [JsonProperty("url_git")]
        public string url_git { get; set; }
        [JsonProperty("id_servidor")]
        public long id_servidor { get; set; }
        [JsonProperty("url_documentos")]
        public string url_documentos { get; set; }
        [JsonProperty("max_mensajes_error")]
        public int max_mensajes_error { get; set; }
        [JsonProperty("id_cliente")]
        public int id_cliente { get; set; }

    }
}