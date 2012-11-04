using Sanet.Kniffel.DiceRoller;
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
    public sealed partial class AboutPage : BasePage
    {
        public AboutPage()
        {
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
        }
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
