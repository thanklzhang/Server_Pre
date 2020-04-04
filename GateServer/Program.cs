using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


class Program
{
    [SecurityCritical]
    static void Main(string[] args)
    {
        //string md5r = EncryptionTool.EncryptionByMd5("zhangxy468484135165");
        //var b = Encoding.UTF8.GetBytes(md5r); 
        //Console.WriteLine(b.Length);

        //DBMgr.Init();

        //var user = UserDBOp.CheckUser("thanklzhang");
        //if (user != null)
        //{
        //    Console.WriteLine("have user " + "thanklzhang");
        //    if (user.password == "123")
        //    {
        //        Console.WriteLine("login success " + user.id + " " + user.account + " " + user.password);
        //    }
        //    else
        //    {
        //        Console.WriteLine("login fail " + user.id + " " + user.account + " " + user.password);
        //    }


        //}
        //else
        //{
        //    Console.WriteLine("no user " + "thanklzhang");
        //    Console.WriteLine("start to create thanklzhang ...");

        //    User u = User.Create(1, "thanklzhang");
        //    u.password = "123";
        //    DBMgr.redis.Set("user:" + 1, u);
        //    DBMgr.redisOrg.HashSet("AccountId", u.account, u.id);



        //}

        //return;

        //开启监听 用来监听用户连接
        GSServer gateServer = new GSServer();

        //自身作为服务器启动
        gateServer.Start();

        while (true)
        {
            Thread.Sleep(1);
        }

    }
}


//public class User
//{
//    public int id; //自动生成
//    public string account;
//    public string password;
//    public int token;
//}
//public class DBSimulate
//{
//    private static DBSimulate instance;
//    public static DBSimulate Instance
//    {
//        get
//        {
//            if (null == instance)
//            {
//                instance = new DBSimulate();
//            }

//            return instance;
//        }
//    }
//    Dictionary<string, User> userDic = new Dictionary<string, User>();


//    public void Init()
//    {

//    }

//    public void AddNewUser(string account, string password)
//    {
//        User u = new User();
//        //u.id = id;
//        u.account = account;
//        u.password = password;

//        u.token = u.id;//目前先用这个
//    }

//    public User FindUser(string account, string password)
//    {
//        var user = userDic.Where(u => u.Key == account && u.Value.password == password);
//        if (user.Count() > 0)
//        {
//            return user.FirstOrDefault().Value;
//        }
//        else
//        {
//            return null;
//        }
//    }

//}

