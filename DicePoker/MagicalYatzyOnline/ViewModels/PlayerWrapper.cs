using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Models;
using System;
using System.Collections.Generic;
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
                _Player.IsHuman = true;
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
        
        

        public int MagicRollsCount
        {
            get
            {
                return RoamingSettings.GetMagicRollsCount(this.Player);
            }
        }
        public int ManualSetsCount
        {
            get
            {
                return RoamingSettings.GetManualSetsCount(this.Player);
            }
        }
        public int RollResetsCount
        {
            get
            {
                return RoamingSettings.GetForthRollsCount(this.Player);
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
                NotifyPropertyChanged("Name");
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
                        ArtifactsInfoMessage = "ChangeNameLabel".Localize();
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

        public List<RollResult> Results 
        {
            get
            {
                return _Player.Results;
            }
            set
            {
                _Player.Results = value;
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
                if (!IsHuman)
                    return false;
                return _ShouldSaveResult;
            }
            set
            {
                _ShouldSaveResult = value;
                NotifyPropertyChanged("ShouldSaveResult");
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


        #endregion


        #region Commands
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand MagicCommand { get; set; }

        protected void CreateCommands()
        {
            DeleteCommand = new RelayCommand(o => Delete(), () => true);
            MagicCommand = new RelayCommand(o => OnMagicPressed(), () => true);
        }



        #endregion
    }
}
