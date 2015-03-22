using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class ManualChangeCommand :FixDiceCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_MANUAL_CHANGE";

        public int NewValue { get; set; }
        
        public ManualChangeCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            NewValue = int.Parse(argsToken.NextToken());
        }

        public ManualChangeCommand(string name, int oldvalue, int newvalue, bool isfixed)
            :base(name,oldvalue,isfixed)
        {
            NewValue = newvalue;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, NewValue);
        }
                
    }
}
