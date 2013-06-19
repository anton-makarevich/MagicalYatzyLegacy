using MagicalYatzyVK.Views;
using Sanet.Kniffel.Models;
using Sanet.Models;
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

namespace MagicalYatzyVK
{
    public partial class MainPage : UserControl
    {
        MenuPage _menuPage = new MenuPage();
        AboutPage _aboutPage = new AboutPage();
        LeaderboardPage _leaderboardPage = new LeaderboardPage();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            AttachNavigationEvents();
        }

        
        void AttachNavigationEvents()
        {
            CommonNavigationActions.OnNavigationToAbout += CommonNavigationActions_OnNavigationToAbout;
            CommonNavigationActions.OnNavigationToLeaderboard += CommonNavigationActions_OnNavigationToLeaderboard;
            CommonNavigationActions.OnNavigationToMainPage += CommonNavigationActions_OnNavigationToMainPage;
            CommonNavigationActions.OnNavigationToOnlineGame += CommonNavigationActions_OnNavigationToOnlineGame;
        }

        void CommonNavigationActions_OnNavigationToMainPage()
        {
            ((BasePage)NavigationBorder.Child).NavigateFrom();
            NavigationBorder.Child = _menuPage;
            _menuPage.NavigateTo();
        }

        void CommonNavigationActions_OnNavigationToOnlineGame()
        {
            
        }

        
        void CommonNavigationActions_OnNavigationToLeaderboard()
        {
            ((BasePage)NavigationBorder.Child).NavigateFrom();
            NavigationBorder.Child = _leaderboardPage;
            _leaderboardPage.NavigateTo();
        }

        void CommonNavigationActions_OnNavigationToAbout()
        {
            ((BasePage)NavigationBorder.Child).NavigateFrom();
            NavigationBorder.Child = _aboutPage;
            _aboutPage.NavigateTo();
        }

        

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Init smartdispatcher
            SmartDispatcher.Initialize(this.Dispatcher);
            NavigationBorder.Child = _menuPage;
            _menuPage.NavigateTo();
        }

                
        

       
    }
}
