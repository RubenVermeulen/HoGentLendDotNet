using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;

namespace HoGentLend.Controllers
{
    public class CatalogusController : Controller
    {
        private IMateriaalRepository _materiaalRepository;

        public IMateriaalRepository MateriaalRepository
        {
            get
            {
                return _materiaalRepository ?? HttpContext.GetOwinContext().Get<IMateriaalRepository>();
            }
            private set
            {
                _materiaalRepository = value;
            }
        }

        public CatalogusController(IMateriaalRepository materiaalRepository)
        {
            MateriaalRepository = materiaalRepository;
        }

        // GET: Catalogus
        public ActionResult Index()
        {
            IEnumerable<MateriaalViewModel> materialen = MateriaalRepository.FindAll()
                .Include(m => m.Firma)
                .Include(m => m.DoelGroepen)
                .Include(m => m.LeerGebieden)
                .ToList()
                .OrderBy(m => m.Name)
                .Select(m => new MateriaalViewModel(m));

            // If lector return all materialen

            // If student return only available, in stock materialen
            return View(materialen);
        }

        // POST
        [HttpPost]
        public ActionResult Filter()
        {
            return View("Index");
        }
    }
}