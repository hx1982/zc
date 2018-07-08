using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zc.Commons;
using zc.Models;

namespace zc.Managers
{
    public class OperatorManager : EntityManager
    {
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

        /// <summary>
        /// 修改操作员密码
        /// </summary>
        /// <param name="operId"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public bool ModifyPwd(int operId, string oldPwd, string newPwd)
        {
            oldPwd = Utility.MD5Encrypt(oldPwd);
            newPwd = Utility.MD5Encrypt(newPwd);
            var op = FindById<_operator>(operId);
            if (op == null)
            {
                return false;
            }
            if (op.oper_password != oldPwd)
            {
                return false;
            }
            op.oper_password = newPwd;
            Update<_operator>(op);
            SaveChanges();
            return true;
        }
    }
}