using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserData : DBEntity
{
    [PrimaryKey]
    public int id;

    public string account;

    public string password;

    public string token;

    //[IgnoreSerialize]
    //public int sessionId;

    //internal void SetSessionId(int gcNetId)
    //{
    //    sessionId = gcNetId;
    //}
}

