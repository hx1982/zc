using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Commons;
using zc.Managers;
using zc.Models;

namespace zc.Areas.Backend.Controllers
{
    public class ModifyPwdController : Controller
    {
        private OperatorManager om = new OperatorManager();

        public ActionResult Index(string oldPwd, string newPwd, string newPwdRepeat)
        {
            if (newPwdRepeat != newPwd)
            {
                return Json(new AjaxResultObject() { code = AjaxResultObject.ERROR, message = "新密码和确认新密码不一致！" });
            }
            int operId = ((Session[SessionConstants.CURRENTOPERATOR] as _operator).oper_id);
            bool success = om.ModifyPwd(operId, oldPwd, newPwd);
            if (success)
            {
                return Json(new AjaxResultObject() { code = AjaxResultObject.OK });
            }
            return Json(new AjaxResultObject() { code = AjaxResultObject.ERROR, message = "原密码不正确！" });
        }
    }
}