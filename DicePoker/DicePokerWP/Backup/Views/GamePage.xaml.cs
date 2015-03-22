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

        ApplicationBarIconButton readyButton;

        ApplicationBarIconButton sendButton;

        ApplicationBarIconButton buyButton;
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

            readyButton = new ApplicationBarIconButton();
            readyButton.IconUri = new Uri("/Assets/ready.png", UriKind.Relative);
            readyButton.Text = Messages.GAME_PLAY_READY.Localize();
            readyButton.Click += readyButton_Click;

            sendButton = new ApplicationBarIconButton();
            sendButton.IconUri = new Uri("/Assets/Send.png", UriKind.Relative);
            sendButton.Text = "SendLabel".Localize();
            sendButton.Click += sendButton_Click;

            buyButton = new ApplicationBarIconButton();
            buyButton.IconUri = new Uri("/Assets/appUnlock.png", UriKind.Relative);
            buyButton.Text = "BuyLabel".Localize();
            buyButton.Click += buyButton_Click;
        }
#if Win8
        async
#endif
        void buyButton_Click(object sender, EventArgs e)
        {
#if Win8
            await
#endif
            StoreManager.RemoveAd();
        }

        void sendButton_Click(object sender, EventArgs e)
        {
            ViewModelProvider.GetViewModel<PlayGameViewModel>().ChatModel.CurrentMessage = chatTextField.Text;
            unFocusButton.Focus();
            ViewModelProvider.GetViewModel<PlayGameViewModel>().ChatModel.SendHandler();
        }

        void readyButton_Click(object sender, EventArgs e)
        {
            ViewModelProvider.GetViewModel<PlayGameViewModel>().Game.SetPlayerReady(true);
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
            dpBackground.Visibility = Visibility.Visible;
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
            //rollPivot.SelectedItem = 1;

            dpBackground.PanelStyle = GetViewModel<PlayGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<PlayGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<PlayGameViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
            dpBackground.WithSound = true;
            dpBackground.ClickToFreeze = false;
            //try
            //{
            //    if (StoreManager.IsAdVisible())
            //        AdRotatorControl.Invalidate();
            //}
            //catch (Exception ex)
            //{
            //    var t = ex.Message;
            //}

            dpBackground.DieFrozen += dpBackground_DieFrozen;
            dpBackground.EndRoll += dpBackground_EndRoll;
            dpBackground.DieChangedManual += dpBackground_DieChangedManual;

            if (GetViewModel<PlayGameViewModel>().IsOnlineGame)
                chatPivotItem.Visibility = Visibility.Visible;
            else
                chatPivotItem.Visibility = Visibility.Collapsed;

            GetViewModel<PlayGameViewModel>().StartGame();

            //check chat pivot
            try
            {

                if (!GetViewModel<PlayGameViewModel>().IsOnlineGame)
                {
                    if (!rollPivot.IsPivotItemHidden(chatPivotItem))
                    {
                        rollPivot.HidePivotItem(chatPivotItem);

                    }
                }
                if (rollPivot.SelectedIndex != 2)
                    rollPivot.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                var t = ex.Message;
            }
            finally
            {
                rollPivot.SelectedIndex = 0;
            }
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
            rollPivot.SelectionChanged += rollPivot_SelectionChanged;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            AddGameHandlers();
            RebuildAppBarForRoll();
            GetViewModel<PlayGameViewModel>().IsBusy = false;
            
        }

        void rollPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModelProvider.HasViewModel<PlayGameViewModel>())
                if (rollPivot.SelectedIndex == 2)
                {
                    RebuildAppBarForChat();
                }
                else
                {
                    if (gridResults.Visibility == Visibility.Visible)
                        RebuildAppBarForEnd();
                    else if (currentAppBarState == appBarState.chat)
                    {
                        if (prevAppBarState == appBarState.ready)
                            RebuildAppBarForReady();
                        else
                            RebuildAppBarForRoll();
                    }
                }
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
                dpBackground.Visibility==Visibility.Visible)
                RebuildAppBarForRoll();
            else if ((e.PropertyName == "CanStart") &&
                dpBackground.Visibility == Visibility.Visible)
            {
                if (GetViewModel<PlayGameViewModel>().CanStart)
                    RebuildAppBarForReady();
                else
                    RebuildAppBarForRoll();
            }
        }

        protected async override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            

            if (gridResults.Visibility == Visibility.Visible)
            {
                gridResults.Visibility = Visibility.Collapsed;
                await GetViewModel<PlayGameViewModel>().SaveResults();
            }
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
            
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (rollPivot.SelectedIndex > 0)
            {
                e.Cancel = true;
                rollPivot.SelectedIndex--;
            }
            //else if (MessageBox.Show("ExitConfirmMessage".Localize(), "AppNameLabel".Localize(),
            //            MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            //{
            //    e.Cancel = true;

            //}
            else
            {
                GetViewModel<PlayGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;

                RemoveGameHandlers();
                if (dpBackground.ManualSetMode)
                    dpBackground.ManualSetMode = false;
                dpBackground.DieFrozen -= dpBackground_DieFrozen;
                dpBackground.EndRoll -= dpBackground_EndRoll;
                dpBackground.DieChangedManual -= dpBackground_DieChangedManual;

                JoinManager.Disconnect();
                base.OnBackKeyPress(e);
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
                rollPivot.SelectedIndex = 2;
            });
        }

        void Game_ResultApplied(object sender, Sanet.Kniffel.Models.Events.ResultEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                if (rollPivot.SelectedIndex != 2)
                    rollPivot.SelectedIndex = 0;
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
            rollPivot.SelectedIndex = 1;
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
            if (rollPivot.SelectedIndex != 2)
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

        appBarState prevAppBarState = appBarState.roll;
        appBarState currentAppBarState = appBarState.roll;

        void RebuildAppBarForRoll()
        {
            if (rollPivot.SelectedIndex == 2)
                return;

            try
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
                //else
                //{
                //    if (StoreManager.IsAdVisible())
                //        this.ApplicationBar.Buttons.Add(buyButton);
                //}
                rollButton.IsEnabled = GetViewModel<PlayGameViewModel>().CanRoll;
                this.ApplicationBar.Buttons.Add(rollButton);
                                
                this.ApplicationBar.IsMenuEnabled = false;
                this.ApplicationBar.Mode = ApplicationBarMode.Default;

                prevAppBarState = currentAppBarState;
                currentAppBarState = appBarState.roll;
            }
            catch { }
        }



        void RebuildAppBarForEnd()
        {
            this.ApplicationBar.Buttons.Clear();

            this.ApplicationBar.Buttons.Add(againButton);
            if (StoreManager.IsAdVisible())
                this.ApplicationBar.Buttons.Add(buyButton);

            this.ApplicationBar.IsMenuEnabled = false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;
            

        }
        
        void RebuildAppBarForReady()
        {
            if (rollPivot.SelectedIndex == 2)
                return;
            this.ApplicationBar.Buttons.Clear();

            this.ApplicationBar.Buttons.Add(readyButton);

            if (StoreManager.IsAdVisible())
                this.ApplicationBar.Buttons.Add(buyButton);

            this.ApplicationBar.IsMenuEnabled = false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;

            prevAppBarState = currentAppBarState;
            currentAppBarState = appBarState.ready;
        }

        void RebuildAppBarForChat()
        {
            this.ApplicationBar.Buttons.Clear();

            this.ApplicationBar.Buttons.Add(sendButton);
            if (StoreManager.IsAdVisible())
                this.ApplicationBar.Buttons.Add(buyButton);

            this.ApplicationBar.IsMenuEnabled = false;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;

            prevAppBarState = currentAppBarState;
            currentAppBarState = appBarState.chat;
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

        enum appBarState
        {
            roll,
            ready,
            chat
        }
                
    }
}