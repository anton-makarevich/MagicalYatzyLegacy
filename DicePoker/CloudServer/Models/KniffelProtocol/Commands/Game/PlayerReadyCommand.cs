using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerReadyCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_READY";

        public bool IsReady { get; set; }

        public PlayerReadyCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            IsReady = bool.Parse(argsToken.NextToken());
            
        }

        public PlayerReadyCommand(string name, bool isready)
            :base(name)
        {
            IsReady = isready;
            
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, IsReady);
            
        }
                
    }
}
