using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GS2MSession : NetSession
{
    GSMsgHandler handler = new GSMsgHandler();

    public GS2MSession()
    {
        SetHandlerAction(Dispatch);
    }

    public void Dispatch(int transId, byte[] body)
    {
        if (transId > (int)GS2CS.MsgId.Begin && transId < (int)GS2CS.MsgId.End)
        {
            //这里是直接处理 GS 到 CS 的消息 不作消息变换
            handler.HandleMsg(0, transId, body);
        }
        else
        {
            //这里是转发

            int bodyLen = body.Length;
            int gcNetId = BitConverter.ToInt32(body, 0);
            int msgId = BitConverter.ToInt32(body, 4);
            int dataLength = BitConverter.ToInt32(body, 4 * 2);
            byte[] currData = new byte[dataLength];

            Array.Copy(body, 4 * 3, currData, 0, dataLength);

            handler.HandleMsg(gcNetId, msgId, currData);
        }
    }
}

