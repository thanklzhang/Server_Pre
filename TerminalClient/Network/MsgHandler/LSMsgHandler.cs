using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using TerminalClient;

public class LSMsgHandler //: MsgHandler
{

    public void HandleMsg(int gcNetId, int msgId, byte[] data)
    {
        if (msgId > (int)GS2GC.MsgId.Begin && msgId < (int)GS2GC.MsgId.End)
        {
            switch ((GS2GC.MsgId)msgId)
            {
                case GS2GC.MsgId.Gs2GcVerificationAskLoginResult:
                    VerifyLoginResult(gcNetId, msgId, data);
                    break;
                    //case GC2LS.MsgId.Gc2LsAskLogin:
                    //    VerifyLogin(gcNetId, data);
                    //    break;
                    //case GC2LS.MsgId.Gc2LsAskLogin:
                    //    VerifyLogin(gcNetId, data);
                    //    break;
            }


        }
    }

    /// <summary>
    /// 验证登陆
    /// </summary>
    /// <param name="gcNetId"></param>
    /// <param name="data"></param>
    public void VerifyLoginResult(int gcNetId, int msgId, byte[] data)
    {
        
        GS2GC.VerificationAskLoginResult loginResult = GS2GC.VerificationAskLoginResult.Parser.ParseFrom(data);
        Console.WriteLine("login " + loginResult.IsSuccess);
        if (loginResult.IsSuccess)
        {
           
            //连接 GS 服务器
            ClientConnect.Instance.ConnectGSServer(() =>
            {
               

                GC2CS.AskEnterGameService enter = new GC2CS.AskEnterGameService();
                enter.Account = loginResult.UserAccount;
                enter.Token = loginResult.Token;
                Console.WriteLine("send enter game");
                
                ClientConnect.Instance.SendTo(SessionType.Client_TMN_OnlyGS, 0, 0, (int)GC2CS.MsgId.Gc2CsAskEnterGameService, enter.ToByteArray());
            });
        }


        //Console.WriteLine("on gate : ask login");

        //GC2LS.AskLogin askLogin = GC2LS.AskLogin.Parser.ParseFrom(data);
        //string account = askLogin.Account;
        //string password = askLogin.Password;




        // GSServer.Instance.TransToCS(0, gcNetId, msgId, data);


        //Console.WriteLine("client login info : " + account + " " + password);

        //bool isLoginSuccess = false;
        //var user = UserDBOp.CheckUser(account);//, password

        //if (user != null)
        //{
        //    Console.WriteLine("server login info : " + user.account + " " + user.password);
        //    if (password == user.password)
        //    {
        //        isLoginSuccess = true;
        //    }
        //}

        //GS2GC.VerificationAskLoginResult loginResult = new GS2GC.VerificationAskLoginResult();
        //loginResult.IsSuccess = isLoginSuccess;
        //loginResult.GateServerIp = ConstInfo.GS_IP;
        //loginResult.GateServerPort = ConstInfo.GS_Client_Port;

        //GSServer.Instance.TransToLS(0, gcNetId, (int)GS2GC.MsgId.Gs2GcVerificationAskLoginResult, loginResult.ToByteArray());
    }
}

