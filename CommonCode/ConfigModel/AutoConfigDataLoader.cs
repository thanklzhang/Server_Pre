/*
 * generate by tool
*/
using System;
using System.Collections.Generic;
using Config;
using System.Collections;
using LitJson;

public class AutoConfigDataLoader
{
    private static AutoConfigDataLoader instance;

    public static AutoConfigDataLoader Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new AutoConfigDataLoader();
            }
            return instance;
        }

    }
    Dictionary<string, IList> configDic = new Dictionary<string, IList>();

    public Dictionary<string, IList> LoadAllData(List<ConfigInfo> files)
    {
        foreach (var f in files)
        {
            var className = f.name.Substring(0, 1).ToUpper() + f.name.Substring(1);

            var m = this.GetType().GetMethod($"Load{className}");
            if (m == null)
                throw new Exception($"class {className} is not exist");

            m.Invoke(this, new object[] { f.json });
        }

        return configDic;
    }

    
    /// <summary>
    /// LoadHeroInfo
    /// </summary>
    public void LoadHeroInfo(string json)
    {
        var list = JsonMapper.ToObject<List<HeroInfo>>(json);
        configDic.Add(typeof(HeroInfo).Name, list);
    }
    /// <summary>
    /// LoadResourcePath
    /// </summary>
    public void LoadResourcePath(string json)
    {
        var list = JsonMapper.ToObject<List<ResourcePath>>(json);
        configDic.Add(typeof(ResourcePath).Name, list);
    }


}
