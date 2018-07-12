using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zc.Models;
using System.Data.Entity;
using System.Linq;

namespace zc_unit_test
{
    [TestClass]
    public class UnitTest1
    {
        private ZCDbContext db = new ZCDbContext();

        [TestMethod]
        public void TestMethod1()
        {
            var userId = 1;

            var query = from ua in db.user_account
                        where ua.user_id == userId
                        select ua;
            user_account userAccount = query.FirstOrDefault();
            Console.WriteLine(userAccount.account1_balance);
        }
    }
}
