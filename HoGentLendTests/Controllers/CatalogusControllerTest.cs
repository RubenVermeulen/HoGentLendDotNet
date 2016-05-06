//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using HoGentLend.Controllers;
//using HoGentLend.Models.DAL;
//using HoGentLend.Models.Domain;
//using HoGentLend.ViewModels;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

//namespace HoGentLendTests.Controllers
//{
//    [TestClass]
//    public class CatalogusControllerTest
//    {

//        private CatalogusController controller;
//        private DummyDataContext ctx;

//        private Mock<IMateriaalRepository> mockMateriaalRepository;
//        private Mock<IGroepRepository> mockGroepRepository;
//        private Mock<IReservatieRepository> mockReservatieRepository;

//        private Gebruiker student;
//        private Gebruiker lector;
//        private const string FILTER_WERELDBOL = "Wereldbol";
//        private const int ID_KLEUTERONDERWIJS = 1;
//        private const int ID_WISKUNDE = 2;
//        private const int ID_AARDRIJKSKUNDE = 3;


//        [TestInitialize]
//        public void SetupContext()
//        {
//            ctx = new DummyDataContext();

//            mockMateriaalRepository = new Mock<IMateriaalRepository>();
//            mockGroepRepository = new Mock<IGroepRepository>();
//            mockReservatieRepository = new Mock<IReservatieRepository>();

//            student = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));
//            lector = ctx.GebruikerList.First(u => u.Email.Equals("lector@hogent.be"));

//            /* de logica hierachter wordt getest in de MateriaalRepositoryTest */
//            mockMateriaalRepository
//                .Setup(m => m.FindByFilter("", 0, 0))
//                .Returns(ctx.MateriaalList);

//            mockMateriaalRepository
//                .Setup(m => m.FindByFilter("Wereldbol", 0, 0))
//                .Returns(ctx.MateriaalList.Where(mat => mat.Name.Equals(FILTER_WERELDBOL)));

//            mockMateriaalRepository
//                .Setup(m => m.FindByFilter("", ID_KLEUTERONDERWIJS, ID_WISKUNDE))
//                .Returns(ctx.MateriaalList
//                    .Where(mat => mat.Doelgroepen.Any(d => d.Name.Equals("Kleuteronderwijs")))
//                    .Where(mat => mat.Leergebieden.Any(d => d.Name.Equals("Wiskunde")))
//                );

//            controller = new CatalogusController(mockMateriaalRepository.Object, mockGroepRepository.Object, mockReservatieRepository.Object);
//        }

//        [TestMethod]
//        public void IndexStudentReturnsAllLendableMaterials()
//        {
//            ViewResult result = controller.Index(student) as ViewResult;
//            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

//            /* zie DummyDataContext -> MateriaalList */
//            Assert.AreEqual(2, materials.Length);
//        }

//        [TestMethod]
//        public void IndexLectorReturnsAllMaterials()
//        {
//            ViewResult result = controller.Index(lector) as ViewResult;
//            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

//            Assert.AreEqual(3, materials.Length);
//        }

//        [TestMethod]
//        public void IndexFilterStringReturnsFilteredMaterials()
//        {
//            ViewResult result = controller.Index(lector, FILTER_WERELDBOL) as ViewResult;
//            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

//            Assert.AreEqual(1, materials.Length);
//            Assert.AreEqual(FILTER_WERELDBOL, materials[0].Name);
//        }

//        [TestMethod]
//        public void IndexFilterDoelgroepLeergebiedReturnsFilteredMaterials()
//        {
//            ViewResult result = controller.Index(lector, "", ID_KLEUTERONDERWIJS, ID_WISKUNDE) as ViewResult;
//            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

//            Assert.AreEqual(1, materials.Length);
//            Assert.AreEqual("Rekenmachine", materials[0].Name);
//        }
//    }
//}
