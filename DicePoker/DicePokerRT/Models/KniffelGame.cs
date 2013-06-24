using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Sanet.Kniffel.DicePanel;
#if SERVER
using System.Timers;

#endif

namespace Sanet.Kniffel.Models
{
    public class KniffelGame : IKniffelGame
    {
        //sync object
        object syncRoot = new object();

        int[] lastRollResults;
        List<int> fixedRollResults = new List<int>();
        Queue<int> thisTurnValues = new Queue<int>();

        /// <summary>
        /// actual dices here :)
        /// </summary>
        Random rand = new Random();

        #if SERVER
        /// <summary>
        /// Timer for game round - every player will have an 
        /// </summary>
        Timer _roundTimer;
#endif

        public KniffelGame()
        {
            //def rule
            Rules = new KniffelRule(Models.Rules.krExtended );
#if SERVER
            _roundTimer = new Timer(100000);
            _roundTimer.Elapsed += _roundTimer_Elapsed;
#endif
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
        /// Notify that game ended
        /// </summary>
        public event EventHandler GameUpdated;

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

        public event EventHandler<PlayerEventArgs> PlayerLeft;

        public event EventHandler<PlayerEventArgs> PlayerReady;

        public event EventHandler<PlayerEventArgs> StyleChanged;

        //Chat Message
        public event EventHandler<ChatMessageEventArgs> OnChatMessage;

        /// <summary>
        /// current player used reset roll
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerRerolled;

        #endregion

        #region Properties
        public string MyName { get; set; }
        public int GameId { get; set; }
        bool _IsPlaying;
        public bool IsPlaying
        {
            get
            {
                if (Players == null)
                    _IsPlaying = false;
                if (Players.Count(f => f.IsReady) == 0)
                    _IsPlaying = false;
                return _IsPlaying;
            }
            set
            {
                _IsPlaying = value;
            }
        }

        /// <summary>
        /// roll of the game-
        /// </summary>
        public int Roll
        {
            get
            {
                int _roll = 1;
                if (Players!=null)
                    foreach (Player p in Players)
                    {
                        if (p.Roll > _roll)
                            _roll = p.Roll;
                    }
                return _roll;
            }
        }

        public string Password { get; set; }

        public DieResult LastDiceResult
        {
            get
            {
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
            fixedRollResults = new List<int>();
            
            if (Rules.Rule == Models.Rules.krMagic)
                RerollMode = false;
            //if we have current player - move is continue, so selecting next
            if (CurrentPlayer != null)
            {
                CurrentPlayer.IsMoving = false;
                //if player left we can't just take next - need to check to the last possible place
                for (int i = CurrentPlayer.SeatNo + 1; i < 5; i++)
                {
                    CurrentPlayer = Players.Where(f => f.IsReady).FirstOrDefault(f => f.SeatNo == i);
                    if (CurrentPlayer != null)
                        break;
                }
            }
            else//else it's new move and we select first player as current
                CurrentPlayer = Players.FirstOrDefault(f => f.SeatNo == 0);
            //if current player null then all palyers are done in this move - move next
            if (CurrentPlayer == null)
            {
                NextMove();
                return;
            }
            CurrentPlayer.IsMoving = true;

#if SERVER
            _roundTimer.Stop();
            _roundTimer.Start();
#endif
            //report to all that player changed
            if (MoveChanged != null)
                MoveChanged(this, new MoveEventArgs(CurrentPlayer, Move));
        }
        /// <summary>
        /// Start next round or end game if last round ended
        /// </summary>
        public void NextMove()
        {
            //if current round is last
                if (Move == Rules.MaxMove)
                {
                    Players=Players.OrderByDescending(f => f.Total).ToList();
                    CurrentPlayer = Players.First();
                    IsPlaying = false;
                    foreach (var p in Players)
                    {
                        SetPlayerReady(p,false);
                    }
                    if (GameFinished != null)
                        GameFinished(this, null);
#if SERVER
                    RestartGame();
#endif
                }
                else
                {
                    ReorderSeats();
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
                if (isfixed)
                {
                    int count = LastDiceResult.NumDiceOf(value);
                    int fixedcount = fixedRollResults.Count(f => f == value);
                    if (count > fixedcount)
                        fixedRollResults.Add(value);
                }
                else
                {
                    if (fixedRollResults.Contains(value))
                        fixedRollResults.Remove(value);
                }
                if (DiceFixed != null)
                    DiceFixed(this, new FixDiceEventArgs(CurrentPlayer, value, isfixed));
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
            LogManager.Log(LogLevel.Message, "Game.ReportRoll",
                "reporting roll results for player {0}. we have {1} fixed dices", CurrentPlayer.Name, fixedRollResults.Count);
            lock (syncRoot)
            {
                int j = 0;
                lastRollResults = new int[5];
                for (int i = j; i < fixedRollResults.Count; i++)
                {
                    lastRollResults[i] = fixedRollResults[i];
                }
                j = fixedRollResults.Count;

                for (int i = j; i <= 4; i++)
                {
                    int ii = DiceRandomGenerator.GetNextDiceResult(fixedRollResults.ToArray()/*lastRollResults*/);//rand.Next(1, 7);//В цикл для нормальной игры, за циклом - только книффеля))

                    lastRollResults[i] = ii;
                    if (Rules.Rule == Models.Rules.krMagic)
                    {
                        if (!RerollMode)
                            thisTurnValues.Enqueue(ii);
                        else if (thisTurnValues.Count>0)
                            lastRollResults[i]=thisTurnValues.Dequeue();
                        
                    }
                }
                //if (Move>6)
                //   lastRollResults = new int[] { 3, 3, 3, 3, 3 };//for debugging


                if (DiceRolled != null)
                    DiceRolled(this, new RollEventArgs(CurrentPlayer, lastRollResults));
            }
        }

        /// <summary>
        /// Pressed 'reroll'
        /// </summary>
        public void ResetRolls()
        {
            CurrentPlayer.Roll = 1;
            RerollMode = true;
            if (PlayerRerolled != null)
                PlayerRerolled(null, new PlayerEventArgs(CurrentPlayer));
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
            //check for kniffel bonus
            if (Rules.HasExtendedBonuses && result.ScoreType != KniffelScores.Kniffel)
            {
                //check if already have kniffel
                var kresult = CurrentPlayer.GetResultForScore( KniffelScores.Kniffel);
                result.HasBonus = (LastDiceResult.KniffelFiveOfAKindScore() == 50 && kresult.Value==kresult.MaxValue);
            }
            //sending result to everyone
            if (ResultApplied != null)
                ResultApplied(this, new ResultEventArgs(CurrentPlayer, result));
            //update players results on server
#if SERVER
            result.Value = result.PossibleValue;
            var cr =CurrentPlayer.Results.FirstOrDefault(f => f.ScoreType == result.ScoreType);
            cr=  result;
            _roundTimer.Stop();
#endif
            //check for numeric bonus and apply it
            if (Rules.HasStandardBonus)
            {
                var bonusResult=CurrentPlayer.Results.FirstOrDefault(f=>f.ScoreType== KniffelScores.Bonus);
                if (result.IsNumeric && !bonusResult.HasValue)
                {
                    if (CurrentPlayer.TotalNumeric > 62)
                    {
                        bonusResult.PossibleValue = 35;
                        ResultApplied(this, new ResultEventArgs(CurrentPlayer, new RollResult()
                        {
                            ScoreType = KniffelScores.Bonus,
                            PossibleValue = bonusResult.PossibleValue
                        }));
#if SERVER
                        bonusResult.Value = bonusResult.PossibleValue;
#endif
                    }
                    else if (CurrentPlayer.TotalNumeric + CurrentPlayer.MaxRemainingNumeric < 64)
                    {
                        bonusResult.PossibleValue = 0;
                        ResultApplied(this, new ResultEventArgs(CurrentPlayer, new RollResult()
                        {
                            ScoreType = KniffelScores.Bonus,
                            PossibleValue = bonusResult.PossibleValue
                        }));
#if SERVER
                        bonusResult.Value = bonusResult.PossibleValue;
#endif
                    }
                    
                }
            }

            DoMove();
           
        }

        void StartGame()
        {
            lock (syncRoot)
            {
                LogManager.Log(LogLevel.Message, "Game.StartGame",
                "Starting game, isplaying: {0}, totalplayers: {1}", IsPlaying,Players.Count);
                {
                    if (IsPlaying)
                        return;
                }
                bool bAllReady = true;
                //Checking if everyone is ready
                foreach (Player player in Players)
                {
                    if (!player.IsReady)
                    {
                        bAllReady = false;
                        break;
                    }
                }
                
                //if yes - starting game
                if (bAllReady)
                {
                    ReorderSeats();
                    CurrentPlayer = null;
                    Move = 1;
                    IsPlaying = true;

                    if (GameUpdated != null)
                        GameUpdated(null, null);

                    DoMove();
                }
            }
        }

        public void ChangeStyle(Player player, DiceStyle style)
        {
            if (player==null)
                return;
            player = Players.FirstOrDefault(f => f.Name == player.Name);
            if (player != null)
            {
                player.SelectedStyle = style;
                if (StyleChanged != null)
                {
                    StyleChanged(null, new PlayerEventArgs(player));
                }
            }
        }

        /// <summary>
        /// Server only method to send chat messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void SendChatMessage(ChatMessage message)
        {
            if (OnChatMessage!=null)
                OnChatMessage(this, new ChatMessageEventArgs(message));
        }


        /// <summary>
        /// On Play Again
        /// </summary>
        public void RestartGame()
        {
            lock (syncRoot)
            {
                IsPlaying = false;
                Move = 1;

                for (int i = 0;i<PlayersNumber;i++)
                {
                    Player p = Players[i];
                    p.Roll = 1;
                    p.SeatNo=i-1;
                    if (p.SeatNo < 0 )
                        p.SeatNo = Players.Count - 1;
                    p.Init();
#if !SERVER
                    p.IsReady = true;
#endif
                }
                Players = Players.OrderBy(f => f.SeatNo).ToList();
                CurrentPlayer = null;
                
                StartGame();
            }
        }

        public void JoinGame(Player player)
        {
            lock (syncRoot)
            {
                if (Players == null)
                    Players = new List<Player>();

                int seat = 0;
                if (Players.Count(f => f.IsReady) == 0)
                {
                    IsPlaying = false;
                    Move = 1;
#if SERVER
                    _roundTimer.Stop();
#endif
                }
                while (Players.FirstOrDefault(f => f.SeatNo == seat) != null)
                {
                    seat++;
                };

                player.SeatNo = seat;
                Players.Add(player);
                player.Game = this;
                if (PlayerJoined != null)
                    PlayerJoined(this, new PlayerEventArgs(player));
            }
            
        }

        public void LeaveGame(Player player)
        {
            player = Players.FirstOrDefault(f => f.Name == player.Name);
            if (player == null)
                return;

            LogManager.Log(LogLevel.Message, "Game.LeaveGame",
                "{0} to leave game, IsPlaying: {1}, total players: {2}",
                player.Name, IsPlaying, Players.Count);

            Players.Remove(player);

            //ReorderSeats();
            
            if (PlayerLeft != null)
                PlayerLeft(null, new PlayerEventArgs(player));
            if (Players.Count(f => f.IsReady) == 0 && IsPlaying)
            {
                LogManager.Log(LogLevel.Message, "Game.LeaveGame",
                "trying to restart, total players: {0}", Players.Count);
                RestartGame();//TODO: incapsulate 2 above lines into this method?
                return;
            }
            else if (CurrentPlayer != null && CurrentPlayer.Name == player.Name && IsPlaying)
            {
                LogManager.Log(LogLevel.Message, "Game.LeaveGame",
                "next player to move");
#if SERVER
                _roundTimer.Stop();
#endif
                DoMove();
                return;
            }
            StartGame();
            
        }

        public void SetPlayerReady(Player player, bool isready)
        {
            //allow to join on virst round
            if (IsPlaying && Move>1)
                isready = false;
            var explayer=Players.FirstOrDefault(f => f.Name== player.Name);
            explayer.IsReady = isready;
            if (PlayerReady != null)
                PlayerReady(null, new PlayerEventArgs(explayer));
            if (isready)
                StartGame();
        }
        public void SetPlayerReady(bool isready)
        {
            
        }

        /// <summary>
        /// in some cases when player leave it may be gap in seats
        /// </summary>
        void ReorderSeats()
        {
            int seat = 0;
            foreach (var player in Players.OrderBy(f => f.SeatNo))
            {
                player.SeatNo = seat;
                seat++;
            }
        }

#if SERVER
        void _roundTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _roundTimer.Stop();
            if (IsPlaying)
            {
                var result = CurrentPlayer.Results.FirstOrDefault(f => !f.HasValue && f.ScoreType != KniffelScores.Bonus);
                result.Value = result.PossibleValue;
                if (ResultApplied != null)
                    ResultApplied(this, new ResultEventArgs(CurrentPlayer, result));
                //update players results on server

                DoMove();
            }
        }
#endif
        /// <summary>
        /// returns wheather we have at least one fixed dice of this value
        /// </summary>
        public bool IsDiceFixed(int value)
        {
            return fixedRollResults.Contains(value);
        }

#endregion
    }
}
