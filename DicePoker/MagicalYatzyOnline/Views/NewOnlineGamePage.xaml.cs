using MonoGame.Framework;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.ViewModels;
using Sanet.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DicePokerRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewOnlineGamePage : BasePage
    {
        DispatcherTimer passRotTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        DispatcherTimer nameRotTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };

        Sanet.Kniffel.Xna.DicePanel dpBackground;

        public NewOnlineGamePage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dpBackground.PanelStyle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<NewOnlineGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
            dpBackground.WithSound = false;
            dpBackground.EndRoll += StartRoll;
            StartRoll();

            
        }

        void NewOnlineGamePage_PasswordTapped(object sender, EventArgs e)
        {
            if (!PassPanel.IsFace)
                passRotTimer.Start();
            else
            {
                passRotTimer.Stop();
                startButton.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }


        void NewOnlineGamePage_NameTapped(object sender, EventArgs e)
        {
            if (!NamePanel.IsFace)
                nameRotTimer.Start();
            else
            {
                nameRotTimer.Stop();
                startButton.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }

        

        void passRotTimer_Tick(object sender, object e)
        {
            passRotTimer.Stop();
            passText.Visibility = Visibility.Visible;
            passText.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        void nameRotTimer_Tick(object sender, object e)
        {
            nameRotTimer.Stop();
            nameText.Visibility = Visibility.Visible;
            nameText.Focus(Windows.UI.Xaml.FocusState.Programmatic);
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
        public override void OnNavigatedTo()
        {
            // Create the game.
            dpBackground = XamlGame<Sanet.Kniffel.Xna.DicePanel>.Create("", Window.Current.CoreWindow, Panel);
            dpBackground.AddHandlers();
            dpBackground.Margin = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);
            
            SetViewModel<NewOnlineGameViewModel>();
            GetViewModel<NewOnlineGameViewModel>().PropertyChanged += GamePage_PropertyChanged;
            passRotTimer.Tick += passRotTimer_Tick;
            nameRotTimer.Tick += nameRotTimer_Tick;
            GetViewModel<NewOnlineGameViewModel>().NameTapped += NewOnlineGamePage_NameTapped;
            GetViewModel<NewOnlineGameViewModel>().PasswordTapped += NewOnlineGamePage_PasswordTapped;
            GetViewModel<NewOnlineGameViewModel>().FillRules();
            GetViewModel<NewOnlineGameViewModel>().InitOnServer(true);
        }

        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<NewOnlineGameViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelStyle;

        }
        public override void OnNavigatedFrom()
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<NewOnlineGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            passRotTimer.Tick -= passRotTimer_Tick;
            nameRotTimer.Tick -= nameRotTimer_Tick;
            GetViewModel<NewOnlineGameViewModel>().NameTapped -= NewOnlineGamePage_NameTapped;
            GetViewModel<NewOnlineGameViewModel>().PasswordTapped -= NewOnlineGamePage_PasswordTapped;
            dpBackground.Dispose();
            dpBackground = null;
            GetViewModel<NewOnlineGameViewModel>().SavePlayers();
        }

        
        private void Like_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CommonNavigationActions.NavigateYatzyFBPage();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            CommonNavigationActions.NavigateToMainPage();
        }
    }
}
