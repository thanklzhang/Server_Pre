using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GSMsgHandler //: MsgHandler
{

    public void HandlMsg(int gcNetId, int msgId, byte[] data)
    {
        if (msgId > (int)GC2LS.MsgId.Begin && msgId < (int)GC2LS.MsgId.End)
        {
            switch ((GC2LS.MsgId)msgId)
            {
                case GC2LS.MsgId.Gc2LsAskLogin:
                    VerifyLoginResult(gcNetId, msgId, data);
                    break;

            }


        }
    }

    private void VerifyLoginResult(int gcNetId, int msgId, byte[] data)
    {
        LSServer.Instance.SendMsgToClient(gcNetId, msgId, data);
    }
}

