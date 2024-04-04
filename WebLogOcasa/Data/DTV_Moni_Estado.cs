using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLogOcasa.Data
{
    public class DTV_Moni_Estado
    {
        public string Clave { get; set; }
        public DateTime Fecha_sys { get; set; }
        public DateTime Fecha_Vcia { get; set; }
        public string Usuario { get; set; }
        public string Desc_Corta { get; set; }
        public string Desc_Larga { get; set; }
        public string Estado { get; set; }
        public double IdEstado { get; set; }
        public string DescripEstado { get; set; }
    }
}