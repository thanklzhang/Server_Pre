using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CSMsgHandler// : MsgHandler
{

    public void HandleMsg(int gcNetId, int msgId, byte[] data)
    {
        //这里是 CS 发过来的 用的都是一个协议
        if (msgId > (int)GC2LS.MsgId.Begin && msgId < (int)GC2LS.MsgId.End)
        {


            if ((GC2LS.MsgId)(msgId) == GC2LS.MsgId.Gc2LsAskLogin)
            {

                GC2LS.respAskLogin result = GC2LS.respAskLogin.Parser.ParseFrom(data);


                Console.WriteLine("verify result from CS : " + result.IsSuccess);
                //将成功登录的信息进行注册 之后在这里进行验证
                if (result.IsSuccess)
                {
                    int id = int.Parse(result.UserId);
                    string account = result.UserAccount;
                    string token = result.Token;
                    GSServer.Instance.AddToken(account, id, token);
                }


                //这个是将要发送到 LS 的消息 特殊对待()
                GSServer.Instance.TransToLS(0, gcNetId, msgId, data);

            }
            else
            {

                Console.WriteLine("send to client : " + (GS2GC.MsgId)msgId);
                GSServer.Instance.SendMsgToClient(gcNetId, msgId, data);
            }

        }

        //客户端请求的回应消息  也可以是服务器推送的消息
        if (msgId > (int)GC2CS.MsgId.Begin && msgId < (int)GC2CS.MsgId.End)
        {
            GSServer.Instance.SendMsgToClient(gcNetId, msgId, data);
        }

        ////转发战斗消息
        //if (msgId > (int)GS2SS.MsgId.Begin && msgId < (int)GS2SS.MsgId.End)
        //{

        //    GSServer.Instance.TransToSS(0, gcNetId, msgId, data);

        //}
    }


}

