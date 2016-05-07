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
        private Config config;

        public CatalogusController(IMateriaalRepository materiaalRepository, IGroepRepository groepRepository, IReservatieRepository reservatieRepository, IConfigWrapper configWrapper)
        {
            this.materiaalRepository = materiaalRepository;
            this.groepRepository = groepRepository;
            this.reservatieRepository = reservatieRepository;
            this.config = configWrapper.GetConfig();
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
            ViewBag.verlanglijst = gebruiker.WishList;

            if (gebruiker.CanSeeAllMaterials()) // If lector return all materialen
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

        public ActionResult Detail(Gebruiker gebruiker, int id)
        {
            Materiaal m = materiaalRepository.FindBy(id);
            List<ReservatieLijn> reservatieLijnen = m.ReservatieLijnen;

            if (m == null)
                return HttpNotFound();

            long convertId = Convert.ToInt64(id);

            int[] chartList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            
            Dictionary<string, List<ReservatieLijnViewModel>> reservaties = new Dictionary<string, List<ReservatieLijnViewModel>>();


            foreach (ReservatieLijn rl in reservatieLijnen)
            {
                for (int i = 0; i < 12; i++)
                {
                    int days = i * 7 * config.LendingPeriod;

                    if (rl.OphaalMoment <= DateTime.Today.AddDays(days) && rl.IndienMoment >= DateTime.Today.AddDays(days))
                    {
                        chartList[i] = chartList[i] + rl.Amount;
                    }
                }

                if (reservaties.ContainsKey(rl.OphaalMoment.ToString()))
                {
                    reservaties[rl.OphaalMoment.ToString()].Add(new ReservatieLijnViewModel(rl));
                }
                else
                {
                    reservaties.Add(rl.OphaalMoment.ToString(), new List<ReservatieLijnViewModel>() {new ReservatieLijnViewModel(rl)});
                }

            }

            ViewBag.chartList = chartList;
            ViewBag.lendingPeriod = config.LendingPeriod;
            ViewBag.reservaties = reservaties;
            ViewBag.InWishlist = gebruiker.WishList.Exists(m.Id);

            return View(new MateriaalViewModel(m));
        }
    }
}