using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Interfaces;

namespace Sanet.Kniffel.Models
{
    public partial class Player:BaseViewModel
    {
        public Player()
        {
            CreateCommands();
        }

        #region Events
        /// <summary>
        /// call this on delete 
        /// </summary>
        public event EventHandler DeletePressed;
        /// <summary>
        /// Call this to ask server about artifacts...
        /// </summary>
        public event EventHandler ArtifactsSyncRequest;
        /// <summary>
        /// call this to open 'magic room popup' for this player 
        /// </summary>
        public event EventHandler MagicPressed;
        #endregion

        #region Prperties
        
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

        
        bool _ShouldSaveResult=true;
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
        /// If to remember pass (works only for human )
        /// </summary>
        private bool _RememberPass;
        public bool RememberPass
        {
            get 
            {
                if (IsHuman)
                    return true;//_RememberPass;
                return false;
            }
            set
            {
                if (_RememberPass != value && IsHuman)
                {
                    _RememberPass = value;
                }
                else
                    _RememberPass = false;
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
        /// Label for "artifacts"
        /// </summary>
        public string ArtifactsLabelLocalized
        {
            get
            {
                return "ArtifactsLabel".Localize();
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

        public string DeleteLabel
        {
            get
            {
                return "DeletePlayerLabel".Localize();
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

        public string TapToChangeLabelLocalized
        {
            get
            {
                return Messages.PLAYER_SAVE_SCORE.Localize();
            }
        } 
        
        
        //Magic artifacts related props
        /// <summary>
        /// Property to check if magic roll currently available 
        /// </summary>
        private bool _IsMagicRollAvailable;
        public bool IsMagicRollAvailable
        {
            get
            {
                //if no game - no sense
                if (Game == null)
                    return false;
                //if Rules are different from magic
                if (Game.Rules.Rule != Rules.krMagic)
                    return false;
                //if no rolls in store
                if (RoamingSettings.GetMagicRollsCount(this) == 0)
                    return false;
                return _IsMagicRollAvailable;
            }
            set
            {
                if (_IsMagicRollAvailable != value)
                {
                    _IsMagicRollAvailable = value;
                    NotifyPropertyChanged("IsMagicRollAvailable");
                }
            }
        }
        private bool _IsManualSetlAvailable;
        public bool IsManualSetlAvailable
        {
            get
            {
                //if no game - no sense
                if (Game == null)
                    return false;
                //if Rules are different from magic
                if (Game.Rules.Rule != Rules.krMagic)
                    return false;
                //if no rolls in store
                if (RoamingSettings.GetManualSetsCount(this) == 0)
                    return false;
                return _IsManualSetlAvailable;
            }
            set
            {
                if (_IsManualSetlAvailable != value)
                {
                    _IsManualSetlAvailable = value;
                    NotifyPropertyChanged("IsManualSetlAvailable");
                }
            }
        }
        private bool _IsForthRolllAvailable;
        public bool IsForthRollAvailable
        {
            get
            {
                //if no game - no sense
                if (Game == null)
                    return false;
                //if Rules are different from magic
                if (Game.Rules.Rule != Rules.krMagic)
                    return false;
                //if no rolls in store
                if (RoamingSettings.GetForthRollsCount(this) == 0)
                    return false;
                return _IsForthRolllAvailable;
            }
            set
            {
                if (_IsForthRolllAvailable != value)
                {
                    _IsForthRolllAvailable = value;
                    NotifyPropertyChanged("IsForthRollAvailable");
                }
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

        public bool IsDefaultName
        {
            get
            {
                var nameparts = Name.Split(' ');
                if (nameparts.Length == 2 && nameparts[0].ToLower() == Messages.PLAYER_NAME_DEFAULT.Localize().ToLower())
                {
                    int n;
                    if (int.TryParse(nameparts[1], out n))
                    {
                        
                        return true;
                    }
                }
                return false;
            }
        }

        

        /// <summary>
        /// Returns if player can buy artifacts
        /// only with unique name and password can buy
        /// </summary>
        public bool CanBuy
        {
            get
            {
                if (string.IsNullOrEmpty(Name) || !HasPassword || IsDefaultName)
                    return false;
                return true;
            }
        }

        public int MagicRollsCount
        {
            get
            {
                return RoamingSettings.GetMagicRollsCount(this);
            }
        }
        public int ManualSetsCount
        {
            get
            {
                return RoamingSettings.GetManualSetsCount(this);
            }
        }
        public int RollResetsCount
        {
            get
            {
                return RoamingSettings.GetForthRollsCount(this);
            }
        }

        public bool HasArtifacts
        {
            get
            {
                return MagicRollsCount!=0 && ManualSetsCount!=0 && RollResetsCount!=0;
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


        #endregion

        #region Methods
        
        private void Delete()
        {
            if (DeletePressed != null)
                DeletePressed(this,null);
        }

        public void UpdateType()
        {
            NotifyPropertyChanged("IsHuman");
        }

        public void RefreshArtifactsInfo(bool aftersync=false, bool forcesync=false)
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

        private void OnMagicPressed()
        {
            if (MagicPressed != null)
                MagicPressed(this, null);
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
