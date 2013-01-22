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

namespace DicePokerWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            SetViewModel<MainPageViewModel>();
            GetViewModel<MainPageViewModel>().PropertyChanged += GamePage_PropertyChanged;
            //if (e.NavigationMode == NavigationMode.Back && ReviewBugger.IsTimeForReview())
            //    await ReviewBugger.PromptUser();
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
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<MainPageViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            dpBackground.Dispose();
            dpBackground = null;
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
    }
}