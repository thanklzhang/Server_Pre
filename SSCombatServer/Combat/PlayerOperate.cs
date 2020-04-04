using NetCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combat
{
    public enum PlayerOperateType
    {
        Move,
        Attack
    }
    public class PlayerOperate
    {
        public int seat;
        public PlayerOperateType type;
        public int x;
        public int y;
        public int value;

        public static PlayerOperate Create(PlayerOperateType type, int seat, int x, int y, int value)
        {
            var op = new PlayerOperate()
            {
                type = type,
                seat = seat,
                x = x,
                y = y,
                value = value
            };

            return op;
        }

        public static PlayerOperate ToPlayerOp(NetCommon.CombatPlayerOperate serverOp)
        {
            var op = PlayerOperate.Create((PlayerOperateType)serverOp.Type, serverOp.Seat, serverOp.X, serverOp.Y, serverOp.Value);
            return op;
        }

        public CombatPlayerOperate ToServerPlayerOp()
        {
            CombatPlayerOperate serverOp = new CombatPlayerOperate()
            {
                Type = (int)type,
                Seat = seat,
                X = x,
                Y = y,
                Value = value
            };

            return serverOp;
        }
    }
}
