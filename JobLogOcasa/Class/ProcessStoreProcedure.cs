using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLogOcasa.Class
{
    public class ProcessStoreProcedure
    {
        public string name { get; set; }
        public List<ProcessStoreProcedureParameters> parameters { get; set; }

        public ProcessStoreProcedure()
        {
            parameters = new List<ProcessStoreProcedureParameters>();
        }

    }
}
