

using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WinRT
using Windows.ApplicationModel;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Media.Imaging;
using DicePokerRT.KniffelLeaderBoardService;
#else
using System.Windows.Media.Imaging;
#endif

namespace Sanet.Kniffel.ViewModels
{
    public class AboutPageViewModel:AdBasedViewModel
    {
        #region Constructor
        public AboutPageViewModel()
        {
            FillAppActions();
            FillOtherApps();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Page title
        /// </summary>
        public string Title
        {
            get
            {
                return "AboutAction".Localize();
            }
        }

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
        /// <summary>
        /// current app label
        /// </summary>
        public string OtherApps
        {
            get
            {
                return Messages.APP_NAME_OTHER.Localize();
            }
        }

        public string MetroStudioText
        {
            get
            {
                return "MetroStudioText".Localize();
            }
        }

        /// <summary>
        /// Display current package version
        /// </summary>  
        public string VersionLabel
        {
            get
            {
#if WinRT
                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;
                return "Version".Localize()+": " + version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
#else
                return "na";
#endif
            }
        }
        /// <summary>
        /// current app label
        /// </summary>
        public string DevelopedByLabel
        {
            get
            {
                return "DevelopedBy/Text".Localize()+" Sanet Soft";
            }
        }
        
        private List<AboutAppAction> _AboutAppActions;
        public List<AboutAppAction> AboutAppActions
        {
            get { return _AboutAppActions; }
            set
            {
                if (_AboutAppActions != value)
                {
                    _AboutAppActions = value;
                    NotifyPropertyChanged("AboutAppActions");
                }
            }
        }

        private List<MainMenuAction> _OtherAppActions;
        public List<MainMenuAction> OtherAppActions
        {
            get { return _OtherAppActions; }
            set
            {
                if (_OtherAppActions != value)
                {
                    _OtherAppActions = value;
                    NotifyPropertyChanged("OtherAppActions");
                }
            }
        }

        #endregion

        #region Methods

        public void FillAppActions()
        {
            _AboutAppActions = new List<AboutAppAction>();
            _AboutAppActions.Add(
                new AboutAppAction
                {
                    Label = "SendFeedbackAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.SendFeedback();
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Mail.png"))
                });
            _AboutAppActions.Add(
                new AboutAppAction
                {
                    Label = "ReviewAppAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.RateApp();
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Rate.png"))
                });
            if (StoreManager.IsAdVisible())
            _AboutAppActions.Add(
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
            NotifyPropertyChanged("AboutAppActions");
        }

        private void FillOtherApps()
        {
            OtherAppActions = new List<MainMenuAction>();
            _OtherAppActions.Add(
                new MainMenuAction
                {
                    Label = "SANET ALLWRITE",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToSanetAllWrite();
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("AWLogo.png")),
                    Description = "AWDescription"
                });
            _OtherAppActions.Add(
                new MainMenuAction
                {
                    Label = "SANET NEWS",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToSanetNews();
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("NewsLogo.png")),
                    Description="NewsDescription"
                });

            NotifyPropertyChanged("OtherAppActions");
        }

        #endregion

        #region Commands
        //public RelayCommand AddPlayerCommand { get; set; }
        
        
        //protected void CreateCommands()
        //{
        //    AddPlayerCommand = new RelayCommand(o => AddPlayer(PlayerType.Local), () => true);
            
        //}



        #endregion


    }
}
