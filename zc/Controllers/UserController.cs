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
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Text;
using zc.Models;
using ThoughtWorks.QRCode.Codec.Data;

namespace zc.Controllers
{
    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register(int? id)
        {
            if (id.HasValue)
            {
                var user = this.userManager.GetUser(id.Value);
                ViewBag.ReferrerUserCode = user.user_code;
                return View(new UserRegisterModel { ReferrerUserCode = user.user_code });
            }

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
        public ActionResult AccountGoldDiamond(int? acc_record_type, DateTime? dateBegin, DateTime? dateEnd, int page = 1, int rows = 20)
        {
            var userId = int.Parse(User.Identity.Name);
            var userAccount = this.userManager.GetUserAccount(userId);
            //统计累计转入，消费
            ViewBag.TotalIncome = this.userManager.GetTotalIncome(AccountConstants.GOLD, userId);
            ViewBag.TotalExpend = this.userManager.GetTotalExpend(AccountConstants.GOLD, userId);
            ViewBag.Balance = userAccount.account1;

            //查询列表
            var accountRecordList = this.userManager.GetAccountRecordList(userId, AccountConstants.GOLD, acc_record_type, dateBegin, dateEnd, page, rows);
            ViewBag.RecordList = accountRecordList;
           
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

        //会员账户推荐码页面
        public ActionResult RefferCode()
        {
            var userId = int.Parse(User.Identity.Name);
            var user = this.userManager.GetUser(userId);
            if(user.user_remark1 == null)
            {
                string str = Utility.GetAppKey("DefaultCodeUrl") +"/User/Register/" +user.user_id;
                user = CreateQRImg(str,user);
            }
            ViewBag.User = user;
            return View(user);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="str"></param>
        private user CreateQRImg(string str,user model)
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
            user user= userManager.SaveUserCodeForRemark1(model);
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
    }
}