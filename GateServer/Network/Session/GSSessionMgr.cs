using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
public class GSSessionMgr : NetSessionMgr
{
    //在这里创建 session 后 不会立即增加到集合中 需要成功建立连接后才会

    public override NetSession CreateSession(SessionType type)
    {
        NetSession netSession = null;
        if (type == SessionType.Server_GS)
        {
            netSession = new Client2MSession();
            netSession.type = SessionType.Server_GS;
            netSession.closeAction += ClientClose;
        }


        return netSession;
    }

    public void SetAccount(int sessionId, string account)
    {
        var session = GetNetSession(sessionId);
        session.account = account;
    }

    public override NetSession CreateConnectorSession(SessionType type)
    {
        NetSession session = null;
        if (type == SessionType.Client_GS_OnlyLS)
        {
            session = new M2LSSession();
            session.type = SessionType.Client_GS_OnlyLS;
        }

        if (type == SessionType.Client_GS_OnlyCS)
        {
            session = new M2CSSession();
            session.type = SessionType.Client_GS_OnlyCS;
        }

        if (type == SessionType.Client_GS_OnlySS)
        {
            session = new M2SSSession();
            session.type = SessionType.Client_GS_OnlySS;
        }

        return session;
    }

    public void ClientClose(int sessionId)
    {
        //client exit will send to center
        var session = GetNetSession(sessionId);

        GSServer.Instance.RemoveToken(session.account);

        GS2CS.reqUserExit exit = new GS2CS.reqUserExit();
        exit.Account = session.account;
        Console.WriteLine("client exit will send to center");
        GSServer.Instance.SendToCS(0, 0, (int)GS2CS.MsgId.Gs2CsUserExit, exit.ToByteArray());
    }

}

