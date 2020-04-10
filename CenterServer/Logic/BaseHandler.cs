using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class BaseHandler
    {
        public virtual void Handler(User user, int gcNetId, byte[] data)
        {

        }

        public void SendToClient(int sessionId, int msgId, byte[] data)
        {
            //Console.WriteLine("send to client : " + currSessionId);
            CSServer.Instance.TransToGS(0, sessionId, msgId, data);
        }

    }

}
