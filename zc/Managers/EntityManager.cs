using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using zc.Models;

namespace zc.Managers
{
    /// <summary>
    /// 通用CRUD类
    /// </summary>
    public class EntityManager
    {
        private ZCDbContext db = new ZCDbContext();

        public void Save<E>(E entity) where E : class
        {
            db.Set<E>().Add(entity);
            db.SaveChanges();
        }

        public void Remove<E>(E entity) where E : class
        {
            db.Entry<E>(entity).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
        } 

        public void Modify<E>(E entity) where E : class
        {
            db.Entry<E>(entity).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public E Find<E>(object id) where E : class
        {
            return db.Set<E>().Find(id);
        }

        public IEnumerable<E> FindAll<E>() where E : class
        {
            return db.Set<E>().ToList();
        }

        public IEnumerable<E> Pagination<E>(
            out int totalRows
            , Expression<Func<E, bool>> where = null
            , Expression<Func<E, object>> orderBy = null
            , int pageNo = 1
            , int pageSize = 10
            , bool desc = false) where E : class
        {
            IQueryable<E> query = db.Set<E>();
            if (where != null)
            {
                query = query.Where(where);
            }
            if (orderBy != null)
            {
                if (desc)
                {
                    query = query.OrderByDescending(orderBy);
                }
                else
                {
                    query = query.OrderBy(orderBy);
                }
            }
            query = query.Skip(pageSize * pageNo - pageSize).Take(pageSize);

            totalRows = where != null ? db.Set<E>().Where(where).Count() : db.Set<E>().Count();

            return query;
        }
    }
}