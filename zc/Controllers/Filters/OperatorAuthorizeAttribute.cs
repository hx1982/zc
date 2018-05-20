using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Commons;
using zc.Managers;
using zc.Models;

namespace zc.Filters
{
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
                         where path.StartsWith(menu.menu_url) //这里假定子菜单的url一定是以父菜单的url开头
                         select menu).Count();
            bool hasPermission = query > 0;
            return hasPermission;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }
}