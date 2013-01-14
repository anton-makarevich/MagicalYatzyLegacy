using DicePokerRT;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sanet.Kniffel.Models
{
    public static  class CommonNavigationActions
    {
        public static Action NavigateToMainPage
        {
            get
            {
                return new Action(()=>
                    {
                        ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
                });
            }
            
        }
        public static Action NavigateToNewGamePage
        {
            get
            {
                return new Action(() =>
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(NewGamePage));
                });
            }

        }
        public static Action NavigateToLeaderboardPage
        {
            get
            {
                return new Action(() =>
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(LeaderboardPage));
                });
            }

        }
        public static Action NavigateToGamePage
        {
            get
            {
                return new Action(() =>
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(GamePage));
                });
            }

        }
        public static Action NavigateToSettingsPage
        {
            get
            {
                return new Action(() =>
                {
                    if (App.Settings == null)
                    {

                        App.Settings = new TaskPanePopup(new SettingsPage());
                    }
                    App.Settings.Show();
                });
            }

        }
    }
}
