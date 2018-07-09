using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Commons;
using zc.Managers;
using zc.Models;
using zc.Models.ViewModels;
using zc.Filters;
namespace zc.Areas.Backend.Controllers
{
    //[OperatorAuthorize]
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
                    register_time = u.register_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    activate_time = u.activate_time == null ? "" : u.activate_time.Value.ToString("yyyy-MM-dd HH:mm:ss")
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

        private object ToBonusRecordViewModel(account_record b)
        {
            return new
            {
                user_name = b.user.user_name,
                user_phone = b.user.user_phone,
                bonus_money = b.cons_value,
                bonus_time = b.acc_record_time.ToString("yyyy-MM-dd HH:mm:ss"),
                bonus_remark = b.acc_remark,
                acc_balance = b.acc_balance
            };
        }

        //private object ToBonusRecordViewModel(bonus_record b)
        //{
        //    return new
        //    {
        //        bonus_record_id = b.bonus_record_id,
        //        user_name = b.user.user_name,
        //        user_phone = b.user.user_phone,
        //        bonus_type = b.bouns_type == 1 ? "众筹1分红" : "众筹2分红",
        //        // todo: 由原来的bouns_money改为了bouns_value, 待测试
        //        bonus_money = b.bouns_value,
        //        bonus_time = b.create_time.ToString("yyyy-MM-dd HH:mm:ss"),
        //        bonus_remark = b.bonus_remark
        //    };
        //}

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
                referrer = a.referrer == null ? "无" : a.referrer.user_name + "(" + a.referrer.user_code + ")",
                register_time = a.register_time.ToString("yyyy-MM-dd HH:mm:ss")
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
        public bool BachAuditCashRequest(int[] recordIds, int cash_status)
        {
            for (int i = 0; i < recordIds.Length; i++)
            {
                int cashId = recordIds[i];
                //获取对象
                cash_record cashRecord = this._userManager.GetCashRecord(cashId);
                var operId = (Session[SessionConstants.CURRENTOPERATOR] as _operator).oper_id;

                if (cashRecord == null)
                {
                    continue;
                }
                cashRecord.oper_id1 = operId;

                //判断是通过审核还是不通过审核
                //如果不通过
                if (cash_status == CashStatus.AUDIT_DENY)
                {
                    cashRecord.cash_status = CashStatus.AUDIT_DENY;
                    cashRecord.cash_time2 = DateTime.Now;

                    this._userManager.UpdateCashRecord(cashRecord);
                    continue;
                }

                //如果是通过的
                var userAccount = this._userManager.GetUserAccount(cashRecord.user_id);//查会员账户
                int cash_money = cashRecord.cash_money;
                int shou_xu_fei = Convert.ToInt32(cash_money * CashRate.SHOU_XU_FEI);
                //int fu_xiao_fei = Convert.ToInt32(cash_money * CashRate.FU_XIAO_FEI);
                //判断是否需要的金额，大于了所剩余额
                if (cashRecord.cash_type == CashType.GOLD_DIAMOND) //金钻账户
                {
                    if (userAccount.account1_balance < (cash_money + shou_xu_fei))
                    {
                        cashRecord.cash_status = CashStatus.AUDIT_DENY;
                        cashRecord.cash_time2 = DateTime.Now;
                        cashRecord.cash_remark1 = "审核不成功，该账户余额不足以支付提现扣除";

                        this._userManager.UpdateCashRecord(cashRecord);

                        continue;
                    }
                }
                if (cashRecord.cash_type == CashType.SILVER_DIAMOND) //银钻账户
                {
                    if (userAccount.account2_balance < (cash_money + shou_xu_fei))
                    {
                        cashRecord.cash_status = CashStatus.AUDIT_DENY;
                        cashRecord.cash_time2 = DateTime.Now;
                        cashRecord.cash_remark1 = "审核不成功，该账户余额不足以支付提现扣除";

                        this._userManager.UpdateCashRecord(cashRecord);

                        continue;
                    }
                }

                cash_record mm = this._userManager.AuidCashRecord(cashRecord);
                if (mm.cash_status == CashStatus.GIVEMONEY_WAITING)
                {
                    var c = mm;
                    continue;
                }
                else
                {
                    continue;
                }
            }
            return true;

        }

        /// <summary>
        /// 单个，要弹出对话框的 审核提现
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
            //int fu_xiao_fei = Convert.ToInt32(cash_money * CashRate.FU_XIAO_FEI);
            //判断是否需要的金额，大于了所剩余额
            if (cashRecord.cash_type == CashType.GOLD_DIAMOND) //金钻账户
            {
                if (userAccount.account1_balance < (cash_money + shou_xu_fei))
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
                if (userAccount.account2_balance < (cash_money + shou_xu_fei))
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
            if (cashRecord.cash_type == CashType.BLUE_DIAMOND) //代币提现
            {
                if (userAccount.account3_balance < (cash_money + shou_xu_fei))
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
            if (mm.cash_status == CashStatus.GIVEMONEY_WAITING)
            {
                var c = mm;
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.OK,
                    message = "审核成功",
                    data = new
                    {
                        user_name = c.user.user_name,
                        user_phone = c.user.user_phone,
                        cash_type = CashType.ToString(c.cash_type),
                        cash_money = c.cash_money,
                        shouxu_money = Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI),
                        //fuxiao_money = Convert.ToInt32(c.cash_money * CashRate.FU_XIAO_FEI),
                        //actual_money = c.cash_money - Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI) - Convert.ToInt32(c.cash_money * 1 /** CashRate.FU_XIAO_FEI*/),
                        cash_status = CashStatus.ToString(c.cash_status),
                        cash_time1 = c.cash_time1.ToString("yyyy-MM-dd HH:mm:ss"),
                        cash_record_id = c.cash_record_id,
                        user_id = c.user_id
                    }
                });
            }
            else
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
                var pageData = this._userManager.GetCashRequests(null, user_name, user_phone, cash_type, CashStatus.AUDIT_WAITING, begin, end, page, rows);
                var data = pageData.Select(c => new
                {
                    user_name = c.user.user_name,
                    user_phone = c.user.user_phone,
                    cash_type = CashType.ToString(c.cash_type),
                    cash_money = c.cash_money,
                    shouxu_money = Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI),
                    cash_status = CashStatus.ToString(c.cash_status),
                    cash_time1 = c.cash_time1.ToString("yyyy-MM-dd HH:mm:ss"),
                    cash_record_id = c.cash_record_id,
                    user_id = c.user_id

                });
                var total = this._userManager.GetCashRequestsTotal(null, user_name, user_phone, cash_type, CashStatus.AUDIT_WAITING, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

        /// <summary>
        /// 发放奖金
        /// </summary>
        /// <returns></returns>
        public bool UpdateGiveMoney(int[] recordIds)
        {
            var operId = (Session[SessionConstants.CURRENTOPERATOR] as _operator).oper_id;

            //var cashRecord = this._userManager.GetCashRecord(cash_record_id);
            //cashRecord.cash_status = CashStatus.GIVEMONEY_OK;
            //cashRecord.oper_id2 = operId;
            //cashRecord.cash_time3 = DateTime.Now;

            //bool result = this._userManager.UpdateCashRecord(cashRecord);
            int[] IDS = recordIds;
            bool result = this._userManager.BachUpdateCashRecord(IDS, operId);

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
                var pageData = this._userManager.GetCashRequests(null, user_name, user_phone, cash_type, CashStatus.GIVEMONEY_WAITING, begin, end, page, rows);
                var data = pageData.Select(c => new
                {
                    user_name = c.user.user_name,
                    user_phone = c.user.user_phone,
                    cash_type = CashType.ToString(c.cash_type),
                    cash_money = c.cash_money,
                    shouxu_money = Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI),
                    cash_status = CashStatus.ToString(c.cash_status),
                    cash_time1 = c.cash_time1.ToString("yyyy-MM-dd HH:mm:ss"),
                    cash_record_id = c.cash_record_id,
                    user_id = c.user_id

                });
                var total = this._userManager.GetCashRequestsTotal(null, user_name, user_phone, cash_type, CashStatus.GIVEMONEY_WAITING, begin, end);
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
        public ActionResult CashRecordAll(string user_name, string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var pageData = this._userManager.GetCashRequests(null, user_name, user_phone, cash_type, cash_status, begin, end, page, rows);
                var data = pageData.Select(c => new
                {
                    user_name = c.user.user_name,
                    user_phone = c.user.user_phone,
                    cash_type = CashType.ToString(c.cash_type),
                    cash_money = c.cash_money,
                    shouxu_money = Convert.ToInt32(c.cash_money * CashRate.SHOU_XU_FEI),
                    cash_status = CashStatus.ToString(c.cash_status),
                    cash_time1 = c.cash_time1.ToString("yyyy-MM-dd HH:mm:ss"),
                    cash_record_id = c.cash_record_id,
                    user_id = c.user_id

                });
                var total = this._userManager.GetCashRequestsTotal(null, user_name, user_phone, cash_type, cash_status, begin, end);
                return Json(new { total = total, rows = data });
            }
            return View();
        }

        #endregion

        #region 等级修改相关

        // 准备修改等级的视图
        public ActionResult PrepareChangeLevel()
        {
            var levels = this._userManager.GetLevelList();
            ViewBag.Levels = levels;
            return View();
        }
        // 修改等级前的查询
        public ActionResult SearchBeforeChangeLevel(string userName, string userPhone, int page = 1, int rows = 10)
        {
            var users = _userManager.SearchNormalUsersByNameAndPhone(userName, userPhone, page, rows);
            var data = users.Select(user => new
            {
                user_id = user.user_id,
                user_code = user.user_code,
                user_name = user.user_name,
                user_phone = user.user_phone,
                id_number = user.id_number,
                level_name = user.level.level_name,
                referrer_name = user.referrer != null ? user.referrer.user_name : "无"
            });
            var total = _userManager.SearchNormalUsersByNameAndPhoneTotal(userName, userPhone);
            return Json(new
            {
                total = total,
                rows = data
            });
        }
        // 修改等级
        public ActionResult ChangeLevel(int[] array_user_id, int new_level)
        {
            try
            {
                this._userManager.ChangeLevel(array_user_id, new_level);
                return Json(new AjaxResultObject { code = AjaxResultObject.OK, message = "等级修改成功" });
            }
            catch (Exception e)
            {
                return Json(new AjaxResultObject { code = AjaxResultObject.ERROR, message = "系统错误: " });
            }
        }
        #endregion

        #region 账户流水查询

        public ActionResult AccountLine(string userName, string userPhone, int? accType, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                if (!accType.HasValue)
                {
                    return null;
                }
                var lines = this._userManager.AccountLines(userName, userPhone, accType.Value, page, rows);
                var total = this._userManager.AccountLinesTotal(userName, userPhone, accType.Value);
                var result = new
                {
                    total = total,
                    rows = lines.Select(a => new
                    {
                        user_name = a.user.user_name,
                        user_phone = a.user.user_phone,
                        acc_type = AccountConstants.ToString(a.acc_type),
                        acc_record_type = AccRecordType.ToString(a.acc_record_type),
                        cons_value = a.cons_type == ConType.EXPEND ? "-" + a.cons_value : "+" + a.cons_value,
                        acc_record_time = a.acc_record_time.ToString("yyyy-MM-dd HH:mm"),
                        acc_remark = a.acc_remark
                    })
                };
                return Json(result);
            }
            return View();
        }

        #endregion

        #region 账户汇总查询

        public ActionResult AccountSumamry()
        {
            return View();
        }

        #endregion

        #region 推荐树

        public ActionResult ReferTree()
        {
            return View();
        }

        public ActionResult ReferTreeTop3Layer(string userName = "", string userPhone = "")
        {
            var users = this._userManager.GetReferTree(userName, userPhone);
            var data = users.Select(a => new
            {
                id = a.user_id,
                pId = a.referrer_id,
                name = a.user_name+"-"+a.user_phone+"-"+a.level.level_name+"-"+a.reg_money+"元",
                isParent = true,
                open = users.Count(item => item.referrer_id == a.user_id) > 0
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReferTreeChilds(int id)
        {
            var children = this._userManager.GetReferTreeChilds(id);
            return Json(children, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 会员账户相关
        
        public ActionResult UserAccountList(string userName, string userPhone, string idNumber, int? levelId, string referrerUserName, int? userStatus, int page = 1, int rows = 10)
        {
            if (Request.IsAjaxRequest())
            {
                var accountUser = this._userManager.GetAllUserAccount(userName, userPhone, idNumber, levelId, referrerUserName, userStatus, page, rows);
                var data = accountUser.Select(u => new
                {
                    user_id = u.user_id,
                    user_code = u.user.user_code,
                    user_name = u.user.user_name,
                    user_phone = u.user.user_phone,
                    id_number = u.user.id_number,
                    level_name = u.user.level == null ? "" : u.user.level.level_name,
                    reg_money = u.user.reg_money,
                    referrer_name = u.user.referrer == null ? "无" : u.user.referrer.user_name,
                    user_status = UserStatusHelper.ToString(u.user.user_status),
                    account1_balance = u.account1_balance,
                    account2_balance = u.account2_balance,
                    account3_balance = u.account3_balance
                });
                var total = this._userManager.GetAllUserAccountTotal(userName, userPhone, idNumber, levelId, referrerUserName, userStatus);
                return Json(new { total = total, rows = data });
            }
            return View();
        }


        [Route("/User/AddDeleteMoney")]
        public bool AddDeleteMoney(int[] userIds, int addOrDelete,string money,int accountType)
        {
            for (int i = 0; i < userIds.Length; i++)
            {
                int userId = userIds[i];
                //获取对象
                user_account userAccount = this._userManager.GetUserAccount(userId);
                var operId = (Session[SessionConstants.CURRENTOPERATOR] as _operator).oper_id;
                if (userAccount == null)
                {
                    continue;
                }
                account_record accountRecord = new account_record();
                accountRecord.user_id = userAccount.user_id;
                accountRecord.cons_value = int.Parse(money);
                accountRecord.oper_id = operId;
                accountRecord.acc_remark = "系统手工操作";
                //判断是增加还是删除
                //addOrDelete增加是1   减是-1
                if (addOrDelete == 1)
                {
                    accountRecord.acc_record_type = AccRecordType.SYS_ADD;
                    accountRecord.cons_type = ConType.INCOME;
                    //金钻账户增加
                    if (accountType == AccountConstants.GOLD)
                    {
                        accountRecord.acc_type = AccountConstants.GOLD;
                        accountRecord.acc_balance = userAccount.account1_balance + int.Parse(money);

                        this._userManager.InsertAccountRecord(accountRecord);
                    }
                    //银钻账户增加
                    if (accountType == AccountConstants.SILVER)
                    {
                        accountRecord.acc_type = AccountConstants.SILVER;
                        accountRecord.acc_balance = userAccount.account2_balance + int.Parse(money);

                        this._userManager.InsertAccountRecord(accountRecord);
                    }
                    //蓝钻账户增加
                    if (accountType == AccountConstants.BLUE)
                    {
                        accountRecord.acc_type = AccountConstants.BLUE;
                        accountRecord.acc_balance = userAccount.account3_balance + int.Parse(money);

                        this._userManager.InsertAccountRecord(accountRecord);
                    }
                    continue;
                }
                if(addOrDelete == -1)
                {
                    accountRecord.acc_record_type = AccRecordType.SYS_DELETE;
                    accountRecord.cons_type = ConType.EXPEND;
                    //金钻账户减少
                    if (accountType == AccountConstants.GOLD)
                    {
                        accountRecord.acc_type = AccountConstants.GOLD;
                        accountRecord.acc_balance = userAccount.account1_balance - int.Parse(money);

                        this._userManager.InsertAccountRecord(accountRecord);
                    }
                    //银钻账户减少
                    if (accountType == AccountConstants.SILVER)
                    {
                        accountRecord.acc_type = AccountConstants.SILVER;
                        accountRecord.acc_balance = userAccount.account2_balance - int.Parse(money);

                        this._userManager.InsertAccountRecord(accountRecord);
                    }
                    //蓝钻账户减少
                    if (accountType == AccountConstants.BLUE)
                    {
                        accountRecord.acc_type = AccountConstants.BLUE;
                        accountRecord.acc_balance = userAccount.account3_balance - int.Parse(money);

                        this._userManager.InsertAccountRecord(accountRecord);
                    }
                    continue;
                }
            }
            return true;
        }


        #endregion

    }
}