using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class TableInfoNeededCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gameTABLE_INFO_NEEDED";

        

        public TableInfoNeededCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            
        }

        public TableInfoNeededCommand(string name)
            : base(name)
        {
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
        }
    }
}
