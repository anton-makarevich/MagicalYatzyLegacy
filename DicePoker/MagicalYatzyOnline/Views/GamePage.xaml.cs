using Sanet.Kniffel.Models;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
using Sanet.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            Window.Current.SizeChanged += Current_SizeChanged;
            dpBackground.PanelStyle = GetViewModel<PlayGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.WithSound = true;
            dpBackground.RollDelay = GetViewModel<PlayGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<PlayGameViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 20;
            dpBackground.ClickToFreeze = false;

            dpBackground.DieFrozen += dpBackground_DieFrozen;
            dpBackground.EndRoll += dpBackground_EndRoll;
            dpBackground.DieChangedManual += dpBackground_DieChangedManual;
            
            GetViewModel<PlayGameViewModel>().StartGame();
        }

        void dpBackground_DieChangedManual(bool isfixed, int oldvalue, int newvalue)
        {
            GetViewModel<PlayGameViewModel>().Game.ManualChange(isfixed,oldvalue,newvalue);
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().UpdateDPWidth();
        }

        void dpBackground_EndRoll()
        {
            //if (GetViewModel<PlayGameViewModel>().SelectedPlayer.IsBot)
            //    dpBackground.ClearFreeze();
            GetViewModel<PlayGameViewModel>().OnRollEnd();
        }

        void dpBackground_DieFrozen(bool isfixed, int value)
        {
            if (GetViewModel<PlayGameViewModel>().SelectedPlayer.IsHuman)
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

            AddGameHandlers();
            GetViewModel<PlayGameViewModel>().PropertyChanged += GamePage_PropertyChanged;
            

            gridResults.Visibility = Visibility.Collapsed;
            dpBackground.Visibility = Visibility.Visible;
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            GetViewModel<PlayGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;

            if (gridResults.Visibility == Visibility.Visible)
            {
                gridResults.Visibility = Visibility.Collapsed;
                await GetViewModel<PlayGameViewModel>().SaveResults();
            }

            RemoveGameHandlers();

            dpBackground.DieFrozen -= dpBackground_DieFrozen;
            dpBackground.EndRoll -= dpBackground_EndRoll;
            dpBackground.DieChangedManual -= dpBackground_DieChangedManual;
            dpBackground.Dispose();

            JoinManager.Disconnect();

        }

        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CanFix")
                dpBackground.ClickToFreeze = GetViewModel<PlayGameViewModel>().CanFix;
            else if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<PlayGameViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<PlayGameViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
            {
                dpBackground.PanelStyle = GetViewModel<PlayGameViewModel>().SettingsPanelStyle;
                GetViewModel<PlayGameViewModel>().Game.ChangeStyle(null, dpBackground.PanelStyle);
            }

        }

        void RemoveGameHandlers()
        {
            
            GetViewModel<PlayGameViewModel>().Game.DiceRolled -= Game_DiceRolled;
            GetViewModel<PlayGameViewModel>().MoveChanged -= Game_MoveChanged;
            GetViewModel<PlayGameViewModel>().GameFinished -= Game_GameFinished;
            GetViewModel<PlayGameViewModel>().DiceFixed -= Game_DiceFixed;
            GetViewModel<PlayGameViewModel>().Game.DiceChanged -= Game_DiceChanged;
            GetViewModel<PlayGameViewModel>().Game.PlayerRerolled -= Game_PlayerRerolled;
            GetViewModel<PlayGameViewModel>().RemoveGameHandlers();
        }

        void AddGameHandlers()
        {
            GetViewModel<PlayGameViewModel>().DiceRolled += Game_DiceRolled;
            GetViewModel<PlayGameViewModel>().MoveChanged += Game_MoveChanged;
            GetViewModel<PlayGameViewModel>().GameFinished += Game_GameFinished;
            GetViewModel<PlayGameViewModel>().DiceFixed += Game_DiceFixed;
            GetViewModel<PlayGameViewModel>().Game.DiceChanged += Game_DiceChanged;
            GetViewModel<PlayGameViewModel>().Game.PlayerRerolled += Game_PlayerRerolled;
        }

        

        void Game_DiceFixed(object sender, Sanet.Kniffel.Models.Events.FixDiceEventArgs e)
        {
            if (!GetViewModel<PlayGameViewModel>().SelectedPlayer.IsHuman)
                dpBackground.FixDice(e.Value,e.Isfixed);
        }

        void Game_GameFinished(object sender, EventArgs e)
        {
            gridResults.Visibility = Visibility.Visible;
            dpBackground.Visibility = Visibility.Collapsed;
        }

        async void Game_MoveChanged(object sender, Sanet.Kniffel.Models.Events.MoveEventArgs e)
        {
            dpBackground.ClearFreeze();

            if (GetViewModel<PlayGameViewModel>().SelectedPlayer.IsBot)
            {
                await Task.Delay(3000);
                GetViewModel<PlayGameViewModel>().Game.ReportRoll();
            }
            if (dpBackground.PanelStyle != GetViewModel<PlayGameViewModel>().SelectedPlayer.SelectedStyle)
            {
                dpBackground.PanelStyle = GetViewModel<PlayGameViewModel>().SelectedPlayer.SelectedStyle;
            }
        }

        void Game_DiceRolled(object sender, Sanet.Kniffel.Models.Events.RollEventArgs e)
        {
            dpBackground.RollDice(e.Value.ToList());
        }

        

        void Game_PlayerRerolled(object sender, Sanet.Kniffel.Models.Events.PlayerEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        dpBackground.ClearFreeze();
                    });
        }

        

        

        void Game_DiceChanged(object sender, Sanet.Kniffel.Models.Events.RollEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
                    {
                        //if (!GetViewModel<PlayGameViewModel>().SelectedPlayer.IsHuman)
                        //{
                        var oldValues = dpBackground.Result.DiceResults.OrderBy(f => f).ToList();
                        var newValues = e.Value.ToList().OrderBy(f => f).ToList();
                        int oldvalue = 0;
                        int newvalue = 0;
                        if (oldValues.Count == newValues.Count)
                        {
                            for (int i = 0; i < oldValues.Count; i++)
                            {
                                var ov = oldValues[i];
                                if (newValues.Contains(ov) && oldValues.Count(f => f == ov) == newValues.Count(f => f == ov))
                                    continue;
                                else if (oldValues.Count(f => f == ov) > newValues.Count(f => f == ov))
                                {
                                    oldvalue = ov;
                                    break;
                                }
                            }
                            for (int i = 0; i < newValues.Count; i++)
                            {
                                var ov = newValues[i];
                                if (oldValues.Contains(ov) && oldValues.Count(f => f == ov) == newValues.Count(f => f == ov))
                                    continue;
                                else if (newValues.Count(f => f == ov) > oldValues.Count(f => f == ov))
                                {
                                    newvalue = ov;
                                    break;
                                }
                            }
                            if (oldvalue != 0 && newvalue != 0)
                                dpBackground.ChangeDice(oldvalue, newvalue);
                        }
                    });
            //}
        }

        
        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (dpBackground.AllDiceFrozen())
                return;
            //SoundsProvider.PlaySound("click");
            GetViewModel<PlayGameViewModel>().Game.ReportRoll();
        }

        private void GridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().Game.ApplyScore(((RollResultWrapper)e.ClickedItem).Result);
        }
        protected async override void GoBack(object sender, RoutedEventArgs e)
        {
            
            base.GoBack(sender, e);
        }
        private void AgainButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().PlayAgain();
            gridResults.Visibility = Visibility.Collapsed;
            dpBackground.Visibility = Visibility.Visible;
        }

        private void MagicRoll_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            
            dpBackground.ClearFreeze();
            GetViewModel<PlayGameViewModel>().Game.ReporMagictRoll();
        }

        private void ManualSet_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            
            dpBackground.ManualSetMode=true;
            GetViewModel<PlayGameViewModel>().IsControlsVisible = false;
        }

        private void ForthRoll_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            
            dpBackground.ClearFreeze();
            GetViewModel<PlayGameViewModel>().Game.ResetRolls();
                        
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await StoreManager.RemoveAd();
            ViewModelProvider.GetViewModel<PlayGameViewModel>().NotifyAdChanged();
            
        }

        private void Button_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            ViewModelProvider.GetViewModel<PlayGameViewModel>().Game.SetPlayerReady( true);
        }

        private void BlurbControl_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            ViewModelProvider.GetViewModel<PlayGameViewModel>().IsChatOpen = true;
        }

        private void AppBar_Closed_1(object sender, object e)
        {
            ViewModelProvider.GetViewModel<PlayGameViewModel>().IsChatOpen = false;
        }

        private void AppBar_Opened_1(object sender, object e)
        {
            ViewModelProvider.GetViewModel<PlayGameViewModel>().IsChatOpen = true;
        }
       
    }
}
