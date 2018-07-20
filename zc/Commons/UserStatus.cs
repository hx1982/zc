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

    public class ActivateType
    {
        /// <summary>
        /// 系统操作员
        /// </summary>
        public static readonly int OPERID = 1;
        /// <summary>
        /// 会员
        /// </summary>
        public static readonly int USERID = 2;
    }
    public class ActivateTypeHelper
    {
        public static string ToString(int activateType)
        {
            string str = string.Empty;
            if (activateType == ActivateType.OPERID)
                str = "操作员";
            else if (activateType == ActivateType.USERID)
                str = "会员";
            return str;
        }
    }
}