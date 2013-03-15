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
                        SoundsProvider.PlaySound("click");
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
                    SoundsProvider.PlaySound("click");
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
                    SoundsProvider.PlaySound("click");
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
                    SoundsProvider.PlaySound("click");
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
                    SoundsProvider.PlaySound("click");
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
                    SoundsProvider.PlaySound("click");
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
                    SoundsProvider.PlaySound("click");
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
                    SoundsProvider.PlaySound("click");
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
                    SoundsProvider.PlaySound("click");
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store:PDP?PFN=43862AntonMakarevich.SanetNews_2wtrjzrdj31kc"));
                });
            }

        }
        public static Action NavigateToSanetAllWrite
        {
            get
            {
                return new Action(async () =>
                {
                    SoundsProvider.PlaySound("click");
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store:PDP?PFN=43862AntonMakarevich.SanetAllWrite_2wtrjzrdj31kc"));
                });
            }
        }
    }
}
