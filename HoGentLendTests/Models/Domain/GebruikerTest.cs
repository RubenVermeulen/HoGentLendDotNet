//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using HoGentLend.Models.Domain;
//using System.Collections.Generic;

//namespace HoGentLendTests.Models.Domain
//{
//    [TestClass]
//    public class GebruikerTest
//    {
//        private Gebruiker student, lector;
//        private string firstName, lastName, email;
//        private VerlangLijst wishList;
//        private List<Reservatie> reservaties;

//        private Materiaal material, materialNotLendable;

//        private Reservatie rStudent, rLector, rStudentOpgehaald, rStudentSingleLine;
//        private ReservatieLijn rStudentLijn, rStudentSingleLineLijn;

//        [TestInitialize]
//        public void setup()
//        {
//            this.firstName = "Firstname";
//            this.lastName = "Lastname";
//            this.email = "Firstname.lastname@hogent.be";
//            this.wishList = new VerlangLijst();
//            this.reservaties = new List<Reservatie>();
//            this.student = new Student(firstName, lastName, email);
//            this.lector = new Lector(firstName, lastName, email);
//            this.material = new Materiaal()
//            {
//                Amount = 1,
//                Name = "Materiaal Naam",
//                IsLendable = true
//            };
//            this.materialNotLendable = new Materiaal()
//            {
//                Amount = 1,
//                Name = "Materiaal Naam",
//                IsLendable = false
//            };

//            Materiaal m1 = new Materiaal
//            {
//                Name = "Wereldbol",
//                Amount = 10,
//                IsLendable = true
//            };

//            Materiaal m2 = new Materiaal
//            {
//                Name = "Rekenmachine",
//                Description = "Reken er op los met deze grafische rekenmachine.",
//                ArticleCode = "abc456",
//                Price = 19.99,
//                Amount = 4,
//                AmountNotAvailable = 0,
//                IsLendable = true,
//                Location = "GSCHB 4.021"
//            };

//            Materiaal m3 = new Materiaal
//            {
//                Name = "Kleurpotloden",
//                Description = "Alle kleuren van de regenboog.",
//                ArticleCode = "abc789",
//                Price = 29.99,
//                Amount = 10,
//                AmountNotAvailable = 0,
//                IsLendable = true,
//                Location = "GSCHB 4.021"
//            };

//            Materiaal m4 = new Materiaal
//            {
//                Name = "Voetbal",
//                Description = "Voetballen voor in het lager onderwijs.",
//                ArticleCode = "abc147",
//                Price = 25.99,
//                Amount = 15,
//                AmountNotAvailable = 3,
//                IsLendable = false,
//                Location = "GSCHB 4.021"
//            };

//            Materiaal m5 = new Materiaal
//            {
//                Name = "Basketbal",
//                Description = "De NBA Allstar biedt de perfecte oplossing op het vlak van duurzaamheid en spelprestaties. Zowel geschikt voor indoor als outdoor. Uitstekende grip!",
//                ArticleCode = "abc258",
//                Price = 25.99,
//                Amount = 12,
//                AmountNotAvailable = 3,
//                IsLendable = true,
//                Location = "GSCHB 4.021",
//                PhotoBytes = null
//            };

//            DateTime _13April2016 = new DateTime(2016, 4, 13);
//            DateTime _20April2016 = new DateTime(2016, 4, 20);

//            DateTime _21April2016 = new DateTime(2016, 4, 21);
//            DateTime _28April2016 = new DateTime(2016, 4, 28);

//            DateTime _1April2016 = new DateTime(2016, 4, 1);
//            DateTime _8April2016 = new DateTime(2016, 4, 8);

//            rStudent = new Reservatie(student, _13April2016, _20April2016);
//            rStudent.ReservatieLijnen = new List<ReservatieLijn>();
//            rStudentLijn = new ReservatieLijn(2, _13April2016, _20April2016, m1, rStudent);
//            rStudent.ReservatieLijnen.Add(rStudentLijn);
//            rStudent.ReservatieLijnen.Add(new ReservatieLijn(3, _13April2016, _20April2016, m2, rStudent));
//            rStudent.ReservatieLijnen.Add(new ReservatieLijn(4, _13April2016, _20April2016, m3, rStudent));

//            rStudentOpgehaald = new Reservatie(student, _1April2016, _8April2016)
//            {
//                Opgehaald = true
//            };
//            rStudentOpgehaald.ReservatieLijnen = new List<ReservatieLijn>();
//            rStudentOpgehaald.ReservatieLijnen.Add(new ReservatieLijn(2, _1April2016, _8April2016, m1, rStudentOpgehaald));
//            rStudentOpgehaald.ReservatieLijnen.Add(new ReservatieLijn(3, _1April2016, _8April2016, m2, rStudentOpgehaald));
//            rStudentOpgehaald.ReservatieLijnen.Add(new ReservatieLijn(4, _1April2016, _8April2016, m3, rStudentOpgehaald));

//            rStudentSingleLine = new Reservatie(student, _13April2016, _20April2016);
//            rStudentSingleLine.ReservatieLijnen = new List<ReservatieLijn>();
//            rStudentSingleLineLijn = new ReservatieLijn(2, _13April2016, _20April2016, m1, rStudentSingleLine);
//            rStudentSingleLine.ReservatieLijnen.Add(rStudentSingleLineLijn);

//            rLector = new Reservatie(lector, _21April2016, _28April2016);
//            rLector.ReservatieLijnen = new List<ReservatieLijn>();
//            rLector.ReservatieLijnen.Add(new ReservatieLijn(2, _21April2016, _28April2016, m4, rLector));
//            rLector.ReservatieLijnen.Add(new ReservatieLijn(3, _21April2016, _28April2016, m5, rLector));
//            rLector.ReservatieLijnen.Add(new ReservatieLijn(4, _21April2016, _28April2016, m3, rLector));

//            student.Reservaties.Add(rStudent);
//            student.Reservaties.Add(rStudentOpgehaald);
//            student.Reservaties.Add(rStudentSingleLine);
//            lector.Reservaties.Add(rLector);
//        }

//        [TestMethod]
//        public void TestFirstNameVerplicht()
//        {
//            new Student(null, lastName, email);
//        }
//        [TestMethod]
//        public void TestLastNameVerplicht()
//        {
//            new Student(firstName, null, email);
//        }
//        [TestMethod]
//        public void TestEmailVerplicht()
//        {
//            new Student(firstName, lastName, null);
//        }
//        [TestMethod]
//        public void TestWishListVerplicht()
//        {
//            new Student(firstName, lastName, email, null, reservaties);
//        }
//        [TestMethod]
//        public void TestReservatiesVerplicht()
//        {
//            new Student(firstName, lastName, email, wishList, null);
//        }

//        [TestMethod]
//        public void TestStudentCanNotSeeAllMaterials()
//        {
//            Assert.IsFalse(student.CanSeeAllMaterials());
//        }
//        [TestMethod]
//        public void TestLectorCanSeeAllMaterials()
//        {
//            Assert.IsTrue(lector.CanSeeAllMaterials());
//        }
//        [TestMethod]
//        public void TestAddToWishListAddsMaterialToWishList()
//        {
//            student.AddToWishList(material);
//            Assert.AreEqual(1, student.WishList.Materials.Count);
//            lector.AddToWishList(material);
//            Assert.AreEqual(1, lector.WishList.Materials.Count);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void TestStudentAddToWishListMateriaalVerplicht()
//        {
//            student.AddToWishList(null);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void TestLectorAddToWishListMateriaalVerplicht()
//        {
//            lector.AddToWishList(null);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void TestStudentAddToWishListMateriaalMustBeLendable()
//        {
//            student.AddToWishList(materialNotLendable);
//        }
//        [TestMethod]
//        public void TestStudentAddToWishListMateriaalCanBeUnlendable()
//        {
//            lector.AddToWishList(materialNotLendable);
//            Assert.AreEqual(1, lector.WishList.Materials.Count);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void TestStudentRemoveFromWishListMateriaalIsVerplicht()
//        {
//            student.RemoveFromWishList(null);
//        }

//        [TestMethod]
//        public void TestStudentRemoveFromWishListMateriaalIsRemoved()
//        {
//            student.AddToWishList(material);
//            student.RemoveFromWishList(material);
//            Assert.AreEqual(0, lector.WishList.Materials.Count);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void TestRemoveReservationReservationIsVerplicht()
//        {
//            student.RemoveReservation(null);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void TestRemoveReservationReservationMustBeAdded()
//        {
//            student.RemoveReservation(rLector);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void TestRemoveReservationReservationMagNietOpgehaaldZijn()
//        {
//            student.RemoveReservation(rStudentOpgehaald);
//        }
//        [TestMethod]
//        public void TestRemoveReservationDidRemoveTheReservation()
//        {
//            var beforeCount = student.Reservaties.Count;
//            student.RemoveReservation(rStudent);
//            Assert.AreEqual(beforeCount - 1, student.Reservaties.Count);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void TestRemoveReservationLineReservationLineIsVerplicht()
//        {
//            student.RemoveReservationLijn(null);
//        }
//        [TestMethod]
//        public void TestRemoveReservationLineDeletesReservationLine()
//        {
//            var beforeCount = rStudent.ReservatieLijnen.Count;
//            student.RemoveReservationLijn(rStudentLijn);
//            Assert.AreEqual(beforeCount - 1, rStudent.ReservatieLijnen.Count);
//        }
//        [TestMethod]
//        public void TestRemoveReservationLineAlsoDeletedReservationIfLastLine()
//        {
//            var beforeCount = student.Reservaties.Count;
//            student.RemoveReservationLijn(rStudentSingleLineLijn);
//            Assert.AreEqual(beforeCount - 1, student.Reservaties.Count);

//        }

//        // TODO: test AddReservation
//    }
//}
