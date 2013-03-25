using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Protocol.Commands.Lobby;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;

namespace Sanet.Kniffel.Protocol.Observer
{
    public class LobbyServerCommandObserver : CommandObserver
    {
        public event EventHandler<CommandEventArgs<DisconnectCommand>> DisconnectCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<JoinCommand>> JoinTableCommandReceived = delegate { };
        public event EventHandler<CommandEventArgs<GameCommand>> GameCommandReceived = delegate { };
                
        protected override void receiveSomething(string line)
        {
            StringTokenizer token = new StringTokenizer(line, AbstractLobbyCommand.Delimitter);
            string commandName = token.NextToken();
            
            if (commandName == DisconnectCommand.COMMAND_NAME)
                DisconnectCommandReceived(this, new CommandEventArgs<DisconnectCommand>(new DisconnectCommand(token)));
            else if (commandName == JoinCommand.COMMAND_NAME)
                JoinTableCommandReceived(this, new CommandEventArgs<JoinCommand>(new JoinCommand(token)));
            else if (commandName == GameCommand.COMMAND_NAME)
                GameCommandReceived(this, new CommandEventArgs<GameCommand>(new GameCommand(token)));
            
        }
    }
}
