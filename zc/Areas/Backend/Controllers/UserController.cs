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

        #region 会员列表相关

        public ActionResult SearchNotActivatedUsers(string userName = "", string userPhone = "", int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var notActivatedUsers = _userManager.SearchNotActivedUsers(userName, userPhone, page, rows);
                var data = notActivatedUsers.Select(this.ToUserViewModel);
                var total = _userManager.SearchTotalOfNotActivatedUsers(userName, userPhone);
                return Json(new { total = total, rows = data });
            }
            //查询等级列表
            var level = this._userManager.GetLevelList();
            ViewBag.Levels = new SelectList(level, "level_money", "level_money");
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
                    level_name = u.level == null ? "" : u.level.level_name,
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

        #endregion

        #region 激活会员

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

        #endregion

        #region 会员分红相关

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
                bonus_type = b.bouns_type == 1 ? "众筹1分红" : "众筹2分红",
                bonus_money = b.bouns_money,
                bonus_time = b.create_time.ToLongDateString(),
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
                referrer = a.referrer == null ? "无" : a.referrer.user_name + "(" + a.referrer.user_code + ")"
            };
        }

        #endregion

        #region 会员提现相关

        /// <summary>
        /// 审核提现
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Route("/User/AuditCashRequest")]
        public ActionResult AuditCashRequest(FormCollection form)
        {
            var cashRecordId = form["cash_record_id"];
            if (string.IsNullOrEmpty(cashRecordId))
            {
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.ERROR,
                    message = "数据校验错误"
                });
            }
            int cashId = int.Parse(cashRecordId);
            //获取对象
            cash_record cashRecord = this._userManager.GetCashRecord(cashId);
            var operId = (Session[SessionConstants.CURRENTOPERATOR] as _operator).oper_id;

            if (cashRecordId == null)
            {
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.ERROR,
                    message = "数据校验错误"
                });
            }
            cashRecord.oper_id1 = operId;

            //判断是通过审核还是不通过审核
            var cash_status = form["cash_status"];
            //如果不通过
            if (cash_status.Equals(CashStatus.AUDIT_DENY))
            {
                cashRecord.cash_status = CashStatus.AUDIT_DENY;
                cashRecord.cash_time2 = DateTime.Now;
                cashRecord.cash_remark1 = form["cash_remark1"];

                this._userManager.UpdateCashRecord(cashRecord);

                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.OK,
                    message = "审核成功",
                    data = cashRecord
                });
            }

            //如果是通过的
            var userAccount = this._userManager.GetUserAccount(cashRecord.user_id);//查会员账户
            int cash_money = cashRecord.cash_money;
            int shou_xu_fei = Convert.ToInt32(cash_money * CashRate.SHOU_XU_FEI);
            int fu_xiao_fei = Convert.ToInt32(cash_money * CashRate.FU_XIAO_FEI);
            //判断是否需要的金额，大于了所剩余额
            if(cashRecord.cash_type == CashType.GOLD_DIAMOND) //金钻账户
            {
                if(userAccount.account1 < (cash_money + shou_xu_fei + fu_xiao_fei))
                {
                    cashRecord.cash_status = CashStatus.AUDIT_DENY;
                    cashRecord.cash_time2 = DateTime.Now;
                    cashRecord.cash_remark1 = "审核不成功，该账户余额不足以支付提现扣除";

                    this._userManager.UpdateCashRecord(cashRecord);

                    return Json(new AjaxResultObject
                    {
                        code = AjaxResultObject.ERROR,
                        message = "审核不成功，该账户余额不足以支付提现扣除"
                    });
                }
            }
            if (cashRecord.cash_type == CashType.SILVER_DIAMOND) //银钻账户
            {
                if (userAccount.account2 < (cash_money + shou_xu_fei + fu_xiao_fei))
                {
                    cashRecord.cash_status = CashStatus.AUDIT_DENY;
                    cashRecord.cash_time2 = DateTime.Now;
                    cashRecord.cash_remark1 = "审核不成功，该账户余额不足以支付提现扣除";

                    this._userManager.UpdateCashRecord(cashRecord);

                    return Json(new AjaxResultObject
                    {
                        code = AjaxResultObject.ERROR,
                        message = "审核不成功，该账户余额不足以支付提现扣除"
                    });
                }
            }
            cashRecord.cash_remark1 = form["cash_remark1"];

            cash_record mm = this._userManager.AuidCashRecord(cashRecord);
            if(mm.cash_status == CashStatus.GIVEMONEY_WAITING)
            {
                var c = mm;
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.OK,
                    message = "审核成功",
                    data = new {
                        user_name = c.user.user_name,
                        user_phone = c.user.user_phone,
                        cash_type = CashType.ToString(c.cash_type),
                        cash_money = c.cash_money,
                        shouxu_money = Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI),
                        fuxiao_money = Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                        actual_money = c.cash_money - Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI) - Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                        cash_status = CashStatus.ToString(c.cash_status),
                        cash_time1 = c.cash_time1.ToLongDateString(),
                        cash_record_id = c.cash_record_id,
                        user_id = c.user_id
                    }
                });
            }else
            {
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.ERROR,
                    message = "审核不成功"
                });
            }
            
        }

        //会员未审核提现请求
        //提现状态 -1-审核不通过 0-待审核 1-待发放 2-已发放
        public ActionResult CashRecordNoAudit(string user_name, string user_phone, int? cash_type, DateTime? begin, DateTime? end, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var pageData = this._userManager.GetCashRequests(null,user_name, user_phone, cash_type, CashStatus.AUDIT_WAITING, begin, end, page, rows);
                var data = pageData.Select(c => new {
                    user_name = c.user.user_name,
                    user_phone = c.user.user_phone,
                    cash_type = CashType.ToString(c.cash_type),
                    cash_money = c.cash_money,
                    shouxu_money = Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI),
                    fuxiao_money = Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                    actual_money = c.cash_money- Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI)- Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                    cash_status = CashStatus.ToString(c.cash_status),
                    cash_time1 = c.cash_time1.ToLongDateString(),
                    cash_record_id = c.cash_record_id,
                    user_id = c.user_id

                });
                var total = this._userManager.GetCashRequestsTotal(null,user_name, user_phone, cash_type, CashStatus.AUDIT_WAITING, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

        /// <summary>
        /// 发放奖金
        /// </summary>
        /// <returns></returns>
        public bool  UpdateGiveMoney(int cash_record_id)
        {
            var cashRecord = this._userManager.GetCashRecord(cash_record_id);
            var operId = (Session[SessionConstants.CURRENTOPERATOR] as _operator).oper_id;
            cashRecord.cash_status = CashStatus.GIVEMONEY_OK;
            cashRecord.oper_id2 = operId;
            cashRecord.cash_time3 = DateTime.Now;

            bool result = this._userManager.UpdateCashRecord(cashRecord);

            return result;
        }

        /// <summary>
        /// 查询未发放的提现记录
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="user_phone"></param>
        /// <param name="cash_type"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult CashRecordNoGrant(string user_name, string user_phone, int? cash_type, DateTime? begin, DateTime? end, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var pageData = this._userManager.GetCashRequests(null,user_name, user_phone, cash_type, CashStatus.GIVEMONEY_WAITING, begin, end, page, rows);
                var data = pageData.Select(c => new {
                    user_name = c.user.user_name,
                    user_phone = c.user.user_phone,
                    cash_type = CashType.ToString(c.cash_type),
                    cash_money = c.cash_money,
                    shouxu_money = Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI),
                    fuxiao_money = Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                    actual_money = c.cash_money - Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI) - Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                    cash_status = CashStatus.ToString(c.cash_status),
                    cash_time1 = c.cash_time1.ToLongDateString(),
                    cash_record_id = c.cash_record_id,
                    user_id = c.user_id

                });
                var total = this._userManager.GetCashRequestsTotal(null,user_name, user_phone, cash_type, CashStatus.GIVEMONEY_WAITING, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

        /// <summary>
        /// 查询全部提现记录
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="user_phone"></param>
        /// <param name="cash_type"></param>
        /// <param name="cash_status"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult CashRecordAll(string user_name, string user_phone, int? cash_type,int? cash_status, DateTime? begin, DateTime? end, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var pageData = this._userManager.GetCashRequests(null,user_name, user_phone, cash_type, cash_status, begin, end, page, rows);
                var data = pageData.Select(c => new {
                    user_name = c.user.user_name,
                    user_phone = c.user.user_phone,
                    cash_type = CashType.ToString(c.cash_type),
                    cash_money = c.cash_money,
                    shouxu_money = Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI),
                    fuxiao_money = Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                    actual_money = c.cash_money - Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI) - Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                    cash_status = CashStatus.ToString(c.cash_status),
                    cash_time1 = c.cash_time1.ToLongDateString(),
                    cash_record_id = c.cash_record_id,
                    user_id = c.user_id

                });
                var total = this._userManager.GetCashRequestsTotal(null,user_name, user_phone, cash_type, cash_status, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

        #endregion

    }
}