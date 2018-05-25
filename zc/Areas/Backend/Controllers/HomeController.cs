using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Filters;

namespace zc.Areas.Backend.Controllers
{
    public class HomeController : Controller
    {
        // GET: Backend/Home
        //[OperatorAuthorize]
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