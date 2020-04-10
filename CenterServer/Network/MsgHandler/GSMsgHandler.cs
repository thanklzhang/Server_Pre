using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using Google.Protobuf;
using Logic;
//处理消息 ， 在 CS 中 将各种业务层的再次提取分开
public class GSMsgHandler //: MsgHandler
{
    string currAccount;
    //int currSessionId;//GS 中的 客户端的 sessionId 
    User currUser;

    public void HandleMsg(int gcNetId, int msgId, byte[] data)
    {

        //非转发消息
        if (msgId > (int)GS2CS.MsgId.Begin && msgId < (int)GS2CS.MsgId.End)
        {
            switch ((GS2CS.MsgId)msgId)
            {
                case GS2CS.MsgId.Gs2CsUserExit:
                    GS2CS.reqUserExit exit = GS2CS.reqUserExit.Parser.ParseFrom(data);
                    Console.WriteLine("user : " + exit.Account + " exit from center ...");
                    UserMgr.Instance.UserExit(exit.Account);
                    //CSServer.Instance.UserExit(exit.Account);
                    break;

            }

        }

        //以下都是转发过来的

        //登录特殊 需要从 CS 验证用户
        if (msgId > (int)GC2LS.MsgId.Begin && msgId < (int)GC2LS.MsgId.End)
        {
            switch ((GC2LS.MsgId)msgId)
            {
                case GC2LS.MsgId.Gc2LsAskLogin:

                    GC2LS.reqAskLogin askLogin = GC2LS.reqAskLogin.Parser.ParseFrom(data);
                    string account = askLogin.Account;
                    string password = askLogin.Password;

                    bool isLoginSuccess = false;
                    UserData userData = DBMgr.userDataMgr.Find(null, "account = '" + account + "'");//, password



                    //List<int> lo = null;
                    //lo.Add(1);
                    bool isNewUser = false;
                    if (userData != null)
                    {
                        //Console.WriteLine("have uer , server login info : " + user + " " + user.account + " " + user.password);
                        if (password == userData.password)
                        {
                            isLoginSuccess = true;
                        }
                        Console.WriteLine("have user , check password : " + isLoginSuccess);
                    }
                    else
                    {
                        isNewUser = true;
                        //Console.WriteLine("server login info : " + " fail");

                        //目前没有的用户先给创建一个
                        Console.WriteLine("no user , create a new user ");
                        //var id = User.GetMaxId();
                        //计算 token
                        Random r = new Random();
                        string tokenStr = account + TimeTool.GetTimeStamp() + r.Next(100000);
                        string token = EncryptionTool.GetMd5Str(tokenStr);
                        userData = UserDataMgr<UserData>.CreateUserData(account, password, token);

                        DBMgr.userDataMgr.Save(userData, true);

                        //改为在内存中赋 id
                        //var userTemp = DBMgr.userDataMgr.Find(new string[] { "id" }, "account = '" + account + "'");//, password
                        //userData.id = userTemp.id;

                        //DBMgr.redisOrg.HashSet("accountId", account, id);
                        isLoginSuccess = true;


                    }


                    GC2LS.respAskLogin loginResult = new GC2LS.respAskLogin();
                    loginResult.IsSuccess = isLoginSuccess;

                    if (isLoginSuccess)
                    {
                        loginResult.GateServerIp = ConstInfo.GS_IP;
                        loginResult.GateServerPort = ConstInfo.GS_Client_Port;
                        loginResult.UserId = "" + userData.id;
                        loginResult.UserAccount = userData.account;

                        //this.currAccount = user.account;

                        //this.currSessionId = user.sessionId;

                        // Console.WriteLine("token : " + user.token);
                        Console.WriteLine("user online : " + userData.account);

                        //用户成功登录
                        User onlineUser = User.Create(userData.id, userData.account, userData.password, userData.token);

                        //加载用户数据
                        if (!onlineUser.isLoadedData)
                        {
                            HeroMgr.Instance.LoadHeroData(onlineUser);
                            // CSServer.Instance.heroMgr.LoadHeroData(onlineUser);

                            onlineUser.isLoadedData = true;
                        }
                        UserMgr.Instance.UserOnline(onlineUser);
                        //CSServer.Instance.UserOnline(onlineUser);

                        //到这里 用户已经可以进入游戏了

                        currUser = UserMgr.Instance.GetUser(account);

                        if (isNewUser)
                        {
                            //test 上场免费 2 个英雄
                            var heroData0 = HeroDataMgr<HeroData>.CreateHeroData(currUser.id, 100);
                            var heroData1 = HeroDataMgr<HeroData>.CreateHeroData(currUser.id, 101);
                            Hero hero0 = HeroMgr.CreateHero(heroData0.configId, heroData0);
                            Hero hero1 = HeroMgr.CreateHero(heroData1.configId, heroData1);
                            DBMgr.SaveAll();
                            HeroMgr.Instance.LoadHeroData(currUser);
                        }




                    }

                    //这个是 登录中 的session id 所以不能用 currSessionId
                    CSServer.Instance.TransToGS(0, gcNetId, (int)GC2LS.MsgId.Gc2LsAskLogin, loginResult.ToByteArray());


                    break;
            }
        }

        ////战斗服务器的请求
        //if (msgId > (int)SS2CS.MsgId.Begin && msgId < (int)SS2CS.MsgId.End)
        //{
        //    switch ((SS2CS.MsgId)msgId)
        //    {
        //        case SS2CS.MsgId.Ss2CsCreateCombatFinish:

        //            CreateCombatFinish(gcNetId, data);
        //            break;
        //    }
        //}


        //客户端的请求
        if (msgId > (int)GC2CS.MsgId.Begin && msgId < (int)GC2CS.MsgId.End)
        {


            var logicHandler = HandlerLogicMgr.Instance.Get(msgId);
            logicHandler.Handler(currUser, gcNetId, data);
            //switch ((GC2CS.MsgId)msgId)
            //{
            //    case GC2CS.MsgId.Gc2CsEnterGameService:

            //        HandleEnterGame(gcNetId, data);
            //        break;
            //    case GC2CS.MsgId.Gc2CsHeroList:

            //        HandleHeroList(gcNetId, data);
            //        break;
            //    case GC2CS.MsgId.Gc2CsAddHeroLevel:

            //        HandleAddHeroLevel(gcNetId, data);
            //        break;
            //        //case GC2CS.MsgId.Gc2CsAskStartNormalManagement:

            //        //    break;
            //        //case GC2CS.MsgId.Gc2CsAskStartOvertimeManagement:
            //        //    StartOvertimelManagement(gcNetId, data);
            //        //    break;
            //        //case GC2CS.MsgId.Gc2CsStartMateCombat:
            //        //    StartMateCombat(gcNetId, data);
            //        //    break;

            //}
        }

    }

    //这些之后会变成单独的业务层处理

    //public void HandleEnterGame(int gcNetId, byte[] data)
    //{
    //    GC2CS.reqEnterGameService enterGame = GC2CS.reqEnterGameService.Parser.ParseFrom(data);
    //    string account = enterGame.Account;

    //    GC2CS.respEnterGameService enterResult = new GC2CS.respEnterGameService();
    //    enterResult.IsSuccess = true;
    //    enterResult.Err = ResultCode.Success;
    //    //this.currSessionId = gcNetId;

    //    //用户可以进行游戏了 这时候需要把 sessionId 赋值
    //    UserMgr.Instance.GetUser(account).SetSessionId(gcNetId);
    //    //CSServer.Instance.GetUser(account).SetSessionId(gcNetId);


    //    Console.WriteLine("trans to GS : " + GC2CS.MsgId.Gc2CsEnterGameService.ToString());
    //    SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsEnterGameService, enterResult.ToByteArray());




    //    //推送一些需要的数据
    //    SyncHeroList(gcNetId);

    //    //Sync info
    //    //currPlayer.SyncPlayerBaseInfo();
    //    //currPlayer.SyncHeroListInfo();
    //    //currPlayer.SyncItemListInfo();
    //    //currPlayer.SyncDrawingListInfo();
    //    //currPlayer.SyncManagementInfo();

    //}

    //private void HandleHeroList(int gcNetId, byte[] data)
    //{
    //    GC2CS.reqHeroList req = GC2CS.reqHeroList.Parser.ParseFrom(data);

    //    SyncHeroList(gcNetId);
    //}

    //void SyncHeroList(int gcNetId)
    //{
    //    GC2CS.respHeroList resp = new GC2CS.respHeroList();
    //    var heroList = currUser.GetOwnHeroes();
    //    for (int i = 0; i < heroList.Count; ++i)
    //    {
    //        var ownHero = heroList[i];
    //        var heroInfo = new GC2CS.HeroInfo()
    //        {
    //            Id = ownHero.data.id,
    //            Level = ownHero.data.level,
    //            ConfigId = ownHero.config.id

    //        };
    //        resp.HeroList.Add(heroInfo);
    //    }
    //    SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsHeroList, resp.ToByteArray());
    //}

    //void HandleAddHeroLevel(int gcNetId, byte[] data)
    //{
    //    GC2CS.reqAddHeroLevel req = GC2CS.reqAddHeroLevel.Parser.ParseFrom(data);
    //    GC2CS.respAddHeroLevel resp = new GC2CS.respAddHeroLevel();
    //    var hero = currUser.GetOwnHero(req.HeroId);
    //    if (hero != null)
    //    {
    //        hero.AddLevel(1);

    //        //如果过多 那么包会过大 那么需要分包 HERO_MAX_SIZE_IN_MSG
    //        HeroMgr.Instance.NotifyUpdateHeroes(currUser, new List<Hero>() { hero });

    //        resp.Err = ResultCode.Success;
    //        SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsAddHeroLevel, resp.ToByteArray());
    //    }
    //    else
    //    {
    //        resp.Err = ResultCode.ErrUnknown;
    //        SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsAddHeroLevel, resp.ToByteArray());
    //    }
    //}


    void SendToClient(int sessionId, int msgId, byte[] data)
    {
        //Console.WriteLine("send to client : " + currSessionId);
        CSServer.Instance.TransToGS(0, sessionId, msgId, data);
    }


}

