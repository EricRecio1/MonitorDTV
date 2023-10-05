using Newtonsoft.Json;

namespace ApiLogOcasa
{
    public class LogType
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }

    }
}