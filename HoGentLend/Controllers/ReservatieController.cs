using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;

namespace HoGentLend.Controllers
{
    [Authorize]
    public class ReservatieController : Controller
    {
        private IReservatieRepository reservatieRepository;
        private IMateriaalRepository materiaalRepository;

        public ReservatieController(IReservatieRepository reservatieRepository,
            IMateriaalRepository materiaalRepository)
        {
            this.reservatieRepository = reservatieRepository;
            this.materiaalRepository = materiaalRepository;
        }

        // GET: Reservatie
        public ActionResult Index(Gebruiker gebruiker)
        {
            IList<ReservatieViewModel> reservaties = new List<ReservatieViewModel>();
            List<ReservatieViewModel> reservatiesGesorteerd;

            foreach (Reservatie reservatie in gebruiker.Reservaties)
            {
                reservaties.Add(new ReservatieViewModel(reservatie));
            };

            reservatiesGesorteerd = reservaties.OrderBy(o => o.Ophaalmoment).ToList();

            return View(reservatiesGesorteerd);
        }

        // POST: Add
        [HttpPost]
        public ActionResult Add(Gebruiker gebruiker, List<ReservatiePartModel> reservatiepartmodels,
            DateTime? ophaalDatum)
        {
            double weeks = 1; // dit zal later nog uit de database gehaald worden
            if (!ophaalDatum.HasValue)
            {
                throw new ArgumentException("De ophaaldatum moet een waarde hebben.");
            }
            DateTime indienDatum = ophaalDatum.Value.AddDays(7 * weeks);

            List<Materiaal> materials = new List<Materiaal>();
            List<long> amounts = new List<long>();

            foreach (ReservatiePartModel rpm in reservatiepartmodels)
            {
                materials.Add(materiaalRepository.FindBy(rpm.
                    MateriaalId));
                amounts.Add(rpm.Amount);
            }
            try
            {
                gebruiker.AddReservation(materials, amounts, ophaalDatum.Value, indienDatum,
                    reservatieRepository.FindAll());
                reservatieRepository.SaveChanges();
                TempData["msg"] = "De reservatie  is toegevoegd aan uw verlanglijst.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: Remove
        [HttpPost]
        public ActionResult Remove(Gebruiker gebruiker, int reservatieId)
        {
            Reservatie res = reservatieRepository.FindBy(reservatieId);
            try
            {
                gebruiker.RemoveReservation(reservatieId);
                reservatieRepository.SaveChanges(); // dit zal ook de gebruiker veranderingen opslaan want het is overal dezeflde context
                TempData["msg"] = "De reservatie is succesvol verwijderd.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }
            return View("Index");
        }

        public ActionResult Detail(int id)
        {
            Reservatie r = reservatieRepository.FindBy(id);

            if (r == null)
                return HttpNotFound();

            List<ReservatieLijnViewModel> rlList = r.ReservatieLijnen
                .OrderBy(rl => rl.Materiaal.Name)
                .Select(rl => new ReservatieLijnViewModel(rl))
                .ToList();

            ReservatieViewModel rv = new ReservatieViewModel(r);
            rv.ReservatieLijnen = rlList;

            return View(rv);
        }
    }
}
