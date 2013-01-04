
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Settings;
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
    public class NewGameViewModel:BaseViewModel
    {
        #region Constructor
        public NewGameViewModel()
        {
            CreateCommands();
            fillPlayers();
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
        /// Page title
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
        /// Players list
        /// </summary>
        private ObservableCollection<Player> _Players;
        public ObservableCollection<Player> Players
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

        #endregion

        #region Methods
        /// <summary>
        /// fills players list
        /// </summary>
        async void  fillPlayers()
        {
            Players = new ObservableCollection<Player>();
            //trying toload previous players from roaming
            for (int i = 0; i < 4; i++)
            {
                var p=RoamingSettings.GetLastPlayer(i);
                if (p == null)
                    break;
                Players.Add(p);
            }
            //if no players loaded - add one default
            if (!HasPlayers && CanAddPlayer )
            {
                //get username from system
                string userName = await UserInformation.GetDisplayNameAsync();
                if (string.IsNullOrEmpty(userName))
                    userName = await UserInformation.GetFirstNameAsync() + await UserInformation.GetFirstNameAsync();
                //if no luck - add default name
                if (string.IsNullOrEmpty(userName))
                    userName = GetNewPlayerName(PlayerType.Local);
                Players.Add(new Player()
                    {
                        Name = userName,
                        Type = PlayerType.Local
                    });
            }  
            NotifyPropertyChanged("Players");
        }
        /// <summary>
        /// Add new player or bot
        /// </summary>
        /// <param name="type"></param>
        void AddPlayer(PlayerType type)
        {
            if (CanAddPlayer)
            {
                //get username from system
                Players.Add(new Player()
                {
                    Name = GetNewPlayerName(type),
                    Type = type
                });
                NotifyPropertyChanged("Players");
            } 
            
        }
        /// <summary>
        /// Looks  for the free default player name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetNewPlayerName(PlayerType type)
        {
            string defName =(type== PlayerType.AI)?Messages.PLAYER_BOTNAME_DEFAULT.Localize():Messages.PLAYER_NAME_DEFAULT.Localize();
            string userName;
            int index = 1;
            do
            {
                userName= string.Format("{0} {1}",defName, index);
                index++;
            }
            while (Players.FirstOrDefault(f => f.Name == userName) != null);
            return userName;
        }

        #endregion

        #region Commands
        public RelayCommand AddPlayerCommand { get; set; }
        public RelayCommand AddBotCommand { get; set; }
        
        protected void CreateCommands()
        {
            AddPlayerCommand = new RelayCommand(o => AddPlayer(PlayerType.Local), () => true);
            AddBotCommand = new RelayCommand(o => AddPlayer(PlayerType.AI), () => true);
            
        }



        #endregion


    }
}
