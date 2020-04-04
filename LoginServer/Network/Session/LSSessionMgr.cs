using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LSSessionMgr : NetSessionMgr
{
    public override NetSession CreateConnectorSession(SessionType type)
    {
        throw new NotImplementedException();
    }

    //在这里创建 session 后 不会立即增加到集合中 需要成功建立连接后才会

    public override NetSession CreateSession(SessionType type)
    {
        NetSession session = null;
        if (type == SessionType.Server_LS)
        {
            session = new Client2MSession();
            session.type = type;
        }

        if (type == SessionType.Server_LS_OnlyGS)
        {
            session = new GS2MSession();
            session.type = type;
        }



        return session;
    }

    //public override NetSession CreateConnectorSession(SessionType type)
    //{
    //    NetSession session = null;
    //    if (type == SessionType.Client_GS_OnlyLS)
    //    {
    //        session = new M2LSSession();
    //        session.type = SessionType.Client_GS_OnlyLS;
    //    }

    //    if (type == SessionType.Client_GS_OnlyCS)
    //    {
    //        session = new M2LSSession();
    //        session.type = SessionType.Client_GS_OnlyCS;
    //    }

    //    return session;
    //}


}

