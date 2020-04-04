using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CSSessionMgr : NetSessionMgr
{
    public override NetSession CreateConnectorSession(SessionType type)
    {
        throw new NotImplementedException();
    }

    //在这里创建 session 后 不会立即增加到集合中 需要成功建立连接后才会

    public override NetSession CreateSession(SessionType type)
    {
        NetSession netSession = null;
        if (type == SessionType.Server_CS_OnlyGS)
        {
            netSession = new GS2MSession();
            netSession.type = SessionType.Server_CS_OnlyGS;
        }


        return netSession;
    }

    //public override NetSession CreateConnectorSession(SessionType type)
    //{
    //    NetSession session = null;
    //    if (type == SessionType.Server_CS_OnlyGS)
    //    {
    //        session = new GS2MSession();
    //        session.type = SessionType.Server_CS_OnlyGS;
    //    }


    //    return session;
    //}


}

