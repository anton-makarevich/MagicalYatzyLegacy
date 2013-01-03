
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Settings;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.ViewModels
{
    public class NewGameViewModel:BaseViewModel
    {
        #region Constructor
        public NewGameViewModel()
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
            if (!HasPlayers)
                for (int i = 1; i < 5; i++)
                {
                    Players.Add(new Player()
                    {
                        Name = "Player "+i.ToString(),
                        Type = PlayerType.Local
                    });
                }
               
            NotifyPropertyChanged("Players");
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
        #endregion
    }
}
