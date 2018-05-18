using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zc.Models;

namespace zc.Managers
{
    public class OperatorManager
    {
        private ZCDbContext db = new ZCDbContext();

        public _operator Get(string operName)
        {
            return db.operators.Where(a => a.oper_name == operName).FirstOrDefault();
        }

        public _operator Get(int operId)
        {
            return db.operators.Find(operId);
        }

        public _operator GetWithPermissions(string operName)
        {
            return db.operators.Include("sysroles.menus").Where(a => a.oper_name == operName).FirstOrDefault();
        }

        public _operator GetWithPermissions(int operId)
        {
            return db.operators.Include("sysroles.menus").Where(a => a.oper_id == operId).FirstOrDefault();
        }

        public bool Login(string operName, string operPassword)
        {
            operPassword = zc.Commons.Utility.MD5Encrypt(operPassword);
            var query = from oper in db.operators
                        where oper.oper_name == operName
                        && oper.oper_password == operPassword
                        select oper;
            return query.FirstOrDefault() != null;
        }
    }
}