using Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Services.BaseServices
{
    public interface IBaseServices<T> :IBaseRepository<T> where T:class
    {
        Dictionary<string,object> add(T entity);
        Dictionary<string,object> del(T entity);
        Dictionary<string, object> edit(T entity);
        Dictionary<string, object> getlist(Expression<Func<T, bool>> predicate);
        Dictionary<string, object> quertyjoin(Expression<Func<T, bool>> predicate, string[] tableNames);
        //Dictionary<string, object> GetListByPageWhere(Expression<Func<T, bool>> predicate, int page, int limit);
    }
}
