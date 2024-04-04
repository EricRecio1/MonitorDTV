using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLogOcasa.Models
{
    public class DTV_Tabla_Error
    {
        public decimal IdDocumento { get; set; }
        public string FechaError { get; set; }
        public string NombreArchivo { get; set; }
        public string DescripIntegra { get; set; }
        public string IdPais { get; set; }
        public string Error { get; set; }
        public string Estado { get; set; }
        public string DescripResponsa { get; set; }
        public string FechaCierre { get; set; }
        public string Observacion { get; set; }
    }
}
