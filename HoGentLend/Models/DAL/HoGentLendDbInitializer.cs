using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class HoGentLendDbInitializer : System.Data.Entity.DropCreateDatabaseAlways<HoGentLendContext>
    {
        protected override void Seed(HoGentLendContext context)
        {
            try
            {
                // Hier zetten we database context initializatie
                Groep d1 = new Groep {Name = "Kleuteronderwijs", IsLeerGebied = false};

                List<Groep> doelgroepen = new List<Groep>();
                doelgroepen.Add(d1);

                Groep l1 = new Groep { Name = "Aardrijkskunde", IsLeerGebied = true};
                Groep l2 = new Groep { Name = "Geografie", IsLeerGebied = true};

                List<Groep> leergebieden = new List<Groep>();
                leergebieden.Add(l1);
                leergebieden.Add(l2);

                Materiaal m1 = new Materiaal
                {
                    Name = "Wereldbol",
                    Description = "Alle landen van de wereld in één handomdraai.",
                    ArticleCode = "abc123",
                    Price = 12.85,
                    Amount = 10,
                    AmountNotAvailable = 5,
                    IsLendable = true,
                    Location = "GSCHB4.021",
                    Firma = new Firma { Name = "Goaty Enterprise", Email = "info@goatyenterprise.be"},
                    DoelGroepen = doelgroepen,
                    LeerGebieden = leergebieden

                };

                context.Materialen.Add(m1);

                context.SaveChanges();
                //base.Seed(context);
            }
            catch (DbEntityValidationException e)
            {
                string s = "Fout creatie database ";
                foreach (var eve in e.EntityValidationErrors)
                {
                    s += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.GetValidationResult());
                    foreach (var ve in eve.ValidationErrors)
                    {
                        s += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(s);
            }
        }
    }
}