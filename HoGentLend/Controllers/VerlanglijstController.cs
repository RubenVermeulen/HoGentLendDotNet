using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoGentLend.Models.Domain;

namespace HoGentLend.Controllers
{
    public class VerlanglijstController : Controller
    {
        // GET: Verlanglijst
        public ActionResult Index(Gebruiker gebruiker)
        {
            return View();
        }
    }
}