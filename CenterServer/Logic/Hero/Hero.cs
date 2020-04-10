using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Hero
{
    public HeroData data;

    public Config.HeroInfo config;

    //Hero() { }

    //public static Hero Create(int configId, HeroData heroData)
    //{
    //    var hero = new Hero();
    //    hero.config = Config.ConfigManager.Instance.GetById <Config.HeroInfo>(configId) ;
    //    hero.data = heroData;

    //    return hero;
    //}


    /// <summary>
    /// 目前是模拟升级
    /// </summary>
    public void AddLevel(int value)
    {
        this.data.AddLevel(value);
        DBMgr.heroDataMgr.NotifyChangeFields(this.data, new string[] { "level" });

    }
}



