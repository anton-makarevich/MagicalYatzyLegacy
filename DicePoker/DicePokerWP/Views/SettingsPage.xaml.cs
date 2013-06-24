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
using Microsoft.Phone.Shell;
using Sanet.Kniffel.Models;

namespace DicePokerWP
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        // Constructor
        public SettingsPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        
        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            dpBackground.PanelStyle = GetViewModel<SettingsViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<SettingsViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<SettingsViewModel>().SettingsPanelAngle;
            dpBackground.MaxRollLoop = 40;
            
            StartRoll();
            try
            {
                if (StoreManager.IsTrial)
                    AdRotatorControl.Invalidate();
            }
            catch (Exception ex)
            {
                var t = ex.Message;
            }
            
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
            dpBackground.EndRoll += StartRoll;
            SetViewModel<SettingsViewModel>();
            GetViewModel<SettingsViewModel>().PropertyChanged += GamePage_PropertyChanged;
            
        }
        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<SettingsViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<SettingsViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<SettingsViewModel>().SettingsPanelStyle;

        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<SettingsViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            //dpBackground.Dispose();
            //dpBackground = null;
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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                AboutAppAction item = (AboutAppAction)(e.AddedItems[0]);
                item.MenuAction();
                ((ListBox)sender).SelectedItem = null;
            }
        }
                
    }
}