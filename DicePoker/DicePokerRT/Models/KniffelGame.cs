﻿using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanet.Kniffel.Models
{
    public class KniffelGame
    {
        //sync object
        object syncRoot = new object();

        int[] lastRollResults = new int[5];
        List<int> fixedRollResults = new List<int>();

        /// <summary>
        /// actual dices here :)
        /// </summary>
        Random rand = new Random();




        public KniffelGame()
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


        /// <summary>
        /// Players Count
        /// </summary>
        public int PlayersNumber
        {
            get
            {
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

        
#endregion

#region Methods
        /// <summary>
        /// Change next player to move
        /// </summary>
        public void DoMove()
        {
            fixedRollResults = new List<int>();
            //if we have current player - move is continue, so selecting next
            if (CurrentPlayer != null)
            {
                CurrentPlayer.IsMoving = false;
                CurrentPlayer = Players.FirstOrDefault(f => f.SeatNo == CurrentPlayer.SeatNo + 1);
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
            lock (syncRoot)
            {
                int j = 0;
                for (int i = j; i < fixedRollResults.Count; i++)
                {
                    lastRollResults[i] = fixedRollResults[i];
                }
                j = fixedRollResults.Count;

                for (int i = j; i <= 4; i++)
                {
                    int ii = rand.Next(1, 7);//В цикл для нормальной игры, за циклом - только книффеля))

                    lastRollResults[i] = ii;
                }
                //if (Move>6)
                //   lastRollResults = new int[] { 3, 3, 3, 3, 3 };//for debugging


                if (DiceRolled != null)
                    DiceRolled(this, new RollEventArgs(CurrentPlayer, lastRollResults));
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
            
            //check for kniffel bonus
            if (Rules.HasExtendedBonuses && result.ScoreType != KniffelScores.Kniffel)
            {
                //check if already have kniffel
                var kresult = CurrentPlayer.GetResultForScore( KniffelScores.Kniffel);
                result.HasBonus = (LastDiceResult.KniffelFiveOfAKindScore() == 50&&kresult.Value==kresult.MaxValue);
            }
            //sending result to everyone
            if (ResultApplied != null)
                ResultApplied(this, new ResultEventArgs(CurrentPlayer, result));
            //check for numeric bonus and apply it
            if (Rules.HasStandardBonus)
            {
                if (result.IsNumeric && !CurrentPlayer.Results.FirstOrDefault(f=>f.ScoreType== KniffelScores.Bonus).HasValue)
                {
                    if (CurrentPlayer.TotalNumeric>62)
                        ResultApplied(this, new ResultEventArgs(CurrentPlayer, new RollResult() 
                        { 
                            ScoreType= KniffelScores.Bonus,
                            PossibleValue=35
                        }));
                    else if (CurrentPlayer.TotalNumeric+CurrentPlayer.MaxRemainingNumeric <64)
                        ResultApplied(this, new ResultEventArgs(CurrentPlayer, new RollResult()
                        {
                            ScoreType = KniffelScores.Bonus,
                            PossibleValue = 0
                        }));
                }
            }
            DoMove();
           
        }

        public void StartGame()
        {
            
            {
                if (IsPlaying)
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
                DoMove();
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
            if (Players==null)
                Players = new List<Player>();
            player.SeatNo=Players.Count;
            Players.Add(player);
            player.Game = this;
            if (PlayerJoined != null)
                PlayerJoined(this, new PlayerEventArgs(player));
        }

        /// <summary>
        /// returns wheather we have at least one fixed dice of this value
        /// </summary>
        public bool IsDiceFiexed(int value)
        {
            return fixedRollResults.Contains(value);
        }

#endregion
    }
}
