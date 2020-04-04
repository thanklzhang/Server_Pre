//using DataModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CenterServer.Mate
//{
//    public class MateManager
//    {
//        //目前匹配只是跟房间一样 凑成一个房间就战斗

//        List<CombatRoom> combatRooms = new List<CombatRoom>();
//        int initRoomNum;
//        int maxRoomNum;

//        public void Init(int initRoomNum = 20, int maxRoomNum = 100)
//        {
//            this.initRoomNum = initRoomNum;
//            this.maxRoomNum = maxRoomNum;

//            CreateRooms(initRoomNum);
//        }

//        void CreateRooms(int num)
//        {
//            for (int i = 0; i < num; ++i)
//            {
//                var combatRoom = CreateRoom();
//                combatRoom.SetEnable();
//            }
//        }

//        CombatRoom CreateRoom()
//        {
//            if (combatRooms.Count >= maxRoomNum)
//            {
//                Console.WriteLine("the num of room is full");
//                return null;
//            }
//            var combatRoom = CombatRoom.Create();
//            combatRooms.Add(combatRoom);
//            return combatRoom;

//        }

//        public CombatRoom FindCanEnterRoom()
//        {
//            var findRoom = combatRooms.Find(room =>
//            {
//                var roomState = room.GetState();
//                return roomState == CombatRoomState.CanEnter;
//            });

//            return findRoom;
//        }


//        public void AddPlayerToMate(Player player)
//        {
//            var findRoom = FindCanEnterRoom();
//            if (null == findRoom)
//            {
//                findRoom = CreateRoom();
//                if (null == findRoom)
//                {
//                    //达到最大可容纳房间数 maxRoomNum
//                    return;
//                }
//            }

//            findRoom.AddPlayer(player);
//        }

//        public CombatRoom FindCombatRoomByPlayer(Player player)
//        {
//            var findRoom = combatRooms.Find(room =>
//            {
//                return room.IsHavePlayer(player);
//            });

//            return findRoom;
//        }

//        public CombatRoom FindCombatRoomByCombatRoomId(int combatRoomId)
//        {
//            var findRoom = combatRooms.Find(room =>
//            {
//                return room.id == combatRoomId;
//            });

//            return findRoom;
//        }

//        public void RemovePlayerFromMate(Player player)
//        {
//            var findCombatRoom = FindCombatRoomByPlayer(player);
//            if (null == findCombatRoom)
//            {
//                Console.WriteLine("the player is not found");
//                return;
//            }

//            findCombatRoom.RemovePlayer(player);

//        }

//        internal void DestroyCombat(int combatRoomId)
//        {

//            var room = FindCombatRoomByCombatRoomId(combatRoomId);

//            if (null == room)
//            {
//                Console.WriteLine("the room is not found");
//                return;
//            }

//            room.EndCombat();
//        }

//        public void StartCombat(int combatId, int combatRoomId)
//        {
//            if (-1 == combatId)
//            {
//                Console.WriteLine("create combat fail");
//                return;
//            }
//            var room = FindCombatRoomByCombatRoomId(combatRoomId);

//            if (null == room)
//            {
//                Console.WriteLine("the room is not found");
//                return;
//            }

//            room.StartCombat(combatId);
//        }

//        /// <summary>
//        /// 在战斗中用户退出
//        /// </summary>
//        public void UserExitCombat(Player player)
//        {
//            var combatRoom = FindCombatRoomByPlayer(player);
//            if (combatRoom != null)
//            {
//                combatRoom.UserExitCombat(player);
//            }
//            else
//            {
//                Console.WriteLine("the room is not found");
//            }
//        }

//        public bool IsPlayerInCombat(Player player)
//        {
//            return player.combatRoomState == PlayerCombatRoomState.Started;
//        }
//    }
//}
