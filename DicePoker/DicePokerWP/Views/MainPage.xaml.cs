using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Sanet.Models;
using Sanet.Kniffel.ViewModels;
using Microsoft.Phone.Shell;
using Sanet.Kniffel.Models;

namespace DicePokerWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        //main appbar buttons
        ApplicationBarIconButton fbButton;
        ApplicationBarIconButton rateButton;
        ApplicationBarIconButton shareButton;
        ApplicationBarIconButton buyButton;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;

            //creating appbar menu elements
            //AppBar
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 0.85;
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 156, 214);
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = false;

            fbButton = new ApplicationBarIconButton();
            fbButton.IconUri = new Uri("/Assets/Facebook.png", UriKind.Relative);
            fbButton.Text = "FacebookLabel".Localize();
            fbButton.Click += fbButton_Click;


            rateButton = new ApplicationBarIconButton();
            rateButton.IconUri = new Uri("/Assets/appRate.png", UriKind.Relative);
            rateButton.Text = "RateLabel".Localize();
            rateButton.Click += rateButton_Click;

            shareButton = new ApplicationBarIconButton();
            shareButton.IconUri = new Uri("/Assets/share.png", UriKind.Relative);
            shareButton.Text = "ShareLabel".Localize();
            shareButton.Click += shareButton_Click;

            buyButton = new ApplicationBarIconButton();
            buyButton.IconUri = new Uri("/Assets/appUnlock.png", UriKind.Relative);
            buyButton.Text = "BuyLabel".Localize();
            buyButton.Click += buyButton_Click;

            
        }

        void DettachNavigationEvents()
        {
            CommonNavigationActions.OnNavigationToAbout -= CommonNavigationActions_OnNavigationToAbout;
            CommonNavigationActions.OnNavigationToLeaderboard -= CommonNavigationActions_OnNavigationToLeaderboard;
            CommonNavigationActions.OnNavigationToSettings -= CommonNavigationActions_OnNavigationToSettings;
            CommonNavigationActions.OnNavigationToNewGame -= CommonNavigationActions_OnNavigationToNewGame;
            CommonNavigationActions.OnNavigationToOnlineGame -= CommonNavigationActions_OnNavigationToOnlineGame;
        }

        void AttachNavigationEvents()
        {
            CommonNavigationActions.OnNavigationToAbout += CommonNavigationActions_OnNavigationToAbout;
            CommonNavigationActions.OnNavigationToLeaderboard += CommonNavigationActions_OnNavigationToLeaderboard;
            CommonNavigationActions.OnNavigationToSettings += CommonNavigationActions_OnNavigationToSettings;
            CommonNavigationActions.OnNavigationToNewGame += CommonNavigationActions_OnNavigationToNewGame;
            CommonNavigationActions.OnNavigationToOnlineGame += CommonNavigationActions_OnNavigationToOnlineGame;
        }

        void CommonNavigationActions_OnNavigationToOnlineGame()
        {
            NavigationService.Navigate(new Uri("/Views/NewOnlineGamePage.xaml", UriKind.RelativeOrAbsolute));
        }
        
        void CommonNavigationActions_OnNavigationToNewGame()
        {
            NavigationService.Navigate(new Uri("/Views/NewGamePage.xaml", UriKind.RelativeOrAbsolute));
        }

        void CommonNavigationActions_OnNavigationToLeaderboard()
        {
            NavigationService.Navigate(new Uri("/Views/LeaderboardPage.xaml", UriKind.RelativeOrAbsolute));
        }

        void CommonNavigationActions_OnNavigationToAbout()
        {
            NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.RelativeOrAbsolute));
        }

        void CommonNavigationActions_OnNavigationToSettings()
        {
            NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        void buyButton_Click(object sender, EventArgs e)
        {
            StoreManager.RemoveAd();
        }

        void shareButton_Click(object sender, EventArgs e)
        {
            CommonNavigationActions.ShareApp();
        }

        void rateButton_Click(object sender, EventArgs e)
        {
            CommonNavigationActions.RateApp();
        }

        void fbButton_Click(object sender, EventArgs e)
        {
            CommonNavigationActions.NavigateYatzyFBPage();
        }

        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Init smartdispatcher
            SmartDispatcher.Initialize(this.Dispatcher);
            //dpBackground = new Sanet.Kniffel.DicePanel.DicePanel();
            dpBackground.PanelStyle = GetViewModel<MainPageViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<MainPageViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<MainPageViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
            
            StartRoll();
        }

        void StartRoll()
        {
            dpBackground.RollDice(null);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
#if DEBUG
            StoreManager.CheckTrialDebug();
#endif
            //dpBackground = new Sanet.Kniffel.DicePanel.DicePanel();
            if (ViewModelProvider.HasViewModel<MainPageViewModel>())
            {
                //StartRoll();
            }
            dpBackground.EndRoll += StartRoll;
            SetViewModel<MainPageViewModel>();
            GetViewModel<MainPageViewModel>().PropertyChanged += GamePage_PropertyChanged;

            if (e.IsNavigationInitiator && ReviewBugger.IsTimeForReview())
                ReviewBugger.PromptUser();
            RebuildAppBarForPage();
            AttachNavigationEvents();
        }
        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<MainPageViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<MainPageViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<MainPageViewModel>().SettingsPanelStyle;

        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<MainPageViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            //dpBackground.Dispose();
            //dpBackground = null;
            DettachNavigationEvents();
        }
        
        /// <summary>
        /// Rebuilding app bar for main page content
        /// </summary>
        void RebuildAppBarForPage()
        {
            this.ApplicationBar.Buttons.Clear();
            
            this.ApplicationBar.Buttons.Add(fbButton);
            this.ApplicationBar.Buttons.Add(rateButton);
            if (StoreManager.IsTrial)
                this.ApplicationBar.Buttons.Add(buyButton);
            this.ApplicationBar.Buttons.Add(shareButton);

            this.ApplicationBar.IsMenuEnabled =false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;
        }

        #region ViewModel
        public void SetViewModel<T>() where T : BaseViewModel
        {
            DataContext = ViewModelProvider.GetViewModel<T>();

        }

        public T GetViewModel<T>() where T : BaseViewModel
        {
            if (DataContext == null)
                SetViewModel<T>();
            return (T)DataContext;
        }

        #endregion

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                MainMenuAction item = (MainMenuAction)(e.AddedItems[0]);
                item.MenuAction();
                ((ListBox)sender).SelectedItem = null;
            }
        }
    }
}