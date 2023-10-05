using Newtonsoft.Json;

namespace ApiLogOcasa.Models
{
    public class LogDetailRequest
    {
        [JsonProperty("id_aplicacion")]
        public int id_aplicacion { get; set; }

        [JsonProperty("fecha")]
        public string fecha { get; set; }

        [JsonProperty("id_tipo_log")]
        public int id_tipo_log { get; set; }

        [JsonProperty("buscar")]
        public string buscar { get; set; }
    }
}