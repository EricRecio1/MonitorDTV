using Newtonsoft.Json;


namespace ApiLogOcasa.Models
{
    public class LogListRequest
    {
        [JsonProperty("id_tipo_log")]
        public int id_tipo_log { get; set; }

        [JsonProperty("fecha")]
        public string fecha { get; set; }
    }
}