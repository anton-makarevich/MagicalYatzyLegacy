﻿using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;

#if WinRT
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
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
            DataTransferManager.GetForCurrentView().DataRequested += MainPageViewModel_DataRequested;
            FillMainActions();
            FillSecondaryActions();
        }

        void MainPageViewModel_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataPackage requestData = args.Request.Data;
            
            requestData.Properties.Title = "ShareTitle".Localize();
            //requestData.Properties.Description = "ShareDescription".Localize(); // The description is optional.
            //requestData.SetText("ShareDescription".Localize());
            requestData.SetUri(new Uri("http://apps.microsoft.com/windows/app/sanet-dice-poker/5b0f9106-65a8-49ca-b1f0-641c54a7e3ef"));
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

        private List<AboutAppAction> _SecondaryMenuActions;
        public List<AboutAppAction> SecondaryMenuActions
        {
            get { return _SecondaryMenuActions; }
            set
            {
                if (_SecondaryMenuActions != value)
                {
                    _SecondaryMenuActions = value;
                    NotifyPropertyChanged("SecondaryMenuActions");
                }
            }
        }
        
#endregion

        #region Methods
        public void FillMainActions()
        {
            MenuActions = new List<MainMenuAction>();
            MenuActions.Add(
                new MainMenuAction
                {
                    Label = "NewLocalGameAction",
                    MenuAction = new Action(() =>
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
        public void FillSecondaryActions()
        {
            _SecondaryMenuActions = new List<AboutAppAction>();
            _SecondaryMenuActions.Add(
                new AboutAppAction
                {
                    Label = "SendFeedbackAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.SendFeedback();
                    }),
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/Mail.png", UriKind.Absolute))
                });
            _SecondaryMenuActions.Add(
                new AboutAppAction
                {
                    Label = "ReviewAppAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.RateApp();
                    }),
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/Rate.png", UriKind.Absolute))
                });
            _SecondaryMenuActions.Add(
                new AboutAppAction
                {
                    Label = "ShareApp",
                    MenuAction = new Action(() =>
                    {
                        DataTransferManager.ShowShareUI(); 
                    }),
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/Share.png", UriKind.Absolute))
                });
            if (StoreManager.IsAdVisible())
                _SecondaryMenuActions.Add(
                    new AboutAppAction
                    {
                        Label = "RemoveAdAction",
                        MenuAction = new Action(async () =>
                        {
                            await StoreManager.RemoveAd();
                            ViewModelProvider.GetViewModel<AboutPageViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<AboutPageViewModel>().FillAppActions();
                            ViewModelProvider.GetViewModel<SettingsViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<SettingsViewModel>().FillAppActions();
                            ViewModelProvider.GetViewModel<MainPageViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<MainPageViewModel>().FillSecondaryActions();
                        }),
                        Image = new BitmapImage(new Uri("ms-appx:///Assets/Unlock.png", UriKind.Absolute))
                    });
            NotifyPropertyChanged("SecondaryMenuActions");
        }
        #endregion

    }
}
