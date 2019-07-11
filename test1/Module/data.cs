using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test1.Module
{
    public class data
    {

        public User User { get; set; }
        public Prod Prod { get; set; }
        public Date Date { get; set; }
        public Money Money { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public string color { get; set; }
    }

    public class Prod
    {
        public string name { get; set; }
        public string color { get; set; }
    }

    public class Date
    {
        public string time { get; set; }
        public string color { get; set; }
    }

    public class Money
    {
        public string value { get; set; }
        public string color { get; set; }
    }

}
