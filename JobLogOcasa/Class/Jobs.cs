using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLogOcasa.Class
{
    public class Jobs
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public object[] parameters { get; set; }
        public long id_application { get; set; }
    }
}
