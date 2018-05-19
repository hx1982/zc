using System.ComponentModel.DataAnnotations;

namespace zc.Models.ViewModels
{
    /// <summary>
    /// 会员登录数据模型
    /// </summary>
    public class UserLoginModel
    {
        [Required(ErrorMessage = "手机号必填")]
        public string UserPhone { get; set; }
        [Required(ErrorMessage = "密码必填")]
        public string LoginPwd { get; set; }
        public bool? RememberMe { get; set; }
    }
}