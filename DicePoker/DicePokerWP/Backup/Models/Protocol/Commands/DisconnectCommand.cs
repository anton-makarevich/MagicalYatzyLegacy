using System;
using System.Collections.Generic;
using System.Text;
using Sanet.Models;

namespace Sanet.Network.Protocol.Commands
{
    public class DisconnectCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return DisconnectCommand.COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "DISCONNECT";


        public DisconnectCommand(StringTokenizer argsToken)
        {
        }
        protected override void Append<T>(StringBuilder sb, T thing)
        {
            sb.Append(thing);
        }

        public DisconnectCommand()
        {
        }
    }
}
