using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MysqlConnect : SqlConnect
{
    //DbConnection conn;
    public override void Connect()
    {
        string connetStr = "server=127.0.0.1;port=3306;user=root;password=zhang425; database=kaller;";
        // server=127.0.0.1/localhost 代表本机，端口号port默认是3306可以不写
        conn = new MySqlConnection(connetStr);
        try
        {
            Console.WriteLine("mysql is connecting ...");
            conn.Open();
            Console.WriteLine("mysql connect success ");

        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }
    
}

