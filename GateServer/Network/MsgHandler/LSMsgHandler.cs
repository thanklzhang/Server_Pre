using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
public class LSMsgHandler //: MsgHandler
{

    public void HandleMsg(int gcNetId, int msgId, byte[] data)
    {
        if (msgId > (int)GC2LS.MsgId.Begin && msgId < (int)GC2LS.MsgId.End)
        {
            switch ((GC2LS.MsgId)msgId)
            {
                case GC2LS.MsgId.Gc2LsAskLogin:
                    VerifyLogin(gcNetId, msgId, data);
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
    public void VerifyLogin(int gcNetId, int msgId, byte[] data)
    {
        Console.WriteLine("on gate : ask login");

        //GC2LS.reqAskLogin askLogin = GC2LS.reqAskLogin.Parser.ParseFrom(data);
        //string account = askLogin.Account;
        //string password = askLogin.Password;

        GSServer.Instance.TransToCS(0, gcNetId, msgId, data);


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

