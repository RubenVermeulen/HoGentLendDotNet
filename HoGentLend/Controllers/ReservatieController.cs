﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoGentLend.Models.DAL;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;

namespace HoGentLend.Controllers
{
    //dont hate the memer
    //hate the meme
    [Authorize]
    public class ReservatieController : Controller
    {
        private IReservatieRepository reservatieRepository;
        private IMateriaalRepository materiaalRepository;
        private IConfigWrapper configWrapper;

        public ReservatieController(IReservatieRepository reservatieRepository,
            IMateriaalRepository materiaalRepository, IConfigWrapper configWrapper)
        {
            this.reservatieRepository = reservatieRepository;
            this.materiaalRepository = materiaalRepository;
            this.configWrapper = configWrapper;
        }

        // GET: Reservatie
        public ActionResult Index(Gebruiker gebruiker)
        {
            IList<ReservatieViewModel> reservaties = new List<ReservatieViewModel>();
            List<ReservatieViewModel> reservatiesGesorteerd;

            foreach (Reservatie reservatie in gebruiker.Reservaties)
            {
                ReservatieViewModel rvm = new ReservatieViewModel(reservatie);
                reservaties.Add(rvm);

                //FindConflicts(reservatie, rvm, gebruiker);

                // @Svonx: wordt later nog in een aparte methode gestoken, staat hier nu voor izi bugfixing
                ConstructReservatieViewModels(reservatie, rvm, gebruiker);

            };

            Config c = configWrapper.GetConfig();

            ViewBag.ophaalTijd = c.Ophaaltijd.ToString("HH:mm");
            ViewBag.indienTijd = c.Indientijd.ToString("HH:mm");

            reservatiesGesorteerd = reservaties.OrderBy(o => o.Ophaalmoment).ToList();

            return View(reservatiesGesorteerd);
        }

        // POST: Add
        [HttpPost]
        public ActionResult Add(Gebruiker gebruiker, List<ReservatiePartModel> reservatiepartmodels,
            DateTime? ophaalDatum)
        {
            Config c = configWrapper.GetConfig();
            int aantalDagen;
            int indienDag;
            int ophaalDag;
            int verschilDagen;


            if (c.Indiendag.Equals("dinsdag"))
            {
                indienDag = 1;
            }
            else if (c.Indiendag.Equals("woensdag"))
            {
                indienDag = 2;
            }
            else if (c.Indiendag.Equals("donderdag"))
            {
                indienDag = 3;
            }
            else if (c.Indiendag.Equals("vrijdag"))
            {
                indienDag = 4;
            }
            else {
                indienDag = 0;
            }

            if (c.Ophaaldag.Equals("maandag"))
            {
                ophaalDag = 0;
            }
            else if (c.Ophaaldag.Equals("dinsdag"))
            {
                ophaalDag = 1;
            }
            else if (c.Ophaaldag.Equals("woensdag"))
            {
                ophaalDag = 2;
            }
            else if (c.Ophaaldag.Equals("donderdag"))
            {
                ophaalDag = 3;
            }
            else
            {
                ophaalDag = 4;
            }

            verschilDagen = ophaalDag - indienDag;

            if (verschilDagen == 0) {
                aantalDagen = c.LendingPeriod * 7;
            }
            else if (c.LendingPeriod == 1)
            {
                aantalDagen = verschilDagen;
            }
            else {
                aantalDagen = (c.LendingPeriod - 1) * 7 + verschilDagen;
            }

            var materialenTeReserveren = new Dictionary<Materiaal, int>();
            var x = 0;
            foreach (ReservatiePartModel rpm in reservatiepartmodels)
            {
                if (rpm.Amount > 0)
                {
                    materialenTeReserveren.Add(materiaalRepository.FindBy(rpm.
                    MateriaalId), rpm.Amount);
                    x++;
                }

            }
            try
            {
                if (!ophaalDatum.HasValue)
                {
                    throw new ArgumentException("De ophaaldatum moet een geldige waarde hebben (Formaat: dd/mm/yyyy).");
                }

                if (x == 0)
                {
                    throw new ArgumentException("Er moet minstens 1 materiaal zijn waarbij het aantal groter is dan 0.");
                }

                DateTime indienDatum = ophaalDatum.Value.AddDays(aantalDagen);
                gebruiker.AddReservation(materialenTeReserveren, ophaalDatum.Value, indienDatum, DateTime.Now,
                    reservatieRepository.FindAll());
                reservatieRepository.SaveChanges();
                TempData["msg"] = "De reservatie  is toegevoegd aan uw verlanglijst.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
                return RedirectToAction("Index", "Verlanglijst");
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
                Reservatie r = gebruiker.RemoveReservation(res);
                reservatieRepository.Delete(r);
                reservatieRepository.SaveChanges();
                TempData["msg"] = "De reservatie is succesvol verwijderd.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveReservationLine(Gebruiker gebruiker, int reservatieId, int reservatieLineId)
        {
            Reservatie res = reservatieRepository.FindBy(reservatieId);

            try
            {
                ReservatieLijn rl = res.ReservatieLijnen.FirstOrDefault(rll => rll.Id == reservatieLineId);
                String name = rl.Materiaal.Name;

                gebruiker.RemoveReservationLijn(rl, reservatieRepository);
                reservatieRepository.SaveChanges();

                TempData["msg"] = "Het materiaal " + name + " is succesvol uit de reservatie verwijderd.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }
            return RedirectToAction("Detail", new { id = reservatieId });
        }

        public ActionResult Detail(Gebruiker gebruiker, int id)
        {
            Reservatie r = reservatieRepository.FindBy(id);

            if (r == null)
                return RedirectToAction("Index");

            ReservatieViewModel rv = new ReservatieViewModel(r);

            ConstructReservatieViewModels(r, rv, gebruiker);

            Config c = configWrapper.GetConfig();

            ViewBag.ophaalTijd = c.Ophaaltijd.ToString("HH:mm");
            ViewBag.indienTijd = c.Indientijd.ToString("HH:mm");

            return View(rv);
        }

        //test
        private void ConstructReservatieViewModels(Reservatie reservatie, ReservatieViewModel rvm, Gebruiker gebruiker)
        {
            List<ReservatieLijn> reservatielijnen = reservatie.ReservatieLijnen.
                    OrderBy(rl => rl.Materiaal.Name).ToList();
            for (int i = 0; i < reservatielijnen.Count; i++)
            {
                int aantalSlechtsBeschikbaar = reservatielijnen[i].FindConflicts(gebruiker.CanSeeAllMaterials());
                if (aantalSlechtsBeschikbaar != 0)
                {
                    rvm.Conflict = true;
                }
                rvm.ReservatieLijnen[i].AantalSlechtsBeschikbaar = aantalSlechtsBeschikbaar;
            }
        }

    }

}
