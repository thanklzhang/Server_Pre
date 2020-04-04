using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
namespace TerminalClient
{
    public class ClientConnect
    {
        public static ClientConnect Instance;

        NetSessionMgr sessionMgr = new TMNSessionMgr();
        public void Startup()
        {
            Instance = this;
            //Startup(2346);

            //客户端连接此服务器
            //sessionMgr.CreateListen(SessionType.Server_GS, ConstInfo.GS_Client_Port);

            //连接登录服务器
            ConnectLSServer();

            ////连接 GS 服务器
            //sessionMgr.CreateConnect(SessionType.Client_M_OnlyGS, ConstInfo.GS_IP, ConstInfo.GS_Client_Port, (isSuccess) =>
            //{
            //    Console.WriteLine("connect : " + SessionType.Client_M_OnlyGS + " " + isSuccess);
            //});
            while (true)
            {

            }
        }

        public void SendTo(SessionType type, int infoId, int nsId, int msgId, byte[] data)
        {
            sessionMgr.SendTo(type, infoId, nsId, msgId, data);
        }

        public void ConnectLSServer()
        {
            //连接 LS 服务器
            sessionMgr.CreateConnect(SessionType.Client_TMN_OnlyLS, ConstInfo.LS_IP, ConstInfo.LS_Client_Port, (isSuccess) =>
            {
                Console.WriteLine("connect : " + SessionType.Client_TMN_OnlyLS.ToString() + " " + isSuccess);
                GC2LS.AskLogin login = new GC2LS.AskLogin();
                login.Account = "test0";
                login.Password = "123";
                SendTo(SessionType.Client_TMN_OnlyLS, 0, 0, (int)GC2LS.MsgId.Gc2LsAskLogin, login.ToByteArray());
            });
        }
        public void ConnectGSServer(Action action = null)
        {
            //连接 GS 服务器
            sessionMgr.CreateConnect(SessionType.Client_TMN_OnlyGS, ConstInfo.GS_IP, ConstInfo.GS_Client_Port, (isSuccess) =>
            {
                sessionMgr.RemoveNetSeesion(SessionType.Client_GS_OnlyLS);

                Console.WriteLine("connect : " + SessionType.Client_TMN_OnlyGS + " " + isSuccess);
                
                action?.Invoke();
               

            });
        }
    }



}
