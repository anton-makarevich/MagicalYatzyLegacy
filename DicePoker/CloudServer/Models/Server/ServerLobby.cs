using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Linq;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Protocol;


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


        public int CreateTable(Rules rule)
        {
            
            KniffelGame game = new KniffelGame();
            game.Rules = new KniffelRule(rule);
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
        public int FindTableForUser(Rules rule, Player player)
        {
            KniffelGame game;
            

            var gamesByRule = m_Games.Values.Where(f => f.Rules.Rule == rule).ToList();
            if (gamesByRule.Count == 0)
            {
                LogManager.Log(LogLevel.Message, "ServerLobby.FindTableForUser", "No games To join");
                return -1;
            }
            //first try to find where this player is playing

            game = gamesByRule.FirstOrDefault(f => f.Players.FirstOrDefault(p => p.Name==player.Name) != null);
            if (game != null)
            {
                var p = game.Players.FirstOrDefault(f =>f.ID == player.ID);
                if (p == null)
                    return game.GameId;
                else
                    gamesByRule.Remove(game);
            }
            
            //so - all tables
            game = gamesByRule.Where(f => f.PlayersNumber < 4).OrderByDescending(f => f.PlayersNumber).FirstOrDefault();
            if (game != null)
                return game.GameId;
            else
                return -1;
        }

        public List<TupleTableInfo> GetTablesList()
        {
            List<TupleTableInfo> rv = new List<TupleTableInfo>();
            rv.Add(new TupleTableInfo(-1, new List<string> { " ", " ", " " }, Rules.krBaby));
            var games = m_Games.Values.Where(f => f.PlayersNumber > 0 && f.PlayersNumber < 4).ToList();
            if (games.Count > 3)
                games = games.Take(3).ToList();

            foreach (var game in games)
            {
                rv.Add(new TupleTableInfo(game.GameId, game.Players.Select(p=>p.Name).ToList(), game.Rules.Rule));
            }
            return rv;
        }
        /// methods to check id user is online
        /// </summary>
        public bool IsUserOnline(string name)
        {
            return (m_Games.Values.SelectMany(f => f.Players).FirstOrDefault(p => p.Name == name) != null);
        }
    }
}
