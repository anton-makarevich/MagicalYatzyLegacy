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
using System.Threading.Tasks;
using System.Threading;

namespace DicePokerWP
{
    public partial class GamePage : PhoneApplicationPage
    {
        ApplicationBarIconButton rollButton;
        ApplicationBarIconButton againButton;

        ApplicationBarIconButton magicRollButton;
        ApplicationBarIconButton manualSetButton;
        ApplicationBarIconButton resetRollButton;

        // Constructor
        public GamePage()
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
            
            rollButton = new ApplicationBarIconButton();
            rollButton.IconUri = new Uri("/Assets/dice.png", UriKind.Relative);
            rollButton.Text = Messages.GAME_ROLL.Localize();
            rollButton.Click += rollButton_Click;

            againButton = new ApplicationBarIconButton();
            againButton.IconUri = new Uri("/Assets/redo.png", UriKind.Relative);
            againButton.Text = Messages.GAME_PLAY_AGAIN.Localize();
            againButton.Click += againButton_Click;

            magicRollButton = new ApplicationBarIconButton();
            magicRollButton.IconUri = new Uri("/Assets/magic_icon.png", UriKind.Relative);
            magicRollButton.Text = "MagicRollLabel".Localize();
            magicRollButton.Click += magicRollButton_Click;

            manualSetButton = new ApplicationBarIconButton();
            manualSetButton.IconUri = new Uri("/Assets/manual_icon.png", UriKind.Relative);
            manualSetButton.Text = "ManualSetLabel".Localize();
            manualSetButton.Click += manualSetButton_Click;

            resetRollButton = new ApplicationBarIconButton();
            resetRollButton.IconUri = new Uri("/Assets/reset_icon.png", UriKind.Relative);
            resetRollButton.Text = "ForthRollLabel".Localize();
            resetRollButton.Click += resetRollButton_Click;
        }

        void resetRollButton_Click(object sender, EventArgs e)
        {
            dpBackground.ClearFreeze();
            GetViewModel<PlayGameViewModel>().Game.ResetRolls();
        }

        void manualSetButton_Click(object sender, EventArgs e)
        {
            dpBackground.ManualSetMode = true;
            GetViewModel<PlayGameViewModel>().IsControlsVisible = false;
        }

        void magicRollButton_Click(object sender, EventArgs e)
        {
            dpBackground.ClearFreeze();
            GetViewModel<PlayGameViewModel>().Game.ReporMagictRoll();
        }

        void againButton_Click(object sender, EventArgs e)
        {
            GetViewModel<PlayGameViewModel>().PlayAgain();
            gridResults.Visibility = Visibility.Collapsed;
            rollPivot.Visibility = Visibility.Visible;
            RebuildAppBarForRoll();
        }

        void rollButton_Click(object sender, EventArgs e)
        {
            if (dpBackground.AllDiceFrozen())
                return;
            //SoundsProvider.PlaySound("click");
            GetViewModel<PlayGameViewModel>().Game.ReportRoll();
        }

                
        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            dpBackground.PanelStyle = GetViewModel<PlayGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<PlayGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<PlayGameViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
            dpBackground.ClickToFreeze = false;
            try
            {
                if (StoreManager.IsTrial)
                    AdRotatorControl.Invalidate();
            }
            catch (Exception ex)
            {
                var t = ex.Message;
            }

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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            SetViewModel<PlayGameViewModel>();
            GetViewModel<PlayGameViewModel>().PropertyChanged += GamePage_PropertyChanged;

            AddGameHandlers();
            RebuildAppBarForRoll();
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
                dpBackground.PanelStyle = GetViewModel<PlayGameViewModel>().SettingsPanelStyle;
            else if (e.PropertyName == "CanRoll")
            {
                if (GetViewModel<PlayGameViewModel>().CanRoll)
                {
                    rollButton.IsEnabled = true;
                }
                else
                    rollButton.IsEnabled = false;
            }
            else if (e.PropertyName == "RollLabel")
                rollButton.Text = GetViewModel<PlayGameViewModel>().RollLabel;
            else if (e.PropertyName == "IsControlsVisible")
                ApplicationBar.IsVisible = GetViewModel<PlayGameViewModel>().IsControlsVisible;
            else if (e.PropertyName == "IsMagicRollEnabled")
                magicRollButton.IsEnabled = GetViewModel<PlayGameViewModel>().IsMagicRollEnabled;
            else if (e.PropertyName == "IsManualSetEnabled")
                manualSetButton.IsEnabled = GetViewModel<PlayGameViewModel>().IsManualSetEnabled;
            else if (e.PropertyName == "IsForthRollEnabled")
                resetRollButton.IsEnabled = GetViewModel<PlayGameViewModel>().IsForthRollEnabled;
            else if ((e.PropertyName == "IsForthRollVisible" || 
                e.PropertyName == "IsMagicRollVisible" || 
                e.PropertyName == "IsManualSetVisible") &&
                rollPivot.Visibility==Visibility.Visible)
                RebuildAppBarForRoll();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            GetViewModel<PlayGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;

            RemoveGameHandlers();

            dpBackground.DieFrozen -= dpBackground_DieFrozen;
            dpBackground.EndRoll -= dpBackground_EndRoll;
            dpBackground.DieChangedManual -= dpBackground_DieChangedManual;
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
            GetViewModel<PlayGameViewModel>().Game.ResultApplied += Game_ResultApplied;
        }

        void Game_ResultApplied(object sender, Sanet.Kniffel.Models.Events.ResultEventArgs e)
        {
            rollPivot.SelectedIndex = 0;
        }



        void Game_DiceFixed(object sender, Sanet.Kniffel.Models.Events.FixDiceEventArgs e)
        {
            if (!GetViewModel<PlayGameViewModel>().SelectedPlayer.IsHuman)
                dpBackground.FixDice(e.Value, e.Isfixed);
        }

        void Game_GameFinished(object sender, EventArgs e)
        {
            gridResults.Visibility = Visibility.Visible;
            rollPivot.Visibility = Visibility.Collapsed;
            RebuildAppBarForEnd();
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
            
            rollPivot.SelectedIndex = 1;
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

        
        void RebuildAppBarForRoll()
        {
            this.ApplicationBar.Buttons.Clear();

            

            if (GetViewModel<PlayGameViewModel>().Game.Rules.Rule == Rules.krMagic)
            {
                if (GetViewModel<PlayGameViewModel>().IsMagicRollVisible)
                    this.ApplicationBar.Buttons.Add(magicRollButton);
                if (GetViewModel<PlayGameViewModel>().IsManualSetVisible)
                    this.ApplicationBar.Buttons.Add(manualSetButton);
                if (GetViewModel<PlayGameViewModel>().IsForthRollVisible)
                    this.ApplicationBar.Buttons.Add(resetRollButton);
            }

            this.ApplicationBar.Buttons.Add(rollButton);

            this.ApplicationBar.IsMenuEnabled = false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;

            
        }
        void RebuildAppBarForEnd()
        {
            this.ApplicationBar.Buttons.Clear();

            this.ApplicationBar.Buttons.Add(againButton);

            this.ApplicationBar.IsMenuEnabled = false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;


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
        //apply roll result
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RollResults.SelectedItem == null)
                return;
            GetViewModel<PlayGameViewModel>().Game.ApplyScore(((RollResultWrapper)(RollResults.SelectedItem)).Result);
            
        }
                
    }
}