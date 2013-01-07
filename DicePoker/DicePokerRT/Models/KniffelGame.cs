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
        /// current player applied roll result
        /// </summary>
        public event EventHandler<ResultEventArgs> ResultApplied;

        /// <summary>
        /// current player join game
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerJoined;

        #endregion

        #region Properties
        public int GameId { get; set; }
        public bool IsPlaying { get; set; }
        public string Password { get; set; }

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
            //if we have current player - move is continue, so selecting next
            if (CurrentPlayer != null)
            {
                CurrentPlayer.IsMoving = false;
                CurrentPlayer = Players.FirstOrDefault(f => f.SeatNo == CurrentPlayer.SeatNo + 1);
            }
            else//else it's new move and we select first player as current
                CurrentPlayer = Players.Find(f => f.SeatNo == 0);
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isfixed"></param>
        public void FixDice(int value, bool isfixed)
        {
            if (DiceFixed != null)
                DiceFixed(this, new FixDiceEventArgs(CurrentPlayer, value,isfixed ));
        }
        /// <summary>
        /// Player wants to move. generating value here with network play in mind
        /// </summary>
        /// <param name="player"></param>
        public void ReportRoll()
        {
              
            var value=new int[5];
            for (int i = 0; i <= 4; i++)
            {
                int ii = rand.Next(1, 7);//В цикл для нормальной игры, за циклом - только книффеля))

                value[i] = ii;
            }
            if (DiceRolled != null)
                DiceRolled(this, new RollEventArgs(CurrentPlayer, value));

        }
        public void ApplyScore(RollResult result)
        {
            if (ResultApplied != null)
                ResultApplied(this, new ResultEventArgs(CurrentPlayer, result));
            
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
                //UpdateGameStatus(msg.GameId, true);
                DoMove();
            }
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

#endregion
    }
}
