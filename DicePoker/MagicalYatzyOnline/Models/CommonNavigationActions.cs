using DicePokerRT;
#if ONLINE
using MagicalYatzyOnline;
#endif
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sanet.Views;

namespace Sanet.Kniffel.Models
{
    public static  class CommonNavigationActions
    {
        public static Action NavigateToMainPage
        {
            get
            {
                return new Action(async()=>
                    {
                        SoundsProvider.PlaySound("click");

                        ((BasePage)Window.Current.Content).OnNavigatedFrom();
                        var page = ViewsProvider.GetPage < MainPage>();
                        Window.Current.Content = page;
                        Window.Current.Activate();
                        page.OnNavigatedTo();
                        if (ReviewBugger.IsTimeForReview())
                            await ReviewBugger.PromptUser();
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

                    ((BasePage)Window.Current.Content).OnNavigatedFrom();
                    var page = ViewsProvider.GetPage<NewGamePage>();
                    Window.Current.Content = page;
                    Window.Current.Activate();
                    page.OnNavigatedTo();
                    
                });
            }

        }
#if ONLINE
        public static Action NavigateToNewOnlineGamePage
        {
            get
            {
                return new Action(() =>
                {
                    SoundsProvider.PlaySound("click");

                    ((BasePage)Window.Current.Content).OnNavigatedFrom();
                    var page =ViewsProvider.GetPage<NewOnlineGamePage>();
                    Window.Current.Content = page;
                    Window.Current.Activate();
                    page.OnNavigatedTo();
                    
                });
            }

        }
#endif
        public static Action NavigateToAboutPage
        {
            get
            {
                return new Action(() =>
                {
                    SoundsProvider.PlaySound("click");

                    ((BasePage)Window.Current.Content).OnNavigatedFrom();
                    var page = ViewsProvider.GetPage<AboutPage>();
                    Window.Current.Content = page;
                    Window.Current.Activate();
                    page.OnNavigatedTo();
                    
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

                    ((BasePage)Window.Current.Content).OnNavigatedFrom();
                    var page = ViewsProvider.GetPage < LeaderboardPage>();
                    Window.Current.Content = page;
                    Window.Current.Activate();
                    page.OnNavigatedTo();
                   
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

                    ((BasePage)Window.Current.Content).OnNavigatedFrom();
                    var page = ViewsProvider.GetPage < GamePage>();
                    Window.Current.Content = page;
                    Window.Current.Activate();
                    page.OnNavigatedTo();
                    
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
                    ReviewBugger.DidReview();
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
        public static Action NavigateYatzyFBPage
        {
            get
            {
                return new Action(async () =>
                {
                    SoundsProvider.PlaySound("click");
                    await Launcher.LaunchUriAsync(new Uri("http://www.facebook.com/MagicalYatzy"));
                });
            }
        }
        public static Action NavigateYatzyVKPage
        {
            get
            {
                return new Action(async () =>
                {
                    SoundsProvider.PlaySound("click");
                    await Launcher.LaunchUriAsync(new Uri("http://www.vk.com/MagicalYatzy"));
                });
            }
        }
    }
}
