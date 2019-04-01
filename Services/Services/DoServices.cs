
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class DoServices:IDoServices
    {
        public string dosomething()
        {
            return "执行";
        }
    }
}
