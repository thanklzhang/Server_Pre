using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


public enum SessionType
{
    Null,
    Server_GS,
    Server_LS,
    Server_LS_OnlyGS,
    Server_CS_OnlyGS,
    Server_SS_OnlyGS,
    Client_GS_OnlyCS,
    Client_GS_OnlyLS,
    Client_GS_OnlySS,
    Client_TMN_OnlyLS,//终端到 LS
    Client_TMN_OnlyGS,//终端到 GS

}
public abstract class NetSessionMgr
{
    //之后这个可以变成一个 池储存
    public NetSession[] netSessions = new NetSession[maxNum];//数组随机取值时间最快 其他花里胡哨的要多一些时间
    int currPos = 1;
    const int maxNum = 10000;
    NetModule netModule;
    public NetSessionMgr()
    {
        netModule = new NetModule();
        StartCheckHeartBeat();
    }

    public TcpNetConnect CreateConnect(Socket socket)
    {
        return netModule.CreateConnect(socket);
    }

    public NetSession GetNetSession(int sessionId)
    {
        if (sessionId > 0 && sessionId < maxNum)
        {
            var session = netSessions[sessionId];
            if (session != null)
            {
                return session;
            }
        }

        return null;
    }

    public int AddNetSession(NetSession session)
    {

        if (currPos < maxNum)
        {
            netSessions[currPos] = session;
            session.closeAction += SessionClose;
            return currPos++;
        }
        else
        {
            //超出最大长度 寻找失效的 session 然后放置 此步之后弄
            Console.WriteLine("over the maxNum");
            if (1 > 0)//validate 有失效的 就放置
            {

            }
            else
            {
                return -1;
            }

        }

        return -1;

    }

    void SessionClose(int sessionId)
    {
        RemoveNetSeesion(sessionId);
    }

    public void RemoveNetSeesion(int pos)
    {
        if (pos < maxNum)
        {
            //netSessions[pos]?.Close();
            netSessions[pos] = null;
        }
    }

    public void RemoveNetSeesion(SessionType type)
    {
        for (int i = 0; i < netSessions.Length; ++i)
        {
            var ns = netSessions[i];
            if (ns != null)
            {
                if (type == ns.type)
                {
                    RemoveNetSeesion(i);
                    break;
                }
            }
        }
    }

    Timer timer;
    int interval = 2000;
    public void StartCheckHeartBeat()
    {
        this.timer = new Timer();
        timer.Interval = interval;
        timer.Elapsed += new ElapsedEventHandler(CheckHeartBeat);
        timer.Enabled = true;
    }

    public void CheckHeartBeat(object source, ElapsedEventArgs e)
    {
        //之后可能能会换成时间片轮询算法
        for (int i = 0; i < currPos - 1; ++i)
        {
            if (netSessions[i] != null)
            {
                var time = DateTime.Now;
                netSessions[i].CheckHeatBeat(time);
            }

        }
    }

    //作为服务端的 session
    public abstract NetSession CreateSession(SessionType type);

    //可连接其他服务端的 session
    public abstract NetSession CreateConnectorSession(SessionType type);

    /// <summary>
    /// 创建监听
    /// </summary>
    public void CreateListen(SessionType type, int port)
    {
        var netListen = netModule.CreateListen();
        //设置监听的信息

        netListen.SetNetSessionMgr(this, type);

        netListen.Start(port);
    }

    /// <summary>
    /// 创建连接
    /// </summary>
    public void CreateConnect(SessionType type, string ip, int port, Action<bool> connectAction = null)
    {
        var netConnect = netModule.CreateConnect();
        var netSession = CreateConnectorSession(type);

        var sId = AddNetSession(netSession);

        //填充 session
        netSession.sessionId = sId;

        netSession.SetConnect(netConnect);

        //netConnect.SetSession(netSession);

        netConnect.Connect(ip, port, connectAction);

    }


    public void SendMsgToSession(int sessionId, int infoId, int nsId, int msgId, byte[] data)
    {
        int dataLength = data.Length;

        Send(sessionId, msgId, data);
    }

    public void TransToSession(int sessionId, int infoId, int nsId, int msgId, byte[] data)
    {
        int dataLength = data.Length;

        byte[] currData = new byte[4 + 4 + 4 + 4 + dataLength];

        //var bInfoId = BitConverter.GetBytes(infoId);


        var bGcNetId = BitConverter.GetBytes(nsId);
        var bMsgId = BitConverter.GetBytes(msgId);
        var bDataLength = BitConverter.GetBytes(dataLength);


        Array.Copy(bGcNetId, 0, currData, 0, 4);
        Array.Copy(bMsgId, 0, currData, 1 * 4, 4);
        Array.Copy(bDataLength, 0, currData, 2 * 4, 4);
        Array.Copy(data, 0, currData, 3 * 4, dataLength);

        Send(sessionId, infoId, currData);
    }

    /// <summary>
    /// 对其中一个 session 发送消息
    /// </summary>
    public void Send(int sessionId, int infoId, byte[] msgBody)
    {
        var session = GetNetSession(sessionId);
        if (session != null)
        {
            session.Send(infoId, msgBody);
        }
        else
        {
            Console.WriteLine("the seesionId is null " + sessionId);
        }
    }

    public void SendTo(SessionType type, int infoId, int nsId, int msgId, byte[] data)
    {
        for (int i = 0; i < netSessions.Length; ++i)
        {
            var ns = netSessions[i];
            if (ns != null)
            {
                if (type == ns.type)
                {
                    SendMsgToSession(ns.sessionId, infoId, nsId, msgId, data);
                    break;
                }
            }
        }
    }

    public void TransTo(SessionType type, int infoId, int nsId, int msgId, byte[] data)
    {
        for (int i = 0; i < netSessions.Length; ++i)
        {
            var ns = netSessions[i];
            if (ns != null)
            {
                if (type == ns.type)
                {
                    
                    TransToSession(ns.sessionId, infoId, nsId, msgId, data);
                    break;
                }
            }
        }
    }



}

