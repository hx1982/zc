using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Commons;
using zc.Managers;
using zc.Models;
using zc.Models.ViewModels;

namespace zc.Areas.Backend.Controllers
{
    public class UserController : Controller
    {
        private UserManager _userManager = new UserManager();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchNotActivatedUsers(string userName = "", string userPhone = "", int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var notActivatedUsers = _userManager.SearchNotActivedUsers(userName, userPhone, page, rows);
                var data = notActivatedUsers.Select(this.ToUserViewModel);
                var total = _userManager.SearchTotalOfNotActivatedUsers(userName, userPhone);
                return Json(new { total = total, rows = data });
            }
            return View();
        }


        public ActionResult All()
        {

            return View();
        }

        public ActionResult ActiveUser(UserActiveModel model)
        {
            var operId = (Session[SessionConstants.CURRENTOPERATOR] as _operator).oper_id;
            //var operId = 2;
            var user = _userManager.ActiveUser(model, operId);
            if (ModelState.IsValid)
            {
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.OK,
                    message = "OK",
                    data = ToUserViewModel(user)
                });
            }
            else
            {
                return Json(new AjaxResultObject {
                    code = AjaxResultObject.ERROR,
                    message = "数据校验错误"
                });
            }
        }

        private object ToUserViewModel(user a)
        {
            return new
            {
                user_id = a.user_id,
                user_status = a.user_status,
                user_code = a.user_code,
                user_name = a.user_name,
                user_phone = a.user_phone,
                id_number = a.id_number,
                province = a.province,
                city = a.city,
                area = a.area,
                address = a.address,
                reg_money = a.reg_money,
                referrer_id = a.referrer_id
            };
        }
    }
}