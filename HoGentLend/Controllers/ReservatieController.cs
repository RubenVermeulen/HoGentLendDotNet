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
        public ActionResult Add(Gebruiker gebruiker, List<ReservatiePartModel> reservatiepartmodels,
            DateTime ophaalDatum)
        {
            double weeks = 1; // dit zal later nog uit de database gehaald worden
            DateTime indienDatum = ophaalDatum.AddDays(7 * weeks);
            

            try
            {
                //gebruiker.AddReservation(materials, amounts, ophaalDatum, indienDatum, reservatieRepository.FindAll());
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
