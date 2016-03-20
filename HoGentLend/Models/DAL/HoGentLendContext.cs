using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class HoGentLendContext : IdentityDbContext<ApplicationUser>
    {
        public HoGentLendContext() : base("HoGentLend")
        { }

        // DbSets
        public DbSet<Materiaal> Materialen { get; set; }
        public DbSet<Groep> Groepen { get; set; }
        public DbSet<Firma> Firmas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static HoGentLendContext Create()
        {
            return DependencyResolver.Current.GetService<HoGentLendContext>();
        }

        static HoGentLendContext()
        {
            Console.WriteLine("setting the initializer");
            Database.SetInitializer<HoGentLendContext>(new HoGentLendDbInitializer());
        }

        public static void Init() { Create().Database.Initialize(true); }
    }

}