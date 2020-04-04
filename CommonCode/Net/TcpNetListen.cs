using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TcpNetListen
{
    Socket socket;
    //public Action<Socket> clientConnectAction;
    NetSessionMgr netMgr;
    SessionType sessionType;
    public void Start(int port, int backlog = 100)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);
        socket.Bind(iep);
        socket.Listen(backlog);

        socket.BeginAccept(Accept, null);
    }

    public void Stop()
    {

    }

    public void Release()
    {

    }

    public void Accept(IAsyncResult e)
    {
        var clientSocket = socket.EndAccept(e);
        //clientConnectAction?.Invoke(clientSocket);
        //创建一个 session
        NetSession netSession = netMgr.CreateSession(sessionType);//这里之前已经被复制赋值(SetNetSession)
        var sId = netMgr.AddNetSession(netSession);
        netSession.sessionId = sId;

        //给 session 增加 connect
        var connect = netMgr.CreateConnect(clientSocket);
        netSession.SetConnect(connect);
        netSession.StartReceive();
        Console.WriteLine("user connect : " + clientSocket.RemoteEndPoint.ToString());

        socket.BeginAccept(Accept, null);
    }

    public void SetNetSessionMgr(NetSessionMgr mgr, SessionType type)
    {
        this.netMgr = mgr;
        this.sessionType = type;
    }
}
