using DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//业务层 handlerMsg
namespace Logic
{
    public class HandlerLogicMgr
    {
        private static HandlerLogicMgr instance;

        public static HandlerLogicMgr Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new HandlerLogicMgr();
                }

                return instance;
            }
        }

        public Dictionary<int, BaseHandler> dic = new Dictionary<int, BaseHandler>();

        public void Init()
        {
            new HeroRegistHandleLogic().Regist(dic);
            new UserRegistHandleLogic().Regist(dic);
        }

        public BaseHandler Get(int msgId)
        {
            if (!dic.ContainsKey(msgId))
            {
                Console.WriteLine("the msgId is not exist : " + msgId);
                return null;
            }

            return dic[msgId];

        }
        
    }

}

