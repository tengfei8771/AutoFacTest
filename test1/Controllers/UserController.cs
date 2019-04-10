using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Redis;
using Services.IServices;
using Until.TokenHelper;

namespace test1.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IRedisHelper _redisHelper;
        public UserController(IUserServices userServices,IRedisHelper redisHelper)
        {
            _userServices = userServices;
            _redisHelper = redisHelper;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            Dictionary<string, object> r = _userServices.getList(p => true);
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
            string value = "abcdefg";
            bool r1 = _redisHelper.SetValue("mykey", value);
            string saveValue = _redisHelper.GetValue("mykey");
            bool r2 = _redisHelper.SetValue("mykey", "NewValue");
            saveValue = _redisHelper.GetValue("mykey");
            bool r3 = _redisHelper.DelValue("mykey");
            string uncacheValue = _redisHelper.GetValue("mykey");
            return Ok(saveValue);
        }
        [HttpGet("getUserAndPet")]
        public IActionResult getUserAndPet()
        {
            //return Ok(_userServices.getUserAndPet());
            string[] tableNames = new string[]
            {
                "PetInfo"
            };
            return Ok(_userServices.getUserAndPet());
        }
        [HttpGet("Token")]
        public IActionResult createToken()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload["USER_ID"] = "77881sdsad";
            int exp = 30;
            string token = TokenHelper.CreateTookenByHandler(payload, exp);
            return Ok(token);
        }
        [HttpGet("validateToken")]
        public IActionResult validateToken(string token)
        {
            if (TokenHelper.Validate(token))
            {
                return Ok(1);
            }
            else
            {
                return Ok(2);
            }
        }
    }
}