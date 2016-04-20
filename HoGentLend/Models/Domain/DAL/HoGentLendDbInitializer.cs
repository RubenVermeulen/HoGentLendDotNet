using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

                List<Groep> leergebiedenSet1 = new List<Groep>();
                leergebiedenSet1.Add(new Groep { Name = "Aardrijkskunde", IsLeerGebied = true });
                leergebiedenSet1.Add(new Groep { Name = "Geografie", IsLeerGebied = true });

                List<Groep> leergebiedenSet2 = new List<Groep>();
                leergebiedenSet2.Add(new Groep { Name = "Wiskunde", IsLeerGebied = true });

                Firma f1 = new Firma {Name = "Goaty Enterprise", Email = "info@goatyenterprise.be"};

                Materiaal m1 = new Materiaal
                {
                    Name = "Wereldbol",
                    //Description = "Alle landen van de wereld in één handomdraai.",
                    //ArticleCode = "abc123",
                    //Price = 12.85,
                    Amount = 10,
                    //AmountNotAvailable = 10,
                    IsLendable = true,
                    //Location = "GSCHB4.021",
                    //Firma = f1,
                    Doelgroepen = doelgroepen,
                    Leergebieden = leergebiedenSet1,
                    //PhotoBytes = new WebClient().DownloadData("https://www.dezwerver.nl/media/cache/e3/88/e38825e2d8175a72d9d346193f983983.jpg")
                };

                Materiaal m2 = new Materiaal
                {
                    Name = "Rekenmachine",
                    Description = "Reken er op los met deze grafische rekenmachine.",
                    ArticleCode = "abc456",
                    Price = 19.99,
                    Amount = 4,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "GSCHB 4.021",
                    Firma = f1,
                    Doelgroepen = doelgroepen,
                    Leergebieden = leergebiedenSet2,
                    PhotoBytes = new WebClient().DownloadData("http://www.epacking.eu/Docs/Images/Groups/1/Product_2012329991k222k87k491_rekenmachine.jpg")
                };

                Materiaal m3 = new Materiaal
                {
                    Name = "Kleurpotloden",
                    Description = "Alle kleuren van de regenboog.",
                    ArticleCode = "abc789",
                    Price = 29.99,
                    Amount = 10,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "GSCHB 4.021",
                    Firma = f1,
                    Doelgroepen = doelgroepen,
                    Leergebieden = leergebiedenSet2,
                };

                Materiaal m4 = new Materiaal
                {
                    Name = "Voetbal",
                    Description = "Voetballen voor in het larger onderwijs.",
                    ArticleCode = "abc147",
                    Price = 25.99,
                    Amount = 15,
                    AmountNotAvailable = 3,
                    IsLendable = false,
                    Location = "GSCHB 4.021",
                    Firma = f1,
                    Doelgroepen = doelgroepen,
                    Leergebieden = leergebiedenSet2,
                    PhotoBytes = new WebClient().DownloadData("http://hobby.blogo.nl/files/2007/11/hoe-voetbal-surprise-maken-680x703.jpg"),
                };

                Materiaal m5 = new Materiaal
                {
                    Name = "Basketbal",
                    Description = "De NBA Allstar biedt de perfecte oplossing op het vlak van duurzaamheid en spelprestaties. Zowel geschikt voor indoor als outdoor. Uitstekende grip!",
                    ArticleCode = "abc258",
                    Price = 25.99,
                    Amount = 12,
                    AmountNotAvailable = 3,
                    IsLendable = true,
                    Location = "GSCHB 4.021",
                    Firma = f1,
                    Doelgroepen = doelgroepen,
                    Leergebieden = leergebiedenSet2,
                    PhotoBytes = new WebClient().DownloadData("https://sportbay.nl/static/files/4/88/488/Basketbal_Nike_Dominate_7.jpg"),
                };

                context.Materialen.Add(m1);
                context.Materialen.Add(m2);
                context.Materialen.Add(m3);
                context.Materialen.Add(m4);
                context.Materialen.Add(m5);

                Gebruiker g1 = new Gebruiker()
                {
                    FirstName = "Offline",
                    LastName = "Student",
                    Email = "offline.student@hogent.be",
                    IsLector = false,
                    WishList = new VerlangLijst(),
                    Reservaties = new List<Reservatie>()
                };
                Gebruiker g2 = new Gebruiker()
                {
                    FirstName = "Offline",
                    LastName = "Lector",
                    Email = "offline.lector@hogent.be",
                    IsLector = true,
                    WishList = new VerlangLijst(),
                    Reservaties = new List<Reservatie>()
                };

                context.Gebruikers.Add(g1);
                context.Gebruikers.Add(g2);

                DateTime _13April2016 = new DateTime(2016, 4, 13);
                DateTime _20April2016 = new DateTime(2016, 4, 20);

                DateTime _21April2016 = new DateTime(2016, 4, 21);
                DateTime _28April2016 = new DateTime(2016, 4, 28);

                Reservatie r1 = new Reservatie(g1, _13April2016, _20April2016);
                r1.ReservatieLijnen = new List<ReservatieLijn>();
                r1.ReservatieLijnen.Add(new ReservatieLijn(2, _13April2016, _20April2016, m1));
                r1.ReservatieLijnen.Add(new ReservatieLijn(3, _13April2016, _20April2016, m2));
                r1.ReservatieLijnen.Add(new ReservatieLijn(4, _13April2016, _20April2016, m3));

                Reservatie r2 = new Reservatie(g1, _21April2016, _28April2016);
                r2.ReservatieLijnen.Add(new ReservatieLijn(2, _21April2016, _28April2016, m4));
                r2.ReservatieLijnen.Add(new ReservatieLijn(3, _21April2016, _28April2016, m5));
                r2.ReservatieLijnen.Add(new ReservatieLijn(4, _21April2016, _28April2016, m3));

                context.Reservaties.Add(r1);
                context.Reservaties.Add(r2);

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