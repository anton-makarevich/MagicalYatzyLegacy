using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sanet.Network.Protocol.Commands
{
    public abstract class CommandTCPCommunicator<T> : TCPCommunicator where T : CommandObserver, new()
    {
        protected T m_CommandObserver = new T();

        public CommandTCPCommunicator() : base()
        {
            InitializeCommandObserver();
        }

        protected abstract void InitializeCommandObserver();

        protected override async Task<string> Receive()
        {
            string line = await base.Receive();
            m_CommandObserver.messageReceived(line);
            return line;
        }

        protected virtual void Send(AbstractCommand command)
        {
            base.Send(command.Encode());
        }
    }
}
