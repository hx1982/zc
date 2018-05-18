using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zc.Models;
namespace zc.Commons
{
    public static class Utility
    {
        public static readonly int USER_CODE_DEFAULT = 10000;

        private static object lockObj = new object();

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

        public static string GenerateLoginPwd(user user)
        {
            return user.id_number.Substring(user.id_number.Length - 6);
        }

        public static string GenereateSecondPwd(user user)
        {
            return GenerateLoginPwd(user);
        }

        public static string MD5Encrypt(string source)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5");
        }
    }
}