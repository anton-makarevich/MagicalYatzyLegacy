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
    public partial class LeaderboardPage : BasePage
    {
        // Constructor
        public LeaderboardPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
                                    
        }

                
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            //dpBackground = new Sanet.Kniffel.DicePanel.DicePanel();
            dpBackground.PanelStyle = GetViewModel<AboutPageViewModel>().SettingsPanelStyle;
            dpBackground.TreeDScaleCoef = 0.38;
            dpBackground.NumDice = 5;
            dpBackground.RollDelay = GetViewModel<AboutPageViewModel>().SettingsPanelSpeed;
            dpBackground.DieAngle = GetViewModel<AboutPageViewModel>().SettingsPanelAngle;
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
            if (ViewModelProvider.HasViewModel<AboutPageViewModel>())
            {
                //StartRoll();
            }
            dpBackground.EndRoll += StartRoll;
            SetViewModel<AboutPageViewModel>();
            GetViewModel<AboutPageViewModel>().PropertyChanged += GamePage_PropertyChanged;

            //if (ReviewBugger.IsTimeForReview())
            //    ReviewBugger.PromptUser();
            

        }
        void GamePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SettingsPanelAngle")
                dpBackground.DieAngle = GetViewModel<AboutPageViewModel>().SettingsPanelAngle;
            else if (e.PropertyName == "SettingsPanelSpeed")
                dpBackground.RollDelay = GetViewModel<AboutPageViewModel>().SettingsPanelSpeed;
            else if (e.PropertyName == "SettingsPanelStyle")
                dpBackground.PanelStyle = GetViewModel<AboutPageViewModel>().SettingsPanelStyle;

        }
        public override void NavigateFrom()
        {
            dpBackground.EndRoll -= StartRoll;
            GetViewModel<AboutPageViewModel>().PropertyChanged -= GamePage_PropertyChanged;
            
        }
        
               

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                AboutAppAction item = (AboutAppAction)(e.AddedItems[0]);
                item.MenuAction();
                ((ListBox)sender).SelectedItem = null;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton button = (HyperlinkButton)sender;
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(button.Tag.ToString()), "_blank");
        }

    }
}
