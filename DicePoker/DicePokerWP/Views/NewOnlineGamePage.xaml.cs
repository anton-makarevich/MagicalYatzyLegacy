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
using Sanet;

namespace DicePokerWP
{
    public partial class NewOnlineGamePage : PhoneApplicationPage
    {
        ApplicationBarIconButton startButton;

        // Constructor
        public NewOnlineGamePage()
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
                        
            startButton = new ApplicationBarIconButton();
            startButton.IconUri = new Uri("/Assets/Media-Play.png", UriKind.Relative);
            startButton.Text = Messages.NEW_GAME_START_GAME.Localize();
            startButton.Click += startButton_Click;

            
        }

        void startButton_Click(object sender, EventArgs e)
        {
            GetViewModel<NewOnlineGameViewModel>().StartGame();
        }

        

        void AttachNavigationEvents()
        {
            CommonNavigationActions.OnNavigationToGame += CommonNavigationActions_OnNavigationToGame;
            
        }
               

        void DettachNavigationEvents()
        {
            CommonNavigationActions.OnNavigationToGame -= CommonNavigationActions_OnNavigationToGame;
        }
        
        void CommonNavigationActions_OnNavigationToGame()
        {
            NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.RelativeOrAbsolute));
        }
        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            dpBackground.PanelStyle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<NewOnlineGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
            
            StartRoll();
            try
            {
                if (StoreManager.IsTrial)
                    AdRotatorControl.Invalidate();
            }
            catch (Exception ex)
            {
                var t = ex.Message;
            }
            RebuildAppBarForRules();
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
            dpBackground.EndRoll += StartRoll;

            SetViewModel<NewOnlineGameViewModel>();
            GetViewModel<NewOnlineGameViewModel>().PropertyChanged += GamePage_PropertyChanged;
            GetViewModel<NewOnlineGameViewModel>().MagicPageOpened += NewGamePage_MagicPageOpened;
            GetViewModel<NewOnlineGameViewModel>().FillRules();
            GetViewModel<NewOnlineGameViewModel>().InitOnServer(true);

            AttachNavigationEvents();
        }

        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<NewOnlineGameViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelStyle;
            else if (e.PropertyName == "IsReadyToPlay")
                if (GetViewModel<NewOnlineGameViewModel>().IsReadyToPlay)
                    startButton.IsEnabled = true;
                else
                    startButton.IsEnabled = false;
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<NewOnlineGameViewModel>().MagicPageOpened -= NewGamePage_MagicPageOpened;
            GetViewModel<NewOnlineGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            try
            {
                GetViewModel<NewOnlineGameViewModel>().SavePlayers();
            }
            catch (Exception ex)
            { }
            DettachNavigationEvents();
        }


        void NewGamePage_MagicPageOpened(object sender, EventArgs e)
        {
            ApplicationBar.IsVisible = false;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!ApplicationBar.IsVisible)
                {
                    e.Cancel = true;
                    GetViewModel<NewGameViewModel>().CloseMagicPage();
                    ApplicationBar.IsVisible = true;
                }
                else if (startPivot.SelectedIndex > 0)
                {
                    e.Cancel = true;
                    startPivot.SelectedIndex--;
                }
                else
                {
                    base.OnBackKeyPress(e);
                }
            }
            catch (Exception ex)
            {
                var t = ex.Message;
            }
        }
               

        void RebuildAppBarForRules()
        {
            this.ApplicationBar.Buttons.Clear();

            this.ApplicationBar.Buttons.Add(startButton);

            this.ApplicationBar.IsMenuEnabled = false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;


            if (GetViewModel<NewOnlineGameViewModel>().IsReadyToPlay)
                startButton.IsEnabled = true;
            else
                startButton.IsEnabled = false;
        }
        
        #region ViewModel
        public void SetViewModel<T>() where T : BaseViewModel
        {
            DataContext = ViewModelProvider.GetViewModel<T>();

        }

        public T GetViewModel<T>() where T : BaseViewModel
        {
            return (T)DataContext;
        }

        #endregion

        private void Like_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CommonNavigationActions.NavigateYatzyFBPage();
        }

        /// <summary>
        /// Facebook login press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            bool isLoaded = false;
            if (GetViewModel<NewOnlineGameViewModel>().SelectedPlayer.IsDefaultName)
            {
                try
                {
                    isLoaded = await App.FBInfo.Login();

                }
                catch (Exception ex)
                {
                    LogManager.Log("NOGVM.FacebookLogin", ex);

                }
            }
            else
            {
                App.FBInfo.Logout();

            }
            GetViewModel<NewOnlineGameViewModel>().LoadFacebookData(isLoaded);
        }

            
    }
}