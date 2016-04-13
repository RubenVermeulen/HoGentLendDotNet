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
    public class VerlanglijstControllerTest
    {
        private DummyDataContext ctx;
        private VerlanglijstController controller;
        private Mock<IMateriaalRepository> mockMateriaalRepository;
        private Mock<IGebruikerRepository> mockGebruikerRepository;

        [TestInitialize]
        public void SetupContext()
        {
            ctx = new DummyDataContext();

            mockMateriaalRepository = new Mock<IMateriaalRepository>();

            //mockMateriaalRepository.Setup(m => m.FindAll()).Returns(ctx.MateriaalList);

            controller = new VerlanglijstController(mockMateriaalRepository.Object);
        }

        [TestMethod]
        public void IndexReturnsAllMaterialsWishList()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            // Act
            ViewResult result = controller.Index(g) as ViewResult;

            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

            // Assert
            Assert.AreEqual(2, materials.Length);
            Assert.AreEqual("Rekenmachine", materials[0].Name);
            Assert.AreEqual(2, materials[0].Amount);
        }
    }
}
