﻿using System;
using System.Collections.Generic;
using System.Text;
using Sanet.Network.Protocol.Commands;

namespace Sanet.Kniffel.Protocol.Commands.Lobby
{
    public abstract class AbstractLobbyResponse<T>: AbstractCommandResponse<T>
        where T : AbstractLobbyCommand
    {
        protected override void Append<T2>(StringBuilder sb, T2 thing)
        {
            sb.Append(thing);
            sb.Append(AbstractLobbyCommand.Delimitter);
        }

        public AbstractLobbyResponse(T command) : base(command)
        {
        }

    }
}
