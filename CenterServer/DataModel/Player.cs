//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Google.Protobuf;
//namespace DataModel
//{
//    public enum PlayerCombatRoomState
//    {
//        Ready,
//        Started,
//        Exit

//    }
//    public class Player
//    {
//        //public static int maxGuid = 1;

//        public int id;

//        public string userAccount;

//        //玩家等级
//        public int level;

//        //玩家金钱
//        public int coin;

//        ////拥有的物品
//        //List<Item> ownItemList = new List<Item>();

//        ////拥有的图纸
//        //List<Drawing> ownDrawingList = new List<Drawing>();

//        ////拥有的英雄
//        //List<Hero> ownHeroList = new List<Hero>();

//        //当前的经营
//        Management management = null;
//        public int canUseMaxBenchNum;//当前能够使用的最大工作台数

//        User user;

//        //战斗 
//        //战斗时候需要玩家的 ip 和 udpPort
//        public string ip;
//        public int udpPort;
//        public PlayerCombatRoomState combatRoomState;

//        public void SetUser(User user)
//        {
//            this.user = user;
//        }

//        public int GetUserSessionId()
//        {
//            return user.sessionId;
//        }

//        public void RemoveItem(int itemId)
//        {
//            DBMgr.redisOrg.ListRemove("playeritem:" + id, itemId);

//            //int removeNum = ownItemList.RemoveAll(item => item.id == itemId);

//            //if (0 == removeNum)
//            //{
//            //    Console.WriteLine("the itemGuid is not found : " + itemId);
//            //}
//        }


//        //management
//        //public Management GetManagementInfo()
//        //{
//        //    return management;
//        //}
//        public ManagementState IsStartManagement()
//        {
//            return management.state;
//        }



//        public void StartManagement(int type, List<SelectWorkBench> selectWorkBenches)
//        {
//            if (null == management)
//                throw new Exception("the management is null");

//            if (0 == type)
//            {
//                management.StartNormal(selectWorkBenches, this);
//            }

//            if (1 == type)
//            {
//                management.StartNormal(selectWorkBenches, this);
//                EndManagement();
//            }

//        }

//        public void ReceiveManagementResult()
//        {
//            EndManagement();
//        }

//        void EndManagement()
//        {
//            if (null == management)
//                throw new Exception("the management is null");

//            var result = management.Finish();
//            PostManagementResult(result.itemList);

//        }

//        void PostManagementResult(List<Item> items)
//        {
//            GS2GC.PostManagementResult result = new GS2GC.PostManagementResult();
//            items.ForEach(item =>
//            {
//                var serverItem = ConverModel.ToServerItem(item);
//                result.ItemList.Add(serverItem);
//            });

//            Console.WriteLine("items : " + result.ItemList.Count);
//            Console.WriteLine("-------------------post : the session id : " + user.sessionId);
//            CSServer.Instance.TransToGS(0, user.sessionId, (int)GS2GC.MsgId.Gs2GcFromCsPostManagementResult, result.ToByteArray());
//        }

//        void PostPlayerInfo()
//        {

//        }

//        void PostManagementInfo()
//        {

//        }

//        void PostCanUseDrawings()
//        {

//        }

//        public GS2GC.ManagementInfo GetPackageMangementInfo()
//        {
//            return management.ToPackageData();
//        }



//        //-----------------------------------------------------------------

//        public static int GetMaxId()
//        {
//            int maxId = 1;
//            if (DBMgr.redisOrg.HashExists("maxId", "playerId"))
//            {
//                var preMaxId = int.Parse(DBMgr.redisOrg.HashGet("maxId", "playerId"));
//                maxId = preMaxId + 1;
//            }
//            DBMgr.redisOrg.HashSet("maxId", "playerId", maxId);
//            return maxId;
//        }

//        public static Player Create(User user)
//        {
//            //根据 user 到数据库中找到该 player
//            var playerId = user.playerId;

//            Player p = new Player();

//            var playerIdStr = "player:" + playerId;
//            if (DBMgr.redisOrg.KeyExists(playerIdStr))
//            {
//                //填充玩家基础信息
//                p.id = int.Parse(DBMgr.redisOrg.HashGet(playerIdStr, "id"));
//                //p.userAccount = DBMgr.redisOrg.HashGet("PlayerId:" + playerId, "userAccount");
//                p.level = int.Parse(DBMgr.redisOrg.HashGet(playerIdStr, "level"));
//                p.coin = int.Parse(DBMgr.redisOrg.HashGet(playerIdStr, "coin"));
//                p.canUseMaxBenchNum = int.Parse(DBMgr.redisOrg.HashGet(playerIdStr, "canUseMaxBenchNum"));

//            }
//            else
//            {
//                //没有玩家 创建新玩家
//                p.id = GetMaxId();
//                p.user = user;

//                //玩家初始信息设置(之后可以配表来实现)
//                p.level = 1;
//                p.coin = 1200;
//                p.canUseMaxBenchNum = 2;

//                //初始图纸
//                var initDrawings = new List<Drawing>()
//                {
//                    Drawing.Create(400000),
//                    Drawing.Create(400001),
//                    Drawing.Create(400002),
//                    Drawing.Create(400003),
//                    Drawing.Create(400004),
//                    Drawing.Create(400005),
//                    Drawing.Create(400006),
//                    Drawing.Create(400007),
//                };
//                p.GainDrawings(initDrawings);


//                //初始英雄
//                var initHeroes = new List<Hero>()
//                {
//                    Hero.Create(10000),
//                    Hero.Create(10001),
//                    Hero.Create(10002),
//                    Hero.Create(10003),
//                };
//                p.GainHeroes(initHeroes);

//                p.Save();
//            }

//            p.SetUser(user);

//            //经营的信息
//            p.management = Management.Create(p);

//            return p;
//        }

//        public void GainDrawing(Drawing drawing)
//        {
//            DBMgr.redisOrg.ListRightPush("playerDrawing:" + id, drawing.id);
//        }

//        public void GainDrawings(List<Drawing> drawings)
//        {
//            drawings.ForEach(drawing =>
//            {
//                GainDrawing(drawing);
//            });
//        }

//        public List<Drawing> GetOwnDrawings()
//        {
//            var ownDrawingList = new List<Drawing>();

//            var redisDrawingList = DBMgr.redisOrg.ListRange("playerDrawing:" + id);

//            if (redisDrawingList != null)
//            {
//                redisDrawingList.ToList().ForEach(rDrawing =>
//                {
//                    var drawing = DrawingDBOp.FindDrawingById(int.Parse(rDrawing));
//                    ownDrawingList.Add(drawing);
//                });
//            }
//            return ownDrawingList;
//        }

//        public List<Hero> GetOwnHeroes()
//        {
//            var ownHeroList = new List<Hero>();

//            var redisHeroList = DBMgr.redisOrg.ListRange("playerHero:" + id);

//            if (redisHeroList != null)
//            {
//                redisHeroList.ToList().ForEach(rHero =>
//                {
//                    var hero = HeroDBOp.FindHeroById(int.Parse(rHero));
//                    ownHeroList.Add(hero);
//                });
//            }
//            return ownHeroList;
//        }


//        public void GainHero(Hero hero)
//        {
//            DBMgr.redisOrg.ListRightPush("playerHero:" + id, hero.id);
//        }

//        public void GainHeroes(List<Hero> heroes)
//        {
//            heroes.ForEach(hero =>
//            {
//                GainHero(hero);
//            });
//        }



//        public List<Item> GetOwnItems()
//        {
//            var ownItemList = new List<Item>();

//            var redisItemList = DBMgr.redisOrg.ListRange("playerItem:" + id);

//            if (redisItemList != null)
//            {
//                redisItemList.ToList().ForEach(rItem =>
//                {
//                    var item = ItemDBOp.FindItemById(int.Parse(rItem));
//                    ownItemList.Add(item);
//                });
//            }
//            return ownItemList;
//        }


//        //item
//        public void GainItems(List<Item> items)
//        {
//            if (null == items)
//            {
//                return;
//            }

//            items.ForEach(item =>
//            {
//                GainItem(item);
//            });

//            //ownItemList.AddRange(items);

//            //发送给客户端
//            SyncItemListInfo();


//        }



//        public void GainItem(Item item)
//        {
//            DBMgr.redisOrg.ListRightPush("playerItem:" + id, item.id);
//        }




//        //后期可能变成异步操作
//        public void Save()
//        {
//            var playerId = "player:" + id;
//            DBMgr.redisOrg.HashSet(playerId, "id", id);
//            //DBMgr.redisOrg.HashSet(playerId, "userAccount", userAccount);
//            DBMgr.redisOrg.HashSet(playerId, "level", level);
//            DBMgr.redisOrg.HashSet(playerId, "coin", coin);
//            DBMgr.redisOrg.HashSet(playerId, "canUseMaxBenchNum", canUseMaxBenchNum);
//        }


//        public void SyncHeroListInfo()
//        {
//            GS2GC.HeroListInfo heroInfo = new GS2GC.HeroListInfo();
//            var heroes = this.GetOwnHeroes();
//            heroes.ForEach(h =>
//            {
//                heroInfo.Heroes.Add(new GS2GC.Hero() { Id = h.id, SN = h.SN });
//            });

//            CSServer.Instance.TransToGS(0, user.sessionId, (int)GS2GC.MsgId.Gs2GcFromCsSyncHeroListInfo, heroInfo.ToByteArray());


//            // SendToClient((int)GS2GC.MsgId.Gs2GcFromCsSyncHeroListInfo, heroInfo.ToByteArray());
//        }

//        public void SyncItemListInfo()
//        {
//            GS2GC.ItemListInfo itemInfo = new GS2GC.ItemListInfo();
//            var items = this.GetOwnItems();
//            items.ForEach(item =>
//            {
//                itemInfo.Items.Add(new GS2GC.Item() { SN = item.SN, Id = item.id, Quality = (int)item.quality, StarLevel = item.starLevel, Price = item.price });
//            });
//            CSServer.Instance.TransToGS(0, user.sessionId, (int)GS2GC.MsgId.Gs2GcFromCsSyncItemListInfo, itemInfo.ToByteArray());
//        }

//        public void SyncPlayerBaseInfo()
//        {
//            GS2GC.PlayerBaseInfo baseInfo = new GS2GC.PlayerBaseInfo();
//            baseInfo.Account = user.account;
//            baseInfo.Coin = this.coin;
//            baseInfo.Level = this.level;

//            //baseInfo.Coin = 1230;
//            //baseInfo.Level = 1;
//            //baseInfo.NickName = "Kaller";
//            //baseInfo.PortraitURL = "";
//            CSServer.Instance.TransToGS(0, user.sessionId, (int)GS2GC.MsgId.Gs2GcFromCsSyncPlayerBaseInfo, baseInfo.ToByteArray());
//        }

//        public void SyncDrawingListInfo()
//        {
//            GS2GC.DrawingListInfo drawingListInfo = new GS2GC.DrawingListInfo();

//            var playerOwnDrawings = this.GetOwnDrawings();
//            playerOwnDrawings.ForEach(drawing =>
//            {
//                drawingListInfo.Drawings.Add(new GS2GC.Drawing() { Id = drawing.id, SN = drawing.SN });
//            });

//            CSServer.Instance.TransToGS(0, user.sessionId, (int)GS2GC.MsgId.Gs2GcFromCsSyncDrawingListInfo, drawingListInfo.ToByteArray());
//        }


//        public void SyncManagementInfo()
//        {
//            GS2GC.ManagementInfo managementInfo = this.GetPackageMangementInfo();
//            //enterResult.ManagementInfo = new GS2GC.ManagementInfo()
//            //{
//            //    State = 0,
//            //    LastStartTime = 0,
//            //    LastFinishTime = 0,
//            //    TotalUseBenchNum = 4//这里先固定值  之后会根据事实来更改
//            //};

//            CSServer.Instance.TransToGS(0, user.sessionId, (int)GS2GC.MsgId.Gs2GcFromCsSyncManagementInfo, managementInfo.ToByteArray());
//        }




//    }

//}

