using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zc.Commons
{
    /// <summary>
    /// 提现状态
    /// </summary>
    public class CashStatus
    {
        /// <summary>
        /// 审核不通过
        /// </summary>
        public static readonly int AUDIT_DENY = -1;
        /// <summary>
        /// 待审核
        /// </summary>
        public static readonly int AUDIT_WAITING = 0;
        /// <summary>
        /// 待发放
        /// </summary>
        public static readonly int GIVEMONEY_WAITING = 1;
        /// <summary>
        /// 已发放
        /// </summary>
        public static readonly int GIVEMONEY_OK = 2;

        public static string ToString(int status)
        {
            string str = "未知";
            if (status == AUDIT_DENY) str = "审核不通过";
            else if (status == AUDIT_WAITING) str = "待审核";
            else if (status == GIVEMONEY_WAITING) str = "待发放";
            else if (status == GIVEMONEY_OK) str = "已发放";
            return str;
        }
    }

    /// <summary>
    /// 提现类型
    /// </summary>
    public class CashType
    {
        /// <summary>
        /// 分红提现
        /// </summary>
        public static readonly int GOLD_DIAMOND = 1;
        /// <summary>
        /// 茶票提现
        /// </summary>
        public static readonly int SILVER_DIAMOND = 2;
        /// <summary>
        /// 代币提现
        /// </summary>
        public static readonly int BLUE_DIAMOND = 3;

        public static string ToString(int type)
        {
            string str = "未知";
            if (type == GOLD_DIAMOND)
            {
                str = "金钻提现";
            }
            else if (type == SILVER_DIAMOND)
            {
                str = "银钻提现";
            }
            else if (type == BLUE_DIAMOND)
            {
                str = "蓝钻提现";
            }
            return str;
        }

    }

    /// <summary>
    /// 提现参数
    /// </summary>
    public class CashRate
    {
        /// <summary>
        /// 手续费
        /// </summary>
        public static readonly decimal SHOU_XU_FEI = decimal.Parse(Utility.GetAppKey("Poundage"));
        ///// <summary>
        ///// 复消费
        ///// </summary>
        //public static readonly decimal FU_XIAO_FEI = 0.2M;
    }
}