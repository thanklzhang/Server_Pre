using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SqlConnectPool
{
    //之后是真正的池子

    List<SqlConnect> connects = new List<SqlConnect>();

    public void AddConnect(SqlConnect connect)
    {
        connects.Add(connect);
        connect.Connect();
    }

    public DbConnection GetConnect()
    {
        SqlConnect connect = null;
        //if (0 == connects.Count)
        //{
        //    connect = new SqlConnect();
        //    connect.Connect();
        //    connects.Add(connect);
        //}
        //else
        //{
        //    connect = connects[0];
        //}

        if (connects.Count > 0)
        {
            connect = connects[0];
        }
        else
        {
            Console.WriteLine("the connect can use is empty");
        }
     

        return connect.ConnectMeta();
    }

}

