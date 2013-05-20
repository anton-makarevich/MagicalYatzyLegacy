using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public abstract class PlayerCommand : AbstractCommand
    {
        
        public string Name { get; set; }

        public PlayerCommand(StringTokenizer argsToken)
        {
            Name = argsToken.NextToken();
        }

        public PlayerCommand(string name)
        {
            Name=name;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, Name);
        }
    }
}
