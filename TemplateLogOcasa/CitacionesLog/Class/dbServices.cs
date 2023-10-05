using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GenericProcessLog.Class
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

        /// <summary>
        /// Retorna un conjunto de registros de configuracion de acuerdo a los filtros
        /// </summary>
        /// <param name="storedprocedure"></param>
        /// <returns></returns>
        public Records<ProcessParameters> GetConfiguracion(ProcessStoreProcedure storedprocedure)
        {
            //ProcessConfiguration configuration = new ProcessConfiguration();
            Records<ProcessParameters> records = new Records<ProcessParameters>();
            List<ProcessParameters> list = new List<ProcessParameters>();
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
                        select new ProcessParameters()
                        {
                            id=int.Parse(dr["id"].ToString()),
                            id_aplicacion=int.Parse(dr["id_aplicacion"].ToString()),
                            clave = dr["clave"].ToString(),
                            valor = dr["valor"].ToString()
                            
                        }).ToList();
                records.items = list;
                records.count = list.Count();

            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                throw ex;
            }
            return records;
        }

        /// <summary>
        /// Actualiza un registro de configuracion
        /// </summary>
        /// <param name="storedprocedure"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateConfiguration(ProcessStoreProcedure storedprocedure)
        {
            Records<Results> records = new Records<Results>();
            List<Results> list = new List<Results>();
            bool ret = true;
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
                        select new Results()
                        {
                            message = dr["message"].ToString(),
                            operation = bool.Parse(dr["operation"].ToString())

                        }).ToList();
                records.items = list;
                records.count = list.Count();

            }
            catch (Exception)
            {

                throw;
            }

            return ret;
        }
    }
}
