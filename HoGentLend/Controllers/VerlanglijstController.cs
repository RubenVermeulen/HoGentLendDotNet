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
    public class VerlanglijstController : Controller
    {
        private IMateriaalRepository materiaalRepository;

        public VerlanglijstController(IMateriaalRepository materiaalRepository)
        {
            this.materiaalRepository = materiaalRepository;
        }

        // GET: Index
        public ActionResult Index(Gebruiker gebruiker)
        {
            IEnumerable<MateriaalViewModel> materials = gebruiker.WishList
                .Materials
                .OrderBy(m => m.Name)
                .ToList()
                .Select(m => new MateriaalViewModel(m));

            return View("Index", materials);
        }

        // POST: Add
        [HttpPost]
        public ActionResult Add(Gebruiker gebruiker, int id) { 


            Materiaal mat = materiaalRepository.FindBy(id);
            try
            {
                gebruiker.AddToWishList(mat);
                materiaalRepository.SaveChanges();
                TempData["msg"] = "Het materiaal " + mat.Name + " is toegevoegd aan uw verlanglijst.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }
            return RedirectToAction("Index");
        }

        // POST: Remove
        //[HttpPost]
        public ActionResult Remove(Gebruiker gebruiker, int materiaalId)
        {
            Materiaal mat = materiaalRepository.FindBy(materiaalId);
            try
            {
                gebruiker.WishList.RemoveMaterial(mat);
                materiaalRepository.SaveChanges(); // dit zal ook de gebruiker veranderingen opslaan want het is overal dezeflde context
                TempData["msg"] = "Het materiaal " + mat.Name + " is verwijderd uit uw verlanglijst.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }
            return RedirectToAction("Index");
        }

    }
}