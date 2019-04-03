using Entity.Models;
using Services.BaseServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.IServices
{
    public interface IUserServices:IBaseServices<User>
    {
        Dictionary<string, object> getUserAndPet();
    }
}
