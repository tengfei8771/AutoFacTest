using Entity.Models;
using Repository.BaseRepository;
using Repository.IRepository;
using Services.BaseServices;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class UserServices: BaseServices<User>,IUserServices
    {
        private readonly IUserRepository _userRepository;
        public  UserServices(IUserRepository userRepository):base(userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
