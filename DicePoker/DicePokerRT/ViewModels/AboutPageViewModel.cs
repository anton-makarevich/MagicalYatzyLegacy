

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
using System.Reflection;
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

        public string OnlineVersionText
        {
            get
            {
                return "OnlineVersionText".Localize();
            }
        }

        public string ForW8Label
        {
            get
            {
                return "ForWin8Label".Localize();
            }
        }
        public string ForWPLabel
        {
            get
            {
                return "ForWPLabel".Localize();
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
                var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
                return  "Version".Localize() + ": " +nameHelper.Version.ToString();
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
                return "DevelopedByText".Localize()+" Sanet Soft";
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
#if !VK
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
#endif
            if (StoreManager.IsAdVisible())
            _AboutAppActions.Add(
                new AboutAppAction
                {
                    Label = "RemoveAdAction",
                    MenuAction = new Action(
#if Win8
                        async
#endif   
                            () =>
                    {
#if Win8
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
#if WinRT
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
#else
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
                    Label = "SANET DICE",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToSanetDice();
                    }),
                    Image = new BitmapImage(SanetImageProvider.GetAssetsImage("SanetDice.png")),
                    Description = "DiceDescription"
                });
#endif
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
