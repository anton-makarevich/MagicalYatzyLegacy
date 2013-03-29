using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Sanet.Network.Protocol;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using Sanet.Kniffel.Protocol.Commands.Lobby;
using Sanet.Models.Collections;
using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.ViewModels;
using Sanet.Kniffel.Models.Interfaces;


namespace Sanet.Kniffel.Protocol
{
    public delegate void DisconnectDelegate();
    /// <summary>
    /// used by both winform and win8 clients
    /// </summary>
    public class LobbyTCPClient : TCPCommunicator
    {
        public event DisconnectDelegate ServerLost = delegate{};
        protected string m_PlayerName;
        
        public string PlayerName
        {
            get { return m_PlayerName; }
        }

        
        protected KniffelGameClient m_Client ;
        protected BlockingQueue<string> m_Incoming = new BlockingQueue<string>();

        public LobbyTCPClient()
            : base()
        {

            MessageReceived += OnMessageReceived;
        }

        public bool Connect()
        {
            return base.Connect();
        }

        public void LeaveTable(int idGame)
        {
            if (m_Client!=null)
            {
                m_Client.Disconnect();
                
            }
        }

        protected StringTokenizer ReceiveCommand(string expected)
        {
            string s = m_Incoming.Dequeue();
            StringTokenizer token = new StringTokenizer(s, AbstractLobbyCommand.Delimitter);
            string commandName = token.NextToken();
            while (s != null && commandName != expected)
            {
                s = m_Incoming.Dequeue();
                token = new StringTokenizer(s, AbstractLobbyCommand.Delimitter);
                commandName = token.NextToken();
            }
            return token;
        }

        protected string Receive(StreamReader reader)
        {
            string line;
            try
            {
                line = reader.ReadLine();
                LogManager.Log(LogLevel.MessageLow, "LobbyTCPClient.Receive", "{0} RECV [{1}]", m_PlayerName, line);
            }
            catch
            {
                return null;
            }
            return line;
        }

        public override void OnReceiveCrashed(Exception e)
        {
            if (e is IOException)
            {
                LogManager.Log(LogLevel.Error, "LobbyTCPClient.OnReceiveCrashed", "Lobby lost connection with server");
                Disconnect();
            }
            else
                base.OnReceiveCrashed(e);
        }

        public override void OnSendCrashed(Exception e)
        {
            if (e is IOException)
            {
                LogManager.Log(LogLevel.Error, "LobbyTCPClient.OnReceiveCrashed", "Lobby lost connection with server");
                Disconnect();
            }
            else
                base.OnSendCrashed(e);
        }
        public void Send(StreamWriter writer, AbstractCommand command)
        {
            writer.WriteLine(command.Encode());
        }
        public void Send(AbstractCommand command)
        {
            base.Send(command.Encode());
        }

        public void Disconnect()
        {

            m_Client.Disconnect();
            if (IsConnected)
            {
                Send(new DisconnectCommand());
                Close();
            }
        }
        
        protected virtual int GetJoinedSeat(ref int p_noPort, Player player)
        {
            JoinCommand command = new JoinCommand(p_noPort, player.Name, player.Password, player.Client,player.Language);
            Send(command);

            StringTokenizer token2 = ReceiveCommand(JoinResponse.COMMAND_NAME);
            if (!token2.HasMoreTokens())
                return -1;
            JoinResponse response2 = new JoinResponse(token2);
            p_noPort = response2.GameId;
            return response2.NoSeat;
        }

        public KniffelGameClient JoinTable(int p_noPort, Rules rule, Player p, IPlayGameView gui)
        {
            int noSeat = GetJoinedSeat(ref p_noPort, p);
            if (noSeat == -1)
            {
                LogManager.Log(LogLevel.MessageLow, "LobbyTCPClient.JoinTable", "Cannot sit at this table: #{0}", p_noPort);
                return null;
            }

            KniffelGameClient client = new KniffelGameClient(/*noSeat, m_PlayerName, p_noPort*/);
            client.SendedSomething += new EventHandler<KeyEventArgs<string>>(client_SendedSomething);
            if (gui != null)
            {
                gui.Game = client;
            }
            client.Start();
            p.Type = PlayerType.Local;
            client.JoinGame(p);
            m_Client= client;
            return client;
        }

        protected void client_SendedSomething(object sender, KeyEventArgs<string> e)
        {
            KniffelGameClient client = (KniffelGameClient)sender;
            Send(new GameCommand(client.GameId, e.Key));
        }
              

        //TODO: look closer how to convert this
        public void OnMessageReceived(object sender, object e)
        {
            var line = sender as string;
            StringTokenizer token = new StringTokenizer(line, AbstractLobbyCommand.Delimitter);
            String commandName = token.NextToken();
            if (commandName == GameCommand.COMMAND_NAME)
            {
                GameCommand c = new GameCommand(token);
                m_Client.Incoming(c.Command);
            }
            //else if (commandName.StartsWith(PlayerLeftCommand.COMMAND_NAME) && m_Clients.Count > 0)
            //{//HACK to support removing player from table when state is lost on the server
            //    m_Clients.Values.First().Incoming(line);
            //}
            else
            {
                m_Incoming.Enqueue(line);
            }
        }

    }
}
