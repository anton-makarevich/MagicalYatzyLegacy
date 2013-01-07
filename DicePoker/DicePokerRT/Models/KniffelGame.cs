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
        public Rules Rules { get; set; }
        /// <summary>
        /// Move number
        /// </summary>
        public int Move { get; set; }
        /// <summary>
        /// Player which gas a move
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// Maximum moves based on rules
        /// </summary>
        public int MaxMove
        {
            get
            {
                switch (Rules)
                {
                    case Models.Rules.krBaby:
                        return 7;
                    
                       
                }
                 return 13;
            }
        }
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
        private void NextMove()
        {
            
                if (Move == MaxMove)
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
        private void FixDice(int value, bool isfixed)
        {
            if (DiceFixed != null)
                DiceFixed(this, new FixDiceEventArgs(CurrentPlayer, value,isfixed ));
        }
        /// <summary>
        /// Player wants to move. generating value here with network play in mind
        /// </summary>
        /// <param name="player"></param>
        private void ReportRoll()
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
        private void ApplyScore(RollResult result)
        {
            if (ResultApplied != null)
                ResultApplied(this, new ResultEventArgs(CurrentPlayer, result));
            
            DoMove();
           
        }

        private void StartGame()
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

#endregion
    }
}
