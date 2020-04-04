using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GCMsgHandler// : MsgHandler
{

    public void HandleMsg(int gcNetId, int msgId, byte[] data)
    {


        if (msgId > (int)GC2CS.MsgId.Begin && msgId < (int)GC2CS.MsgId.End)
        {
            //这个特殊 需要验证 token
            if ((GC2CS.MsgId)(msgId) == GC2CS.MsgId.Gc2CsEnterGameService)
            {

                GC2CS.reqEnterGameService result = GC2CS.reqEnterGameService.Parser.ParseFrom(data);

                var account = result.Account;
                var token = result.Token;

                //bool isSuccess = GSServer.Instance.VerityToken(account, token);
                bool isSuccess = true;//test


                //向客户端发送验证 token 的结果
                GC2CS.respEnterGameService veToken = new GC2CS.respEnterGameService();
                veToken.Err = ResultCode.Success;
                veToken.IsSuccess = isSuccess;
                GSServer.Instance.SendMsgToClient(gcNetId, msgId, data);


                Console.WriteLine("verify token result: " + isSuccess);
                if (isSuccess)
                {
                    //记录 account 到 session 中
                    GSServer.Instance.SetSessionAccount(gcNetId, account);

                    Console.WriteLine("verify success : " + account);

                    Console.WriteLine("send to CS : " + (GC2CS.MsgId)msgId);
                    GSServer.Instance.TransToCS(0, gcNetId, msgId, data);
                }

            }
            else
            {
                Console.WriteLine("trans to CS ..." + " msgId : " + msgId + " netId : " + gcNetId);
                GSServer.Instance.TransToCS(0, gcNetId, msgId, data);
            }






        }


    }


}

