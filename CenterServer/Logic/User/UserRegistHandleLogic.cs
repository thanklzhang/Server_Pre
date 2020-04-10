using DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class UserRegistHandleLogic
    {
        public void Regist(Dictionary<int, BaseHandler> dic)
        {
            dic.Add((int)GC2CS.MsgId.Gc2CsEnterGameService, new EnterGameServiceHandler());

        }
    }

}
