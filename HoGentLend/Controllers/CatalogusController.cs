using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoGentLend.Controllers
{
    public class CatalogusController : Controller
    {
        private IMateriaalRepository _materiaalRepository;

        public IMateriaalRepository MateriaalRepository
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        // GET: Catalogus
        public ActionResult Index()
        {
            return View();
        }

        // POST
        [HttpPost]
        public ActionResult Filter()
        {
            return View("Index");
        }
    }
}