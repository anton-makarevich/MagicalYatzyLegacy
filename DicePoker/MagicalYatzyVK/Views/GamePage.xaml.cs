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
using System.Windows.Navigation;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
using Sanet.Kniffel.Models;

namespace MagicalYatzyVK.Views
{
    public partial class GamePage : BasePage
    {
        // Constructor
        public GamePage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
                                    
        }

                
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            dpBackground.PanelStyle = GetViewModel<PlayGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<PlayGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<PlayGameViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
            dpBackground.WithSound = true;
            dpBackground.ClickToFreeze = false;
            dpBackground.DieFrozen += dpBackground_DieFrozen;
            dpBackground.EndRoll += dpBackground_EndRoll;
            dpBackground.DieChangedManual += dpBackground_DieChangedManual;

            GetViewModel<PlayGameViewModel>().StartGame();

        }

        void dpBackground_DieChangedManual(bool isfixed, int oldvalue, int newvalue)
        {
            GetViewModel<PlayGameViewModel>().Game.ManualChange(isfixed, oldvalue, newvalue);
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

        void StartRoll()
        {
            dpBackground.RollDice(null);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        public override void NavigateTo()
        {

            SetViewModel<PlayGameViewModel>();
            GetViewModel<PlayGameViewModel>().PropertyChanged += GamePage_PropertyChanged;
            //rollPivot.SelectionChanged += rollPivot_SelectionChanged;
            AddGameHandlers();
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
        public async override void NavigateFrom()
        {
            if (gridResults.Visibility == Visibility.Visible)
            {
                gridResults.Visibility = Visibility.Collapsed;
                await GetViewModel<PlayGameViewModel>().SaveResults();
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
            GetViewModel<PlayGameViewModel>().Game.ResultApplied -= Game_ResultApplied;
            GetViewModel<PlayGameViewModel>().Game.OnChatMessage -= Game_OnChatMessage;
            GetViewModel<PlayGameViewModel>().RemoveGameHandlers();
        }

        void AddGameHandlers()
        {
            try
            {
                GetViewModel<PlayGameViewModel>().DiceRolled += Game_DiceRolled;
                GetViewModel<PlayGameViewModel>().MoveChanged += Game_MoveChanged;
                GetViewModel<PlayGameViewModel>().GameFinished += Game_GameFinished;
                GetViewModel<PlayGameViewModel>().DiceFixed += Game_DiceFixed;
                GetViewModel<PlayGameViewModel>().Game.DiceChanged += Game_DiceChanged;
                GetViewModel<PlayGameViewModel>().Game.PlayerRerolled += Game_PlayerRerolled;
                GetViewModel<PlayGameViewModel>().Game.ResultApplied += Game_ResultApplied;
                GetViewModel<PlayGameViewModel>().Game.OnChatMessage += Game_OnChatMessage;
            }
            catch
            { }
        }

        void Game_OnChatMessage(object sender, Sanet.Kniffel.Models.Events.ChatMessageEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                //rollPivot.SelectedIndex = 2;
            });
        }

        void Game_ResultApplied(object sender, Sanet.Kniffel.Models.Events.ResultEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                //if (rollPivot.SelectedIndex != 2)
                //    rollPivot.SelectedIndex = 0;
            });
        }



        void Game_DiceFixed(object sender, Sanet.Kniffel.Models.Events.FixDiceEventArgs e)
        {
            if (!GetViewModel<PlayGameViewModel>().SelectedPlayer.IsHuman)
                dpBackground.FixDice(e.Value, e.Isfixed);
        }

        void Game_GameFinished(object sender, EventArgs e)
        {
            gridResults.Visibility = Visibility.Visible;
            dpBackground.Visibility = Visibility.Collapsed;
            //rollPivot.SelectedIndex = 1;
            
        }

        void Game_MoveChanged(object sender, Sanet.Kniffel.Models.Events.MoveEventArgs e)
        {
            dpBackground.ClearFreeze();

            if (GetViewModel<PlayGameViewModel>().SelectedPlayer.IsBot)
            {
                //Thread.Sleep(1000);
                GetViewModel<PlayGameViewModel>().Game.ReportRoll();
            }
            if (dpBackground.PanelStyle != GetViewModel<PlayGameViewModel>().SelectedPlayer.SelectedStyle)
            {
                dpBackground.PanelStyle = GetViewModel<PlayGameViewModel>().SelectedPlayer.SelectedStyle;
            }
        }

        void Game_DiceRolled(object sender, Sanet.Kniffel.Models.Events.RollEventArgs e)
        {
            //if (rollPivot.SelectedIndex != 2)
            //    rollPivot.SelectedIndex = 1;
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
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RemoveGameHandlers();

            if (dpBackground.ManualSetMode)
                dpBackground.ManualSetMode = false;
            dpBackground.DieFrozen -= dpBackground_DieFrozen;
            dpBackground.EndRoll -= dpBackground_EndRoll;
            dpBackground.DieChangedManual -= dpBackground_DieChangedManual;

            JoinManager.Disconnect();
            CommonNavigationActions.NavigateToNewOnlineGamePage();
        }

        private void MagicRoll_Tapped_1(object sender, RoutedEventArgs e)
        {

            dpBackground.ClearFreeze();
            GetViewModel<PlayGameViewModel>().Game.ReporMagictRoll();
        }

        private void ManualSet_Tapped_1(object sender, RoutedEventArgs e)
        {

            dpBackground.ManualSetMode = true;
            GetViewModel<PlayGameViewModel>().IsControlsVisible = false;
        }

        private void ForthRoll_Tapped_1(object sender, RoutedEventArgs e)
        {

            dpBackground.ClearFreeze();
            GetViewModel<PlayGameViewModel>().Game.ResetRolls();

        }

        private void Button_Tapped_1(object sender, RoutedEventArgs e)
        {
            if (dpBackground.AllDiceFrozen())
                return;
            //SoundsProvider.PlaySound("click");
            GetViewModel<PlayGameViewModel>().Game.ReportRoll();
        }

        private void Button_Tapped_2(object sender, RoutedEventArgs e)
        {
            ViewModelProvider.GetViewModel<PlayGameViewModel>().Game.SetPlayerReady(true);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RollResults.SelectedItem == null)
                return;
            GetViewModel<PlayGameViewModel>().Game.ApplyScore(((RollResultWrapper)(RollResults.SelectedItem)).Result);
            
        }

        private void AgainButton_Tapped_1(object sender, RoutedEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().PlayAgain();
            gridResults.Visibility = Visibility.Collapsed;
            dpBackground.Visibility = Visibility.Visible;
        }

    }
}
