using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;

namespace Sanet.Kniffel.ViewModels
{
    public class MainPageViewModel:BaseViewModel
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
                    Description = "NewLocalGameDescription"
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
                    Description = "SettingsDescription"
                });
            MenuActions.Add(
                new MainMenuAction
                {
                    Label = "LeaderboardAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToLeaderboardPage();
                    }),
                    Description = "LeaderboardDescription"
                });
            MenuActions.Add(
                new MainMenuAction
                {
                    Label = "AboutAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToAboutPage();
                    }),
                    Description = "AboutDescription"
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
