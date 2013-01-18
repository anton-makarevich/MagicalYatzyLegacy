using Sanet.AllWrite;
using Sanet.Kniffel.ViewModels;
using Sanet.Kniffel.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Sanet.Models;
using Sanet.Kniffel.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Sanet.Kniffel.DiceRoller
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            
        }
        private Accelerometer _accelerometer;
        private void VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            if (e.Visible)
                {
                    // Re-enable sensor input
                    _accelerometer.Shaken += new TypedEventHandler<Accelerometer, AccelerometerShakenEventArgs>(Shaken);
                }
                else
                {
                    // Disable sensor input
                    _accelerometer.Shaken -= new TypedEventHandler<Accelerometer, AccelerometerShakenEventArgs>(Shaken);
                }
            
        }

        DispatcherTimer adTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(3)
        };

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            StartLayoutUpdates(sender,e);

            DataContext = App.ViewModel;
            DicePanel1.PanelStyle = App.ViewModel.Settings.DiceStyle;
            DicePanel1.TreeDScaleCoef = 0.38;
            DicePanel1.NumDice = App.ViewModel.Settings.DiceNumber;
            DicePanel1.RollDelay = App.ViewModel.Settings.DiceSpeed;
            DicePanel1.DieAngle = App.ViewModel.Settings.DiceAngle;
            DicePanel1.MaxRollLoop = 40;
            DicePanel1.ClickToFreeze = true;
            
            _accelerometer = Accelerometer.GetDefault();
            if (_accelerometer != null)
            {
                Window.Current.VisibilityChanged += new WindowVisibilityChangedEventHandler(VisibilityChanged);
                _accelerometer.Shaken += new TypedEventHandler<Accelerometer, AccelerometerShakenEventArgs>(Shaken);
            }
            this.Focus(FocusState.Programmatic); 
            this.KeyUp += MainPage_KeyUp;

            DicePanel1.DieFrozen += new Sanet.Kniffel.DicePanel.DicePanel.DieFrozenEventHandler(DicePanel1_DieFrozen);
            DicePanel1.EndRoll += new Sanet.Kniffel.DicePanel.DicePanel.EndRollEventHandler(DicePanel1_EndRoll);
            //ABClear.IsEnabled = false;
            pokerAd.FontSize = 35;
            pokerAd.LifeTime = 7;

            adTimer.Tick += adTimer_Tick;
            adTimer.Start();
        }
        /// <summary>
        /// timer to run dicepoker ad
        /// </summary>
        int adLine = 1;
        void adTimer_Tick(object sender, object e)
        {
            pokerAd.ShowText(("DicePokerAdLine"+adLine.ToString()).Localize(), Brushes.SolidSanetBlue.Color);
            adLine++;
            if (adLine > 8)
                adLine = 1;
        }

        void MainPage_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.R)
            {
                rollDice();
            }
        }

        void DicePanel1_EndRoll()
        {
            App.ViewModel.OnRollEnd(DicePanel1.Result);
        }

        void DicePanel1_DieFrozen(bool @fixed, int Value)
        {
            if (DicePanel1.AllDiceFrozen()) RollButton.IsEnabled = false;
            else RollButton.IsEnabled = true;
            if (DicePanel1.FrozenCount() == 0) ClearButton.IsEnabled = false;
            else ClearButton.IsEnabled = true;
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.ViewModel.ClearResultsList();
        }
        //roll events
        async private void Shaken(object sender, AccelerometerShakenEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                rollDice();
            });
        }
        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            rollDice();
        }

        void rollDice()
        {
           App.ViewModel.ClearResultsList();
            DicePanel1.RollDice(new List<int>());
        }

        private void ClearButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            DicePanel1.ClearFreeze();
            RollButton.IsEnabled = true;
            ClearButton.IsEnabled = false;
        }

        private void settingsButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(SettingsPage), App.ViewModel.Settings);
        }

        private void helpButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AboutPage));
        }
        #region Visual state switching
        private List<Control> _layoutAwareControls;
        /// <summary>
        /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Loaded"/>
        /// event of a <see cref="Control"/> within the page, to indicate that the sender should
        /// start receiving visual state management changes that correspond to application view
        /// state changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
        /// management corresponding to view states.</param>
        /// <param name="e">Event data that describes how the request was made.</param>
        /// <remarks>The current view state will immediately be used to set the corresponding
        /// visual state when layout updates are requested.  A corresponding
        /// <see cref="FrameworkElement.Unloaded"/> event handler connected to
        /// <see cref="StopLayoutUpdates"/> is strongly encouraged.  Instances of
        /// <see cref="LayoutAwarePage"/> automatically invoke these handlers in their Loaded and
        /// Unloaded events.</remarks>
        /// <seealso cref="DetermineVisualState"/>
        /// <seealso cref="InvalidateVisualState"/>
        public void StartLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            if (this._layoutAwareControls == null)
            {
                // Start listening to view state changes when there are controls interested in updates
                Window.Current.SizeChanged += this.WindowSizeChanged;
                this._layoutAwareControls = new List<Control>();
            }
            this._layoutAwareControls.Add(control);

            // Set the initial visual state of the control
            VisualStateManager.GoToState(control, DetermineVisualState(ApplicationView.Value), false);
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Unloaded"/>
        /// event of a <see cref="Control"/>, to indicate that the sender should start receiving
        /// visual state management changes that correspond to application view state changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
        /// management corresponding to view states.</param>
        /// <param name="e">Event data that describes how the request was made.</param>
        /// <remarks>The current view state will immediately be used to set the corresponding
        /// visual state when layout updates are requested.</remarks>
        /// <seealso cref="StartLayoutUpdates"/>
        public void StopLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null || this._layoutAwareControls == null) return;
            this._layoutAwareControls.Remove(control);
            if (this._layoutAwareControls.Count == 0)
            {
                // Stop listening to view state changes when no controls are interested in updates
                this._layoutAwareControls = null;
                Window.Current.SizeChanged -= this.WindowSizeChanged;
            }
        }

        /// <summary>
        /// Translates <see cref="ApplicationViewState"/> values into strings for visual state
        /// management within the page.  The default implementation uses the names of enum values.
        /// Subclasses may override this method to control the mapping scheme used.
        /// </summary>
        /// <param name="viewState">View state for which a visual state is desired.</param>
        /// <returns>Visual state name used to drive the
        /// <see cref="VisualStateManager"/></returns>
        /// <seealso cref="InvalidateVisualState"/>
        protected virtual string DetermineVisualState(ApplicationViewState viewState)
        {
            return viewState.ToString();
        }

        /// <summary>
        /// Updates all controls that are listening for visual state changes with the correct
        /// visual state.
        /// </summary>
        /// <remarks>
        /// Typically used in conjunction with overriding <see cref="DetermineVisualState"/> to
        /// signal that a different value may be returned even though the view state has not
        /// changed.
        /// </remarks>
        public void InvalidateVisualState()
        {
            if (this._layoutAwareControls != null)
            {
                string visualState = DetermineVisualState(ApplicationView.Value);
                foreach (var layoutAwareControl in this._layoutAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                }
            }
            //if (DataContext != null && DataContext is MenuViewModel)
            //    (DataContext as MenuViewModel).UpdateMenuWidth();
        }

        #endregion

        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            CommonNavigationActions.NavigateToSanetDicePoker();
        }
    }
}
