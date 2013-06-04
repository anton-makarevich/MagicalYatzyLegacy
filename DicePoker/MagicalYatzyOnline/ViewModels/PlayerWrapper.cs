using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
namespace Sanet.Kniffel.ViewModels
{
    public class PlayerWrapper:BaseViewModel,IPlayer
    {

        Player _Player;
        public Player Player
        {
            get
            {
                return _Player;
            }
        }

        public event EventHandler MagicPressed;
        public event EventHandler ArtifactsSyncRequest;
        public event EventHandler DeletePressed;
        public event EventHandler FacebookClicked;
        

        public PlayerWrapper(Player player)
        {
            _Player = player;
            CreateCommands();
        }

        public bool AllNumericFilled
        {
            get
            {
                return _Player.AllNumericFilled;
            }
        }

        private string _ArtifactsInfoMessage;
        public string ArtifactsInfoMessage
        {
            get { return _ArtifactsInfoMessage; }
            set
            {
                if (_ArtifactsInfoMessage != value)
                {
                    _ArtifactsInfoMessage = value;
                    NotifyPropertyChanged("ArtifactsInfoMessage");
                }
            }
        }

        /// <summary>
        /// Label for "artifacts"
        /// </summary>
        public string ArtifactsLabelLocalized
        {
            get
            {
                return "ArtifactsLabel".Localize();
            }
        }

        
        public bool CanBuy
        {
            get
            {
                return _Player.CanBuy;
            }
        }
        
        public ClientType Client 
        {
            get
            {
                return _Player.Client;
            }
           
        }



        public string DeleteLabel
        {
            get
            {
                return "DeletePlayerLabel".Localize();
            }
        }

        
        public IKniffelGame Game
        { 
            get
            {
                return _Player.Game;
            }
            set 
            {
                _Player.Game = value;
                NotifyPropertyChanged("Game");
            }
        }
        
        private bool _IsBotPossible=true;
        public bool IsBotPossible
        {
            get { return _IsBotPossible; }
            set
            {
                if (_IsBotPossible != value)
                {
                    _IsBotPossible = value;
                    NotifyPropertyChanged("IsBotPossible");
                }
            }
        }

        
        private bool _HadStartupMagic;
        public bool HadStartupMagic
        {
            get { return _HadStartupMagic; }
            set
            {
                if (_HadStartupMagic != value)
                {
                    _HadStartupMagic = value;
                    NotifyPropertyChanged("HadStartupMagic");
                }
            }
        }


        public bool HasArtifacts
        {
            get
            {
                return MagicRollsCount != 0 && ManualSetsCount != 0 && RollResetsCount != 0;
            }
        }

        public bool HasPassword { get { return _Player.HasPassword; } }
        
        public void Init()
        {
            IsMagicRollAvailable = true;
            IsManualSetlAvailable = true;
            IsForthRollAvailable = true;
            Player.Init();
        }

        public bool IsBot 
        {
            get
            {
                return _Player.IsBot;
            }
            set
            {
                _Player.IsBot = value;
            }
        }

        public bool IsDefaultName { get { return _Player.IsDefaultName; } }
        /// <summary>
        /// Check if it can be deleted - game must have at least one player
        /// </summary>
        bool _IsDeleteable;
        public bool IsDeleteable
        {
            get
            {

                return _IsDeleteable;
            }
            set
            {
                _IsDeleteable = value;
                NotifyPropertyChanged("IsDeleteable");
            }
        }

        //Magic artifacts related props
        /// <summary>
        /// Property to check if magic roll currently available 
        /// </summary>
        
        public bool IsMagicRollAvailable
        {
            get
            {
                //if no game - no sense
                if (Game == null || Player == null)
                    return false;
                //if Rules are different from magic
                if (Game.Rules.Rule != Rules.krMagic)
                    return false;
                //if no rolls in store
                if (RoamingSettings.GetMagicRollsCount(this.Player) < 1)
                    return false;
                return _Player.IsMagicRollAvailable;
            }
            set
            {
                if (_Player.IsMagicRollAvailable != value)
                {
                    _Player.IsMagicRollAvailable = value;
                    NotifyPropertyChanged("IsMagicRollAvailable");
                }
            }
        }

        
        public bool IsManualSetlAvailable
        {
            get
            {
                //if no game - no sense
                if (Game == null || Player == null)
                    return false;
                //if Rules are different from magic
                if (Game.Rules.Rule != Rules.krMagic)
                    return false;
                //if no rolls in store
                if (RoamingSettings.GetManualSetsCount(this.Player) < 1)
                    return false;
                return _Player.IsManualSetlAvailable;
            }
            set
            {
                if (_Player.IsManualSetlAvailable != value)
                {
                    _Player.IsManualSetlAvailable = value;
                    NotifyPropertyChanged("IsManualSetlAvailable");
                }
            }
        }
        
        public bool IsForthRollAvailable
        {
            get
            {
                //if no game - no sense
                if (Game == null||Player==null)
                    return false;
                //if Rules are different from magic
                if (Game.Rules.Rule != Rules.krMagic)
                    return false;
                //if no rolls in store
                if (RoamingSettings.GetForthRollsCount(this.Player) < 1)
                    return false;
                return Player.IsForthRolllAvailable;
            }
            set
            {
                if (Player.IsForthRolllAvailable != value)
                {
                    Player.IsForthRolllAvailable = value;
                    NotifyPropertyChanged("IsForthRollAvailable");
                }
            }
        }
        
        public bool IsHuman
        {
            get
            {
                return _Player.IsHuman;
            }
            set 
            {
                if (value)
                    Type = PlayerType.Local;
                else
                    Type = PlayerType.AI;
                NotifyPropertyChanged("IsHuman");
            }
        }
        
        public bool IsMoving 
        {
            get
            {
                return _Player.IsMoving;
            }
            set
            {
                _Player.IsMoving = value;
#if ONLINE
                if (_Player.Game!=null && _Player.Game is KniffelGameClient)
                    IsMyTurn = IsMoving;
#endif
                NotifyPropertyChanged("IsMoving");
            }
        }

        /// <summary>
        /// this property binded to rotating panel with passwors - updated only on rotate
        /// </summary>
        private bool _IsPasswordReady;
        public bool IsPasswordReady
        {
            get { return _IsPasswordReady; }
            set
            {
                if (_IsPasswordReady != value)
                {
                    _IsPasswordReady = value;
                    NotifyPropertyChanged("IsPasswordReady");
                    if (value && !string.IsNullOrEmpty(Password))
                        RefreshArtifactsInfo(false, true);
                }
            }
        }

        /// <summary>
        /// Fake property to check if rotating panel is opened
        /// </summary>
        private bool _IsNameOpened;
        public bool IsNameOpened
        {
            get { return _IsNameOpened; }
            set
            {
                if (_IsNameOpened != value)
                {
                    _IsNameOpened = value;
                    NotifyPropertyChanged("IsNameOpened");
                }
            }
        }



        public bool IsReady
        {
            get
            {
                return _Player.IsReady;
            }
            set
            {
                _Player.IsReady = value;
                NotifyPropertyChanged("IsReady");
            }
        }

        
        public string Language 
        {
            get
            {
                return Player.Language;
            }
            set
            {
                _Player.Language = value;
            }
        }

        public DiceStyle SelectedStyle
        {
            get
            { return Player.SelectedStyle; }
            set
            {
                Player.SelectedStyle = value;
            }
        }

        public int MagicRollsCount
        {
            get
            {
                if (this.Player == null || string.IsNullOrEmpty(this.Player.Password))
                    return 0;
                var res=RoamingSettings.GetMagicRollsCount(this.Player);
                if (res<0) res=0;
                return res;
            }
        }
        public int ManualSetsCount
        {
            get
            {
                if (this.Player == null || string.IsNullOrEmpty(this.Player.Password))
                    return 0;
                var res = RoamingSettings.GetManualSetsCount(this.Player);
                if (res < 0) res = 0;
                return res;
            }
        }
        public int RollResetsCount
        {
            get
            {
                if (this.Player == null || string.IsNullOrEmpty(this.Player.Password))
                    return 0;
                var res = RoamingSettings.GetForthRollsCount(this.Player);
                if (res < 0) res = 0;
                return res;
            }
        }

        public int MaxRemainingNumeric
        {
            get
            { return _Player.MaxRemainingNumeric; }
        }

        public string Name
        {
            get
            {
                return _Player.Name;
            }
            set
            {
                _Player.Name = value;
                Password = "";
                _Player.PicUrl = "";
                _ProfilePhoto = null;
                NotifyPropertyChanged("Name");
                NotifyPropertyChanged("IsDefaultName");
                NotifyPropertyChanged("FacebookName");
                NotifyPropertyChanged("ProfilePhoto");
                NotifyPropertyChanged("FacebookLoginLabel"); 
                ArtifactsInfoMessage = "ChangePasswordLabel".Localize();
                //RememberPass = false;
                HadStartupMagic = false;
                if (!string.IsNullOrEmpty(value))
                    RefreshArtifactsInfo();
            }
        }

        public void OnMagicRollUsed()
        {
            IsMagicRollAvailable = false;
        }
        public void OnManaulSetUsed()
        {
            IsManualSetlAvailable = false;
        }
        public void OnForthRollUsed()
        {
            IsForthRollAvailable = false;
        }

        public string Password 
        {
            get
            {
                return _Player.Password;
            }
            set 
            {
                _Player.Password = value;
                NotifyPropertyChanged("Password");
                NotifyPropertyChanged("HasPassword");
                NotifyPropertyChanged("PlayerPasswordLabelLocalized");
                NotifyPropertyChanged("ProfilePhoto");
            }
        }

        public string FacebookLoginLabel
        {
            get
            {
                return (IsDefaultName) ?
                    "LoginLabel".Localize() :
                    "LogoutLabel".Localize();

            }
        }

        public string FacebookName
        {
            get
            {
                return
                    (IsDefaultName) ? "" :
                    Name;
            }
        }

        /// <summary>
        /// Label for 'Human'
        /// </summary>
        public string PlayerHumanLabelLocalized
        {
            get
            {
                return Messages.PLAYER_HUMAN.Localize();
            }
        }

        /// <summary>
        /// Label for 'Bot'
        /// </summary>
        public string PlayerBotLabelLocalized
        {
            get
            {
                return Messages.PLAYER_BOT.Localize();
            }
        }
        /// <summary>
        /// Label for 'Remember password'
        /// </summary>
        public string PlayerRememberLabelLocalized
        {
            get
            {
                return Messages.PLAYER_PASSWORD_REMEMBER.Localize();
            }
        }

        /// <summary>
        /// Label for 'save results to leaderboard'
        /// </summary>
        public string PlayerSaveScoreLabelLocalized
        {
            get
            {
                return Messages.PLAYER_SAVE_SCORE.Localize();
            }
        }
        /// <summary>
        /// Label for user name
        /// </summary>
        public string PlayerNameLabelLocalized
        {
            get
            {
                return Messages.PLAYER_NAME.Localize();
            }
        }



        /// <summary>
        /// Label for user password
        /// </summary>
        public string PlayerPasswordLabelLocalized
        {
            get
            {
                if (HasPassword)
                    return Messages.PLAYER_PASSWORD.Localize();
                return Messages.PLAYER_NO_PASSWORD.Localize();
            }
        }
        /// <summary>
        /// Label for user type
        /// </summary>
        public string PlayerTypeLabelLocalized
        {
            get
            {
                return Messages.PLAYER_TYPE.Localize();
            }
        }
        
        public string ChangeNameLabel
        {
            get
            {
                return (IsLocalProfile) ? "ChangeNameLabel".Localize() :
                    "LoginToFBLabel".Localize();
            }
        }

        public void RefreshArtifactsInfo(bool aftersync = false, bool forcesync = false)
        {
            NotifyPropertyChanged("MagicRollsCount");
            NotifyPropertyChanged("ManualSetsCount");
            NotifyPropertyChanged("RollResetsCount");
            NotifyPropertyChanged("HasArtifacts");
            if (!HasArtifacts || forcesync)
            {
                if (HadStartupMagic)
                    ArtifactsInfoMessage = Messages.PLAYER_ARTIFACTS_WINBUY.Localize();
                else
                {
                    if (aftersync)
                        ArtifactsInfoMessage = "WrongNamePassLabel".Localize();

                    if (IsDefaultName)
                    {
                        ArtifactsInfoMessage = ChangeNameLabel;
                        return;
                    }
                    if (string.IsNullOrEmpty(Password))
                    {
                        ArtifactsInfoMessage = "ChangePasswordLabel".Localize();
                        return;
                    }
                    if (!aftersync)
                    {
                        if (ArtifactsSyncRequest != null)
                        {
                            ArtifactsSyncRequest(this, null);
                            ArtifactsInfoMessage = "CheckingLabel".Localize();
                        }
                        else
                            ArtifactsInfoMessage = "NoInternetLabel".Localize();
                    }
                }

            }


        }

        bool RememberPass
        {
            get
            { return true; }
            set
            { _Player.RememberPass = true; }
        }

        List<RollResultWrapper> _Results;
        public List<RollResultWrapper> Results 
        {
            get
            {
                if (_Player.Results==null)
                    return null;
                if (_Results == null)
                {
                    _Results = new List<RollResultWrapper>();
                    foreach (var r in _Player.Results)
                        _Results.Add(new RollResultWrapper(r));
                }
                return _Results;
            }
            set
            {
                if (value==null && _Results!=null)
                    foreach (var r in _Results)
                        ((RollResultWrapper)r).Dispose();
                _Results = value;
                NotifyPropertyChanged("Results");
            }
        }
        public int Roll
        {
            get
            {
                return _Player.Roll;
            }
            set
            {
                _Player.Roll = value;
                NotifyPropertyChanged("Roll");
            }
        }
                
        public int SeatNo
        {
            get
            { return _Player.SeatNo; }
            
        }

        bool _ShouldSaveResult = true;
        public bool ShouldSaveResult
        {
            get
            {
                if (!IsHuman || Total==0)
                    return false;
                return _ShouldSaveResult;
            }
            set
            {
                _ShouldSaveResult = value;
                NotifyPropertyChanged("ShouldSaveResult");
            }
        }

        public bool CanSaveResult
        {
            get
            {
                return (IsHuman && Total != 0);
                   
            }
            
        }

        /// <summary>
        /// Labels - tile helpers for new user ui
        /// </summary>
        public string TapToChangeLabel
        {
            get
            {
                return "TapToChangeLabel".Localize();
            }
        }
        public string TapToApplyLabel
        {
            get
            {
                return "TapToApplyLabel".Localize();
            }
        }




        public string TapToChangeLabelLocalized
        {
            get
            {
                return Messages.PLAYER_SAVE_SCORE.Localize();
            }
        }
                
        public ProfileType Profile
        {
            get { return _Player.Profile; }
            set
            {
                if (_Player.Profile != value)
                {
                    _Player.Profile = value;
                    NotifyPropertyChanged("Profile");
                    NotifyPropertyChanged("IsLocalProfile");
                    NotifyPropertyChanged("IsFacebookProfile");
                    NotifyPropertyChanged("HasPassword");
                    NotifyPropertyChanged("IsDefaultName");
                    NotifyPropertyChanged("Name");
                    NotifyPropertyChanged("Password");
                    NotifyPropertyChanged("PlayerPasswordLabelLocalized");
                    NotifyPropertyChanged("FacebookName");
                    NotifyPropertyChanged("FacebookLoginLabel");
                    //NotifyPropertyChanged("HasArtifacts");
                    RefreshArtifactsInfo();
                }
            }
        }

        public bool IsLocalProfile
        {
            get
            {
                return Profile == ProfileType.Local;
            }
            set
            {
                if (value)
                    Profile = ProfileType.Local;
                else
                    Profile = ProfileType.Facebook;
            }
        }
        public bool IsFacebookProfile
        {
            get
            {
                return Profile == ProfileType.Facebook;
            }
            set
            {
                if (value)
                    Profile = ProfileType.Facebook;
                else
                    Profile = ProfileType.Local;
            }
        }

        public string LocalProfileLabel
        {
            get
            {
                return "LocalLabel".Localize();
            }
        }

        public string ProfileLabel
        {
            get
            {
                return "ProfileLabel".Localize();
            }
        }

        public int Total { get { return _Player.Total; } }
        public int TotalNumeric { get {return _Player.TotalNumeric; } }
        
        public PlayerType Type
        {
            get
            {
                return _Player.Type;
            }
            set
            {
                _Player.Type = value;
                NotifyPropertyChanged("Type");
                NotifyPropertyChanged("IsBot");
                NotifyPropertyChanged("IsHuman");
            }
        }

        ImageSource _ProfilePhoto;
        public ImageSource ProfilePhoto
        {
            get
            {
                if (Player.PicUrl!="na" )
                    _ProfilePhoto = new BitmapImage(new Uri(Player.PicUrl));
                
                return _ProfilePhoto;
            }

        }

        /// <summary>
        /// notifying that total changed
        /// </summary>
        public void UpdateTotal()
        {
            NotifyPropertyChanged("Total");
        }

        public void UpdateType()
        {
            NotifyPropertyChanged("IsHuman");
        }

        public void Refresh()
        {
            NotifyPropertyChanged("IsMoving");
            NotifyPropertyChanged("Results");
            foreach (var r in Results)
                r.Refresh();
        }

        #region Methods

        private void Delete()
        {
            if (DeletePressed != null)
                DeletePressed(this, null);
        }


        private void OnMagicPressed()
        {
            if (MagicPressed != null)
                MagicPressed(this, null);
        }


        public override void Dispose()
        {
            if (_timer != null)
            {
                StopTimer();
                _timer.Tick -= _timer_Tick;
                _timer = null;
            }
            base.Dispose();
        }

        #endregion

        #region timer
        //Used to show timer progress until round ends in online game 
        int _Counter ;
        public int Counter
        {
            get
            {
                return _Counter;
            }
            set
            {
                _Counter = value;
                //Status = "Thinking..."; // +value;
                NotifyPropertyChanged("Counter");
            }
        }
        DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        void _timer_Tick(object sender, object e)
        {
            if (Counter == 5 && Player.Name == Game.CurrentPlayer.Name)
                SoundsProvider.PlaySound(_player, "timeoutwarning");
            if (Counter == 0)//time off
            {
                StopTimer();
            }
            else
            {
                Counter--;
            }

        }


        public Duration Duration
        {
            get
            {
                return new Duration(TimeSpan.FromSeconds(Timeout));
            }

        }


        private int _Timeout=100;
        public int Timeout
        {
            get
            { 
                return _Timeout; 
            }
            set
            {
                if (_Timeout != value)
                {
                    _Timeout = value;
                    NotifyPropertyChanged("Timeout");
                    NotifyPropertyChanged("Duration");
                }
            }
        }


        private bool _IsMyTurn=false;
        public bool IsMyTurn
        {
            get
            { 
                return _IsMyTurn;
            }
            set
            {
                if (_IsMyTurn!=value)

                    _IsMyTurn = value;
                    
                    if (value)
                    {
                        //if (Player.SeatNo == Game.CurrentPlayer.SeatNo)
                        //    SoundsProvider.PlaySound(_player, "myturn");
                        StartTimer();
                    }
                    else
                        StopTimer();
                NotifyPropertyChanged("IsMyTurn");
            }
        }


        private void StartTimer()
        {
            //starting timer
            Counter = Timeout;
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }
        public void StopTimer()
        {
            if (_timer!=null && _timer.IsEnabled)
            {
                _timer.Tick -= _timer_Tick;
                _timer.Stop();
            }
        }

        private void OnFBTapped()
        {
            if (FacebookClicked != null)
                FacebookClicked(this,null);
        }

        #endregion


        #region Commands
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand MagicCommand { get; set; }
        public RelayCommand FBCommand { get; set; }

        protected void CreateCommands()
        {
            DeleteCommand = new RelayCommand(o => Delete(), () => true);
            MagicCommand = new RelayCommand(o => OnMagicPressed(), () => true);
            FBCommand = new RelayCommand(o => OnFBTapped(), () => true);
        }



        #endregion
    }
}
