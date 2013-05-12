
using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Models.Interfaces;
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
    public class PlayGameViewModel : AdBasedViewModel, IPlayGameView
    {

        /// <summary>
        /// current player rolled dices
        /// </summary>
        public event EventHandler<RollEventArgs> DiceRolled;
        /// <summary>
        /// Notify that dice was fixed
        /// </summary>
        public event EventHandler<FixDiceEventArgs> DiceFixed;
        /// <summary>
        /// Notify that move started
        /// </summary>
        public event EventHandler<MoveEventArgs> MoveChanged;
        /// <summary>
        /// Notify that game ended
        /// </summary>
        public event EventHandler GameFinished;


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
        /// Remove ad Button label
        /// </summary>
        public string RemoveAdLabel
        {
            get
            {
                return "RemoveAdLabel".Localize();
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
        /// Play Again Button label
        /// </summary>
        public string ReadyToPlayLabel
        {
            get
            {
                return Messages.GAME_PLAY_READY.Localize();
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
        ObservableCollection<PlayerWrapper> _Players;
        public ObservableCollection<PlayerWrapper> Players
        {
            get
            {
                if (_Players == null)
                {
                    if (Game.Players == null)
                        return null;
                    
                    var resList = new List<PlayerWrapper>();
                    foreach (var p in Game.Players)
                        resList.Add(new PlayerWrapper(p));
                    _Players= new ObservableCollection<PlayerWrapper>(resList);
                }
                
                return _Players;
            }
            
        }

        /// <summary>
        /// If dices can be rolled
        /// </summary>
        private bool _CanRoll=false;
        public bool CanRoll
        {
            get { return _CanRoll; }
            
        }

        /// <summary>
        /// If dices can be fixed by player
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
        /// if player can press 'ready to play'
        /// </summary>
        public bool CanStart
        {
            get
            {
                
                if (CanRoll)
                    return false;
#if ONLINE
                try
                {
                    var sp = Players.FirstOrDefault(f => f.Name == ((KniffelGameClient)Game).MyName);
                    if (sp == null)
                        return false;
                    if (Game.Move < 2 && !sp.IsReady )//&& Game.Roll == 1
                    {
                        Title = "WaitForPlayersLabel".Localize();
                        return true;
                    }
                    else
                        Title = "WaitForGameLabel".Localize();
                }
                catch(Exception ex)
                {
                    var t = ex.Message;
                    return false;
                }
#endif
                   
                return false;
            }
        }

        /// <summary>
        /// Selected player -actually current player;
        /// </summary>
        public PlayerWrapper SelectedPlayer
        {
            get
            {
                if (Game.CurrentPlayer == null)
                    return null;
                return Players.FirstOrDefault(f=>f.SeatNo==Game.CurrentPlayer.SeatNo);
            }
        }
        /// <summary>
        /// Do we have selected player?
        /// </summary>
        public bool IsPlayerSelected
        {
            get
            {
                return SelectedPlayer.Player != null;
            }
        }
        
        
        private IKniffelGame _Game;
        public IKniffelGame Game
        {
            get { return _Game; }
            set
            {
                RemoveGameHandlers();
                if (_Game != value)
                {
                    if(_Players!=null)
                    foreach (var p in Players)
                        p.Dispose();
                    if(_SampleResults!=null)
                    foreach (var r in SampleResults)
                        ((RollResultWrapper)r).Dispose();

                    if (_Game != null)
                        RemoveGameHandlers();

                    _Players = null;
                    _SampleResults = null;

                    _Game = value;
                    AddGameHandlers();
                    NotifyPropertyChanged("Game");
                    if (IsOnlineGame)
                        ChatModel = new ChatViewModel(_Game);

                }
            }
        }

        
        private ChatViewModel _ChatModel;
        public ChatViewModel ChatModel
        {
            get { return _ChatModel; }
            set
            {
                if (_ChatModel != value)
                {
                    _ChatModel = value;
                    NotifyPropertyChanged("ChatModel");
                }
            }
        }


        public bool IsOnlineGame
        {
            get
            {
                if (Game==null)
                    return false;
#if !ONLINE
                return false;
#else
                return Game is KniffelGameClient;
#endif
            }
        }

        /// <summary>
        /// Roll results for user to show
        /// </summary>
        List<IRollResult> _RollResults;
        public List<IRollResult> RollResults
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
        List<IRollResult> _SampleResults;
        public List<IRollResult> SampleResults
        {
            get
            {
                  if (_SampleResults == null)
                    {
                        _SampleResults = new List<IRollResult>();

                        if (Game == null)
                            return null;
                        Player sp = new Player();
                        sp.Game = Game;
                        {
                            foreach (var r in sp.Results)
                                _SampleResults.Add(new RollResultWrapper(r));
                        }
                        sp = null;
                    }
                    return _SampleResults;
                
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
                int playersCount = (Players == null) ? 0 : Players.Count;
                if (ApplicationView.Value == ApplicationViewState.FullScreenLandscape)
                    return 1366 - 230 - (60 * playersCount);
                else if (ApplicationView.Value == ApplicationViewState.Filled)
                    return 1024 - 230 - (60 * playersCount);
                else if (ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
                    return 768 - 230 - (60 * playersCount);

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
                if (SelectedPlayer.Roll == 3 && lastRoll)
                    return true;
                if (SelectedPlayer.Roll > 1 && CanRoll)
                    return true;
                return false;
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
                if (SelectedPlayer.Roll == 3 && lastRoll)
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
            
        
        private string _ChatMessage;
        public string ChatMessage
        {
            get { return _ChatMessage; }
            set
            {
                if (_ChatMessage != value)
                {
                    _ChatMessage = value;
                    NotifyPropertyChanged("ChatMessage");
                }
            }
        }
            
        
        private bool _IsChatOpen;
        public bool IsChatOpen
        {
            get { return _IsChatOpen; }
            set
            {
                //if (_IsChatOpen != value)
                //{
                    _IsChatOpen = value;
                    NotifyPropertyChanged("IsChatOpen");
                //}
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
                    Game.PlayerReady -= Game_PlayerReady;
                    Game.PlayerLeft -= Game_PlayerLeft;
                }
            }
            catch (Exception ex)
            {
                LogManager.Log("PGVM.ClearHandlers", ex);
            }
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
                Game.PlayerReady += Game_PlayerReady;
                Game.PlayerLeft += Game_PlayerLeft;
                Game.OnChatMessage += Game_OnChatMessage;
            }
        }

        void Game_OnChatMessage(object sender, ChatMessageEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        var msg = e.Message;
                        if (msg != null)
                        {
                            if (msg.SenderName != _Game.MyName && !IsChatOpen)
                            {
                                ShowChatMessage(string.Format("{0}: {1}", msg.SenderName, msg.Message));
                            }
                        }
                    });
        }

        void ShowChatMessage(string message)
        {
            ChatMessage = message;
            ChatMessage = "";
        }

        void Game_PlayerLeft(object sender, PlayerEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        var p = _Players.FirstOrDefault(f => f.Name == e.Player.Name);
                        if (p != null)
                        {
                            _Players.Remove(p);
                            p.Dispose();
                            p = null;
                            NotifyPropertyChanged("Players");
                            
                        }
                    });
        }

        void Game_PlayerReady(object sender, PlayerEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        var p = Players.FirstOrDefault(f => f.Name == e.Player.Name);
                        p.IsReady = e.Player.IsReady;
#if ONLINE
                        if (Game.MyName==e.Player.Name)
                            NotifyPropertyChanged("CanStart");
#endif
                    });
        }

        void Game_DiceChanged(object sender, RollEventArgs e)
        {
            SelectedPlayer.Player.CheckRollResults();
            SelectedPlayer.OnManaulSetUsed();
            var resList = new List<IRollResult>();
            foreach (var r in SelectedPlayer.Results.Where(f => !f.HasValue && f.ScoreType != KniffelScores.Bonus).ToList())
                resList.Add(r);
            RollResults = resList;
            IsControlsVisible = true;
            NotifyPlayerChanged();
        }

        void Game_MagicRollUsed(object sender, PlayerEventArgs e)
        {
            SelectedPlayer.OnMagicRollUsed();
        }

        void Game_ResultApplied(object sender, ResultEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        if (e.Result.PossibleValue > 0 || e.Result.HasBonus)
                        {
                            if (e.Result.PossibleValue == 50 || e.Result.HasBonus || e.Result.ScoreType == KniffelScores.Bonus)
                                SoundsProvider.PlaySound(_player, "fanfare");
                            else
                                SoundsProvider.PlaySound(_player, "win");
                        }
                        else
                        {
                            SoundsProvider.PlaySound(_player, "wrong");
                        }
                        var p = Players.FirstOrDefault(f => f.Name == e.Player.Name);
                        var r = p.Results.Find(f => f.ScoreType == e.Result.ScoreType);
                        r.Value = e.Result.PossibleValue;
                        p.UpdateTotal();
                        p.IsMoving = false;
                        NotifyPlayerChanged();
                        RollResults = null;
                    });
        }

        void Game_PlayerJoined(object sender, PlayerEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        if (_Players != null)
                        {
                            var p = Players.FirstOrDefault(f => f.Name == e.Player.Name);
                            if (p==null)
                            _Players.Add(new PlayerWrapper(e.Player));
                        }
                        NotifyPropertyChanged("Players");
                        NotifyPropertyChanged("DicePanelRTWidth");
                        NotifyPropertyChanged("CanStart");
                    });
        }

        void Game_MoveChanged(object sender, MoveEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        if (MoveChanged != null)
                            MoveChanged(this, e);
                        SetCanRoll(true);
                        var p = Players.FirstOrDefault(f=>f.Name==e.Player.Name);
                        if (p != null)
                            p.IsMoving = true;
                        NotifyPlayerChanged();
                    });
            
        }

        void Game_GameFinished(object sender, EventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        //Utilities.ShowToastNotification("GAMOVER vsem");
                        if (GameFinished != null)
                            GameFinished(null, e);
                        SetCanRoll(false);
                        NotifyPlayerChanged();
                        if (IsPlayerSelected)
                        {
                            Title = Messages.GAME_FINISHED.Localize();
                            NotifyPropertyChanged("Players");
                        }
                    });
        }

        void Game_DiceRolled(object sender, RollEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        if (DiceRolled != null)
                            DiceRolled(this, e);
                        SelectedPlayer.Player.CheckRollResults();
                        RollResults = null;
                        SetCanRoll(false);
                        NotifyPlayerChanged();
                    });
        }

        void Game_DiceFixed(object sender, FixDiceEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        if (DiceFixed != null)
                            DiceFixed(this, e);
                        
                    });
        }

        void NotifyPlayerChanged()
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        NotifyPropertyChanged("SelectedPlayer");
                        NotifyPropertyChanged("RollLabel");
                        NotifyPropertyChanged("CanFix");
                        NotifyPropertyChanged("CanStart");

                        if (IsPlayerSelected)
                        {
                            Title = string.Format("{2} {0}, {1}", Game.Move, SelectedPlayer.Name, Messages.GAME_MOVE.Localize());
                            foreach (var pw in Players)
                                pw.Refresh();
                        }
                        if (Game.Rules.Rule == Rules.krMagic)
                        {
                            NotifyPropertyChanged("IsMagicRollEnabled");
                            NotifyPropertyChanged("IsManualSetEnabled");
                            NotifyPropertyChanged("IsForthRollEnabled");
                            NotifyPropertyChanged("IsMagicRollVisible");
                            NotifyPropertyChanged("IsManualSetVisible");
                            NotifyPropertyChanged("IsForthRollVisible");
                        }
                    });
        }
        bool lastRoll;
        /// <summary>
        /// dices stop
        /// </summary>
        public void OnRollEnd()
        {
            
            lastRoll =SelectedPlayer.Roll == 3;

            SetCanRoll(SelectedPlayer.Roll < 3);
            SelectedPlayer.Roll++;

            if (SelectedPlayer.IsHuman)
            {
                var resList = new List<IRollResult>();
                foreach (var r in SelectedPlayer.Results.Where(f => !f.HasValue && f.ScoreType != KniffelScores.Bonus).ToList())
                    resList.Add(r);

                RollResults = resList;
            }

            NotifyPlayerChanged();

            //if bot
            if (SelectedPlayer.IsBot)
            {
                if ( lastRoll|| !SelectedPlayer.Player.AINeedRoll())
                    SelectedPlayer.Player.AIDecideFill();
                else
                {
                    SelectedPlayer.Player.AIFixDices();
                    if (Game.FixedDicesCount==5)
                        SelectedPlayer.Player.AIDecideFill();
                    else
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
            SmartDispatcher.BeginInvoke(() =>
                    {
                        if (!IsPlayerSelected)
                            _CanRoll = false;
                        else if (!SelectedPlayer.IsHuman || !SelectedPlayer.IsReady)
                            _CanRoll = false;
                        else
                            _CanRoll = value;
                        NotifyPropertyChanged("CanRoll");
                        NotifyPropertyChanged("CanStart");
                    });
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task SaveResults()
        {
            IsBusy = true;
            try
            {

                var needsave = Players.Count(f => f.ShouldSaveResult) > 0;
                bool inet = true;
                DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient ks = null;
                if (needsave)
                {
                    inet = InternetCheker.IsInternetAvailable();
                    if (!inet)
                    {
                        Utilities.ShowMessage("NoInetMessage".Localize(), Messages.APP_NAME.Localize());
                    }
                    else
                    {
                        if (ks == null)
                            ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                    }
                }

                int score, scoreb, scores, scoree, scorem;
                scoreb = RoamingSettings.LocalBabyRecord;
                scoree = RoamingSettings.LocalExtendedRecord;
                score = RoamingSettings.LocalSimpleRecord;
                scores = RoamingSettings.LocalStandardRecord;
                scorem = RoamingSettings.LocalStandardRecord;
                foreach (var p in Players)
                {
                    //don't do anything score related for bots
                    if (!p.IsHuman)
                        continue;

                    //decreasing amount of magic artifacts
                    if (Game.Rules.Rule == Rules.krMagic)
                    {
                        int rollsused = (p.IsMagicRollAvailable) ? 0 : -1;
                        int manualsused = (p.IsManualSetlAvailable) ? 0 : -1;
                        int resetsused = (p.IsForthRollAvailable) ? 0 : -1;

                        if (rollsused != 0)
                            RoamingSettings.SetMagicRollsCount(p.Player, RoamingSettings.GetMagicRollsCount(p.Player) + rollsused);
                        if (rollsused != 0)
                            RoamingSettings.SetManualSetsCount(p.Player, RoamingSettings.GetManualSetsCount(p.Player) + manualsused);
                        if (rollsused != 0)
                            RoamingSettings.SetForthRollsCount(p.Player, RoamingSettings.GetForthRollsCount(p.Player) + resetsused);

                        if (ks == null)
                            ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                        await ks.AddPlayersMagicsAsync(p.Name, p.Password.Encrypt(33), rollsused.ToString().Encrypt(33), manualsused.ToString().Encrypt(33), resetsused.ToString().Encrypt(33));
                    }//add bonus
                    else if (Game.Rules.Rule == Rules.krExtended && p.HasPassword)
                    {
                        int addartifacts = 0;

                        if (p.Total > 600)
                            addartifacts = 100;
                        else if (p.Total > 500)
                            addartifacts = 30;
                        else if (p.Total > 400)
                            addartifacts = 10;
                        if (addartifacts > 0)
                        {
                            RoamingSettings.SetMagicRollsCount(p.Player, RoamingSettings.GetMagicRollsCount(p.Player) + addartifacts);
                            RoamingSettings.SetManualSetsCount(p.Player, RoamingSettings.GetManualSetsCount(p.Player) + addartifacts);
                            RoamingSettings.SetForthRollsCount(p.Player, RoamingSettings.GetForthRollsCount(p.Player) + addartifacts);
                            if (ks == null)
                                ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                            var res = await ks.AddPlayersMagicsAsync(p.Name, p.Password.Encrypt(33), addartifacts.ToString().Encrypt(33), addartifacts.ToString().Encrypt(33), addartifacts.ToString().Encrypt(33));
                            if (res.Body.AddPlayersMagicsResult)
                                Utilities.ShowToastNotification(string.Format(Messages.PLAYER_ARTIFACTS_BONUS.Localize(), p.Name, addartifacts));
                        }
                    }

                    //score saving to leaderboard
                    if (p.ShouldSaveResult && inet)
                    {
                        if (ks == null)
                            ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                        LogManager.Log(LogLevel.Message, "GameVM.SaveResults", "{0} scores for {1} will be saved", p.Total, p.Name);
                        int attempt = 1;
                        bool done = false;
                        do
                        {
                            var rs = await ks.PutScoreIntoTableWithPicPureNameAsync(p.Name, p.Password.Encrypt(33), p.Total.ToString().Encrypt(33), Game.Rules.ToString().Encrypt(33), p.Player.PicUrl);
                            done = rs.Body.PutScoreIntoTableWithPicPureNameResult;
                            if (attempt == 3)
                                break;
                            attempt++;
                        }
                        while (!done);
                        LogManager.Log(LogLevel.Message, "GameVM.SaveResults", "Save score operation for {0} result is: {1}, attempts used: {2}", p.Name, done, attempt);

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

                for (int i = tileLines.Count; i <= 4; i++)
                    tileLines.Add("");

                if (inet && ks != null)
                    await ks.CloseAsync();
                TileHelper.UpdateTileContent("main", "BestLocalLabel".Localize(), tileLines[0], tileLines[1], tileLines[2], tileLines[3]);
            }
            catch (Exception ex)
            {
                LogManager.Log("GVM.SaveResults", ex);
            }
            finally 
            {
                IsBusy = false;
            }
        }

        public void StartGame()
        {
            
            foreach (var p in Game.Players)
            {
                p.Game = Game;
                if (!IsOnlineGame)
                    Game.SetPlayerReady(p, true);
            }
            NotifyPropertyChanged("SampleResults");
        }

        /// <summary>
        /// userc clicked 'Play Again'
        /// </summary>
        public async void PlayAgain()
        {
            
            await SaveResults();
            
            Game.RestartGame();
            foreach (var p in Players)
            {
                p.Results = null;
                p.Dispose();
            }
            _Players = null;
            _SampleResults = null;

            NotifyPropertyChanged("Players");
        }
        /// <summary>
        /// Player used "Reset Roll magic"
        /// </summary>
        public void ResetRolls()
        {
            SelectedPlayer.Roll = 1;
            SelectedPlayer.OnForthRollUsed();
            Game.RerollMode = true;
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
