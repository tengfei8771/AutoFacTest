using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
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
    }
}