using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//目前使用 mysql
public class SqlConnect
{
    protected DbConnection conn;

    public virtual void Connect()
    {

    }
    
    public DbConnection ConnectMeta()
    {
        return conn;
    }
    public void Close()
    {
        conn?.Close();
    }
}

