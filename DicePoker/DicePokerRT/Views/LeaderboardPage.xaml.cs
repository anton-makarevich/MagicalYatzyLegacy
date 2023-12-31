﻿using Sanet.Kniffel.Models;
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
    public sealed partial class LeaderboardPage : BasePage
    {
        public LeaderboardPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dpBackground.PanelStyle = GetViewModel<LeaderboardViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<LeaderboardViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<LeaderboardViewModel>().SettingsPanelAngle;
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetViewModel<LeaderboardViewModel>();
            GetViewModel<LeaderboardViewModel>().RefreshScores();
            GetViewModel<LeaderboardViewModel>().PropertyChanged += GamePage_PropertyChanged;
        }
        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<LeaderboardViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<LeaderboardViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<LeaderboardViewModel>().SettingsPanelStyle;

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<LeaderboardViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            dpBackground.Dispose();
            dpBackground = null;
           
        }
    }
}
