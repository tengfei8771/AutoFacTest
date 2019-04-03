using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Redis;
using Services.IServices;

namespace test1.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            Dictionary<string, object> r = _userServices.getlist(p => true);
            return Ok(r);
        }

        [HttpPost("create")]
        public IActionResult create()
        {
            List<User> users = new List<User>();
            for(int i = 0; i < 100000; i++)
            {
                User model = new User();
                model.Id = Guid.NewGuid().ToString();
                model.Account = "tesxst";
                model.PassWord = "213213";
                users.Add(model);
            }           
            return Json(_userServices.InsertList(users));
        }
        [HttpGet("redis")]
        public IActionResult redis()
        {
            RedisHelper redisHelper = new RedisHelper();
            string value = "abcdefg";
            bool r1 = redisHelper.SetValue("mykey", value);
            string saveValue = redisHelper.GetValue("mykey");
            bool r2 = redisHelper.SetValue("mykey", "NewValue");
            saveValue = redisHelper.GetValue("mykey");
            bool r3 = redisHelper.DelValue("mykey");
            string uncacheValue = redisHelper.GetValue("mykey");
            return Ok(saveValue);
        }
        [HttpGet("getUserAndPet")]
        public IActionResult getUserAndPet()
        {
            return Ok(_userServices.getUserAndPet());
        }
    }
}