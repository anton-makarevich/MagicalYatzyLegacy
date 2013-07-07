using MonoGame.Framework;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
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
    public sealed partial class MainPage : BasePage
    {
        Sanet.Kniffel.Xna.DicePanel dpBackground;
        public MainPage()
        {
            this.InitializeComponent();

            // Create the game.
            dpBackground = XamlGame<Sanet.Kniffel.Xna.DicePanel>.Create("", Window.Current.CoreWindow, Panel);

            this.Loaded += MainPage_Loaded;
            this.OnNavigatedTo();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dpBackground.PanelStyle = GetViewModel<MainPageViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<MainPageViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<MainPageViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
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
            SetViewModel<MainPageViewModel>();
            GetViewModel<MainPageViewModel>().PropertyChanged += GamePage_PropertyChanged;
            
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
        public override void OnNavigatedFrom()
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<MainPageViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            dpBackground.Dispose();
            dpBackground = null;
        }
        private void itemListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            ((MainMenuAction)e.ClickedItem).MenuAction();
        }

        private void itemGridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            ((AboutAppAction)e.ClickedItem).MenuAction();
        }
    }
}
