using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sanet.Kniffel.Models.Events
{
    public class CommandEventArgs<T> : EventArgs
       where T : AbstractCommand
    {
        private readonly T m_Command;
        public T Command { get { return m_Command; } }

        public CommandEventArgs(T c)
        {
            m_Command = c;
        }
    }
}