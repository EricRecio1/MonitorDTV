﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OcasaExtranetEntities : DbContext
    {
        public OcasaExtranetEntities()
            : base("name=OcasaExtranetEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DTV_Moni_Error> DTV_Moni_Error { get; set; }
        public virtual DbSet<DTV_Moni_Estado> DTV_Moni_Estado { get; set; }
        public virtual DbSet<DTV_Moni_Integ> DTV_Moni_Integ { get; set; }
        public virtual DbSet<DTV_Moni_Respon> DTV_Moni_Respon { get; set; }
    }
}
