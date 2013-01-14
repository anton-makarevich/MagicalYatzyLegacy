
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
    public class LeaderboardViewModel:BaseViewModel
    {
        #region Constructor
        public LeaderboardViewModel()
        {
            GetScores();
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
                return Messages.NEW_GAME_START.Localize();
            }
        }
        /// <summary>
        /// Start button label
        /// </summary>
        public string StartLabel
        {
            get
            {
                return Messages.NEW_GAME_START_GAME.Localize();
            }
        }
        /// <summary>
        /// Players group label
        /// </summary>
        public string PlayersLabel
        {
            get
            {
                return Messages.NEW_GAME_PLAYERS.Localize();
            }
        }
        /// <summary>
        /// Ruless group label
        /// </summary>
        public string RulesLabel
        {
            get
            {
                return Messages.NEW_GAME_RULES.Localize();
            }
        }
        /// <summary>
        /// Add Player appbar buton caption
        /// </summary>
        public string AddPlayerLabel
        {
            get
            {
                return Messages.NEW_GAME_ADD_HUMAN.Localize();
            }
        }
        /// <summary>
        /// Add Bot appbar buton caption
        /// </summary>
        public string AddBotLabel
        {
            get
            {
                return Messages.NEW_GAME_ADD_BOT.Localize();
            }
        }

        /// <summary>
        /// Players list
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

        

        #endregion

        #region Methods

        private async void GetScores()
        {
            var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
            //ObservableCollection<KniffelScore> SimpleScores=new ObservableCollection<KniffelScore>();
            var res =await ks.GetTopPlayersAsync("simple", null);
            foreach (KniffelScore score in res.Body.Players)
                Scores.Add(new KniffelScoreWrapper(score));
            NotifyPropertyChanged("Scores");
            
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
