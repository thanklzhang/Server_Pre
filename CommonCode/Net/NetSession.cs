using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

public enum NetState
{
    Null,
    Connect,
    Close,
}

public class NetSession //: ITcpClient
{
    //event
    public Action<bool> connectAction;
    public Action<int> closeAction;


    public Action<int, byte[]> handleMsgAction;
    public Action<int, byte[]> handleUnkonowMsgAction;
    TcpNetConnect netConnect;
    public int sessionId;
    public string account = "";//先用账号 登陆之后有值
    //public int userId;
    public SessionType type;

    DateTime heartTime;
    public NetSession()
    {
        Init();
    }
    void Init()
    {
        ////作为服务端时候接受 client 的心跳
        //ResetHeartBeat();

        ////客户端心跳服务
        //heartBeatService = new HeartBeatService(5000, this);
    }


    internal void SetConnect(TcpNetConnect netConnect)
    {
        netConnect.SetSession(this);
        this.netConnect = netConnect;

    }

    //public void ResetHeartBeat()
    //{
    //    heartTime = DateTime.Now;
    //}

    public void Close()
    {
        Console.WriteLine("session close : the sessionId is : " + this.sessionId);
        closeAction?.Invoke(this.sessionId);

        connectAction = null;
        closeAction = null;

        handleMsgAction = null;
        handleUnkonowMsgAction = null;
        netConnect = null;

    }


    public void Send(int infoId, byte[] msgBody)
    {
        netConnect.Send(infoId, msgBody);
    }

    public void OnConnect(bool isSuccess)
    {

    }

    public void OnReceive(int msgHeader, byte[] body)
    {
        new Task(() =>
        {
            //if (msgHeader == ConstInfo.sendHeartBeatMsgId)
            //{
            //    //Console.WriteLine("receive 100");
            //    //客户端发来的心跳
            //    ResetHeartBeat();

            //    //发送心跳返回
            //    Send(ConstInfo.receiveHeartBeatMsgId, new byte[] { });
            //}
            //else if (msgHeader == ConstInfo.receiveHeartBeatMsgId)
            //{

            //    //服务端的心跳返回
            //    netConnect.heartBeatByClientByClient.ResetTimeout();

            //}
            //else
            //{
            //    //正常消息处理
            //    handleMsgAction?.Invoke(msgHeader, body);
            //}

            //暂时不处理心跳
            try
            {
                handleMsgAction?.Invoke(msgHeader, body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            
        }).Start();
        
    }

    public void SetHandlerAction(Action<int, byte[]> action)
    {
        this.handleMsgAction = action;
    }

    internal void StartReceive()
    {
        netConnect.StartReceive();
        isStartReceived = true;

    }

    bool isStartReceived;//是否已经开始接受消息

    internal void CheckHeatBeat(DateTime time)
    {
        //暂时不用
        //if (!isStartReceived)
        //{
        //    return;
        //}

        //if ((time - heartTime).Seconds > ConstInfo.heartBeatInterval * 2)
        //{
        //    this.Close();
        //}

    }
}

