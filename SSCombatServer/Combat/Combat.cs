using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Google.Protobuf;
using NetCommon;

namespace Combat
{
    public enum CombatState
    {
        Ready,
        Started
    }
    public class Combat
    {
        public int id;
        public CombatState state;
        List<Player> playerList;
        Dictionary<int, Player> playerDic;
        List<Frame> frames = new List<Frame>();
        Timer timer;


        public static Combat Create()
        {
            var combat = new Combat();
            combat.state = CombatState.Ready;
            return combat;
        }

        public void Start(List<Player> players)
        {


            if (this.state == CombatState.Started)
                throw new Exception("the state of combat is begin");

            this.playerList = players;
            this.state = CombatState.Started;

            if (null == this.playerList)
                throw new Exception("no player");




            playerDic = playerList.ToDictionary(key => key.seat, value => value);
            for (int i = 0; i < this.playerList.Count; ++i)
            {
                var player = playerList[i];
                player.StartCombat();
            }

            Console.WriteLine("start combat !");

            //这里会用 tcp 通知战斗开始 之后均用 udp 进行帧同步
            //tcpServer.Broadcast...

            frameList.Add(Frame.Create());//创建一个空帧



            this.timer = new Timer();
            timer.Interval = 66;
            timer.Elapsed += new ElapsedEventHandler(HandleLogicFrame);
            timer.Enabled = true;


        }

        private void CapturePlayerOperations()
        {
            playerOperateList?.Clear();
        }

        int currFrameId = 1;
        List<Frame> frameList = new List<Frame>();
        /// <summary>
        /// 处理当前逻辑帧
        /// </summary>
        public void HandleLogicFrame(object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("frame logic ... " + currFrameId);

            Frame frame = Frame.Create();
            frame.id = currFrameId;
            frame.SetPlayerOperations(playerOperateList);
            frameList.Add(frame);

            //发送服务器认为玩家未同步的所有帧 playerSyncFrameId + 1 -> currFrame - 1
            playerList.ForEach(p =>
            {
                List<Frame> playerNotSyncFrames = GetPlayerNotSyncFrames(p, currFrameId - 1);
                //for (int i = p.currSyncFrameId + 1; i <= currFrameId; ++i)
                //{
                //    playerNotSyncFrames.Add(frameList[i]);
                //}

                var sendFrame = new SS2GC.FrameAction();
                sendFrame.CurrFrame = frame.ToServerFrame();
                if (playerNotSyncFrames.Count > 0)
                {
                    playerNotSyncFrames.ForEach(playerNotSyncFrame =>
                    {
                        sendFrame.NotSyncFrames.Add(playerNotSyncFrame.ToServerFrame());
                    });

                }

                p.Send((int)SS2GC.MsgId.Ss2GcFrameAction, sendFrame.ToByteArray());//playerNotSyncFrames
            });



            currFrameId++;

            //进行采集玩家帧操作等
            CapturePlayerOperations();


        }

        //public void PlayerExit(string account)
        //{
        //    var player = FindPlayerByAccount(account);
        //    player?.ExitCombat();

        //    //if (IsAllPlayerExit())
        //    //{
        //    //    End();
        //    //}
        //}

        ///// <summary>
        ///// 判断是否所有玩家都退出了
        ///// </summary>
        ///// <returns></returns>
        //bool IsAllPlayerExit()
        //{
        //    foreach (var item in playerDic)
        //    {
        //        if (item.Value.state == PlayerState.Ready)
        //        {
        //            if (item.Value.state == PlayerState.Ready)
        //            {
        //                Console.WriteLine("warning : " + "the state is " + PlayerState.Ready);
        //                continue;
        //            }
        //        }

        //        if (item.Value.state == PlayerState.Started)
        //        {
        //            return false;
        //        }

        //    }

        //    return true;
        //}

        public void End()
        {
            Console.WriteLine("end combat : " + this.id);
            timer.Enabled = false;
            this.timer = null;

            if (playerList != null)
            {
                for (int i = 0; i < playerList.Count; ++i)
                {
                    var player = playerList[i];
                    player?.FinishCombat();
                }
            }
            playerList = null;
            playerDic = null;
            this.state = CombatState.Ready;
        }

        List<PlayerOperate> playerOperateList = new List<PlayerOperate>();

        public void ReceiveMsg(int seat, CombatFrame frame)
        {
            var player = FindPlayerBySeat(seat);
            if (null == player)
                throw new Exception("the player is null , the seat is : " + seat);

            /////////////////////判断是否是玩家在战斗中的操作


            // GC2SS.SendFrame sendFrame = GC2SS.SendFrame.Parser.ParseFrom(data);

            //获得玩家的操作 存入 操作列表 中 --------------------------------
            Frame clientFrame = Frame.ToFrame(frame);
            var clientFrameId = clientFrame.id;
            //这里发过来的 id 是 服务端认为客户端同步的最大帧 + 1 由客户端提供
            if (player.currSyncFrameId < clientFrameId - 1)
            {
                player.currSyncFrameId = clientFrameId - 1;
            }


            if (clientFrameId != this.currFrameId)
            {
                //只收集本帧的操作 如果服务器 10 ， 收到 11  几乎不可能  
                //如果服务器 11 ， 收到 10  反过来  是客户端发来之后有延迟 直接丢弃(可以考虑放到下一帧等操作)
                return;
            }

            List<PlayerOperate> ops = new List<PlayerOperate>();
            frame.Opts.ToList().ForEach(serverOp =>
            {
                var op = PlayerOperate.ToPlayerOp(serverOp);
                ops.Add(op);
            });

            //List<PlayerOperate> simulatOps = new List<PlayerOperate>();

            //for (int i = 0; i < simulatOps.Count; ++i)
            //{
            //    var simulateOp = simulatOps[i];
            //    //var framId = simulateOp.frameId;
            //    ops.Add(simulateOp);
            //}

            playerOperateList.AddRange(ops);//PlayerOperate.Create
        }

        //public void SendToPlayer(int seat, byte[] data)
        //{

        //    var player = FindPlayerBySeat(seat);
        //    if (null == player)
        //        throw new Exception("the player is null , the seat is : " + seat);
        //    player.Send(data);
        //}

        public Player FindPlayerByAccount(string acconnt)
        {
            Player player = null;
            foreach (var item in playerDic)
            {
                if (item.Value.userAccount == acconnt)
                {
                    player = item.Value;
                    break;
                }
            }
            return player;
        }

        public Player FindPlayerBySeat(int seat)
        {
            Player player = null;
            if (playerDic.ContainsKey(seat))
            {
                player = playerDic[seat];
            }

            return player;
        }

        public List<Frame> GetPlayerNotSyncFrames(Player player, int toFrame)
        {
            List<Frame> frames = new List<Frame>();
            for (int i = player.currSyncFrameId + 1; i <= toFrame; ++i)
            {
                var frame = frameList[i];
                frames.Add(frame);
            }

            return frames;
        }
    }
}
