using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Combat
{
    public enum PlayerState
    {
        Ready,
        Started,
        Exit
    }

    public class Player
    {
        public string userAccount;
        public EndPoint endPoint;
        public UdpNet net;

        //游戏数据-----

        //玩家初始数据
        //public int team;
        public int seat;

        //public int initSpeed;
        //public int initAttack;
        //public int initDefence;
        //public int initMaxHealth;

        public int currSyncFrameId;//服务器认为用户当前已经同步的帧

        public PlayerState state;


        public static Player Create()
        {
            return new Player();
        }

        public void StartCombat()
        {
            net = new UdpNet();
            this.state = PlayerState.Started;
        }

        public void Send(int msgId, byte[] data)
        {
            net.Send(endPoint, msgId, data);
        }


        public void ExitCombat()
        {
            net?.Close();
            this.state = PlayerState.Exit;
        }

        public void FinishCombat()
        {
            net?.Close();
            this.state = PlayerState.Ready;
        }
    }
}
