using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zc.Commons
{
    /// <summary>
    /// 账户类型 金，银，蓝
    /// </summary>
    public class AccountConstants
    {
        /// <summary>
        /// 金钻
        /// </summary>
        public static readonly int GOLD = 1;
        /// <summary>
        /// 银钻
        /// </summary>
        public static readonly int SILVER = 2;
        /// <summary>
        /// 蓝钻
        /// </summary>
        public static readonly int BLUE = 3;
        /// <summary>
        /// 茶票
        /// </summary>
        public static readonly int TEATICKET = 4;

        public static string ToString(int accType)
        {
            string str = string.Empty;
            switch (accType)
            {
                case 1: str = "金钻"; break;
                case 2: str = "银钻"; break;
                case 3: str = "蓝钻"; break;
                case 4: str = "茶票"; break;
            }
            return str;
        }

    }

   /// <summary>
   /// 消费类型 增加，减少
   /// </summary>
    public class ConType
    {
        /// <summary>
        /// 增加
        /// </summary>
        public static readonly int INCOME = 1;
        /// <summary>
        /// 减少
        /// </summary>
        public static readonly int EXPEND = -1;
    }

    /// <summary>
    /// 记录类型
    /// </summary>
    public class AccRecordType
    {
        /// <summary>
        /// 分红（奖金）
        /// </summary>
        public static readonly int BONUS_SHARE = 1;
        /// <summary>
        /// 系统增加(手工加)
        /// </summary>
        public static readonly int SYS_ADD = 2;
        /// <summary>
        /// 提现
        /// </summary>
        public static readonly int WITHDRAWAL = 3;
        /// <summary>
        /// 系统扣减(手工减)
        /// </summary>
        public static readonly int SYS_DELETE = 4;
        /// <summary>
        /// 手续费
        /// </summary>
        public static readonly int POUNDAGE = 5;
        /// <summary>
        /// 充值
        /// </summary>
        public static readonly int RECHARGE = 6;
        /// <summary>
        /// 激活会员
        /// </summary>
        public static readonly int ACTIVATE = 7;

        /// <summary>
        /// 转汉字
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToString(int status)
        {
            string str = "未知";
            if (status == BONUS_SHARE) str = "分红(奖金)";
            else if (status == SYS_ADD) str = "系统增加(手工加)";
            else if (status == WITHDRAWAL) str = "提现";
            else if (status == SYS_DELETE) str = "系统扣减(手工减)";
            else if (status == RECHARGE) str = "手续费";
            else if (status == POUNDAGE) str = "充值";
            else if (status == ACTIVATE) str = "激活会员";
            return str;
        }
    }
}