using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sanet.Kniffel.Models
{
    public class KniffelRule
    {
        public KniffelRule(Rules rule)
        {
            Rule = rule;
        }

        #region Properties
        /// <summary>
        /// Rule
        /// </summary>
        private Rules _Rule;
        public Rules Rule
        {
            get { return _Rule; }
            set
            {
                if (_Rule != value)
                {
                    _Rule = value;
                    //NotifyPropertyChanged("Rule");
                    //NotifyPropertyChanged("RuleNameLocalized");
                }
            }
        }
        
        /// <summary>
        /// Set of Specific combinations for rule
        /// </summary>
        public List<KniffelScores> Scores
        {
            get
            {
                switch (Rule)
                {
                    case Rules.krBaby:
                        return new List<KniffelScores>
                        {
                            KniffelScores.Ones,
                            KniffelScores.Twos,
                            KniffelScores.Threes,
                            KniffelScores.Fours,
                            KniffelScores.Fives,
                            KniffelScores.Sixs,
                            KniffelScores.Kniffel
                        };
                    case Rules.krSimple:
                        return new List<KniffelScores>
                        {
                            KniffelScores.Ones,
                            KniffelScores.Twos,
                            KniffelScores.Threes,
                            KniffelScores.Fours,
                            KniffelScores.Fives,
                            KniffelScores.Sixs,
                            KniffelScores.ThreeOfAKind,
                            KniffelScores.FourOfAKind,
                            KniffelScores.FullHouse,
                            KniffelScores.SmallStraight,
                            KniffelScores.LargeStraight,
                            KniffelScores.Total,
                            KniffelScores.Kniffel
                        };
                    default:
                        return new List<KniffelScores>
                        {
                            KniffelScores.Ones,
                            KniffelScores.Twos,
                            KniffelScores.Threes,
                            KniffelScores.Fours,
                            KniffelScores.Fives,
                            KniffelScores.Sixs,
                            KniffelScores.Bonus,
                            KniffelScores.ThreeOfAKind,
                            KniffelScores.FourOfAKind,
                            KniffelScores.FullHouse,
                            KniffelScores.SmallStraight,
                            KniffelScores.LargeStraight,
                            KniffelScores.Total,
                            KniffelScores.Kniffel
                        };
                }
                
            }
        }
        /// <summary>
        /// Maximum moves count based on rules
        /// </summary>
        public int MaxMove
        {
            get
            {
                switch (Rule)
                {
                    case Models.Rules.krBaby:
                        return 7;


                }
                return 13;
            }
        }

        
        /// <summary>
        /// Helper method to get if we play with extended bonuses
        /// </summary>
        public bool HasExtendedBonuses
        {
            get
            {
                return Rule == Rules.krExtended || Rule == Rules.krMagic;
            }
        }

        /// <summary>
        /// Helper method to get if we play with standard bonuses
        /// </summary>
        public bool HasStandardBonus
        {
            get
            {
                return Rule==Rules.krStandard || Rule == Rules.krExtended || Rule == Rules.krMagic;
            }
        }


        /// <summary>
        /// returns list of available hands
        /// </summary>
        static public KniffelScores[] PokerHands = new KniffelScores[]
        {
            KniffelScores.ThreeOfAKind,
            KniffelScores.FourOfAKind,
            KniffelScores.FullHouse,
            KniffelScores.SmallStraight,
            KniffelScores.LargeStraight,
            KniffelScores.Kniffel
        };
        
        #endregion

        #region Methods

        public override string ToString()
        {
            switch (Rule)
            {
                case Rules.krBaby:
                    return "ScoresB";
                case Rules.krExtended:
                    return "ScoresE";
                case Rules.krStandard:
                    return "ScoresS";
                case Rules.krMagic:
                    return "ScoresM";
            }
            return "Scores";
        }

        #endregion
    }
}
