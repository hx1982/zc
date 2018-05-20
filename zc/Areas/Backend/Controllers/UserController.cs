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


        public ActionResult All(string userName, string userPhone, string idNumber, int? levelId, string province, string city, string area, string referrerUserName, int? userStatus, DateTime? beginRegDate, DateTime? endRegDate, DateTime? beginActiveDate, DateTime? endActiveDate, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var users = this._userManager.GetAllUsers(userName, userPhone, idNumber, levelId, province, city, area, referrerUserName, userStatus, beginRegDate, endRegDate, beginActiveDate, endActiveDate, page, rows);
                var data = users.Select(u => new
                {
                    user_id = u.user_id,
                    user_code = u.user_code,
                    user_name = u.user_name,
                    user_phone = u.user_phone,
                    id_number = u.id_number,
                    level_name = u.level.level_name,
                    province = u.province,
                    city = u.city,
                    area = u.area,
                    address = u.address,
                    reg_money = u.reg_money,
                    referrer_name = u.referrer == null ? "无" : u.referrer.user_name,
                    user_status = UserStatusHelper.ToString(u.user_status),
                    register_time = u.register_time.ToLongDateString(),
                    activate_time = u.activate_time == null ? "" : u.activate_time.Value.ToLongDateString()
                });
                var total = this._userManager.GetAllUsersTotal(userName, userPhone, idNumber, levelId, province, city, area, referrerUserName, userStatus, beginRegDate, endRegDate, beginActiveDate, endActiveDate);
                return Json(new { total = total, rows = data });
            }
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
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.ERROR,
                    message = "数据校验错误"
                });
            }
        }

        // 财务管理 > 会员分红记录
        public ActionResult BonusRecords(string user_name, string user_phone, DateTime? begin, DateTime? end, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var pageData = this._userManager.GetBonusRecords(user_name, user_phone, begin, end, page, rows);
                var data = pageData.Select(ToBonusRecordViewModel);
                var total = this._userManager.GetBonusRecordsTotal(user_name, user_phone, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

        private object ToBonusRecordViewModel(bonus_record b)
        {
            return new
            {
                bonus_record_id = b.bonus_record_id,
                user_name = b.user.user_name,
                user_phone = b.user.user_phone,
                bonus_type = b.bouns_type == 1 ? "本金分红" : "推荐分红",
                bonus_money = b.bouns_money,
                bonus_time = b.create_time,
                bonus_remark = b.bonus_remark
            };
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