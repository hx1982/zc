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

        //会员未审核提现请求
        //提现状态 -1-审核不通过 0-待审核 1-待发放 2-已发放
        public ActionResult CashRecordNoAudit(string user_name, string user_phone, int? cash_type, DateTime? begin, DateTime? end, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var pageData = this._userManager.GetCashRequests(user_name, user_phone, cash_type, 0, begin, end, page, rows);
                var data = pageData.Select(c => new {
                    sdf = c.cash_money,

                });
                var total = this._userManager.GetCashRequestsTotal(user_name, user_phone, cash_type, 0, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

        public ActionResult CashRecordNoGrant(string user_name, string user_phone, int? cash_type, DateTime? begin, DateTime? end, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var pageData = this._userManager.GetCashRequests(user_name, user_phone, cash_type, 1, begin, end, page, rows);
                var data = pageData.Select(c => new {
                    sdf = c.cash_money,

                });
                var total = this._userManager.GetCashRequestsTotal(user_name, user_phone, cash_type, 1, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

        public ActionResult CashRecordAll(string user_name, string user_phone, int? cash_type,int? cash_status, DateTime? begin, DateTime? end, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var pageData = this._userManager.GetCashRequests(user_name, user_phone, cash_type, cash_status, begin, end, page, rows);
                var data = pageData.Select(c => new {
                    sdf = c.cash_money,

                });
                var total = this._userManager.GetCashRequestsTotal(user_name, user_phone, cash_type, cash_status, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

    }
}