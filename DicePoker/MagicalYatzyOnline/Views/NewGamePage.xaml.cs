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
    public sealed partial class NewGamePage : BasePage
    {
        Sanet.Kniffel.Xna.DicePanel dpBackground;

        public NewGamePage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dpBackground.PanelStyle = GetViewModel<NewGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<NewGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<NewGameViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
            dpBackground.WithSound = false;
            dpBackground.EndRoll += StartRoll;
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
        public override void OnNavigatedTo()
        {
            // Create the game.
            dpBackground = XamlGame<Sanet.Kniffel.Xna.DicePanel>.Create("", Window.Current.CoreWindow, Panel);
            dpBackground.AddHandlers();
            dpBackground.Margin = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);
            
            SetViewModel<NewGameViewModel>();
            GetViewModel<NewGameViewModel>().PropertyChanged += GamePage_PropertyChanged;
            GetViewModel<NewGameViewModel>().FillRules();
        }
        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<NewGameViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<NewGameViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<NewGameViewModel>().SettingsPanelStyle;

        }
        public override void OnNavigatedFrom()
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<NewGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            dpBackground.Dispose();
            dpBackground = null;
            GetViewModel<NewGameViewModel>().SavePlayers();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            CommonNavigationActions.NavigateToMainPage();
        }
    }
}
