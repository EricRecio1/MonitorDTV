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
        public void SaveChanges(string table, AddChange param)
        {
            GenericResponse respuesta = new GenericResponse();

            try
            {
                string fechaCierre = "";

                if (param.id_estado == 3)
                {
                    fechaCierre = DateTime.Today.ToString("dd/MM/yyyy");
                }

                Connection();

                string query = "UPDATE DTV_Moni_Error SET ";
                query += "IdResponsable = " + param.id_responsable;
                query += ",IdEstado = " + param.id_estado;
                query += ",Observacion = '" + param.observacion + "', ";
                query += "FechaCierre = '" + fechaCierre + "' ";
                query += "WHERE IdDocumento = " + param.id_docu;

                _Con.Open();
                SqlCommand com = new SqlCommand(query, _Con);
                com.CommandType = CommandType.Text;
                com.ExecuteNonQuery();
                //SqlDataAdapter da = new SqlDataAdapter(com);
                //SqlDataAdapter da = new SqlDataAdapter(com);
                //DataTable dt = new DataTable();
                //_Con.Open();
                //da.Fill(dt);
                _Con.Close();
            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
                respuesta.response = "Error";
                respuesta.description = ex.Message;
                respuesta.operation = false;
            }
            //return respuesta;
        }

        public Records<DTV_Tabla_Error> ListErrors(StoredProcedure storedProcedure)
        {
            Records<DTV_Tabla_Error> list = new Records<DTV_Tabla_Error>();
            try
            {
                Connection();
                //SqlCommand com = new SqlCommand("SELECT * FROM " + logErrores, _Con);
                SqlCommand com = new SqlCommand(storedProcedure.name, _Con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                //DataTable dt = new DataTable();
                DataSet dt = new DataSet();
                com.Parameters.AddWithValue("@nroDoc", storedProcedure.parameters[0].value);
                com.Parameters.AddWithValue("@estado", storedProcedure.parameters[1].value);
                com.Parameters.AddWithValue("@fechaDesde", storedProcedure.parameters[2].value);
                com.Parameters.AddWithValue("@fechaHasta", storedProcedure.parameters[3].value);
                com.Parameters.AddWithValue("@idPais", storedProcedure.parameters[4].value);
                com.Parameters.AddWithValue("@idIntegracion", storedProcedure.parameters[5].value);
                _Con.Open();
                da.Fill(dt);
                _Con.Close();
    
                list.items = (from DataRow dr in dt.Tables[0].Rows
                                select new DTV_Tabla_Error()
                                {
                                    IdDocumento = decimal.TryParse((dr["IdDocumento"].ToString()), out decimal idDoc) ? idDoc : 0.0m,
                                    FechaError = (dr["FechaError"].ToString()),
                                    NombreArchivo = (dr["NombreArchivo"].ToString()),
                                    DescripIntegra = (dr["DescripIntegra"].ToString()),
                                    IdPais = (dr["IdPais"].ToString()),
                                    Error = (dr["Error"].ToString()),
                                    Estado = (dr["Estado"].ToString()),
                                    DescripResponsa  = (dr["DescripResponsa"].ToString()),
                                    FechaCierre = (dr["FechaCierre"].ToString()),
                                    Observacion = (dr["Observacion"].ToString())
                                }).ToList();
            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
            }
            return list;
        }

        public Records<DTV_Moni_Respon> ListResponsable(string responsable)
        {
            Records<DTV_Moni_Respon> list = new Records<DTV_Moni_Respon>();
            try
            {
                Connection();
                SqlCommand com = new SqlCommand("SELECT * FROM " + responsable, _Con);

                com.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(com);

                DataTable dt = new DataTable();
                _Con.Open();
                da.Fill(dt);
                _Con.Close();

                list.items = (from DataRow dr in dt.Rows
                              select new DTV_Moni_Respon()
                              {
                                  Clave = dr["Clave"].ToString(),
                                  DescripResponsa = (dr["DescripResponsa"].ToString())
                              }).ToList();
            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
            }
            return list;
        }
        //Lista de estados combobox
        public Records<DTV_Moni_Estado> ListStates(string estados)
        {
            Records<DTV_Moni_Estado> list = new Records<DTV_Moni_Estado>();
            try
            {
                Connection();
                SqlCommand com = new SqlCommand("SELECT * FROM " + estados, _Con);

                com.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(com);

                DataTable dt = new DataTable();
                _Con.Open();
                da.Fill(dt);
                _Con.Close();

                list.items = (from DataRow dr in dt.Rows
                        select new DTV_Moni_Estado()
                        {
                            Clave = dr["Clave"].ToString(),
                            Fecha_sys = Convert.ToDateTime(dr["Fecha_sys"].ToString()),
                            Fecha_Vcia = Convert.ToDateTime(dr["Fecha_Vcia"].ToString()),
                            Usuario = (dr["Usuario"].ToString()),
                            Desc_Corta = (dr["Desc_Corta"].ToString()),
                            Desc_Larga = (dr["Desc_Larga"].ToString()),
                            Estado = (dr["Estado"].ToString()),
                            IdEstado = decimal.TryParse((dr["IdEstado"].ToString()), out decimal idEst) ? idEst : 0.0m,
                            DescripEstado = (dr["DescripEstado"].ToString())
                        }).ToList();
            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
            }
            return list;
        }
        //Lista de integracion

        public Records<DTV_Moni_Integ> ListIntegracion(string integ)
        {
            Records<DTV_Moni_Integ> list = new Records<DTV_Moni_Integ>();
            try
            {
                Connection();
                SqlCommand com = new SqlCommand("SELECT * FROM " + integ, _Con);

                com.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(com);

                DataTable dt = new DataTable();
                _Con.Open();
                da.Fill(dt);
                _Con.Close();

                list.items = (from DataRow dr in dt.Rows
                              select new DTV_Moni_Integ()
                              {
                                  Clave = dr["Clave"].ToString(),
                                  Fecha_sys = Convert.ToDateTime(dr["Fecha_sys"].ToString()),
                                  Fecha_Vcia = Convert.ToDateTime(dr["Fecha_Vcia"].ToString()),
                                  Usuario = (dr["Usuario"].ToString()),
                                  Desc_Corta = (dr["Desc_Corta"].ToString()),
                                  Desc_Larga = (dr["Desc_Larga"].ToString()),
                                  Estado = (dr["Estado"].ToString()),
                                  IdIntegracion = decimal.TryParse((dr["IdIntegracion"].ToString()), out decimal idInt) ? idInt : 0.0m,
                                  DescripIntegra = (dr["DescripIntegra"].ToString())
                              }).ToList();

            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open) _Con.Close();
                error_message = ex.Message;
            }
            return list;
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