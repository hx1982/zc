using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using zc.Commons;
using zc.Models;

namespace zc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var init = new DevModeDatabaseInitializer();
            //Database.SetInitializer<ZCDbContext>(init);
            //init.InitializeDatabase(new ZCDbContext());
        }
    }

    public class DevModeDatabaseInitializer : DropCreateDatabaseAlways<ZCDbContext>
    {
        protected override void Seed(ZCDbContext context)
        {
            level cuprumLevel = new level { level_name = "铜卡" };
            level siverLevel = new level { level_name = "银卡" };
            level goldLevel = new level { level_name = "金卡" };
            context.levels.Add(cuprumLevel);
            context.levels.Add(siverLevel);
            context.levels.Add(goldLevel);
            context.SaveChanges();

            sysrole super = new sysrole { role_name = "超级管理员", role_remark = "Super Administrator" };
            _operator oper = new _operator { oper_name = "admin", oper_password = Utility.MD5Encrypt("admin"), oper_code = "oper01", oper_department = "综合管理部", oper_permission = "all", oper_phone = "000" };
            menu menuUser = new menu { menu_name = "会员管理", menu_url = "/Backend/User" };
            context.sysroles.Add(super);
            context.operators.Add(oper);
            context.menus.Add(menuUser);
            context.SaveChanges();

            menu menuSearchNotActivatedUser = new menu { menu_name = "未激活会员", menu_url = "/Backend/User/SearchNotActivedUsers", menu_parent_id = menuUser.menu_id };
            context.menus.Add(menuSearchNotActivatedUser);
            context.SaveChanges();

            super.menus.Add(menuUser);
            super.menus.Add(menuSearchNotActivatedUser);
            super.operators.Add(oper);
        }
    }
}
