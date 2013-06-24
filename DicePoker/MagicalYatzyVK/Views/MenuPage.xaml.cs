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
using System.Windows.Navigation;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
using Sanet.Kniffel.Models;

namespace MagicalYatzyVK.Views
{
    public partial class MenuPage : BasePage
    {
        // Constructor
        public MenuPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
                                    
        }

                
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            //dpBackground = new Sanet.Kniffel.DicePanel.DicePanel();
            dpBackground.PanelStyle = GetViewModel<MainPageViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<MainPageViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<MainPageViewModel>().SettingsPanelAngle;
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

            //dpBackground = new Sanet.Kniffel.DicePanel.DicePanel();
            if (ViewModelProvider.HasViewModel<MainPageViewModel>())
            {
                //StartRoll();
            }
            dpBackground.EndRoll += StartRoll;
            SetViewModel<MainPageViewModel>();
            GetViewModel<MainPageViewModel>().PropertyChanged += GamePage_PropertyChanged;

            //if (ReviewBugger.IsTimeForReview())
            //    ReviewBugger.PromptUser();
            

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
        public override void NavigateFrom()
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<MainPageViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            
        }
        
               

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                MainMenuAction item = (MainMenuAction)(e.AddedItems[0]);
                item.MenuAction();
                ((ListBox)sender).SelectedItem = null;
            }
        }

    }
}
