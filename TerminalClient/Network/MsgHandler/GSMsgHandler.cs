using GC2CS;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerminalClient;

public class GSMsgHandler //: MsgHandler
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
                case GS2GC.MsgId.Gs2GcFromCsAskEnterGameServiceResult:
                    EnterGameResult(gcNetId, msgId, data);
                    break;
                case GS2GC.MsgId.Gs2GcFromCsPostManagementResult:
                    ManagementResult(gcNetId, msgId, data);
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

    private void VerifyLoginResult(int gcNetId, int msgId, byte[] data)
    {

        //GS2GC.VerificationAskLoginResult loginResult = GS2GC.VerificationAskLoginResult.Parser.ParseFrom(data);
        //Console.WriteLine("login " + loginResult.IsSuccess);
        //if (loginResult.IsSuccess)
        //{
        //    Console.WriteLine("start enter game");
        //    //连接 GS 服务器
        //    ClientConnect.Instance.ConnectGSServer(() =>
        //    {
        //        GC2CS.AskEnterGameService enter = new GC2CS.AskEnterGameService();
        //        enter.Account = loginResult.UserAccount;
        //        enter.Token = loginResult.Token;
        //        Console.WriteLine("ask enter game");
        //        ClientConnect.Instance.SendTo(SessionType.Client_TMN_OnlyGS, 0, 0, (int)GC2CS.MsgId.Gc2CsAskEnterGameService, enter.ToByteArray());
        //    });
        //}
        // LSServer.Instance.SendMsgToClient(gcNetId, msgId, data);
    }

    private void EnterGameResult(int gcNetId, int msgId, byte[] data)
    {
        GS2GC.AskEnterGameServiceResult enterResult = GS2GC.AskEnterGameServiceResult.Parser.ParseFrom(data);
        Console.WriteLine("enterGame " + enterResult.IsSuccess);


        for (int i = 0; i < 30;++i)
        {
            var startoverTime = new GC2CS.AskStartOvertimeManagement();
            WorkBench b = new WorkBench()
            {
                Index = 0,
                WorkerId = 1,
            };
            WorkProject project = new WorkProject()
            {
                Drawing = new Drawing()
                {
                    DrawingSN = 400000
                }
            };
            WorkProject project2 = new WorkProject()
            {
                Drawing = new Drawing()
                {
                    DrawingSN = 400001
                }
            };
            WorkProject project3 = new WorkProject()
            {
                Drawing = new Drawing()
                {
                    DrawingSN = 400002
                }
            };
            b.WorkProjectList.Add(project);
            b.WorkProjectList.Add(project2);
            b.WorkProjectList.Add(project3);
            startoverTime.WorkBenchList.Add(b);

            ClientConnect.Instance.SendTo(SessionType.Client_TMN_OnlyGS, 0, 0, (int)GC2CS.MsgId.Gc2CsAskStartOvertimeManagement, startoverTime.ToByteArray());

            Thread.Sleep(150);
        }

     

    }

    private void ManagementResult(int gcNetId, int msgId, byte[] data)
    {
        GS2GC.PostManagementResult result = GS2GC.PostManagementResult.Parser.ParseFrom(data);

        result.ItemList.ToList().ForEach(item =>
        {
            Console.WriteLine("itemId : " + item.Id);
        });
    }
}

