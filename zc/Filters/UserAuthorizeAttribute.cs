using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zc.Commons;
using zc.Models;

namespace zc.Filters
{
    // TODO: 待完善
    /// <summary>
    /// 会员授权过滤器
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsAuthenticated)
            {
                using (ZCDbContext db = new ZCDbContext())
                {
                    user user = null;
                    user = db.users.Find(int.Parse(HttpContext.Current.User.Identity.Name));
                    UserPrincipal upc = new UserPrincipal(user);
                    HttpContext.Current.User = upc;
                    return true;
                }
            }
            return false;
        }
    }
}