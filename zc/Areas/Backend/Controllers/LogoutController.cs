using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Commons;

namespace zc.Areas.Backend.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Backend/Logout
        public ActionResult Index()
        {
            Session[SessionConstants.CURRENTOPERATOR] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}