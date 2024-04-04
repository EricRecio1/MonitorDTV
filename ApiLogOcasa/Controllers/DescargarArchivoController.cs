using ApiLogOcasa.Enum;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace ApiLogOcasa.Controllers
{
    public class DescargarArchivoController : ApiController
    {
        string[] folders;

        [HttpPost]
        [Route("api/GuardarArchivo")]
        public HttpResponseMessage GuardarArchivo(string nombre_documento)
        {
            HttpRequestMessage request = this.ActionContext.Request;

            folders = new string[] {
                "Int008_error",
                "Int009_error",
                "Int010a_error",
                "Int010b_error",
                "Int012_error",
                "Int015a_error",
                "Int015b_error",
                "Int015c_error",
                "Int038_error",
            };

            var nombreCorto = nombre_documento.Substring(0, 6);

            PropertyInfo[] lst = typeof(FoldersSFTP).GetProperties();
            foreach(var item in lst)
            {
                if (item.Name.Contains(nombreCorto))
                {
                    Download(item.Name, nombre_documento);
                }
            }

            //foreach (var item in folders)
            //{
            //    if (item.Contains(nombreCorto))
            //    {
            //        Download(item, nombre_documento);
            //    }
            //}
            return request.CreateResponse(System.Net.HttpStatusCode.OK);
        }
        
        public void Download(string path, string fileName) 
        { 
            string host = @"ftp.dtvpan.com";
            string username = "ocasa_prod";
            string password = "A9Y1&U07M9yA9";
            int port = 22;
            
            using (SftpClient sftp = new SftpClient(host, port, username, password))
            {
                sftp.Connect();

                //IEnumerable<SftpFile> res = (idPais == "AR") ? client.ListDirectory(_configuration[path]) : client.ListDirectory(_configuration[path + "_" + idPais]);
              
                //var fileFound = res.Where(x => x.Name == fileName).FirstOrDefault();

                //foreach (var file in files)
                //{
                //    if (!file.Name.StartsWith("."))
                //    {
                //        string remoteFileName = file.Name;
                //        if (file.LastWriteTime.Date == DateTime.Today)

                //            Console.WriteLine(file.FullName);

                //        File.OpenWrite(localFileName);

                //        string sDir = @"localpath";

                //        Stream file1 = File.OpenRead(remoteDirectory + file.Name);
                //        sftp.DownloadFile(remoteDirectory, file1);
                //    }
                //}
            }       
        }
    }
}