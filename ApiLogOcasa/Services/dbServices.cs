using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ApiLogOcasa.Models;

using System.Collections.Generic;
using System.Globalization;

namespace ApiLogOcasa
{
    public class dbServices
    {
        protected SqlConnection _Con { get; set; }

        public string error_message { get; set; }

        public dbServices() { error_message = string.Empty;  }

        public bool Connection()
        {
            _Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["StringConection"].ConnectionString );
            // (Helper.ConfigurationHelper.GetValue("Configuration", "StringConection"));

            return true;
        }
        public GenericResponse SaveMessageLog(StoredProcedure storedprocedure)
        {
            
            GenericResponse respuesta = new GenericResponse();

            try
            {
                Connection();
                SqlCommand com = new SqlCommand(storedprocedure.name, _Con);

                if (storedprocedure.parameters != null) { 
                    foreach(StoredProcedureParameters param in storedprocedure.parameters)
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

                respuesta = (from DataRow dr in dt.Rows
                             select new GenericResponse()
                             {
                                 response = Convert.ToString(dr["Respuesta"] ?? ""),
                                 description = Convert.ToString(dr["Descripcion"] ?? ""),
                                 operation = Convert.ToString(dr["Respuesta"] ?? "").ToUpper() == "OK" ? true : false
                             }).First();
                

            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
                respuesta.response = "Error";
                respuesta.description = ex.Message;
                respuesta.operation = false;
            }
            return respuesta;
        }

        public GenericResponse Save(StoredProcedure storedprocedure)
        {
            GenericResponse respuesta = new GenericResponse();

            try
            {
                Connection();
                SqlCommand com = new SqlCommand(storedprocedure.name, _Con);

                if (storedprocedure.parameters != null)
                {
                    foreach (StoredProcedureParameters param in storedprocedure.parameters)
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

                respuesta = (from DataRow dr in dt.Rows
                             select new GenericResponse()
                             {
                                 response = Convert.ToString(dr["Id"] ?? ""),
                                 description = Convert.ToString(dr["Descripcion"] ?? ""),
                                 operation = Convert.ToString(dr["Respuesta"] ?? "").ToUpper() == "OK" ? true : false
                             }).First();


            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
                respuesta.response = "Error";
                respuesta.description = ex.Message;
                respuesta.operation = false;
            }
            return respuesta;
        }

        public Records<SummaryLogsResponse> ListLogs(StoredProcedure storedprocedure)
        {
            Records<SummaryLogsResponse> records = new Records<SummaryLogsResponse>();
            List<SummaryLogsResponse> list = new List<SummaryLogsResponse>();

            try
            {
                Connection();
                SqlCommand com = new SqlCommand(storedprocedure.name, _Con);

                if (storedprocedure.parameters != null)
                {
                    foreach (StoredProcedureParameters param in storedprocedure.parameters)
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
                         select new SummaryLogsResponse()
                         {
                             id = int.Parse(dr["id"].ToString() ?? "0"),
                             nombre = dr["nombre"].ToString() ?? "",
                             activo = Boolean.Parse(dr["activo"].ToString()),
                             cantidad_log = int.Parse(dr["cantidad_log"].ToString() ?? ""),
                             criticidad = (dr["criticidad"]??"").ToString(),
                             descripcion = (dr["descripcion"] ?? "").ToString(),
                             servidor = (dr["servidor"] ?? "").ToString(),                             
                             nivel = (dr["nivel"]==null?0: int.Parse(dr["nivel"].ToString() ?? "")),
                             max_mensajes_error = (dr["max_mensajes_error"].ToString()==""?0:int.Parse(dr["max_mensajes_error"].ToString())),
                             id_tipo_log = int.Parse(dr["id_tipo_log"].ToString() ?? "0"),
                             tipo_log = (dr["tipo_log"] ?? "").ToString()

                         }).ToList();
                records.items = list;
                records.count = list.Count();
            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;                
            }
            return records;
        }

        public Records<Log> ListLogsDetail(StoredProcedure storedprocedure)
        {
            Records<Log> records = new Records<Log>();
            List<Log> list = new List<Log>();
            DateTime ddt=new DateTime();

            try
            {
                Connection();
                SqlCommand com = new SqlCommand(storedprocedure.name, _Con);

                if (storedprocedure.parameters != null)
                {
                    foreach (StoredProcedureParameters param in storedprocedure.parameters)
                    {
                        switch(param.type)
                        {
                            case SqlDbType.DateTime:
                            case SqlDbType.Date:
                                
                                DateTime.TryParseExact(param.value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out ddt);
                                com.Parameters.Add(param.name, param.type).Value = DateTime.Parse(param.value.ToString());
                                break;
                            default:
                                com.Parameters.Add(param.name, param.type).Value = param.value;
                                break;
                        }
                        
                    }
                }
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);

                DataTable dt = new DataTable();
                _Con.Open();
                da.Fill(dt);
                _Con.Close();

                list = (from DataRow dr in dt.Rows
                        select new Log()
                        {
                            id = int.Parse(dr["id"].ToString() ?? "0"),
                            id_aplicacion = int.Parse(dr["id_aplicacion"].ToString() ?? "0"),
                            fecha = DateTime.Parse(dr["fecha"].ToString()),
                            id_tipo_log = int.Parse(dr["id_tipo_log"].ToString() ?? "0"),
                            descripcion = (dr["descripcion"] ?? "").ToString(),
                            codigo_agrupador = (dr["codigo_agrupador"].ToString()),
                            procedencia = (dr["procedencia"].ToString() ?? ""),
                            descripcion_error = (dr["descripcion_error"] ?? "").ToString(),                            
                            descripcion_paquete = (dr["descripcion_paquete"] ?? "").ToString(),
                            descripcion_respuesta = (dr["descripcion_respuesta"] ?? "").ToString(),
                            id_estado_log = Convert.ToInt32((dr["id_estado_log"] ?? "").ToString()),
                            descripcion_estado = (dr["descripcion_estado"]).ToString()
                            

                        }).ToList();
                records.items = list;
                records.count = list.Count();
            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
            }
            return records;
        }
        public Records<LogType> ListLogTypes(StoredProcedure storedprocedure)
        {
            Records<LogType> records = new Records<LogType>();
            List<LogType> list = new List<LogType>();
            try
            {
                Connection();
                SqlCommand com = new SqlCommand(storedprocedure.name, _Con);

                if (storedprocedure.parameters != null) { 
                    foreach (StoredProcedureParameters param in storedprocedure.parameters)
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
                             select new LogType()
                             {
                                 id = int.Parse(Convert.ToString(dr["id"] ?? "").ToString()),
                                 description = Convert.ToString(dr["Descripcion"] ?? "")

                             }).ToList();
                records.items = list;
                records.count = list.Count();

            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;                
            }
            return records;
        }
        
        public Records<Application> ListApplications(StoredProcedure storedprocedure)
        {
            Records<Application> records = new Records<Application>();
            List<Application> list = new List<Application>();
            try
            {
                Connection();
                SqlCommand com = new SqlCommand(storedprocedure.name, _Con);

                if (storedprocedure.parameters != null)
                {
                    foreach (StoredProcedureParameters param in storedprocedure.parameters)
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
                        select new Application()
                        {
                            id = int.Parse(Convert.ToString(dr["id"] ?? "").ToString()),
                            nombre = Convert.ToString(dr["nombre"] ?? ""),
                            fecha_alta = Convert.ToString(dr["fecha_alta"] ?? ""),
                            fecha_creacion = Convert.ToString(dr["fecha_creacion"] ?? ""),
                            activo = Convert.ToBoolean(dr["activo"]??false),
                            descripcion = Convert.ToString(dr["descripcion"] ?? ""),
                            especificacion = Convert.ToString(dr["especificaciones"] != DBNull.Value ? dr["especificaciones"] : ""),
                            url_git = Convert.ToString(dr["url_git"] ?? ""),
                            url_documentos = Convert.ToString(dr["url_documentos"] != DBNull.Value ? dr["url_documentos"] : ""),
                            id_servidor = Convert.ToInt64(dr["id_servidor"] != DBNull.Value ? dr["id_servidor"] : 0),
                            id_cliente = Convert.ToInt32(dr["id_cliente"] != DBNull.Value ? dr["id_cliente"]: 0),
                            max_mensajes_error = Convert.ToInt32(dr["max_mensajes_error"] != DBNull.Value ? dr["max_mensajes_error"] : 0)

                        }).ToList();
                records.items = list;
                records.count = list.Count();

            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
            }
            return records;
        }
    }
}