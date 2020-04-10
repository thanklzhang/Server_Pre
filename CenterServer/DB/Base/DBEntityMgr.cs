using System;
using System.Collections.Generic;
using System.Data;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DBEntityMgr<T> where T : DBEntity, new()
{
    ISqlOperator<T> operation;

    //自增 id
    public static HashSet<int> set = new HashSet<int>();
    public static int maxId = 1;
    public static int GetNewId()
    {
        if (maxId >= int.MaxValue)
        {
            for (int i = 1; i < int.MaxValue; ++i)
            {
                if (!set.Contains(i))
                {
                    set.Add(maxId);
                    maxId = i + 1;
                    return i;
                }
            }
        }
        else
        {
            set.Add(maxId);
            return maxId++;
        }


        Console.WriteLine("error : too much amount ... ");
        return -1;
    }

    public T Find(string[] fieldNames, string param)
    {
        return operation.Find(fieldNames, param);
    }

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

