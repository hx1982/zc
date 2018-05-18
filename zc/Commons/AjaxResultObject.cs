using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zc.Commons
{
    public class AjaxResultObject
    {
        public static readonly int OK = 200;
        public static readonly int ERROR = 500;

        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}