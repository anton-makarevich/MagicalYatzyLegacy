using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class FixDiceCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_FIX_DICE";

        public int Value { get; set; }
        public bool IsFixed { get; set; }

        public FixDiceCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            Value = int.Parse(argsToken.NextToken());
            IsFixed = bool.Parse(argsToken.NextToken());
        }

        public FixDiceCommand(int pos, int value, bool isfixed)
            :base(pos)
        {
            Value = value;
            IsFixed = isfixed;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, Value);
            Append(sb, IsFixed);
        }
                
    }
}
