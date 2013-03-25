using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Linq;
using Sanet.Kniffel.Models;


namespace Sanet.Kniffel.Server
{

    /// <summary>
    /// we are only supporting career table
    /// since m_games contain both carrier and training tables
    /// </summary>
    public class ServerLobby
    {
        private readonly int m_NoPort;
        //private readonly TcpListener m_SocketServer; ??maybe something old?

        private readonly List<string> m_UsedNames = new List<string>();

        private object gamesDictLockObj = new object();
        private readonly Dictionary<int, KniffelGame> m_Games = new Dictionary<int, KniffelGame>();

        

        public KniffelGame GetGame(int id)
        {
            lock (gamesDictLockObj)
            {
                return m_Games[id];
            }
        }

                
        public KniffelGame GetGameByUser(string name)
        {
            lock (gamesDictLockObj)
            {
                var g = m_Games.Values.Where(f => f.Players.Any(p => p.Name == name)).FirstOrDefault();
                return g;
            }
        }

        

        public ServerLobby(int port)
        {
            m_NoPort = port;
            
            LogManager.Log(LogLevel.Message, "BluffinPokerServer", "Server started on port {0}", port);
        }

        private object usedNameLockObj = new object();

        
        public int CreateTable()
        {
            
            KniffelGame game = new KniffelGame();

            int m_LastUsedID = 0;
            lock (gamesDictLockObj)
            {
                while (m_Games.ContainsKey(m_LastUsedID))
                {
                    m_LastUsedID++;
                }
                game.GameId = m_LastUsedID;
                m_Games.Add(m_LastUsedID, game);
            }
            
            return m_LastUsedID;
        }

        /// <summary>
        /// </summary>
        /// <param name="tableId"></param>
        private void RemoveTables(List<int> tableIdToRemove)
        {
            lock (gamesDictLockObj)
            {
                foreach (int i in tableIdToRemove)
                {
                    KniffelGame game = m_Games[i];
                    //game.Dispose();
                    m_Games.Remove(i);
                }
            }
        }

                       

        /// <summary>
        /// Method to automatically find a table for the user
        /// </summary>
        public int FindTableForUser()
        { 
            
            if (m_Games.Count == 0)
            {
                LogManager.Log(LogLevel.Message, "ServerLobby.FindTableForUser", "No games To join");
                return -1;
            }
            
            //so - all tables
            var game= m_Games.Values.OrderByDescending(f=>f.PlayersNumber).Where(f=>f.PlayersNumber<4).FirstOrDefault();
            if (game != null)
                return game.GameId;
            else
                return -1;
        }

        
        /// <summary>
        /// methods to check id user is online
        /// </summary>
        public bool IsUserOnline(string name)
        {
            return (m_Games.Values.SelectMany(f => f.Players).FirstOrDefault(p => p.Name == name) != null);
        }
    }
}
