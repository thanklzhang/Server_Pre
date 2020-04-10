using DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HeroMgr
{
    private static HeroMgr instance;

    public static HeroMgr Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new HeroMgr();
            }

            return instance;
        }
    }

   
    /// <summary>
    /// 加载属于该用户的英雄数据(只在用户登录成功会执行一次)
    /// </summary>
    /// <param name="user"></param>
    public void LoadHeroData(User user)
    {
        var heroDataList = DBMgr.heroDataMgr.FindAll(null, " userId = " + user.id);
        for (int i = 0; i < heroDataList.Count; ++i)
        {
            var heroData = heroDataList[i];
            Hero hero = CreateHero(heroData.configId, heroData);
            user.PutHero(hero);
        }

    }

    public static Hero CreateHero(int configId, HeroData heroData)
    {
        var hero = new Hero();
        hero.config = Config.ConfigManager.Instance.GetById<Config.HeroInfo>(configId);
        hero.data = heroData;

        return hero;
    }

    /// <summary>
    /// 通知英雄更改了信息 需要发送给用户
    /// </summary>
    public void NotifyUpdateHeroes(User user, List<Hero> heroes)
    {
        GC2CS.respNotifyUpdateHeroes resp = new GC2CS.respNotifyUpdateHeroes();

        for (int i = 0; i < heroes.Count; ++i)
        {
            var hero = heroes[i];
            resp.HeroInfoList.Add(new GC2CS.HeroInfo()
            {
                //之后这些会抽出来自动转换
                ConfigId = hero.config.id,
                Id = hero.data.id,
                Level = hero.data.level
            });
        }

        user.SendMsg((int)GC2CS.MsgId.Gc2CsNotifyUpdateHeroes, resp.ToByteArray());
    }

    public void CreateHero(User user,Hero hero)
    {
        user.PutHero(hero);
    }

}

