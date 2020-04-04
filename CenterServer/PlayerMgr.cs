//using DataModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public class PlayerMgr
//{
//    Dictionary<int, Player> guidPlayerDic = new Dictionary<int, Player>();
//    Dictionary<string, Player> accountPlayerDic = new Dictionary<string, Player>();

//    internal void Create(User user)
//    {
//        //从数据库中读取

//        //目前先新创建
//        Player player = Player.Create(user);
      
//        guidPlayerDic.Add(player.id, player);

//        accountPlayerDic.Add(user.account, player);

//    }

//    internal void RemovePlayer(string account)
//    {
//        if (accountPlayerDic.ContainsKey(account))
//        {
//            var guid = accountPlayerDic[account].id;

//            accountPlayerDic.Remove(account);
//            guidPlayerDic.Remove(guid);
//        }
//    }

//    internal Player GetPlayer(string account)
//    {
//        if (accountPlayerDic.ContainsKey(account))
//        {
//            var guid = accountPlayerDic[account].id;

//            if (guidPlayerDic.ContainsKey(guid))
//            {
//                return guidPlayerDic[guid];
//            }

//        }

//        return null;
//    }
//    //Dictionary<int, User> idUserDic = new Dictionary<int, User>();
//    //Dictionary<int, string> idTokenDic = new Dictionary<int, string>();

//    //public void UserOnline(User u)
//    //{
//    //    accountUserDic.Add(u.account, u);
//    //    // idUserDic.Add(u.id, u);
//    //}

//    //public void UserExit(string account)
//    //{
//    //    if (accountUserDic.ContainsKey(account))
//    //    {
//    //        accountUserDic.Remove(account);
//    //    }

//    //}

//    //public void UserExit(int id)
//    //{
//    //    if (idUserDic.ContainsKey(id))
//    //    {
//    //        idUserDic.Remove(id);
//    //    }

//    //}
//}

