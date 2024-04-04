using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiLogOcasa.Models
{
    [Table("DTV_Moni_Respon")]
    public class DTV_Moni_Respon
    {
        public string Clave { get; set; }
        public DateTime Fecha_sys { get; set; }
        public DateTime Fecha_Vcia { get; set; }
        public string Usuario { get; set; }
        public string Desc_Corta { get; set; }
        public string Desc_Larga { get; set; }
        public string Estado { get; set; }
        [Key]
        public decimal IdResponsable { get; set; }
        public string DescripResponsa { get; set; }
    }
}