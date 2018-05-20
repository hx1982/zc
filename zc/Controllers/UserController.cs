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
            if (ModelState.IsValid)
            {
                var regSuccess = this.userManager.Register(model);
                if (regSuccess)
                {
                    return Content("<script>alert('注册成功, 请等待工作人员为您激活账户!');</script>");
                }
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
                else
                {
                    ViewBag.Message = "登录失败, 您可能手误, 也可能您的账户尚未激活";
                    return View(model);
                }
            }
            FormsAuthentication.SignOut();
            return View(model);
        }

        // 会员中心
        public ActionResult Center()
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            var userAccount = this.userManager.GetUserAccount(userId);
            ViewBag.User = user;
            return View(userAccount);
        }

        // 会员账户 - 金钻
        public ActionResult AccountGoldDiamond()
        {
            return View();
        }

        // 会员账户 - 银钻
        public ActionResult AccountSilverDiamond()
        {
            return View();
        }

        // 会员账户 - 蓝钻
        public ActionResult AccountBlueDiamond()
        {
            return View();
        }

        // 金钻提现申请
        public ActionResult CashGoldDiamond()
        {
            return View();
        }

        // 银钻提现申请
        public ActionResult CashSilverDiamond()
        {
            return View();
        }
    }
}