//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//namespace DataModel
//{
//    public class Hero
//    {
//        public int SN;
//        public int id;
//        public Config.EntityConfig combatConfig;
//        public int combatLevel = 1;
//        public int managementLevel = 1;
//        public int canMakeProjectNum = 2;//一次经营在一个工作台中可以做的项目数

//        public int smithWhiteQualityTimes;
//        public int smithGreenQualityTimes;
//        public int smithBlueQualityTimes;
//        public int smithPurpleQualityTimes;
//        public int smithOrangeQualityTimes;

//        OutItemFormula outItemFormula;

//        void Init()
//        {
//            outItemFormula = OutItemFormula.Create();
//            outItemFormula.Init(this);
//        }

//        //public static int maxGuid = 1;
//        public static Hero Create(int heroSN)
//        {
//            var hero = new Hero();
//            hero.SN = heroSN;
//            hero.id = GetMaxId();
//            hero.combatConfig = Config.ConfigManager.Instance.GetBySN<Config.EntityConfig>(heroSN);


//            hero.Init();

//            hero.Save();


//            return hero;
//        }

//        //private static int GetGuid()
//        //{
//        //    return maxGuid++;
//        //}

//        public static Hero CreateTemp()
//        {
//            var hero = new Hero();
//            hero.Init();
//            return hero;
//        }

//        public static int GetMaxId()
//        {
//            var j = DBMgr.redisOrg.HashGet("maxId", "heroId");
//            var preMaxId = j.IsNull ? 0 : int.Parse(j);
//            int maxId = preMaxId + 1;
//            DBMgr.redisOrg.HashSet("maxId", "heroId", maxId);
//            return maxId;
//        }

//        //后期可能变成异步操作
//        public void Save()
//        {
//            var heroIdStr = "hero:" + id;
//            DBMgr.redisOrg.HashSet(heroIdStr, "id", id);
//            //DBMgr.redisOrg.HashSet(playerId, "userAccount", userAccount);
//            DBMgr.redisOrg.HashSet(heroIdStr, "SN", SN);
//            DBMgr.redisOrg.HashSet(heroIdStr, "combatLevel", combatLevel);
//            DBMgr.redisOrg.HashSet(heroIdStr, "managementLevel", managementLevel);
//            DBMgr.redisOrg.HashSet(heroIdStr, "canMakeProjectNum", canMakeProjectNum);
//            DBMgr.redisOrg.HashSet(heroIdStr, "smithWhiteQualityTimes", smithWhiteQualityTimes);
//            DBMgr.redisOrg.HashSet(heroIdStr, "smithGreenQualityTimes", smithGreenQualityTimes);
//            DBMgr.redisOrg.HashSet(heroIdStr, "smithBlueQualityTimes", smithBlueQualityTimes);
//            DBMgr.redisOrg.HashSet(heroIdStr, "smithPurpleQualityTimes", smithPurpleQualityTimes);
//            DBMgr.redisOrg.HashSet(heroIdStr, "smithOrangeQualityTimes", smithOrangeQualityTimes);

//        }

//        internal void ForceFinishProject(WorkProject project)
//        {
//            project.ForceFinish(this);
//        }


//        /// <summary>
//        /// 生产一个物品
//        /// </summary>
//        /// <param name="itemSN"></param>
//        /// <returns></returns>
//        public Item OutputItem(int itemSN)
//        {
//            return outItemFormula.OutputItem(itemSN);
//        }

//    }

//}

