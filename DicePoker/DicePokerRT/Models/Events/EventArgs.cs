using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models.Events
{
    /// <summary>
    /// Event arg when player changed
    /// </summary>
    public class PlayerEventArgs : EventArgs
    {
        //Current player
        private Player _Player;
        public Player Player { get { return _Player; } }

        public PlayerEventArgs(Player player)
        {
            _Player = player;

        }
    }
    /// <summary>
    /// Event when move changed
    /// </summary>
    public class MoveEventArgs : PlayerEventArgs
    {
        //new move order
        int _Move;
        public int Move
        {
            get
            {
                return _Move;
            }
        }

        public MoveEventArgs(Player player, int move)
            : base(player)
        {
            _Move = move;
        }
    }
    /// <summary>
    /// Event when player fix (unfix) dice with value
    /// </summary>
    public class FixDiceEventArgs :PlayerEventArgs
    {
        //dice with value to fix
        int _Value;
        public int Move
        {
            get
            {
                return _Value;
            }
        }
        //fix or unfix
        bool _IsFixed;
        public bool Isfixed
        {
            get { return _IsFixed; }
        }

        public FixDiceEventArgs(Player player, int value, bool isfixed):base(player)
        {
            _Value = value;
            _IsFixed = isfixed;
        }
    }
    /// <summary>
    /// Event when move changed
    /// </summary>
    public class RollEventArgs : PlayerEventArgs
    {
        //new move order
        int[] _Value;
        public int[] Value
        {
            get
            {
                return _Value;
            }
        }

        public RollEventArgs(Player player, int[] value)
            : base(player)
        {
            _Value = value;
        }
    }
    /// <summary>
    /// Event to notified about score applied
    /// </summary>
    public class ResultEventArgs : PlayerEventArgs
    {
        //new move order
        RollResult _Result;
        public RollResult Result
        {
            get
            {
                return _Result;
            }
        }

        public ResultEventArgs(Player player, RollResult result)
            : base(player)
        {
            _Result=result;
        }
    }
}
