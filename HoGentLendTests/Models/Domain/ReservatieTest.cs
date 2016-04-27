using HoGentLend.Models.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoGentLendTests.Models.Domain
{
    [TestClass]
    public class ReservatieTest
    {

        private Reservatie reservatie;
        private Gebruiker lener;
        private DateTime ophaalMoment;
        private DateTime indienMoment;


        [TestInitialize]
        public void setup()
        {
            this.lener = new Student("Lener", "De Lener", "lener@email.com");

            this.ophaalMoment = new DateTime(2016, 4, 1);
            this.indienMoment = new DateTime(2016, 4, 8);

            this.reservatie = new Reservatie(lener, ophaalMoment, indienMoment);
            this.lener.Reservaties.Add(reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLenerVerplicht()
        {
            new Reservatie(null, ophaalMoment, indienMoment);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestOphaalMomentVerplicht()
        {
            new Reservatie(lener, null, indienMoment);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIndienMomentVerplicht()
        {
            new Reservatie(lener, ophaalMoment, null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIndienMomentNaOphaalMomentVerplicht()
        {
            new Reservatie(lener, indienMoment, ophaalMoment);
        }

        [TestMethod]
        public void TestOpgehaaldIsFalseBijNieuweReservatie()
        {
            Assert.IsFalse(reservatie.Opgehaald);
        }

        [TestMethod]
        public void TestReservatieMomentIsNowBijNieuweReservatie()
        {
            DateTime before = DateTime.Now;
            Reservatie r = new Reservatie(lener, ophaalMoment, indienMoment);
            DateTime after = DateTime.Now;
            Assert.IsTrue(before <= r.Reservatiemoment && r.Reservatiemoment >= after);
        }

        [TestMethod]
        public void TestAddReservatieLijnAddReservationLineAddReservationLine()
        {
            reservatie.AddReservationLine(new Materiaal()
            {
                Name = "Een Materiaal",
                Amount = 1,
            }, 1, ophaalMoment, indienMoment);
            Assert.AreEqual(1, reservatie.ReservatieLijnen.Count);
        }
    }
}
