using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoginServer
{
    class Program
    {
        static void Main(string[] args)
        {



            //连接 GateServer
            //NetClient LS2GSConnect = new NetClient();
            //LS2GSConnect.ConnectServer("127.0.0.1", 11000, (isConnect, i) =>
            //{
            //    if (isConnect)
            //    {
            //        Console.WriteLine("connect GateServer success");

            //        //开始启动 LoginServer
            //        NetServer loginServer = new NetServer();
            //        loginServer.Start(10000);
            //    }
            //    else
            //    {
            //        Console.WriteLine("connect GateServer fail");
            //    }
            //});

            //开始启动 LoginServer
            LSServer loginServer = new LSServer();
            loginServer.Start();


            while (true)
            {
                Thread.Sleep(1);
            }
        }
    }


}
