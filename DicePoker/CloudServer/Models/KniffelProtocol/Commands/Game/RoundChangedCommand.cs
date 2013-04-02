using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class RoundChangedCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_ROUND_CHANGED";

        public int Round { get; set; }
        

        public RoundChangedCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            Round = int.Parse(argsToken.NextToken());
            
        }

        public RoundChangedCommand(string name, int round)
            :base(name)
        {
            Round = round;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, Round);
        }
                
    }
}
