using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace HoGentLend.Models.DAL
{
    public class HoGentLendDbContext : IdentityDbContext<ApplicationUser>
    {
        public HoGentLendDbContext() : base("HoGentLend")
        { }

        // hier komen de DbSet's

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static HoGentLendDbContext Create()
        {
            return DependencyResolver.Current.GetService<HoGentLendDbContext>();
        }

        static HoGentLendDbContext()
        {
            Console.WriteLine("setting the initializer");
            Database.SetInitializer<HoGentLendDbContext>(new HoGentLendDbInitializer());
        }
        public static void init() { Create().Database.Initialize(true); }
    }

}