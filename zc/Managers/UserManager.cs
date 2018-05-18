using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using zc.Commons;
using zc.Models;
using zc.Models.ViewModels;

namespace zc.Managers
{
    public class UserManager
    {
        private ZCDbContext db = new ZCDbContext();

        public user ActiveUser(UserActiveModel model, int operId)
        {

            using (TransactionScope tx = new TransactionScope())
            {
                var user = db.users.Find(model.user_id);
                user.reg_money = model.reg_money;
                user.province = model.province;
                user.city = model.city;
                user.area = model.area;
                user.address = model.address;
                user.activate_id = operId;
                user.level_id = (from lev in db.levels where lev.level_money == user.reg_money select lev).FirstOrDefault().level_id;
                user.user_status = UserStatus.NORMAL;
                user.activate_time = DateTime.Now;

                db.SaveChanges();

                var dist_money = user.level.level_money1;
                var bonus = new user_bonus
                {
                    user = user,
                    dist_money = dist_money,
                    dist_balance = dist_money,
                    dist_number = 22
                };

                if (user.referrer != null)
                {
                    int referrer_money1 = Convert.ToInt32(user.referrer.level.recom_rate1 * user.reg_money);
                    bonus.referrer_id1 = user.referrer_id;
                    bonus.referrer_money1 = bonus.referrer_balance1 = referrer_money1;
                    bonus.referrer_number1 = 22;
                    if (user.referrer.referrer != null)
                    {
                        int referrer_money2 = Convert.ToInt32(user.referrer.referrer.level.recom_rate2 * user.reg_money);
                        bonus.referrer_id2 = user.referrer.referrer.user_id;
                        bonus.referrer_money2 = bonus.referrer_balance2 = referrer_money2;
                        bonus.referrer_number1 = 22;
                    }
                }

                db.user_bonus.Add(bonus);

                db.SaveChanges();

                tx.Complete();

                return user;
            }
        }

        public bool Login(string phone, string pwd, string validateCode)
        {
            throw new NotImplementedException();
        }

        public bool Register(UserRegisterModel model)
        {
            // 创建持久对象
            user newUser = new user();
            // 复制有效值到持久对象
            newUser.user_name = model.UserName;
            newUser.user_phone = model.UserPhone;
            newUser.id_number = model.IdNumber;
            newUser.referrer_id = model.ReferrerId;
            // 新注册会员状态为未激活
            newUser.user_status = UserStatus.NOT_ACTIVATED;
            // 初始等级
            newUser.level_id = 0;
            // 生成用户编号(user_code)
            newUser.user_code = Utility.GenerateUserCode();
            // 生成默认登录密码
            newUser.login_password = Utility.GenerateLoginPwd(newUser);
            // 生成默认二级密码
            newUser.second_password = Utility.GenereateSecondPwd(newUser);
            // 加密
            newUser.login_password = Utility.MD5Encrypt(newUser.login_password);
            newUser.second_password = Utility.MD5Encrypt(newUser.second_password);
            // 注册时间
            newUser.register_time = DateTime.Now;
            // 注册金额
            newUser.reg_money = 0;
            // 持久化
            db.users.Add(newUser);
            db.SaveChanges();
            return true;
        }

        public List<user> SearchNotActivedUsers(string userName, string userPhone, int pageNo, int pageSize)
        {
            var query = (from u in db.users
                         where
                             u.user_name.Contains(userName) // 用户姓名模糊搜索
                             && u.user_phone.Contains(userPhone) // 用户电话模糊搜索
                             && u.user_status == UserStatus.NOT_ACTIVATED // 状态为未激活
                         orderby u.user_id ascending
                         select u)
                         .Skip((pageNo - 1) * pageSize)
                         .Take(pageSize);
            return query.ToList();
        }

        public int SearchTotalOfNotActivatedUsers(string userName, string userPhone)
        {
            var query = from u in db.users
                        where
                            u.user_name.Contains(userName) // 用户姓名模糊搜索
                             && u.user_phone.Contains(userPhone) // 用户电话模糊搜索
                             && u.user_status == UserStatus.NOT_ACTIVATED // 状态为未激活
                        select u;
            return query.Count();
        }
    }
}