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
        /// 本金分红 改为 众筹1分红
        /// </summary>
        public static readonly int PRINCIPAL_SHARE = 1;
        /// <summary>
        /// 推荐分红 改为 众筹2分红
        /// </summary>
        public static readonly int REFERRER_SHARE = 2;
        /// <summary>
        /// 系统补偿
        /// </summary>
        public static readonly int SYSTEM_COMPENSATION = 3;
        /// <summary>
        /// 扣手续费
        /// </summary>
        public static readonly int MINUS_SHOU_XU_FEI = 4;

        /// <summary>
        /// 扣复消费
        /// </summary>
        public static readonly int MINUS_FU_XIAO_FEI = 5;

        /// <summary>
        /// 系统扣除
        /// </summary>
        public static readonly int MINUS_SYSTEM = 6;

        /// <summary>
        /// 余额转入
        /// </summary>
        public static readonly int ADD_BALANCE = 7;

        /// <summary>
        /// 金钻提现
        /// </summary>
        public static readonly int GOLD_CASH = 8;

        /// <summary>
        /// 银钻提现
        /// </summary>
        public static readonly int SILVER_CASH = 9;

        /// <summary>
        /// 金钻转入
        /// </summary>
        public static readonly int ADD_GOLD = 10;

        /// <summary>
        /// 银钻转入
        /// </summary>
        public static readonly int ADD_SILVER =11;

        /// <summary>
        /// 转汉字
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToString(int status)
        {
            string str = "未知";
            if (status == PRINCIPAL_SHARE) str = "众筹1分红";
            else if (status == REFERRER_SHARE) str = "众筹2分红";
            else if (status == SYSTEM_COMPENSATION) str = "系统补偿";
            else if (status == MINUS_SHOU_XU_FEI) str = "扣手续费";
            else if (status == MINUS_FU_XIAO_FEI) str = "扣复消费";
            else if (status == MINUS_SYSTEM) str = "系统扣除";
            else if (status == ADD_BALANCE) str = "余额转入";
            else if (status == GOLD_CASH) str = "金钻提现";
            else if (status == SILVER_CASH) str = "银钻提现";
            else if (status == ADD_GOLD) str = "金钻转入";
            else if (status == ADD_SILVER) str = "银钻转入";
            return str;
        }
    }
}