using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//using CenterServer.Mate;
using DataModel;
using Google.Protobuf;
//using SS2CS;

public class CSServer
{
    public static CSServer Instance;
    CSSessionMgr gsSessionMgr;

    public UserMgr userMgr;
   // public PlayerMgr playerMgr;

   // public MateManager mateManager;
    public CSServer()
    {
        Instance = this;
        userMgr = new UserMgr();
        //playerMgr = new PlayerMgr();
    }
    public void Start()
    {
        //Config.ConfigManager.Instance.LoadConfig();


        //Startup(2346);

        //匹配管理初始化
        //mateManager = new MateManager();
       // mateManager.Init();

        //session
        gsSessionMgr = new CSSessionMgr();
        //客户端(GS)连接此服务器
        gsSessionMgr.CreateListen(SessionType.Server_CS_OnlyGS, ConstInfo.CS_GS_Port);
        Console.WriteLine(SessionType.Server_CS_OnlyGS.ToString() + " : start to listen");



    }

    public User GetUser(string account)
    {
        return userMgr.GetUser(account);
    }

    public void UserOnline(User user)
    {
        userMgr.UserOnline(user);
        //playerMgr.Create(user);
    }

    //public Player GetPlayer(string account)
    //{
    //    return playerMgr.GetPlayer(account);
    //}

    internal void UserExit(string account)
    {
        //var player = GetPlayer(account);
        

        ////如果在战斗中 需要处理退出战斗
        //if (player != null)
        //{
        //    if (mateManager.IsPlayerInCombat(player))
        //    {
        //        mateManager.UserExitCombat(GetPlayer(account));
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("the player is not found : " + account);
        //}

        userMgr.UserExit(account);
        //playerMgr.RemovePlayer(account);
    }

    internal void TransToGS(int infoId, int gcNsId, int msgId, byte[] data)
    {
        gsSessionMgr.TransTo(SessionType.Server_CS_OnlyGS, infoId, gcNsId, msgId, data);
    }



    //public void PlayerStartMate(Player player)
    //{
    //    mateManager.AddPlayerToMate(player);
    //}

    //public void PlayerCancelMate(Player player)
    //{
    //    mateManager.RemovePlayerFromMate(player);
    //}

    //internal void CreateCombatFinish(CreateCombatFinish createCombatFinish)
    //{
    //    mateManager.StartCombat(createCombatFinish.CombatId, createCombatFinish.CombatRoomId);
    //}

    //public void DestroyCombat()
    //{
    //    mateManager.DestroyCombat(1, 1);
    //}
}