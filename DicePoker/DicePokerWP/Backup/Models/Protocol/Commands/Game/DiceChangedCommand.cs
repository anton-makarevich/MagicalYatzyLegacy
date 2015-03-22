using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class DiceChangedCommand :RollReportCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_DICE_CHANGED";

        
        public DiceChangedCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            
        }

        public DiceChangedCommand(string name, List<int> results)
            :base(name,results)
        {
           
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            
        }
    }
}
