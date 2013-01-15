using DicePokerRT;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
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
        public static Action NavigateToAboutPage
        {
            get
            {
                return new Action(() =>
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(AboutPage));
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
        public static Action SendFeedback
        {
            get
            {
                return new  Action(async() =>
                {
                    await Launcher.LaunchUriAsync(new Uri("mailto:support@sanet.by"));
                });
            }

        }
        public static Action RateApp
        {
            get
            {
                return new Action(async () =>
                {
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=43862AntonMakarevich.SanetDicePoker_2wtrjzrdj31kc"));
                });
            }

        }
        public static Action NavigateToSanetNews
        {
            get
            {
                return new Action(async () =>
                {
                    await Launcher.LaunchUriAsync(new Uri("http://apps.microsoft.com/windows/app/sanet-news/1b98de81-1b9a-4ee2-a266-aa3bc336f507"));
                });
            }

        }
        public static Action NavigateToSanetAllWrite
        {
            get
            {
                return new Action(async () =>
                {
                    await Launcher.LaunchUriAsync(new Uri("http://apps.microsoft.com/windows/app/sanet-allwrite/022fd522-54cf-4acd-9341-eab7c2185218"));
                });
            }
        }
    }
}
