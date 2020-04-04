using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class CommonFunc
{
    public static int GetRandomValueIndexByWeights(List<int> weights)
    {
        int sumWeight = weights.Sum();

        Random r = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")));

        int rand = r.Next(sumWeight);//之后随机都按照一个玩家始终一个随机种子

        int sum = 0;
        int i = 0;
        for (; i < weights.Count; ++i)
        {
            int weight = weights[i];
            sum += weight;

            if (sum >= rand)
            {
                break;
            }
        }

        return i;
    }

    //之后改成 dic 
    public static string GetFieldSqlType(FieldInfo field)
    {
        var typeStr = "";
        if (field.FieldType == typeof(int))
        {
            typeStr = "INT";
        }
        else if (field.FieldType == typeof(string))
        {
            typeStr = "VARCHAR(64)";
        }
        return typeStr;
    }

    public static string GetValueStrSafe(DBEntity obj, FieldInfo field)
    {
        var value = field.GetValue(obj);
        string valueStr = "";
        if (field.FieldType == TypeHelper.intType)
        {
            valueStr = value != null ? value.ToString() : "0";

        }
        else
        if (field.FieldType == TypeHelper.stringType)
        {
            valueStr = value != null ? "'" + value.ToString() + "'" : "''";

        }

        return valueStr;
    }
}

