using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Error
{
    public class ErrorHelper
    {
        private static string GetRunDirectory()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
        private static string GetMensajeError(Exception exception)
        {
            return exception.Message.ToString();
        }
        private static string GetLinesErrors(Exception exception)
        {
            string result = "";
            var st = new StackTrace(exception, true);
            string callingFilePath = "";
            string caller = "";
            int lineNumber = 0;
            var frames = st.GetFrames();
            foreach (StackFrame frame in frames)
            {
                if ((frame.GetFileLineNumber() < 1))
                {
                    break;
                }
                callingFilePath = frame.GetFileName();
                caller = frame.GetMethod().Name;
                lineNumber = frame.GetFileLineNumber();
                result += " Line " + lineNumber.ToString() + " ( Method: " + caller + " - File: " + callingFilePath + ")" + "\r\n";
            }
            return result;
        }

        public static string ErrorToString(Exception exception)
        {
            string result = "";

            try
            {
                result = "Run In: " + GetRunDirectory() + " - " + "</br>" + "\r\n";
                result += "- Error: " + GetMensajeError(exception) + " - " + "</br>" + "\r\n";
                result += "- Lines: " + GetLinesErrors(exception) + " - " + "</br>" + "\r\n";
            }
            catch (Exception ex)
            {
                result = GetMensajeError(exception);
            }
            return result;
        }

    }
}
