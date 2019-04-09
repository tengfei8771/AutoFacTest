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

        public dynamic GetUserAndPet(out int toatal)
        {
            var list = (from User in _appDBContext.User
                        join pet in _appDBContext.PetInfo on User.Id equals pet.OwnerId into temp
                        from t in temp.DefaultIfEmpty()
                        where User.Id != "9999999"
                        select new
                        {
                            User.Id,
                            User.PassWord,
                            User.Account,
                            t.PetId,
                            t.PetAge,
                            t.PetName,
                            t.PetSex
                        }).ToList();
            toatal = list.Count();
            return list;
        }
    }
}
