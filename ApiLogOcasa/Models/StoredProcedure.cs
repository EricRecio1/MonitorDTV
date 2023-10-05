using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLogOcasa
{
    public class StoredProcedure
    {
        public string name { get; set; }
        public List<StoredProcedureParameters> parameters { get; set; }

        public StoredProcedure()
        {
            parameters = new List<StoredProcedureParameters>();
        }
    }
}