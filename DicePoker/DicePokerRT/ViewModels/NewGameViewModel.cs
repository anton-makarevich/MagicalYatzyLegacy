
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerRT.ViewModels
{
    public class NewGameViewModel:BaseViewModel
    {
        public string Title
        {
            get
            {
                return Messages.NEW_GAME_START.Localize();
            }
        }

        
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

    }
}
