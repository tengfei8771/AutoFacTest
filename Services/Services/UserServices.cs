using Entity.Models;
using Repository.BaseRepository;
using Repository.IRepository;
using Services.BaseServices;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Dictionary<string, object> getUserAndPet()
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                List<User> users = _userRepository.GetUserAndPet();
                if (users.Count() > 0)
                {
                    res["code"] = 2000;
                    res["items"] = users;
                    res["message"] = "成功";
                }
                else
                {
                    res["code"] = 2001;
                    res["message"] = "成功,但没有数据";
                }
            }
            catch(Exception e)
            {
                res["message"] = e.Message;
                res["code"] = -1;
            }
            return res;
        }
    }
}
