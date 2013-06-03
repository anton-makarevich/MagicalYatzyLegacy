
using DicePokerRT;
using DicePokerRT.KniffelLeaderBoardService;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Protocol;
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Sanet.Kniffel.ViewModels
{
    public class NewOnlineGameViewModel : NewGameViewModelBase
    {
        Popup _facebookPopup = new Popup();
        FacebookPage _facebook = new FacebookPage();

        public event EventHandler PasswordTapped;
        public event EventHandler NameTapped;

        string[] _language = Windows.System.UserProfile.GlobalizationPreferences.Languages[0].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

        DispatcherTimer _updateTimer = new DispatcherTimer() 
        {
            Interval=TimeSpan.FromSeconds(40)
        };
   
        #region Constructor
        public NewOnlineGameViewModel()
            :base()
        {
            _updateTimer.Tick += _updateTimer_Tick;

            _facebookPopup.Child = _facebook;
            _facebook.Tag = _facebookPopup;
            _facebookPopup.Closed += _facebookPopup_Closed;

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
        
        /// <summary>
        /// Tables group label
        /// </summary>
        public string TablesLabel
        {
            get
            {
                return Messages.GAME_TABLES.Localize();
            }
        }
        /// <summary>
        /// Label "Status"
        /// </summary>
        public string StatusLabel
        {
            get
            {
                return Messages.GAME_STATUS.Localize();
            }
        }

        /// <summary>
        /// Just label "Server status"
        /// </summary>
        public string ServerLabel
        {
            get { return Messages.MP_SERVER.Localize(); }
            
        }

        /// <summary>
        /// Just label "Client status"
        /// </summary>
        public string ClientLabel
        {
            get { return Messages.MP_CLIENT.Localize(); }
            
        }

        /// <summary>
        /// Displays if server online
        /// </summary>
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

        /// <summary>
        /// Displays if client version ok
        /// </summary>
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
                    NotifyPropertyChanged("IsReadyToPlay");
                }
            }
        }

        /// <summary>
        /// Display any custom message from server
        /// </summary>
        string _ClientServerStatusMessage;
        public string ClientServerStatusMessage
        {
            get { return _ClientServerStatusMessage; }
            set
            {
                if (_ClientServerStatusMessage != value)
                {
                    _ClientServerStatusMessage = value;
                    NotifyPropertyChanged("ClientServerStatusMessage");
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
                if (SelectedPlayer == null || !SelectedPlayer.HasPassword || SelectedPlayer.IsDefaultName)
                    return false;
                if (string.IsNullOrEmpty(ServerStatusMessage))
                    return false;
                if (ServerStatusMessage.Contains( Messages.MP_SERVER_ONLINE.Localize())
                    && ClientStatusMessage==Messages.MP_CLIENT_UPDATED.Localize())
                        return true;
                    else
                        return false;

                
            }
        }
                    
        private ObservableCollection<TupleTableInfo> _Tables;
        public ObservableCollection<TupleTableInfo> Tables
        {
            get { return _Tables; }
            set
            {
                if (_Tables != value)
                {
                    _Tables = value;
                    NotifyPropertyChanged("Tables");
                }
            }
        }

        
        private TupleTableInfo _SelectedTable;
        public TupleTableInfo SelectedTable
        {
            get { return _SelectedTable; }
            set
            {
                if (_SelectedTable != value)
                {
                    _SelectedTable = value;
                    if (value.Id != -1)
                        SelectedRule = Rules.FirstOrDefault(f => f.Rule.Rule == value.Rule);
                    
                    NotifyPropertyChanged("SelectedTable");
                }
            }
        }

        /// <summary>
        /// Selected rule for game
        /// </summary>
        public override RuleWrapper SelectedRule
        {
            get { return _SelectedRule; }
            set
            {
                if (_SelectedRule != value)
                {
                    if (value != null)
                    {
                        _SelectedRule = value;
                        if (SelectedTable!=null && 
                            SelectedTable.Id != -1 
                            && SelectedTable.Rule != value.Rule.Rule)
                            SelectedTable = Tables[0];
                    }
                    NotifyPropertyChanged("SelectedRule");
                    NotifyPropertyChanged("IsReadyToPlay");
                }
            }
        }
        

        #endregion

        #region Methods

        /// <summary>
        /// fills players list
        /// </summary>
        protected override void FillPlayers()
        {
            Players = new ObservableCollection<PlayerWrapper>();
            var p = RoamingSettings.GetLastPlayer(5);
            if (p.Player == null)
            {
                
                var    userName = GetNewPlayerName(PlayerType.Local);
                p = new PlayerWrapper(new Player())
                {
                    Type = PlayerType.Local,
                    Profile = ProfileType.Facebook,
                    Name=userName
                };
               
            }
            p.RefreshArtifactsInfo();
            p.MagicPressed += p_MagicPressed;
            p.ArtifactsSyncRequest += ArtifactsSyncRequest;
            p.PropertyChanged += p_PropertyChanged;
            p.FacebookClicked += p_FacebookClicked;
            p.IsBotPossible = false;
            p.IsHuman = true;
            p.Player.Client = Config.GetClientType();
            p.Language = _language[0];
            Players.Add(p);
            SelectedPlayer = p;
            
        }

        void p_FacebookClicked(object sender, EventArgs e)
        {
            _facebookPopup.IsOpen = true;
            _facebook.IsOk = false;
            if (SelectedPlayer.IsDefaultName)
                _facebook.LogIn();
            else
                _facebook.LogOut();
            
        }

        void _facebookPopup_Closed(object sender, object e)
        {
            if (SelectedPlayer.IsLocalProfile)
                return;
            if (_facebook.IsOk)
            {
                SelectedPlayer.Name = ViewModelProvider.GetViewModel<FacebookViewModel>().Name;
                SelectedPlayer.Password = Player.FB_PREFIX+ ViewModelProvider.GetViewModel<FacebookViewModel>().FacebookId;
                
            }
            else
            {
                SelectedPlayer.Name = "";
                SelectedPlayer.Password = "";
                
            }
            SelectedPlayer.RefreshArtifactsInfo();
            SavePlayers();
        }

        

        void p_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="Password" ||e.PropertyName=="Name")
                NotifyPropertyChanged("IsReadyToPlay");
            if (e.PropertyName == "IsPasswordReady")
            {
                if (PasswordTapped != null)
                    PasswordTapped(sender, null);
            }
            if (e.PropertyName == "IsNameOpened")
            {
                if (NameTapped != null)
                    NameTapped(sender, null);
            }
        }

        void _updateTimer_Tick(object sender, object e)
        {
            StopUpdating();
            InitOnServer(false);
        }

        public void StopUpdating()
        {
            _updateTimer.Stop();
        }

        protected override string GetNewPlayerName(PlayerType type)
        {
            return Player.GetDeafaultPlayerName(); 

        }


        protected override void NotifyPlayersChanged()
        {
           
            
        }
        
        /// <summary>
        /// Saves settings to roaming
        /// </summary>
        public override void SavePlayers()
        {
            StopUpdating();
            RoamingSettings.SaveLastPlayer(SelectedPlayer.Player, 5);
            if (SelectedRule != null)
                RoamingSettings.LastRule = SelectedRule.Rule.Rule;
        }

        public async override void StartGame()
        {
            
            SavePlayers();
            SelectedPlayer.SelectedStyle = RoamingSettings.DiceStyle;
            var tableId = -1;
            if (SelectedTable != null)
            {
                tableId = SelectedTable.Id;
                if (SelectedTable.Players.Contains(SelectedPlayer.Name))
                {
                    //Utilities.ShowMessage("AlreadyInGameMessage".Localize(), "AppNameLabel".Localize());
                    //return;
                    tableId = -1;
                }
            }
            await JoinManager.JoinTable(tableId, SelectedRule.Rule.Rule);
        }

        /// <summary>
        /// When user enters lobby we ask server about its status and display this info for user
        /// </summary>
        public async void InitOnServer(bool showWait)
        {
            if (SelectedPlayer == null || BusyWithServer)
                return;

            SelectedPlayer.RefreshArtifactsInfo();

            if (!InternetCheker.IsInternetAvailable())
            {
                Utilities.ShowMessage("NoInetMessage".Localize(), Messages.APP_NAME.Localize());
                ServerStatusMessage = Messages.MP_SERVER_OFFLINE.Localize();
                return;
            }
            try
            {
                InitService initService = new InitService();
                BusyWithServer = showWait;
                var respond = await initService.InitPlayer(SelectedPlayer.Player.ID,_language[0]);
                BusyWithServer = false;
                
                if (respond != null)
                {
                    if (respond.IsServerOnline)
                    {
                        ServerStatusMessage = string.Format("{0}", Messages.MP_SERVER_ONLINE.Localize());//, respond.OnlinePlayersCount
                        if (respond.IsClientUpdated)
                        {
                            ClientStatusMessage = Messages.MP_CLIENT_UPDATED.Localize();
                            //if (respond.Message == Messages.MP_SERVER_MAINTANANCE)
                            //    ClientServerStatusMessage = string.Format(Messages.MP_SERVER_MAINTANANCE.Localize(), respond.ServerRestartDate.ToString());
                            //else
                            //    ClientServerStatusMessage = "";
                            ClientServerStatusMessage = respond.Message;
                        }
                        else
                        {
                            ClientStatusMessage = Messages.MP_CLIENT_OUTDATED.Localize();
                            ClientServerStatusMessage = Messages.MP_CLIENT_OUTDATED_STATUS.Localize();
                        }

                    }
                    else
                    {
                        ServerStatusMessage = Messages.MP_SERVER_OFFLINE.Localize();
                        ClientStatusMessage = Messages.MP_CLIENT_UPDATED.Localize();
                        ClientServerStatusMessage = Messages.MP_SERVER_OFFLINE_STATUS.Localize();
                    }
                    foreach (var game in respond.Tables)
                        if (game.Id == -1)
                            game.Name = "RandomLabel".Localize();
                        else
                            game.Name = game.Rule.ToString().Localize();

                    Tables = new ObservableCollection<TupleTableInfo>(respond.Tables);
                    SelectedTable = Tables[0];
                }
                else
                {
                    ServerStatusMessage = Messages.MP_SERVER_OFFLINE.Localize();
                    ClientStatusMessage = Messages.MP_CLIENT_UPDATED.Localize();
                    ClientServerStatusMessage = "";
                }
            }
            catch (Exception ex)
            {
                LogManager.Log("NOGVM.InitOnServer", ex);
                ClientServerStatusMessage = ex.Message;
            }
            _updateTimer.Start();
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
