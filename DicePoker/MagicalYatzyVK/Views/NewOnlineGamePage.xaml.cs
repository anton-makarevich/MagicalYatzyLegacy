﻿using System;
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
    public partial class NewOnlineGamePage : BasePage
    {
        // Constructor
        public NewOnlineGamePage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
                                    
        }

                
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            //dpBackground = new Sanet.Kniffel.DicePanel.DicePanel();
            dpBackground.PanelStyle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<NewOnlineGameViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;

                                    
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
        public override void NavigateTo()
        {

            dpBackground.EndRoll += StartRoll;
            SetViewModel<NewOnlineGameViewModel>();
            GetViewModel<NewOnlineGameViewModel>().PropertyChanged += GamePage_PropertyChanged;
            GetViewModel<NewOnlineGameViewModel>().FillRules();
            GetViewModel<NewOnlineGameViewModel>().InitOnServer(true);
        }
        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<NewOnlineGameViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<NewOnlineGameViewModel>().SettingsPanelStyle;

        }
        public override void NavigateFrom()
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<NewOnlineGameViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            
        }
        
          

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton button = (HyperlinkButton)sender;
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(button.Tag.ToString()), "_blank");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonNavigationActions.NavigateToMainPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetViewModel<NewOnlineGameViewModel>().StartGame();
        }
    }
}
