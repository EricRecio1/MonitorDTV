using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class StringHelper
    {
        public static string Left(string param, int maxLength)
        {
            if (string.IsNullOrEmpty(param)) return param;
            maxLength = Math.Abs(maxLength);

            return (param.Length <= maxLength
                   ? param
                   : param.Substring(0, maxLength)
                   );
        }

        public static string Right(string param, int length)
        {
            int pos = param.Length - length;
            string result = param.Substring(pos, length);
            return result;
        }
    }
}
