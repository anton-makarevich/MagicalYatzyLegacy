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
        public event EventHandler<CommandEventArgs<PlayerChatMessageCommand>> ChatMessageCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerPingCommand>> PlayerPingCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<TableInfoNeededCommand>> TableInfoNeededCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<RollReportCommand>> RollReportCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<FixDiceCommand>> FixDiceCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<ApplyScoreCommand>> ApplyScoreCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerLeftCommand>> PlayerLeftCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerReadyCommand>> PlayerReadyCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<MagicRollCommand>> MagicRollCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<ManualChangeCommand>> ManualChangeCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerRerolledCommand>> PlayerRerolledCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<ChangeStyleCommand>> ChangeStyleCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerDeactivatedCommand>> PlayerDeactivatedCommandReceived = delegate { };

        protected override void receiveSomething(string line)
        {
            StringTokenizer token = new StringTokenizer(line, AbstractCommand.Delimitter);
            string commandName = token.NextToken();
            if (commandName == DisconnectCommand.COMMAND_NAME)
                DisconnectCommandReceived(this, new CommandEventArgs<DisconnectCommand>(new DisconnectCommand(token)));
            //else if (commandName == PlayerSitOutChangedCommand.COMMAND_NAME)
            //    SitOutChangedCommandReceived(this, new CommandEventArgs<PlayerSitOutChangedCommand>(new PlayerSitOutChangedCommand(token)));
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
            else if (commandName == PlayerReadyCommand.COMMAND_NAME)
                PlayerReadyCommandReceived(this, new CommandEventArgs<PlayerReadyCommand>(new PlayerReadyCommand(token)));
            else if (commandName == MagicRollCommand.COMMAND_NAME)
                MagicRollCommandReceived(this, new CommandEventArgs<MagicRollCommand>(new MagicRollCommand(token)));
            else if (commandName == ManualChangeCommand.COMMAND_NAME)
                ManualChangeCommandReceived(this, new CommandEventArgs<ManualChangeCommand>(new ManualChangeCommand(token)));
            else if (commandName == PlayerRerolledCommand.COMMAND_NAME)
                PlayerRerolledCommandReceived(this, new CommandEventArgs<PlayerRerolledCommand>(new PlayerRerolledCommand(token)));
            else if (commandName == ChangeStyleCommand.COMMAND_NAME)
                ChangeStyleCommandReceived(this, new CommandEventArgs<ChangeStyleCommand>(new ChangeStyleCommand(token)));
            else if (commandName == PlayerDeactivatedCommand.COMMAND_NAME)
                PlayerDeactivatedCommandReceived(this, new CommandEventArgs<PlayerDeactivatedCommand>(new PlayerDeactivatedCommand(token)));
        }
    }
}
