using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserDataMgr<T> : DBEntityMgr<T> where T : DBEntity, new()
{
    public static UserData CreateUser(string account, string password, string token)
    {
        UserData user = new UserData();
        user.account = account;
        user.password = password;
        user.token = token;
        return user;
    }
}

