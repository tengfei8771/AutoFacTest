using Entity.Models;
using System;
using System.Collections.Generic;
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

        public T GetById(string id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetListByPageWhere(Expression<Func<T, bool>> predicate, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public List<T> GetListWhere(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public List<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames)
        {
            throw new NotImplementedException();
        }

        public List<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames, int page, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
