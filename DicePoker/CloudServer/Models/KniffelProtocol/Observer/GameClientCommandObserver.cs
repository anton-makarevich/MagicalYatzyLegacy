using System;
using System.Collections.Generic;
using System.Text;
using EricUtility;

namespace Sanet.Kniffel.Protocol.Observer
{
    public class GameClientCommandObserver : CommandObserver
    {
        public event EventHandler<CommandEventArgs<BetTurnEndedCommand>> BetTurnEndedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<BetTurnStartedCommand>> BetTurnStartedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<GameEndedCommand>> GameEndedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<GameStartedCommand>> GameStartedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerHoleCardsChangedCommand>> PlayerHoleCardsChangedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerJoinedCommand>> PlayerJoinedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerLeftCommand>> PlayerLeftCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerMoneyChangedCommand>> PlayerMoneyChangedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerTurnBeganCommand>> PlayerTurnBeganCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerTurnEndedCommand>> PlayerTurnEndedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerWonPotCommand>> PlayerWonPotCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<TableClosedCommand>> TableClosedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<TableInfoCommand>> TableInfoCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerSitOutChangedCommand>> PlayerSitOutChangedCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerInfoCommand>> PlayerInfoCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerChatMessageCommand>> ChatMessageCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<TipDealerCommand>> TipDealerCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerNotificationCommand>> PlayerNotificationCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<PlayerBoughtChipsCommand>> PlayerBoughtChipsCommandReceived = delegate { };

        protected override void receiveSomething(string line)
        {
            StringTokenizer token = new StringTokenizer(line, AbstractCommand.Delimitter);
            string commandName = token.NextToken();
            if (commandName == BetTurnEndedCommand.COMMAND_NAME)
                BetTurnEndedCommandReceived(this, new CommandEventArgs<BetTurnEndedCommand>(new BetTurnEndedCommand(token)));
            else if (commandName == BetTurnStartedCommand.COMMAND_NAME)
                BetTurnStartedCommandReceived(this, new CommandEventArgs<BetTurnStartedCommand>(new BetTurnStartedCommand(token)));
            else if (commandName == GameEndedCommand.COMMAND_NAME)
                GameEndedCommandReceived(this, new CommandEventArgs<GameEndedCommand>(new GameEndedCommand(token)));
            else if (commandName == GameStartedCommand.COMMAND_NAME)
                GameStartedCommandReceived(this, new CommandEventArgs<GameStartedCommand>(new GameStartedCommand(token)));
            else if (commandName == PlayerHoleCardsChangedCommand.COMMAND_NAME)
                PlayerHoleCardsChangedCommandReceived(this, new CommandEventArgs<PlayerHoleCardsChangedCommand>(new PlayerHoleCardsChangedCommand(token)));
            else if (commandName == PlayerJoinedCommand.COMMAND_NAME)
                PlayerJoinedCommandReceived(this, new CommandEventArgs<PlayerJoinedCommand>(new PlayerJoinedCommand(token)));
            else if (commandName == PlayerLeftCommand.COMMAND_NAME)
                PlayerLeftCommandReceived(this, new CommandEventArgs<PlayerLeftCommand>(new PlayerLeftCommand(token)));
            else if (commandName == PlayerMoneyChangedCommand.COMMAND_NAME)
                PlayerMoneyChangedCommandReceived(this, new CommandEventArgs<PlayerMoneyChangedCommand>(new PlayerMoneyChangedCommand(token)));
            else if (commandName == PlayerTurnBeganCommand.COMMAND_NAME)
                PlayerTurnBeganCommandReceived(this, new CommandEventArgs<PlayerTurnBeganCommand>(new PlayerTurnBeganCommand(token)));
            else if (commandName == PlayerTurnEndedCommand.COMMAND_NAME)
                PlayerTurnEndedCommandReceived(this, new CommandEventArgs<PlayerTurnEndedCommand>(new PlayerTurnEndedCommand(token)));
            else if (commandName == PlayerWonPotCommand.COMMAND_NAME)
                PlayerWonPotCommandReceived(this, new CommandEventArgs<PlayerWonPotCommand>(new PlayerWonPotCommand(token)));
            else if (commandName == TableClosedCommand.COMMAND_NAME)
                TableClosedCommandReceived(this, new CommandEventArgs<TableClosedCommand>(new TableClosedCommand(token)));
            else if (commandName == TableInfoCommand.COMMAND_NAME)
                TableInfoCommandReceived(this, new CommandEventArgs<TableInfoCommand>(new TableInfoCommand(token)));
            else if (commandName == PlayerSitOutChangedCommand.COMMAND_NAME)
                PlayerSitOutChangedCommandReceived(this, new CommandEventArgs<PlayerSitOutChangedCommand>(new PlayerSitOutChangedCommand(token)));
            else if (commandName == PlayerInfoCommand.COMMAND_NAME)
                PlayerInfoCommandReceived(this, new CommandEventArgs<PlayerInfoCommand>(new PlayerInfoCommand(token)));
            else if (commandName == PlayerChatMessageCommand.COMMAND_NAME)
                ChatMessageCommandReceived(this, new CommandEventArgs<PlayerChatMessageCommand>(new PlayerChatMessageCommand(token)));
            else if (commandName == TipDealerCommand.COMMAND_NAME)
                TipDealerCommandReceived(this, new CommandEventArgs<TipDealerCommand>(new TipDealerCommand(token)));
            else if (commandName == PlayerNotificationCommand.COMMAND_NAME)
                PlayerNotificationCommandReceived(this, new CommandEventArgs<PlayerNotificationCommand>(new PlayerNotificationCommand(token)));
            else if (commandName == PlayerBoughtChipsCommand.COMMAND_NAME)
                PlayerBoughtChipsCommandReceived(this, new CommandEventArgs<PlayerBoughtChipsCommand>(new PlayerBoughtChipsCommand(token)));
        }
    }
}
