using System;

namespace Repository
{
    public class PersonRepository:IPersonRepository
    {
        public string Eat()
        {
            return "吃了";
        }
    }
}
