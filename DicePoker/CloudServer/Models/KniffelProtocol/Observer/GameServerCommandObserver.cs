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
        public event EventHandler<CommandEventArgs<TableInfoNeededCommand>> TableInfoNeededCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<RollReportCommand>> RollReportCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<FixDiceCommand>> FixDiceCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<ApplyScoreCommand>> ApplyScoreCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerLeftCommand>> PlayerLeftCommandReceived = delegate { };

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
            else if (commandName == TableInfoNeededCommand.COMMAND_NAME)
                TableInfoNeededCommandReceived(this, new CommandEventArgs<TableInfoNeededCommand>(new TableInfoNeededCommand(token)));
            else if (commandName == RollReportCommand.COMMAND_NAME)
                RollReportCommandReceived(this, new CommandEventArgs<RollReportCommand>(new RollReportCommand(token)));
            else if (commandName == FixDiceCommand.COMMAND_NAME)
                FixDiceCommandReceived(this, new CommandEventArgs<FixDiceCommand>(new FixDiceCommand(token)));
            else if (commandName == ApplyScoreCommand.COMMAND_NAME)
                ApplyScoreCommandReceived(this, new CommandEventArgs<ApplyScoreCommand>(new ApplyScoreCommand(token)));
            else if (commandName == PlayerLeftCommand.COMMAND_NAME)
                PlayerLeftCommandReceived(this, new CommandEventArgs<PlayerLeftCommand>(new PlayerLeftCommand(token)));
        }
    }
}
