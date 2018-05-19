using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zc.Commons;
using zc.Models;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using zc.Managers;

namespace zc_unit_test
{
    [TestClass]
    public class UnitTest1
    {
        ZCDbContext db = new ZCDbContext();
        [TestMethod]
        public void TestMethod1()
        {
            var pwdHash = Utility.MD5Encrypt("admin");
            Console.WriteLine(pwdHash);
        }

        [TestMethod]
        public void TestMethod2()
        {

            var list = db.menus.SqlQuery(@"WITH temp
                    AS
                    (
                    --父项
                    SELECT * FROM menu WHERE menu_id = 1
                    UNION ALL
                    --递归结果集中的下级
                    SELECT m.* FROM menu AS m
                    INNER JOIN temp AS child ON m.menu_parent_id = child.menu_id
                    )
                    SELECT * FROM temp").ToList();
            list.ForEach(a => { Console.WriteLine(a.menu_name); });
        }

        [TestMethod]
        public void TestMethod3()
        {
            OperatorManager operatorManager = new OperatorManager();
            var oper = operatorManager.GetWithPermissions("admin");
            oper.sysroles.ToList().ForEach(a => Console.WriteLine(a.role_name));
        }

        [TestMethod]
        public void TestMethod4()
        {
            var oper = db.operators.Include("sysroles.menus")
                .Where(a => a.oper_name == "admin").FirstOrDefault();
            db.Dispose();
            foreach (var item in oper.sysroles)
            {
                Console.WriteLine(item.role_name);
            }
        }

        [TestMethod]
        public void TestMethod5()
        {
            string userName = null;
            string userPhone = null;

            var query = from u in db.users
                        where
                            //u.user_name.Contains(userName) // 用户姓名模糊搜索
                             //&& u.user_phone.Contains(userPhone) // 用户电话模糊搜索
                             //&& 
                             u.user_status == UserStatus.NOT_ACTIVATED // 状态为未激活
                        select u;
            var total = query.Count();
            Console.WriteLine(total);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var q = db.bonus_record.Include("user").Include("user1").ToList();
            Console.WriteLine(q);
        }
    }
}
