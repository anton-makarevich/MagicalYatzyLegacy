using MagicalYatzyVK.Views;
using Sanet.Kniffel.Models;
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
            CommonNavigationActions.OnNavigationToSettings += CommonNavigationActions_OnNavigationToSettings;
            CommonNavigationActions.OnNavigationToOnlineGame += CommonNavigationActions_OnNavigationToOnlineGame;
        }

        void CommonNavigationActions_OnNavigationToOnlineGame()
        {
            //NavigationService.Navigate(new Uri("/Views/NewOnlineGamePage.xaml", UriKind.RelativeOrAbsolute));
        }

        
        void CommonNavigationActions_OnNavigationToLeaderboard()
        {
            //NavigationService.Navigate(new Uri("/Views/LeaderboardPage.xaml", UriKind.RelativeOrAbsolute));
        }

        void CommonNavigationActions_OnNavigationToAbout()
        {
            //NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.RelativeOrAbsolute));
        }

        void CommonNavigationActions_OnNavigationToSettings()
        {
           // NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }
               


        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Init smartdispatcher
            SmartDispatcher.Initialize(this.Dispatcher);
            NavigationBorder.Child = _menuPage;
        }

                
        

       
    }
}
