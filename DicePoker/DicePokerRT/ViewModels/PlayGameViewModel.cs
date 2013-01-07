
using Sanet.Kniffel.Models;
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
        /// Ruless group label
        /// </summary>
        public string RulesLabel
        {
            get
            {
                return Messages.NEW_GAME_RULES.Localize();
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
                    return Window.Current.Bounds.Width - 130 - (60 * Players.Count);
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
            throw new NotImplementedException();
        }

        void Game_PlayerJoined(object sender, PlayerEventArgs e)
        {
            NotifyPropertyChanged("Players");
            NotifyPropertyChanged("DicePanelRTWidth");
        }

        void Game_MoveChanged(object sender, MoveEventArgs e)
        {
            NotifyPropertyChanged("SelectedPlayer");
            Title = SelectedPlayer.Name + " Move";
        }

        void Game_GameFinished(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void Game_DiceRolled(object sender, RollEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Game_DiceFixed(object sender, FixDiceEventArgs e)
        {
            throw new NotImplementedException();
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
