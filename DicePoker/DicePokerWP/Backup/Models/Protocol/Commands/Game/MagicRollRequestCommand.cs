using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class MagicRollCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gameMAGIC_ROLL_REQUEST";

        

        public MagicRollCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            
        }

        public MagicRollCommand(string name)
            :base(name)
        {
            
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            
        }
    }
}
