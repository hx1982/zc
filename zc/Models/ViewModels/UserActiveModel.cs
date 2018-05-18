using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zc.Models.ViewModels
{
    /// <summary>
    /// 会员激活数据模型
    /// </summary>
    public class UserActiveModel
    {
        [Display(Name = "注册金额")]
        [Required(ErrorMessage = "注册金额必填")]
        [RegMoneyValidaton(ErrorMessage = "注册金额非法")]
        public int reg_money { get; set; }

        public int user_id { get; set; }

        [Display(Name = "省")]
        [StringLength(50)]
        public string province { get; set; }
        [StringLength(50)]
        [Display(Name = "市")]
        public string city { get; set; }
        [StringLength(50)]
        [Display(Name = "区")]
        public string area { get; set; }
        [StringLength(200)]
        [Display(Name = "详细地址")]
        public string address { get; set; }
    }

    /// <summary>
    /// 注册金额验证器
    /// </summary>
    public class RegMoneyValidatonAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int regMoney = Convert.ToInt32(value);
            int[] range = { 18000, 32000, 76000 };
            var query = from item in range where item == regMoney select item;
            return query.Count() == 1;
        }
    }
    
}