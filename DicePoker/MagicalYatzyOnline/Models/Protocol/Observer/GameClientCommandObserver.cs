using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Protocol.Commands.Game;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanet.Kniffel.Protocol.Observer
{
    public class GameClientCommandObserver : CommandObserver
    {
        public event EventHandler<CommandEventArgs<GameEndedCommand>> GameEndedCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<GameStartedCommand>> GameStartedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerJoinedCommand>> PlayerJoinedCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<PlayerLeftCommand>> PlayerLeftCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<PlayerTurnBeganCommand>> PlayerTurnBeganCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<PlayerTurnEndedCommand>> PlayerTurnEndedCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<PlayerWonPotCommand>> PlayerWonPotCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<TableClosedCommand>> TableClosedCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<TipDealerCommand>> TipDealerCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<PlayerNotificationCommand>> PlayerNotificationCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<PlayerInfoCommand>> PlayerInfoCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<PlayerBoughtChipsCommand>> PlayerBoughtChipsCommandReceived = delegate { };
        //public event EventHandler<CommandEventArgs<PlayerSitOutChangedCommand>> PlayerSitOutChangedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerChatMessageCommand>> ChatMessageCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<TableInfoCommand>> TableInfoCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<RollReportCommand>> RollReportCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<FixDiceCommand>> FixDiceCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<ApplyScoreCommand>> ApplyScoreCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<RoundChangedCommand>> RoundChangedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerLeftCommand>> PlayerLeftCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerReadyCommand>> PlayerReadyCommandReceived = delegate { };

        protected override void receiveSomething(string line)
        {
            StringTokenizer token = new StringTokenizer(line, AbstractCommand.Delimitter);
            string commandName = token.NextToken();
            if (commandName == PlayerChatMessageCommand.COMMAND_NAME)
                ChatMessageCommandReceived(this, new CommandEventArgs<PlayerChatMessageCommand>(new PlayerChatMessageCommand(token)));
            else if (commandName == PlayerJoinedCommand.COMMAND_NAME)
                PlayerJoinedCommandReceived(this, new CommandEventArgs<PlayerJoinedCommand>(new PlayerJoinedCommand(token)));
            else if (commandName == TableInfoCommand.COMMAND_NAME)
                TableInfoCommandReceived(this, new CommandEventArgs<TableInfoCommand>(new TableInfoCommand(token)));
            else if (commandName == RollReportCommand.COMMAND_NAME)
                RollReportCommandReceived(this, new CommandEventArgs<RollReportCommand>(new RollReportCommand(token)));
            else if (commandName == FixDiceCommand.COMMAND_NAME)
                FixDiceCommandReceived(this, new CommandEventArgs<FixDiceCommand>(new FixDiceCommand(token)));
            else if (commandName == ApplyScoreCommand.COMMAND_NAME)
                ApplyScoreCommandReceived(this, new CommandEventArgs<ApplyScoreCommand>(new ApplyScoreCommand(token)));
            else if (commandName == RoundChangedCommand.COMMAND_NAME)
                RoundChangedCommandReceived(this, new CommandEventArgs<RoundChangedCommand>(new RoundChangedCommand(token)));
            else if (commandName == PlayerLeftCommand.COMMAND_NAME)
                PlayerLeftCommandReceived(this, new CommandEventArgs<PlayerLeftCommand>(new PlayerLeftCommand(token)));
            else if (commandName == PlayerReadyCommand.COMMAND_NAME)
                PlayerReadyCommandReceived(this, new CommandEventArgs<PlayerReadyCommand>(new PlayerReadyCommand(token)));
            else if (commandName == GameEndedCommand.COMMAND_NAME)
                GameEndedCommandReceived(this, new CommandEventArgs<GameEndedCommand>(new GameEndedCommand()));
        }
    }
}
