using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoGentLend.Models.Domain;
using System.Collections.Generic;

namespace HoGentLendTests.Models.Domain
{
    [TestClass]
    public class GebruikerTest
    {
        private Gebruiker student;
        private Gebruiker lector;
        private string firstName;
        private string lastName;
        private string email;
        private VerlangLijst wishList;
        private List<Reservatie> reservaties;

        private Materiaal material;
        private Materiaal materialNotLendable;

        [TestInitialize]
        public void setup()
        {
            this.firstName = "Firstname";
            this.lastName = "Lastname";
            this.email = "Firstname.lastname@hogent.be";
            this.wishList = new VerlangLijst();
            this.reservaties = new List<Reservatie>();
            this.student = new Student(firstName, lastName, email);
            this.lector = new Lector(firstName, lastName, email);
            this.material = new Materiaal()
            {
                Amount = 1,
                Name = "Materiaal Naam",
                IsLendable = true
            };
            this.materialNotLendable = new Materiaal()
            {
                Amount = 1,
                Name = "Materiaal Naam",
                IsLendable = false
            };
        }

        [TestMethod]
        public void TestFirstNameVerplicht()
        {
            new Student(null, lastName, email);
        }
        [TestMethod]
        public void TestLastNameVerplicht()
        {
            new Student(firstName, null, email);
        }
        [TestMethod]
        public void TestEmailVerplicht()
        {
            new Student(firstName, lastName, null);
        }
        [TestMethod]
        public void TestWishListVerplicht()
        {
            new Student(firstName, lastName, email, null, reservaties);
        }
        [TestMethod]
        public void TestReservatiesVerplicht()
        {
            new Student(firstName, lastName, email, wishList, null);
        }

        [TestMethod]
        public void TestStudentCanNotSeeAllMaterials()
        {
            Assert.IsFalse(student.CanSeeAllMaterials());
        }
        [TestMethod]
        public void TestLectorCanSeeAllMaterials()
        {
            Assert.IsTrue(lector.CanSeeAllMaterials());
        }
        [TestMethod]
        public void TestAddToWishListAddsMaterialToWishList()
        {
            student.AddToWishList(material);
            Assert.AreEqual(1, student.WishList.Materials.Count);
            lector.AddToWishList(material);
            Assert.AreEqual(1, lector.WishList.Materials.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStudentAddToWishListMateriaalVerplicht()
        {
            student.AddToWishList(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLectorAddToWishListMateriaalVerplicht()
        {
            lector.AddToWishList(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStudentAddToWishListMateriaalMustBeLendable()
        {
            student.AddToWishList(materialNotLendable);
        }
        [TestMethod]
        public void TestStudentAddToWishListMateriaalCanBeUnlendable()
        {
            lector.AddToWishList(materialNotLendable);
            Assert.AreEqual(1, lector.WishList.Materials.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStudentRemoveFromWishListMateriaalIsVerplicht()
        {
            student.RemoveFromWishList(null);
        }

        [TestMethod]
        public void TestStudentRemoveFromWishListMateriaalIsRemoved()
        {
            student.AddToWishList(material);
            student.RemoveFromWishList(material);
            Assert.AreEqual(0, lector.WishList.Materials.Count);
        }
        // TODO: test AddReservation, RemoveReservation, RemoveReservationLijn
    }
}
