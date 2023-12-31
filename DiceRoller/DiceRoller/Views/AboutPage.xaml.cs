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
using System.Reflection;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Tasks;
using Sanet.DiceRoller.Models;
using Sanet.Models;


namespace Sanet.DiceRoller.Views
{
    public partial class AboutPage : UserControl
    {
        public ResourceModel Rmodel;
        //MarketplaceDetailTask _marketPlaceDetailTask = new MarketplaceDetailTask();
        MarketplaceReviewTask _marketPlaceReviewTask = new MarketplaceReviewTask();
        public AboutPage()
        {
            InitializeComponent();
             
        }
        public void PopulateTexts()
        {
            var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            
            VersionText.Text = Rmodel.GetString("Version")+" " + nameHelper.Version.ToString();
            AboutText.Text = Rmodel.GetString("AboutLabel");
            OnlineVersionText.Text = Rmodel.GetString("OnlineLabel");
            OnlineLink.Content = Rmodel.GetString("AboutOnlineLabel");
            AuthorText.Text = Rmodel.GetString("DevelopedByLabel");
            SupportText.Text = Rmodel.GetString("SupportLabel");
            //PrimoText.Text = Rmodel.GetString("PrimoLabel");
            //PrimoLink.Content = Rmodel.GetString("AboutPrimoLabel");
            CloseButton.Content = Rmodel.GetString("CloseLabel");
            RateButton.Content = Rmodel.GetString("RateLabel");

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Popup)this.Parent).IsOpen = false;
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            _marketPlaceReviewTask.Show();
        }
    }
}
