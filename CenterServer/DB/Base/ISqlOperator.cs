using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISqlOperator<T> where T : DBEntity
{
    T Find(string[] fieldNames, string param);

    List<T> FindAll(string[] fieldNames, string param);

    void SetDBEntityMeta(DBEntityMeta<T> meta);
    void AutoSave();
    void Save(T obj, bool isImmediate = false);
    //void Update(T obj);
    //void InsertImmediate(T obj);
    void Update(T obj, bool isImmediate = false);

    void NotifyChangeFields(T changeObj, string[] fieldNames);
}

