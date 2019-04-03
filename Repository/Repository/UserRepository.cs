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

        public List<User> GetUserAndPet()
        {
            var list = _appDBContext.User.Where(p => p.Id == "10086").Join(_appDBContext.PetInfo, user => user.Id, pet => pet.OwnerId, (user, pet) => new User
            {
                Account = user.Account,
                Id = user.Id,
                PassWord = user.PassWord,
                PetId = pet.PetName,
            }).ToList();
            return list;
        }
    }
}
