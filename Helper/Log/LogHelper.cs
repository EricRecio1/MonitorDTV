using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Runtime.CompilerServices;
using Helper.Error;

namespace Helper
{
    public class LogHelper
    {
        private static LogHelper instance = null;
        private static object synclock = new object();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LogHelper()
        {
        }
        private ILog Log()
        {
            return log;
        }

        public void PrintDebug(string text,
                                [CallerLineNumber] int lineNumber = 0,
                                [CallerMemberName] string caller = null,
                                [CallerFilePath] string callingFilePath = null)
        {
            log.Debug(text + " linea " + lineNumber + " (" + caller + " archivo " + callingFilePath + ")");
        }
        public void PrintError(Exception exception)
        {
            log.Debug(ErrorHelper.ErrorToString(exception));
        }
        public void PrintError(string text,
                                [CallerLineNumber] int lineNumber = 0,
                                [CallerMemberName] string caller = null,
                                [CallerFilePath] string callingFilePath = null)
        {
            log.Debug(text + " linea " + lineNumber + " (" + caller + " archivo " + callingFilePath + ")");
        }

        public static LogHelper GetInstance()
        {
                if (LogHelper.instance == null)
                {
                    lock (synclock)
                    {
                        if (LogHelper.instance == null)
                        {
                            LogHelper.instance = new LogHelper();
                            //var configFileDirectory = (new DirectoryInfo(TraceExtension.AssemblyDirectory)).Parent;
                            //log4net.Config.XmlConfigurator.Configure(new FileInfo(configFileDirectory.FullName + "\\Web.config") );
                            //var dllPath = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
                            var dllPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
                            //dllPath.ToString() + "\\Web.config")
                            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
                        }
                    }
                }
                return LogHelper.instance; 
        }

        public void Refresh()
        {
            instance = null;
        }
    }
}
