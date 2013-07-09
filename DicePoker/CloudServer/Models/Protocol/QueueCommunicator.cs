//using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
using System.Threading.Tasks;
using Sanet.Models.Collections;
using Sanet.Kniffel.Models.Events;

namespace Sanet.Network.Protocol
{
    public class QueueCommunicator
    {
        protected bool m_IsConnected;
        protected BlockingQueue<String> m_Incoming = new BlockingQueue<String>();
        public event EventHandler<KeyEventArgs<string>> ReceivedSomething = delegate { };
        public event EventHandler<KeyEventArgs<string>> SendedSomething = delegate { };
        public QueueCommunicator()
        {
            m_IsConnected = false;
        }

        public virtual bool IsConnected
        {
            get { return m_IsConnected; }
            set { m_IsConnected = value; } 
        }

        protected string Receive()
        {
            string line = m_Incoming.Dequeue();
            if (ReceivedSomething != null)
            {
                ReceivedSomething(this, new KeyEventArgs<string>(line));
            }
            return line;
        }

        public void Send(string line)
        {
            if (m_IsConnected && SendedSomething != null)
            {
                SendedSomething(this, new KeyEventArgs<string>(line));
            }
        }
        protected virtual void Run()
        {
            while (IsConnected)
            {
                Receive();
            }
        }

        public void Start()
        {
            m_IsConnected = true;
#if WinRT
//            //TODO: think about this
                Task t =new Task(Run);
                t.Start();
                t.AsAsyncAction();
#else
            new Thread(new ThreadStart(Run)).Start();
#endif
        }

       
        public virtual void Incoming(string message)
        {
            m_Incoming.Enqueue(message);
            LogManager.Log(LogLevel.Message, "Client.Incoming", "data:{0}", message);
            //try
            //{
            //    if (/*IsConnected &&*/ ReceivedSomething != null)
            //    {
            //        ReceivedSomething(this, new KeyEventArgs<string>(message));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var t = ex.Message;
            //}
        }

        public virtual void Dispose()
        {
            m_Incoming = null;
            ReceivedSomething = null;
            SendedSomething = null;
        }
    }
}
