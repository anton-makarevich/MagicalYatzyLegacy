using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Kniffel.Protocol.Commands.Game;
using Sanet.Kniffel.Protocol.Observer;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Sanet.Kniffel.Models
{
    public class KniffelGameClient : CommandQueueCommunicator<GameClientCommandObserver>, IKniffelGame
    {
        public string MyName { get; set; }

        object syncRoot = new object();

        int[] lastRollResults;
        List<int> fixedRollResults = new List<int>();
        Queue<int> thisTurnValues = new Queue<int>();

        /// <summary>
        /// actual dices here :)
        /// </summary>
        Random rand = new Random();




        public KniffelGameClient()
        {
            //def rule
            Rules = new KniffelRule(Models.Rules.krExtended );
        }

        #region Events
        /// <summary>
        /// Notify that move started
        /// </summary>
        public event EventHandler<MoveEventArgs> MoveChanged;

        /// <summary>
        /// Notify that game ended
        /// </summary>
        public event EventHandler GameFinished;

        /// <summary>
        /// Notify that dice was fixed
        /// </summary>
        public event EventHandler<FixDiceEventArgs> DiceFixed;

        /// <summary>
        /// current player rolled dices
        /// </summary>
        public event EventHandler<RollEventArgs> DiceRolled;

        /// <summary>
        /// current player manually changed dice
        /// </summary>
        public event EventHandler<RollEventArgs> DiceChanged;

        /// <summary>
        /// current player applied roll result
        /// </summary>
        public event EventHandler<ResultEventArgs> ResultApplied;

        /// <summary>
        /// current player join game
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerJoined;

        public event EventHandler<PlayerEventArgs> PlayerLeft;

        public event EventHandler<PlayerEventArgs> PlayerReady;

        /// <summary>
        /// current player used Magical Roll
        /// </summary>
        public event EventHandler<PlayerEventArgs> MagicRollUsed;

        /// <summary>
        /// Chat message received
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> OnChatMessage;

        #endregion

        #region Properties
        public int GameId { get; set; }
        public bool IsPlaying { get; set; }
        public string Password { get; set; }

        public DieResult LastDiceResult
        {
            get
            {
                if (lastRollResults == null)
                    return null;
                return
                    new DieResult() 
                    {
                         DiceResults=lastRollResults.ToList()
                    };
            }
        }

        public List<Player> Players
        {
            get;
            set;
        }

        public int FixedDicesCount
        {
            get
            {
                if (fixedRollResults == null)
                    return 0;
                return fixedRollResults.Count;
            }
        }

        /// <summary>
        /// Players Count
        /// </summary>
        public int PlayersNumber
        {
            get
            {
                if (Players == null)
                    return 0;
                return Players.Count;
            }
        }
        /// <summary>
        /// Game rules
        /// </summary>
        public KniffelRule Rules { get; set; }
        /// <summary>
        /// Move number
        /// </summary>
        public int Move { get; set; }
        /// <summary>
        /// Player which gas a move
        /// </summary>
        public Player CurrentPlayer { get; set; }

        
        private bool _RerollMode;
        public bool RerollMode
        {
            get { return _RerollMode; }
            set
            {
                _RerollMode = value;
                if (!value)
                    thisTurnValues = new Queue<int>();
                else
                    fixedRollResults = new List<int>();
                LogManager.Log(LogLevel.Message, "Game.RerollMode", "RerollMode set to {0}", value);
            }
        }

        
#endregion

#region Methods
        /// <summary>
        /// Change next player to move
        /// </summary>
        public void DoMove()
        {
            
            //report to all that player changed
            if (MoveChanged != null)
                MoveChanged(this, new MoveEventArgs(CurrentPlayer, Move));
        }
        /// <summary>
        /// Increase move or end game
        /// </summary>
        public void NextMove()
        {
            
                if (Move == Rules.MaxMove)
                {
                    Players=Players.OrderByDescending(f => f.Total).ToList();
                    CurrentPlayer = Players.First();
                    if (GameFinished != null)
                        GameFinished(this, null);
                }
                else
                {
                    
                    Move++;
                    DoMove();
                }
            
        }
        
        /// <summary>
        /// fix singe dice with value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isfixed"></param>
        public void FixDice(int value, bool isfixed)
        {
            lock (syncRoot)
            {
                Send(new FixDiceCommand(CurrentPlayer.Name,value,isfixed));

                
            }
        }

        /// <summary>
        /// fix all dice with value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isfixed"></param>
        public void FixAllDices(int value, bool isfixed)
        {
            lock (syncRoot)
            {
                if (isfixed)
                {
                    int count = LastDiceResult.NumDiceOf(value);
                    int fixedcount = fixedRollResults.Count(f => f == value);
                    for (int i = 0; i < (count - fixedcount); i++)
                    {
                        fixedRollResults.Add(value);
                        if (DiceFixed != null)
                            DiceFixed(this, new FixDiceEventArgs(CurrentPlayer, value, isfixed));
                    }
                }
                else
                {
                    while (fixedRollResults.Contains(value))
                    {
                        fixedRollResults.Remove(value);
                        if (DiceFixed != null)
                            DiceFixed(this, new FixDiceEventArgs(CurrentPlayer, value, isfixed));
                    }
                }
            }
            
        }

        /// <summary>
        /// Player wants to move. generating value here with network play in mind
        /// </summary>
        public void ReportRoll()
        {
            
            lock (syncRoot)
            {
                //if (CurrentPlayer!=null)
                    Send(new RollReportCommand(MyName, new List<int>() { 0,0,0,0,0}));

            }
        }

        /// <summary>
        /// Player changed dice result manually
        /// </summary>
        public void ManualChange(bool isfixed, int oldvalue, int newvalue)
        {
            lock (syncRoot)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (lastRollResults[i] == oldvalue /*&& fixedRollResults.Contains(oldvalue)==isfixed*/)
                    {
                        lastRollResults[i] = newvalue;
                        break;
                    }
                }
                if (DiceChanged != null)
                    DiceChanged(this, new RollEventArgs(CurrentPlayer, lastRollResults));
            }
        }
        /// <summary>
        /// Player used 'MagicRoll'
        /// </summary>
        public void ReporMagictRoll()
        {
            lock (syncRoot)
            {
                //TODO: check artifacts?
                Send(new MagicRollCommand(MyName));
            }
        }
                

        public void ApplyScore(RollResult result)
        {
            LogManager.Log(LogLevel.Message,"Game.ApplyScore","Applying score {0} of {1} for {2}", result.PossibleValue,result.ScoreType,CurrentPlayer.Name);
            
            Send(new ApplyScoreCommand(CurrentPlayer.Name,result));         
            
            
           
        }

        public void StartGame()
        {
            
            {
                if (IsPlaying || Players==null)
                    return;
            }
            bool bAllReady = true;
            //Checking if everyone is ready
            lock (syncRoot)
            {
                foreach (Player player in Players)
                {
                    if (!player.IsReady)
                    {
                        bAllReady = false;
                        break;
                    }
                }
            }
            //if yes - starting game
            if (bAllReady)
            {
                Move = 1;
                //DoMove();
            }
        }

        /// <summary>
        /// On Play Again
        /// </summary>
        public void RestartGame()
        {
            foreach (Player p in Players)
            {
                p.Init();
            }
            Players = Players.OrderBy(f => f.SeatNo).ToList();
            SetPlayerReady(true);
            
        }

        public void JoinGame(Player player)
        {
            lock (syncRoot)
            {
                if (Players == null)
                    Players = new List<Player>();
                var explayer = Players.FirstOrDefault(f => f.SeatNo == player.SeatNo);
                if (explayer == null)
                {
                    Players.Add(player);
                    Players = Players.OrderBy(f => f.SeatNo).ToList();
                }
                else
                    explayer = player;
                player.Game = this;

                player.Type = (player.Name == MyName) ? PlayerType.Local : PlayerType.Network;
                player.Password = (player.Name == MyName) ? 
                    ViewModelProvider.GetViewModel<NewOnlineGameViewModel>().SelectedPlayer.Password : 
                    "";

                if (PlayerJoined != null)
                    PlayerJoined(this, new PlayerEventArgs(player));
            }
        }

        public void LeaveGame(Player player)
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
                if (PlayerLeft != null)
                    PlayerLeft(null, new PlayerEventArgs(player));
                player = null;
            }
        }

        public void SendChatMessage(ChatMessage message)
        {
            Send (new PlayerChatMessageCommand(message.SenderName,message.Message,message.ReceiverName,message.IsPrivate));
        }

        /// <summary>
        /// returns wheather we have at least one fixed dice of this value
        /// </summary>
        public bool IsDiceFixed(int value)
        {
            return fixedRollResults.Contains(value);
        }

        public void SetPlayerReady(Player player, bool isready)
        {
            Send(new PlayerReadyCommand(player.Name,isready));
        }
        public void SetPlayerReady(bool isready)
        {
            Send(new PlayerReadyCommand(MyName, isready));
        }

        protected override void InitializeCommandObserver()
        {
            LogManager.Log(LogLevel.Message, "GameClient.InitObserver", "Added handlers at game client");
            m_CommandObserver.CommandReceived += m_CommandObserver_CommandReceived;
            m_CommandObserver.PlayerJoinedCommandReceived += m_CommandObserver_PlayerJoinedCommandReceived;
            m_CommandObserver.TableInfoCommandReceived += m_CommandObserver_TableInfoCommandReceived;
            m_CommandObserver.ChatMessageCommandReceived += m_CommandObserver_ChatMessageCommandReceived;
            m_CommandObserver.RollReportCommandReceived += m_CommandObserver_RollReportCommandReceived;
            m_CommandObserver.FixDiceCommandReceived += m_CommandObserver_FixDiceCommandReceived;
            m_CommandObserver.ApplyScoreCommandReceived += m_CommandObserver_ApplyScoreCommandReceived;
            m_CommandObserver.RoundChangedCommandReceived += m_CommandObserver_RoundChangedCommandReceived;
            m_CommandObserver.PlayerLeftCommandReceived += m_CommandObserver_PlayerLeftCommandReceived;
            m_CommandObserver.PlayerReadyCommandReceived += m_CommandObserver_PlayerReadyCommandReceived;
            m_CommandObserver.GameEndedCommandReceived += m_CommandObserver_GameEndedCommandReceived;
            m_CommandObserver.MagicRollCommandReceived += m_CommandObserver_MagicRollCommandReceived;
        }

        void m_CommandObserver_MagicRollCommandReceived(object sender, CommandEventArgs<MagicRollCommand> e)
        {
            var player = Players.Find(f => f.Name == e.Command.Name);
            if (MagicRollUsed != null)
                MagicRollUsed(null,new PlayerEventArgs(player));
        }

        void m_CommandObserver_ChatMessageCommandReceived(object sender, CommandEventArgs<PlayerChatMessageCommand> e)
        {
            if (OnChatMessage!=null)
            OnChatMessage(null, new ChatMessageEventArgs(new ChatMessage()
            { 
                IsPrivate=e.Command.IsPrivate,
                Message=e.Command.Message,
                ReceiverName=e.Command.ReceiverName,
                SenderName=e.Command.Name
            }));
        }

        void m_CommandObserver_GameEndedCommandReceived(object sender, CommandEventArgs<GameEndedCommand> e)
        {
            if (GameFinished != null)
                GameFinished(null, null);
        }

        void m_CommandObserver_PlayerReadyCommandReceived(object sender, CommandEventArgs<PlayerReadyCommand> e)
        {
            var player = Players.Find(f => f.Name == e.Command.Name);
            player.IsReady = e.Command.IsReady;
            if (PlayerReady != null)
                PlayerReady(null, new PlayerEventArgs(player));
        }

        void m_CommandObserver_PlayerLeftCommandReceived(object sender, CommandEventArgs<PlayerLeftCommand> e)
        {
            var p = Players.FirstOrDefault(f => f.Name == e.Command.Name);
            if (p != null)
            {
                LeaveGame(p);
            }
        }

        void m_CommandObserver_RoundChangedCommandReceived(object sender, CommandEventArgs<RoundChangedCommand> e)
        {
            lock (syncRoot)
            {
                LogManager.Log(LogLevel.Message, "GameClient", "Change round received, new player {0}, new round #{1}", e.Command.Name, e.Command.Round);
                fixedRollResults = new List<int>();
                if (CurrentPlayer != null)
                    CurrentPlayer.IsMoving = false;
                foreach (Player p in Players)
                    p.IsMoving = false;

                Move = e.Command.Round;
                CurrentPlayer = Players.Find(f => f.Name == e.Command.Name);
                CurrentPlayer.IsMoving = true;
                CurrentPlayer.Roll = 1;
                DoMove();
            }
        }

        void m_CommandObserver_ApplyScoreCommandReceived(object sender, CommandEventArgs<ApplyScoreCommand> e)
        {
            lock (syncRoot)
            {
                LogManager.Log(LogLevel.Message, "GameClient", "Result {1} applied by player {0}", e.Command.Name,e.Command.PossibleValue);
                var p = Players.Find(f => f.Name == e.Command.Name);
                RollResult result = p.Results.Find(f => f.ScoreType == e.Command.ScoreType);
                result.PossibleValue = e.Command.PossibleValue;
                result.HasBonus = e.Command.HasBonus;
                //sending result to everyone
                if (ResultApplied != null)
                    ResultApplied(this, new ResultEventArgs(p, result));
                //check for numeric bonus and apply it
            }
            
        }

        void m_CommandObserver_FixDiceCommandReceived(object sender, CommandEventArgs<FixDiceCommand> e)
        {
            var p = Players.FirstOrDefault(f => f.Name == e.Command.Name);
            if (p == null)
                return;
            if (CurrentPlayer == null)
                CurrentPlayer = p;
            if (DiceFixed != null)
                DiceFixed(this, new FixDiceEventArgs(p, e.Command.Value, e.Command.IsFixed));
        }

        void m_CommandObserver_RollReportCommandReceived(object sender, CommandEventArgs<RollReportCommand> e)
        {
            lock (syncRoot)
            {
                var p = Players.FirstOrDefault(f => f.Name == e.Command.Name);
                if (p == null)
                    return;
                if (CurrentPlayer == null)
                    CurrentPlayer = p;
                lastRollResults = e.Command.LastResult.ToArray();

                if (DiceRolled != null)
                    DiceRolled(this, new RollEventArgs(p, lastRollResults));
            }
        }

        void m_CommandObserver_TableInfoCommandReceived(object sender, CommandEventArgs<Protocol.Commands.Game.TableInfoCommand> e)
        {
            
            Move = e.Command.Round;
            //remove players 'dead' players from the UI
            if (Players != null)
            {
                foreach (Player exp in Players)
                {
                    var s = e.Command.Seats.FirstOrDefault(f => f.Name == exp.Name);
                    if (s == null)
                        LeaveGame(exp);
                }
            }

            foreach (var seat in e.Command.Seats)
            {
                //await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { });
                var player = new Player();

                player.Game = this;
                player.SeatNo = seat.SeatNo;
                player.Name = seat.Name;
                player.Client = seat.ClientType;
                player.IsMoving = seat.IsPlaying;
                player.Language = seat.Language;
                player.PicUrl = seat.PhotoUri;
                player.IsReady = seat.IsReady;

                if (PlayerReady != null)
                    PlayerReady(null, new PlayerEventArgs(player));

                int resCount = 0;
                foreach (var result in player.Results)
                {
                    var value = seat.Results[resCount];
                    if (value != -1)
                        result.Value = value;
                    resCount++;
                    value = seat.Results[resCount];
                    if (value != 0)
                        result.HasBonus = true;
                    resCount++;
                }

                JoinGame(player);
            }
            
            
            
        }

        void m_CommandObserver_PlayerJoinedCommandReceived(object sender, CommandEventArgs<Protocol.Commands.Game.PlayerJoinedCommand> e)
        {

            lock (syncRoot)
            {
                var player = new Player();

                player.Name = e.Command.Name;
                player.SeatNo = e.Command.SeatNo;
                player.Client = e.Command.PlayerClient;
                player.Language = e.Command.PlayerLanguage;
                player.Type = PlayerType.Network;
                JoinGame(player);
            }
            
            
        }

        void m_CommandObserver_CommandReceived(object sender, StringEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                m_CommandObserver.CommandReceived -= m_CommandObserver_CommandReceived;
                m_CommandObserver.PlayerJoinedCommandReceived -= m_CommandObserver_PlayerJoinedCommandReceived;
                m_CommandObserver.TableInfoCommandReceived -= m_CommandObserver_TableInfoCommandReceived;
                m_CommandObserver.ChatMessageCommandReceived -= m_CommandObserver_ChatMessageCommandReceived;
                m_CommandObserver.RollReportCommandReceived -= m_CommandObserver_RollReportCommandReceived;
                m_CommandObserver.FixDiceCommandReceived -= m_CommandObserver_FixDiceCommandReceived;
                m_CommandObserver.ApplyScoreCommandReceived -= m_CommandObserver_ApplyScoreCommandReceived;
                m_CommandObserver.RoundChangedCommandReceived -= m_CommandObserver_RoundChangedCommandReceived;
                m_CommandObserver.PlayerLeftCommandReceived -= m_CommandObserver_PlayerLeftCommandReceived;
                m_CommandObserver.PlayerReadyCommandReceived -= m_CommandObserver_PlayerReadyCommandReceived;
                m_CommandObserver.GameEndedCommandReceived -= m_CommandObserver_GameEndedCommandReceived;
                m_CommandObserver.MagicRollCommandReceived -= m_CommandObserver_MagicRollCommandReceived;

                Send(new DisconnectCommand());
            }
        }

#endregion
    }
}
