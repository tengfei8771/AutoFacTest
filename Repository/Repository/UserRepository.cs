using Entity.Models;
using Repository.BaseRepository;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repository
{
    public class UserRepository:BaseRepository<User>,IUserRepository
    {
        public UserRepository(AppDBContext appDBContext) : base(appDBContext)
        {

        }
    }
}
