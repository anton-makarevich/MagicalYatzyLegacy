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
    public partial class NewGamePage : PhoneApplicationPage
    {
        ApplicationBarIconButton addPlayerButton;
        ApplicationBarIconButton addBotButton;
        ApplicationBarIconButton deleteButton;
        ApplicationBarIconButton startButton;

        // Constructor
        public NewGamePage()
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

            addPlayerButton = new ApplicationBarIconButton();
            addPlayerButton.IconUri = new Uri("/Assets/user-add.png", UriKind.Relative);
            addPlayerButton.Text = Messages.NEW_GAME_ADD_HUMAN.Localize();
            addPlayerButton.Click += addPlayerButton_Click;


            addBotButton = new ApplicationBarIconButton();
            addBotButton.IconUri = new Uri("/Assets/laptop.png", UriKind.Relative);
            addBotButton.Text = Messages.NEW_GAME_ADD_BOT.Localize();
            addBotButton.Click += addBotButton_Click;

            deleteButton = new ApplicationBarIconButton();
            deleteButton.IconUri = new Uri("/Assets/delete.png", UriKind.Relative);
            deleteButton.Text = "DeletePlayerLabel".Localize();
            deleteButton.Click += deleteButton_Click;

            startButton = new ApplicationBarIconButton();
            startButton.IconUri = new Uri("/Assets/dice.png", UriKind.Relative);
            startButton.Text = Messages.NEW_GAME_START_GAME.Localize();
            startButton.Click += startButton_Click;

            
        }

        void startButton_Click(object sender, EventArgs e)
        {
            GetViewModel<NewGameViewModel>().StartGame();
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            GetViewModel<NewGameViewModel>().DeletePlayer();
        }

        void addBotButton_Click(object sender, EventArgs e)
        {
            GetViewModel<NewGameViewModel>().AddPlayer(PlayerType.AI);
        }

        void addPlayerButton_Click(object sender, EventArgs e)
        {
            GetViewModel<NewGameViewModel>().AddPlayer(PlayerType.Local);
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
            
            dpBackground.PanelStyle = GetViewModel<NewGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<NewGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<NewGameViewModel>().SettingsPanelAngle;
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
            if (startPivot.SelectedIndex == 0)
                RebuildAppBarForPlayers();
            else
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
            startPivot.SelectionChanged += Pivot_SelectionChanged;
            SetViewModel<NewGameViewModel>();
            GetViewModel<NewGameViewModel>().PropertyChanged += GamePage_PropertyChanged;
            GetViewModel<NewGameViewModel>().FillRules();
            AttachNavigationEvents();
        }
        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<NewGameViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<NewGameViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<NewGameViewModel>().SettingsPanelStyle;
            else if (e.PropertyName == "CanAddPlayer")
            {
                if (GetViewModel<NewGameViewModel>().CanAddPlayer)
                {
                    addPlayerButton.IsEnabled = true;
                    addBotButton.IsEnabled = true;
                }
                else
                {
                    addPlayerButton.IsEnabled = false;
                    addBotButton.IsEnabled = false;
                }
            }
            else if (e.PropertyName == "CanDeletePlayer")
                if (GetViewModel<NewGameViewModel>().CanDeletePlayer)
                    deleteButton.IsEnabled = true;
                else
                    deleteButton.IsEnabled = false;

            else if (e.PropertyName == "IsReadyToPlay")
                if (GetViewModel<NewGameViewModel>().IsReadyToPlay)
                    startButton.IsEnabled = true;
                else
                    startButton.IsEnabled = false;
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            dpBackground.EndRoll -= StartRoll;
            startPivot.SelectionChanged -= Pivot_SelectionChanged;
            GetViewModel<NewGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            try
            {
                GetViewModel<NewGameViewModel>().SavePlayers();
            }
            catch (Exception ex)
            { }
            DettachNavigationEvents();
        }

        void RebuildAppBarForPlayers()
        {
            this.ApplicationBar.Buttons.Clear();

            this.ApplicationBar.Buttons.Add(addPlayerButton);
            this.ApplicationBar.Buttons.Add(addBotButton);
            this.ApplicationBar.Buttons.Add(deleteButton);

            this.ApplicationBar.IsMenuEnabled = false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;

            if (GetViewModel<NewGameViewModel>().CanAddPlayer)
            {
                addPlayerButton.IsEnabled = true;
                addBotButton.IsEnabled = true;
            }
            else
            {
                addPlayerButton.IsEnabled = false;
                addBotButton.IsEnabled = false;
            }
            
           
            if (GetViewModel<NewGameViewModel>().CanDeletePlayer)
                deleteButton.IsEnabled = true;
            else
                deleteButton.IsEnabled = false;
        }

        void RebuildAppBarForRules()
        {
            this.ApplicationBar.Buttons.Clear();

            this.ApplicationBar.Buttons.Add(startButton);

            this.ApplicationBar.IsMenuEnabled = false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;

            
            if (GetViewModel<NewGameViewModel>().IsReadyToPlay)
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

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (startPivot.SelectedIndex == 0)
                RebuildAppBarForPlayers();
            else
                RebuildAppBarForRules();
        }
                
    }
}