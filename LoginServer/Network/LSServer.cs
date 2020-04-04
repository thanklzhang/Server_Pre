using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class LSServer// : TcpServer
{
    //NetSession GSClientConnect;//这个是连接到 LS 这里的客户端(这个是 GS 充当客户端)
    public static LSServer Instance;
    public LSServer()
    {
        Instance = this;
    }
    LSSessionMgr lsSessionMgr;
    public void Start()
    {
        lsSessionMgr = new LSSessionMgr();

        //客户端连接此服务器
        lsSessionMgr.CreateListen(SessionType.Server_LS, ConstInfo.LS_Client_Port);
        Console.WriteLine(SessionType.Server_LS.ToString() + " start success");

        //GS 连接此服务器(LS)
        lsSessionMgr.CreateListen(SessionType.Server_LS_OnlyGS, ConstInfo.LS_GS_Port);
        Console.WriteLine(SessionType.Server_LS_OnlyGS.ToString() + " start success");



    }

    internal void TransToGS(int infoId, int gcNsId, int msgId, byte[] data)
    {
        lsSessionMgr.TransTo(SessionType.Server_LS_OnlyGS, infoId, gcNsId, msgId, data);
    }

    public void SendMsgToClient(int gcNSId, int msgId, byte[] data)
    {
        lsSessionMgr.SendMsgToSession(gcNSId, 0, gcNSId, msgId, data);
    }

    ////因为 GS 连接 LS 所以从这里接受消息
    //protected override void HandleClientMsg(NetSession client, MsgPack pack)
    //{
    //    base.HandleClientMsg(client, pack);

    //    Console.WriteLine("receive : " + client.socket.RemoteEndPoint.ToString() + " msgId: " +
    //        pack.msgId);

    //    //包括登录验证发送 GS 地址
    //    if (pack.msgId > (int)GS2GC.MsgId.Begin && pack.msgId < (int)GS2GC.MsgId.End)//GS2GC  //因为这里是 GS 传给 GC , 所以这里只转发即可
    //    {
    //        var result = GS2GC.VerificationAskLoginResult.Parser.ParseFrom(pack.data);

    //        //这里的 clientId 是指 在 LS 上 的 clienId
    //        SendToClient(result.ClientId, pack.msgId,  pack.data);
    //    }

    //    if (pack.msgId > (int)GS2LS.MsgId.Begin && pack.msgId < (int)GS2LS.MsgId.End)//GS2LS regist server //这个仅仅是为了 GS 注册一下 让 LS 知道这个客户端是 GS
    //    {
    //        if (pack.msgId == (int)GS2LS.MsgId.Gs2LsRegistServer)
    //        {
    //            Console.WriteLine("regist server by GS");
    //            GSClientConnect = client;
    //        }
    //    }

    //    if (pack.msgId > (int)GC2LS.MsgId.Begin && pack.msgId < (int)GC2LS.MsgId.End)//GS2LS regist server //这个仅仅是为了 GS 注册一下 让 LS 知道这个客户端是 GS
    //    {
    //        if (pack.msgId == (int)GC2LS.MsgId.Gc2LsAskLogin)
    //        {
    //            var cLoginData = GC2LS.AskLogin.Parser.ParseFrom(pack.data);

    //            //这里需要填充信息 发到 GS 
    //            LS2GS.VerificationAskLogin login = new LS2GS.VerificationAskLogin();
    //            login.Account = cLoginData.Account;
    //            login.Password = cLoginData.Password;
    //            login.ClientId = client.clientId;//这里是 LS 的 clientId
    //            Console.WriteLine("receive and to send to GS : " + client.socket.RemoteEndPoint.ToString() + " msgId: " +
    //       pack.msgId);
    //            //...此时还没有 token
    //            GSClientConnect.Send((ushort)LS2GS.MsgId.Ls2GsVerificationAskLogin,login.ToByteArray());
    //        }

    //    }

    //}
}