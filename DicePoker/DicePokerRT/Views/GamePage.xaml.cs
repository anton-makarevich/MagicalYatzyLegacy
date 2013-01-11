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
    public sealed partial class GamePage : BasePage
    {
        public GamePage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dpBackground.PanelStyle = Sanet.Kniffel.DicePanel.dpStyle.dpsBlue;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = 15;
            dpBackground.DieAngle = 3;
            dpBackground.MaxRollLoop = 40;
            dpBackground.ClickToFreeze = false;

            dpBackground.DieFrozen += dpBackground_DieFrozen;
            dpBackground.EndRoll += dpBackground_EndRoll;
        }

        void dpBackground_EndRoll()
        {
            GetViewModel<PlayGameViewModel>().OnRollEnd();
        }

        void dpBackground_DieFrozen(bool isfixed, int value)
        {
            GetViewModel<PlayGameViewModel>().Game.FixDice(value, isfixed);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetViewModel<PlayGameViewModel>();
            GetViewModel<PlayGameViewModel>().Game.StartGame();
            GetViewModel<PlayGameViewModel>().Game.DiceRolled += Game_DiceRolled;
            GetViewModel<PlayGameViewModel>().Game.MoveChanged += Game_MoveChanged;
            GetViewModel<PlayGameViewModel>().Game.GameFinished += Game_GameFinished;
            GetViewModel<PlayGameViewModel>().PropertyChanged += GamePage_PropertyChanged;
            gridResults.Visibility = Visibility.Collapsed;
            dpBackground.Visibility = Visibility.Visible;
        }

        void Game_GameFinished(object sender, EventArgs e)
        {
            gridResults.Visibility = Visibility.Visible;
            dpBackground.Visibility = Visibility.Collapsed;
        }

        void Game_MoveChanged(object sender, Sanet.Kniffel.Models.Events.MoveEventArgs e)
        {
            dpBackground.ClearFreeze();
        }

        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CanFix")
                dpBackground.ClickToFreeze = GetViewModel<PlayGameViewModel>().CanFix;
        }

        void Game_DiceRolled(object sender, Sanet.Kniffel.Models.Events.RollEventArgs e)
        {
            dpBackground.RollDice(e.Value.ToList());
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            GetViewModel<PlayGameViewModel>().Game.DiceRolled -= Game_DiceRolled;
            GetViewModel<PlayGameViewModel>().Game.MoveChanged -= Game_MoveChanged;
            GetViewModel<PlayGameViewModel>().Game.GameFinished -= Game_GameFinished;
            GetViewModel<PlayGameViewModel>().RemoveGameHandlers();

            dpBackground.DieFrozen -= dpBackground_DieFrozen;
            dpBackground.EndRoll -= dpBackground_EndRoll;
            dpBackground.Dispose();
            dpBackground = null;
        }

        private void ClearButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().Game.ReportRoll();
        }

        private void GridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().Game.ApplyScore((RollResult)e.ClickedItem);
        }
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (gridResults.Visibility==Visibility.Visible)
                GetViewModel<PlayGameViewModel>().SaveResults();
            base.GoBack(sender, e);
        }
        private void AgainButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().PlayAgain();
            gridResults.Visibility = Visibility.Collapsed;
            dpBackground.Visibility = Visibility.Visible;
        }
       
    }
}
