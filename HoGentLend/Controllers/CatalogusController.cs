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
        private IReservatieRepository reservatieRepository;

        public CatalogusController(IMateriaalRepository materiaalRepository, IGroepRepository groepRepository, IReservatieRepository reservatieRepository)
        {
            this.materiaalRepository = materiaalRepository;
            this.groepRepository = groepRepository;
            this.reservatieRepository = reservatieRepository;
        }

        // GET: Catalogus
        public ActionResult Index(Gebruiker gebruiker, String filter = "", int doelgroepId = 0,
            int leergebiedId = 0)
        {
            IEnumerable<MateriaalViewModel> materialen =
                materiaalRepository.FindByFilter(filter, doelgroepId, leergebiedId)
                .Select(m => new MateriaalViewModel(m));

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

        public ActionResult Detail(int id)
        {
            Materiaal m = materiaalRepository.FindBy(id);

            if (m == null)
                return HttpNotFound();

            long convertId = Convert.ToInt64(id);

            int[] chartList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (ReservatieLijn rl in m.ReservatieLijnen)
            {
                for (int i = 0; i < 14; i++)
                {
                    if (rl.OphaalMoment <= DateTime.Today.AddDays(i) && rl.IndienMoment >= DateTime.Today.AddDays(i))
                    {
                        chartList[i]++;
                    }
                }

            }
            ViewBag.chartList = chartList;
            return View(new MateriaalViewModel(m));
        }
    }
}