using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Client2MSession : NetSession
{
    GCMsgHandler handler = new GCMsgHandler();

    public Client2MSession()
    {
        SetHandlerAction(Dispatch);
    }


    public void Dispatch(int infoId, byte[] body)
    {
        Console.WriteLine("receive msg from GC");
        //从客户端发来的消息和服务端之间发送消息不同
        int msgId = infoId;
        byte[] data = body;

        handler.HandleMsg(sessionId, msgId, data);


    }
}

