using DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class EnterGameServiceHandler : BaseHandler
    {

        public override void Handler(User user, int gcNetId, byte[] data)
        {
            GC2CS.reqEnterGameService enterGame = GC2CS.reqEnterGameService.Parser.ParseFrom(data);
            string account = enterGame.Account;

            GC2CS.respEnterGameService enterResult = new GC2CS.respEnterGameService();
            enterResult.IsSuccess = true;
            enterResult.Err = ResultCode.Success;
            //this.currSessionId = gcNetId;

            //用户可以进行游戏了 这时候需要把 sessionId 赋值
            UserMgr.Instance.GetUser(account).SetSessionId(gcNetId);
            //CSServer.Instance.GetUser(account).SetSessionId(gcNetId);


            Console.WriteLine("trans to GS : " + GC2CS.MsgId.Gc2CsEnterGameService.ToString());
            SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsEnterGameService, enterResult.ToByteArray());

            //推送一些需要的数据
            SyncHeroList(user, gcNetId);
        }

        void SyncHeroList(User user, int gcNetId)
        {
            //之后会有数据层和网络数据层的装换
            GC2CS.respHeroList resp = new GC2CS.respHeroList();
            var heroList = user.GetOwnHeroes();
            for (int i = 0; i < heroList.Count; ++i)
            {
                var ownHero = heroList[i];
                var heroInfo = new GC2CS.HeroInfo()
                {
                    Id = ownHero.data.id,
                    Level = ownHero.data.level,
                    ConfigId = ownHero.config.id

                };
                resp.HeroList.Add(heroInfo);
            }
            SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsHeroList, resp.ToByteArray());
        }


    }

}
