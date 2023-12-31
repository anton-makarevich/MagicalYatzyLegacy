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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sanet.Kniffel.Views
{
    public sealed partial class SettingsPage : BasePage
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CommonNavigationActions.NavigateToMainPage();
        }
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var s = navigationParameter as SettingsViewModel;
            this.DataContext = (SettingsViewModel)navigationParameter;
        }
    }
}
