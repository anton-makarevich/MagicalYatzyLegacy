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
            LoadWeekScores();
            LoadDayScores();
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
        //wwek scores
        /// <summary>
        /// just label
        /// </summary>
        private string _BestWeekScoreLabel;
        public string BestWeekScoreLabel
        {
            get { return _BestWeekScoreLabel; }
            set
            {
                if (_BestWeekScoreLabel != value)
                {
                    _BestWeekScoreLabel = value;
                    NotifyPropertyChanged("BestWeekScoreLabel");
                }
            }
        }
                /// <summary>
                /// Best week player name
                /// </summary>
        private string _BestWeekScorePlayer;
        public string BestWeekScorePlayer
        {
            get { return _BestWeekScorePlayer; }
            set
            {
                if (_BestWeekScorePlayer != value)
                {
                    _BestWeekScorePlayer = value;
                    NotifyPropertyChanged("BestWeekScorePlayer");
                }
            }
        }
                /// <summary>
                /// Best week score
                /// </summary>
        private string _BestWeekScore;
        public string BestWeekScore
        {
            get { return _BestWeekScore; }
            set
            {
                if (_BestWeekScore != value)
                {
                    _BestWeekScore = value;
                    NotifyPropertyChanged("BestWeekScore");
                }
            }
        }

        /// <summary>
        /// Deetermines if recordscore loading in progress
        /// </summary>
        private bool _IsWeekScoreLoading;
        public bool IsWeekScoreLoading
        {
            get { return _IsWeekScoreLoading; }
            set
            {
                if (_IsWeekScoreLoading != value)
                {
                    _IsWeekScoreLoading = value;
                    NotifyPropertyChanged("IsWeekScoreLoading");
                }
            }
        }

        //dayscores
        /// <summary>
        /// just label
        /// </summary>
        private string _BestDayScoreLabel;
        public string BestDayScoreLabel
        {
            get { return _BestDayScoreLabel; }
            set
            {
                if (_BestDayScoreLabel != value)
                {
                    _BestDayScoreLabel = value;
                    NotifyPropertyChanged("BestDayScoreLabel");
                }
            }
        }
        /// <summary>
        /// Best week player name
        /// </summary>
        private string _BestDayScorePlayer;
        public string BestDayScorePlayer
        {
            get { return _BestDayScorePlayer; }
            set
            {
                if (_BestDayScorePlayer != value)
                {
                    _BestDayScorePlayer = value;
                    NotifyPropertyChanged("BestDayScorePlayer");
                }
            }
        }
        /// <summary>
        /// Best week score
        /// </summary>
        private string _BestDayScore;
        public string BestDayScore
        {
            get { return _BestDayScore; }
            set
            {
                if (_BestDayScore != value)
                {
                    _BestDayScore = value;
                    NotifyPropertyChanged("BestDayScore");
                }
            }
        }

        /// <summary>
        /// Deetermines if recordscore loading in progress
        /// </summary>
        private bool _IsDayScoreLoading;
        public bool IsDayScoreLoading
        {
            get { return _IsDayScoreLoading; }
            set
            {
                if (_IsDayScoreLoading != value)
                {
                    _IsDayScoreLoading = value;
                    NotifyPropertyChanged("IsDayScoreLoading");
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
        /// <summary>
        /// Get best players for week
        /// </summary>
        private async void LoadWeekScores()
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

                    IsWeekScoreLoading = true;
#if WinRT
                    var res = await ks.GetLastWeekChempionAsync(this.ToString(), BestWeekScorePlayer, BestWeekScore);
                    if (!string.IsNullOrEmpty(res.Body.Score))
                    {
                        BestWeekScore = res.Body.Score;
                        BestWeekScorePlayer = res.Body.Name;
                        IsWeekScoreLoading = false;
                        BestWeekScoreLabel = "BestWeekLabel".Localize();
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
                    IsWeekScoreLoading = false;
                }
            }
            else
            {
                LoadLocalScores();
            }
        }

        // <summary>
        /// Get best players for week
        /// </summary>
        private async void LoadDayScores()
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

                    IsDayScoreLoading = true;
#if WinRT
                    var res = await ks.GetLastDayChempionAsync(this.ToString(), BestDayScorePlayer, BestDayScore);
                    if (!string.IsNullOrEmpty(res.Body.Score))
                    {
                        BestDayScore = res.Body.Score;
                        BestDayScorePlayer = res.Body.Name;
                        IsDayScoreLoading = false;
                        BestDayScoreLabel = "BestDayLabel".Localize();
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
                    IsDayScoreLoading = false;
                }
            }
            else
            {
                LoadLocalScores();
            }
        }

        /// <summary>
        /// Get best local results
        /// </summary>
        private void LoadLocalScores()
        {
            switch (Rule)
            {
                case Rules.krBaby:
                    BestWeekScore=BestDayScore = RoamingSettings.LocalBabyRecord.ToString();
                    break;
                case Rules.krExtended:
                    BestWeekScore =BestDayScore= RoamingSettings.LocalExtendedRecord.ToString();
                    break;
                case Rules.krStandard:
                    BestWeekScore = BestDayScore = RoamingSettings.LocalStandardRecord.ToString();
                    break;
                case Rules.krSimple:
                    BestWeekScore = BestDayScore = RoamingSettings.LocalSimpleRecord.ToString();
                    break;
                case Rules.krMagic:
                    BestWeekScore = BestDayScore = RoamingSettings.LocalMagicRecord.ToString();
                    break;
            }
            BestWeekScorePlayer= BestDayScorePlayer = "";
            BestWeekScoreLabel =BestDayScoreLabel= "BestLocalShort".Localize();
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
