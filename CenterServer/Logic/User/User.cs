using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class User
    {
        public int id;
        public string account;
        public string password;
        public string token;

        public int sessionId;
        //public int playerId;

        public bool isLoadedData;//是否加载了所有用户相关的数据

        Dictionary<int, Hero> heroDic = new Dictionary<int, Hero>();

        public static User Create(int id, string account, string password, string token)
        {
            var user = new User()
            {
                id = id,
                account = account,
                password = password,
                token = token
            };
            return user;

        }

        internal void SetSessionId(int gcNetId)
        {
            sessionId = gcNetId;
        }

        /// <summary>
        /// 放入一个英雄数据
        /// </summary>
        /// <param name="heroDataList"></param>
        internal void PutHero(Hero hero)
        {
            if (heroDic.ContainsKey(hero.data.id))
            {
                Console.WriteLine("put hero : the key is exist , id : " + hero.data.id);
                //Console.WriteLine("put hero : the key is exist , will cover  , id : " + hero.data.id);

                //heroDic.Remove(hero.data.id);

                return;
            }

            heroDic.Add(hero.data.id, hero);

        }

        public List<Hero> GetOwnHeroes()
        {
            return heroDic.Select(pair => pair.Value).ToList();
        }

        public Hero GetOwnHero(int id)
        {
            if (heroDic.ContainsKey(id))
            {
                return heroDic[id];
            }

            return null;
        }

        /// <summary>
        /// 给该用户发送消息
        /// </summary>
        public void SendMsg(int msgId, byte[] data)
        {
            CSServer.Instance.TransToGS(0, this.sessionId, msgId, data);
        }
    }


}

