using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository.Repository
{
    public class UserRepository:BaseRepository<User>,IUserRepository
    {
        private readonly AppDBContext _appDBContext;
        public UserRepository(AppDBContext appDBContext) : base(appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public List<User> GetUserAndPet(Expression<Func<User, bool>> predicate)
        {
            return _appDBContext.Set<User>().Where(predicate).Include(p => p.Id).ToList();
        }
    }
}
