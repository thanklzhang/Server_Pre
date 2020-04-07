using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OnlineUser
    {
        public int id;
        public string account;
        public string password;
        public string token;

        public int sessionId;
        //public int playerId;

        public static OnlineUser Create(int id, string account, string password, string token)
        {
            var user = new OnlineUser()
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

        //---------------------------------

        //public static int GetMaxId()
        //{
        //    int maxId = 1;
        //    if (DBMgr.redisOrg.HashExists("maxId", "userId"))
        //    {
        //        var preMaxId = int.Parse(DBMgr.redisOrg.HashGet("maxId", "userId"));
        //        maxId = preMaxId + 1;
        //    }
        //    DBMgr.redisOrg.HashSet("maxId", "userId", maxId);

        //    return maxId;
        //}

        //后期可能变成异步操作
        //public void Save()
        //{
        //    //var userId = "user:" + id;
        //    //DBMgr.redisOrg.HashSet(userId, "id", id);
        //    ////DBMgr.redisOrg.HashSet(playerId, "userAccount", userAccount);
        //    //DBMgr.redisOrg.HashSet(userId, "account", account);
        //    //DBMgr.redisOrg.HashSet(userId, "password", password);//先这么弄
        //    //DBMgr.redisOrg.HashSet(userId, "token", token);
        //    ////DBMgr.redisOrg.HashSet(userId, "sessionId", sessionId);//注意 第一次创建用户的时候 这里是无效的
        //    //DBMgr.redisOrg.HashSet(userId, "playerId", playerId);
        //    Console.WriteLine("user save ... : " + this.account);
        //}

        //internal static int GetMaxId()
        //{
        //    return 1;
        //}

        //public void Load(User user)
        //{
        //    this.id = user.id;
        //    this.account = user.account;
        //    this.password = user.password;
        //}

        //public void LoadFromDB()
        //{
        //    var user = DBMgr.redis.Get<User>("user:" + id);
        //    Load(user);

        //}

        //public void Save()
        //{
        //    DBMgr.redis.Set("user:" + id, this);
        //}

    }


}

