
using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;
using Windows.UI.Xaml;

namespace Sanet.Kniffel.ViewModels
{
    public class PlayGameViewModel:BaseViewModel
    {
        #region Constructor
        public PlayGameViewModel()
        {
            CreateCommands();
            
        }
        #endregion

        #region Properties
        /// <summary>
        /// Page title
        /// </summary>
        string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        
        /// <summary>
        /// Players group label
        /// </summary>
        public string PlayersLabel
        {
            get
            {
                return Messages.NEW_GAME_PLAYERS.Localize();
            }
        }
        /// <summary>
        /// Rules label
        /// </summary>
        public string RulesLabel
        {
            get
            {
                return Messages.NEW_GAME_RULES.Localize();
            }
        }
        /// <summary>
        /// Rules name label
        /// </summary>
        public string RulesNameLabel
        {
            get
            {
                if (Game != null)
                    return Game.Rules.Rule.ToString().Localize();
                return null;
            }
        }
        /// <summary>
        /// Roll button label
        /// </summary>
        public string RollLabel
        {
            get
            {
                if (SelectedPlayer != null)
                    return string.Format("{0} {1}", Messages.GAME_ROLL.Localize(),SelectedPlayer.Roll);
                return string.Empty;
            }
        }
        /// <summary>
        /// ClearButton label
        /// </summary>
        public string ClearLabel
        {
            get
            {
                return Messages.GAME_CLEAR.Localize();
            }
        }
        
        /// <summary>
        /// Play Again Button label
        /// </summary>
        public string PlayAgainLabel
        {
            get
            {
                return Messages.GAME_PLAY_AGAIN.Localize();
            }
        }

        /// <summary>
        /// Total for result label
        /// </summary>
        public string TotalLabel
        {
            get
            {
                return KniffelScores.Total.ToString().Localize();
            }
        }

        /// <summary>
        /// Players list
        /// </summary>
        public ObservableCollection<Player> Players
        {
            get
            {
                if (Game.Players == null)
                    return null;
                return new ObservableCollection<Player>(Game.Players);
            }
            
        }

        /// <summary>
        /// If dices can be rolled
        /// </summary>
        private bool _CanRoll=true;
        public bool CanRoll
        {
            get { return _CanRoll; }
            
        }

        /// <summary>
        /// If dices can be rolled
        /// </summary>
        public bool CanFix
        {
            get 
            {
                if (!IsPlayerSelected)
                    return  false;
                else if (!SelectedPlayer.IsHuman)
                    return  false;
                if (SelectedPlayer.Roll == 1)
                    return false;
                return true;
            }

        }

        /// <summary>
        /// Selected player -actually current player;
        /// </summary>
        public Player SelectedPlayer
        {
            get
            {
                return Game.CurrentPlayer;
            }
        }
        /// <summary>
        /// Do we have selected player?
        /// </summary>
        public bool IsPlayerSelected
        {
            get
            {
                return SelectedPlayer != null;
            }
        }

        public double DicePanelRTWidth
        {
            get
            {
                if (Window.Current!=null && Players!=null)
                    return Window.Current.Bounds.Width - 230 - (60 * Players.Count);
                return 1238;
            }
        }
        
        private KniffelGame _Game;
        public KniffelGame Game
        {
            get { return _Game; }
            set
            {
                RemoveGameHandlers();
                if (_Game != value)
                {
                    _Game = value;
                    AddGameHandlers();
                    NotifyPropertyChanged("Game");
                }
            }
        }

        /// <summary>
        /// Roll results for user to show
        /// </summary>
        List<RollResult> _RollResults;
        public List<RollResult> RollResults
        {
            get
            {
                return _RollResults;
            }
            set
            {
                _RollResults = value;
                NotifyPropertyChanged("RollResults");
            }
        }

        /// <summary>
        /// Results list to bind to table side caption
        /// </summary>
        public List<RollResult> SampleResults
        {
            get
            {
                if (IsPlayerSelected)
                    return SelectedPlayer.Results;
                return null;
            }
            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Remove game handlers
        /// should run this to dispose game oblject
        /// </summary>
        public void RemoveGameHandlers()
        {
            try
            {
                if (Game != null)
                {
                    Game.DiceFixed -= Game_DiceFixed;
                    Game.DiceRolled -= Game_DiceRolled;
                    Game.GameFinished -= Game_GameFinished;
                    Game.MoveChanged -= Game_MoveChanged;
                    Game.PlayerJoined -= Game_PlayerJoined;
                    Game.ResultApplied -= Game_ResultApplied;
                }
            }
            catch { }
        }
        /// <summary>
        /// Game events handlers
        /// all player actions should be as respond to game events
        /// </summary>
        void AddGameHandlers()
        {
            if (Game != null)
            {
                Game.DiceFixed += Game_DiceFixed;
                Game.DiceRolled += Game_DiceRolled;
                Game.GameFinished += Game_GameFinished;
                Game.MoveChanged += Game_MoveChanged;
                Game.PlayerJoined += Game_PlayerJoined;
                Game.ResultApplied += Game_ResultApplied;
                
            }
        }

        void Game_ResultApplied(object sender, ResultEventArgs e)
        {
            SelectedPlayer.Results.Find(f => f.ScoreType == e.Result.ScoreType).Value = e.Result.PossibleValue;
            SelectedPlayer.UpdateTotal();
            RollResults = null;
        }

        void Game_PlayerJoined(object sender, PlayerEventArgs e)
        {
            NotifyPropertyChanged("Players");
            NotifyPropertyChanged("DicePanelRTWidth");
        }

        void Game_MoveChanged(object sender, MoveEventArgs e)
        {
            SetCanRoll(true);
            NotifyPlayerChanged();
            
        }

        void Game_GameFinished(object sender, EventArgs e)
        {
            //Utilities.ShowToastNotification("GAMOVER vsem");
            SetCanRoll(false);
            NotifyPlayerChanged();
            if (IsPlayerSelected)
            {
                Title = Messages.GAME_FINISHED.Localize();
                NotifyPropertyChanged("Players");
            }
        
        }

        void Game_DiceRolled(object sender, RollEventArgs e)
        {
           
            SelectedPlayer.CheckRollResults();
            RollResults = null;
            SetCanRoll ( false);
             NotifyPlayerChanged();
        }

        void Game_DiceFixed(object sender, FixDiceEventArgs e)
        {
            
        }

        void NotifyPlayerChanged()
        {
            NotifyPropertyChanged("SelectedPlayer");
            
            NotifyPropertyChanged("RollLabel");
            NotifyPropertyChanged("CanFix");
            NotifyPropertyChanged("SampleResults");
            if (IsPlayerSelected)
                Title = string.Format("{2} {0}, {1}",Game.Move ,SelectedPlayer.Name,Messages.GAME_MOVE.Localize() );
        }
        /// <summary>
        /// dices stop
        /// </summary>
        public void OnRollEnd()
        {
            
            bool lastRoll =SelectedPlayer.Roll == 3;

            SetCanRoll(SelectedPlayer.Roll < 3);
            SelectedPlayer.Roll++;

            RollResults = SelectedPlayer.Results.Where(f => !f.HasValue && f.ScoreType != KniffelScores.Bonus).ToList();
            NotifyPlayerChanged();

            //if bot
            if (SelectedPlayer.IsBot)
            {
                if ( lastRoll|| !SelectedPlayer.AINeedRoll())
                    SelectedPlayer.AIDecideFill();
                else
                {
                    SelectedPlayer.AIFixDices();
                    Game.ReportRoll();
                }
            }
            
        }
        /// <summary>
        /// method to check if player can manually roll dices
        /// </summary>
        /// <param name="value"></param>
        void SetCanRoll(bool value)
        {
            if (!IsPlayerSelected)
                _CanRoll = false;
            else if (!SelectedPlayer.IsHuman)
                _CanRoll = false;
            else
                _CanRoll = value;
            NotifyPropertyChanged("CanRoll");
        }

        public void SaveResults()
        {
            foreach (Player p in Players)
            {
                if (p.ShouldSaveResult)
                {
                    var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                    var ename = 
                    ks.PutScoreIntoTableWithPicAsync(Encryptor.Encrypt(p.Name,33),Encryptor.Encrypt(p.Password,33),Encryptor.Encrypt(p.Total.ToString(),33), Encryptor.Encrypt(Game.Rules.ToString(),33),p.PicUrl);
                }
            }
        }

        public void PlayAgain()
        {
            SaveResults();
            Game.RestartGame();
            NotifyPropertyChanged("Players");
        }

        #endregion

        #region Commands
        public RelayCommand DeleteCommand { get; set; }
        
        protected void CreateCommands()
        {
            //DeleteCommand = new RelayCommand(o => DeletePlayer(), () => true);
        }

        
        #endregion


    }
}
