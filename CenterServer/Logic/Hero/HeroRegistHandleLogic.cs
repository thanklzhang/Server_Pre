using DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class HeroRegistHandleLogic
    {
        public void Regist(Dictionary<int, BaseHandler> dic)
        {
            dic.Add((int)GC2CS.MsgId.Gc2CsHeroList, new HeroListHandler());
            dic.Add((int)GC2CS.MsgId.Gc2CsAddHeroLevel, new AddHeroLevelHandler());
            dic.Add((int)GC2CS.MsgId.Gc2CsNotifyUpdateHeroes, new NotifyUpdateHeroesHandler());
        }
    }

}
