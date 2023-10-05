using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericProcessLog.Class
{
    class GenericResponse
    {
        [JsonProperty("response")]
        public string response { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("operation")]
        public bool operation { get; set; }

        public GenericResponse(){ response = ""; description = ""; operation = true; }
        public GenericResponse(string res, string des, bool oper)
        {
            response = res; description = des; operation = oper;
        }
    }
}
