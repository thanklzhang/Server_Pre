using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSCombatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //CombatManager SSServer = new CombatManager();
            //SSServer.Start();
            SSServer combatServer = new SSServer();
            combatServer.Start();

            while (true)
            {
                Thread.Sleep(1);
            }

            //string welcome = "Welcome to my test server!";
            //data = Encoding.ASCII.GetBytes(welcome);
            //newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
            //while (true)
            //{
            //    data = new byte[1024];
            //    length = newsock.ReceiveFrom(data, ref Remote);
            //    Console.WriteLine(Encoding.ASCII.GetString(data, 0, length));
            //    newsock.SendTo(data, length, SocketFlags.None, Remote);
            //}
        }
    }
}
