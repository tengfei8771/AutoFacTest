using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.BaseRepository
{
    public interface IBaseRepository<T> where T:class
    {
        int Insert(T entity);
        int Delete(T entity);
        int edit(T entity);
    }
}
