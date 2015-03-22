using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerLeftCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_LEFT_GAME";

       
        public PlayerLeftCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            
        }

        public PlayerLeftCommand(string name)
            :base(name)
        {
            
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            
        }
                
    }
}
