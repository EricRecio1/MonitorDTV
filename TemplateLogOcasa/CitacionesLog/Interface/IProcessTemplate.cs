using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericProcessLog.Interface
{
    public interface IProcessTemplate
    {
        object Process(object[] parameters);
    }
}
