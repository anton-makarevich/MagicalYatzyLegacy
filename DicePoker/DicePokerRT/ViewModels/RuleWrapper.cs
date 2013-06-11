using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sanet.Kniffel.Models
{
    public class RuleWrapper:BaseViewModel
    {
        KniffelRule _rule;

        public RuleWrapper(KniffelRule rule)
        {
            _rule = rule;

            LoadWeekScores();
            LoadDayScores();
        }

        #region Properties
        
        /// <summary>
        /// Localized rule name
        /// </summary>
        public string NameLocalized
        {
            get
            {
                return _rule.Rule.ToString().Localize().ToUpper();
            }
        }

        /// <summary>
        /// Short description
        /// </summary>
        public string ShortDescriptionLocalized
        {
            get
            {
                return (_rule.Rule.ToString() + "Short").Localize();
            }
        }

        public KniffelRule Rule
        {
            get
            {
                return _rule;
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
            
        
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        
        
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
                    var res = await ks.GetLastWeekChempionAsync(_rule.ToString(), BestWeekScorePlayer, BestWeekScore);
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
                    var res = (await ks.GetLastWeekChempionTaskAsync(_rule.ToString())).ToList();
                    if (!string.IsNullOrEmpty(res[1]))
                    {
                        BestWeekScore = res[1];
                        BestWeekScorePlayer = res[0];
                        IsWeekScoreLoading = false;
                        BestWeekScoreLabel = "BestWeekLabel".Localize();
                    }
                    else
                        LoadLocalScores();
#endif

                }
                catch (Exception ex)
                {
                    LogManager.Log( "Rule.LoadWeekScores", ex);
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
                    var res = await ks.GetLastDayChempionAsync(_rule.ToString(), BestDayScorePlayer, BestDayScore);
                    if (!string.IsNullOrEmpty(res.Body.Score))
                    {
                        BestDayScore = res.Body.Score;
                        BestDayScorePlayer = res.Body.Name;
                        IsDayScoreLoading = false;
                        BestDayScoreLabel = "BestDayLabel".Localize();
                    }
                    else
                        LoadLocalScores();
                    await
#else
                    var res = (await ks.GetLastDayChempionTaskAsync(_rule.ToString())).ToList();
                    if (!string.IsNullOrEmpty(res[1]))
                    {
                        BestDayScore = res[1];
                        BestDayScorePlayer = res[0];
                        IsDayScoreLoading = false;
                        BestDayScoreLabel = "BestDayLabel".Localize();
                    }
                    else
                        LoadLocalScores();
#endif
                     ks.CloseAsync();
                }
                catch (Exception ex)
                {
                    LogManager.Log("Rule.LoadDayScores", ex);
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
            switch (_rule.Rule)
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
                

        #endregion
    }
}
