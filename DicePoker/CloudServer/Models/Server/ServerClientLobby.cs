using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using Sanet.Network.Protocol.Commands;
using Sanet.Kniffel.Protocol.Observer;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Protocol.Commands.Lobby;
using Sanet.Kniffel.Models.Events;

namespace Sanet.Kniffel.Server
{
    /// <summary>
    /// a new class is instantiated per client-
    /// </summary>
    public class ServerClientLobby : CommandTCPCommunicator<LobbyServerCommandObserver>, IDisposable
    {
        public static ConcurrentDictionary<string, ServerClientLobby> playerToServerClientLobbyMapping = new ConcurrentDictionary<string, ServerClientLobby>();

        private string m_PlayerName = "?";
        private string _playerId;

        private readonly ServerLobby m_Lobby;
        KniffelGameServer m_Table ;

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }
        }
            

                
        public ServerClientLobby(ServerLobby lobby, string playerId)
            : base()
        {
            m_Lobby = lobby;
            _playerId = playerId;

            removeClientAfterDelayTimer = new System.Timers.Timer();
            removeClientAfterDelayTimer.Interval = 15 * 1000; //15 seconds wait time
            removeClientAfterDelayTimer.Elapsed +=removeClientAfterDelayTimer_Elapsed;
        }



        protected override void InitializeCommandObserver()
        {
            m_CommandObserver.CommandReceived +=m_CommandObserver_CommandReceived;
            m_CommandObserver.DisconnectCommandReceived +=m_CommandObserver_DisconnectCommandReceived;
            m_CommandObserver.JoinTableCommandReceived +=m_CommandObserver_JoinTableCommandReceived;
            m_CommandObserver.GameCommandReceived +=m_CommandObserver_GameCommandReceived;

            
        }

                        
        /// <summary>
        /// Game command recieved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_CommandObserver_GameCommandReceived(object sender, CommandEventArgs<GameCommand> e)
        {
            if (m_Table!=null)
            {
                m_Table.Incoming(e.Command.Command);//redirecting it to gameserver communicator
            }
            //else
            //{//HACK HACK when server does not have table (happens when server gets restarted)
            //    //send leave table command in case we dont have table info. this can happen when server restarts and no state info
            //    TableInfoNeededCommand cmd = null;
            //    if (e.Command != null && e.Command.Command != null)
            //    {
            //        StringTokenizer token = new StringTokenizer(e.Command.Command, AbstractCommand.Delimitter);
            //        string commandName = token.NextToken();

            //        if (commandName == TableInfoNeededCommand.COMMAND_NAME)
            //        {
            //            cmd = new TableInfoNeededCommand(token);
            //            Send1(new PlayerLeftCommand(cmd.PlayerPos));
            //        }
            //    }

            //    LogManager.Log(LogLevel.Error, "ServerClientLobby.CommandReceived", "No game found for TableID '{0}'", e.Command.TableID);
            //}
        }

        /*
        public void ForceJoinTable(int tableid)
        {
            LogManager.Log(LogLevel.Message, "ServerClientLobby.ForceJoinTable", "player:{0},  TableID '{1}'", PlayerName, tableid);
            JoinCommand joincommand = new JoinCommand(tableid, m_PlayerName);
            m_CommandObserver_JoinTableCommandReceived(null, new CommandEventArgs<JoinCommand>(joincommand));
        }*/

        
        /// <summary>
        /// Join to table command received
        /// </summary>
        void m_CommandObserver_JoinTableCommandReceived(object sender, CommandEventArgs<JoinCommand> e)
        {
            int tableID=e.Command.TableID;
            KniffelGame game;
          

            Player player = new Player();
            player.Name = e.Command.PlayerName;
            player.Password = e.Command.PlayerPass;
            if (player.Password.StartsWith(Player.FB_PREFIX))
                player.PicUrl = string.Format("https://graph.facebook.com/{0}/picture", player.Password.Replace(Player.FB_PREFIX, ""));
            else if (!string.IsNullOrEmpty(e.Command.PicUrl))
                player.PicUrl = e.Command.PicUrl;
            player.Language = e.Command.PlayerLanguage;
            player.Client = e.Command.PlayerClient;
            player.SelectedStyle = e.Command.SelectedStyle;
            
            //check no table# in params(-1)- then Autojoin
            if (tableID == -1)
            {
                LogManager.Log(LogLevel.Message, "ServerClientLobby.JoinTable", "{0} - autojoin", e.Command.PlayerName);

                tableID = m_Lobby.FindTableForUser(e.Command.GameRule, player);
                if (tableID == -1)
                    tableID = m_Lobby.CreateTable(e.Command.GameRule);
            }
            game = m_Lobby.GetGame(tableID);

            
            //new instance of servercommunicator for this client
            KniffelGameServer client = new KniffelGameServer(game);

            client.LeftTable += client_LeftTable;
            client.SendedSomething += client_SendedSomething;
            

            if (game.Players!=null)
            {
                //check players and remove 'dead'
                //removing inactive players
                //try
                //{
                //    List<Player> deadPlayers = new List<Player>();
                //    foreach (Player p in game.Players)
                //    {
                //        if ((DateTime.Now-p.LastTimeActive).TotalMinutes > 10)
                //            deadPlayers.Add(p);
                //    }
                //    foreach (Player p in deadPlayers)
                //    {
                //        game.LeaveGame(p);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    LogManager.Log("SCL.ReoveDead", ex);
                //}
                var exPlayer = game.Players.FirstOrDefault(f=>f.Name==e.Command.PlayerName);
                if (exPlayer!=null)
                {
                    if (exPlayer.ID != player.ID)
                    {
                        Send1(e.Command.EncodeResponse(-1, -1));
                        return;
                    }
                    else
                    {
                        player = exPlayer;
                        
                        //client = m_Table;
                    }

                }
            }
            player.IsReady = false;

            client.JoinGame(player);
            //ToDO: check if needed to close previous
            m_Table= client;

            client.Start();
            
            LogManager.Log(LogLevel.Message, "ServerClientLobby.m_CommandObserver_JoinTableCommandReceived", "> Client '{0}' seated ({3}) at table: {2}:{1}", m_PlayerName,client.Game.GameId, e.Command.TableID, client.Player.SeatNo);
            Send1(e.Command.EncodeResponse(player.SeatNo,tableID));
            //Thread.Sleep(200);
            //client.SendTableInfo();
            
        }
        

        void client_LeftTable(object sender, KeyEventArgs<int> e)
        {
            try
            {
                m_Table.LeftTable -= client_LeftTable;

                //try to dispose gameserver - we don't need it anymore
                m_Table.Dispose();
                m_Table = null;
                //TODO remove empty table

                RemovePlayersSvcLobby();
            }
            catch (Exception ex)
            {
                LogManager.Log("ServerLobby.LeftTable", ex);
            }
            LogManager.Log(LogLevel.Message, "ServerClientLobby.client_LeftTable", "game server for old instance was disposed, Player:{0}", PlayerName);
        }

        /// <summary>
        /// Game command sended from gameserver to client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SendedSomething(object sender, KeyEventArgs<string> e)
        {
            if (m_Table == null)
            {

                return;
            }
            Send1(new GameCommand(m_Table.Game.GameId, e.Key));
        }

        private System.Timers.Timer removeClientAfterDelayTimer ;
        private int numberOfTimesFoundClientNotConnectedInaRow = 0; 

        void removeClientAfterDelayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            removeClientAfterDelayTimer.Stop();
            LogManager.Log(LogLevel.Message, "ServerClientLoby.removeClientAfterDelayTimer_Elapsed", "remove serverclientLobby timer for player:{0}, firedTime:{1}", PlayerName, numberOfTimesFoundClientNotConnectedInaRow); 
            if (!IsConnected && m_Table!=null)
            {//it client didnt reconnect in given time...remove player
                //this should fire up the process
                m_Table.LeaveGame();
                
            }
            else
            {
                LogManager.Log(LogLevel.Message, "ServerClientLoby.removeClientAfterDelayTimer_Elapsed", "seems like client connected back, not removing player:{0}", PlayerName); 
          
                Interlocked.Exchange(ref numberOfTimesFoundClientNotConnectedInaRow, 0);
            }
            
        }

        public override void OnSendCrashed(Exception e)
        {
            base.OnSendCrashed(e);

            StartRemoveTimer();

            
        }

        void StartRemoveTimer()
        {
            if (!removeClientAfterDelayTimer.Enabled)
            {
                removeClientAfterDelayTimer.Interval = m_Table.RemoveTimerDelay * 1000;
                removeClientAfterDelayTimer.Start();
            }
        }

        public override void OnReceiveCrashed(Exception e)
        {
            
            base.OnReceiveCrashed(e);
            StartRemoveTimer();

        }

        private void RemovePlayersSvcLobby()
        {
            LogManager.Log(LogLevel.Message, "ServerClientLoby.RemovePlayersSvcLobby", "player:{0}", PlayerName);
            ServerClientLobby lobby;
            playerToServerClientLobbyMapping.TryRemove(_playerId, out lobby);
            
            Dispose();
        }

        private void CloseAndRemovePlayersSvClLobby()
        {
            RemovePlayersSvcLobby();
            //Task t = Close();
            ///t.Wait();
        }

        protected void Send1(AbstractCommand command)
        {
            //LogManager.Log(LogLevel.MessageLow, "ServerClientLobby.Send", "Server SEND to {0} [{1}]", m_PlayerName, line);
            base.Send(command);
            
        }

        protected void Send1(string line)
        {
            LogManager.Log(LogLevel.MessageLow, "ServerClientLobby.Send", "Server SEND to {0} [{1}]", m_PlayerName, line);
            base.Send(line);
            
        }

        void m_CommandObserver_CommandReceived(object sender, StringEventArgs e)
        {
            LogManager.Log(LogLevel.MessageLow, "ServerClientLobby.m_CommandObserver_CommandReceived", "Server RECV from {0} [{1}]", m_PlayerName, e.Str);
        }

        
        void m_CommandObserver_DisconnectCommandReceived(object sender, CommandEventArgs<DisconnectCommand> e)
        {
            LogManager.Log(LogLevel.Message, "ServerClientLobby.m_CommandObserver_DisconnectCommandReceived", "> Client disconnected: {0}", m_PlayerName);
            DisconnectCommand c = e.Command;
            CloseAndRemovePlayersSvClLobby();
        }


        async public void Dispose()
        {
            if (removeClientAfterDelayTimer != null)
            {
                removeClientAfterDelayTimer.Stop();
                removeClientAfterDelayTimer.Elapsed -= removeClientAfterDelayTimer_Elapsed;
                removeClientAfterDelayTimer.Dispose();
                removeClientAfterDelayTimer = null;
            }

            m_CommandObserver.CommandReceived -= m_CommandObserver_CommandReceived;
            m_CommandObserver.DisconnectCommandReceived -= m_CommandObserver_DisconnectCommandReceived;
            m_CommandObserver.JoinTableCommandReceived -= m_CommandObserver_JoinTableCommandReceived;
            m_CommandObserver.GameCommandReceived -= m_CommandObserver_GameCommandReceived;
            try
            {
                await Close();
            }
            catch { }
        }
    }
}
