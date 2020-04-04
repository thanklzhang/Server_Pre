using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DBEntityMgr<T> where T : DBEntity, new()
{
    ISqlOperator<T> operation;

    public List<T> FindAll(string[] fieldNames, string param)
    {
        return operation.FindAll(fieldNames, param);
    }

    public void Save(T obj, bool immediate = false)
    {
        operation.Save(obj, immediate);
    }

    public void Update(T obj, bool immediate = false)
    {
        operation.Update(obj, immediate);
    }
    
    public void SaveAll()
    {
        this.operation.AutoSave();
    }

    public void NotifyChangeFields(T changeObj, string[] fieldNames)
    {
        this.operation.NotifyChangeFields(changeObj, fieldNames);
    }

    public void SetOperator(ISqlOperator<T> op)
    {
        this.operation = op;
        this.operation.SetDBEntityMeta(new DBEntityMeta<T>());

    }
}

