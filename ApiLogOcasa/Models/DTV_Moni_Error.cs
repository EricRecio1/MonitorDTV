using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiLogOcasa.Models
{
    [Table("DTV_Moni_Error")]
    public class DTV_Moni_Error
    {
        [Key]
        public int Clave { get; set; }
        public DateTime Fecha_sys { get; set; }
        public DateTime Fecha_Vcia { get; set; }
        public string Usuario { get; set; }
        public string Desc_Corta { get; set; }
        public string Desc_Larga { get; set; }
        public string Estado { get; set; }
        public decimal IdDocumento { get; set; }
        public string FechaError { get; set; }
        public string NombreArchivo { get; set; }
        public decimal IdIntegracion { get; set; }
        public string IdPais { get; set; }
        public decimal IdEstado { get; set; }
        public decimal? IdResponsable { get; set; }
        public string FechaCierre { get; set; }
        public string Error { get; set; }
        public string Observacion { get; set; }
    }
}