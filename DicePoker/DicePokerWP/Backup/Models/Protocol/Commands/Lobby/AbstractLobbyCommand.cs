using System;
using System.Collections.Generic;
using System.Text;
using Sanet.Network.Protocol.Commands;

namespace Sanet.Kniffel.Protocol.Commands.Lobby
{
    public abstract class AbstractLobbyCommand : AbstractCommand
    {
        public static new char Delimitter { get { return '|'; } }

        protected override void Append<T>(StringBuilder sb, T thing)
        {
            sb.Append(thing);
            sb.Append(AbstractLobbyCommand.Delimitter);
        }
    }
}
