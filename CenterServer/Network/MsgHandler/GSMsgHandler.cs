using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using Google.Protobuf;
public class GSMsgHandler //: MsgHandler
{
    string currAccount;
    //int currSessionId;//GS 中的 客户端的 sessionId 
    //Player currPlayer;

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
                    CSServer.Instance.UserExit(exit.Account);
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
                    User user = null;
                    //var user = UserDBOp.CheckUser(account);//, password




                    //if (user != null)
                    //{
                    //    Console.WriteLine("server login info : " + user + " " + user.account + " " + user.password);
                    //    if (password == user.password)
                    //    {
                    //        isLoginSuccess = true;
                    //    }
                    //    Console.WriteLine("check password : " + password == user.password);
                    //}
                    //else
                    //{
                    //    //Console.WriteLine("server login info : " + " fail");

                    //    //目前没有的用户先给创建一个
                    //    Console.WriteLine("new user");
                    //    var id = User.GetMaxId();

                    //    user = User.Create(id, account);
                    //    user.password = password;
                    //    //计算 token
                    //    Random r = new Random();
                    //    string tokenStr = user.id + user.account + TimeTool.GetTimeStamp() + r.Next(100000);
                    //    string token = EncryptionTool.GetMd5Str(tokenStr);
                    //    user.token = token;


                    //    DBMgr.redisOrg.HashSet("accountId", account, id);
                    //    isLoginSuccess = true;


                    //}

                    //test
                    user = new User()
                    {
                        id = 1,
                        account = "test0",
                        password = "123",
                        token = "123456"
                    };//, password

                    isLoginSuccess = true;

                    GC2LS.respAskLogin loginResult = new GC2LS.respAskLogin();
                    loginResult.IsSuccess = isLoginSuccess;

                    if (isLoginSuccess)
                    {
                        loginResult.GateServerIp = ConstInfo.GS_IP;
                        loginResult.GateServerPort = ConstInfo.GS_Client_Port;
                        loginResult.UserId = "" + user.id;
                        loginResult.UserAccount = user.account;

                        //this.currAccount = user.account;

                        //this.currSessionId = user.sessionId;

                        ////计算 token
                        //Random r = new Random();
                        //string tokenStr = user.id + user.account + TimeTool.GetTimeStamp() + r.Next(100000);
                        //string token = EncryptionTool.GetMd5Str(tokenStr);
                        loginResult.Token = "" + user.token;
                        //user.token = token;


                        Console.WriteLine("token : " + user.token);
                        Console.WriteLine("user online : " + user.account);

                        //用户成功登录
                        CSServer.Instance.UserOnline(user);
                        //CSServer.Instance.playerMgr.Create(user);
                        Console.WriteLine("when verify netId : " + gcNetId);
                        //user.SetSessionId(gcNetId);//这个是 登录中 的sessionId 所以不能用 currSessionId

                        //currPlayer = CSServer.Instance.GetPlayer(account);
                        //user.playerId = currPlayer.id;

                        //同步到数据库
                        //DBMgr.redis.Set("user:" + user.id, user);
                        user.Save();


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
            switch ((GC2CS.MsgId)msgId)
            {
                case GC2CS.MsgId.Gc2CsEnterGameService:

                    HandleEnterGame(gcNetId, data);
                    break;
                case GC2CS.MsgId.Gc2CsHeroList:

                    HandleHeroList(gcNetId, data);
                    break;
                    //case GC2CS.MsgId.Gc2CsAskStartNormalManagement:

                    //    break;
                    //case GC2CS.MsgId.Gc2CsAskStartOvertimeManagement:
                    //    StartOvertimelManagement(gcNetId, data);
                    //    break;
                    //case GC2CS.MsgId.Gc2CsStartMateCombat:
                    //    StartMateCombat(gcNetId, data);
                    //    break;

            }
        }

    }



    public void HandleEnterGame(int gcNetId, byte[] data)
    {
        GC2CS.reqEnterGameService enterGame = GC2CS.reqEnterGameService.Parser.ParseFrom(data);
        string account = enterGame.Account;

        GC2CS.respEnterGameService enterResult = new GC2CS.respEnterGameService();
        enterResult.IsSuccess = true;
        //this.currSessionId = gcNetId;
        //enterResult.UserInfo = new GS2GC.UserInfo()
        //{
        //    Account = "thanklzhang",
        //    NickName = "kaller",
        //    PortraitURL = "",
        //    //以下是 player 属性
        //    Level = 3,
        //    Coin = 450000
        //};
        //enterResult.ManagementInfo = new GS2GC.ManagementInfo()
        //{
        //    State = 0,
        //    LastStartTime = 0,
        //    LastFinishTime = 0,
        //    TotalUseBenchNum = 4//这里先固定值  之后会根据事实来更改
        //};

        //用户可以进行游戏了 这时候需要把 sessionId 赋值
        CSServer.Instance.GetUser(account).SetSessionId(gcNetId);


        Console.WriteLine("trans to GS : " + GC2CS.MsgId.Gc2CsEnterGameService.ToString());
        SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsEnterGameService, enterResult.ToByteArray());

        //Sync info
        //currPlayer.SyncPlayerBaseInfo();
        //currPlayer.SyncHeroListInfo();
        //currPlayer.SyncItemListInfo();
        //currPlayer.SyncDrawingListInfo();
        //currPlayer.SyncManagementInfo();

    }

    private void HandleHeroList(int gcNetId, byte[] data)
    {
        GC2CS.reqHeroList req = GC2CS.reqHeroList.Parser.ParseFrom(data);


        GC2CS.respHeroList resp = new GC2CS.respHeroList();
        resp.HeroList.Add(new GC2CS.HeroInfo() { Id = 1, Level = 2 });
        resp.HeroList.Add(new GC2CS.HeroInfo() { Id = 2, Level = 5 });

        SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsHeroList, resp.ToByteArray());
    }

    void SendToClient(int sessionId, int msgId, byte[] data)
    {
        //Console.WriteLine("send to client : " + currSessionId);
        CSServer.Instance.TransToGS(0, sessionId, msgId, data);
    }


    //public void GetPlayerInfo(int gcNetId, byte[] data)
    //{

    //}

    //public void GetManagementInfo(int gcNetId, byte[] data)
    //{

    //}

    //public void GetCanUseDrawings(int gcNetId, byte[] data)
    //{

    //}

    //public void StartNormalManagement(int gcNetId, byte[] data)
    //{
    //    var startManagement = GC2CS.AskStartNormalManagement.Parser.ParseFrom(data);
    //    var clientBenches = startManagement.WorkBenchList;
    //    List<SelectWorkBench> selectBenches = ConvertToSelectWorkBench(clientBenches.ToList());
    //    currPlayer.StartManagement(0, selectBenches);

    //}

    //public void StartOvertimelManagement(int gcNetId, byte[] data)
    //{
    //    Console.WriteLine("StartOvertimelManagement");
    //    var startManagement = GC2CS.AskStartNormalManagement.Parser.ParseFrom(data);

    //    //Test
    //    //List<SelectWorkBench> selectBenches = new List<SelectWorkBench>();
    //    //selectBenches.Add(SelectWorkBench.Create(0, 10000, new List<int>() { 400002, 400003 }));
    //    //selectBenches.Add(SelectWorkBench.Create(1, 10001, new List<int>() { 400001, 400004, 400005 }));
    //    //selectBenches.Add(SelectWorkBench.Create(2, 10002, new List<int>() { 400007 }));

    //    var clientBenches = startManagement.WorkBenchList;
    //    List<SelectWorkBench> selectBenches = ConvertToSelectWorkBench(clientBenches.ToList());


    //    currPlayer.StartManagement(1, selectBenches);
    //}

    //List<SelectWorkBench> ConvertToSelectWorkBench(List<GC2CS.WorkBench> clientBenches)
    //{
    //    //var clientBenches = startManagement.WorkBenchList;
    //    List<SelectWorkBench> selectBenches = new List<SelectWorkBench>();
    //    clientBenches.ToList().ForEach(cBench =>
    //    {
    //        List<SelectWorkProject> selectProjects = new List<SelectWorkProject>();
    //        cBench.WorkProjectList.ToList().ForEach(cProject =>
    //        {
    //            SelectDrawing selectDrawing = new SelectDrawing()
    //            {
    //                drawingSN = cProject.Drawing.DrawingSN
    //            };
    //            var selectProject = new SelectWorkProject()
    //            {
    //                drawing = selectDrawing
    //            };

    //            selectProjects.Add(selectProject);
    //        });
    //        SelectWorkBench selectWorkBench = new SelectWorkBench()
    //        {
    //            index = cBench.Index,
    //            workerId = cBench.WorkerId,
    //            workProjects = selectProjects
    //        };
    //        selectBenches.Add(selectWorkBench);
    //    });

    //    return selectBenches;
    //}

    //public void ReceiveManagementResult(int gcNetId, byte[] data)
    //{
    //    currPlayer.ReceiveManagementResult();
    //}

    //public void StartMateCombat(int gcNetId, byte[] data)
    //{
    //    Console.WriteLine("receive start mate request");
    //    GC2CS.AskStartMateCombat askMate = GC2CS.AskStartMateCombat.Parser.ParseFrom(data);
    //    if (currPlayer != null)
    //    {
    //        //这里 player 中的 ip 和 port 只是为了传递
    //        currPlayer.ip = askMate.Ip;
    //        currPlayer.udpPort = askMate.UdpPort;

    //        CSServer.Instance.PlayerStartMate(currPlayer);
    //    }

    //}

    //public void CreateCombatFinish(int gcNetId, byte[] data)
    //{
    //    Console.WriteLine("receive create combat finish");
    //    SS2CS.CreateCombatFinish createCombatFinish = SS2CS.CreateCombatFinish.Parser.ParseFrom(data);

    //    CSServer.Instance.CreateCombatFinish(createCombatFinish);
    //}
}

