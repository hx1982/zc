using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using zc.Commons;
using zc.Managers;
using zc.Models;

namespace zc.Filters
{
    // TODO: 待完善
    /// <summary>
    /// 操作员授权过滤器
    /// </summary>
    public class OperatorAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var oper = httpContext.Session[SessionConstants.CURRENTOPERATOR] as _operator;
            if (oper == null)
            {
                return false;
            }
            string path = HttpContext.Current.Request.Path;
            var query = (from role in oper.sysroles
                         from menu in role.menus
                         where menu.menu_url != null && path.StartsWith(menu.menu_url)
                         select menu).Count();
            bool hasPermission = query > 0;
            if (!hasPermission)
            {
                HttpContext.Current.Response.StatusCode = 403;
            }
            return hasPermission;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                filterContext.Result = new ViewResult() { ViewName = "~/Areas/Backend/Views/Shared/Forbidden.cshtml" };
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }
}