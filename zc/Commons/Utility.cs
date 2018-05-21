using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        private static object lockObj = new object();

        /// <summary>
        /// 生成会员编号user_code, 策略: user数量 + 1
        /// </summary>
        /// <returns></returns>
        public static string GenerateUserCode()
        {
            lock (lockObj)
            {
                using (ZCDbContext db = new ZCDbContext())
                {
                    int usercode = 0;
                    var query = db.users.Count();
                    if (query > 0)
                    {
                        usercode = query;
                    }
                    usercode++;
                    return usercode.ToString("00000000");
                }
            }
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