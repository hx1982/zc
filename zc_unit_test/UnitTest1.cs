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
using System.Text;
using System.Threading;
using System.Data.SqlClient;

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
            //Console.WriteLine(pwdHash);
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
            UserManager um = new UserManager();
            var users = um.GetAllUsers("", "", "", null, "", "", "", "", null, null, null, null, null, 1, 10);
            var data = users.Select(u => new
            {
                user_id = u.user_id,
                user_code = u.user_code,
                user_name = u.user_name,
                user_phone = u.user_phone,
                id_number = u.id_number,
                level_name = u.level.level_name,
                province = u.province,
                city = u.city,
                area = u.area,
                address = u.address,
                reg_money = u.reg_money,
                referrer_name = u.referrer == null ? "无" : u.referrer.user_name,
                user_status = u.user_status,
                register_time = u.register_time,
                activate_time = u.activate_time
            });
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod]
        public void TestMethod7()
        {
            var user = new user
            {
                id_number = "00000000000000000X",
                user_phone = "13666660005"
            };
            var uc = Utility.GenerateUserCode(user);
            Console.WriteLine(uc);
        }


        [TestMethod]
        public void TestMethod8()
        {
            using (ZCDbContext db = new ZCDbContext())
            {
                var c = db.Database.SqlQuery<int>(
                    "select count(0) from [user] where user_code='00000005'");
                Console.WriteLine(c.Single());
            }
        }

        [TestMethod]
        public void testUserTree()
        {
            var m = new UserManager();
            var tree = m.GetReferTree("", "");
            foreach (var item in tree)
            {
                Console.WriteLine(item.user_name);
            }
        }

    }
}
