using Newtonsoft.Json;
using System.Collections;

namespace ApiLogOcasa
{
    public class GenericResponse
    {
        [JsonProperty("response")]
        public string response { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("operation")]
        public bool operation { get; set; }


    }
}