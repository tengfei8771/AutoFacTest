using Entity.Models;
using Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Repository.IRepository
{
    public interface IUserRepository:IBaseRepository<User>
    {
        List<User> GetUserAndPet();
    }
}
