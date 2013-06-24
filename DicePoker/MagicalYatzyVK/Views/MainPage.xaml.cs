using MagicalYatzyVK.Views;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using VKontakte;

namespace MagicalYatzyVK
{
    public partial class MainPage : UserControl
    {
        //Vkontakte variables
        // ID of silverlight object in HTML page markup
        private string objectID = "kniffelsl";
        // Your application ID
        private int appID = 2121738;
        // Your application secret
        private string secret = "GyrW1jQHamrqhXG8hV7T";
        // User ID
        private string iuid = "0";
        //private long[] uids;

        //pages
        MenuPage _menuPage = new MenuPage();
        AboutPage _aboutPage = new AboutPage();
        LeaderboardPage _leaderboardPage = new LeaderboardPage();
        NewOnlineGamePage _newPage = new NewOnlineGamePage();
        GamePage _gamePage = new GamePage();

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
            CommonNavigationActions.OnNavigationToGame += CommonNavigationActions_OnNavigationToGame;
        }

        void CommonNavigationActions_OnNavigationToGame()
        {
            ((BasePage)NavigationBorder.Child).NavigateFrom();
            NavigationBorder.Child = _gamePage;
            _gamePage.NavigateTo();
        }

        void CommonNavigationActions_OnNavigationToMainPage()
        {
            ((BasePage)NavigationBorder.Child).NavigateFrom();
            NavigationBorder.Child = _menuPage;
            _menuPage.NavigateTo();
        }

        void CommonNavigationActions_OnNavigationToOnlineGame()
        {
            ((BasePage)NavigationBorder.Child).NavigateFrom();
            NavigationBorder.Child = _newPage;
            _newPage.NavigateTo();
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
            try
            {
                iuid = HtmlPage.Document.QueryString["viewer_id"];

                LoadVK();
            }
            catch 
            {
                App.VKName = "VK TestUser";
                App.VKPass = "vk_0123456789";
            }
            //Init smartdispatcher
            SmartDispatcher.Initialize(this.Dispatcher);
            NavigationBorder.Child = _menuPage;
            _menuPage.NavigateTo();
        }

        
        #region VKConnection
        APIConnection VK;

        void LoadVK()
        {
            VK = new APIConnection(secret, MainErrorHendler);
            VK.LoadProfile(this.Dispatcher, LoadProfileHandler);
        }


        # region Handlers

        void MainErrorHendler(XDocument error)
        {
            MessageBox.Show( error.ToString());
        }


        void GetPhotoHendler(string response)
        {
            if (response != null)
            {
                MessageBox.Show(response);
            }
        }

        void LoadProfileHandler(Profile response)
        {
            if (response != null)
            {
                App.VKName=response.FirstName + " " + response.LastName;
                App.VKPass = "vk_" + iuid;
                App.VKPic = response.Photo;
                //MessageBox.Show(response.Photo);
            }
        }
        #endregion
        #endregion

    }
}
