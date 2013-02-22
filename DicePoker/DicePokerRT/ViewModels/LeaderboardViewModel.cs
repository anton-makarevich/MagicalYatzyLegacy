
using DicePokerRT.KniffelLeaderBoardService;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;

namespace Sanet.Kniffel.ViewModels
{
    public class LeaderboardViewModel : AdBasedViewModel
    {
        #region Constructor
        public LeaderboardViewModel()
        {
            //RefreshScores();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Page title
        /// </summary>
        public string Title
        {
            get
            {
                return "LeaderboardAction".Localize();
            }
        }

        /// <summary>
        /// baby rules label
        /// </summary>
        public string RulesBLabel
        {
            get
            {
                return Rules.krBaby.ToString().Localize();
            }
        }
        /// <summary>
        /// simple rules label
        /// </summary>
        public string RulesLabel
        {
            get
            {
                return Rules.krSimple.ToString().Localize();
            }
        }
        /// <summary>
        /// standard rules label
        /// </summary>
        public string RulesSLabel
        {
            get
            {
                return Rules.krStandard.ToString().Localize();
            }
        }
        /// <summary>
        /// advanced rules label
        /// </summary>
        public string RulesELabel
        {
            get
            {
                return Rules.krExtended.ToString().Localize();
            }
        }
        /// <summary>
        /// magic rules label
        /// </summary>
        public string RulesMLabel
        {
            get
            {
                return Rules.krMagic.ToString().Localize();
            }
        }

        /// <summary>
        /// go to full leaderboard label
        /// </summary>
        public string NavigateToLeaderboardLabel
        {
            get
            {
                return Messages.LEADERBOARD_ALL_RECORDS.Localize();
            }
        }
        /// <summary>
        /// player name label
        /// </summary>
        public string PlayerNameLabel
        {
            get
            {
                return Messages.PLAYER_NAME_DEFAULT.Localize();
            }
        }
        /// <summary>
        /// player games label
        /// </summary>
        public string PlayerGamesLabel
        {
            get
            {
                return Messages.PLAYER_SCORE_COUNT.Localize();
            }
        }
        /// <summary>
        /// player name label
        /// </summary>
        public string PlayerTotalLabel
        {
            get
            {
                return Messages.PLAYER_SCORE_TOTAL.Localize();
            }
        }
        /// <summary>
        /// player score label
        /// </summary>
        public string PlayerScoreLabel
        {
            get
            {
                return Messages.PLAYER_SCORE.Localize();
            }
        }

        /// <summary>
        /// Scores for simple 
        /// </summary>
        private ObservableCollection<KniffelScoreWrapper> _Scores= new ObservableCollection<KniffelScoreWrapper>();
        public ObservableCollection<KniffelScoreWrapper> Scores
        {
            get { return _Scores; }
            set
            {
                if (_Scores != value)
                {
                    _Scores = value;
                    NotifyPropertyChanged("Scores");
                }
            }
        }
        /// <summary>
        /// Scores for baby
        /// </summary>
        private ObservableCollection<KniffelScoreWrapper> _ScoresB = new ObservableCollection<KniffelScoreWrapper>();
        public ObservableCollection<KniffelScoreWrapper> ScoresB
        {
            get { return _ScoresB; }
            set
            {
                if (_ScoresB != value)
                {
                    _ScoresB = value;
                    NotifyPropertyChanged("ScoresB");
                }
            }
        }
        /// <summary>
        /// Scores for standard
        /// </summary>
        private ObservableCollection<KniffelScoreWrapper> _ScoresS = new ObservableCollection<KniffelScoreWrapper>();
        public ObservableCollection<KniffelScoreWrapper> ScoresS
        {
            get { return _ScoresS; }
            set
            {
                if (_ScoresS != value)
                {
                    _ScoresS = value;
                    NotifyPropertyChanged("ScoresS");
                }
            }
        }
        /// <summary>
        /// Scores for advanced
        /// </summary>
        private ObservableCollection<KniffelScoreWrapper> _ScoresE = new ObservableCollection<KniffelScoreWrapper>();
        public ObservableCollection<KniffelScoreWrapper> ScoresE
        {
            get { return _ScoresE; }
            set
            {
                if (_ScoresE != value)
                {
                    _ScoresE = value;
                    NotifyPropertyChanged("ScoresE");
                }
            }
        }
        /// <summary>
        /// Scores for magic
        /// </summary>
        private ObservableCollection<KniffelScoreWrapper> _ScoresM = new ObservableCollection<KniffelScoreWrapper>();
        public ObservableCollection<KniffelScoreWrapper> ScoresM
        {
            get { return _ScoresM; }
            set
            {
                if (_ScoresM != value)
                {
                    _ScoresM = value;
                    NotifyPropertyChanged("ScoresM");
                }
            }
        }
        
        /// <summary>
        /// Flag to show waiting ring on loading 'baby' scores
        /// </summary>
        private bool _ScoresBLoading;
        public bool ScoresBLoading
        {
            get { return _ScoresBLoading; }
            set
            {
                if (_ScoresBLoading != value)
                {
                    _ScoresBLoading = value;
                    NotifyPropertyChanged("ScoresBLoading");
                }
            }
        }
        /// <summary>
        /// Flag to show waiting ring on loading 'simple' scores
        /// </summary>
        private bool _ScoresLoading;
        public bool ScoresLoading
        {
            get { return _ScoresLoading; }
            set
            {
                if (_ScoresLoading != value)
                {
                    _ScoresLoading = value;
                    NotifyPropertyChanged("ScoresLoading");
                }
            }
        }
        /// <summary>
        /// Flag to show waiting ring on loading 'standard' scores
        /// </summary>
        private bool _ScoresSLoading;
        public bool ScoresSLoading
        {
            get { return _ScoresSLoading; }
            set
            {
                if (_ScoresSLoading != value)
                {
                    _ScoresSLoading = value;
                    NotifyPropertyChanged("ScoresSLoading");
                }
            }
        }
        /// <summary>
        /// Flag to show waiting ring on loading 'advanced' scores
        /// </summary>
        private bool _ScoresELoading;
        public bool ScoresELoading
        {
            get { return _ScoresELoading; }
            set
            {
                if (_ScoresELoading != value)
                {
                    _ScoresELoading = value;
                    NotifyPropertyChanged("ScoresELoading");
                }
            }
        }
        /// <summary>
        /// Flag to show waiting ring on loading 'magic' scores
        /// </summary>
        private bool _ScoresMLoading;
        public bool ScoresMLoading
        {
            get { return _ScoresMLoading; }
            set
            {
                if (_ScoresMLoading != value)
                {
                    _ScoresMLoading = value;
                    NotifyPropertyChanged("ScoresMLoading");
                }
            }
        }

        #endregion

        #region Methods

        public void RefreshScores()
        {
            if (InternetCheker.IsInternetAvailable())
            {
                GetScores();
                GetScoresB();
                GetScoresS();
                GetScoresE();
                GetScoresM();
            }
            else
            {
                Utilities.ShowMessage("NoInetMessage".Localize(), Messages.APP_NAME.Localize());
            }
        }

        /// <summary>
        /// Loads scores for simple rules
        /// </summary>
        private async void GetScores()
        {
            Scores = new ObservableCollection<KniffelScoreWrapper>();
            var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
            ScoresLoading = true;
            try
            {
                var res =await ks.GetTopPlayersAsync("simple", null);
                foreach (KniffelScore score in res.Body.Players)
                    Scores.Add(new KniffelScoreWrapper(score));
                NotifyPropertyChanged("Scores");
            }
            catch { }
            finally
            {
                ScoresLoading = false;
            }
        }
        /// <summary>
        /// Loads scores for baby rules
        /// </summary>
        private async void GetScoresB()
        {
            ScoresB = new ObservableCollection<KniffelScoreWrapper>();
            var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
            ScoresBLoading = true;
            try
            {
                var res = await ks.GetTopPlayersAsync("baby", null);
                foreach (KniffelScore score in res.Body.Players)
                    ScoresB.Add(new KniffelScoreWrapper(score));
                NotifyPropertyChanged("ScoresB");
            }
            catch { }
            finally
            {
                ScoresBLoading = false;
            }

        }
        /// <summary>
        /// Loads scores for standard rules
        /// </summary>
        private async void GetScoresS()
        {
            ScoresS = new ObservableCollection<KniffelScoreWrapper>();
            var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
            ScoresSLoading = true;
            try
            {
                var res = await ks.GetTopPlayersAsync("standard", null);
                foreach (KniffelScore score in res.Body.Players)
                    ScoresS.Add(new KniffelScoreWrapper(score));
                NotifyPropertyChanged("ScoresS");
            }
            catch { }
            finally
            {
                ScoresSLoading = false;
            }
        }
        /// <summary>
        /// Loads scores for advanced rules
        /// </summary>
        private async void GetScoresE()
        {
            ScoresE = new ObservableCollection<KniffelScoreWrapper>();
            var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
            ScoresELoading = true;
            try
            {
                var res = await ks.GetTopPlayersAsync("full", null);
                foreach (KniffelScore score in res.Body.Players)
                    ScoresE.Add(new KniffelScoreWrapper(score));
                NotifyPropertyChanged("ScoresE");
            }
            catch { }
            finally
            {
                ScoresELoading = false;
            }
        }
        /// <summary>
        /// Loads scores for magical rules
        /// </summary>
        private async void GetScoresM()
        {
            ScoresE = new ObservableCollection<KniffelScoreWrapper>();
            var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
            ScoresMLoading = true;
            try
            {
                var res = await ks.GetTopPlayersAsync("magic", null);
                foreach (KniffelScore score in res.Body.Players)
                    ScoresM.Add(new KniffelScoreWrapper(score));
                NotifyPropertyChanged("ScoresM");
            }
            catch { }
            finally
            {
                ScoresMLoading = false;
            }
        }

        #endregion

        #region Commands
        //public RelayCommand AddPlayerCommand { get; set; }
        
        
        //protected void CreateCommands()
        //{
        //    AddPlayerCommand = new RelayCommand(o => AddPlayer(PlayerType.Local), () => true);
            
        //}



        #endregion


    }
}
