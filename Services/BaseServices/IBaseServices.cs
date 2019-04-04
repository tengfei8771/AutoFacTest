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
        Dictionary<string, object> getList(Expression<Func<T, bool>> predicate);
        Dictionary<string, object> quertyJoin(Expression<Func<T, bool>> predicate, string[] tableNames);
        Dictionary<string, object> getListByPage(Expression<Func<T, bool>> predicate, int page, int limit);

        Dictionary<string, object> quertyJoinByPage(Expression<Func<T, bool>> predicate, string[] tableNames,int page,int limit);
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list">实体list</param>
        /// <returns></returns>
        Dictionary<string,object> InsertList(List<T> list);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list">实体list</param>
        /// <returns></returns>
        Dictionary<string, object> DelList(List<T> list);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns>操作是否成功</returns>
        Dictionary<string, object> UpdatetList(List<T> list);
    }
}
