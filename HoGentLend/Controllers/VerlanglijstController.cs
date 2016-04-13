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
            // Twee materialen toevoegen om te testen
            //Add(gebruiker, 5);
            //Add(gebruiker, 3);


            IEnumerable<MateriaalViewModel> materials = gebruiker.WishList
                .Materials
                .ToList()
                .OrderBy(m => m.Name)
                .Select(m => new MateriaalViewModel(m));

            return View("Index", materials);
        }

        // POST: Add
        [HttpPost]
        public ActionResult Add(Gebruiker gebruiker, int id)
        {
            Materiaal mat = materiaalRepository.FindBy(id);
            if (mat == null || !gebruiker.CanSeeMaterial(mat))
            {
                TempData["err"] = "Het materiaal dat u wenste toe te voegen aan uw verlanglijst is niet beschikbaar.";
            }
            else
            {
                gebruiker.WishList.addMaterial(mat);
                materiaalRepository.SaveChanges(); // dit zal ook de gebruiker veranderingen opslaan want het is overal dezeflde context
                TempData["msg"] = "Het materiaal " + mat.Name + " is toegevoegd aan uw verlanglijst.";
            }

            return Index(gebruiker);
        }

        // POST: Remove
        [HttpPost]
        public ActionResult Remove(Gebruiker gebruiker, int materiaalId)
        {
            Materiaal mat = materiaalRepository.FindBy(materiaalId);
            if (mat == null)
            {
                TempData["err"] = "Het materiaal dat u wenste te verwijderen uit uw verlanglijst is niet beschikbaar.";
            }
            else
            {
                gebruiker.WishList.removeMaterial(mat);
                materiaalRepository.SaveChanges(); // dit zal ook de gebruiker veranderingen opslaan want het is overal dezeflde context
                TempData["msg"] = "Het materiaal " + mat.Name + " is verwijderd uit uw verlanglijst.";
            }
            return View("Index");
        }
        
    }
}