using Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Services.BaseServices
{
    public interface IBaseServices<T>  where T:class
    {
        Dictionary<string,object> add(T entity);
        Dictionary<string,object> del(T entity);
        Dictionary<string, object> edit(T entity);
        Dictionary<string, object> getlist(Expression<Func<T, bool>> predicate);
        Dictionary<string, object> quertyjoin(Expression<Func<T, bool>> predicate, string[] tableNames);
        //Dictionary<string, object> GetListByPageWhere(Expression<Func<T, bool>> predicate, int page, int limit);

        Dictionary<string,object> InsertList(List<T> list);

        Dictionary<string, object> DelList(List<T> list);


        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns>操作是否成功</returns>
        Dictionary<string, object> UpdatetList(List<T> list);
    }
}
