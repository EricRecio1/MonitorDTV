using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericProcessLog.Interface;

namespace GenericProcessLog.Class
{
    public class Results /*: IReturnResults*/
    {
        public bool operation { get; set; }
        public string message { get; set; }

        public Results() { operation = false; message = string.Empty; }
        public Results(bool operationResult, string messageResult)
        {
            operation = operationResult;
            message = messageResult;
        }
    }
}
