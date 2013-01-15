
using DicePokerRT.KniffelLeaderBoardService;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Media.Imaging;

namespace Sanet.Kniffel.ViewModels
{
    public class AboutPageViewModel:BaseViewModel
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
        /// <summary>
        /// Display current package version
        /// </summary>  
        public string VersionLabel
        {
            get
            {
                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;
                return "Version".Localize()+": " + version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
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

        private List<OtherAppAction> _OtherAppActions;
        public List<OtherAppAction> OtherAppActions
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

        private void FillAppActions()
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
                    Image = new BitmapImage(new Uri( "ms-appx:///Assets/Mail.png", UriKind.Absolute))
                });
            _AboutAppActions.Add(
                new AboutAppAction
                {
                    Label = "ReviewAppAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.SendFeedback();
                    }),
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/Rate.png", UriKind.Absolute))
                });
            _AboutAppActions.Add(
                new AboutAppAction
                {
                    Label = "RemoveAdAction",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.SendFeedback();
                    }),
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/Unlock.png", UriKind.Absolute))
                });
            NotifyPropertyChanged("AboutAppActions");
        }

        private void FillOtherApps()
        {
            OtherAppActions = new List<OtherAppAction>();
            _OtherAppActions.Add(
                new OtherAppAction
                {
                    Label = "SANET ALLWRITE",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToSanetAllWrite();
                    }),
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/AWLogo.png", UriKind.Absolute)),
                    Description = "AWDescription"
                });
            _OtherAppActions.Add(
                new OtherAppAction
                {
                    Label = "SANET NEWS",
                    MenuAction = new Action(() =>
                    {
                        CommonNavigationActions.NavigateToSanetNews();
                    }),
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/NewsLogo.png", UriKind.Absolute)),
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
