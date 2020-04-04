using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetModule
{
    public TcpNetListen CreateListen()
    {
        TcpNetListen listen = new TcpNetListen();
        return listen;
    }

    //根据已有的 socket 来创建创建连接
    public TcpNetConnect CreateConnect(Socket socket)
    {
        TcpNetConnect connect = new TcpNetConnect(socket);
        return connect;
    }

    //创建一个空的连接 需要自己去连接
    public TcpNetConnect CreateConnect()
    {
        TcpNetConnect connect = new TcpNetConnect();
        return connect;
    }
}

