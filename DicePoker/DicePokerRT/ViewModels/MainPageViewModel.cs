using Sanet.Kniffel.Models;
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
#if WinRT
            DataTransferManager.GetForCurrentView().DataRequested += MainPageViewModel_DataRequested;
#endif
            FillMainActions();
            FillSecondaryActions();
        }
#if WinRT
        void MainPageViewModel_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataPackage requestData = args.Request.Data;
            
            requestData.Properties.Title = "ShareTitle".Localize();
            //requestData.Properties.Description = "ShareDescription".Localize(); // The description is optional.
            //requestData.SetText("ShareDescription".Localize());
            requestData.SetUri(new Uri("http://apps.microsoft.com/windows/app/sanet-dice-poker/5b0f9106-65a8-49ca-b1f0-641c54a7e3ef"));
        }
#endif
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
#if ONLINE
            MenuActions.Add(
                new MainMenuAction
                {
                    Label = "NewOnlineGameAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToNewOnlineGamePage();
                    }),
                    Description = "NewOnlineGameDescription",
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("OnlineGame.png")),
                });
#endif
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
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Mail.png"))
                });
            _SecondaryMenuActions.Add(
                new AboutAppAction
                {
                    Label = "ReviewAppAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.RateApp();
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Rate.png"))
                });
            _SecondaryMenuActions.Add(
                new AboutAppAction
                {
                    Label = "FBPage",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateYatzyFBPage(); 
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("facebook.png"))
                });
            _SecondaryMenuActions.Add(
                new AboutAppAction
                {
                    Label = "FKPage",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateYatzyVKPage();
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("vk.png"))
                });
            _SecondaryMenuActions.Add(
                new AboutAppAction
                {
                    Label = "ShareApp",
                    MenuAction = new Action(() =>
                    {
#if WinRT
                        DataTransferManager.ShowShareUI();
#endif
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Share.png"))
                });
            if (StoreManager.IsAdVisible())
                _SecondaryMenuActions.Add(
                    new AboutAppAction
                    {
                        Label = "RemoveAdAction",
                        MenuAction = new Action(
#if WinRT
                            async
#endif
                                () =>
                        {
#if WinRT
                            await
#endif
                            StoreManager.RemoveAd();
                            ViewModelProvider.GetViewModel<AboutPageViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<AboutPageViewModel>().FillAppActions();
                            ViewModelProvider.GetViewModel<SettingsViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<SettingsViewModel>().FillAppActions();
                            ViewModelProvider.GetViewModel<MainPageViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<MainPageViewModel>().FillSecondaryActions();
                        }),
                        Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Unlock.png"))
                    });
            NotifyPropertyChanged("SecondaryMenuActions");
        }
        #endregion

    }
}
