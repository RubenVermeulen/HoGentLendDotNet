using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoGentLend.Models.Domain;

namespace HoGentLendTests.Controllers
{
    class DummyDataContext
    {
        public IQueryable<Gebruiker> GebruikerList { get; set; }
        public IQueryable<Materiaal> MateriaalList { get; set; }

        public DummyDataContext()
        {
            /* Materials */
            Materiaal m1 = new Materiaal
            {
                Name = "Wereldbol",
                Amount = 10,
            };

            Materiaal m2 = new Materiaal
            {
                Name = "Rekenmachine",
                Amount = 2,
            };

            IList<Materiaal> MList = (new Materiaal[] {m1, m2}).ToList();
            MateriaalList = MList.AsQueryable();

            /* Whislists */
            VerlangLijst l1 = new VerlangLijst
            {
                Materials = MList
            };

            VerlangLijst l2 = new VerlangLijst
            {
                Materials = new List<Materiaal>()
            };

            /* Users */
            Gebruiker g1 = new Gebruiker()
            {
                Email = "ruben@hogent.be",
                FirstName = "Ruben",
                LastName = "Vermeulen",
                IsLector = false,
                WishList = l1
            };

            Gebruiker g2 = new Gebruiker() /* Empty wishlist */
            {
                Email = "sven@hogent.be",
                FirstName = "Sven",
                LastName = "Dedeene",
                IsLector = false,
                WishList = l2
            };

            GebruikerList = (new Gebruiker[] { g1, g2 }).ToList().AsQueryable();
        } 
    }
}
