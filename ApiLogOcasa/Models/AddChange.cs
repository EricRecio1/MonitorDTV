using Newtonsoft.Json;

namespace ApiLogOcasa
{
    public class AddChange
    {
        [JsonProperty("id_docu")]
        public decimal id_docu { get; set; }
        [JsonProperty("id_estado")]
        public decimal id_estado { get; set; }
        [JsonProperty("id_responsable")]
        public decimal id_responsable { get; set; }
        [JsonProperty("observacion")]
        public string observacion { get; set; }

    }
}