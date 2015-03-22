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
        public int Value
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
    /// Chat message event args
    /// </summary>
    public class ChatMessageEventArgs : EventArgs
    {
        private readonly ChatMessage m_Message;

        public ChatMessage Message { get { return m_Message; } }


        public ChatMessageEventArgs(ChatMessage message)
        {
            m_Message = message;
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

    public class StringEventArgs : EventArgs
    {
        private readonly string m_Str;
        public string Str { get { return m_Str; } }

        public StringEventArgs(string s)
        {
            m_Str = s;
        }
    }

    /// <summary>
    /// EventArgs avec un param d'un type quelconque (Key)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class KeyEventArgs<TKey> : EventArgs
    {
        private TKey m_Key;
        public TKey Key { get { return m_Key; } }

        public KeyEventArgs(TKey key)
        {
            m_Key = key;
        }
    }

    /// <summary>
    /// EventArgs avec un param d'un type quelconque (Key) et un autre param d'un type quelconque (Value)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValueEventArgs<TKey, TValue> : EventArgs
    {
        private TKey m_Key;
        private TValue m_Value;
        public TKey Key { get { return m_Key; } }
        public TValue Value { get { return m_Value; } }

        public KeyValueEventArgs(TKey key, TValue value)
        {
            m_Key = key;
            m_Value = value;
        }
    }

    /// <summary>
    /// EventArgs avec un param d'un type quelconque (Key) et d'autres params d'un type quelconque (Values)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValuesEventArgs<TKey, TValue> : EventArgs
    {
        private TKey m_Key;
        private TValue[] m_Values;
        public TKey Key { get { return m_Key; } }
        public TValue[] Values { get { return m_Values; } }

        public KeyValuesEventArgs(TKey key, params TValue[] values)
        {
            m_Key = key;
            m_Values = values;
        }
    }
    
}
