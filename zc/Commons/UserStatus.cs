using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zc.Commons
{
    /// <summary>
    /// 会员状态
    /// </summary>
    public class UserStatus
    {
        /// <summary>
        /// 冻结
        /// </summary>
        public static readonly int LOCKED = 0;
        /// <summary>
        /// 正常
        /// </summary>
        public static readonly int NORMAL = 1;
        /// <summary>
        /// 未激活
        /// </summary>
        public static readonly int NOT_ACTIVATED = 2;
    }

    public class UserStatusHelper
    {
        public static string ToString(int userStatus)
        {
            string str = string.Empty;
            if (userStatus == UserStatus.LOCKED)
                str = "冻结";
            else if (userStatus == UserStatus.NORMAL)
                str = "正常";
            else if (userStatus == UserStatus.NOT_ACTIVATED)
                str = "未激活";
            return str;
        }
    }
}