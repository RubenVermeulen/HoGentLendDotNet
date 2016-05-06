using System;
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
                ReservatieViewModel rvm = new ReservatieViewModel(reservatie);
                reservaties.Add(rvm);

                //FindConflicts(reservatie, rvm, gebruiker);

                // @Svonx: wordt later nog in een aparte methode gestoken, staat hier nu voor izi bugfixing
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
            // TODO  zorgen dat ophaaldatum en indiendatum ook de uren hebben van uit de config

            
            var materialenTeReserveren = new Dictionary<Materiaal, long>();
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

                DateTime indienDatum = ophaalDatum.Value.AddDays(7 * weeks);
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

            FindConflicts(r, rv, gebruiker);

            return View(rv);
        }

        //test

        private ReservatieViewModel FindConflicts(Reservatie reservatie, ReservatieViewModel rvm, Gebruiker gebruiker)
        {
            List<ReservatieLijn> reservatielijnen = reservatie.ReservatieLijnen.
                OrderBy(rl => rl.Materiaal.Name).ToList();
            for (int i = 0; i < reservatielijnen.Count; i++)
            {
                ReservatieLijn rl = reservatielijnen[i];
                Materiaal m = rl.Materiaal;

                DateTime? indienmoment = rl.IndienMoment;
                DateTime? ophaalmoment = rl.OphaalMoment;

                //alle overlappende reservaties in 1 lijst
                List<ReservatieLijn> overlappendeLijnen = m.ReservatieLijnen.Where(r => (
                    (r.IndienMoment <= indienmoment && r.OphaalMoment > indienmoment)
                    || (r.IndienMoment <= ophaalmoment && r.OphaalMoment > ophaalmoment)
                    || (r.IndienMoment >= indienmoment && r.OphaalMoment <= ophaalmoment)
                    )).ToList();

                int totaalAantalBeschikbaar = m.Amount - m.AmountNotAvailable;

                //als er meer gereserveerd zijn dan beschikbaar
                if (overlappendeLijnen.Sum(r => r.Amount) > totaalAantalBeschikbaar)
                {
                    int aantalNogBeschikbaar = totaalAantalBeschikbaar;

                    //als geen lector is
                    //verminder aantalNogBeschikbare Reservaties indien een lijn wordt tegengekomen met 
                    //vroegere reservatiedatum
                    if (!gebruiker.CanSeeAllMaterials())
                    {
                        foreach (var lijn in overlappendeLijnen)
                        {
                            Reservatie bijhorendeReservatie = reservatieRepository.FindBy((int)lijn.ReservatieId);
                            System.Diagnostics.Debug.WriteLine("DetailReservatie van overlappende lijn" + ": " + bijhorendeReservatie);
                            if (bijhorendeReservatie.Lener.CanSeeAllMaterials() ||
                                (bijhorendeReservatie.Reservatiemoment < reservatie.Reservatiemoment))
                            {
                                aantalNogBeschikbaar -= (int)lijn.Amount;
                            }
                        }
                    }
                    //als wel lector is
                    //verminder aantalNogBeschikbare enkel wanneer de lijn met vroegere reservatiedatum
                    //ook van een lector was
                    else
                    {
                        foreach (var lijn in overlappendeLijnen)
                        {
                            Reservatie bijhorendeReservatie = reservatieRepository.FindBy((int)lijn.ReservatieId);
                            if (bijhorendeReservatie.Lener.CanSeeAllMaterials()
                                && bijhorendeReservatie.Reservatiemoment < reservatie.Reservatiemoment)
                            {
                                aantalNogBeschikbaar -= (int)lijn.Amount;
                            }
                        }
                    }

                    //Indien gebruiker laatste was om te reserveren, en dus materiaal niet kan meekrijgen,
                    //wordt er berekend hoeveel hij er slechts krijgt
                    if (aantalNogBeschikbaar < rl.Amount)
                    {
                        System.Diagnostics.Debug.WriteLine(aantalNogBeschikbaar + ", " + rl.Amount);
                        rvm.Conflict = true;
                        //laat view weten dat er geen materialen meer beschikbaar zijn voor gebruiker
                        if (aantalNogBeschikbaar <= 0)
                        {
                            rvm.ReservatieLijnen[i].AantalSlechtsBeschikbaar = -1;
                        }
                        else
                        {
                            rvm.ReservatieLijnen[i].AantalSlechtsBeschikbaar = (int)rl.Amount - aantalNogBeschikbaar;
                        }
                    }

                }
            }

            return rvm;
        }
    }

}
