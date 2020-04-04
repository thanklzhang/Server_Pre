using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCommon;

namespace Combat
{
    public class Frame
    {
        public int id;
        List<PlayerOperate> playerOpts = new List<PlayerOperate>();

        public static Frame Create()
        {
            return new Frame();
        }

        public void AddPlayerOperation(PlayerOperate playerOp)
        {
            playerOpts.Add(playerOp);
        }

        public void AddPlayerOperation(PlayerOperateType type, int seat, int x, int y, int value)
        {
            var op = PlayerOperate.Create(type, seat, x, y, value);
            AddPlayerOperation(op);
        }

        public void SetPlayerOperations(List<PlayerOperate> playerOps)
        {
            playerOpts.AddRange(playerOps);
        }

        public static Frame ToFrame(NetCommon.CombatFrame combatFrame)
        {
            Frame frame = new Frame()
            {
                id = combatFrame.Id
            };

            combatFrame.Opts.ToList().ForEach(serverOp =>
            {
                frame.AddPlayerOperation(PlayerOperate.ToPlayerOp(serverOp));
            });

            //frame.AddPlayerOperation();

            return frame;
        }

        public NetCommon.CombatFrame ToServerFrame()
        {
            CombatFrame serverFrame = new CombatFrame()
            {
                Id = id,

            };

            if (playerOpts.Count > 0)
            {
                playerOpts.ForEach(op =>
                {
                    serverFrame.Opts.Add(op.ToServerPlayerOp());
                });
            }
            return serverFrame;

        }


    }
}
