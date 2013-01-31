using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WinRT
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace Sanet.Kniffel.Models
{
    public class KniffelRule:BaseViewModel
    {
        public KniffelRule(Rules rule)
        {
            Rule = rule;
            LoadScores();
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
                    NotifyPropertyChanged("Rule");
                    NotifyPropertyChanged("RuleNameLocalized");
                }
            }
        }
        /// <summary>
        /// Localized rule name
        /// </summary>
        public string NameLocalized
        {
            get
            {
                return Rule.ToString().Localize().ToUpper();
            }
        }

        /// <summary>
        /// Short description
        /// </summary>
        public string ShortDescriptionLocalized
        {
            get
            {
                return (Rule.ToString()+"Short").Localize();
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

        /*/// <summary>
        /// Icon for main menu
        /// </summary>
        public ImageSource Image
        {
            get 
            {
                switch (Rule)
                {
                    case Rules.krBaby:
                        return new BitmapImage(new Uri("ms-appx:///Assets/Baby.png", UriKind.Absolute));

                    case Rules.krSimple:
                        return new BitmapImage(new Uri("ms-appx:///Assets/Children.png", UriKind.Absolute));

                    case Rules.krStandard:
                        return new BitmapImage(new Uri("ms-appx:///Assets/Customer.png", UriKind.Absolute));
                    case Rules.krExtended:
                        return new BitmapImage(new Uri("ms-appx:///Assets/Expert.png", UriKind.Absolute));
                        
                }
                return null;
            }
            
        }*/
                
        private string _BestScoreLabel;
        public string BestScoreLabel
        {
            get { return _BestScoreLabel; }
            set
            {
                if (_BestScoreLabel != value)
                {
                    _BestScoreLabel = value;
                    NotifyPropertyChanged("BestScoreLabel");
                }
            }
        }
                
        private string _BestScorePlayer;
        public string BestScorePlayer
        {
            get { return _BestScorePlayer; }
            set
            {
                if (_BestScorePlayer != value)
                {
                    _BestScorePlayer = value;
                    NotifyPropertyChanged("BestScorePlayer");
                }
            }
        }
                
        private string _BestScore;
        public string BestScore
        {
            get { return _BestScore; }
            set
            {
                if (_BestScore != value)
                {
                    _BestScore = value;
                    NotifyPropertyChanged("BestScore");
                }
            }
        }

        /// <summary>
        /// Deetermines if recordscore loading in progress
        /// </summary>
        private bool _IsScoreLoading;
        public bool IsScoreLoading
        {
            get { return _IsScoreLoading; }
            set
            {
                if (_IsScoreLoading != value)
                {
                    _IsScoreLoading = value;
                    NotifyPropertyChanged("IsScoreLoading");
                }
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
        private async void LoadScores()
        {
            if (InternetCheker.IsInternetAvailable())
            {
                try
                {
                    var ks = new 
#if WinRT
                        DicePokerRT
#else
                    DicePokerWP
#endif
                        .KniffelLeaderBoardService.KniffelServiceSoapClient();

                    IsScoreLoading = true;
#if WinRT
                    var res = await ks.GetLastWeekChempionAsync(this.ToString(), BestScorePlayer, BestScore);
                    if (!string.IsNullOrEmpty(res.Body.Score))
                    {
                        BestScore = res.Body.Score;
                        BestScorePlayer = res.Body.Name;
                        IsScoreLoading = false;
                        BestScoreLabel = "BestWeekLabel".Localize();
                    }
                    else
                        LoadLocalScores();
#else
                    var res = (await ks.GetLastWeekChempionTaskAsync(this.ToString())).ToList();
                    if (!string.IsNullOrEmpty(res[1]))
                    {
                        BestScore = res[1];
                        BestScorePlayer = res[0];
                        IsScoreLoading = false;
                        BestScoreLabel = "BestWeekLabel".Localize();
                    }
                    else
                        LoadLocalScores();
#endif

                }
                catch (Exception ex)
                {
                    var t = ex.Message;
                    LoadLocalScores();
                }
                finally
                {
                    IsScoreLoading = false;
                }
            }
            else
            {
                LoadLocalScores();
            }
        }

        private void LoadLocalScores()
        {
            switch (Rule)
            {
                case Rules.krBaby:
                    BestScore = RoamingSettings.LocalBabyRecord.ToString();
                    break;
                case Rules.krExtended:
                    BestScore = RoamingSettings.LocalExtendedRecord.ToString();
                    break;
                case Rules.krStandard:
                    BestScore = RoamingSettings.LocalStandardRecord.ToString();
                    break;
                case Rules.krSimple:
                    BestScore = RoamingSettings.LocalSimpleRecord.ToString();
                    break;
                case Rules.krMagic:
                    BestScore = RoamingSettings.LocalMagicRecord.ToString();
                    break;
            }
            BestScorePlayer = "";
            BestScoreLabel = "BestLocalShort".Localize();
        }

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
