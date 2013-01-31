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
            }
        }
        /// <summary>
        /// Player Password
        /// </summary>
        public string Password { get; set; }
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
                    return _RememberPass;
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
        public bool IsForthRolllAvailable
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
                    NotifyPropertyChanged("IsForthRolllAvailable");
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
            IsForthRolllAvailable = true;
            var results = new List<RollResult>();
            foreach (var score in _Game.Rules.Scores)
                results.Add(new RollResult { ScoreType = score });
            IsMoving = false;
            Results = results;
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
        #endregion

        public void OnMagicRollUsed()
        {
            IsMagicRollAvailable = false;
        }

        #region Commands
        public RelayCommand DeleteCommand { get; set; }

        protected void CreateCommands()
        {
            DeleteCommand = new RelayCommand(o => Delete(), () => true);
        }



        #endregion

    }
}
