using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ApiLogOcasa
{
    public class StoredProcedureParameters
    {
       
        public string name { get; set; }
        
        public SqlDbType type { get; set; }
        
        public object value { get; set; }

    }
}