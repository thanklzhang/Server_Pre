using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HeroDataMgr<T> : DBEntityMgr<T> where T : DBEntity, new()
{
    public static HeroData CreateHeroData(int userId, int configId)
    {
        HeroData heroData = new HeroData();
        heroData.id = GetNewId();
        heroData.userId = userId;
        heroData.configId = configId;
        heroData.level = 1;

        DBMgr.heroDataMgr.Save(heroData);

        return heroData;
    }
}

