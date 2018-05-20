using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zc.Models.ViewModels
{
    /// <summary>
    /// 会员注册数据模型
    /// </summary>
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "真实姓名必填")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "手机号码必填")]
        public string UserPhone { get; set; }
        [Required(ErrorMessage = "身份证号码必填")]
        public string IdNumber { get; set; }
        [Required(ErrorMessage = "推荐人编码必填")]
        public string ReferrerUserCode { get; set; }
        [Required(ErrorMessage = "必须同意注册协议")]
        public bool? AgreeRegisterProtocal { get; set; }
    }
}