using Combat;
using Google.Protobuf;
using SSCombatServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class SSServer// : TcpServer
{
    public static SSServer Instance;
    public SSServer()
    {
        Instance = this;
    }
    SSSessionMgr ssSessionMgr;
    CombatManager combatManager;
    public void Start()
    {
        ssSessionMgr = new SSSessionMgr();

        //GS 连接此服务器(LS)
        ssSessionMgr.CreateListen(SessionType.Server_SS_OnlyGS, ConstInfo.SS_Tcp_Port);
        Console.WriteLine(SessionType.Server_SS_OnlyGS.ToString() + " start success");

        combatManager = new CombatManager();
        combatManager.Init();

    }

    internal void TransToGS(int infoId, int gcNsId, int msgId, byte[] data)
    {
        ssSessionMgr.TransTo(SessionType.Server_SS_OnlyGS, infoId, gcNsId, msgId, data);
    }

    public void CreateCombat(GS2SS.CreateCombat createCombat)
    {
        var combat = combatManager.StartACombat(createCombat.PlayerLocations.ToList());

        SS2CS.CreateCombatFinish createCombatFinish = new SS2CS.CreateCombatFinish()
        {
            CombatId = null == combat ? -1 : combat.id,
            CombatRoomId = createCombat.RoomId
        };
        Console.WriteLine("create combat finish");
        TransToGS(0, 0, (int)SS2CS.MsgId.Ss2CsCreateCombatFinish, createCombatFinish.ToByteArray());

    }

    //public void PlayerExit()
    //{
    //    combatManager.PlayerExit(combatId, account);
    //}

    public void DestroyCombat(int combatId)
    {
        combatManager.DestroyCombat(combatId);
    }


}