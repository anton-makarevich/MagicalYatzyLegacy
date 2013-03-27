
using DicePokerRT;
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
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;

namespace Sanet.Kniffel.ViewModels
{
    public abstract class NewGameViewModelBase : AdBasedViewModel
    {
        protected Popup _magicPopup = new Popup();
        protected MagicRoomPage _magic = new MagicRoomPage();

        #region Constructor
        public NewGameViewModelBase()
        {
            
            _magicPopup.Child = _magic;
            _magic.Tag = _magicPopup;
            //fillRules();
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
        /// Selected player, used to delete and maybe other actions
        /// </summary>
        protected Player _SelectedPlayer;
        abstract public Player SelectedPlayer{get;set;}
        
        /// <summary>
        /// Do we have selected player?
        /// </summary>
        public abstract bool IsPlayerSelected {get;}
        

        /// <summary>
        /// Rules list
        /// </summary>

        private List<RuleWrapper> _Rules;
        public List<RuleWrapper> Rules
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
        private RuleWrapper _SelectedRule;
        public RuleWrapper SelectedRule
        {
            get { return _SelectedRule; }
            set
            {
                if (_SelectedRule != value)
                {
                    if (value!=null)
                        _SelectedRule = value;
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
        
        protected void p_MagicPressed(object sender, EventArgs e)
        {
            _magic.GetViewModel<MagicRoomViewModel>().CurrentPlayer = (Player)sender;
            _magicPopup.IsOpen = true;
        }
        /// <summary>
        /// fills players list
        /// </summary>
        public void FillRules()
        {
            Rules = new List<RuleWrapper>();
            //create all possible rules
            foreach (Rules rule in Enum.GetValues(typeof(Rules)))
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
            var player = sender as Player;
            if (InternetCheker.IsInternetAvailable())
            {
                KniffelServiceSoapClient client = new KniffelServiceSoapClient();
                try
                {
                    
                    int rolls = 0;
                    int manuals = 0;
                    int resets = 0;

                    var result = await client.GetPlayersMagicsAsync(player.Name, player.Password.Encrypt(33), rolls, manuals, resets);
                    player.HadStartupMagic = result.Body.GetPlayersMagicsResult;
                    if (RoamingSettings.GetMagicRollsCount(player) == 0 && result.Body.rolls == 10)
                        Utilities.ShowToastNotification(string.Format(Messages.PLAYER_ARTIFACTS_BONUS.Localize(), player.Name, 10));
                    RoamingSettings.SetMagicRollsCount(player, result.Body.rolls);
                    RoamingSettings.SetManualSetsCount(player, result.Body.manuals);
                    RoamingSettings.SetForthRollsCount(player, result.Body.resets);
                    await client.CloseAsync();
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
