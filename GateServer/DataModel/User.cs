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
        

        public static User Create(int id, string account)
        {
            var user = new User()
            {
                id = id,
                account = account
            };
            return user;

        }

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

