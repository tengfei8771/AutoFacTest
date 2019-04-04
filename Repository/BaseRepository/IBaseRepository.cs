using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        /// <returns>是否成功</returns>
        bool Insert(T entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>是否成功</returns>
        bool Delete(T entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>是否成功</returns>
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
        IQueryable<T> QueryJoin(Expression<Func<T, bool>> predicate, string[] tableNames);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页最大条数</param>
        /// <returns></returns>
        List<T> GetListByPageWhere(Expression<Func<T, bool>> predicate,int page,int limit);

        /// <summary>
        /// 多表联查分页方法 (外键关联)
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="tableNames">表名</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns>实体类list</returns>
        IQueryable<T> QueryJoinByPage(Expression<Func<T, bool>> predicate, string[] tableNames,int page,int limit);
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list">实体</param>
        /// <returns>操作是否成功</returns>
        void InsertList(List<T> list);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list">实体</param>
        /// <returns></returns>
        void DelList(List<T> list);


        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns>操作是否成功</returns>
        void UpdatetList(List<T> list);
        /// <summary>
        /// 执行存储过程(可以执行非查询的SQL语句以及存储过程)
        /// </summary>
        /// <param name="CommandName">存储过程名称</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns>执行是否成功</returns>
        bool ExecuteSqlCommand(string CommandName, SqlParameter[] sqlParameters);
        /// <summary>
        /// 传统sql语句执行
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>实体</returns>
        IQueryable<T> FromSql(string sql);
    }
}
