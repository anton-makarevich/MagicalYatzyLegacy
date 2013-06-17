#if WinRT
using DicePokerRT;
using DicePokerRT.KniffelLeaderBoardService;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
#endif

using Sanet.AllWrite;
using Sanet.Common;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sanet.Kniffel.ViewModels
{
    public class NewGameViewModel : NewGameViewModelBase
    {
       
        #region Constructor
        public NewGameViewModel()
        {
            CreateCommands();
            FillPlayers();

            
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
        /// Selected player, used to delete and maybe other actions
        /// </summary>
        public override PlayerWrapper SelectedPlayer
        {
            get { return _SelectedPlayer; }
            set
            {
                if (_SelectedPlayer != value)
                {
                    _SelectedPlayer = value;
                    NotifyPropertyChanged("SelectedPlayer");
                    NotifyPropertyChanged("IsPlayerSelected");
                    NotifyPropertyChanged("CanDeletePlayer");
                }
            }
        }
        /// <summary>
        /// Do we have selected player?
        /// </summary>
        public override bool IsPlayerSelected
        {
            get
            {
                return _SelectedPlayer != null;
            }
        }
              
        
        /// <summary>
        /// Returns if any players added
        /// </summary>
        public bool HasPlayers
        {
            get
            {
                if (Players != null && Players.Count > 0)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// If players count less then 4 we can add one more player
        /// </summary>
        public bool CanAddPlayer
        {
            get
            {
                if (Players != null && Players.Count <4)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// If players count less then 4 we can add one more player
        /// </summary>
        public bool CanDeletePlayer
        {
            get
            {
                if (IsPlayerSelected && Players.Count >1)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Conditions to start game
        /// </summary>
        public override bool IsReadyToPlay
        {
            get 
            {
                return (Players.Count > 0 && SelectedRule != null);
                
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// fills players list
        /// </summary>
        protected override async void  FillPlayers()
        {
            if (Players != null)
            {
                foreach (var p in Players)
                {
                    p.Dispose();
                    
                }
            }
            Players = new ObservableCollection<PlayerWrapper>();
            //trying toload previous players from roaming
            for (int i = 0; i < 4; i++)
            {
                var p=RoamingSettings.GetLastPlayer(i);
                if (p==null||p.Player == null)
                    break;
                p.DeletePressed += p_DeletePressed;
                p.MagicPressed += p_MagicPressed;
                p.NameClicked += p_NameClicked;
                p.PassClicked += p_PassClicked;
                p.ArtifactsSyncRequest+=ArtifactsSyncRequest;
                p.RefreshArtifactsInfo(false,true);
                Players.Add(p);
            }
            //if no players loaded - add one default
            if (!HasPlayers && CanAddPlayer )
            {

                //get username from system
                string userName =
#if WinRT
                    await UserInformation.GetDisplayNameAsync();
                if (string.IsNullOrEmpty(userName))
                    userName = await UserInformation.GetFirstNameAsync() + await UserInformation.GetFirstNameAsync();
#endif
#if WINDOWS_PHONE
                WPIdentifyHelpers.GetWindowsLiveAnonymousID();
#endif
#if VK
                    "";
#endif
                //if no luck - add default name
                if (string.IsNullOrEmpty(userName))
                    userName = GetNewPlayerName(PlayerType.Local);
                var p = new PlayerWrapper(new Player())
                    {
                        Name = userName,
                        Type = PlayerType.Local
                    };
                p.DeletePressed += p_DeletePressed;
                p.MagicPressed += p_MagicPressed;
                p.NameClicked += p_NameClicked;
                p.PassClicked += p_PassClicked;
                p.ArtifactsSyncRequest += ArtifactsSyncRequest;
                p.RefreshArtifactsInfo();
                Players.Add(p);
            }
            SelectedPlayer = Players.Last();
            NotifyPlayersChanged();
            
        }

        void p_PassClicked(object sender, EventArgs e)
        {
            var p = (PlayerWrapper)sender;
            ChangeUserPass(p);
        }

        void p_NameClicked(object sender, EventArgs e)
        {
            var p = (PlayerWrapper)sender;
            ChangeUserName(p);
        }
               

        /// <summary>
        /// Add new player or bot
        /// </summary>
        /// <param name="type"></param>
        public void AddPlayer(PlayerType type)
        {
            if (CanAddPlayer)
            {
                //get username from system
                var p=new PlayerWrapper(new Player());
                p.DeletePressed += p_DeletePressed;
                p.MagicPressed += p_MagicPressed;
                p.NameClicked += p_NameClicked;
                p.PassClicked += p_PassClicked;
                p.ArtifactsSyncRequest += ArtifactsSyncRequest;
                Players.Add(p);
                NotifyPlayersChanged();
                p.Name = GetNewPlayerName(type);
                p.Type = type;
                SelectedPlayer = p;
            }
            NotifyPropertyChanged("CanAddPlayer");
            
        }

        /// <summary>
        /// Looks  for the free default player name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override string GetNewPlayerName(PlayerType type)
        {
            string defName = (type == PlayerType.AI) ? Messages.PLAYER_BOTNAME_DEFAULT.Localize() : Messages.PLAYER_NAME_DEFAULT.Localize();
            string userName;
            int index = 1;
            do
            {
                userName = string.Format("{0} {1}", defName, index);
                index++;
            }
            while (Players.FirstOrDefault(f => f.Name == userName) != null);
            return userName;
        }

        /// <summary>
        /// Update UI according to players state
        /// </summary>
        protected override void NotifyPlayersChanged()
        {
            NotifyPropertyChanged("Players");
            NotifyPropertyChanged("CanAddPlayer");
            NotifyPropertyChanged("CanDeletePlayer");
            NotifyPropertyChanged("IsReadyToPlay");
            foreach (var p in Players)
            {
                p.IsDeleteable = Players.Count > 1;
                //p.UpdateType();
            }
            
        }
        /// <summary>
        /// Delete selected player from list
        /// </summary>
        public void DeletePlayer()
        {
            if (IsPlayerSelected && CanDeletePlayer)
            {
                Players.Remove(SelectedPlayer);
                SelectedPlayer =Players.Last();
                NotifyPlayersChanged();
            }
            NotifyPropertyChanged("CanDeletePlayer");
        }
        void p_DeletePressed(object sender, EventArgs e)
        {
            SelectedPlayer = (PlayerWrapper)sender;
            SelectedPlayer.DeletePressed -= p_DeletePressed;
            DeletePlayer();
            
        }
        /// <summary>
        /// Saves settings to roaming
        /// </summary>
        public override void SavePlayers()
        {
            int index = 0;
            //save players in list
            foreach (PlayerWrapper player in Players)
            {
                RoamingSettings.SaveLastPlayer(player.Player, index);
                index++;
            }
            //save nulls for players if less then (in case if were deleted)
            for(;index<4;index++)
                RoamingSettings.SaveLastPlayer(null, index);
            if (SelectedRule != null)
                RoamingSettings.LastRule = SelectedRule.Rule.Rule;
        }

        public override async void StartGame()
        {
            if (HasPlayers && SelectedRule != null)
            {
                foreach (var p in Players)
                    p.SelectedStyle = RoamingSettings.DiceStyle;

                var gameModel = ViewModelProvider.GetViewModel<PlayGameViewModel>();
                gameModel.Game = new KniffelGame();
                gameModel.Game.Rules = SelectedRule.Rule;
                gameModel.RollResults = null;
                foreach (PlayerWrapper player in Players)
                {
                    //notify user that he haven't artifacts
                    if (SelectedRule.Rule.Rule == Models.Rules.krMagic)
                    {
                        if (!player.HasArtifacts && player.IsHuman)
                        {
#if WinRT
                            var msg = new MessageDialog(string.Format("NoArtifactsMessage".Localize(),player.Name), "NoArtifactsLabel".Localize());

                            // Add buttons and set their command handlers
                            msg.Commands.Add(new UICommand("MoreLabel".Localize()));
                            msg.Commands.Add(new UICommand("ContinueLabel".Localize()));

                            // Show the message dialog
                            IUICommand res =await msg.ShowAsync();
                            if (res.Label == "MoreLabel".Localize())
                            {
                                p_MagicPressed(player, null);
                                return;
                            }
#endif
                        }
                    }
                    //player.Roll = 1;
                    player.IsReady = true;
                    gameModel.Game.JoinGame(player.Player);
                }

                CommonNavigationActions.NavigateToGamePage();
            }
        }

        #endregion

        #region Commands
        public RelayCommand AddPlayerCommand { get; set; }
        public RelayCommand AddBotCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand StartCommand { get; set; }
        
        protected void CreateCommands()
        {
            AddPlayerCommand = new RelayCommand(o => AddPlayer(PlayerType.Local), () => true);
            AddBotCommand = new RelayCommand(o => AddPlayer(PlayerType.AI), () => true);
            DeleteCommand = new RelayCommand(o => DeletePlayer(), () => true);
            StartCommand = new RelayCommand(o => StartGame(), () => true);
        }



        #endregion


    }
}
