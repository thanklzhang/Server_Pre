using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class UdpNet
{
    Socket sendSocket;
    Socket receiveSocket;
    //发送消息的目标 endPoint
    const int bufferSize = 8 * 1024;
    byte[] buffer = new byte[bufferSize];

    EndPoint myEndPoint;
    //EndPoint otherEndPoint;

    public Action<byte[]> ReceiveAction;

    public UdpNet()
    {
        receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    }
    //public void SetSendEndPoint(string targetIp, int targetPort)
    //{
    //    sendMsgTarget = new IPEndPoint(IPAddress.Parse(targetIp), targetPort);
    //}

    public void SetReceiveEndPoint(int port)
    {
        myEndPoint = new IPEndPoint(IPAddress.Any, port);
    }


    public void StartReceive(int port)
    {
        receiveSocket.Bind(myEndPoint);
        EndPoint otherEndPoint = new IPEndPoint(IPAddress.Any, port);

        Console.WriteLine("start receive" + " " + port);
        receiveSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref otherEndPoint, Receive, otherEndPoint);

    }

    void Receive(IAsyncResult e)
    {
        var ep = (EndPoint)e.AsyncState;
        var length = receiveSocket.EndReceiveFrom(e, ref ep);
        //Console.WriteLine("received from : " + otherEndPoint.ToString());
        byte[] data = new byte[length];
        Array.Copy(buffer, data, length);
        ReceiveAction?.Invoke(data);
        //Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, length));

        receiveSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ep, Receive, ep);
    }

    public void Send(EndPoint sendMsgTarget, int msgId, byte[] msg)
    {
        //Console.WriteLine("send msg to player ... ");
        var data = BuildData(msgId, msg);
        sendSocket.SendTo(data, data.Length, SocketFlags.None, sendMsgTarget);//将数据发送到指定的终结点
    }

    public virtual byte[] BuildData(int msgHeader, byte[] dataContent)// int userId,
    {
        byte[] data = null;
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write(msgHeader);
            writer.Write(dataContent);
            writer.Flush();

            data = ms.ToArray();
            writer.Close();
        }

        return data;

    }

    public void Close()
    {
        if (sendSocket != null)
        {
            sendSocket.Close();
        }

        if (receiveSocket != null)
        {
            receiveSocket.Close();
        }

        myEndPoint = null;
        //otherEndPoint = null;
        ReceiveAction = null;
    }

}

