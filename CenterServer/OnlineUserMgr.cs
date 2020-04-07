using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OnlineUserMgr
{
    Dictionary<string, OnlineUser> accountUserDic = new Dictionary<string, OnlineUser>();
    //Dictionary<int, User> idUserDic = new Dictionary<int, User>();
    //Dictionary<int, string> idTokenDic = new Dictionary<int, string>();

    public void UserOnline(OnlineUser u)
    {
        accountUserDic.Add(u.account, u);
        // idUserDic.Add(u.id, u);
    }

    public void UserExit(string account)
    {
        if (accountUserDic.ContainsKey(account))
        {
            accountUserDic.Remove(account);
        }

    }

    public OnlineUser GetUser(string account)
    {
        if (accountUserDic.ContainsKey(account))
        {
            return accountUserDic[account];
        }
        return null;
    }

    //public void UserExit(int id)
    //{
    //    if (idUserDic.ContainsKey(id))
    //    {
    //        idUserDic.Remove(id);
    //    }

    //}
}

