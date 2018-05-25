using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using zc.Models;
namespace zc.Commons
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Utility
    {
        public static readonly int USER_CODE_DEFAULT = 10000;
        public static Random random = new Random();
        private static object lockObj = new object();

        /// <summary>
        /// 生成会员编号user_code
        /// </summary>
        /// <returns></returns>
        public static string GenerateUserCode(user user)
        {
            string usercode = user.id_number.Substring(user.id_number.Length - 4)
                + user.user_phone.Substring(user.user_phone.Length - 4);
            usercode = usercode.Replace('x', '0').Replace('X', '0');
            bool exists = AllreadyExsits(usercode);
            if (exists)
            {
                usercode = RandomUserCode();
            }
            return usercode;
        }

        private static bool AllreadyExsits(string usercode)
        {
            bool exists = false;
            using (ZCDbContext db = new ZCDbContext())
            {
                var query = db.users.Where(a => a.user_code.Equals(usercode)).Count();
                exists = query > 0;
            }
            return exists;
        }

        private static string RandomUserCode()
        {
            string usercode;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                sb.Append(random.Next(10));
            }
            usercode = sb.ToString();
            if (AllreadyExsits(usercode))
            {
                usercode = RandomUserCode();
            }
            return usercode;
        }

        /// <summary>
        /// 生成登录密码, 策略: 身份证号后6位
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GenerateLoginPwd(user user)
        {
            return user.id_number.Substring(user.id_number.Length - 6);
        }

        /// <summary>
        /// 生成二级密码, 策略: 和登录密码相同
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GenereateSecondPwd(user user)
        {
            return GenerateLoginPwd(user);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string source)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5");
        }

        /// <summary>
        /// 获取配置文件的Key对应的值   
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static string GetAppKey(string keyValue)
        {
            return ConfigurationManager.AppSettings[keyValue];
        }
    }
}