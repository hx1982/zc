using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zc.Areas.Backend.Controllers
{
    public class OrderController : Controller
    {
        // GET: Backend/Order/UnPayed
        public ActionResult UnPayed()
        {
            return View();
        }
    }
}