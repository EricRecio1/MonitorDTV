using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace JobLogOcasa.Class
{
    public class dbServices
    {
        protected SqlConnection _Con { get; set; }

        public string error_message { get; set; }

        public dbServices() { error_message = string.Empty; }

        public bool Connection()
        {
            _Con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["SqlConection"]);            
            return true;
        }

        public Records<Jobs> ListJobs(ProcessStoreProcedure storedprocedure)
        {
            Records<Jobs> records = new Records<Jobs>();
            List<Jobs> list = new List<Jobs>();
            try
            {
                Connection();
                SqlCommand com = new SqlCommand(storedprocedure.name, _Con);

                if (storedprocedure.parameters != null)
                {
                    foreach (ProcessStoreProcedureParameters param in storedprocedure.parameters)
                    {
                        com.Parameters.Add(param.name, param.type).Value = param.value;
                    }
                }
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);

                DataTable dt = new DataTable();
                _Con.Open();
                da.Fill(dt);
                _Con.Close();

                list = (from DataRow dr in dt.Rows
                        select new Jobs()
                        {
                            id = int.Parse(Convert.ToString(dr["id"] ?? "").ToString()),
                            name = Convert.ToString(dr["nombre"] ?? ""),
                            path = Convert.ToString(dr["path"] ?? ""),
                            parameters = dr["parametros"].ToString().Split(";".ToCharArray(),StringSplitOptions.RemoveEmptyEntries),
                            id_application = long.Parse(Convert.ToString(dr["id_aplicacion"] ?? "").ToString())

                        }).ToList();
                records.items = list;
                records.count = list.Count();

            }
            catch (Exception ex)
            {
                error_message = ex.Message;


            }
            return records;
        }

        public Records<Configuration> ListConfiguration(ProcessStoreProcedure storedprocedure)
        {
            Records<Configuration> records = new Records<Configuration>();
            List<Configuration> list = new List<Configuration>();
            try
            {
                Connection();
                SqlCommand com = new SqlCommand(storedprocedure.name, _Con);

                if (storedprocedure.parameters != null)
                {
                    foreach (ProcessStoreProcedureParameters param in storedprocedure.parameters)
                    {
                        com.Parameters.Add(param.name, param.type).Value = param.value;
                    }
                }
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);

                DataTable dt = new DataTable();
                _Con.Open();
                da.Fill(dt);
                _Con.Close();

                list = (from DataRow dr in dt.Rows
                        select new Configuration()
                        {
                            id = int.Parse(Convert.ToString(dr["id"] ?? "").ToString()),
                            parametros = Convert.ToString(dr["parametros"] ?? "")

                        }).ToList();
                records.items = list;
                records.count = list.Count();

            }
            catch (Exception ex)
            {
                error_message = ex.Message;


            }
            return records;
        }
    }
}
