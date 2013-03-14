using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;
using Sanet.Kniffel.Models.Enums;

namespace Sanet.Kniffel.Models
{
    public class Player:BaseViewModel
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
        /// Player display name
        /// </summary>
        string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
                Password = "";
                //RememberPass = false;
                HadStartupMagic = false;
                if (!string.IsNullOrEmpty(value))
                    RefreshArtifactsInfo();
                //NotifyPropertyChanged("HasArtifacts");
            }
        }
        /// <summary>
        /// Player Password
        /// </summary>
        string _Password;
        public string Password 
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                NotifyPropertyChanged("Password");
                
                //NotifyPropertyChanged("HasArtifacts");
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
        /// Player ID (GUID?)
        /// </summary>
        public string ID { get; set; }
        
        /// <summary>
        /// is this player moves now
        /// </summary>
        private bool _IsMoving=false; 
        public bool IsMoving
        {
            get { return _IsMoving; }
            set
            {
                if (_IsMoving != value)
                {
                    _IsMoving = value;
                    Roll = 1;
                    NotifyPropertyChanged("IsMoving");
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

        //Public Property GamePlatform As KniffelGamePlatform
        /// <summary>
        /// Avatar URI
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// Player type (human, network or AI controlled)
        /// </summary>
        
        private PlayerType _Type = PlayerType.Local;
        public PlayerType Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    NotifyPropertyChanged("Type");
                    NotifyPropertyChanged("IsBot");
                    NotifyPropertyChanged("IsHuman");
                }
            }
        }

        /// <summary>
        /// Oreder in game
        /// </summary>
        public int SeatNo
        { get; set; }


        /// <summary>
        /// Game instance (game to which this player connected)
        /// </summary>
        private KniffelGame _Game;
        public KniffelGame Game
        {
            get { return _Game; }
            set
            {

                if (_Game != value)
                {
                                                         
                    _Game = value;
                    Init();
                    NotifyPropertyChanged("Game");
                }
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
        /// If ready to start game, has sense only for network game
        /// </summary>
        private bool _IsReady;
        public bool IsReady
        {
            get
            {
                if (Type != PlayerType.Network)
                    return true;
                return _IsReady; 
            }
            set
            {
                if (_IsReady != value)
                {
                    _IsReady = value;
                    NotifyPropertyChanged("IsReady");
                }
            }
        }


        /// <summary>
        /// Returns if current player is bot
        /// </summary>
        public bool IsBot
        {
            get
            {
                return Type == PlayerType.AI;
            }
            set
            {
                if (value)
                    Type = PlayerType.AI;
            }
        }
        /// <summary>
        /// Returns if current player is human
        /// </summary>
        public bool IsHuman
        {
            get
            {
                return Type == PlayerType.Local;
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
                return Messages.PLAYER_PASSWORD.Localize();
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
        
        private List<RollResult> _Results;
        public List<RollResult> Results
        {
            get { return _Results; }
            set
            {
                if (_Results != value)
                {
                    _Results = value;
                    NotifyPropertyChanged("Results");
                }
            }
        }

        
        
        private int _Roll=1;
        public int Roll
        {
            get { return _Roll; }
            set
            {
                if (_Roll != value)
                {
                    if (value > 3)
                        value = 3;
                    if (value < 1)
                        value = 1;
                    _Roll = value;
                    NotifyPropertyChanged("Roll");
                }
            }
        }

        //Scores in current game

        /// <summary>
        /// Total score
        /// </summary>
        public int Total
        {
            get
            {
                var results = (from r in Results where r.HasValue select r.Value).ToList();
                return results.Sum() + Results.Count(f=>f.HasBonus)*100;
            }
        }

        /// <summary>
        /// Total score for numeric hands
        /// </summary>
        public int TotalNumeric
        {
            get
            {
                var results = (from r in Results where (r.IsNumeric && r.HasValue) select r.Value).ToList();
                return results.Sum();
            }
        }
        /// <summary>
        /// posible scores for numeric hands which is not filled yet
        /// </summary>
        public int MaxRemainingNumeric
        {
            get
            {
                var results = (from r in Results where (!r.HasValue && r.IsNumeric) select r.MaxValue).ToList();
                return results.Sum();
            }
        }

        public bool AllNumericFilled
        {
            get
            {
                if (Results != null)
                    return Results.Count(f => f.IsNumeric && !f.HasValue) == 0;
                return false;
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
        /// <summary>
        /// Calculates possible results for each combination
        /// </summary>
        public void CheckRollResults()
        {
            //LogManager.Log(LogLevel.WarningLow, "Player.CheckRollResults",
            //    "Player {0} checking rool result", Name);
            foreach (RollResult result in Results)
            {
                if (!result.HasValue)
                {
                    if (result.IsNumeric)
                        result.PossibleValue = Game.LastDiceResult.KniffelNumberScore((int)result.ScoreType);
                    else
                    switch (result.ScoreType)
                    {
                        case KniffelScores.ThreeOfAKind:
                            result.PossibleValue = Game.LastDiceResult.KniffelOfAKindScore(3);
                            break;
                        case KniffelScores.FourOfAKind:
                            result.PossibleValue = Game.LastDiceResult.KniffelOfAKindScore(4);
                            break;
                        case KniffelScores.FullHouse:
                           //if now kniffel extended rules and kniffel has value (0 or 50)
                            if (IsScoreFilled(KniffelScores.Kniffel) && Game.Rules.HasExtendedBonuses && Game.LastDiceResult.KniffelFiveOfAKindScore()==50)
                            {
                                //and numeric result corresponded to that kniffel also filled
                                RollResult nresult = null;
                                nresult=GetResultForScore((KniffelScores)Game.LastDiceResult.DiceResults[0]);
                                if (nresult.HasValue)
                                    result.PossibleValue = 25;//appying kniffel-joker
                                else
                                    result.PossibleValue = 0;
                                break;
                            }
                            result.PossibleValue = Game.LastDiceResult.KniffelFullHouseScore();
                            break;
                        case KniffelScores.SmallStraight:
                            if (IsScoreFilled(KniffelScores.Kniffel) && Game.Rules.HasExtendedBonuses && Game.LastDiceResult.KniffelFiveOfAKindScore() == 50)
                            {
                                //and numeric result corresponded to that kniffel also filled
                                RollResult nresult = null;
                                nresult = GetResultForScore((KniffelScores)Game.LastDiceResult.DiceResults[0]);
                                if (nresult.HasValue)
                                    result.PossibleValue = 30;//appying kniffel-joker
                                else
                                    result.PossibleValue = 0;
                                break;
                            }
                            result.PossibleValue = Game.LastDiceResult.KniffelSmallStraightScore();
                            break;
                        case KniffelScores.LargeStraight:
                            if (IsScoreFilled(KniffelScores.Kniffel) && Game.Rules.HasExtendedBonuses && Game.LastDiceResult.KniffelFiveOfAKindScore()==50)
                            {
                                //and numeric result corresponded to that kniffel also filled
                                RollResult nresult = null;
                                nresult = GetResultForScore((KniffelScores)Game.LastDiceResult.DiceResults[0]);
                                if (nresult.HasValue)
                                    result.PossibleValue = 40;//appying kniffel-joker
                                else
                                    result.PossibleValue = 0;
                                break;
                            }
                            result.PossibleValue = Game.LastDiceResult.KniffelLargeStraightScore();
                            break;
                        case KniffelScores.Total:
                            result.PossibleValue = Game.LastDiceResult.KniffelChanceScore();
                            break;
                        case KniffelScores.Kniffel:
                            result.PossibleValue = Game.LastDiceResult.KniffelFiveOfAKindScore();
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// notifying that total changed
        /// </summary>
        public void UpdateTotal()
        {
            NotifyPropertyChanged("Total");
        }
        /// <summary>
        /// before each game start
        /// </summary>
        public void Init()
        {
            Roll = 1;
            IsMagicRollAvailable = true;
            IsManualSetlAvailable = true;
            IsForthRollAvailable = true;
            var results = new List<RollResult>();
            foreach (var score in _Game.Rules.Scores)
                results.Add(new RollResult { ScoreType = score });
            IsMoving = false;
            Results = results;
            //NotifyPropertyChanged("HasArtifacts");
        }

        public RollResult GetResultForScore(KniffelScores score)
        {
            if (Results==null)
                return null;

            return Results.FirstOrDefault(f => f.ScoreType == score);
        }

        /// <summary>
        /// Determine if current score has any value (including 0)
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool IsScoreFilled(KniffelScores score)
        {
            
            var kresult =GetResultForScore( score);
            if (kresult != null)
                return kresult.HasValue;
                    
            return false;
            
        }

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
                    var nameparts = Name.Split(' ');
                    if (nameparts.Length == 2 && nameparts[0] == Messages.PLAYER_NAME_DEFAULT.Localize())
                    {
                        int n;
                        if (int.TryParse(nameparts[1], out n))
                        {
                            ArtifactsInfoMessage = "ChangeNameLabel".Localize();
                            return;
                        }
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
