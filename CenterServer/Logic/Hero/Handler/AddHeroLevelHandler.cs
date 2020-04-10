using DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class AddHeroLevelHandler : BaseHandler
    {
        public override void Handler(User user, int gcNetId, byte[] data)
        {
            GC2CS.reqAddHeroLevel req = GC2CS.reqAddHeroLevel.Parser.ParseFrom(data);
            GC2CS.respAddHeroLevel resp = new GC2CS.respAddHeroLevel();
            var hero = user.GetOwnHero(req.HeroId);
            if (hero != null)
            {
                hero.AddLevel(1);

                //如果过多 那么包会过大 那么需要分包 HERO_MAX_SIZE_IN_MSG
                HeroMgr.Instance.NotifyUpdateHeroes(user, new List<Hero>() { hero });

                resp.Err = ResultCode.Success;
                SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsAddHeroLevel, resp.ToByteArray());
            }
            else
            {
                resp.Err = ResultCode.ErrUnknown;
                SendToClient(gcNetId, (int)GC2CS.MsgId.Gc2CsAddHeroLevel, resp.ToByteArray());
            }
        }
    }

}
