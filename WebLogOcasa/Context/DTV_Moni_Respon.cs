//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebLogOcasa.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class DTV_Moni_Respon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DTV_Moni_Respon()
        {
            this.DTV_Moni_Error = new HashSet<DTV_Moni_Error>();
        }
    
        public string Clave { get; set; }
        public Nullable<System.DateTime> Fecha_sys { get; set; }
        public Nullable<System.DateTime> Fecha_Vcia { get; set; }
        public string Usuario { get; set; }
        public string Desc_Corta { get; set; }
        public string Desc_Larga { get; set; }
        public string Estado { get; set; }
        public decimal IdResponsable { get; set; }
        public string DescripResponsa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DTV_Moni_Error> DTV_Moni_Error { get; set; }
    }
}
