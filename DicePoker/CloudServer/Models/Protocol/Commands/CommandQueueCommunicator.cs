//using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
using System.Threading.Tasks;
using Sanet.Kniffel.Models.Events;

namespace Sanet.Network.Protocol.Commands
{
    public abstract class CommandQueueCommunicator<T> : QueueCommunicator where T : CommandObserver, new()
    {
        protected T m_CommandObserver = new T();

        public CommandQueueCommunicator()
        {
            InitializeCommandObserver();
            base.ReceivedSomething += new EventHandler<KeyEventArgs<string>>(CommandQueueCommunicator_ReceivedSomething);
        }

        void CommandQueueCommunicator_ReceivedSomething(object sender, KeyEventArgs<string> e)
        {
            m_CommandObserver.messageReceived(e.Key);
        }

        protected abstract void InitializeCommandObserver();

        public virtual void Send(AbstractCommand command)
        {
            base.Send(command.Encode());
        }

        /// <summary>
        /// trying to dispose this object
        /// </summary>
        public override void Dispose()
        {
            base.ReceivedSomething -= CommandQueueCommunicator_ReceivedSomething;
            base.Dispose();

           // Task.Run(async delegate { await Task.Delay(1000); m_CommandObserver = null; });
            m_CommandObserver = null;
        }
    }
}
