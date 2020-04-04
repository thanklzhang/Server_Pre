using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 接受数据时处理发生的异常
/// </summary>
public class HandleReceiveException : Exception
{
    public HandleReceiveException(string message) : base(message)
    {
    }
}

public class TcpNetConnect
{
    Socket socket;
    NetSession netSession;
    const int buffSize = 8 * 1024;
    private byte[] buffer = new byte[buffSize];
    public byte[] dataBuffer;
    public NetState netState;
    int fixedHeadByteLength = 8;//4 4 X (传输消息  长度 包体)
    int fixedHeadDataBodyLengthByteLength = 4;//数据包体长度所占的字节长度
    //public HeartBeatService heartBeatByClient;//作为客户端发送心跳
    //public int heartBeatInterval = 2000;
    public TcpNetConnect()
    {

    }

    public TcpNetConnect(Socket socket)
    {
        this.socket = socket;
        //heartBeat = new HeartBeatService(heartBeatInterval, this);
    }


    public void Connect(string ip, int port, Action<bool> connectAction = null)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress mIp = IPAddress.Parse(ip);
            IPEndPoint ip_end_point = new IPEndPoint(mIp, port);
            socket.BeginConnect(ip_end_point, (e) =>
            {
                netSession.OnConnect(socket.Connected);
                connectAction?.Invoke(socket.Connected);
                if (socket.Connected)
                {
                    netState = NetState.Connect;
                    StartReceive();

                    //暂时不用
                    //heartBeatByClientByClient = new HeartBeatService(this);
                    //heartBeatByClientByClient.Start();
                }


            }, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ChangeToCloseState();
        }

    }

    /// <summary>
    /// connect 之前已经初始化 session 的初始信息了
    /// </summary>
    /// <param name="session"></param>
    public void SetSession(NetSession session)
    {
        this.netSession = session;
    }


    public void Send(int msgHeader, byte[] data)
    {
        try
        {
            byte[] resultData = BuildData(msgHeader, data);//userId,
            socket.BeginSend(resultData, 0, resultData.Length, SocketFlags.None, SendMsg, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ChangeToCloseState();
        }


    }
    private void SendMsg(IAsyncResult ar)
    {
        socket.EndSend(ar);
    }

    /// <summary>
    /// 默认 4 4 x
    /// </summary>
    /// <param name="msg"></param>
    public virtual byte[] BuildData(int msgHeader, byte[] dataContent)// int userId,
    {
        byte[] data = null;
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write(msgHeader);
            //writer.Write(userId);
            writer.Write(dataContent.Length);
            writer.Write(dataContent);
            writer.Flush();

            data = ms.ToArray();
            writer.Close();
        }

        return data;

    }


    public void StartReceive()
    {
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), null);

    }

    void Receive(IAsyncResult ar)
    {
        if (this.netState == NetState.Close)
        {
            return;
        }

        try
        {
            var length = socket.EndReceive(ar);
            //Console.WriteLine("msg length : " + length);
            if (length == 0)
            {
                ChangeToCloseState();
                //Console.WriteLine(socket.RemoteEndPoint + " : disconnect");
                return;
            }
            //dataBuffer 加上这段数据
            if (dataBuffer == null)
            {
                dataBuffer = new byte[length];
                Array.Copy(buffer, dataBuffer, length);
                //Console.WriteLine("test1 : dataBuffer" + dataBuffer + " " + dataBuffer.Length);
            }
            else
            {
                byte[] finalB = new byte[length];
                Array.Copy(buffer, 0, finalB, 0, length);
                dataBuffer = dataBuffer.Concat(finalB).ToArray();
            }

            while (dataBuffer.Length >= fixedHeadByteLength)
            {
                int bodyLength = BitConverter.ToInt32(dataBuffer, fixedHeadDataBodyLengthByteLength);
                if (bodyLength <= dataBuffer.Length - fixedHeadByteLength)
                {
                    byte[] currData = new byte[bodyLength + fixedHeadByteLength];

                    Array.Copy(dataBuffer, 0, currData, 0, bodyLength + fixedHeadByteLength);

                    byte[] nextData = new byte[dataBuffer.Length - fixedHeadByteLength - bodyLength];
                    Array.Copy(dataBuffer, fixedHeadByteLength + bodyLength, nextData, 0, dataBuffer.Length - fixedHeadByteLength - bodyLength);
                    dataBuffer = nextData;

                    //处理消息
                    ParseFromMsg(currData); //此处如果有错误 那么跳过此次
                }
                else
                {
                    //接收数据缓冲区满了 没到达一个数据包 所以接着接收 直到到了一个数据包的长度
                    break;
                }
            }

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), null);
        }
        catch (HandleReceiveException e)
        {
            Console.WriteLine("HandleReceiveException : " + e);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), null);
            //这里先不关闭连接 只是将此次包的处理逻辑跳过
            //ChangeToCloseState();
        }
        catch (Exception e)
        {
            Console.WriteLine("normal exception : " + e);
            ChangeToCloseState();
        }


    }



    /// <summary>
    /// 默认 4 4 x 
    /// </summary>
    /// <param name="msg"></param>
    public virtual void ParseFromMsg(byte[] msg)
    {
        try
        {
            // Console.WriteLine("parse from msg");
            MemoryStream ms = null;
            using (ms = new MemoryStream(msg))
            {
                BinaryReader reader = new BinaryReader(ms);
                int infoId = reader.ReadInt32();
                //int userId = reader.ReadInt32();
                int len = reader.ReadInt32();

                byte[] body = reader.ReadBytes(len);

                netSession.OnReceive(infoId, body);

                //var msgBytes = MsgPack.Create(transId, data);//userId,

                //ReceiveMsgAction?.Invoke(this, msgBytes);

                reader.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("the error : " + e);
            throw new HandleReceiveException(e.Message);
        }



    }

    public void ChangeToCloseState()
    {
        this.netState = NetState.Close;
        netSession?.Close();
        //heartBeatByClient?.Stop();
        this.Close();
    }

    public void Close()
    {
        if (socket != null)
        {
            socket.Close();
        }

        netSession = null;
        buffer = new byte[buffSize];
        dataBuffer = null;



    }

    //public virtual void HandleMsg(MsgPack pack)
    //{
    //    //作为客户端
    //    if (NetCommonMsgId.heatBeatHandshake == pack.msgId)
    //    {
    //        heartBeatService.Start();
    //    }

    //    if (NetCommonMsgId.heatBeatBack == pack.msgId)
    //    {
    //        heartBeatService.ResetTimeout();
    //    }

    //    //作为服务端的 client 连接
    //    if (NetCommonMsgId.heatBeatSend == pack.msgId)
    //    {
    //        ResetHeartBeat();
    //    }


    //}


}

