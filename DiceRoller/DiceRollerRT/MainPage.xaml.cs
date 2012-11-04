using Sanet.Kniffel.ViewModels;
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

namespace Sanet.Kniffel.DiceRoller
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            
        }
        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = App.ViewModel;
            DicePanel1.PanelStyle = App.ViewModel.Settings.DiceStyle;
            DicePanel1.TreeDScaleCoef = 0.38;
            DicePanel1.NumDice = App.ViewModel.Settings.DiceNumber;
            DicePanel1.RollDelay = App.ViewModel.Settings.DiceSpeed;
            DicePanel1.DieAngle = App.ViewModel.Settings.DiceAngle;
            DicePanel1.MaxRollLoop = 40;
            DicePanel1.ClickToFreeze = true;


            //_shakeSensor = new AccelerometerSensorWithShakeDetection();
            //_shakeSensor.ShakeDetectedHandler += ShakeDetected;
            //_shakeSensor.Start();

            DicePanel1.DieFrozen += new Sanet.Kniffel.DicePanel.DicePanel.DieFrozenEventHandler(DicePanel1_DieFrozen);
            DicePanel1.EndRoll += new Sanet.Kniffel.DicePanel.DicePanel.EndRollEventHandler(DicePanel1_EndRoll);
            //ABClear.IsEnabled = false;
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
        }

        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            DicePanel1.RollDice(new List<int>());
            App.ViewModel.ClearResultsList();
        }

        private void ClearButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            DicePanel1.ClearFreeze();
            RollButton.IsEnabled = true;
            ClearButton.IsEnabled = false;
        }
    }
}
