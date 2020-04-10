using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class MysqlOperator<T> : ISqlOperator<T> where T : DBEntity, new()
{
    SqlConnectPool pool;
    DBEntityMeta<T> meta;
    public string tableName;

    public List<T> newObjList = new List<T>();
    public List<T> changeObjList = new List<T>();

    public void SetConnectPool(SqlConnectPool pool)
    {
        this.pool = pool;
    }

    public void SetDBEntityMeta(DBEntityMeta<T> meta)
    {
        this.meta = meta;
        tableName = typeof(T).ToString();
        Console.WriteLine("query table : " + tableName);

        CheckDB();
    }

    /// <summary>
    /// 检查数据库结构
    /// </summary>
    public void CheckDB()
    {
        var fieldList = this.meta.GetFields();

        //检查是否有该表
        string isHaveTableSql = "SHOW TABLES LIKE '" + tableName + "';";
        var checkTable = ExecuteDataTable(isHaveTableSql);
        if (checkTable.Rows.Count > 0)
        {
            //有表

            string showColsSql = "SHOW COLUMNS FROM " + tableName + ";";

            //查找表中所有字段
            var colsDataTable = ExecuteDataTable(showColsSql);

            var rows = colsDataTable.Rows;
            var cols = colsDataTable.Columns;
            List<string> sqlCols = new List<string>();
            foreach (DataRow row in rows)
            {
                var fieldName = row.Field<string>("Field");
                var type = row.Field<string>("Type");

                sqlCols.Add(fieldName);
            }

            //获取需要增加的字段
            string addSql = "ALTER TABLE " + tableName + " ";

            bool isHaveAddField = false;

            for (int i = 0; i < fieldList.Count; i++)
            {
                var item = fieldList[i];

                if (item.isIgnoreSerialize)
                {
                    continue;
                }
                string fieldName = item.field.Name;
                var isExistField = sqlCols.Exists(c => c == fieldName);
                if (!isExistField)
                {
                    isHaveAddField = true;

                    var typeStr = CommonFunc.GetFieldSqlType(item.field);

                    if (0 == i)
                    {
                        addSql += " ADD " + fieldName + " " + typeStr + " FIRST ,";
                    }
                    else
                    {
                        addSql += " ADD " + fieldName + " " + typeStr + " AFTER " + fieldList[i - 1].field.Name + " ,";
                    }
                }
            }

            addSql = addSql.Substring(0, addSql.Length - 1);

            //将要删除的字段
            string delSql = "ALTER TABLE " + tableName + " ";
            var isHaveDelField = false;
            foreach (var colName in sqlCols)
            {
                var isWillDel = !fieldList.Exists(c => c.field.Name == colName);
                if (isWillDel)
                {
                    delSql += " DROP " + colName + " , ";
                    isHaveDelField = true;
                }
            }

            delSql = delSql.Substring(0, delSql.Length - 1);

            //执行
            int result = 0;
            if (isHaveAddField)
            {
                addSql = addSql.Substring(0, addSql.Length - 1);

                Console.WriteLine("add sql : " + addSql);
                result = ExecuteNonQuery(addSql);
            }
            if (isHaveDelField)
            {
                delSql = delSql.Substring(0, delSql.Length - 1);

                Console.WriteLine("del sql : " + delSql);
                result = ExecuteNonQuery(delSql);
            }
        }
        else
        {
            string createTableSql = "CREATE TABLE " + tableName + " ( ";

            List<string> primaryKeyStrs = new List<string>();
            for (int i = 0; i < fieldList.Count; ++i)
            {
                var currField = fieldList[i];
                var typeStr = CommonFunc.GetFieldSqlType(currField.field);

                if (currField.isIgnoreSerialize)
                {
                    continue;
                }

                //需要判断是否是主键  目前默认主键都是递增的
                if (currField.isPrimaryKey)
                {
                    primaryKeyStrs.Add(currField.field.Name);
                    createTableSql += currField.field.Name + " " + typeStr + " ,";// AUTO_INCREMENT
                }
                else
                {
                    createTableSql += currField.field.Name + " " + typeStr + " ,";
                }
            }

            createTableSql = createTableSql.Substring(0, createTableSql.Length - 1);
            if (primaryKeyStrs.Count > 0)
            {
                createTableSql += " , primary key ( ";
                for (int i = 0; i < primaryKeyStrs.Count; i++)
                {
                    var priKeyStr = primaryKeyStrs[i];
                    createTableSql += priKeyStr + ",";
                }
                createTableSql = createTableSql.Substring(0, createTableSql.Length - 1);

                createTableSql += ") ";
            }

            createTableSql += ");";

            Console.WriteLine("create table sql : " + createTableSql);
            var result = ExecuteNonQuery(createTableSql);
        }
    }
    
    public T Find(string[] fieldNames, string param)
    {
        var list = FindAll(fieldNames, param);
        if (list != null && list.Count > 0)
        {
            return list[0];
        }

        return null;

    }

    public List<T> FindAll(string[] fieldNames, string param)
    {
      
        string fields = "";
        if (null == fieldNames || 0 == fieldNames.Length)
        {
            fields = "*";
        }
        else
        {
          
            for (int i = 0; i < fieldNames.Length; i++)
            {
                var currField = fieldNames[i];
                fields += currField + ",";
            }

            fields = fields.Substring(0, fields.Length - 1);
            
        }
      
        string tableName = this.tableName;

        //List<int> lo = null;
        //lo.Add(1);

        string sql = string.Format("select {0} from {1} where {2};", fields, tableName, param);

        Console.WriteLine("sql : " + sql);

        var conn = pool.GetConnect();
        var table = ExecuteDataTable(sql);

        var rows = table.Rows;
        var cols = table.Columns;
        List<T> dataList = new List<T>();
       
        foreach (DataRow item in rows)
        {
            T data = new T();
            for (int i = 0; i < cols.Count; i++)
            {
                var col = cols[i];

                //find by reflect
                //item.Field<int>(col.ColumnName);
                var classField = meta.GetField(col.ColumnName);
              
                if (classField != null)
                {
                    var type = classField.field.FieldType;
                   
                    if (type == TypeHelper.intType)
                    {
                        var value = item.Field<int>(col.ColumnName);
                        classField.field.SetValue(data, value);
                    }

                    if (type == TypeHelper.stringType)
                    {
                        var value = item.Field<string>(col.ColumnName);
                        classField.field.SetValue(data, value);
                    }
                 
                }
            }
            dataList.Add(data);
        }
      
        return dataList;
    }

    public void Save(T obj, bool isImmediate = false)
    {
        if (isImmediate)
        {
            SaveImmediate(obj);
        }
        else
        {
            if (0 == obj.version)
            {
                newObjList.Add(obj);
            }
        }
    }


    public void SaveImmediate(T obj)
    {
        if (0 == obj.version)
        {
            InsertImmediate(obj);
        }
        else
        {
            Update(obj);
        }
    }

    public void InsertImmediate(T obj)
    {
        string sql = "INSERT INTO " + this.tableName + " ( ";
        var fields = this.meta.GetFields();
        
        for (int i = 0; i < fields.Count; i++)
        {
            var currField = fields[i];
            if (currField.isIgnoreSerialize)
            {
                continue;
            }

            //if ("id" == currField.field.Name)
            //{
            //    continue;
            //}


            sql += currField.field.Name + ",";
        }

        sql = sql.Substring(0, sql.Length - 1) + ") VALUES ( ";

        for (int i = 0; i < fields.Count; i++)
        {
            var currField = fields[i];
            if (currField.isIgnoreSerialize)
            {
                continue;
            }
            //if ("id" == currField.field.Name)
            //{
            //    continue;
            //}

            var value = currField.field.GetValue(obj);
            string valueStr = "";
            if (currField.field.FieldType == TypeHelper.intType)
            {
                valueStr = value != null ? value.ToString() : "0";
                sql += valueStr + ",";
            }
            else
            if (currField.field.FieldType == TypeHelper.stringType)
            {
                valueStr = value != null ? "'" + value.ToString() + "'" : "''";
                sql += valueStr + ",";
            }

            //var type = SqlBuilder.GetFieldSqlType(currField.field);
        }

        sql = sql.Substring(0, sql.Length - 1) + ");";
        Console.WriteLine("insert sql : " + sql);
        ExecuteNonQuery(sql);
    }

    public void Update(T obj, bool isImmediate = false)
    {
        if (isImmediate)
        {
            UpdateImmediate(obj);
        }
        else
        {
            changeObjList.Add(obj);
        }
    }

    //这里是按照收集变化的字段进行处理 如果是传字段  可能会导致过多的装箱拆箱问题
    public void UpdateImmediate(T obj)//{string,object } {string,object } {string,object } ...
    {
        string sql = "UPDATE " + this.tableName + " SET ";
        var changeFieldNames = obj.CollectChangeFields();

        if (changeFieldNames.Count > 0)
        {
            for (int i = 0; i < changeFieldNames.Count; i++)
            {
                var currFieldName = changeFieldNames[i];
                var currField = this.meta.GetField(currFieldName);

                var value = currField.field.GetValue(obj);
                sql += currField.field.Name + " = " +
                    CommonFunc.GetValueStrSafe(obj, currField.field) + ",";
            }
            sql = sql.Substring(0, sql.Length - 1);

            var pris = this.meta.GetPrimaryKeyList();


            if (pris.Count > 0)
            {
                var pri = pris[0];//先用一个主键
                string valueStr = CommonFunc.GetValueStrSafe(obj, pri.field);
                sql += " WHERE " + pris[0].field.Name + " = " + valueStr + " ; ";

                Console.WriteLine("update sql : " + sql);
                ExecuteNonQuery(sql);
                obj.ClearChangeFields();
            }
            else
            {
                throw new Exception("the count of primary key is 0");
            }
        }
        else
        {
            Console.WriteLine("update sql , but no update : " + this.tableName);
        }
    }

    public void NotifyChangeFields(T changeObj, string[] fieldNames)
    {
        if (!changeObjList.Contains(changeObj))
        {
            changeObjList.Add(changeObj);
        }
        changeObj.NotifyChangeFields(fieldNames);
    }



    public void AutoSave()
    {
        //这里之后会变成批量处理

        Console.WriteLine("start auto save ... ");
        //new
        for (int i = 0; i < newObjList.Count; ++i)
        {
            var newObj = newObjList[i];
            InsertImmediate(newObj);
        }
        newObjList.Clear();

        //change
        for (int i = 0; i < changeObjList.Count; ++i)
        {
            var changeObj = changeObjList[i];
            UpdateImmediate(changeObj);
        }
        changeObjList.Clear();

        Console.WriteLine("finish auto save");
    }



    /// <summary>
    /// 非查询操作
    /// </summary>
    /// <param name="cmdText"></param>
    /// <param name="cmdType"></param>
    /// <param name="cmdParms"></param>
    /// <returns></returns>
    public int ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.Text, params MySqlParameter[] cmdParms)
    {
        int errorCode = 0;
        try
        {
            MySqlConnection conn = (MySqlConnection)pool.GetConnect();
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            errorCode = 1;
        }

        return errorCode;
    }

    public static MySqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
    {
        MySqlCommand cmd = new MySqlCommand();
        MySqlConnection conn = new MySqlConnection(connectionString);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return reader;
        }
        catch
        {
            cmd.Dispose();
            conn.Close();
            throw;
        }
    }

    public DataTable ExecuteDataTable(string cmdText, CommandType cmdType = CommandType.Text, params MySqlParameter[] cmdParms)
    {
        MySqlCommand cmd = new MySqlCommand();
        MySqlConnection conn = (MySqlConnection)pool.GetConnect();
        DataTable dt = null;

        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        adapter.SelectCommand = cmd;
        dt = new DataTable();
        adapter.Fill(dt);
        cmd.Parameters.Clear();

        cmd.Dispose();
        conn.Close();
        conn.Dispose();

        return dt;
    }


    private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
    {
        if (conn.State != ConnectionState.Open)
            conn.Open();

        cmd.Connection = conn;
        cmd.CommandText = cmdText;

        if (trans != null)
            cmd.Transaction = trans;

        cmd.CommandType = cmdType;

        if (cmdParms != null)
        {
            foreach (MySqlParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }


}

