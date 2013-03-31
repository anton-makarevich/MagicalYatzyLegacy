﻿using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Kniffel.Protocol.Commands.Game;
using Sanet.Kniffel.Protocol.Observer;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Sanet.Kniffel.Models
{
    public class KniffelGameClient : CommandQueueCommunicator<GameClientCommandObserver>, IKniffelGame
    {
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

        /// <summary>
        /// current player used Magical Roll
        /// </summary>
        public event EventHandler<PlayerEventArgs> MagicRollUsed;

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
                Send(new FixDiceCommand(CurrentPlayer.SeatNo,value,isfixed));

                
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
                if (CurrentPlayer!=null)
                    Send(new RollReportCommand(CurrentPlayer.SeatNo, new List<int>() { 0,0,0,0,0}));

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
                //set magic value
                //1) check how much free hands we have
                int freeHandsIndex = 0;
                Dictionary<int,KniffelScores> freeHands = new Dictionary<int,KniffelScores>();
                foreach (var score in KniffelRule.PokerHands)
                {
                    if (!CurrentPlayer.GetResultForScore(score).HasValue)
                    {
                        freeHands.Add(freeHandsIndex, score);
                        freeHandsIndex++;
                    }
                }
                if (freeHands.Count==0)
                    ReportRoll();// no available hands - just roll
                else
                {
                    setMagicResults( freeHands[rand.Next(freeHands.Count)]);

                    if (MagicRollUsed != null)
                        MagicRollUsed(this, new PlayerEventArgs(CurrentPlayer));
                    //CurrentPlayer.OnMagicRollUsed();
                    foreach (int result in lastRollResults)
                        thisTurnValues.Enqueue(result);
                    //roll report
                    if (DiceRolled != null)
                        DiceRolled(this, new RollEventArgs(CurrentPlayer, lastRollResults));
                }
            }
        }

        void setMagicResults(KniffelScores hands)
        {
            int firstinrow;
            switch (hands)
            {
                case KniffelScores.ThreeOfAKind:
                    lastRollResults[0] = lastRollResults[1] = lastRollResults[2] = rand.Next(1, 7);
                    lastRollResults[3]= rand.Next(1, 7);
                    lastRollResults[4]= rand.Next(1, 7);
                    break;
                case KniffelScores.FourOfAKind:
                    lastRollResults[0] = lastRollResults[1] = lastRollResults[2] = lastRollResults[3] =rand.Next(1, 7);
                    lastRollResults[4] = rand.Next(1, 7);
                    break;
                case KniffelScores.FullHouse:
                    lastRollResults[0] = lastRollResults[1] = lastRollResults[2] = rand.Next(1, 7);
                    lastRollResults[3] = lastRollResults[4] = rand.Next(1, 7);
                    break;
                case KniffelScores.SmallStraight:
                    firstinrow = rand.Next(1,4);
                    for (int i = 0; i < 4; i++)
                        lastRollResults[i] = firstinrow + i;
                    lastRollResults[4] = rand.Next(1, 7);
                    break;
                case KniffelScores.LargeStraight:
                    firstinrow = rand.Next(1,3);
                    for (int i = 0; i < 5; i++)
                        lastRollResults[i] = firstinrow + i;
                    break;
                case KniffelScores.Kniffel:
                    firstinrow = rand.Next(1, 7);
                    for (int i = 0; i < 5; i++)
                        lastRollResults[i] = firstinrow;
                    break;
                    
            }
        }

        public void ApplyScore(RollResult result)
        {
            LogManager.Log(LogLevel.Message,"Game.ApplyScore","Applying score {0} of {1} for {2}", result.PossibleValue,result.ScoreType,CurrentPlayer.Name);
            
            Send(new ApplyScoreCommand(CurrentPlayer.SeatNo,result));         
            
            
           
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
                p.Roll = 1;
                p.SeatNo--;
                if (p.SeatNo < 0)
                    p.SeatNo = Players.Count - 1;
                p.Init();
            }
            Players = Players.OrderBy(f => f.SeatNo).ToList();
            CurrentPlayer = null;
            StartGame();
        }

        public void JoinGame(Player player)
        {
            lock (syncRoot)
            {
                if (Players == null)
                    Players = new List<Player>();
                var explayer = Players.FirstOrDefault(f => f.SeatNo == player.SeatNo);
                if (explayer == null)
                    Players.Add(player);
                else
                    explayer = player;
                player.Game = this;

                if (PlayerJoined != null)
                    PlayerJoined(this, new PlayerEventArgs(player));
            }
        }

        /// <summary>
        /// returns wheather we have at least one fixed dice of this value
        /// </summary>
        public bool IsDiceFixed(int value)
        {
            return fixedRollResults.Contains(value);
        }

        protected override void InitializeCommandObserver()
        {
            LogManager.Log(LogLevel.Message, "GameClient.InitObserver", "Added handlers at game client");
            m_CommandObserver.CommandReceived += m_CommandObserver_CommandReceived;
            //m_CommandObserver.BetTurnEndedCommandReceived += new EventHandler<CommandEventArgs<BetTurnEndedCommand>>(m_CommandObserver_BetTurnEndedCommandReceived);
            //m_CommandObserver.BetTurnStartedCommandReceived += new EventHandler<CommandEventArgs<BetTurnStartedCommand>>(m_CommandObserver_BetTurnStartedCommandReceived);
            //m_CommandObserver.GameEndedCommandReceived += new EventHandler<CommandEventArgs<GameEndedCommand>>(m_CommandObserver_GameEndedCommandReceived);
            //m_CommandObserver.GameStartedCommandReceived += new EventHandler<CommandEventArgs<GameStartedCommand>>(m_CommandObserver_GameStartedCommandReceived);
            //m_CommandObserver.PlayerHoleCardsChangedCommandReceived += new EventHandler<CommandEventArgs<PlayerHoleCardsChangedCommand>>(m_CommandObserver_PlayerHoleCardsChangedCommandReceived);
            m_CommandObserver.PlayerJoinedCommandReceived += m_CommandObserver_PlayerJoinedCommandReceived;
            //m_CommandObserver.PlayerLeftCommandReceived += new EventHandler<CommandEventArgs<PlayerLeftCommand>>(m_CommandObserver_PlayerLeftCommandReceived);
            //m_CommandObserver.PlayerMoneyChangedCommandReceived += new EventHandler<CommandEventArgs<PlayerMoneyChangedCommand>>(m_CommandObserver_PlayerMoneyChangedCommandReceived);
            //m_CommandObserver.PlayerTurnBeganCommandReceived += new EventHandler<CommandEventArgs<PlayerTurnBeganCommand>>(m_CommandObserver_PlayerTurnBeganCommandReceived);
            //m_CommandObserver.PlayerTurnEndedCommandReceived += new EventHandler<CommandEventArgs<PlayerTurnEndedCommand>>(m_CommandObserver_PlayerTurnEndedCommandReceived);
            //m_CommandObserver.PlayerWonPotCommandReceived += new EventHandler<CommandEventArgs<PlayerWonPotCommand>>(m_CommandObserver_PlayerWonPotCommandReceived);
            //m_CommandObserver.TableClosedCommandReceived += new EventHandler<CommandEventArgs<TableClosedCommand>>(m_CommandObserver_TableClosedCommandReceived);
            m_CommandObserver.TableInfoCommandReceived += m_CommandObserver_TableInfoCommandReceived;
            //m_CommandObserver.PlayerSitOutChangedCommandReceived += new EventHandler<CommandEventArgs<PlayerSitOutChangedCommand>>(m_CommandObserver_PlayerSitOutChangedCommandReceived);
            //m_CommandObserver.PlayerInfoCommandReceived += new EventHandler<CommandEventArgs<PlayerInfoCommand>>(m_CommandObserver_PlayerInfoCommandReceived);
            //m_CommandObserver.ChatMessageCommandReceived += new EventHandler<CommandEventArgs<PlayerChatMessageCommand>>(m_CommandObserver_ChatMessageCommandReceived);
            //m_CommandObserver.TipDealerCommandReceived += new EventHandler<CommandEventArgs<TipDealerCommand>>(m_CommandObserver_TipDealerCommandReceived);
            //m_CommandObserver.PlayerNotificationCommandReceived += new EventHandler<CommandEventArgs<PlayerNotificationCommand>>(m_CommandObserver_PlayerNotificationCommandReceived);
            //m_CommandObserver.PlayerBoughtChipsCommandReceived += m_CommandObserver_PlayerBoughtChipsCommandReceived;
            m_CommandObserver.RollReportCommandReceived += m_CommandObserver_RollReportCommandReceived;
            m_CommandObserver.FixDiceCommandReceived += m_CommandObserver_FixDiceCommandReceived;
            m_CommandObserver.ApplyScoreCommandReceived += m_CommandObserver_ApplyScoreCommandReceived;
            m_CommandObserver.RoundChangedCommandReceived += m_CommandObserver_RoundChangedCommandReceived;
        }

        void m_CommandObserver_RoundChangedCommandReceived(object sender, CommandEventArgs<RoundChangedCommand> e)
        {
            lock (syncRoot)
            {
                fixedRollResults = new List<int>();
                if (CurrentPlayer != null)
                    CurrentPlayer.IsMoving = false;

                CurrentPlayer = Players.Find(f => f.SeatNo == e.Command.PlayerPos);
                CurrentPlayer.IsMoving = true;
                CurrentPlayer.Roll = 1;
                DoMove();
            }
        }

        void m_CommandObserver_ApplyScoreCommandReceived(object sender, CommandEventArgs<ApplyScoreCommand> e)
        {
            lock (syncRoot)
            {
                RollResult result = CurrentPlayer.Results.Find(f => f.ScoreType == e.Command.ScoreType);
                result.PossibleValue = e.Command.PossibleValue;
                result.HasBonus = e.Command.HasBonus;
                //sending result to everyone
                if (ResultApplied != null)
                    ResultApplied(this, new ResultEventArgs(CurrentPlayer, result));
                //check for numeric bonus and apply it
            }
            
        }

        void m_CommandObserver_FixDiceCommandReceived(object sender, CommandEventArgs<FixDiceCommand> e)
        {
            if (DiceFixed != null)
                DiceFixed(this, new FixDiceEventArgs(CurrentPlayer, e.Command.Value, e.Command.IsFixed));
        }

        void m_CommandObserver_RollReportCommandReceived(object sender, CommandEventArgs<RollReportCommand> e)
        {
            lock (syncRoot)
            {
                lastRollResults = e.Command.LastResult.ToArray();

                if (DiceRolled != null)
                    DiceRolled(this, new RollEventArgs(CurrentPlayer, lastRollResults));
            }
        }

        void m_CommandObserver_TableInfoCommandReceived(object sender, CommandEventArgs<Protocol.Commands.Game.TableInfoCommand> e)
        {
            
                Move = e.Command.Round;
                foreach (var seat in e.Command.Seats)
                {
                    //await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { });
                    var player = new Player();

                    player.Game = this;
                    

                    player.Client = seat.ClientType;
                    player.IsMoving = seat.IsPlaying;
                    player.Language = seat.Language;
                    player.PicUrl = seat.PhotoUri;


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

                player.Name = e.Command.PlayerName;
                player.SeatNo = e.Command.PlayerPos;
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
                //m_CommandObserver.BetTurnEndedCommandReceived -= m_CommandObserver_BetTurnEndedCommandReceived;
                //m_CommandObserver.BetTurnStartedCommandReceived -= m_CommandObserver_BetTurnStartedCommandReceived;
                //m_CommandObserver.GameEndedCommandReceived -= m_CommandObserver_GameEndedCommandReceived;
                //m_CommandObserver.GameStartedCommandReceived -= m_CommandObserver_GameStartedCommandReceived;
                //m_CommandObserver.PlayerHoleCardsChangedCommandReceived -= m_CommandObserver_PlayerHoleCardsChangedCommandReceived;
                m_CommandObserver.PlayerJoinedCommandReceived -= m_CommandObserver_PlayerJoinedCommandReceived;
                //m_CommandObserver.PlayerLeftCommandReceived -= m_CommandObserver_PlayerLeftCommandReceived;
                //m_CommandObserver.PlayerMoneyChangedCommandReceived -= m_CommandObserver_PlayerMoneyChangedCommandReceived;
                //m_CommandObserver.PlayerTurnBeganCommandReceived -= m_CommandObserver_PlayerTurnBeganCommandReceived;
                //m_CommandObserver.PlayerTurnEndedCommandReceived -= m_CommandObserver_PlayerTurnEndedCommandReceived;
                //m_CommandObserver.PlayerWonPotCommandReceived -= m_CommandObserver_PlayerWonPotCommandReceived;
                //m_CommandObserver.TableClosedCommandReceived -= m_CommandObserver_TableClosedCommandReceived;
                m_CommandObserver.TableInfoCommandReceived -= m_CommandObserver_TableInfoCommandReceived;
                //m_CommandObserver.PlayerSitOutChangedCommandReceived -= m_CommandObserver_PlayerSitOutChangedCommandReceived;
                //m_CommandObserver.PlayerInfoCommandReceived -= m_CommandObserver_PlayerInfoCommandReceived;
                //m_CommandObserver.ChatMessageCommandReceived -= m_CommandObserver_ChatMessageCommandReceived;
                //m_CommandObserver.TipDealerCommandReceived -= m_CommandObserver_TipDealerCommandReceived;
                //m_CommandObserver.PlayerNotificationCommandReceived -= m_CommandObserver_PlayerNotificationCommandReceived;
                //m_CommandObserver.PlayerBoughtChipsCommandReceived -= m_CommandObserver_PlayerBoughtChipsCommandReceived;
                m_CommandObserver.RollReportCommandReceived -= m_CommandObserver_RollReportCommandReceived;
                m_CommandObserver.FixDiceCommandReceived -= m_CommandObserver_FixDiceCommandReceived;
                m_CommandObserver.ApplyScoreCommandReceived -= m_CommandObserver_ApplyScoreCommandReceived;
                m_CommandObserver.RoundChangedCommandReceived -= m_CommandObserver_RoundChangedCommandReceived;
                Send(new DisconnectCommand());
            }
        }

#endregion
    }
}