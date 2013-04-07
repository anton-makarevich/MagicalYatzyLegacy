
using DicePokerRT;
using DicePokerRT.KniffelLeaderBoardService;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.WebApi;
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
            FillPlayers();
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

                
        private string _ServerLabel;
        public string ServerLabel
        {
            get { return _ServerLabel; }
            set
            {
                if (_ServerLabel != value)
                {
                    _ServerLabel = value;
                    NotifyPropertyChanged("ServerLabel");
                }
            }
        }


        private string _ClientLabel;
        public string ClientLabel
        {
            get { return _ClientLabel; }
            set
            {
                if (_ClientLabel != value)
                {
                    _ClientLabel = value;
                    NotifyPropertyChanged("ClientLabel");
                }
            }
        }


        private string  _ServerStatusMessage;
        public string  ServerStatusMessage
        {
            get { return _ServerStatusMessage; }
            set
            {
                if (_ServerStatusMessage != value)
                {
                    _ServerStatusMessage = value;
                    NotifyPropertyChanged("ServerStatusMessage");
                    NotifyPropertyChanged("IsReadyToPlay");
                }
            }
        }


        private string _ClientStatusMessage;
        public string ClientStatusMessage
        {
            get { return _ClientStatusMessage; }
            set
            {
                if (_ClientStatusMessage != value)
                {
                    _ClientStatusMessage = value;
                    NotifyPropertyChanged("ClientStatusMessage");
                }
            }
        }


        public string ClientServerStausMessage
        {
            get { return _ClientServerStausMessage; }
            set
            {
                if (_ClientServerStausMessage != value)
                {
                    _ClientServerStausMessage = value;
                    NotifyPropertyChanged("ClientServerStausMessage");
                }
            }
        }


        private bool _BusyWithServer;
        public bool BusyWithServer
        {
            get { return _BusyWithServer; }
            set
            {
                if (_BusyWithServer != value)
                {
                    _BusyWithServer = value;
                    NotifyPropertyChanged("BusyWithServer");
                }
            }
        }


        
        /// <summary>
        /// Selected player, used to delete and maybe other actions
        /// </summary>
        public override PlayerWrapper SelectedPlayer
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
                if (_ServerStatusMessage == Messages.MP_SERVER_ONLINE)
                        return true;
                    else
                        return false;

                
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// fills players list
        /// </summary>
        protected override async void FillPlayers()
        {
            Players = new ObservableCollection<PlayerWrapper>();
            var p = RoamingSettings.GetLastPlayer(0);
            if (p.Player == null)
            {
                //get username from system
                string userName = await UserInformation.GetDisplayNameAsync();
                if (string.IsNullOrEmpty(userName))
                    userName = await UserInformation.GetFirstNameAsync() + await UserInformation.GetFirstNameAsync();
                //if no luck - add default name
                if (string.IsNullOrEmpty(userName))
                    userName = GetNewPlayerName(PlayerType.Local);
                p = new PlayerWrapper(new Player())
                {
                    Name = userName,
                    Type = PlayerType.Local
                };
                p.MagicPressed += p_MagicPressed;
                p.ArtifactsSyncRequest += ArtifactsSyncRequest;
                p.RefreshArtifactsInfo();
                
            }
            p.IsBotPossible = false;
            p.IsHuman = true;
            p.Player.Client = Config.GetClientType();
            var language=Windows.System.UserProfile.GlobalizationPreferences.Languages[0].Split(new string[]{"-"}, StringSplitOptions.RemoveEmptyEntries);
            p.Language = language[0];
            Players.Add(p);
            SelectedPlayer = p;
            InitOnServer();
        }
               

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
            RoamingSettings.SaveLastPlayer(SelectedPlayer.Player, 0);
        }

        public async override void StartGame()
        {
            SavePlayers();

            await JoinManager.JoinTable(-1, SelectedRule.Rule.Rule);
        }

        async void InitOnServer()
        {
            if (!InternetCheker.IsInternetAvailable())
            {
                Utilities.ShowMessage("NoInetMessage".Localize(), Messages.APP_NAME.Localize());
                ServerStatusMessage = Messages.MP_SERVER_OFFLINE.Localize();
                return;
            }
            InitService initService = new InitService();
            var respond = await initService.InitPlayer(SelectedPlayer.Player.ID);
            if (respond != null)
            {
                ServerStatusMessage = (respond.IsServerOnline) ? Messages.MP_SERVER_ONLINE.Localize() 
                    : Messages.MP_SERVER_OFFLINE.Localize();
            }
            else;
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
