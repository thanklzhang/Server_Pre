using DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class HeroListHandler : BaseHandler
    {
        public override void Handler(User user, int gcNetId, byte[] data)
        {
            GC2CS.reqHeroList req = GC2CS.reqHeroList.Parser.ParseFrom(data);

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
