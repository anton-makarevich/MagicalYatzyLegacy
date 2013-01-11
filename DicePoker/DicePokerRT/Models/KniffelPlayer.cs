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
        #region Prperties
        /// <summary>
        /// Player display name
        /// </summary>
        public string Name { get; set; }
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
                    switch (result.ScoreType)
                    {
                        case KniffelScores.Ones:
                            result.PossibleValue = Game.LastDiceResult.KniffelNumberScore(1);
                            break;
                        case KniffelScores.Twos:
                            result.PossibleValue = Game.LastDiceResult.KniffelNumberScore(2);
                            break;
                        case KniffelScores.Threes:
                            result.PossibleValue = Game.LastDiceResult.KniffelNumberScore(3);
                            break;
                        case KniffelScores.Fours:
                            result.PossibleValue = Game.LastDiceResult.KniffelNumberScore(4);
                            break;
                        case KniffelScores.Fives:
                            result.PossibleValue = Game.LastDiceResult.KniffelNumberScore(5);
                            break;
                        case KniffelScores.Sixs:
                            result.PossibleValue = Game.LastDiceResult.KniffelNumberScore(6);
                            break;
                        case KniffelScores.ThreeOfAKind:
                            result.PossibleValue = Game.LastDiceResult.KniffelOfAKindScore(3);
                            break;
                        case KniffelScores.FourOfAKind:
                            result.PossibleValue = Game.LastDiceResult.KniffelOfAKindScore(4);
                            break;
                        case KniffelScores.FullHouse:
                            result.PossibleValue = Game.LastDiceResult.KniffelFullHouseScore();
                            break;
                        case KniffelScores.SmallStraight:
                            result.PossibleValue = Game.LastDiceResult.KniffelSmallStraightScore();
                            break;
                        case KniffelScores.LargeStraight:
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

        public void UpdateTotal()
        {
            NotifyPropertyChanged("Total");
        }

        public void Init()
        {
            Roll = 1;
            var results = new List<RollResult>();
            foreach (var score in _Game.Rules.Scores)
                results.Add(new RollResult { ScoreType = score });
            Results = results;
        }

        #endregion

    }
}
