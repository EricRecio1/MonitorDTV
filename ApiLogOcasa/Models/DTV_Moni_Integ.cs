using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiLogOcasa.Models
{
    [Table("DTV_Moni_Integ")]
    public class DTV_Moni_Integ
    {
        public string Clave { get; set; }
        public DateTime Fecha_sys { get; set; }
        public DateTime Fecha_Vcia { get; set; }
        public string Usuario { get; set; }
        public string Desc_Corta { get; set; }
        public string Desc_Larga { get; set; }
        public string Estado { get; set; }
        [Key]
        public decimal IdIntegracion { get; set; }
        public string DescripIntegra { get; set; }
    }
}