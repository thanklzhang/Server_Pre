using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
public class DBHeartBeat
{
    bool isFinish;
    //public UserDataMgr<UserData> userDataMgr;
    public void Start(Action action, int time = 60)
    {
        isFinish = false;
        while (!isFinish)
        {
            Thread.Sleep(time * 1000);
            action?.Invoke();
        }

    }

}

