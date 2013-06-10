#if WinRT
using DicePokerRT;
using DicePokerRT.KniffelLeaderBoardService;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
#endif
#if WINDOWS_PHONE
using System.Windows.Controls.Primitives;
using DicePokerWP.KniffelLeaderBoardService;
using DicePokerWP;
using Coding4Fun.Phone.Controls;
#endif
using Sanet.Common;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.AllWrite;


namespace Sanet.Kniffel.ViewModels
{
    public abstract class NewGameViewModelBase : AdBasedViewModel
    {

        protected Popup _magicPopup = new Popup();
        protected MagicRoomPage _magic = new MagicRoomPage();

        public event EventHandler MagicPageOpened;

        
        #region Constructor
        public NewGameViewModelBase()
        {

            _magicPopup.Child = _magic;
            _magic.Tag = _magicPopup;

        }
        #endregion

        #region Properties
        
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
        /// Players list
        /// </summary>
        protected ObservableCollection<PlayerWrapper> _Players;
        public ObservableCollection<PlayerWrapper> Players
        {
            get { return _Players; }
            set
            {
                if (_Players != value)
                {
                    _Players = value;
                    NotifyPropertyChanged("Players");
                }
            }
        }

        /// <summary>
        /// Selected player, used to delete and maybe other actions
        /// </summary>
        protected PlayerWrapper _SelectedPlayer;
        abstract public PlayerWrapper SelectedPlayer { get; set; }
        
        /// <summary>
        /// Do we have selected player?
        /// </summary>
        public abstract bool IsPlayerSelected {get;}
        

        /// <summary>
        /// Rules list
        /// </summary>

        private ObservableCollection<RuleWrapper> _Rules;
        public ObservableCollection<RuleWrapper> Rules
        {
            get { return _Rules; }
            set
            {
                if (_Rules != value)
                {
                    _Rules = value;
                    NotifyPropertyChanged("Rules");
                }
            }
        }


        
        /// <summary>
        /// Selected rule for game
        /// </summary>
        protected RuleWrapper _SelectedRule;
        public virtual RuleWrapper SelectedRule
        {
            get 
            {
                return _SelectedRule;
            }
            set
            {
                if (_SelectedRule != value)
                {
                    if (value != null)
                    {
                        if (_SelectedRule != null)
                            _SelectedRule.IsSelected = false;
                        _SelectedRule = value;
                        _SelectedRule.IsSelected = true;
                    }
                    NotifyPropertyChanged("SelectedRule");
                    NotifyPropertyChanged("IsReadyToPlay");
                }
            }
        }

        
        /// <summary>
        /// Conditions to start game
        /// </summary>
        public abstract bool IsReadyToPlay
        {
            get;
        }

        #endregion

        #region Methods
        protected void ChangeUserPass(PlayerWrapper p)
        {
#if SILVERLIGHT
            PasswordInputPrompt input = new PasswordInputPrompt
            {
                Background = Brushes.SolidSanetBlue,
                Value = p.Password
            };

            input.Completed += (s, e1) =>
            {
                p.Password = input.Value;
                p.RefreshArtifactsInfo();
            };

            input.Show();
#endif
        }

        protected void ChangeUserName(PlayerWrapper p)
        {
#if SILVERLIGHT
            InputPrompt input = new InputPrompt
            {
                Title = "ChangeNameLabel".Localize(),
                Background = Brushes.SolidSanetBlue,
                Value = p.Name
            };

            input.Completed += (s, e1) =>
            {
                if (p.Name != input.Value)
                    p.Name = input.Value;
            };

            input.Show();
#endif
        }

        protected void p_MagicPressed(object sender, EventArgs e)
        {

            _magic.GetViewModel<MagicRoomViewModel>().CurrentPlayer = (PlayerWrapper)sender;
            _magicPopup.IsOpen = true;
            if (MagicPageOpened != null)
                MagicPageOpened(null, null);
        }

        public void CloseMagicPage()
        {
            _magicPopup.IsOpen = false;
        }


        protected abstract void FillPlayers();

        /// <summary>
        /// fills players list
        /// </summary>
        public void FillRules()
        {
            Rules = new ObservableCollection<RuleWrapper>();
            //create all possible rules
#if WinRT
            var rulesList = Enum.GetValues(typeof(Rules));
#endif
#if WINDOWS_PHONE
            var rulesList = EnumCompactExtension.GetValues<Rules>().ToList();
#endif
            foreach (Rules rule in rulesList)
                Rules.Add(new RuleWrapper( new KniffelRule(rule)));
            NotifyPropertyChanged("Rules");
            //try to get prev selected from roaming
            var lastRule = RoamingSettings.LastRule;
            SelectedRule = Rules.FirstOrDefault(f => f.Rule.Rule == lastRule);
            if (SelectedRule == null)
                SelectedRule = Rules[0];
            
        }
               

        /// <summary>
        /// Sync artifacts data with server
        /// </summary>
        protected async void ArtifactsSyncRequest(object sender, EventArgs e)
        {
            var player = sender as PlayerWrapper;
            if (InternetCheker.IsInternetAvailable())
            {
                KniffelServiceSoapClient client = new KniffelServiceSoapClient();
                try
                {
                    
                    int rolls = 0;
                    int manuals = 0;
                    int resets = 0;
#if WinRT
                    GetPlayersMagicsResponse result = await client.GetPlayersMagicsAsync(player.Name, player.Password.Encrypt(33), rolls, manuals, resets);
#endif
#if WINDOWS_PHONE
                    GetPlayersMagicsResponse result = await client.GetPlayersMagicsTaskAsync(player.Name, player.Password.Encrypt(33), rolls, manuals, resets);
#endif
                    player.HadStartupMagic = result.Body.GetPlayersMagicsResult;
                    if (RoamingSettings.GetMagicRollsCount(player.Player) == 0 && result.Body.rolls == 10)
                        Utilities.ShowToastNotification(string.Format(Messages.PLAYER_ARTIFACTS_BONUS.Localize(), player.Name, 10));
                    RoamingSettings.SetMagicRollsCount(player.Player, result.Body.rolls);
                    RoamingSettings.SetManualSetsCount(player.Player, result.Body.manuals);
                    RoamingSettings.SetForthRollsCount(player.Player, result.Body.resets);
#if WinRT
                    await 
#endif
                    client.CloseAsync();

                }
                catch (Exception ex)
                {
                    LogManager.Log("NGVM.SyncArtifacts", ex);
                }
               
                
            }
            player.RefreshArtifactsInfo(true);
            
        }

        
        /// <summary>
        /// Looks  for the free default player name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract string GetNewPlayerName(PlayerType type);
        
        /// <summary>
        /// Update UI according to players state
        /// </summary>
        protected abstract void NotifyPlayersChanged();
       
        /// <summary>
        /// Saves settings to roaming
        /// </summary>
        public abstract void SavePlayers();


        public abstract void StartGame();
        

        #endregion

        

    }
}
