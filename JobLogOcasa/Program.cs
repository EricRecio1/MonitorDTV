using JobLogOcasa.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using log4net.Config;
using log4net;

namespace JobLogOcasa
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        static void Main(string[] args)
        {
            Records<Jobs> list_config = new Records<Jobs>();
            dbServices services = new dbServices();              
            string file = string.Empty;
            List<Template> template_list = new List<Template>();
            Template template;
            
            string error_messages = string.Empty;

            BasicConfigurator.Configure();

            try
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

                //log.Info("Prueba de log");

                /**********************************************/
                string stored_procedure_jobs = System.Configuration.ConfigurationManager.AppSettings["StoredProcedureJobsName"];
                string path_templates = System.Configuration.ConfigurationManager.AppSettings["PathTemplates"];

                // Configuracion para temporizador de ejecuciones
                //DateTime StartDate= DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["StartDate"]); // 
                //DateTime EndDate  = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["EndDate"]);   // 
                //string StartHour= System.Configuration.ConfigurationManager.AppSettings["StartHour"];                   // 
                //string EndHour  = System.Configuration.ConfigurationManager.AppSettings["EndHour"];                     // 
                //int Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Interval"]);            // 
                //string WeekDays = System.Configuration.ConfigurationManager.AppSettings["WeekDays"];                    // 
                /// infinite - none(corre y termina)-- >
                /// infinite : tiene en cuenta todos los parametros
                /// none: no tiene en cuenta los parametros y solo corre cuando se lo invoca desde afuera, solo una vez 
                //string RunMode = System.Configuration.ConfigurationManager.AppSettings["RunMode"];
                /**********************************************/

                string template_start_path = (path_templates.Contains("~/")? Application.StartupPath + @"\" + path_templates.Replace("~/", ""): path_templates);    


                // Busca la configuracion de templates
                
                list_config = services.ListJobs(new ProcessStoreProcedure()
                {
                    name = stored_procedure_jobs
                });
                
                // Recorre todos los datos configurados de Jobs
                if(list_config.count>0)
                {
                    // Carga las dll definidas en la configuracion
                    foreach(Jobs job in list_config.items)
                    {

                        file = template_start_path + @"\" + job.path;
                        if (File.Exists(file))
                        {
                            var DLL = Assembly.LoadFrom(file);
                            // Exploración del template
                            foreach (Type class_type in DLL.GetExportedTypes())
                            {

                                if (!class_type.IsAbstract)
                                {

                                    MemberInfo[] info = class_type.GetMembers();
                                    MethodInfo Mymethodinfo;
                                    ParameterInfo[] paramInfo;
                                    foreach (MemberInfo item in info)
                                    {
                                        if (item.MemberType == MemberTypes.Method)
                                        {
                                            Mymethodinfo = class_type.GetMethod(((System.Reflection.MethodBase)item).Name);
                                            if (Mymethodinfo.IsFinal && ((System.Reflection.MethodBase)item).IsPublic)
                                            {
                                                if (!((System.Reflection.MethodBase)item).IsAbstract)
                                                {
                                                    paramInfo = Mymethodinfo.GetParameters();
                                                    template = new Template()
                                                    {
                                                        Asembly = DLL,
                                                        FileName = job.path,
                                                        ClassName = class_type.Name,
                                                        FullName = class_type.FullName,
                                                        //MethoName = ((System.Reflection.MethodBase)item).Name,
                                                        MethoName = Mymethodinfo.Name,
                                                        Params = paramInfo,
                                                        MethoReturnType = Mymethodinfo.ReturnType,
                                                        Type = (int)MethodType.Type.Executable,
                                                        ApplicationId = job.id_application,
                                                        ApplicationParamenters = job.parameters
                                                    };
                                                    template_list.Add(template);
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                        }    
                
                    }
    
                }
        

                if(template_list.Count>0)
                {

                    //var options = new ParallelOptions() { MaxDegreeOfParallelism = 50 };
                    //Parallel.ForEach(config.items, options, item =>
                    //{
                    //    Colocar aqui si se desean lanzar todos los procesos en simultáneo
                    //});

                    foreach(Template item in template_list)
                    {
                        if(item.MethoName == "Process")
                        {
                            switch (item.Type)
                            {
                                case (int)MethodType.Type.Executable:

                                    ArrayList param_list = new ArrayList();                                    
                                    
                                    object[] parameters=item.ApplicationParamenters;
                                    param_list.AddRange(parameters);
                                    param_list.Add("idapp=" + item.ApplicationId);
                                    parameters=param_list.ToArray();

                                    GenericResponse ret = Execute(item, item.MethoName, new object[] { parameters });
                                    if (!ret.operation)
                                    {
                                        error_messages = ret.description.Replace("\r", "").Replace("\n", "");
                                        log.Info(error_messages);
                                        RegisterError(ret.description);

                                    }
                                    else
                                        log.Info("Se ejecutó correctamente " + item.ClassName);

                                    break;

                            }
                        }
                    }
                    
                }

                
            }
            catch (Exception ex)
            {
                log.Error("Se produjo un error en el lanzador:" + ex.Message);
                // Intentará enviar a la api el error obtenido
                RegisterError("Job Lanzador: " + ex.Message);
            }

        }

        /// <summary>
        /// Ejecuta un metodo del plugins template
        /// </summary>
        /// <param name="template"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static GenericResponse Execute(Template template, string method, object[] parameters)
        {
            object execution_result = "";
            GenericResponse result = new GenericResponse();
            try
            {

                //Assembly testAssembly = Assembly.LoadFile(filePath);
                Assembly assembly = template.Asembly;

                //Type calcType = assembly.GetType("TemplateCalculator.Calculator");
                Type calcType = assembly.GetType(
                    template.FullName
                    );
                object calcInstance = Activator.CreateInstance(calcType);

                execution_result = calcType.InvokeMember(method,
                    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                    null, calcInstance, parameters);

                if (execution_result != null) { 
                    result.operation = (bool)execution_result.GetType().GetProperty("operation").GetValue(execution_result, null);
                    result.description = (string)execution_result.GetType().GetProperty("description").GetValue(execution_result, null);
                    result.response = (string)execution_result.GetType().GetProperty("response").GetValue(execution_result, null);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }


        /// <summary>
        /// Registra el error en la api
        /// </summary>
        /// <param name="message"></param>
        private static void RegisterError(string message)
        {
            string api = System.Configuration.ConfigurationManager.AppSettings["apiAddLog"];
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                //token                 
                //request.Headers.Add("Authorization", token);

                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                LogClass requestMessage = new LogClass();

                requestMessage.id_aplicacion = 0;
                requestMessage.fecha = DateTime.Now;
                requestMessage.id_tipo_log = (int)LogTypes.Types.INFO;
                requestMessage.descripcion_general = "Error";
                requestMessage.procedencia = "JOB Orquestador";
                requestMessage.descripcion_paquete ="";
                requestMessage.descripcion_error = message;
                requestMessage.descripcion_respuesta ="";
                requestMessage.codigo_agrupador ="";
                requestMessage.id_cliente = "0";
                
                var requestLog = Newtonsoft.Json.JsonConvert.SerializeObject(requestMessage);

                Byte[] byteArray = encoding.GetBytes(requestLog);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse response = request.GetResponse();
                if ((int)((System.Net.HttpWebResponse)response).StatusCode != 200)
                {
                    // Se produjo un error al llamar a la api.
                    log.Info("No fue posible invocar a la api para almacenar el log [" + requestLog + "]");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return;
            }
            return;
        }

        /// <summary>
        /// Valida el certificado de SSL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
