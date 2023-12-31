﻿#if WinRT
using DicePokerRT;
using DicePokerRT.KniffelLeaderBoardService;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using MagicalYatzyOnline;

#endif
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

using Sanet.Common;
#if SILVERLIGHT
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

#endif
#if WINDOWS_PHONE

using DicePokerWP;
#endif
#if VK
using MagicalYatzyVK;
#endif

namespace Sanet.Kniffel.ViewModels
{
    public class NewOnlineGameViewModel : NewGameViewModelBase
    {
        
        public event EventHandler PasswordTapped;
        public event EventHandler NameTapped;
#if WinRT
        string[] _language = Windows.System.UserProfile.GlobalizationPreferences.Languages[0].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
#endif
#if SILVERLIGHT
        string[] _language = new string[] { System.Threading.Thread.CurrentThread.CurrentCulture.Name.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0] };
#endif
        DispatcherTimer _updateTimer = new DispatcherTimer() 
        {
            Interval=TimeSpan.FromSeconds(40)
        };
   
        #region Constructor
        public NewOnlineGameViewModel()
            :base()
        {
            _updateTimer.Tick += _updateTimer_Tick;

            
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
                    NotifyPropertyChanged("IsReadyToPlay");
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
                if (BusyWithServer)
                    return false;
                if (IsNoPlayerInfo)
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

        public bool IsNoPlayerInfo
        {
            get
            {
                return SelectedPlayer == null || !SelectedPlayer.HasPassword || SelectedPlayer.IsDefaultName;
            }
        }

        private ObservableCollection<TableWrapper> _Tables;
        public ObservableCollection<TableWrapper> Tables
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

        
        private TableWrapper _SelectedTable;
        public TableWrapper SelectedTable
        {
            get { return _SelectedTable; }
            set
            {
                if (_SelectedTable != value && value!=null)
                {
                    if (_SelectedTable != null)
                        _SelectedTable.IsSelected = false;
                    _SelectedTable = value;
                    if (_SelectedTable != null)
                    _SelectedTable.IsSelected = true;
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
                        if (_SelectedRule != null)
                             _SelectedRule.IsSelected = false;
                        _SelectedRule = value;
                        _SelectedRule.IsSelected = true;
                       
                        if (SelectedTable!=null && 
                            SelectedTable.Id != -1 
                            && SelectedTable.Rule != value.Rule.Rule)
                            SelectedTable = Tables[0];
                    }
                    NotifyPropertyChanged("SelectedRule");
                    NotifyPropertyChanged("IsNoPlayerInfo");
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
#if VK
            var p = new PlayerWrapper(new Player())
                {

                    Type = PlayerType.Local,
                    Profile = ProfileType.VKontakte,
                    Name = App.VKName,
                    Password = App.VKPass
                };
                    
#else

            var p = RoamingSettings.GetLastPlayer(5);
            if (p == null || p.Player == null)
                p = RoamingSettings.GetLastPlayer(0);

            if (p == null || p.Player == null ||(p.IsDefaultName|| !p.HasPassword))
            {

                var userName = "PlayerNoNameLabel".Localize();//GetNewPlayerName(PlayerType.Local);
                p = new PlayerWrapper(new Player())
                {

                    Type = PlayerType.Local,
                    Profile = ProfileType.Local,
                    Name = userName

                };
                p.Profile = ProfileType.Facebook;
            }
            
#endif
           
            p.MagicPressed += p_MagicPressed;
            p.ArtifactsSyncRequest += ArtifactsSyncRequest;
            p.NameClicked += p_NameClicked;
            p.PassClicked += p_PassClicked;
            p.PropertyChanged += p_PropertyChanged;
#if !VK
            p.FacebookClicked += p_FacebookClicked;
#else
            p.Player.PicUrl = App.VKPic;
#endif
            p.RefreshArtifactsInfo();
            p.IsBotPossible = false;
            p.IsHuman = true;
            p.Player.Client = Config.GetClientType();
            p.Language = _language[0];
            Players.Add(p);
            SelectedPlayer = p;
            
        }
#if !VK
        async void p_FacebookClicked(object sender, EventArgs e)
        {

            bool isLoaded = false;
            if (SelectedPlayer.IsDefaultName)
            {
                try
                {
                    isLoaded = await App.FBInfo.Login();
                    
                }
                catch (Exception ex)
                {
                    LogManager.Log("NOGVM.FacebookLogin", ex);
                    
                }
            }
            else
            {
                App.FBInfo.Logout();
                
            }
            LoadFacebookData(isLoaded);

        }
#endif
        public void LoadFacebookData(bool hasValue)
        {
#if !VK
            if (hasValue)
            {
                SelectedPlayer.Name = App.FBInfo.UserName;
                SelectedPlayer.Password = Player.FB_PREFIX + App.FBInfo.FacebookId;
            }
            else
            {
                SelectedPlayer.Name = "";
                SelectedPlayer.Password = "";
            }
            SelectedPlayer.RefreshArtifactsInfo(false,true);
            SavePlayers();
#endif
        }

       
       
        void p_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Password" || e.PropertyName == "Name")
            {
                NotifyPropertyChanged("IsNoPlayerInfo");
                NotifyPropertyChanged("IsReadyToPlay");
            }
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

        void p_PassClicked(object sender, EventArgs e)
        {
            var p = (PlayerWrapper)sender;
            ChangeUserPass(p);
        }

        void p_NameClicked(object sender, EventArgs e)
        {
            var p = (PlayerWrapper)sender;
            ChangeUserName(p);
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
            BusyWithServer=true;
            await JoinManager.JoinTable(tableId, SelectedRule.Rule.Rule);
            BusyWithServer = false;
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

                    Tables = new ObservableCollection<TableWrapper>();
                    foreach (var t in respond.Tables)
                    {
                        Tables.Add(new TableWrapper(t));
                    }
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
