using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zc.Commons
{
    public class OrderType
    {
        public static readonly int REGISTER = 1;
        public static readonly int FUXIAO = 2;

        public static string ToString(int type)
        {
            var str = string.Empty;
            switch (type)
            {
                case 1: str = "注册单"; break;
                case 2: str = "复消单"; break;
            }
            return str;
        }
    }
    public class PayType
    {
        public static readonly int ONLINE = 1;
        public static readonly int OFFLINE = 2;

        public static string ToString(int type)
        {
            var str = string.Empty;
            switch (type)
            {
                case 1: str = "在线支付"; break;
                case 2: str = "线下支付"; break;
            }
            return str;
        }
    }

}