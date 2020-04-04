using NetCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Combat
{


    public class CombatManager
    {

        //UdpServer udpServer;
        //TcpServer tcpServer;//tcp 传来所有玩家开始 然后才用 udp 真正开始游戏操作

        List<Combat> combatList = new List<Combat>();
        Dictionary<int, Combat> combatDic;
        int initMaxCombatCount = 10;
        public void Init()
        {

            //空间换时间
            for (int i = 0; i < initMaxCombatCount; ++i)
            {
                var combat = Combat.Create();
                combat.id = i + 1;
                combatList.Add(combat);

            }

            combatDic = combatList.ToDictionary(key => key.id, value => value);

            //战斗部分用 udp
            //udpServer = new UdpServer();
            //udpServer.Start();

            //网络连接
            UdpNet udpNet = new UdpNet();
            udpNet.SetReceiveEndPoint(ConstInfo.SS_Udp_Port);
            udpNet.StartReceive(2349);
            udpNet.ReceiveAction += ReceiveMsg;

            // StartCombat();

        }


        public void ReceiveMsg(byte[] msg)
        {
            MemoryStream ms = null;
            using (ms = new MemoryStream(msg))
            {
                BinaryReader reader = new BinaryReader(ms);
                int msgId = reader.ReadInt32();
                int len = msg.Length - 4;
                byte[] data = reader.ReadBytes(len);

                HandleMsg(msgId, data);

                reader.Close();
            }
        }

        public void HandleMsg(int msgId, byte[] msg)
        {
            var id = (GC2SS.MsgId)msgId;
            switch (id)
            {
                case GC2SS.MsgId.Gc2SsSendFrame:


                    GC2SS.SendFrame sendFrame = GC2SS.SendFrame.Parser.ParseFrom(msg);

                    ReceiveMsg(sendFrame.CombatId, sendFrame.Seat, sendFrame.CurrFrame);

                    break;
            }

        }

        internal void DestroyCombat(int combatId)
        {
            var combat = FindCombatById(combatId);

            if (null == combat)
                throw new Exception("the combat is null , the id : " + combatId);

            combat.End();
        }

        public void ReceiveMsg(int combatId, int playerSeat, CombatFrame frame)
        {
            var combat = FindCombatById(combatId);

            if (null == combat)
                throw new Exception("the combat is null , the id : " + combatId);

            combat.ReceiveMsg(playerSeat, frame);

        }




        public Combat StartACombat(List<GS2SS.PlayerLocation> playerLocation)
        {
            var canUseCombat = combatList.Find(c =>
            {
                return c.state == CombatState.Ready;
            });

            if (null == canUseCombat)
            {
                Console.WriteLine("the combatList can used is null");
                return null;
            }
            var players = new List<Player>();

            playerLocation.ForEach(pLocation =>
            {
                var player = new Player()
                {
                    endPoint = new IPEndPoint(IPAddress.Parse(pLocation.Ip), pLocation.UdpPort),
                    seat = pLocation.Seat
                };
                players.Add(player);
            });

            canUseCombat.Start(players);
            return canUseCombat;

        }

        public Combat FindCombatById(int id)
        {
            Combat combat = null;
            if (combatDic.ContainsKey(id))
            {
                combat = combatDic[id];
            }

            return combat;
        }


        //public void PlayerExit(int combatId, string account)
        //{
        //    var combat = FindCombatById(combatId);
        //    if (combat != null)
        //    {
        //        combat.PlayerExit(account);
        //    }
        //    else
        //    {
        //        Console.WriteLine("the combat is not exist : " + combatId);
        //    }
        //}
    }
}
