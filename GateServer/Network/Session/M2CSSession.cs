using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class M2CSSession : NetSession
{
    CSMsgHandler handler = new CSMsgHandler();

    public M2CSSession()
    {
        SetHandlerAction(Dispatch);
    }
    

    public void Dispatch(int transId, byte[] body)
    {
        Console.WriteLine("receive msg from CS ");
        int bodyLen = body.Length;
        int gcNetId = BitConverter.ToInt32(body, 0);
        int msgId = BitConverter.ToInt32(body, 4);
        int dataLength = BitConverter.ToInt32(body, 4 * 2);
        byte[] currData = new byte[dataLength];

        Array.Copy(body, 4 * 3, currData, 0, dataLength);

        
        handler.HandleMsg(gcNetId, msgId, currData);


    }



}

