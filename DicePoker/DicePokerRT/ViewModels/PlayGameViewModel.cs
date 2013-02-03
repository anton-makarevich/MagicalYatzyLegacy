
using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Sanet.Kniffel.ViewModels
{
    public class PlayGameViewModel : AdBasedViewModel
    {
        #region Constructor
        public PlayGameViewModel()
        {
            CreateCommands();
            
        }
        #endregion

        #region Properties
        /// <summary>
        /// Page title
        /// </summary>
        string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    NotifyPropertyChanged("Title");
                }
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
        /// Rules label
        /// </summary>
        public string RulesLabel
        {
            get
            {
                return Messages.NEW_GAME_RULES.Localize();
            }
        }
        /// <summary>
        /// Rules name label
        /// </summary>
        public string RulesNameLabel
        {
            get
            {
                if (Game != null)
                    return Game.Rules.Rule.ToString().Localize();
                return null;
            }
        }
        /// <summary>
        /// Roll button label
        /// </summary>
        public string RollLabel
        {
            get
            {
                if (SelectedPlayer != null)
                    return string.Format("{0} {1}", Messages.GAME_ROLL.Localize(),SelectedPlayer.Roll);
                return string.Empty;
            }
        }
        /// <summary>
        /// ClearButton label
        /// </summary>
        public string ClearLabel
        {
            get
            {
                return Messages.GAME_CLEAR.Localize();
            }
        }
        
        /// <summary>
        /// Play Again Button label
        /// </summary>
        public string PlayAgainLabel
        {
            get
            {
                return Messages.GAME_PLAY_AGAIN.Localize();
            }
        }

        /// <summary>
        /// Total for result label
        /// </summary>
        public string TotalLabel
        {
            get
            {
                return KniffelScores.Total.ToString().Localize();
            }
        }

        /// <summary>
        /// Players list
        /// </summary>
        public ObservableCollection<Player> Players
        {
            get
            {
                if (Game.Players == null)
                    return null;
                return new ObservableCollection<Player>(Game.Players);
            }
            
        }

        /// <summary>
        /// If dices can be rolled
        /// </summary>
        private bool _CanRoll=true;
        public bool CanRoll
        {
            get { return _CanRoll; }
            
        }

        /// <summary>
        /// If dices can be rolled
        /// </summary>
        public bool CanFix
        {
            get 
            {
                if (!IsPlayerSelected)
                    return  false;
                else if (!SelectedPlayer.IsHuman)
                    return  false;
                if (SelectedPlayer.Roll == 1)
                    return false;
                return true;
            }

        }

        /// <summary>
        /// Selected player -actually current player;
        /// </summary>
        public Player SelectedPlayer
        {
            get
            {
                return Game.CurrentPlayer;
            }
        }
        /// <summary>
        /// Do we have selected player?
        /// </summary>
        public bool IsPlayerSelected
        {
            get
            {
                return SelectedPlayer != null;
            }
        }
        
        private KniffelGame _Game;
        public KniffelGame Game
        {
            get { return _Game; }
            set
            {
                RemoveGameHandlers();
                if (_Game != value)
                {
                    _Game = value;
                    AddGameHandlers();
                    NotifyPropertyChanged("Game");
                }
            }
        }

        /// <summary>
        /// Roll results for user to show
        /// </summary>
        List<RollResult> _RollResults;
        public List<RollResult> RollResults
        {
            get
            {
                return _RollResults;
            }
            set
            {
                _RollResults = value;
                NotifyPropertyChanged("RollResults");
            }
        }

        /// <summary>
        /// Results list to bind to table side caption
        /// </summary>
        public List<RollResult> SampleResults
        {
            get
            {
                if (IsPlayerSelected)
                    return SelectedPlayer.Results;
                return null;
            }
            
        }

        /// <summary>
        /// on window size change
        /// </summary>
        public void UpdateDPWidth()
        {
            NotifyPropertyChanged("DicePanelRTWidth");
            NotifyPropertyChanged("DicePanelRTHeight");
        }

        /// <summary>
        /// View state manadgment for dice panel
        /// </summary>
        public double DicePanelRTWidth
        {
            get
            {
                if (ApplicationView.Value == ApplicationViewState.FullScreenLandscape)
                    return 1366 - 230 - (60 * Players.Count);
                else if (ApplicationView.Value == ApplicationViewState.Filled)
                    return 1024 - 230 - (60 * Players.Count);
                else if (ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
                    return 768 - 230 - (60 * Players.Count);

                return 1238;
            }
        }
        public double DicePanelRTHeight
        {
            get
            {
                if (ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
                    return 1136;

                return 538;
            }
        }

        //Magic things
        public bool IsMagicRollVisible
        {
            get
            {
                if (Game == null||Game.Rules.Rule != Rules.krMagic)
                    return false;
                if (SelectedPlayer == null)
                    return false;
                return SelectedPlayer.IsMagicRollAvailable;
            }
        }
        /// <summary>
        /// Wheather we can use magic roll
        /// </summary>
        public bool IsMagicRollEnabled
        {
            get
            {
                if (SelectedPlayer == null)
                    return false;
                if (!CanRoll)
                    return false;
                return true;
            }
        }
        public string MagicRollLabel
        {
            get
            {
                return "MagicRollLabel".Localize();
            }
        }

        public bool IsManualSetVisible
        {
            get
            {
                if (Game == null || Game.Rules.Rule != Rules.krMagic)
                    return false;
                if (SelectedPlayer == null)
                    return false;
                return SelectedPlayer.IsManualSetlAvailable;
            }
        }
        /// <summary>
        /// Wheather we can use manual set
        /// </summary>
        public bool IsManualSetEnabled
        {
            get
            {
                if (SelectedPlayer == null)
                    return false;
                if (SelectedPlayer.Roll < 2)
                    return false;
                return true;
            }
        }
        public string ManualSetLabel
        {
            get
            {
                return "ManualSetLabel".Localize();
            }
        }

        public bool IsForthRollVisible
        {
            get
            {
                if (Game == null || Game.Rules.Rule != Rules.krMagic)
                    return false;
                if (SelectedPlayer == null)
                    return false;
                return SelectedPlayer.IsForthRollAvailable;
            }
        }
        /// <summary>
        /// Wheather we can use forth roll
        /// </summary>
        public bool IsForthRollEnabled
        {
            get
            {
                if (SelectedPlayer == null)
                    return false;
                if (SelectedPlayer.Roll == 3 && !CanRoll)
                    return true;
                return false;
            }
        }
        public string ForthRollLabel
        {
            get
            {
                return "ForthRollLabel".Localize();
            }
        }

        
        private bool _IsControlsVisible=true;
        public bool IsControlsVisible
        {
            get { return _IsControlsVisible; }
            set
            {
                if (_IsControlsVisible != value)
                {
                    _IsControlsVisible = value;
                    NotifyPropertyChanged("IsControlsVisible");
                }
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// Remove game handlers
        /// should run this to dispose game oblject
        /// </summary>
        public void RemoveGameHandlers()
        {
            try
            {
                if (Game != null)
                {
                    Game.DiceFixed -= Game_DiceFixed;
                    Game.DiceRolled -= Game_DiceRolled;
                    Game.GameFinished -= Game_GameFinished;
                    Game.MoveChanged -= Game_MoveChanged;
                    Game.PlayerJoined -= Game_PlayerJoined;
                    Game.ResultApplied -= Game_ResultApplied;
                    Game.MagicRollUsed -= Game_MagicRollUsed;
                    Game.DiceChanged -= Game_DiceChanged;
                }
            }
            catch { }
        }
        /// <summary>
        /// Game events handlers
        /// all player actions should be as respond to game events
        /// </summary>
        void AddGameHandlers()
        {
            if (Game != null)
            {
                Game.DiceFixed += Game_DiceFixed;
                Game.DiceRolled += Game_DiceRolled;
                Game.GameFinished += Game_GameFinished;
                Game.MoveChanged += Game_MoveChanged;
                Game.PlayerJoined += Game_PlayerJoined;
                Game.ResultApplied += Game_ResultApplied;
                Game.MagicRollUsed += Game_MagicRollUsed;
                Game.DiceChanged += Game_DiceChanged;
            }
        }

        void Game_DiceChanged(object sender, RollEventArgs e)
        {
            SelectedPlayer.CheckRollResults();
            SelectedPlayer.OnManaulSetUsed();
            RollResults = SelectedPlayer.Results.Where(f => !f.HasValue && f.ScoreType != KniffelScores.Bonus).ToList();
            IsControlsVisible = true;
            NotifyPlayerChanged();
        }

        void Game_MagicRollUsed(object sender, PlayerEventArgs e)
        {
            SelectedPlayer.OnMagicRollUsed();
        }

        void Game_ResultApplied(object sender, ResultEventArgs e)
        {
            SelectedPlayer.Results.Find(f => f.ScoreType == e.Result.ScoreType).Value = e.Result.PossibleValue;
            SelectedPlayer.UpdateTotal();
            RollResults = null;
        }

        void Game_PlayerJoined(object sender, PlayerEventArgs e)
        {
            NotifyPropertyChanged("Players");
            NotifyPropertyChanged("DicePanelRTWidth");
        }

        void Game_MoveChanged(object sender, MoveEventArgs e)
        {
            SetCanRoll(true);
            NotifyPlayerChanged();
            
        }

        void Game_GameFinished(object sender, EventArgs e)
        {
            //Utilities.ShowToastNotification("GAMOVER vsem");
            SetCanRoll(false);
            NotifyPlayerChanged();
            if (IsPlayerSelected)
            {
                Title = Messages.GAME_FINISHED.Localize();
                NotifyPropertyChanged("Players");
            }
        
        }

        void Game_DiceRolled(object sender, RollEventArgs e)
        {
           
            SelectedPlayer.CheckRollResults();
            RollResults = null;
            SetCanRoll ( false);
            NotifyPlayerChanged();
        }

        void Game_DiceFixed(object sender, FixDiceEventArgs e)
        {
            
        }

        void NotifyPlayerChanged()
        {
            NotifyPropertyChanged("SelectedPlayer");
            
            NotifyPropertyChanged("RollLabel");
            NotifyPropertyChanged("CanFix");
            NotifyPropertyChanged("SampleResults");
            if (IsPlayerSelected)
                Title = string.Format("{2} {0}, {1}",Game.Move ,SelectedPlayer.Name,Messages.GAME_MOVE.Localize() );
            if (Game.Rules.Rule== Rules.krMagic)
            {
                NotifyPropertyChanged("IsMagicRollEnabled");
                NotifyPropertyChanged("IsManualSetEnabled");
                NotifyPropertyChanged("IsForthRollEnabled");
                NotifyPropertyChanged("IsMagicRollVisible");
                NotifyPropertyChanged("IsManualSetVisible");
                NotifyPropertyChanged("IsForthRollVisible");
            }
        }
        /// <summary>
        /// dices stop
        /// </summary>
        public void OnRollEnd()
        {
            
            bool lastRoll =SelectedPlayer.Roll == 3;

            SetCanRoll(SelectedPlayer.Roll < 3);
            SelectedPlayer.Roll++;

            RollResults = SelectedPlayer.Results.Where(f => !f.HasValue && f.ScoreType != KniffelScores.Bonus).ToList();
            NotifyPlayerChanged();

            //if bot
            if (SelectedPlayer.IsBot)
            {
                if ( lastRoll|| !SelectedPlayer.AINeedRoll())
                    SelectedPlayer.AIDecideFill();
                else
                {
                    SelectedPlayer.AIFixDices();
                    
                    Game.ReportRoll();
                }
            }
            
        }
        /// <summary>
        /// method to check if player can manually roll dices
        /// </summary>
        /// <param name="value"></param>
        void SetCanRoll(bool value)
        {
            if (!IsPlayerSelected)
                _CanRoll = false;
            else if (!SelectedPlayer.IsHuman)
                _CanRoll = false;
            else
                _CanRoll = value;
            NotifyPropertyChanged("CanRoll");
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveResults()
        {
            try
            {
                var needsave = Players.Count(f => f.ShouldSaveResult) > 0;
                bool inet=true;
                if (needsave)
                {
                    inet = InternetCheker.IsInternetAvailable();
                    if (!inet)
                    {
                        Utilities.ShowMessage("NoInetMessage".Localize(), Messages.APP_NAME.Localize());
                    }
                }

                int score, scoreb, scores, scoree, scorem;
                scoreb=RoamingSettings.LocalBabyRecord;
                scoree = RoamingSettings.LocalExtendedRecord;
                score = RoamingSettings.LocalSimpleRecord;
                scores = RoamingSettings.LocalStandardRecord;
                scorem = RoamingSettings.LocalStandardRecord;
                foreach (Player p in Players)
                {
                    if (p.IsBot)
                        continue;
                    if (p.ShouldSaveResult && inet)
                    {
                        var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                        ks.PutScoreIntoTableWithPicAsync(p.Name.Encrypt( 33),p.Password.Encrypt(33), p.Total.ToString().Encrypt(33), Game.Rules.ToString().Encrypt(33), p.PicUrl);
                    }
                    switch (Game.Rules.Rule)
                    {
                        case Rules.krBaby:
                            if (p.Total > scoreb)
                            {
                                scoreb = p.Total;
                                RoamingSettings.LocalBabyRecord = scoreb;
                            }
                            break;
                        case Rules.krExtended:
                            if (p.Total > scoree)
                            {
                                scoree = p.Total;
                                RoamingSettings.LocalExtendedRecord = scoree;
                            }
                            break;
                        case Rules.krSimple:
                            if (p.Total > score)
                            {
                                score = p.Total;
                                RoamingSettings.LocalSimpleRecord = score;
                            }
                            break;
                        case Rules.krStandard:
                            if (p.Total > scores)
                            {
                                scores = p.Total;
                                RoamingSettings.LocalStandardRecord = scores;
                            }
                            break;
                        case Rules.krMagic:
                            if (p.Total > scorem)
                            {
                                scorem = p.Total;
                                RoamingSettings.LocalMagicRecord = scorem;
                            }
                            break;
                    }
                }
                List<string> tileLines = new List<string>();
                if (scorem > 0) tileLines.Add(string.Format("{1} - {0}", Rules.krMagic.ToString().Localize(), scorem));
                if (scoree > 0) tileLines.Add(string.Format("{1} - {0}", Rules.krExtended.ToString().Localize(), scoree));
                if (scores > 0) tileLines.Add(string.Format("{1} - {0}", Rules.krStandard.ToString().Localize(), scores));
                if (score > 0) tileLines.Add(string.Format("{1} - {0}", Rules.krSimple.ToString().Localize(), score));
                if (scoreb > 0) tileLines.Add(string.Format("{1} - {0}", Rules.krBaby.ToString().Localize(), scoreb));

                for (int i = tileLines.Count; i <= 4;i++ )
                    tileLines.Add("");

                TileHelper.UpdateTileContent("main", "BestLocalLabel".Localize(), tileLines[0], tileLines[1], tileLines[2], tileLines[3]);
            }
            catch { }
        }

        /// <summary>
        /// userc clicked 'Play Again'
        /// </summary>
        public void PlayAgain()
        {
            SaveResults();
            Game.RestartGame();
            NotifyPropertyChanged("Players");
        }
        /// <summary>
        /// Player used "Reset Roll magic"
        /// </summary>
        public void ResetRolls()
        {
            SelectedPlayer.Roll = 1;
            SelectedPlayer.OnForthRollUsed();
            RollResults = null;
            SetCanRoll(true);
            NotifyPlayerChanged();
        }
        
        #endregion

        #region Commands
       // public RelayCommand DeleteCommand { get; set; }
        
        protected void CreateCommands()
        {
            //DeleteCommand = new RelayCommand(o => DeletePlayer(), () => true);
        }

        
        #endregion


    }
}
