using EFCore.BulkExtensions;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository.BaseRepository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDBContext _appDBContext;
        public BaseRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public bool Insert(T entity)
        {
            _appDBContext.Add(entity);
            return _appDBContext.SaveChanges() > 0;
        }
        public bool Delete(T entity)
        {
            _appDBContext.Remove(entity);
            return _appDBContext.SaveChanges()>0;
        }

        public bool Edit(T entity)
        {
            _appDBContext.Update(entity);
            return _appDBContext.SaveChanges()>0;
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


        public IQueryable<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames)
        {
            DbSet<T> list;
            if (tableNames == null && tableNames.Any() == false)
            {
                throw new Exception("缺少表的名称");
            }
            else
            {
                list = _appDBContext.Set<T>();
                foreach(var table in tableNames)
                {
                    list.Include(table);
                }
                return list.Where(predicate);
            }
        }

        public List<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public void InsertList(List<T> list)
        {
            _appDBContext.BulkInsert(list);
        }

        public void DelList(List<T> list)
        {
            _appDBContext.BulkDelete(list);
        }

        public void UpdatetList(List<T> list)
        {
            _appDBContext.BulkUpdate(list);
        }

        public bool ExecuteSqlCommand(string CommandName, SqlParameter[] sqlParameters)
        {
            return _appDBContext.Database.ExecuteSqlCommand(CommandName, sqlParameters) > 0;
        }

        public T FromSql(string sql)
        {
            throw new NotImplementedException();
        }
    }
}
