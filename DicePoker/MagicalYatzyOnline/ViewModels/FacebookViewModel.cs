
using DicePokerRT;
using DicePokerRT.KniffelLeaderBoardService;
using MagicalYatzyOnline;
using Sanet.Common;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Protocol;
using Sanet.Kniffel.WebApi;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Sanet.Kniffel.ViewModels
{
    public class FacebookViewModel: AdBasedViewModel
    {
        #region Events
        public event Action Done;
        #endregion

        #region Constructor
        public FacebookViewModel()
            :base()
        {
            CreateCommands();
        }

        
        #endregion

        #region Properties
        /// <summary>
        /// Page title
        /// </summary>
        public string Title
        {
            get
            {
                return "LoginToFBLabel".Localize();
            }
        }

               

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            private set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;

                }
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            private set
            {
                if (_LastName != value)
                {
                    _LastName = value;

                }
            }
        }

        public string Name
        {
            get
            {
                return string.Format("{0} {1}",FirstName,LastName);
            }
        }

        private List<FacebookUserInfo> _FacebookFriends;
        public List<FacebookUserInfo> FacebookFriends
        {
            get { return _FacebookFriends; }
            set
            {
                if (_FacebookFriends != value)
                {
                    _FacebookFriends = value;

                }
            }
        }


        private string _FacebookId;
        public string FacebookId
        {
            get { return _FacebookId; }
            private set
            {
                if (_FacebookId != value)
                {
                    _FacebookId = value;

                }
            }
        }




        public string[] UserFriendsFBIDs
        {
            get
            {
                return _FacebookFriends.Select(f => f.Id).ToArray();
            }

        }


        private string _Email;
        public string Email
        {
            get { return _Email; }
            private set
            {
                if (_Email != value)
                {
                    _Email = value;
                }
            }
        }
        bool _IsMale;
        public bool IsMale
        {
            get
            { return _IsMale; }
            set
            {
                _IsMale = value;
            }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Facebook login finished- checking data
        /// </summary>
        /// <param name="uri"></param>
        public async void FBLoginLoaded(Uri uri)
        {
            //IsBusy = true;
            if (await App.FBInfo.CheckFBLogin(uri))
            {
                //succesfully login to facebook


                FirstName = App.FBInfo.FacebookUser.FirstName;
                LastName = App.FBInfo.FacebookUser.LastName;
                FacebookId = App.FBInfo.FacebookUser.Id;
                Email = App.FBInfo.FacebookUser.Email;
                IsMale = App.FBInfo.FacebookUser.Gender == "male";
                FacebookFriends = App.FBInfo.FacebookFriends;

                //looking for accountid by 


                //finalizing registration
                if (Done != null)
                    Done();

            }
            {
                //InfoText = "Can't login to facebook";
            }
            //IsBusy = false;
        }

       
        
        #endregion

        #region Commands
       
        //public RelayCommand StartCommand { get; set; }
        
        protected void CreateCommands()
        {
            
            //StartCommand = new RelayCommand(o => StartGame(), () => true);
        }



        #endregion


    }
}
