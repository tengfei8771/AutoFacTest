using Entity.Models;
using Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Services.BaseServices
{
    public abstract class BaseServices<T> : IBaseServices<T> where T : class
    {
        private readonly IBaseRepository<T> _baseRepository;
        public BaseServices(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public Dictionary<string, object> add(T entity)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (_baseRepository.Insert(entity))
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = "失败";
                    r["code"] = -1;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> del(T entity)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (_baseRepository.Delete(entity))
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = "失败";
                    r["code"] = -1;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }


        public Dictionary<string, object> DelList(List<T> list)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _baseRepository.DelList(list);
                sw.Stop();
                r["message"] = "成功,共耗时" + sw.ElapsedMilliseconds + "毫秒";
                r["code"] = 2000;
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> edit(T entity)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (_baseRepository.Edit(entity))
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = "失败";
                    r["code"] = -1;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }


        public Dictionary<string, object> getlist(Expression<Func<T, bool>> predicate)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                List<T> list = _baseRepository.GetListWhere(predicate);
                if (list.Count() > 0)
                {
                    r["code"] = 2000;
                    r["items"] = list;
                    r["message"] = "查询成功";
                    r["total"] = list.Count();
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功,但没有数据";
                    r["total"] = 0;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> getlistbypage(Expression<Func<T, bool>> predicate, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                List<T> list = _baseRepository.GetListByPageWhere(predicate,page,limit);
                if (list.Count() > 0)
                {
                    r["code"] = 2000;
                    r["items"] = list;
                    r["message"] = "查询成功";
                    r["total"] = list.Count();
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功,但没有数据";
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> InsertList(List<T> list)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _baseRepository.InsertList(list);
                sw.Stop();
                r["message"] = "成功,共耗时" + sw.ElapsedMilliseconds + "毫秒";
                r["code"] = 2000;
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> quertyjoin(Expression<Func<T, bool>> predicate, string[] tableNames)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                IQueryable<T> list = _baseRepository.QueryJoin(predicate, tableNames);
                if (list.Count() > 0)
                {
                    r["message"] = "成功";
                    r["items"] = list;
                    r["total"] = list.Count();
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = "成功，但没有数据";
                    r["total"] = 0;
                    r["code"] = 2000;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> quertyjoinbypage(Expression<Func<T, bool>> predicate, string[] tableNames, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                IQueryable<T> list = _baseRepository.QueryJoinByPage(predicate, tableNames,page,limit);
                if (list.Count() > 0)
                {
                    r["message"] = "成功";
                    r["items"] = list;
                    r["total"] = list.Count();
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = "成功，但没有数据";
                    r["total"] = 0;
                    r["code"] = 2000;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> UpdatetList(List<T> list)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _baseRepository.UpdatetList(list);
                sw.Stop();
                r["message"] = "成功,共耗时" + sw.ElapsedMilliseconds + "毫秒";
                r["code"] = 2000;
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
    }
}
