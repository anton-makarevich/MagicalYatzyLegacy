
using DicePokerRT;
using DicePokerRT.KniffelLeaderBoardService;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;

namespace Sanet.Kniffel.ViewModels
{
    public class NewOnlineGameViewModel : NewGameViewModelBase
    {
        
        #region Constructor
        public NewOnlineGameViewModel()
            :base()
        {
            CreateCommands();
             
        }
        #endregion

        #region Properties
        /// <summary>
        /// Page title
        /// </summary>
        public string Title
        {
            get
            {
                return Messages.NEW_ONLINE_GAME_START.Localize();
            }
        }
        
        /// <summary>
        /// Players group label
        /// </summary>
        public string PlayersLabel
        {
            get
            {
                return Messages.PLAYER_NAME_DEFAULT.Localize();
            }
        }
        
        
        /// <summary>
        /// Selected player, used to delete and maybe other actions
        /// </summary>
        public override Player SelectedPlayer
        {
            get { return _SelectedPlayer; }
            set
            {
                if (_SelectedPlayer != value)
                {
                    _SelectedPlayer = value;
                    NotifyPropertyChanged("SelectedPlayer");
                }
            }
        }
        /// <summary>
        /// Do we have selected player?
        /// </summary>
        public override bool IsPlayerSelected
        {
            get
            {
                return true;
            }
        }
        

        /// <summary>
        /// Conditions to start game
        /// </summary>
        public override bool IsReadyToPlay
        {
            get 
            {
                return true;
                
            }
        }

        #endregion

        #region Methods

        protected override string GetNewPlayerName(PlayerType type)
        {return string.Format("{0} 1",PlayersLabel); }

        protected override void NotifyPlayersChanged()
        {
           
            
        }
        
        /// <summary>
        /// Saves settings to roaming
        /// </summary>
        public override void SavePlayers()
        {
            
        }

        public override void StartGame()
        {
            
        }

        #endregion

        #region Commands
       
        public RelayCommand StartCommand { get; set; }
        
        protected void CreateCommands()
        {
            
            StartCommand = new RelayCommand(o => StartGame(), () => true);
        }



        #endregion


    }
}
