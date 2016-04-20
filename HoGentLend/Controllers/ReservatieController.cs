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
        //[HttpPost]
        // public ActionResult Add(Gebruiker gebruiker, int id)
        // {
        //     Materiaal mat = materiaalRepository.FindBy(id);
        //     try
        //     {
        //         gebruiker.AddToWishList(mat);
        //         materiaalRepository.SaveChanges();
        //         TempData["msg"] = "Het materiaal " + mat.Name + " is toegevoegd aan uw verlanglijst.";
        //     }
        //     catch (ArgumentException e)
        //     {
        //         TempData["err"] = e.Message;
        //     }
        //     return Index(gebruiker);
        // }
    }
}
