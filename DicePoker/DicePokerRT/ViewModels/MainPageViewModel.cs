using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;

#if WinRT
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Media.Imaging;
#endif

namespace Sanet.Kniffel.ViewModels
{
    public class MainPageViewModel:AdBasedViewModel
    {

#region Constructor
        public MainPageViewModel()
        {
            MenuActions = new List<MainMenuAction>();
            MenuActions.Add(
                new MainMenuAction
                {
                    Label="NewLocalGameAction",
                    MenuAction=new Action(()=>
                        {
                            CommonNavigationActions.NavigateToNewGamePage();
                        }),
                    Description = "NewLocalGameDescription",
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("SanetDice.png")),
                });
            //MenuActions.Add(
            //    new MainMenuAction
            //    {
            //        Label = "NewOnlineGameAction",
            //        MenuAction = new Action(() =>
            //        {
            //            Utilities.ShowToastNotification(Messages.NETWORK_GAME_IS_NOT_READY.Localize());
            //        }),
            //        Description = "NewOnlineGameDescription"
            //    });
            MenuActions.Add(
                new MainMenuAction
                {
                    Label = "SettingsAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToSettingsPage();
                    }),
                    Description = "SettingsDescription",
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Settings.png")),
                });
            MenuActions.Add(
                new MainMenuAction
                {
                    Label = "LeaderboardAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToLeaderboardPage();
                    }),
                    Description = "LeaderboardDescription",
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Victory.png")),
                });
            MenuActions.Add(
                new MainMenuAction
                {
                    Label = "AboutAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToAboutPage();
                    }),
                    Description = "AboutDescription",
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("About.png")),
                });
            NotifyPropertyChanged("MenuActions");
        }

#endregion
#region Properties
        /// <summary>
        /// app name label
        /// </summary>
        public string CurrentAppName
        {
            get
            {
                return Messages.APP_NAME.Localize();
            }
        }
        public string CurrentAppNameUpper
        {
            get
            {
                return Messages.APP_NAME.Localize().ToUpper();
            }
        }

        private List<MainMenuAction> _MenuActions;
        public List<MainMenuAction> MenuActions
        {
            get { return _MenuActions; }
            set
            {
                if (_MenuActions != value)
                {
                    _MenuActions = value;
                    NotifyPropertyChanged("MenuActions");
                }
            }
        }


        
#endregion

    }
}
