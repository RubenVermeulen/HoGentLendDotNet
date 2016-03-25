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
    [Authorize]
    public class CatalogusController : Controller
    {
        private IMateriaalRepository materiaalRepository;

        public CatalogusController(IMateriaalRepository materiaalRepository)
        {
            this.materiaalRepository = materiaalRepository;
        }

        // GET: Catalogus
        public ActionResult Index(Gebruiker gebruiker)
        {
            IEnumerable<MateriaalViewModel> materialen = materiaalRepository.FindAll()
                .Include(m => m.Firma)
                .Include(m => m.Doelgroepen)
                .Include(m => m.Leergebieden)
                .ToList()
                .OrderBy(m => m.Name)
                .Select(m => new MateriaalViewModel(m));

            
            if (gebruiker.ToonAlleMaterialen()) // If lector return all materialen
            {
                return View(materialen);
            }
            else // If student return only available, in stock materialen
            {
                return View(materialen.Where(m => m.IsLendable));
            }
            
        }

        // POST
        [HttpPost]
        public ActionResult Filter()
        {
            return View("Index");
        }

        public ActionResult Detail(int id)
        {
            Materiaal m = materiaalRepository.FindBy(id);

            if (m == null)
                return HttpNotFound();

            return View(new MateriaalViewModel(m));
        }
    }
}