﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace diplom_2
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class meotis_newtricEntities : DbContext
    {
        public meotis_newtricEntities()
            : base("name=meotis_newtricEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<field_revision_field_catalog> field_revision_field_catalog { get; set; }
        public virtual DbSet<node> node { get; set; }
        public virtual DbSet<taxonomy_term_data> taxonomy_term_data { get; set; }
        public virtual DbSet<file_managed> file_managed { get; set; }
        public virtual DbSet<field_data_field_image> field_data_field_image { get; set; }
        public virtual DbSet<field_revision_body> field_revision_body { get; set; }
        public virtual DbSet<field_revision_field_polotno> field_revision_field_polotno { get; set; }
        public virtual DbSet<field_revision_field_collection> field_revision_field_collection { get; set; }
    }
}