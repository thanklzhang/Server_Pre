using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GCMsgHandler //: MsgHandler
{

    public void HandlMsg(int nsId, int msgId, byte[] data)
    {
        //这里过滤一下 如果是 LS 登录等消息 则特殊对待 否则统一转发
        if (msgId > (int)GC2LS.MsgId.Begin && msgId < (int)GC2LS.MsgId.End)
        {
            //登录相关
            if ((GC2LS.MsgId)msgId == GC2LS.MsgId.Gc2LsAskLogin)
            {
                Console.WriteLine("ask login");

                GC2LS.reqAskLogin askLogin = GC2LS.reqAskLogin.Parser.ParseFrom(data);
                string account = askLogin.Account;
                string password = askLogin.Password;

                Console.WriteLine("LS : client login info : " + account + " " + password);

                //转发给 GS
                int infoId = 0;
                LSServer.Instance.TransToGS(infoId, nsId, msgId, data);

                //目前一个请求上行和下行协议都用一个

                //test
                //GC2LS.respAskLogin resp = new GC2LS.respAskLogin()
                //{
                //    Err = ResultCode.Success,
                //    UserId = "1",
                //    UserAccount = "zxy",
                //    GateServerIp = "127.0.0.1",
                //    GateServerPort = ConstInfo.GS_Client_Port,
                //    IsSuccess = true,
                //    Token = "EEEEEE"

                //};
                //LSServer.Instance.SendMsgToClient(nsId, (int)GC2LS.MsgId.Gc2LsAskLogin, resp.ToByteArray());

            }
        }


    }


}

