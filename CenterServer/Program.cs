using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CenterServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DBMgr.Init();

            UserData user = new UserData()
            {
                id = 1,
                account = "zhang0",
                password = "425",

            };

            var user2 = new UserData()
            {
                id = 21,
                account = "zhang0",
                password = "425",

            };

            DBMgr.userDataMgr.Save(user, true);
            DBMgr.userDataMgr.Save(user2, true);

            user.password = "update0";
            user.account = "update0";
            
            DBMgr.userDataMgr.NotifyChangeFields(user, new string[] { "password", "account" });
            Console.Read();
            
            return;
            DBMgr.Init();

            CSServer centerServer = new CSServer();
            centerServer.Start();

            while (true)
            {
                Thread.Sleep(1);
            }

        }
    }

}
