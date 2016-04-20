using System;
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

        public ReservatieController(IReservatieRepository reservatieRepository)
        {
            this.reservatieRepository = reservatieRepository;
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
        public ActionResult Add(Gebruiker gebruiker, List<Materiaal> materials, List<long> amounts, DateTime ophaalDatum, DateTime indienDatum)
        {
            double weeks = (indienDatum - ophaalDatum).TotalDays / 7;

            try
            {
                if (weeks > 1.5)
                {
                    throw new ArgumentException("De indiendatum mag niet meer dan een week na de ophaaldatum liggen.");
                }
                gebruiker.AddReservation(materials, amounts, ophaalDatum, indienDatum);
                reservatieRepository.SaveChanges();
                TempData["msg"] = "De reservatie  is toegevoegd aan uw verlanglijst.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }

            return Index(gebruiker);
        }
    }
}
