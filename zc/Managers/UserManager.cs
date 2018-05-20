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
                // 保存用户激活信息
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

                // 保存分红信息
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

                //todo: 是否需要保存分红记录 ? bonus_record ?

                tx.Complete();
                return user;
            }
        }

        public user Login(string phone, string pwd)
        {
            pwd = Utility.MD5Encrypt(pwd);
            var query = from u in db.users
                        where u.user_phone == phone && u.login_password == pwd 
                        && u.user_status == UserStatus.NORMAL
                        select u;
            return query.FirstOrDefault();
        }

        public bool Register(UserRegisterModel model)
        {
            // 校验user_phone唯一性
            var query = from u in db.users where u.user_phone == model.UserPhone select u;
            if (query.Count() > 0)
            {
                throw new Exception("手机号码\"" + model.UserPhone + "\"在系统中已存在!");
            }
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

        /// <summary>
        /// 获得账户数据
        /// </summary>
        /// <param name="userId">会员id</param>
        /// <returns></returns>
        public user_account GetUserAccount(int userId)
        {
            var query = from ua in db.user_account
                        where ua.user_id == userId
                        select ua;
            return query.FirstOrDefault();
        }

        public user GetUser(int userId)
        {
            return db.users.Find(userId);
        }

        public List<bonus_record> GetBonusRecords(string user_name, string user_phone, DateTime? begin, DateTime? end, int pageNo, int pageSize)
        {
            var query = from b in db.bonus_record select b;
            if (!string.IsNullOrEmpty(user_name))
            {
                query = query.Where(b => b.user.user_name.Contains(user_name));
            }
            if (!string.IsNullOrEmpty(user_phone))
            {
                query = query.Where(b => b.user.user_phone.Contains(user_phone));
            }
            if (begin.HasValue)
            {
                query = query.Where(b => b.create_time >= begin);
            }
            if (end.HasValue)
            {
                query = query.Where(b => b.create_time <= end);
            }
            return query.OrderBy(b => b.bonus_record_id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetBonusRecordsTotal(string user_name, string user_phone, DateTime? begin, DateTime? end)
        {
            var query = from b in db.bonus_record select b;
            if (!string.IsNullOrEmpty(user_name))
            {
                query = query.Where(b => b.user.user_name.Contains(user_name));
            }
            if (!string.IsNullOrEmpty(user_phone))
            {
                query = query.Where(b => b.user.user_phone.Contains(user_phone));
            }
            if (begin.HasValue)
            {
                query = query.Where(b => b.create_time >= begin);
            }
            if (end.HasValue)
            {
                query = query.Where(b => b.create_time <= end);
            }
            return query.Count();
        }

        //提现请求
        public List<cash_record> GetCashRequests(string user_name,string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end, int pageNo, int pageSize)
        {
            var query = from b in db.cash_record select b;
            if (!string.IsNullOrEmpty(user_name))
            {
                query = query.Where(b => b.user.user_name.Contains(user_name));
            }
            if (!string.IsNullOrEmpty(user_phone))
            {
                query = query.Where(b => b.user.user_phone.Contains(user_phone));
            }
            if (cash_type != null)
            {
                query = query.Where(b=>b.cash_type == cash_type);
            }
            if (cash_status != null) {
                query = query.Where(b => b.cash_status == cash_status);
            }
            if (begin.HasValue)
            {
                query = query.Where(b => b.cash_time1 >= begin);
            }
            if (end.HasValue)
            {
                query = query.Where(b => b.cash_time1 <= end);
            }
            return query.OrderBy(b => b.cash_record_id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetCashRequestsTotal(string user_name,string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end)
        {
            var query = from b in db.cash_record select b;
            if (!string.IsNullOrEmpty(user_name))
            {
                query = query.Where(b => b.user.user_name.Contains(user_name));
            }
            if (!string.IsNullOrEmpty(user_phone))
            {
                query = query.Where(b => b.user.user_phone.Contains(user_phone));
            }
            if (cash_type != null)
            {
                query = query.Where(b => b.cash_type == cash_type);
            }
            if (cash_status != null)
            {
                query = query.Where(b => b.cash_status == cash_status);
            }
            if (begin.HasValue)
            {
                query = query.Where(b => b.cash_time1 >= begin);
            }
            if (end.HasValue)
            {
                query = query.Where(b => b.cash_time1 <= end);
            }
            return query.Count();
        }
    }
}