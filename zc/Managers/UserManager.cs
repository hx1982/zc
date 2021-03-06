﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using zc.Commons;
using zc.Models;
using zc.Models.ViewModels;

namespace zc.Managers
{
    public class UserManager : EntityManager
    {
        public List<level> GetLevelList()
        {
            var query = from b in db.levels select b;
            return query.ToList();
        }

        #region 激活会员相关

        /// <summary>
        /// 激活会员
        /// </summary>
        /// <param name="model"></param>
        /// <param name="operId">操作员id</param>
        /// <returns></returns>
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
                user.activate_type = ActivateType.OPERID;

                db.SaveChanges();
                
                tx.Complete();
                return user;
            }
        }
        /// <summary>
        /// 激活会员
        /// </summary>
        public user ActiveUser(int? auserId,int regMoney, int userId)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                // 保存用户激活信息
                var user = db.users.Find(auserId);
                user.reg_money = regMoney;
                user.activate_id = userId;
                user.level_id = (from lev in db.levels where lev.level_money == user.reg_money select lev).FirstOrDefault().level_id; 
                user.user_status = UserStatus.NORMAL;
                user.activate_time = DateTime.Now;
                user.activate_type = ActivateType.USERID;
                db.SaveChanges();

                //写入流水
                account_record model = new account_record();
                model.user_id = userId;
                model.acc_type = AccountConstants.GOLD;
                model.cons_type = ConType.EXPEND;
                model.acc_record_type = AccRecordType.ACTIVATE;
                model.cons_value = regMoney;
                model.acc_balance = 0;
                model.acc_remark = "激活"+user.user_name+"-"+user.user_phone+"花费";
                model.acc_record_time = DateTime.Now;
                // 持久化
                db.account_record.Add(model);
                db.SaveChanges();
                tx.Complete();
                return user;
            }
        }
        /// <summary>
        /// 将user_remark1用作用户推荐二维码地址的存储
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public user SaveUserCodeForRemark1(user model)
        {
            var user = db.users.Find(model.user_id);
            user.user_remark1 = model.user_remark1;
            db.SaveChanges();

            return user;
        }

        #endregion

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public user Login(string phone, string pwd)
        {
            pwd = Utility.MD5Encrypt(pwd);
            var query = from u in db.users
                        where u.user_phone == phone && u.login_password == pwd
                        && u.user_status == UserStatus.NORMAL
                        select u;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Register(UserRegisterModel model)
        {
            // 校验user_phone唯一性
            var query = from u in db.users where u.user_phone == model.UserPhone select u;
            if (query.Count() > 0)
            {
                throw new Exception("手机号码\"" + model.UserPhone + "\"在系统中已存在!");
            }

            // 验证推荐人是否存在
            var referrer = (from u in db.users
                            where u.user_code == model.ReferrerUserCode
                            select u).FirstOrDefault();
            if (referrer == null)
            {
                throw new Exception("推荐人编码\"" + model.ReferrerUserCode + "\"不存在!");
            }
            // 验证推荐人是否是已激活的正常会员
            if (referrer.user_status != UserStatus.NORMAL)
            {
                throw new Exception("推荐人\"" + model.ReferrerUserCode + "\"不能是未激活或冻结的会员!");
            }

            // 创建持久对象
            user newUser = new user();
            // 复制有效值到持久对象
            newUser.user_name = model.UserName;
            newUser.user_phone = model.UserPhone;
            newUser.id_number = model.IdNumber;
            newUser.referrer_id = referrer.user_id;
            // 新注册会员状态为未激活
            newUser.user_status = UserStatus.NOT_ACTIVATED;
            // 初始等级
            newUser.level_id = 0;
            // 生成用户编号(user_code)
            newUser.user_code = Utility.GenerateUserCode(newUser);
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

        #region 查询会员

        /// <summary>
        /// 根据条件查询所有会员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPhone"></param>
        /// <param name="idNumber"></param>
        /// <param name="levelId"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="area"></param>
        /// <param name="referrerUserName"></param>
        /// <param name="userStatus"></param>
        /// <param name="beginRegDate"></param>
        /// <param name="endRegDate"></param>
        /// <param name="beginActiveDate"></param>
        /// <param name="endActiveDate"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<user> GetAllUsers(string userName, string userPhone, string idNumber, int? levelId, string province, string city, string area, string referrerUserName, int? userStatus, DateTime? beginRegDate, DateTime? endRegDate, DateTime? beginActiveDate, DateTime? endActiveDate, int pageNo, int pageSize)
        {
            var query = db.users.AsQueryable();
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(u => u.user_name.Contains(userName));
            }
            if (!string.IsNullOrEmpty(userPhone))
            {
                query = query.Where(u => u.user_phone.Contains(userPhone));
            }
            if (!string.IsNullOrEmpty(idNumber))
            {
                query = query.Where(u => u.id_number.Contains(idNumber));
            }
            if (levelId.HasValue)
            {
                query = query.Where(u => u.level_id == levelId);
            }
            if (!string.IsNullOrEmpty(province))
            {
                query = query.Where(u => u.province.Contains(province));
            }
            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(u => u.city.Contains(city));
            }
            if (!string.IsNullOrEmpty(area))
            {
                query = query.Where(u => u.area.Contains(area));
            }
            if (!string.IsNullOrEmpty(province))
            {
                query = query.Where(u => u.province.Contains(province));
            }
            if (!string.IsNullOrEmpty(referrerUserName))
            {
                query = query.Where(u => u.referrer.user_name.Contains(referrerUserName));
            }
            if (userStatus.HasValue)
            {
                query = query.Where(u => u.user_status == userStatus);
            }
            if (beginRegDate.HasValue)
            {
                query = query.Where(u => u.register_time >= beginRegDate);
            }
            if (endRegDate.HasValue)
            {
                query = query.Where(u => u.register_time <= endRegDate);
            }
            if (beginActiveDate.HasValue)
            {
                query = query.Where(u => u.activate_time >= beginActiveDate);
            }
            if (endActiveDate.HasValue)
            {
                query = query.Where(u => u.activate_time <= endActiveDate);
            }
            return query.OrderBy(u => u.user_id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 符合条件的会员总数
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPhone"></param>
        /// <param name="idNumber"></param>
        /// <param name="levelId"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="area"></param>
        /// <param name="referrerUserName"></param>
        /// <param name="userStatus"></param>
        /// <param name="beginRegDate"></param>
        /// <param name="endRegDate"></param>
        /// <param name="beginActiveDate"></param>
        /// <param name="endActiveDate"></param>
        /// <returns></returns>
        public int GetAllUsersTotal(string userName, string userPhone, string idNumber, int? levelId, string province, string city, string area, string referrerUserName, int? userStatus, DateTime? beginRegDate, DateTime? endRegDate, DateTime? beginActiveDate, DateTime? endActiveDate)
        {
            var query = db.users.AsQueryable();
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(u => u.user_name.Contains(userName));
            }
            if (!string.IsNullOrEmpty(userPhone))
            {
                query = query.Where(u => u.user_phone.Contains(userPhone));
            }
            if (!string.IsNullOrEmpty(idNumber))
            {
                query = query.Where(u => u.id_number.Contains(idNumber));
            }
            if (levelId.HasValue)
            {
                query = query.Where(u => u.level_id == levelId);
            }
            if (!string.IsNullOrEmpty(province))
            {
                query = query.Where(u => u.province.Contains(province));
            }
            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(u => u.city.Contains(city));
            }
            if (!string.IsNullOrEmpty(area))
            {
                query = query.Where(u => u.area.Contains(area));
            }
            if (!string.IsNullOrEmpty(province))
            {
                query = query.Where(u => u.province.Contains(province));
            }
            if (!string.IsNullOrEmpty(referrerUserName))
            {
                query = query.Where(u => u.referrer.user_name.Contains(referrerUserName));
            }
            if (userStatus.HasValue)
            {
                query = query.Where(u => u.user_status == userStatus);
            }
            if (beginRegDate.HasValue)
            {
                query = query.Where(u => u.register_time >= beginRegDate);
            }
            if (endRegDate.HasValue)
            {
                query = query.Where(u => u.register_time <= endRegDate);
            }
            if (beginActiveDate.HasValue)
            {
                query = query.Where(u => u.activate_time >= beginActiveDate);
            }
            if (endActiveDate.HasValue)
            {
                query = query.Where(u => u.activate_time <= endActiveDate);
            }
            return query.Count();
        }

        /// <summary>
        /// 未激活会员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPhone"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 未激活会员总数
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPhone"></param>
        /// <returns></returns>
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

        ///<summary>
        /// 会员查询未激活会员
        /// </summary>
        public List<user> SearchNotActivedUsers(string userPhone, int? userId, int pageNo, int pageSize)
        {
            var query = db.users.AsQueryable();
            if (userId!=null)
            {
                query = query.Where(u => u.ref_url.Contains(userId+"-"));
            }
            if (!string.IsNullOrEmpty(userPhone))
            {
                query = query.Where(u => u.user_phone.Contains(userPhone));
            }
                query = query.Where(u => u.user_status == UserStatus.NOT_ACTIVATED);
            
            return query.OrderBy(u => u.user_id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            
        }

        /// <summary>
        /// 会员查询未激活会员总数
        /// </summary>
        public int SearchTotalOfNotActivatedUsers(string userPhone,int? userId)
        {
            var query = db.users.AsQueryable();
            if (userId != null)
            {
                query = query.Where(u => u.ref_url.Contains(userId + "-"));
            }
            if (!string.IsNullOrEmpty(userPhone))
            {
                query = query.Where(u => u.user_phone.Contains(userPhone));
            }
            query = query.Where(u => u.user_status == UserStatus.NOT_ACTIVATED);

            return query.Count();
        }



        #endregion

        /// <summary>
        /// 查会员账户--根据userID查询
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

        /// <summary>
        /// 修改会员账户
        /// </summary>
        /// <returns></returns>
        public bool UpdateUserAccount(user_account model)
        {
            db.SaveChanges();
            return true;
        }

        public user GetUser(int userId)
        {
            return db.users.Find(userId);
        }

        #region 分红记录

        #region 分红记录新

        /// <summary>
        /// 分红记录
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="user_phone"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<account_record> GetBonusRecords(int? acc_type,string user_name, string user_phone, DateTime? begin, DateTime? end, int pageNo, int pageSize)
        {
            var query = from b in db.account_record select b;
            // 只查已支付的分红
            // todo: 现已没有user_bonus实体
            query = query.Where(b => true);

            if(acc_type != null)
            {
                query = query.Where(b => b.acc_type == acc_type);
            }
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
                query = query.Where(b => b.acc_record_time >= begin);
            }
            if (end.HasValue)
            {
                query = query.Where(b => b.acc_record_time <= end);
            }
            query = query.Where(b => b.acc_record_type == 1);
            return query.OrderBy(b => b.user_id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 分红记录总数
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="user_phone"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetBonusRecordsTotal(int? acc_type, string user_name, string user_phone, DateTime? begin, DateTime? end)
        {
            var query = from b in db.account_record select b;
            // 只查已支付的分红
            // todo: 现已没有user_bonus实体
            query = query.Where(b => true);
            if (acc_type != null)
            {
                query = query.Where(b => b.acc_type == acc_type);
            }
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
                query = query.Where(b => b.acc_record_time >= begin);
            }
            if (end.HasValue)
            {
                query = query.Where(b => b.acc_record_time <= end);
            }
            query = query.Where(b => b.acc_record_type == 1);
            return query.Count();
        }
        #endregion


        #region 分红记录老

        ///// <summary>
        ///// 分红记录
        ///// </summary>
        ///// <param name="user_name"></param>
        ///// <param name="user_phone"></param>
        ///// <param name="begin"></param>
        ///// <param name="end"></param>
        ///// <param name="pageNo"></param>
        ///// <param name="pageSize"></param>
        ///// <returns></returns>
        //public List<bonus_record> GetBonusRecords(string user_name, string user_phone, DateTime? begin, DateTime? end, int pageNo, int pageSize)
        //{
        //    var query = from b in db.bonus_record select b;
        //    // 只查已支付的分红
        //    // todo: 现已没有user_bonus实体
        //    query = query.Where(b => true);

        //    if (!string.IsNullOrEmpty(user_name))
        //    {
        //        query = query.Where(b => b.user.user_name.Contains(user_name));
        //    }
        //    if (!string.IsNullOrEmpty(user_phone))
        //    {
        //        query = query.Where(b => b.user.user_phone.Contains(user_phone));
        //    }
        //    if (begin.HasValue)
        //    {
        //        query = query.Where(b => b.create_time >= begin);
        //    }
        //    if (end.HasValue)
        //    {
        //        query = query.Where(b => b.create_time <= end);
        //    }
        //    return query.OrderBy(b => b.bonus_record_id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        //}

        ///// <summary>
        ///// 分红记录总数
        ///// </summary>
        ///// <param name="user_name"></param>
        ///// <param name="user_phone"></param>
        ///// <param name="begin"></param>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //public int GetBonusRecordsTotal(string user_name, string user_phone, DateTime? begin, DateTime? end)
        //{
        //    var query = from b in db.bonus_record select b;
        //    // 只查已支付的分红
        //    // todo: 现已没有user_bonus实体
        //    query = query.Where(b => true);

        //    if (!string.IsNullOrEmpty(user_name))
        //    {
        //        query = query.Where(b => b.user.user_name.Contains(user_name));
        //    }
        //    if (!string.IsNullOrEmpty(user_phone))
        //    {
        //        query = query.Where(b => b.user.user_phone.Contains(user_phone));
        //    }
        //    if (begin.HasValue)
        //    {
        //        query = query.Where(b => b.create_time >= begin);
        //    }
        //    if (end.HasValue)
        //    {
        //        query = query.Where(b => b.create_time <= end);
        //    }
        //    return query.Count();
        //}

        #endregion

        #endregion

        #region 提现请求相关

        /// <summary>
        /// 插入提现请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertCashRecord(cash_record model)
        {
            cash_record newCashRecord = new cash_record();
            newCashRecord.user_id = model.user_id;
            newCashRecord.cash_type = model.cash_type;
            newCashRecord.cash_money = model.cash_money;
            newCashRecord.cash_status = CashStatus.AUDIT_WAITING;
            newCashRecord.cash_time1 = DateTime.Now;
            // 持久化
            db.cash_record.Add(newCashRecord);

            db.SaveChanges();
            return true;
        }

        #region 审核提现操作相关

        /// <summary>
        /// 查提现申请详情
        /// </summary>
        /// <param name="cash_record_id">提现申请ID</param>
        /// <returns></returns>
        public cash_record GetCashRecord(int cash_record_id)
        {
            var query = from ua in db.cash_record
                        where ua.cash_record_id == cash_record_id
                        select ua;
            return query.FirstOrDefault();
        }
        /// <summary>
        /// 修改提现申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCashRecord(cash_record model)
        {
            //int[] array = new int[] { 1, 2 };
            //query = query.Where(b => array.Contains(b.cash_status));

            db.SaveChanges();
            return true;
        }
        /// <summary>
        /// 批量修改未发放的状态
        /// </summary>
        /// <param name="recordIds"></param>
        /// <returns></returns>
        public bool BachUpdateCashRecord(int[] recordIds, int operId)
        {
            var customers = db.cash_record.Where(c => recordIds.Contains(c.cash_record_id));
            foreach (var customer in customers)
            {
                customer.cash_status = CashStatus.GIVEMONEY_OK;
                customer.cash_time3 = DateTime.Now;
                customer.oper_id2 = operId;
            }
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 通过提现申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public cash_record AuidCashRecord(cash_record model)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                //修改审核状态
                model.cash_status = CashStatus.GIVEMONEY_WAITING;
                model.cash_time2 = DateTime.Now;
                db.SaveChanges();

                //查询用户的账户资金
                user_account userModel = this.GetUserAccount(model.user_id);

                int accRecordType = AccRecordType.WITHDRAWAL;
                int accBalance = 0;

                if (model.cash_type == CashType.GOLD_DIAMOND)
                {
                    accBalance = userModel.account1_balance - model.cash_money;
                }
                //if (model.cash_type == CashType.SILVER_DIAMOND)
                //{
                //    accBalance = userModel.account2_balance - model.cash_money;
                //}
                if (model.cash_type == CashType.SILVER_DIAMOND || model.cash_type == CashType.BLUE_DIAMOND)
                {
                    accBalance = userModel.account3_balance - model.cash_money;
                }

                //插入资金变动记录
                account_record accModel = new account_record();
                accModel.user_id = model.user_id;
                accModel.acc_type = model.cash_type==1? AccountConstants.GOLD:AccountConstants.BLUE;
                accModel.cons_type = ConType.EXPEND;
                accModel.acc_record_type = accRecordType;
                accModel.acc_balance = accBalance;
                accModel.cons_value = model.cash_money;
                accModel.oper_id = model.oper_id1;
                accModel.acc_record_time = DateTime.Now;
                db.account_record.Add(accModel);

                if (model.cash_type == CashType.GOLD_DIAMOND)
                {
                    int shou_xu_fei = Convert.ToInt32(model.cash_money * CashRate.SHOU_XU_FEI);
                    accModel = new account_record();
                    accModel.user_id = model.user_id;
                    accModel.acc_type = model.cash_type;
                    accModel.cons_type = ConType.EXPEND;
                    accModel.acc_record_type = AccRecordType.POUNDAGE;
                    accModel.acc_balance = accBalance - shou_xu_fei;
                    accModel.cons_value = shou_xu_fei;
                    accModel.oper_id = model.oper_id1;
                    accModel.acc_record_time = DateTime.Now;
                    db.account_record.Add(accModel);
                }
                //如果银钻并且手续费不未0的情况
                if(model.cash_type != CashType.GOLD_DIAMOND && CashRate.SIVLVER_SHOU_XU_FEI != 0)
                {
                    int shou_xu_fei = Convert.ToInt32(model.cash_money * CashRate.SIVLVER_SHOU_XU_FEI);
                    accModel = new account_record();
                    accModel.user_id = model.user_id;
                    accModel.acc_type = AccountConstants.BLUE;
                    accModel.cons_type = ConType.EXPEND;
                    accModel.acc_record_type = AccRecordType.POUNDAGE;
                    accModel.acc_balance = accBalance - shou_xu_fei;
                    accModel.cons_value = shou_xu_fei;
                    accModel.oper_id = model.oper_id1;
                    accModel.acc_record_time = DateTime.Now;
                    db.account_record.Add(accModel);
                }
                db.SaveChanges();
                
                tx.Complete();
                return model;
            }
        }


        #endregion

        /// <summary>
        /// 总计提现成功了的总数
        /// </summary>
        /// <param name="acc_type"></param>
        /// <returns></returns>
        public int GetCashMoneyTotal(int? cash_type, int? user_id, int? cash_status)
        {
            var query = from b in db.cash_record select b;
            if (cash_type != null)
            {
                if (cash_type == 1)
                {
                    query = query.Where(b => b.cash_type == cash_type);
                }else
                {
                    int[] array = new int[] { 2, 3 };
                    query = query.Where(b => array.Contains(b.cash_type));
                }
            }
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
            if (cash_status != null)
            {
                if (cash_status == -1 || cash_status == 0)
                {
                    query = query.Where(b => b.cash_status == cash_status);
                }
                else
                {
                    int[] array = new int[] { 1, 2 };
                    query = query.Where(b => array.Contains(b.cash_status));
                }
            }
            return query.Select(b => b.cash_money).DefaultIfEmpty(0).Sum();
        }

        //提现请求列表
        public List<cash_record> GetCashRequests(int? user_id, string user_name, string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end, int pageNo, int pageSize)
        {
            var query = from b in db.cash_record select b;
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
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
                if (cash_type == 1)
                {
                    query = query.Where(b => b.cash_type == cash_type);
                }
                else
                {
                    int[] array = new int[] { 2, 3 };
                    query = query.Where(b => array.Contains(b.cash_type));
                }
            }
            if (cash_status != null)
            {
                if (cash_status == 5)
                {
                    int[] array = new int[] { 1, 2 };
                    query = query.Where(b => array.Contains(b.cash_status));
                }
                else
                {
                    query = query.Where(b => b.cash_status == cash_status);
                }
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
        //提现请求分页计数
        public int GetCashRequestsTotal(int? user_id, string user_name, string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end)
        {
            var query = from b in db.cash_record select b;
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
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
                if (cash_type == 1)
                {
                    query = query.Where(b => b.cash_type == cash_type);
                }
                else
                {
                    int[] array = new int[] { 2, 3 };
                    query = query.Where(b => array.Contains(b.cash_type));
                }
            }
            if (cash_status != null)
            {
                if (cash_status == 5)
                {
                    int[] array = new int[] { 1, 2 };
                    query = query.Where(b => array.Contains(b.cash_status));
                }
                else
                {
                    query = query.Where(b => b.cash_status == cash_status);
                }
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

        #region 代币的提现请求


        //提现请求列表
        public List<cash_record> BlueGetCashRequests(int? user_id, string user_code, string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end, int pageNo, int pageSize)
        {
            var query = from b in db.cash_record select b;
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
            if (!string.IsNullOrEmpty(user_code))
            {
                query = query.Where(b => b.user.user_code.Contains(user_code));
            }
            if (!string.IsNullOrEmpty(user_phone))
            {
                query = query.Where(b => b.user.user_phone.Contains(user_phone));
            }
            if (cash_type != null)
            {
                if (cash_type == 1)
                {
                    query = query.Where(b => b.cash_type == cash_type);
                }
                else
                {
                    int[] array = new int[] { 2, 3 };
                    query = query.Where(b => array.Contains(b.cash_type));
                }
            }
            if (cash_status != null)
            {
                if (cash_status == 5)
                {
                    int[] array = new int[] { 1, 2 };
                    query = query.Where(b => array.Contains(b.cash_status));
                }
                else
                {
                    query = query.Where(b => b.cash_status == cash_status);
                }
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
        //提现请求分页计数
        public int BlueGetCashRequestsTotal(int? user_id, string user_code, string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end)
        {
            var query = from b in db.cash_record select b;
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
            if (!string.IsNullOrEmpty(user_code))
            {
                query = query.Where(b => b.user.user_code.Contains(user_code));
            }
            if (!string.IsNullOrEmpty(user_phone))
            {
                query = query.Where(b => b.user.user_phone.Contains(user_phone));
            }
            if (cash_type != null)
            {
                if (cash_type == 1)
                {
                    query = query.Where(b => b.cash_type == cash_type);
                }
                else
                {
                    int[] array = new int[] { 2, 3 };
                    query = query.Where(b => array.Contains(b.cash_type));
                }
            }
            if (cash_status != null)
            {
                if (cash_status == 5)
                {
                    int[] array = new int[] { 1, 2 };
                    query = query.Where(b => array.Contains(b.cash_status));
                }
                else
                {
                    query = query.Where(b => b.cash_status == cash_status);
                }
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


        #endregion


        #region 为了区分开金钻蓝钻提现，单独的方法


        //提现请求列表
        public List<cash_record> GetCashRequests1(int? user_id, string user_name, string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end, int pageNo, int pageSize)
        {
            var query = from b in db.cash_record select b;
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
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
            else
            {
                query = query.Where(b => b.cash_type == CashType.GOLD_DIAMOND || b.cash_type == CashType.SILVER_DIAMOND);
            }
            if (cash_status != null)
            {
                if (cash_status == 5)
                {
                    int[] array = new int[] { 1, 2 };
                    query = query.Where(b => array.Contains(b.cash_status));
                }
                else
                {
                    query = query.Where(b => b.cash_status == cash_status);
                }
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
        //提现请求分页计数
        public int GetCashRequestsTotal1(int? user_id, string user_name, string user_phone, int? cash_type, int? cash_status, DateTime? begin, DateTime? end)
        {
            var query = from b in db.cash_record select b;
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
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
            else
            {
                query = query.Where(b => b.cash_type == CashType.GOLD_DIAMOND || b.cash_type == CashType.SILVER_DIAMOND);
            }
            if (cash_status != null)
            {
                if (cash_status == 5)
                {
                    int[] array = new int[] { 1, 2 };
                    query = query.Where(b => array.Contains(b.cash_status));
                }
                else
                {
                    query = query.Where(b => b.cash_status == cash_status);
                }
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


        #endregion

        #endregion

        #region 金钻，银钻，蓝钻 收入，支出总数

        /// <summary>
        /// 收入
        /// </summary>
        /// <param name="acc_type"></param>
        /// <returns></returns>
        public int GetTotalIncome(int? acc_type, int? user_id)
        {
            var query = from b in db.account_record select b;
            if (acc_type != null)
            {
                query = query.Where(b => b.acc_type == acc_type);
            }
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
            query = query.Where(b => b.cons_type == ConType.INCOME);

            return query.Select(b => b.cons_value).DefaultIfEmpty(0).Sum();
        }
        /// <summary>
        /// 支出
        /// </summary>
        /// <param name="acc_type"></param>
        /// <returns></returns>
        public int GetTotalExpend(int? acc_type, int? user_id)
        {
            var query = from b in db.account_record select b;
            if (acc_type != null)
            {
                query = query.Where(b => b.acc_type == acc_type);
            }
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
            query = query.Where(b => b.cons_type == ConType.EXPEND);

            return query.Select(b => b.cons_value).DefaultIfEmpty(0).Sum();
        }

        /// <summary>
        /// 账户变动记录
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="acc_type">账户类型</param>
        /// <param name="acc_record_type">记录类型</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<account_record> GetAccountRecordList(int? user_id, int? acc_type, int? acc_record_type, DateTime? begin, DateTime? end, int pageNo, int pageSize)
        {
            var query = from b in db.account_record select b;
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
            if (acc_type != null)
            {
                query = query.Where(b => b.acc_type == acc_type);
            }
            if (acc_record_type != null)
            {
                query = query.Where(b => b.acc_record_type == acc_record_type);
            }
            if (begin.HasValue)
            {
                query = query.Where(b => b.acc_record_time >= begin);
            }
            if (end.HasValue)
            {
                query = query.Where(b => b.acc_record_time <= end);
            }
            return query.OrderBy(b => b.acc_record_id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }


        /// <summary>
        /// 账户变动记录
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="acc_type">账户类型</param>
        /// <param name="acc_record_type">记录类型</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetAccountRecordTotal(int? user_id, int? acc_type, int? acc_record_type, DateTime? begin, DateTime? end)
        {
            var query = from b in db.account_record select b;
            if (user_id != null)
            {
                query = query.Where(b => b.user_id == user_id);
            }
            if (acc_type != null)
            {
                query = query.Where(b => b.acc_type == acc_type);
            }
            if (acc_record_type != null)
            {
                query = query.Where(b => b.acc_record_type == acc_record_type);
            }
            if (begin.HasValue)
            {
                query = query.Where(b => b.acc_record_time >= begin);
            }
            if (end.HasValue)
            {
                query = query.Where(b => b.acc_record_time <= end);
            }
            return query.Count();
        }

        #endregion

        #region 修改/完善个人信息/修改密码

        public void UpdateUserPersonalInfo(int user_id, string province, string city, string area, string address, string account_num, string bank_name, string wallet_adder)
        {
            var user = db.users.Find(user_id);
            user.province = province;
            user.city = city;
            user.area = area;
            user.address = address;
            user.account_num = account_num;
            user.bank_name = bank_name;
            user.wallet_adder = wallet_adder;
            db.SaveChanges();
        }

        public void UpdateLoginPwd(int userId, string loginPwd)
        {
            var user = db.users.Find(userId);
            user.login_password = Utility.MD5Encrypt(loginPwd);
            db.SaveChanges();
        }

        public void UpdateSecondPwd(int userId, string secondPwd)
        {
            var user = db.users.Find(userId);
            user.second_password = Utility.MD5Encrypt(secondPwd);
            db.SaveChanges();
        }

        #endregion

        #region 分红信息的查询 user_bouns

        public user_bonus GetUserBonus(int userId)
        {
            //todo: 现已没有user_bonus实体, 得从bouns_record查出来

            return null;
        }

        /// <summary>
        /// 以当前登录用户ID作为推荐人1或者推荐人2，获取的推荐金额总和
        /// </summary>
        /// <param name="referrer_id1"></param>
        /// <returns></returns>
        public int GetReferrerMoney1Total(int? referrer_id1)
        {
            //todo: 现已没有user_bonus实体
            return 0;
        }
        /// <summary>
        /// 以当前登录用户ID作为推荐人1或者推荐人2，获取的推荐金额总和
        /// </summary>
        /// <param name="referrer_id1"></param>
        /// <returns></returns>
        public int GetReferrerMoney2Total(int? referrer_id2)
        {
            //todo: 现已没有user_bonus实体
            return 0;
        }

        /// <summary>
        /// 以当前登录用户ID作为推荐人1或者推荐人2，获取的推荐余额总和
        /// </summary>
        /// <param name="referrer_id1"></param>
        /// <returns></returns>
        public int GetReferrerBalance1Total(int? referrer_id1)
        {
            //var query = from b in db.user_bonus select b;
            //if (referrer_id1 != null)
            //{
            //    query = query.Where(b => b.referrer_id1 == referrer_id1);
            //}
            //return query.Select(b => b.referrer_balance1).DefaultIfEmpty(0).Sum();

            // todo: 现已没有user_bonus实体
            return 0;
        }
        /// <summary>
        /// 以当前登录用户ID作为推荐人1或者推荐人2，获取的推荐余额总和
        /// </summary>
        /// <param name="referrer_id1"></param>
        /// <returns></returns>
        public int GetReferrerBalance2Total(int? referrer_id2)
        {
            //todo: 现已没有user_bonus实体
            return 0;
        }

        #endregion

        #region 等级修改相关

        /// <summary>
        /// 根据会员名和会员电话模糊分页查询正常用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPhone"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<user> SearchNormalUsersByNameAndPhone(string userName, string userPhone, int page, int rows)
        {
            var query = from u in db.users
                        where u.user_name.Contains(userName)
                        && u.user_phone.Contains(userPhone)
                        && u.user_status == UserStatus.NORMAL
                        orderby u.user_id ascending
                        select u;
            return query.Skip(rows * page - rows).Take(rows).ToList();
        }

        public int SearchNormalUsersByNameAndPhoneTotal(string userName, string userPhone)
        {
            var query = from u in db.users
                        where u.user_name.Contains(userName)
                        && u.user_phone.Contains(userPhone)
                        && u.user_status == UserStatus.NORMAL
                        select u;
            return query.Count();
        }


        public void ChangeLevel(int[] array_user_id, int new_level)
        {
            if (array_user_id == null || array_user_id.Length == 0)
            {
                return;
            }
            var sql = "update [user] set level_id = @new_level where user_id in (" + string.Join(",", array_user_id) + ")";
            db.Database.ExecuteSqlCommand(sql, new SqlParameter("@new_level", new_level));
        }

        #endregion

        #region 账户流水

        public List<account_record> AccountLines(string userName, string userPhone, int accType, int page, int rows)
        {
            var query = from a in db.account_record
                        where a.user.user_name.Contains(userName)
                        && a.user.user_phone.Contains(userPhone)
                        && a.acc_type == accType
                        orderby a.acc_record_id ascending
                        select a;
            return query.Skip(page * rows - rows).Take(rows).ToList();
        }

        public int AccountLinesTotal(string userName, string userPhone, int accType)
        {
            var query = from a in db.account_record
                        where a.user.user_name.Contains(userName)
                        && a.user.user_phone.Contains(userPhone)
                        && a.acc_type == accType
                        select a;
            return query.Count();
        }

        /// <summary>
        /// 插入账户流水
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertAccountRecord(account_record model)
        {
            model.acc_record_time = DateTime.Now;
            // 持久化
            db.account_record.Add(model);

            db.SaveChanges();
            return true;
        }


        #endregion

        #region 推荐树

        public List<user> GetReferTree(string userName, string userPhone)
        {
            var query1 = db.users.Where(a => a.user_name.Contains(userName) && a.user_phone.Contains(userPhone));
            var query2 = query1.SelectMany(item => db.users.Where(a => a.referrer_id == item.user_id));
            var query3 = query2.SelectMany(item => db.users.Where(a => a.referrer_id == item.user_id));
            return query1.Concat(query2).Concat(query3).Distinct().ToList();
            //var query = db.users.Where(a => a.user_name.Contains(userName) && a.user_phone.Contains(userPhone))
            //    .Select(item => new
            //    {
            //        self = item,
            //        childs = db.users.Where(a => a.referrer_id == item.user_id).Select(item2 => new
            //        {
            //            self = item2,
            //            childs = db.users.Where(a => a.referrer_id == item2.user_id).Select(item3 => new
            //            {
            //                self = item3
            //            })
            //        })
            //    });
            //return query.ToList();
        }

        public List<ReferTreeNode> GetReferTreeChilds(int userId)
        {
            return db.users.Where(a => a.referrer_id == userId).Select(a => new ReferTreeNode
            {
                id = a.user_id,
                pId = a.referrer_id,
                name = a.user_name,
                isParent = db.users.Count(item => item.referrer_id == a.user_id) > 0
            }).ToList();
        }

        #endregion

        #region 会员账户信息

        /// <summary>
        /// 根据条件查询所有会员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPhone"></param>
        /// <param name="idNumber"></param>
        /// <param name="levelId"></param>
        /// <param name="referrerUserName"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<user_account> GetAllUserAccount(bool? isActive,string userName, string userPhone, string idNumber, int? levelId, string referrerUserName,int? userStatus, int pageNo, int pageSize)
        {
            var query = db.user_account.AsQueryable();
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(u => u.user.user_name.Contains(userName));
            }
            if(isActive != null)
            {
                query = query.Where(u => u.user.is_activate== isActive);
            }
            if (!string.IsNullOrEmpty(userPhone))
            {
                query = query.Where(u => u.user.user_phone.Contains(userPhone));
            }
            if (!string.IsNullOrEmpty(idNumber))
            {
                query = query.Where(u => u.user.id_number.Contains(idNumber));
            }
            if (levelId.HasValue)
            {
                query = query.Where(u => u.user.level_id == levelId);
            }
            if (!string.IsNullOrEmpty(referrerUserName))
            {
                query = query.Where(u => u.user.referrer.user_name.Contains(referrerUserName));
            }
            if (userStatus.HasValue)
            {
                query = query.Where(u => u.user.user_status == userStatus);
            }
            return query.OrderBy(u => u.user_id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 符合条件的会员总数
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPhone"></param>
        /// <param name="idNumber"></param>
        /// <param name="levelId"></param>
        /// <param name="referrerUserName"></param>
        /// <returns></returns>
        public int GetAllUserAccountTotal(bool? isActive, string userName, string userPhone, string idNumber, int? levelId, string referrerUserName,int? userStatus)
        {
            var query = db.user_account.AsQueryable();
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(u => u.user.user_name.Contains(userName));
            }
            if (isActive != null)
            {
                query = query.Where(u => u.user.is_activate == isActive);
            }
            if (!string.IsNullOrEmpty(userPhone))
            {
                query = query.Where(u => u.user.user_phone.Contains(userPhone));
            }
            if (!string.IsNullOrEmpty(idNumber))
            {
                query = query.Where(u => u.user.id_number.Contains(idNumber));
            }
            if (levelId.HasValue)
            {
                query = query.Where(u => u.user.level_id == levelId);
            }
            if (!string.IsNullOrEmpty(referrerUserName))
            {
                query = query.Where(u => u.user.referrer.user_name.Contains(referrerUserName));
            }
            if (userStatus.HasValue)
            {
                query = query.Where(u => u.user.user_status == userStatus);
            }
            return query.Count();
        }

        #endregion

        #region 后台修改用户钱包地址

        public void FillWalletAddressByIdentityNum(string identityNum, string walletAddress)
        {
            var user = db.users.Where(a => a.id_number == identityNum).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("身份证号码不正确");
            }
            user.wallet_adder = walletAddress;
            db.SaveChanges();
        }

        #endregion

        /// <summary>
        /// 批量修改会员是否有激活权限
        /// </summary>
        public bool BachUpdateUserActivate(int[] userIds)
        {
            var users = db.users.Where(c => userIds.Contains(c.user_id));
            foreach (var customer in users)
            {
                customer.is_activate = true;
            }
            db.SaveChanges();
            return true;
        }
    }
}