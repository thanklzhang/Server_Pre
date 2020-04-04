using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DBEntity
{
    [IgnoreSerialize]
    public int version = 0;
    [IgnoreSerialize]
    public List<string> willChangeFieldNames;
    public DBEntity()
    {
        willChangeFieldNames = new List<string>();
    }
    
    public void NotifyChangeFields(string[] fieldNames)
    {
        willChangeFieldNames.AddRange(fieldNames);
        UpdateVersion();
    }

    public List<string> CollectChangeFields()
    {
        List<string> list = new List<string>(willChangeFieldNames);
        //willChangeFieldNames.Clear();

        return list;
    }

    public void ClearChangeFields()
    {
        willChangeFieldNames.Clear();
    }

    public void UpdateVersion()
    {
        if (this.version >= int.MaxValue)
        {
            this.version = 1;
            return;
        }

        version += 1;
    }
}

