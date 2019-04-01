using Entity.Models;
using Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Services.BaseServices
{
    public class BaseServices<T> : IBaseServices<T> where T : class
    {
        private IBaseServices<T> _baseServices;
        public BaseServices(IBaseServices<T> baseServices)
        {
            _baseServices = baseServices;
        }

        public Dictionary<string, object> add(T entity)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (_baseServices.Insert(entity))
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
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> edit(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Edit(T entity)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> getlist(Expression<Func<T, bool>> predicate)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                List<T> list = _baseServices.GetListWhere(predicate);
                if (list.Count() > 0)
                {
                    r["code"] = 2000;
                    r["items"] = list;
                    r["message"] = "查询成功";
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功,但没有数据";
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public List<T> GetListByPageWhere(Expression<Func<T, bool>> predicate, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public List<T> GetListWhere(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> quertyjoin(Expression<Func<T, bool>> predicate, string[] tableNames)
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
