using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserDataMgr<T> : DBEntityMgr<T> where T : DBEntity, new()
{
    public static UserData CreateUserData(string account, string password, string token)
    {
        UserData userData = new UserData();
        userData.id = GetNewId();
        userData.account = account;
        userData.password = password;
        userData.token = token;
        return userData;
    }
}

