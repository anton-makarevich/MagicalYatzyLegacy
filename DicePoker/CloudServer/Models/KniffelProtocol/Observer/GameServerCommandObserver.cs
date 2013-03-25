using System;
using System.Collections.Generic;
using System.Text;
using Sanet.Network.Protocol.Commands;
using Sanet.Kniffel.Models.Events;
using Sanet.Models;
using Sanet.Kniffel.Protocol.Commands.Game;

namespace Sanet.Kniffel.Protocol.Observer
{
    public class GameServerCommandObserver : CommandObserver
    {
        public event EventHandler<CommandEventArgs<DisconnectCommand>> DisconnectCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerSitOutChangedCommand>> SitOutChangedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerChatMessageCommand>> ChatMessageCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerPingCommand>> PlayerPingCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<TableInfoNeededCommand>> TableInfoNeededCommandReceived = delegate { };

        protected override void receiveSomething(string line)
        {
            StringTokenizer token = new StringTokenizer(line, AbstractCommand.Delimitter);
            string commandName = token.NextToken();
            if (commandName == DisconnectCommand.COMMAND_NAME)
                DisconnectCommandReceived(this, new CommandEventArgs<DisconnectCommand>(new DisconnectCommand(token)));
            else if (commandName == PlayerSitOutChangedCommand.COMMAND_NAME)
                SitOutChangedCommandReceived(this, new CommandEventArgs<PlayerSitOutChangedCommand>(new PlayerSitOutChangedCommand(token)));
            else if (commandName == PlayerChatMessageCommand.COMMAND_NAME)
                ChatMessageCommandReceived(this, new CommandEventArgs<PlayerChatMessageCommand>(new PlayerChatMessageCommand(token)));
            else if (commandName == PlayerPingCommand.COMMAND_NAME)
                PlayerPingCommandReceived(this, new CommandEventArgs<PlayerPingCommand>(new PlayerPingCommand(token)));
            //else if (commandName == TableInfoNeededCommand.COMMAND_NAME)
            //    TableInfoNeededCommandReceived(this, new CommandEventArgs<TableInfoNeededCommand>(new TableInfoNeededCommand(token)));
        }
    }
}
