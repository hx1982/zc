using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zc.Areas.Backend.Controllers
{
    public class HomeController : Controller
    {
        // GET: Backend/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }
    }
}