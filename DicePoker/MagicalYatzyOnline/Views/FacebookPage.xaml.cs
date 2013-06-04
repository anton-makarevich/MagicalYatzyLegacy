using MagicalYatzyOnline;
using Sanet.Controls;
using Sanet.Kniffel.Models;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DicePokerRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FacebookPage : PopupPaneBase
    {
        public FacebookPage()
        {
            this.InitializeComponent();
            SetViewModel<FacebookViewModel>();
            GetViewModel<FacebookViewModel>().Done += FacebookPage_Done;
        }

        void FacebookPage_Done()
        {
            IsOk = true;
            parentPopup.IsOpen = false;
        }

       

        #region Facebook specific things

        /// <summary>
        /// FaceBook login uri
        /// </summary>
        Uri _FBLoginUri;
        public Uri FBLoginUri
        {
            get
            {
                return _FBLoginUri;//
            }
            set
            {
                _FBLoginUri = value;
                NavigateToFB();
            }
        }

        /// <summary>
        /// FaceBook logouturi
        /// </summary>
        Uri _FBLogoutUri;
        public Uri FBLogoutUri
        {
            get
            {
                return _FBLogoutUri;//
            }
            set
            {
                _FBLogoutUri = value;
                NavigateOutOfFB();
            }
        }

        void FBLoginView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            GetViewModel<FacebookViewModel>().FBLoginLoaded(e.Uri);

        }

        //Facebook

        

        void NavigateOutOfFB()
        {
            FBLoginView.LoadCompleted += CloseFB;
            var loginUrl = FBLogoutUri;
            if (loginUrl == null)
            {
                parentPopup.IsOpen = false;
                return;
            }
            FBLoginView.Navigate(loginUrl);
        }

        void CloseFB(object sender, NavigationEventArgs e)
        {
            FBLoginView.LoadCompleted -= CloseFB;
            parentPopup.IsOpen = false;
            App.FBInfo.LogoutSucceded();
        }



        void NavigateToFB()
        {
            FBLoginView.LoadCompleted += FBLoginView_LoadCompleted;
            var loginUrl = FBLoginUri;
            if (loginUrl == null)
            {
                parentPopup.IsOpen = false;
                return;
            }
            FBLoginView.Navigate(loginUrl);
        }

        public void LogIn()
        {
            FBLoginUri = App.FBInfo.GetFacebookLoginUrl();
        }

        public void LogOut()
        {
            var uri = App.FBInfo.GetFaceBookLogoutUrl();
            if (uri == null)
            {
                parentPopup.IsOpen = false;
                return;
            }
            FBLogoutUri = uri;
        }


        #endregion
    }
}
