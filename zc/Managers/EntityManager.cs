using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using zc.Models;

namespace zc.Managers
{

    /// <summary>
    /// <para>通用 C U R D 类</para>
    /// <para>为便于灵活处理事务, 本类中所有方法都没有执行SaveChanges</para>
    /// <para>执行CUD时, 务必调用SaveChanges()</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityManager
    {
        protected ZCDbContext db = new ZCDbContext();

        #region INSERT

        /// <summary>
        /// 新增 实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Insert<T>(T model) where T : class
        {
            db.Set<T>().Add(model);
        }

        /// <summary>
        /// 普通批量插入
        /// </summary>
        /// <param name="datas"></param>
        public void InsertRange<T>(List<T> datas) where T : class
        {
            db.Set<T>().AddRange(datas);
        }

        #endregion INSERT

        #region DELETE

        /// <summary>
        /// 根据模型删除
        /// </summary>
        /// <param name="model">包含要删除id的对象</param>
        /// <returns></returns>
        public void Delete<T>(T model) where T : class
        {
            db.Set<T>().Attach(model);
            db.Set<T>().Remove(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="whereLambda"></param>
        public void Delete<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            var toDelete = db.Set<T>().Where(whereLambda);
            foreach (T item in toDelete)
            {
                db.Entry<T>(item).State = EntityState.Deleted;
            }
        }

        #endregion DELETE

        #region UPDATE

        /// <summary>
        /// 单个对象指定列修改
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="proNames">要修改的 属性 名称</param>
        /// <param name="isProUpdate"></param>
        /// <returns></returns>
        public void Update<T>(T model, List<string> proNames, bool isProUpdate = true) where T : class
        {
            //将 对象 添加到 EF中
            db.Set<T>().Attach(model);
            var setEntry = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntry(model);
            //指定列修改
            if (isProUpdate)
            {
                foreach (string proName in proNames)
                {
                    setEntry.SetModifiedProperty(proName);
                }
            }
            //忽略类修改
            else
            {
                Type t = typeof(T);
                List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                foreach (var item in proInfos)
                {
                    string proName = item.Name;
                    if (proNames.Contains(proName))
                    {
                        continue;
                    }
                    setEntry.SetModifiedProperty(proName);
                }
            }
        }

        /// <summary>
        /// 单个对象修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Update<T>(T model) where T : class
        {
            var entry = db.Entry<T>(model);
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public void UpdateAll<T>(List<T> models) where T : class
        {
            foreach (var model in models)
            {
                DbEntityEntry entry = db.Entry(model);
                entry.State = EntityState.Modified;
            }
        }

        /// <summary>
        /// 批量统一修改
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="modifiedProNames"></param>
        /// <returns></returns>
        public void Update<T>(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames)
             where T : class
        {
            //查询要修改的数据
            List<T> listModifing = db.Set<T>().Where(whereLambda).ToList();
            Type t = typeof(T);
            List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            Dictionary<string, PropertyInfo> dictPros = new Dictionary<string, PropertyInfo>();
            proInfos.ForEach(p =>
            {
                if (modifiedProNames.Contains(p.Name))
                {
                    dictPros.Add(p.Name, p);
                }
            });
            if (dictPros.Count <= 0)
            {
                throw new Exception("指定修改的字段名称有误或为空");
            }
            foreach (var item in dictPros)
            {
                PropertyInfo proInfo = item.Value;

                //取出 要修改的值
                object newValue = proInfo.GetValue(model, null);

                //批量设置 要修改 对象的 属性
                foreach (T oModel in listModifing)
                {
                    //为 要修改的对象 的 要修改的属性 设置新的值
                    proInfo.SetValue(oModel, newValue, null);
                }
            }

        }

        #endregion UPDATE

        #region SELECT

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T FindById<T>(dynamic id) where T : class
        {
            return db.Set<T>().Find(id);
        }

        /// <summary>
        /// 获取默认一条数据，没有则为NULL
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T FirstOrDefault<T>(Expression<Func<T, bool>> whereLambda = null) where T : class
        {
            if (whereLambda == null)
            {
                return db.Set<T>().FirstOrDefault();
            }
            return db.Set<T>().FirstOrDefault(whereLambda);
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll<T, TProperty>(Expression<Func<T, TProperty>> ordering = null) where T : class
        {
            return ordering == null
                ? db.Set<T>().ToList()
                : db.Set<T>().OrderBy(ordering).ToList();
        }

        /// <summary>
        /// 带条件查询获取数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="ordering"></param>
        /// <returns></returns>
        public List<T> GetAll<T, TProperty>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TProperty>> ordering = null) where T : class
        {
            var iQueryable = db.Set<T>().Where(whereLambda);
            return ordering == null
                ? iQueryable.ToList()
                : iQueryable.OrderBy(ordering).ToList();
        }

        /// <summary>
        /// 带条件查询获取数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetAllIQueryable<T>(Expression<Func<T, bool>> whereLambda = null)
            where T : class
        {
            return whereLambda == null ? db.Set<T>() : db.Set<T>().Where(whereLambda);
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="whereLambd"></param>
        /// <returns></returns>
        public int GetCount<T>(Expression<Func<T, bool>> whereLambd = null)
            where T : class
        {
            return whereLambd == null ? db.Set<T>().Count() : db.Set<T>().Where(whereLambd).Count();
        }

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="whereLambd"></param>
        /// <returns></returns>
        public bool Any<T>(Expression<Func<T, bool>> whereLambd)
            where T : class
        {
            return db.Set<T>().Where(whereLambd).Any();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rows">总条数</param>
        /// <param name="orderBy">排序条件（一定要有）</param>
        /// <param name="whereLambda">查询条件（可有，可无）</param>
        /// <param name="isOrder">是否是Order排序</param>
        /// <returns></returns>
        public List<T> Page<T, TProperty>(int pageIndex, int pageSize, out int rows, Expression<Func<T, TProperty>> orderBy, Expression<Func<T, bool>> whereLambda = null, bool isOrder = true)
            where T : class
        {
            IQueryable<T> data = isOrder ?
                db.Set<T>().OrderBy(orderBy) :
                db.Set<T>().OrderByDescending(orderBy);

            if (whereLambda != null)
            {
                data = data.Where(whereLambda);
            }
            rows = data.Count();
            return data.Skip(pageSize * pageIndex - pageSize).Take(pageSize).ToList();
        }

        #endregion SELECT

        #region ORTHER

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            if (db.Database.CurrentTransaction == null)
                db.Database.BeginTransaction();
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public void SaveChanges()
        {
            db.SaveChanges();
        }


        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public void Commit()
        {
            if(db.Database.CurrentTransaction != null)
                db.Database.CurrentTransaction.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBack()
        {
            if (db.Database.CurrentTransaction != null)
                db.Database.CurrentTransaction.Rollback();
        }

        #endregion ORTHER

    }

}