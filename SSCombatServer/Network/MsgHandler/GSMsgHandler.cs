using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GSMsgHandler //: MsgHandler
{

    public void HandlMsg(int gcNetId, int msgId, byte[] data)
    {
        if (msgId > (int)GS2SS.MsgId.Begin && msgId < (int)GS2SS.MsgId.End)
        {
            switch ((GS2SS.MsgId)msgId)
            {
                case GS2SS.MsgId.Gs2SsFromCsCreateCombat:
                    CreateCombat(gcNetId, msgId, data);
                    break;
                case GS2SS.MsgId.Gs2SsFromCsDestroyCombat:
                    DestroyCombat(gcNetId, msgId, data);
                    break;
            }
        }
    }

    private void CreateCombat(int gcNetId, int msgId, byte[] data)
    {
        Console.WriteLine("receive create combat request");
        //LSServer.Instance.SendMsgToClient(gcNetId, msgId, data);

        GS2SS.CreateCombat createCombat = GS2SS.CreateCombat.Parser.ParseFrom(data);

        SSServer.Instance.CreateCombat(createCombat);

    }

    //void PlayerExit(int gcNetId, int msgId, byte[] data)
    //{

    //}

    private void DestroyCombat(int gcNetId, int msgId, byte[] data)
    {
        Console.WriteLine("destroy combat");
        GS2SS.DestroyCombat destroyCombat = GS2SS.DestroyCombat.Parser.ParseFrom(data);
        SSServer.Instance.DestroyCombat(destroyCombat.CombatId);
    }
}

