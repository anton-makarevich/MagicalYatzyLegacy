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
using Microsoft.Devices.Sensors;
using Sanet.DiceRoller.Models;
using Microsoft.Phone.Shell;
using Sanet.Kniffel.WP.DicePanel;
using System.Windows.Controls.Primitives;
using Sanet.DiceRoller.Views;

namespace Sanet.DiceRoller
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        ApplicationBarIconButton AboutButton;
        ApplicationBarIconButton ClearButton;
        ApplicationBarIconButton RollButton;
        Popup aboutPopup;

        ResourceModel RModel = new ResourceModel();

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            //AppBar
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = false;

            AboutButton = new ApplicationBarIconButton();
            AboutButton.IconUri = new Uri("/Images/help.png", UriKind.Relative);
            AboutButton.Text =RModel.GetString("about");
            AboutButton.Click += new EventHandler(About_Click);
            ApplicationBar.Buttons.Add(AboutButton);

            ClearButton = new ApplicationBarIconButton();
            ClearButton.IconUri = new Uri("/Images/clear.png", UriKind.Relative);
            ClearButton.Text = RModel.GetString("clear");
            ClearButton.Click += new EventHandler(Clear_Click);
            ClearButton.IsEnabled = false;
            ApplicationBar.Buttons.Add(ClearButton);

            RollButton = new ApplicationBarIconButton();
            RollButton.IconUri = new Uri("/Images/dice.png", UriKind.Relative);
            RollButton.Text =RModel.GetString("roll");
            RollButton.Click += new EventHandler(Roll_Click);
            ApplicationBar.Buttons.Add(RollButton);
            //Popups
            aboutPopup = new Popup();
            aboutPopup.HorizontalOffset = 5;
            aboutPopup.VerticalOffset = 40;
            aboutPopup.Child = new AboutPage();
            ((AboutPage)aboutPopup.Child).Rmodel = RModel;
            ((AboutPage)aboutPopup.Child).PopulateTexts();

            //setting butons
            (SpeedButton.Content as TextBlock).Text= RModel.GetString("FastLabel");
            (ProjectionButton.Content  as TextBlock).Text= RModel.GetString("LowLabel");

            (FastButton.Content  as TextBlock).Text= RModel.GetString("FastLabel");
            (VeryFastButton.Content  as TextBlock).Text= RModel.GetString("VeryFastLabel");
            (SlowButton.Content  as TextBlock).Text= RModel.GetString("SlowLabel");
            (VerySlowButton.Content  as TextBlock).Text= RModel.GetString("VerySlowLabel");

            (HighButton.Content as TextBlock).Text = RModel.GetString("HighLabel");
            (VeryHighButton.Content as TextBlock).Text = RModel.GetString("VeryHighLabel");
            (LowButton.Content as TextBlock).Text = RModel.GetString("LowLabel");
            
        }
        AccelerometerSensorWithShakeDetection _shakeSensor;
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            DicePanel1.Style = Sanet.Kniffel.WP.DicePanel.dpStyle.dpsBlue;
            DicePanel1.TreeDScaleCoef = 0.38;
        //DicePanel1.NumDice = 5;
        DicePanel1.RollDelay = 5;
        DicePanel1.MaxRollLoop = 40;
        DicePanel1.ClickToFreeze = true;


        _shakeSensor = new AccelerometerSensorWithShakeDetection();
        _shakeSensor.ShakeDetectedHandler +=ShakeDetected;
            _shakeSensor.Start();

            DicePanel1.DieFrozen += new Sanet.Kniffel.WP.DicePanel.DicePanel.DieFrozenEventHandler(DicePanel1_DieFrozen);
            DicePanel1.EndRoll += new Sanet.Kniffel.WP.DicePanel.DicePanel.EndRollEventHandler(DicePanel1_EndRoll);
            //ABClear.IsEnabled = false;
        }

        void DicePanel1_EndRoll()
        {
            TipsProvider1.Children.Clear();
            //MessageBox.Show(DicePanel1.Result.ToString());
            foreach(Die d in DicePanel1.aDice)
                TipsProvider1.ShowText(d.Result.ToString(),Colors.Blue);
            if (DicePanel1.NumDice > 1) TipsProvider1.ShowText(DicePanel1.Result.ToString(), Colors.Red);//(string.Format("{0}D6: {1}", DicePanel1.NumDice,DicePanel1.Result), Colors.Red);
            //if (DicePanel1.YhatzeeeFiveOfAKindScore() > 0) TipsProvider1.ShowText("FiveOfAKind",Colors.Orange);
            if (DicePanel1.NumDice > 3 && DicePanel1.NumPairs()>1)
            {
                TipsProvider1.ShowText(string.Format("{0} {1}", DicePanel1.NumPairs(), RModel.GetString("PairsLabel")), Colors.Orange);
            }
            if (DicePanel1.NumDice == 5)
            {
                if (DicePanel1.YhatzeeeFullHouseScore() > 0) TipsProvider1.ShowText(RModel.GetString("FullHouseLabel"), Colors.Orange);
                if (DicePanel1.YhatzeeeLargeStraightScore() > 0) TipsProvider1.ShowText(RModel.GetString("LargeStraightLabel"), Colors.Orange);
                else
                {
                    bool rb = false;
                    if (DicePanel1.YhatzeeeSmallStraightScore(false, ref rb) > 0) TipsProvider1.ShowText(RModel.GetString("SmallStraightLabel"), Colors.Orange);
                }
            }

            for (int i=6;i>2;i--)
                if (DicePanel1.YhatzeeeOfAKindScore(i) > 1)
                {
                    TipsProvider1.ShowText(string.Format("{0} {1}", i, RModel.GetString("OfAKindLabel")), Colors.Orange);
                    break;
                }
            
        }

        void DicePanel1_DieFrozen(bool @fixed, int Value)
        {
            if (DicePanel1.AllDiceFrozen()) RollButton.IsEnabled = false;
            else RollButton.IsEnabled = true;
            if (DicePanel1.FrozenCount()==0) ClearButton.IsEnabled = false;
            else ClearButton.IsEnabled = true;
        }
        private void About_Click(object sender, EventArgs e)
        {
            aboutPopup.IsOpen = !aboutPopup.IsOpen;
        }
        private void Roll_Click(object sender, EventArgs e)
        {
            DicePanel1.RollDice(null);
            TipsProvider1.Children.Clear();
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            DicePanel1.ClearFreeze();
            RollButton.IsEnabled = true;
            ClearButton.IsEnabled = false;
        }
        #region settings buttons
        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var cb = new Image { Source = (b.Content as Image).Source };
            if (b.Tag.ToString()=="Red")
            DicePanel1.Style = Sanet.Kniffel.WP.DicePanel.dpStyle.dpsBrutalRed;
            else if (b.Tag.ToString()=="Blue")
            DicePanel1.Style = Sanet.Kniffel.WP.DicePanel.dpStyle.dpsBlue;
            else DicePanel1.Style = Sanet.Kniffel.WP.DicePanel.dpStyle.dpsClassic;
            StyleButton.Content = cb;
            hideSettings();
        }
        private void hideSettings()
        {
            StyleButtons.Visibility = Visibility.Collapsed;
            CountButtons.Visibility = Visibility.Collapsed;
            SpeedButtons.Visibility = Visibility.Collapsed;
            ProjectionButtons.Visibility = Visibility.Collapsed;
            SettingsButtons.Visibility = Visibility.Visible;
            
        }
        private void StyleButton_Click(object sender, RoutedEventArgs e)
        {
            StyleButtons.Visibility = Visibility.Visible;
            SettingsButtons.Visibility = Visibility.Collapsed;
        }

        private void LowButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var cb = b.Content as TextBlock;
            DicePanel1.DieAngle = int.Parse(b.Tag.ToString());
            (ProjectionButton.Content as TextBlock).Text = cb.Text;
            hideSettings();
        }

        private void OneButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var cb = b.Content as TextBlock;
            DicePanel1.NumDice = int.Parse(cb.Text);
            (CountButton.Content as TextBlock).Text = cb.Text;
            hideSettings();
            TipsProvider1.Children.Clear();
            RollButton.IsEnabled = true;
            ClearButton.IsEnabled = false;
            
        }

        private void CountButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsButtons.Visibility = Visibility.Collapsed;
            CountButtons.Visibility = Visibility.Visible;
        }

        private void SpeedButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsButtons.Visibility = Visibility.Collapsed;
            SpeedButtons.Visibility = Visibility.Visible;
        }
        
        private void ProjectionButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsButtons.Visibility = Visibility.Collapsed;
            ProjectionButtons.Visibility = Visibility.Visible;
        }
        private void SlowButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var cb = b.Content as TextBlock;
            DicePanel1.RollDelay = int.Parse(b.Tag.ToString());
            (SpeedButton.Content as TextBlock).Text = cb.Text;
            hideSettings();
        }
        #endregion

        #region shaking
        private readonly Accelerometer _sensor = new Accelerometer();

        #endregion

        public void ShakeDetected(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>  TipsProvider1.Children.Clear());
            Dispatcher.BeginInvoke(() =>  DicePanel1.RollDice(null));
           
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (SettingsButtons.Visibility == Visibility.Collapsed)
            {
                e.Cancel = true;
                hideSettings();
                return;
            }
            else
            {
                {
                    base.OnBackKeyPress(e);
                }
            }


        }
    }
}