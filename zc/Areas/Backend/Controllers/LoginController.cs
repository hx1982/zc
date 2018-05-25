using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using zc.Commons;
using zc.Managers;

namespace zc.Areas.Backend.Controllers
{
    public class LoginController : Controller
    {

        private OperatorManager _operationManager = new OperatorManager();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Backend/Login
        [HttpPost]
        public ActionResult Index(string loginName, string loginPwd)
        {
            var success = _operationManager.Login(loginName, loginPwd);
            if (success)
            {
                //FormsAuthentication.SetAuthCookie(loginName, true);
                var currentOper = _operationManager.GetWithPermissions(loginName);
                Session[SessionConstants.CURRENTOPERATOR] = currentOper;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}