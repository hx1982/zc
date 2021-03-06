﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Managers;
using zc.Models.ViewModels;
using zc.Filters;
using System.Web.Security;
using zc.Commons;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Text;
using zc.Models;
using ThoughtWorks.QRCode.Codec.Data;

namespace zc.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();

        public ActionResult Index()
        {
            return View();
        }
        #region 注册相关

        [AllowAnonymous]
        public ActionResult Register(int? id)
        {
            if (id.HasValue)
            {
                var user = this.userManager.GetUser(id.Value);
                ViewBag.ReferrerUserCode = user.user_code;
                return View(new UserRegisterModel { ReferrerUserCode = user.user_code, HelpMode = true });
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var regSuccess = this.userManager.Register(model);
                if (regSuccess)
                {
                    if (model.HelpMode.HasValue && model.HelpMode.Value)
                    {
                        return RedirectToAction("Center");
                    }
                    return Content("<script>alert('注册成功, 请等待工作人员为您激活账户!');</script>");
                }
            }
            return View();
        }

        #endregion

        #region 登录相关

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.userManager.Login(model.UserPhone, model.LoginPwd);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.user_id.ToString(), false);
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

        #endregion

        #region 会员账户相关

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
        public ActionResult AccountGoldDiamond(int? acc_record_type, DateTime? dateBegin, DateTime? dateEnd, int page = 1, int rows = 20)
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            var userAccount = this.userManager.GetUserAccount(userId);
            ViewBag.User = user;
            //统计累计转入，消费
            ViewBag.TotalIncome = this.userManager.GetTotalIncome(AccountConstants.GOLD, userId);
            ViewBag.TotalExpend = this.userManager.GetTotalExpend(AccountConstants.GOLD, userId);
            ViewBag.Balance = userAccount.account1_balance;

            //查询列表
            var accountRecordList = this.userManager.GetAccountRecordList(userId, AccountConstants.GOLD, acc_record_type, dateBegin, dateEnd, page, rows);
            ViewBag.RecordList = accountRecordList;

            //如果是Ajax请求, 说明是上拉加载
            if (Request.IsAjaxRequest())
            {
                return JsonForPullUp(accountRecordList);
            }

            return View();
        }

        private ActionResult JsonForPullUp(List<account_record> accountRecordList)
        {
            if (accountRecordList.Count > 0)
            {
                return Json(new AjaxResultObject()
                {
                    code = AjaxResultObject.OK,
                    message = "OK",
                    data = accountRecordList.Select(a => new
                    {
                        acc_record_type = AccRecordType.ToString(a.acc_record_type),
                        cons_value = a.cons_value,
                        acc_record_time = a.acc_record_time.ToString("yyyy/MM/dd")
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new AjaxResultObject()
                {
                    code = AjaxResultObject.ERROR,
                    message = "没更多有数据了"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        // 会员账户 - 银钻
        public ActionResult AccountSilverDiamond(int? acc_record_type, DateTime? dateBegin, DateTime? dateEnd, int page = 1, int rows = 20)
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            var userAccount = this.userManager.GetUserAccount(userId);
            ViewBag.User = user;
            //统计累计转入，消费
            ViewBag.TotalIncome = this.userManager.GetTotalIncome(AccountConstants.SILVER, userId);
            ViewBag.TotalExpend = this.userManager.GetTotalExpend(AccountConstants.SILVER, userId);
            ViewBag.Balance = userAccount.account2_balance;

            //查询列表
            var accountRecordList = this.userManager.GetAccountRecordList(userId, AccountConstants.SILVER, acc_record_type, dateBegin, dateEnd, page, rows);
            ViewBag.RecordList = accountRecordList;

            //如果是Ajax请求, 说明是上拉加载
            if (Request.IsAjaxRequest())
            {
                return JsonForPullUp(accountRecordList);
            }
            return View();
        }

        // 会员账户 - 蓝钻
        public ActionResult AccountBlueDiamond(int? acc_record_type, DateTime? dateBegin, DateTime? dateEnd, int page = 1, int rows = 20)
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            var userAccount = this.userManager.GetUserAccount(userId);
            ViewBag.User = user;
            //统计累计转入，消费
            ViewBag.TotalIncome = this.userManager.GetTotalIncome(AccountConstants.BLUE, userId);
            ViewBag.TotalExpend = this.userManager.GetTotalExpend(AccountConstants.BLUE, userId);
            ViewBag.Balance = userAccount.account3_balance;

            //查询列表
            var accountRecordList = this.userManager.GetAccountRecordList(userId, AccountConstants.BLUE, acc_record_type, dateBegin, dateEnd, page, rows);
            ViewBag.RecordList = accountRecordList;

            //如果是Ajax请求, 说明是上拉加载
            if (Request.IsAjaxRequest())
            {
                return JsonForPullUp(accountRecordList);
            }

            return View();
        }

        #endregion

        #region 提现相关


        // 金钻提现申请  分红提现
        public ActionResult CashGoldDiamond(int? cash_money, string second_password)
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            var userAccount = this.userManager.GetUserAccount(userId);

            if (!cash_money.HasValue)
            {
                ViewBag.user = user;
                ViewBag.userAccount = userAccount;
                return View();
            }
            //验证二次密码是否正确
            if (!user.second_password.Equals(Utility.MD5Encrypt(second_password)))
            {
                return Content("false");
            }

            //获取提现的值
            cash_record model = new cash_record();
            model.cash_money = int.Parse(cash_money.ToString());
            model.user_id = userAccount.user_id;
            model.cash_type = CashType.GOLD_DIAMOND;

            bool result = this.userManager.InsertCashRecord(model);

            return Content(result.ToString());
        }

        // 银钻提现申请  茶票提现
        public ActionResult CashSilverDiamond(int? cash_money, string second_password)
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            var userAccount = this.userManager.GetUserAccount(userId);

            if (!cash_money.HasValue)
            {
                ViewBag.user = user;
                ViewBag.userAccount = userAccount;
                return View();
            }
            //验证二次密码是否正确
            if (!user.second_password.Equals(Utility.MD5Encrypt(second_password)))
            {
                return Content("false");
            }

            //获取提现的值
            cash_record model = new cash_record();
            model.cash_money = int.Parse(cash_money.ToString());
            model.user_id = userAccount.user_id;
            model.cash_type = CashType.SILVER_DIAMOND;

            bool result = this.userManager.InsertCashRecord(model);

            return Content(result.ToString());
        }
       
        //蓝钻提现
        public ActionResult CashBlueDiamond(int? cash_money, string second_password,int? cash_type)
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            var userAccount = this.userManager.GetUserAccount(userId);

            if (!cash_money.HasValue)
            {
                ViewBag.user = user;
                ViewBag.userAccount = userAccount;
                return View();
            }
            //验证二次密码是否正确
            if (!user.second_password.Equals(Utility.MD5Encrypt(second_password)))
            {
                return Content("false");
            }

            //获取提现的值
            cash_record model = new cash_record();
            model.cash_money = int.Parse(cash_money.ToString());
            model.user_id = userAccount.user_id;
            model.cash_type = (int)cash_type;

            bool result = this.userManager.InsertCashRecord(model);

            return Content(result.ToString());
        }

        // 用户在蓝钻提现时补填钱包地址
        public ActionResult FillUpWalletAddress(string wallet_address)
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            user.wallet_adder = wallet_address;
            this.userManager.Update<user>(user);
            this.userManager.SaveChanges();
            return Content("补填钱包地址成功");
        }

        /// <summary>
        /// 提现记录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CashRecordList()
        {
            var userId = int.Parse(User.Identity.Name);
            int completeGoldCash = this.userManager.GetCashMoneyTotal(CashType.GOLD_DIAMOND, userId, CashStatus.GIVEMONEY_OK);
            //int completeSilverCash = this.userManager.GetCashMoneyTotal(CashType.SILVER_DIAMOND, userId, CashStatus.GIVEMONEY_OK);
            int completeBlueCash = this.userManager.GetCashMoneyTotal(CashType.BLUE_DIAMOND, userId, CashStatus.GIVEMONEY_OK);
            int sumCashMoney = completeGoldCash+ completeBlueCash;

            ViewBag.CompleteGoldCash = completeGoldCash;
            //ViewBag.CompleteSilverCash = completeSilverCash;
            ViewBag.completeBlueCash = completeBlueCash;
            ViewBag.SumCashMoney = sumCashMoney;
            // 直接查出未审核提现, 不用Ajax, 但最多20条(应该够了吧)
            //var auditWaiting = this.userManager.GetCashRequests(userId, null, null, null, CashStatus.AUDIT_WAITING, null, null, 1, 20);
            //ViewBag.AuditWaiting = auditWaiting;

            return View();
        }

        /// <summary>
        /// 查询提现记录的列表
        /// </summary>
        /// <param name="cash_type">金钻还是银钻</param>
        /// <param name="cash_status">状态</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="rows">条数</param>
        /// <returns></returns>
        public ActionResult SelectCashRecordList(int? cash_type, int? cash_status, DateTime? beginTime, DateTime? endTime, int page = 1, int rows = 20)
        {
            var userId = int.Parse(User.Identity.Name);

            var pageData = this.userManager.GetCashRequests(userId, null, null, cash_type, cash_status, beginTime, endTime, page, rows);

            var data = pageData.Select(c => new
            {
                user_id = c.user_id,
                cash_type = CashType.ToString(c.cash_type),
                cash_money = c.cash_money,
                cash_status = CashStatus.ToString(c.cash_status),
                cash_time1 = c.cash_time1.ToString("yyyy-MM-dd HH:mm:ss"),
                cash_record_id = c.cash_record_id

            });
            var total = this.userManager.GetCashRequestsTotal(userId, null, null, cash_type, cash_status, beginTime, endTime);
            return Json(new { total = total, rows = data });
        }


        #endregion

        #region 会员推荐码相关

        //会员账户推荐码页面
        public ActionResult RefferCode()
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            if (user.user_remark1 == null)
            {
                string str = Utility.GetAppKey("DefaultCodeUrl") + "/User/Register/" + user.user_id;
                user = CreateQRImg(str, user);
            }
            ViewBag.User = user;
            return View(user);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="str"></param>
        private user CreateQRImg(string str, user model)
        {
            string enCodeString = str;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            System.Drawing.Image image = qrCodeEncoder.Encode(enCodeString);
            string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
            string filepath = Server.MapPath(@"\photos") + "\\" + filename;
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);

            fs.Close();
            image.Dispose();

            //将路径保存在user表的remark1字段中
            model.user_remark1 = "/photos/" + filename;
            user user = userManager.SaveUserCodeForRemark1(model);
            return user;
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns></returns>
        public string CodeDecoder(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            Bitmap myBitmap = new Bitmap(Image.FromFile(filePath));
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }

        #endregion

        #region 个人信息及密码修改
        public ActionResult PersonalInfo()
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            return View(user);
        }

        [HttpPost]
        public ActionResult PersonalInfo(int user_id, string province, string city, string area, string address, string account_num, string bank_name, string wallet_adder)
        {
            this.userManager.UpdateUserPersonalInfo(user_id, province, city, area, address, account_num, bank_name, wallet_adder);
            return Json(new AjaxResultObject
            {
                code = AjaxResultObject.OK,
                message = "修改个人信息成功"
            });
        }

        public ActionResult ChangePwd()
        {
            return View();
        }

        public ActionResult SaveLoginPwd(string loginPwd)
        {
            var userId = int.Parse(User.Identity.Name);
            this.userManager.UpdateLoginPwd(userId, loginPwd);
            return Json(new AjaxResultObject
            {
                code = AjaxResultObject.OK,
                message = "登录密码修改成功"
            });
        }

        public ActionResult SaveSecondPwd(string secondPwd)
        {
            var userId = int.Parse(User.Identity.Name);
            this.userManager.UpdateSecondPwd(userId, secondPwd);
            return Json(new AjaxResultObject
            {
                code = AjaxResultObject.OK,
                message = "二级密码修改成功"
            });
        }
        #endregion

        #region 会员激活会员相关

        public ActionResult NoActivateList(string phone, int page = 1, int rows = 20)
        {
            if (Request.IsAjaxRequest())
            {
                var userId = int.Parse(User.Identity.Name);
                var pageData = this.userManager.SearchNotActivedUsers(phone, userId, page, rows);

                var data = pageData.Select(c => new
                {
                    user_id = c.user_id,
                    user_code = c.user_code,
                    user_name = c.user_name,
                    phone = c.user_phone,
                    reffer_name = c.referrer.user_name
                });
                var total = this.userManager.SearchTotalOfNotActivatedUsers(phone, userId);
                return Json(new { total = total, rows = data }, JsonRequestBehavior.AllowGet);
            }
            //会员等级
            var levels = this.userManager.GetLevelList();
            ViewBag.Levels = levels;
            return View();
        }

        public ActionResult ActiveUser(int? auserId,int regMoney)
        {
            var userId = int.Parse(User.Identity.Name);
            var userAccount = userManager.GetUserAccount(userId);
            if(userAccount.account1_balance < regMoney)
            {
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.ERROR,
                    message = "金钻余额不足以支付激活所花费"
                });
            }

            var user = userManager.ActiveUser(auserId, regMoney, userId);
            if (ModelState.IsValid)
            {
                return Json(new AjaxResultObject
                {
                    code = AjaxResultObject.OK,
                    message = "OK"
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

    }
}