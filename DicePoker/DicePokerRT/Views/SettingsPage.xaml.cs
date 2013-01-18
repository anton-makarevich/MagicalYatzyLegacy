using Sanet.Kniffel.Models;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
using Sanet.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DicePokerRT
{
    public sealed partial class SettingsPage : BasePage
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.Width = Window.Current.Bounds.Width;
        }

        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetViewModel<SettingsViewModel>();
            //dpBackground.PanelStyle = Sanet.Kniffel.DicePanel.dpStyle.dpsBlue;
            //dpBackground.TreeDScaleCoef = 0.38;
            //dpBackground.NumDice = 5;
            //dpBackground.RollDelay = 15;
            //dpBackground.DieAngle = 3;
            //dpBackground.MaxRollLoop = 40;
            //dpBackground.EndRoll += StartRoll;
            //StartRoll();
            
        }

        //void StartRoll()
        //{
        //    dpBackground.RollDice(null);
        //}

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //dpBackground.EndRoll -= StartRoll;
            //dpBackground.Dispose();
            //dpBackground = null;
            this.Loaded -= MainPage_Loaded;
        }
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            ((Popup)this.Parent).IsOpen = false;
            
            if (ViewModelProvider.GetViewModel<PlayGameViewModel>() != null)
                ViewModelProvider.GetViewModel<PlayGameViewModel>().NotifySettingsChanged();

            if (ViewModelProvider.GetViewModel<MainPageViewModel>() != null)
                ViewModelProvider.GetViewModel<MainPageViewModel>().NotifySettingsChanged();

            if (ViewModelProvider.GetViewModel<AboutPageViewModel>() != null)
                ViewModelProvider.GetViewModel<AboutPageViewModel>().NotifySettingsChanged();

            if (ViewModelProvider.GetViewModel<LeaderboardViewModel>() != null)
                ViewModelProvider.GetViewModel<LeaderboardViewModel>().NotifySettingsChanged();

            if (ViewModelProvider.GetViewModel<NewGameViewModel>()!= null)
                ViewModelProvider.GetViewModel<NewGameViewModel>().NotifySettingsChanged();

            //SettingsPane.Show();
        }

        private void itemListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            ((AboutAppAction)e.ClickedItem).MenuAction();
        }
    }
}
