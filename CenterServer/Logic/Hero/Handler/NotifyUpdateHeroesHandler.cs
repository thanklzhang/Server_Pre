using DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class NotifyUpdateHeroesHandler : BaseHandler
    {
        public override void Handler(User user, int gcNetId, byte[] data)
        {
            base.Handler(user, gcNetId, data);
        }
    }

}
