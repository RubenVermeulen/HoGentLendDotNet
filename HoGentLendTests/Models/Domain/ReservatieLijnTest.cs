using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoGentLend.Models.Domain;

namespace HoGentLendTests.Models.Domain
{

    [TestClass]
    class ReservatieLijnTest
    {

        private ReservatieLijn reservatieLijn;
        private Reservatie reservatie;
        private DateTime ophaalMoment;
        private DateTime indienMoment;
        private Materiaal materiaal;
        private long amount;


        [TestInitialize]
        public void setup()
        {
            this.ophaalMoment = new DateTime(2016, 4, 1);
            this.indienMoment = new DateTime(2016, 4, 8);
            var lener = new Student("Lener", "lener@email.com", "De Lener");
            this.reservatie = new Reservatie(lener, ophaalMoment, indienMoment);
            lener.Reservaties.Add(reservatie);
            this.amount = 1;
            this.materiaal = new Materiaal()
            {
                Name = "Een Materiaal",
                Amount = 1,
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAmountMagNietKleinerDanNulZijn()
        {
            new ReservatieLijn(-1, ophaalMoment, indienMoment, materiaal, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAmountMagNietNulZijn()
        {
            new ReservatieLijn(0, ophaalMoment, indienMoment, materiaal, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestOphaalMomentVerplicht()
        {
            new ReservatieLijn(amount, null, indienMoment, materiaal, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIndienMomentVerplicht()
        {
            new ReservatieLijn(amount, ophaalMoment, null, materiaal, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMateriaalVerplicht()
        {
            new ReservatieLijn(amount, ophaalMoment, indienMoment, null, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReservatieVerplicht()
        {
            new ReservatieLijn(amount, ophaalMoment, indienMoment, materiaal, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIndienMomentNaOphaalMomentVerplicht()
        {
            new ReservatieLijn(amount, indienMoment, ophaalMoment, materiaal, reservatie);
        }
    }
}
