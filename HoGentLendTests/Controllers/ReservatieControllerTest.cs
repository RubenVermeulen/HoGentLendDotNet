using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HoGentLend.Controllers;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HoGentLendTests.Controllers
{
    [TestClass]
    public class ReservatieControllerTest
    {


        private DummyDataContext ctx;
        private ReservatieController reservatieController;


        private Mock<IMateriaalRepository> mockMateriaalRepository;
        private Mock<IGebruikerRepository> mockGebruikerRepository;
        private Mock<IReservatieRepository> mockReservatieRepository;
        private Mock<IConfigWrapper> mockConfigWrapper;

        private ReservatiePartModel rpm;
        private Materiaal m1;
        private List<Groep> dgList;
        private Groep dg1;
        private List<Groep> lgListAardrijkskunde;
        private Groep lg2;

        [TestInitialize]
        public void SetupContext()
        {
            ctx = new DummyDataContext();

            rpm = new ReservatiePartModel();
            rpm.Amount = 5;
            rpm.MateriaalId = 342;

            dg1 = new Groep
            {
                IsLeerGebied = false,
                Name = "Kleuteronderwijs"
            };

            dgList = (new Groep[] { dg1 }).ToList();

            lg2 = new Groep
            {
                IsLeerGebied = true,
                Name = "Aardrijkskunde"
            };

            lgListAardrijkskunde = (new Groep[] { lg2 }).ToList();


            m1 = new Materiaal
            {
                Name = "Wereldbol",
                Amount = 10,
                Doelgroepen = dgList,
                Leergebieden = lgListAardrijkskunde,
                IsLendable = true,
            };

            mockMateriaalRepository = new Mock<IMateriaalRepository>();
            mockConfigWrapper = new Mock<IConfigWrapper>();

            //mockMateriaalRepository.Setup(m => m.FindAll()).Returns(ctx.MateriaalList);
            mockConfigWrapper.Setup(c => c.GetConfig()).Returns(new Config()
            {
                Indiendag = "vrijdag",
                Ophaaldag = "maandag",
                Indientijd = new DateTime(1, 1, 1, 17, 30, 0),
                Ophaaltijd = new DateTime(1, 1, 1, 10, 30, 0),
                LendingPeriod = 1
            });


            mockReservatieRepository = new Mock<IReservatieRepository>();
            mockMateriaalRepository = new Mock<IMateriaalRepository>();

            mockMateriaalRepository.Setup(m => m.FindBy(342)).Returns(m1);
            mockReservatieRepository.Setup(m => m.FindBy(342)).Returns(ctx.reservatie);

            reservatieController = new ReservatieController(mockReservatieRepository.Object,
                mockMateriaalRepository.Object, mockConfigWrapper.Object);
        }

        [TestMethod]
        public void IndexReturnsZeroReservationsForRuben()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            // Act
            ViewResult result = reservatieController.Index(g) as ViewResult;
            ReservatieViewModel[] reservations = ((IEnumerable<ReservatieViewModel>)result.Model).ToArray();

            // Assert
            Assert.AreEqual(1, reservations.Length);
        }

        [TestMethod]
        public void AddSetsReservationsForRubenOnTwo()
        {


            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));
            ViewResult result = reservatieController.Index(g) as ViewResult;
            List<ReservatiePartModel> rpms = new List<ReservatiePartModel>();
            //mockReservatieRepository.Setup(m => m.SaveChanges());
            rpms.Add(rpm);

            // Act

            reservatieController.Add(g, rpms, DateTime.Now.AddDays(12));

            //Assert
            Assert.AreEqual(2, g.Reservaties.Count);
            mockReservatieRepository.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual("De reservatie  is toegevoegd aan uw verlanglijst.", result.TempData.Peek("msg"));

        }

        [TestMethod]
        public void RemoveReservationsFailsWrongId()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = reservatieController.Index(g) as ViewResult;

            reservatieController.Remove(g, 341);

            Assert.AreEqual(1, g.Reservaties.Count);
            mockReservatieRepository.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual("De reservatie is niet beschikbaar of mogelijk al verwijderd.", result.TempData.Peek("err"));
        }

        [TestMethod]
        public void RemoveReservationsRemovesRubensOnlyReservation()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = reservatieController.Index(g) as ViewResult;

            List<ReservatiePartModel> rpms = new List<ReservatiePartModel>();
            rpms.Add(rpm);

            reservatieController.Remove(g, 342);

            Assert.AreEqual(0, g.Reservaties.Count);
            mockReservatieRepository.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual("De reservatie is succesvol verwijderd.", result.TempData.Peek("msg"));
        }

        [TestMethod]
        public void DetailReservationsDoesntFail()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = reservatieController.Detail(g, 342) as ViewResult;
        }
    }
}
