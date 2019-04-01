using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Repository.BaseRepository
{
    public interface IBaseRepository<T> where T:class
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>受影响的行数</returns>
        bool Insert(T entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>受影响的行数</returns>
        bool Delete(T entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>受影响的行数</returns>
        bool Edit(T entity);

        /// <summary>
        /// lambda表达式查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>实体类list</returns>
        List<T> GetListWhere(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 多表联查
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="tableNames"></param>
        /// <returns>实体类list</returns>
        List<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页最大条数</param>
        /// <returns></returns>
        List<T> GetListByPageWhere(Expression<Func<T, bool>> predicate,int page,int limit);

        /// <summary>
        /// 多表联查分页方法
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="tableNames">表名</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns>实体类list</returns>
        List<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames,int page,int limit);
    }
}
