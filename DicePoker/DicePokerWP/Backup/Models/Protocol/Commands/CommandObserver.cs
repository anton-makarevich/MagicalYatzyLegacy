using Sanet.Kniffel.Models.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanet.Network.Protocol.Commands
{
    public abstract class CommandObserver
    {
        public event EventHandler<StringEventArgs> CommandReceived = delegate { };

        protected abstract void receiveSomething(string line);

        public virtual void messageReceived(string line)
        {
            if (line == null)
            {
                return;
            }
            //LogManager.Log(LogLevel.MessageLow, "GameClient.m_CommandObserver_CommandReceived", " RECV -={0}=-", line);
            CommandReceived(this, new StringEventArgs(line));
            receiveSomething(line);
        }
    }
}
