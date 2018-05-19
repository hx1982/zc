using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Managers;
using zc.Models.ViewModels;
using zc.Filters;
using System.Web.Security;
using zc.Commons;

namespace zc.Controllers
{
    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRegisterModel model)
        {
            var regSuccess = this.userManager.Register(model);
            if (regSuccess)
            {
                return Content("注册成功, 请等待工作人员为您激活账户!");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.userManager.Login(model.UserPhone, model.LoginPwd);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.user_id.ToString(), true);
                    return RedirectToAction("Center");
                }
            }
            FormsAuthentication.SignOut();
            return View();
        }

        public ActionResult Center()
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            var userAccount = this.userManager.GetUserAccount(userId);
            ViewBag.User = user;
            return View(userAccount);
        }
        
        public ActionResult AccountGoldDiamond()
        {
            return View();
        }

        public ActionResult AccountSilverDiamond()
        {
            return View();
        }

        public ActionResult AccountBlueDiamond()
        {
            return View();
        }
    }
}