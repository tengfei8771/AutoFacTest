using System;
using System.Collections.Generic;
using System.Text;

namespace Redis
{
    public interface IRedisHelper
    {
        bool SetValue(string Key, string Value);
        bool DelValue(string Key);
        string GetValue(string Key);
    }
}
