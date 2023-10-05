using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobLogOcasa.Class;

namespace JobLogOcasa.Interfase
{
    public interface ILogProcess
    {
        ReturnResponse Process(ListParameters parameters);
    }
}
