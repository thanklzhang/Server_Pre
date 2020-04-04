using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
public class GSServer
{
    public static GSServer Instance;
    public GSServer()
    {
        Instance = this;
        accountTokenDic = new Dictionary<string, string>();
    }
    //NetSession LSServerConnect;//这个是连接到的服务端
    //NetSession CSServerConnect;
    //NetSession DBServerConnect;



    ////userId 和 clientId (实际上是 userId 和 socket 的对应关系)
    //public Dictionary<int, int> userId2ClientIdDic = new Dictionary<int, int>();

    ////userId 和 token 的
    //public Dictionary<int, string> userId2TokenDic = new Dictionary<int, string>();
    GSSessionMgr gsSessionMgr;

    public void SetSessionAccount(int sessionId, string account)
    {
        gsSessionMgr.SetAccount(sessionId, account);
    }

    //这里之后会变成类
    public Dictionary<string, string> accountTokenDic = new Dictionary<string, string>();//账号和 token 对应表
    //public Dictionary<int, string> idTokenDic = new Dictionary<int, string>();//id 和 token 对应表

    public void AddToken(string account, int id, string token)
    {
        accountTokenDic.Add(account, token);
        //idTokenDic.Add(id, token);
    }

    public void RemoveToken(string account)
    {
        //if (null == accountTokenDic)
        //{
        //    return;
        //}
        if (accountTokenDic.ContainsKey(account))
        {
            accountTokenDic.Remove(account);
        }
    }

    public bool VerityToken(string account, string token)
    {
        if (accountTokenDic.ContainsKey(account))
        {
            return accountTokenDic[account] == token;
        }

        return false;
    }


    public void Start()
    {

        gsSessionMgr = new GSSessionMgr();
        //Startup(2346);

        //客户端连接此服务器
        gsSessionMgr.CreateListen(SessionType.Server_GS, ConstInfo.GS_Client_Port);

        //连接登录服务器
        gsSessionMgr.CreateConnect(SessionType.Client_GS_OnlyLS, ConstInfo.LS_IP, ConstInfo.LS_GS_Port, (isSuccess) =>
        {
            Console.WriteLine("connect : " + SessionType.Client_GS_OnlyLS.ToString() + " " + isSuccess);
        });

        //连接中心服务器
        gsSessionMgr.CreateConnect(SessionType.Client_GS_OnlyCS, ConstInfo.CS_IP, ConstInfo.CS_GS_Port, (isSuccess) =>
        {
            Console.WriteLine("connect : " + SessionType.Client_GS_OnlyCS.ToString() + " " + isSuccess);
        });

        //连接战斗服务器
        gsSessionMgr.CreateConnect(SessionType.Client_GS_OnlySS, ConstInfo.SS_IP, ConstInfo.SS_Tcp_Port, (isSuccess) =>
        {
            Console.WriteLine("connect : " + SessionType.Client_GS_OnlySS.ToString() + " " + isSuccess);
        });

        //ConnectServer();
    }

    internal void TransToLS(int infoId, int nsId, int msgId, byte[] data)
    {
        gsSessionMgr.TransTo(SessionType.Client_GS_OnlyLS, infoId, nsId, msgId, data);
    }

    internal void TransToCS(int infoId, int nsId, int msgId, byte[] data)
    {
        gsSessionMgr.TransTo(SessionType.Client_GS_OnlyCS, infoId, nsId, msgId, data);
    }

    internal void TransToSS(int infoId, int nsId, int msgId, byte[] data)
    {
        gsSessionMgr.TransTo(SessionType.Client_GS_OnlySS, infoId, nsId, msgId, data);
    }

    internal void SendToCS(int infoId, int nsId, int msgId, byte[] data)
    {
        gsSessionMgr.SendTo(SessionType.Client_GS_OnlyCS, infoId, nsId, msgId, data);
    }

    public void SendMsgToClient(int gcNSId, int msgId, byte[] data)
    {
        gsSessionMgr.SendMsgToSession(gcNSId, 0, gcNSId, msgId, data);
    }

    //public void SendToUser(MsgPack pack)
    //{
    //    SendToUser(pack.msgId, pack.userId, pack.data);
    //}

    //public void SendToUser(int userId, ushort msgId, byte[] data)
    //{
    //    if (userId2ClientIdDic.ContainsKey(userId))
    //    {
    //        var clientId = userId2ClientIdDic[userId];
    //        SendToClient(clientId, msgId, data);
    //    }
    //    else
    //    {
    //        Console.WriteLine("the clientId is not exist");
    //    }
    //}

    //public void ConnectServer()
    //{
    //    //connect LS
    //    LSServerConnect = new NetSession();
    //    LSServerConnect.ReceiveMsgAction += ReceiveLSMsg;
    //    LSServerConnect.connectAction += (isSuccess) =>
    //      {
    //          if (isSuccess)
    //          {
    //              Console.WriteLine("success to connect LS");

    //              GS2LS.RegistServer registS = new GS2LS.RegistServer();


    //              LSServerConnect.Send((int)GS2LS.MsgId.Gs2LsRegistServer, registS.ToByteArray());
    //          }
    //          else
    //          {
    //              Console.WriteLine("fail to connect LS");
    //          }
    //      };
    //    LSServerConnect.Connect("127.0.0.1", 2345);

    //    //connect CS
    //    CSServerConnect = new NetSession();
    //    CSServerConnect.ReceiveMsgAction += ReceiveCSMsg;
    //    CSServerConnect.connectAction += (isSuccess) =>
    //    {
    //        if (isSuccess)
    //        {
    //            Console.WriteLine("success to connect CS");

    //            //GS2LS.RegistServer registS = new GS2LS.RegistServer();


    //            //LSServerConnect.Send((int)GS2LS.MsgId.Gs2LsRegistServer, registS.ToByteArray());
    //        }
    //        else
    //        {
    //            Console.WriteLine("fail to connect CS");
    //        }
    //    };
    //    //CSServerConnect.Connect("127.0.0.1", 2347);

    //    //connect DB
    //    //DBServerConnect = new TcpClient();
    //    //DBServerConnect.Connect("", 0);
    //}

    ////接收到 LS 服务端的消息(这里连接 LS)
    //public void ReceiveLSMsg(NetSession client, MsgPack pack)
    //{
    //    var msgId = (LS2GS.MsgId)pack.msgId;

    //    if (msgId > LS2GS.MsgId.Begin && msgId < LS2GS.MsgId.End)//LS2GS
    //    {
    //        if (msgId == LS2GS.MsgId.Ls2GsVerificationAskLogin)//模拟是 LS 需要 GS 验证登录
    //        {
    //            //parse pack after...
    //            var loginData = LS2GS.VerificationAskLogin.Parser.ParseFrom(pack.data);
    //            Console.WriteLine("receve LS msg : verificationLogin");
    //            //vertify account

    //            //先模拟登录成功

    //            //获得 token (md5)
    //            int userId = 0;//模拟此时已经获得 userId
    //            var r = new Random();
    //            string willMd5 = "" + userId + TimeTool.GetTimeStamp() + r.Next(0, 1000);
    //            string token = EncryptionTool.GetMd5Str32(willMd5);

    //            GS2GC.VerificationAskLoginResult resultData = new GS2GC.VerificationAskLoginResult();
    //            resultData.IsSuccess = true;
    //            resultData.GateServerPort = 2346;
    //            resultData.GateServerIp = "127.0.0.1";
    //            resultData.ClientId = loginData.ClientId;//这里是 LS 的 clientId
    //            resultData.Token = token;

    //            Console.WriteLine("send to LS : verificationLoginResult");
    //            LSServerConnect.Send((ushort)GS2GC.MsgId.Gs2LsVerificationAskLoginResult, resultData.ToByteArray());
    //        }
    //    }
    //}



    ////接收到 CS 服务端的消息(这里连接 CS)
    //public void ReceiveCSMsg(NetSession client, MsgPack pack)
    //{
    //    var msgId = (GS2GC.MsgId)pack.msgId;

    //    if (msgId > GS2GC.MsgId.Begin && msgId < GS2GC.MsgId.End)
    //    {
    //        if (msgId == GS2GC.MsgId.Gs2GcFromCsAskEnterGameServiceResult)
    //        {
    //            //转发
    //            SendToUser(pack);
    //        }

    //    }
    //}


    //void SendToLS(MsgPack pack)
    //{
    //    //这里只是转发 消息体目前没有任何改变 msgId 是 GS2GCFormCS 这种形式 这样的直接转发即可
    //    LSServerConnect.Send(pack);
    //}


    ///// <summary>
    ///// 客户端发来的消息
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="pack"></param>
    //protected override void HandleClientMsg(NetSession client, MsgPack pack)
    //{
    //    base.HandleClientMsg(client, pack);


    //    var msgId = (GC2CS.MsgId)pack.msgId;

    //    if (msgId > GC2CS.MsgId.Begin && msgId < GC2CS.MsgId.End)
    //    {
    //        if (msgId == GC2CS.MsgId.Gc2CsAskEnterGameService)//客户端请求进入游戏(此时登录已经成功)
    //        {

    //            var enterService = GC2GS.AskEnterGameServer.Parser.ParseFrom(pack.data);

    //            //这里进行验证 token () 成功后加入 Dic  之后的请求都会加入验证 token

    //            //这里验证 token 后 获得相应 userId
    //            int userId = 0;
    //            //userId2TokenDic.Add(pack.userId, token);
    //            userId2ClientIdDic.Add(userId, client.clientId);

    //            client.userId = userId;

    //            CSServerConnect.Send(pack);
    //        }

    //    }
    //    if (pack.msgId > 101 && pack.msgId < 200)//GC2LS (enter game service)
    //    {

    //    }

    //    if (pack.msgId > 201 && pack.msgId < 300)//other
    //    {
    //        //
    //    }


    //}

    //public override void ClientClose(int clientId)
    //{
    //    //清除 User 和 ClientId 对应关系

    //    if (clientDic.ContainsKey(clientId))
    //    {
    //        var userId = clientDic[clientId].userId;

    //        if (userId2ClientIdDic.ContainsKey(userId))
    //        {
    //            userId2ClientIdDic.Remove(userId);
    //        }
    //    }


    //    //清除 clientId
    //    base.ClientClose(clientId);

    //    //之后可能会清除 token 的 Dic

    //}

}