﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NavigationDrawerPopUpMenu2.db
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HomeRepairEntities : DbContext
    {
        public HomeRepairEntities()
            : base("name=HomeRepairEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Auth> Auth { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Foreman> Foreman { get; set; }
        public virtual DbSet<Kontrakt> Kontrakt { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Zayavki> Zayavki { get; set; }
    }
}