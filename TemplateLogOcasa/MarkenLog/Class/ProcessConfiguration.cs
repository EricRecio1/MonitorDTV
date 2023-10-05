using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericProcessLog.Class
{
    public class ProcessConfiguration
    {
        public int id { get; set; }
        public int id_aplicacion { get; set; }
        public string path { get; set; }
        public string extension { get; set; }
        public bool clasifica_subcarpeta { get; set; }
        public DateTime ultima_fecha_procesada { get; set; }
        public string ultima_hora_procesada { get; set; }
        public int ultima_linea_procesada { get; set; }
        public int ultima_posicion_procesada { get; set; }
        public string palabra_clave_busqueda { get; set; }
        public string parametros { get; set; }

        public ProcessConfiguration() { }
        public ProcessConfiguration(Records<ProcessParameters> param)
        {
            // Obtiene la configuracion del job 
            foreach (ProcessParameters item in param.items)
            {
                switch (item.clave)
                {
                    case "path_log":
                        path = item.valor;
                        break;
                    case "palabra_clave_busqueda":
                        palabra_clave_busqueda = item.valor;
                        break;
                    case "ultima_fecha_procesada":
                        ultima_fecha_procesada = DateTime.Parse(item.valor);
                        break;
                    case "ultima_linea_procesada":
                        ultima_linea_procesada = int.Parse(item.valor);
                        break;
                    case "ultima_posicion_procesada":
                        ultima_posicion_procesada = int.Parse(item.valor);
                        break;
                }
            }
        }
    }
}
