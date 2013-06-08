using Microsoft.Phone.Tasks;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sanet.Kniffel.Models
{
    public static  class CommonNavigationActions
    {
        #region events
        public static event Action OnNavigationToAbout;
        public static event Action OnNavigationToLeaderboard;
        public static event Action OnNavigationToSettings;
        public static event Action OnNavigationToNewGame;
        public static event Action OnNavigationToGame;
        #endregion


        public static Action NavigateToMainPage
        {
            get
            {
                return new Action(()=>
                    {
                        //((Frame)Window.Current.Content).Navigate(typeof(MainPage));
                });
            }
            
        }
        public static Action NavigateToNewGamePage
        {
            get
            {
                return new Action(() =>
                {
                    if (OnNavigationToNewGame != null)
                        OnNavigationToNewGame();
                });
            }

        }
        public static Action NavigateToNewOnlineGamePage
        {
            get
            {
                return new Action(() =>
                {
                    //((Frame)Window.Current.Content).Navigate(typeof(NewGamePage));
                });
            }

        }
        public static Action NavigateToAboutPage
        {
            get
            {
                return new Action(() =>
                {
                    if (OnNavigationToAbout != null)
                        OnNavigationToAbout();
                });
            }

        }
        public static Action ShareApp
        {
            get
            {
                return new Action(() =>
                {
                    ShareLinkTask shareStatusTask = new ShareLinkTask();
                    shareStatusTask.LinkUri = new Uri("http://windowsphone.com/s?appid=f2993622-c41f-4cd5-8188-403a3efe6383");
                    shareStatusTask.Title = "AppNameLabel".Localize();
                    shareStatusTask.Message = "ShareMessage".Localize();
                    shareStatusTask.Show();
                });
            }

        }
        public static Action NavigateToLeaderboardPage
        {
            get
            {
                return new Action(() =>
                {
                    if (OnNavigationToLeaderboard != null)
                        OnNavigationToLeaderboard();
                });
            }

        }
        public static Action NavigateToGamePage
        {
            get
            {
                return new Action(() =>
                {
                    if (OnNavigationToGame != null)
                        OnNavigationToGame();
                });
            }

        }
        public static Action NavigateToSettingsPage
        {
            get
            {
                return new Action(() =>
                {
                    if (OnNavigationToSettings != null)
                        OnNavigationToSettings();
                });
            }

        }
        public static Action SendFeedback
        {
            get
            {
                return new  Action(() =>
                {
                    EmailComposeTask emailComposeTask = new EmailComposeTask();

                    emailComposeTask.Subject = "Magical Yatzy WP7";
                    emailComposeTask.To = "support@sanet.by";
                    
                    emailComposeTask.Show();
                });
            }

        }
        public static Action RateApp
        {
            get
            {
                return new Action( () =>
                {
                    MarketplaceReviewTask _marketPlaceReviewTask = new MarketplaceReviewTask();
                    _marketPlaceReviewTask.Show();
                });
            }

        }
        public static Action NavigateToSanetNews
        {
            get
            {
                return new Action( () =>
                {
                    //await Launcher.LaunchUriAsync(new Uri("http://apps.microsoft.com/windows/app/sanet-news/1b98de81-1b9a-4ee2-a266-aa3bc336f507"));
                });
            }

        }
        public static Action NavigateToSanetAllWrite
        {
            get
            {
                return new Action(() =>
                {
                    var task = new Microsoft.Phone.Tasks.WebBrowserTask
                    {
                        Uri = new Uri("http://windowsphone.com/s?appid=b588316a-e1a2-4e84-91d5-a773850a915d")
                    };

                    task.Show();
                });
            }
        }
        public static Action NavigateToSanetDice
        {
            get
            {
                return new Action(() =>
                {
                    var task = new Microsoft.Phone.Tasks.WebBrowserTask
                    {
                        Uri = new Uri("http://windowsphone.com/s?appid=bc105e9d-f31e-4c1d-89d2-6f5d8bd877e6")
                    };

                    task.Show();
                });
            }
        }
        public static Action NavigateYatzyFBPage
        {
            get
            {
                return new Action( () =>
                {
                    SoundsProvider.PlaySound("click");
                    var task = new Microsoft.Phone.Tasks.WebBrowserTask
                    {
                        Uri = new Uri("http://www.facebook.com/MagicalYatzy")
                    };

                    task.Show();
                    
                });
            }
        }
        public static Action NavigateYatzyVKPage
        {
            get
            {
                return new Action( () =>
                {
                    SoundsProvider.PlaySound("click");
                    //await Launcher.LaunchUriAsync(new Uri("http://www.vk.com/MagicalYatzy"));
                });
            }
        }
    }
}
