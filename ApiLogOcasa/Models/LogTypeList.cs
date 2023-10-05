using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLogOcasa
{
    public class LogTypeList
    {
        public List<LogType> Types { get; set; }
        public LogTypeList()
        {
            Types = new List<LogType>();
        }
    }
}