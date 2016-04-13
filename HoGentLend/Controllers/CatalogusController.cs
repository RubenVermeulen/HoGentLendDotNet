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
        private IGroepRepository groepRepository;

        public CatalogusController(IMateriaalRepository materiaalRepository, IGroepRepository groepRepository)
        {
            this.materiaalRepository = materiaalRepository;
            this.groepRepository = groepRepository;
        }

        // GET: Catalogus
        public ActionResult Index(Gebruiker gebruiker, String filter="", int doelgroepId = 0,
            int leergebiedId = 0)
        {
            IEnumerable<MateriaalViewModel> materialen = null;
            Groep dg = null;
            Groep lg = null;

            if(String.IsNullOrEmpty(filter) && doelgroepId == 0 && leergebiedId == 0)
            {
                materialen = materiaalRepository.FindAll()
               .Include(m => m.Firma)
               .Include(m => m.Doelgroepen)
               .Include(m => m.Leergebieden)
               .ToList()
               .OrderBy(m => m.Name)
               .Select(m => new MateriaalViewModel(m));
            }
            else
            {
                materialen = materiaalRepository.FindByFilter(filter, null, null)
               .Include(m => m.Firma)
               .Include(m => m.Doelgroepen)
               .Include(m => m.Leergebieden)
               .ToList()
               .OrderBy(m => m.Name)
               .Select(m => new MateriaalViewModel(m));

                if(doelgroepId != 0)
                {
                    dg = groepRepository.FindBy(doelgroepId);
                    materialen = materialen.Where(m => m.Doelgroepen.Any(d => d.Equals(dg.Name)));
                }

                if (leergebiedId != 0)
                {
                    lg = groepRepository.FindBy(leergebiedId);
                    materialen = materialen.Where(m => m.Leergebieden.Any(d => d.Equals(lg.Name)));
                }

            }
           
            ViewBag.Doelgroepen = GroepenSelectList(groepRepository.FindAllDoelGroepen());
            ViewBag.Leergebieden = GroepenSelectList(groepRepository.FindAllLeerGebieden());
            ViewBag.doelgroepId = doelgroepId;
            ViewBag.leergebiedId = leergebiedId;
            ViewBag.filter = filter;

            if (gebruiker.DoShowAllMaterials()) // If lector return all materialen
            {
                return View(materialen);
            }
            else // If student return only available, in stock materialen
            {
                return View(materialen.Where(m => m.IsLendable));
            }
            //return View(materialen);

        }

        private SelectList GroepenSelectList(IQueryable<Groep> groepen)
        {
            return new SelectList(groepen.OrderBy(g => g.Name), "Id", "Name");
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