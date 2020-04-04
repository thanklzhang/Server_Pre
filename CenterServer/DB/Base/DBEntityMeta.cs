using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class DBEntityMeta<T> where T : DBEntity
{
    //private Type type;
    //private string name;
    private Dictionary<string, DBEntityFieldMeta> fieldMetaDic;
    private List<DBEntityFieldMeta> fieldMetaList;//为了顺序

    public List<DBEntityFieldMeta> primaryKeys;


    public DBEntityMeta()
    {
        fieldMetaDic = new Dictionary<string, DBEntityFieldMeta>();
        fieldMetaList = new List<DBEntityFieldMeta>();

        primaryKeys = new List<DBEntityFieldMeta>();
        //fieldMetaDic fill

        var type = typeof(T);
        var fields = type.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            var currField = fields[i];
            bool isIgnore = false;
            //需要判断是否忽略
            var ignoreAttrLsit = Attribute.GetCustomAttributes(currField, typeof(IgnoreSerializeAttribute)) as IgnoreSerializeAttribute[];
            if (ignoreAttrLsit != null && ignoreAttrLsit.Length > 0)
            {
                isIgnore = true;
            }

            bool isPrimaryKey = false;
            //判断是否是主键
            var keyAttrLsit = Attribute.GetCustomAttributes(currField, typeof(PrimaryKeyAttribute)) as PrimaryKeyAttribute[];
            if (keyAttrLsit != null && keyAttrLsit.Length > 0)
            {
                isPrimaryKey = true;
            }
            
            DBEntityFieldMeta fieldMeta = new DBEntityFieldMeta()
            {
                field = currField,
                isIgnoreSerialize = isIgnore,
                isPrimaryKey = isPrimaryKey
            };

            if (isPrimaryKey)
            {
                primaryKeys.Add(fieldMeta);
            }

            fieldMetaList.Add(fieldMeta);
            fieldMetaDic.Add(currField.Name, fieldMeta);
        }
        
    }

    public DBEntityFieldMeta GetField(string fieldName)
    {
        if (fieldMetaDic.ContainsKey(fieldName))
        {
            return fieldMetaDic[fieldName];
        }

        Console.WriteLine("the field is not exist : " + fieldName);
        return null;
    }

    public List<DBEntityFieldMeta> GetPrimaryKeyList()
    {
        return primaryKeys;
    }

    public List<DBEntityFieldMeta> GetFields()
    {
        return fieldMetaList;
    }


}

