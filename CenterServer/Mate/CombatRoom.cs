//using DataModel;
//using Google.Protobuf;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CenterServer.Mate
//{
//    public enum CombatRoomState
//    {
//        Disable,
//        CanEnter,
//        Full,
//        Started

//    }

//    public class CombatRoom
//    {
//        public int id;
//        static int maxId = 1;
//        List<Player> players = new List<Player>();
//        CombatRoomState state;

//        const int maxPlayerNum = 2;
//        public int currCombatId;
//        public static CombatRoom Create()
//        {
//            var combatRoom = new CombatRoom()
//            {
//                id = GetId(),
//            };
//            return combatRoom;
//        }

//        public static int GetId()
//        {
//            return maxId++;
//        }

//        public void SetEnable()
//        {
//            if (state == CombatRoomState.Disable)
//            {
//                state = CombatRoomState.CanEnter;
//            }
//        }

//        public CombatRoomState GetState()
//        {
//            return state;
//        }

//        public bool IsHavePlayer(Player player)
//        {
//            return players.Exists(p => p.id == player.id);
//        }

//        public void AddPlayer(Player player)
//        {
//            if (null == player)
//            {
//                Console.WriteLine("the player is null");
//                return;
//            }

//            if (state == CombatRoomState.Full)
//            {
//                Console.WriteLine("the room is full");
//                return;
//            }

//            players.Add(player);

//            if (players.Count >= maxPlayerNum)
//            {
//                state = CombatRoomState.Full;

//                //目前人满就开 (之后可能会来个消息队列 然后在主线程处理这类的消息)
//                AskCreateCombat();
//            }

//        }

//        public void AskCreateCombat()
//        {
//            Console.WriteLine("the room is full , ask create combat");
//            GS2SS.CreateCombat createCombat = new GS2SS.CreateCombat();
//            for (int i = 0; i < players.Count; ++i)
//            {
//                var currPlayer = players[i];

//                GS2SS.PlayerLocation playerLocation = new GS2SS.PlayerLocation()
//                {
//                    Ip = currPlayer.ip,
//                    UdpPort = currPlayer.udpPort,
//                    Seat = i,
//                };

//                createCombat.PlayerLocations.Add(playerLocation);
//            }
//            createCombat.RoomId = this.id;

//            CSServer.Instance.TransToGS(0, 0, (int)GS2SS.MsgId.Gs2SsFromCsCreateCombat, createCombat.ToByteArray());

//        }

//        public void RemovePlayer(Player player)
//        {

//            if (null == player)
//            {
//                Console.WriteLine("the player is null");
//                return;
//            }

//            if (!players.Contains(player))
//            {
//                Console.WriteLine("the player is not found in the room");
//                return;
//            }

//            players.Remove(player);

//            state = CombatRoomState.CanEnter;
//        }

//        public void StartCombat(int combatId)
//        {
//            if (state == CombatRoomState.Full)
//            {
//                //start
//                currCombatId = combatId;
//                for (int i = 0; i < players.Count; ++i)
//                {
//                    var player = players[i];
//                    player.combatRoomState = PlayerCombatRoomState.Started;
//                    GS2GC.StartCombat startCombat = new GS2GC.StartCombat()
//                    {
//                        CombatId = combatId,
//                        MapSN = 101010,
//                    };

//                    startCombat.Seat = i;
//                    for (int j = 0; j < players.Count; ++j)
//                    {
//                        GS2GC.CombatHero combatHero = new GS2GC.CombatHero()
//                        {
//                            SN = 10000,//test
//                            Seat = j,
//                            Team = j,//目前先乱斗
//                        };

//                        startCombat.CombatHeroes.Add(combatHero);
//                    }
//                    Console.WriteLine("send combat info");
//                    CSServer.Instance.TransToGS(0, player.GetUserSessionId(), (int)GS2GC.MsgId.Gs2GcFromCsStartCombat, startCombat.ToByteArray());
//                }

//            }
//            else
//            {
//                Console.WriteLine("the room is not full");
//            }
//        }

//        bool IsAllPlayersExit()
//        {
//            foreach (var item in players)
//            {
//                if (item.combatRoomState != PlayerCombatRoomState.Exit)
//                {
//                    return false;
//                }
//            }

//            return true;
//        }

//        internal void UserExitCombat(Player player)
//        {
//            player.combatRoomState = PlayerCombatRoomState.Exit;

//            if (IsAllPlayersExit())
//            {
//                //结束战斗
//                EndCombat();
//            }
//        }



//        public void EndCombat()
//        {
//            Console.WriteLine("end combat and send destroy combat msg");
//            //发送给 SS 销毁战斗
//            GS2SS.DestroyCombat destroyCombat = new GS2SS.DestroyCombat()
//            {
//                CombatId = this.currCombatId
//            };
//            CSServer.Instance.TransToGS(0, 0, (int)GS2SS.MsgId.Gs2SsFromCsDestroyCombat, destroyCombat.ToByteArray());


//            Reset();
//        }

//        public void Reset()
//        {
//            players?.Clear();
//            state = CombatRoomState.CanEnter;
//        }
//    }
//}
