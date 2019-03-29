using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private AppDBContext _appDBContext;
        public BaseRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public int Insert(T entity)
        {
            _appDBContext.Add(entity);
            return _appDBContext.SaveChanges();
        }
        public int Delete(T entity)
        {
            _appDBContext.Remove(entity);
            return _appDBContext.SaveChanges();
        }

        public int Edit(T entity)
        {
            _appDBContext.Update(entity);
            return _appDBContext.SaveChanges();
        }


        public List<T> GetListByPageWhere(Expression<Func<T, bool>> predicate, int page, int limit)
        {
            var list= _appDBContext.Set<T>().Where(predicate).ToList();
            int total = list.Count();
            if (total <= limit)
            {
                return list;
            }
            else 
            {
                return list.Skip((page - 1) * limit).Take(limit).ToList();
            }
        }

        public List<T> GetListWhere(Expression<Func<T, bool>> predicate)
        {
            return _appDBContext.Set<T>().Where(predicate).ToList();
        }


        public List<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames)
        {
            List<T> list=new List<T>();
            if (tableNames == null && tableNames.Any() == false)
            {
                throw new Exception("缺少表的名称");
            }
            else
            {
                foreach(string table in tableNames)
                {
                    list = _appDBContext.Set<T>().Where(predicate).Include(table).ToList();
                }
                return list;
            }
        }

        public List<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames, int page, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
