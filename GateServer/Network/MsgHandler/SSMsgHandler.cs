using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
public class SSMsgHandler //: MsgHandler
{

    public void HandleMsg(int gcNetId, int msgId, byte[] data)
    {
        //if (msgId > (int)SS2CS.MsgId.Begin && msgId < (int)SS2CS.MsgId.End)
        //{

        //    switch ((SS2CS.MsgId)msgId)
        //    {
        //        case SS2CS.MsgId.Ss2CsCreateCombatFinish:
        //            CreateCombatFinish(gcNetId, msgId, data);
        //            break;
        //    }

        //}
    }

    public void CreateCombatFinish(int gcNetId, int msgId, byte[] data)
    {
        Console.WriteLine("on gate : create combat finish");

        GSServer.Instance.TransToCS(0, gcNetId, msgId, data);
    }
}

