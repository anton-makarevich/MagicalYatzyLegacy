using Facebook;
using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Facebook.Client;
#if WinRT
using System.Dynamic;

#endif
#if WINDOWS_PHONE
using System.Windows.Threading;
#endif
namespace Sanet.Common
{
    public class FacebookInfo
    {
        private const string _AppId = "438894422873149";

                
        private FacebookSessionClient FacebookSessionClient = new FacebookSessionClient(_AppId);
        private FacebookClient _FBClient;
        private FacebookSession _session;
        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string _ExtendedPermissions = "user_about_me,email,read_stream,publish_stream";
        FacebookUserInfo userData;
        List<FacebookUserInfo> friendsData;


        #region Methods
        public async Task<bool> Login()
        {
            _session =await FacebookSessionClient.LoginAsync(_ExtendedPermissions);
            AccessToken = _session.AccessToken;
            FacebookId = _session.FacebookId;
            _FBClient = new FacebookClient(_session.AccessToken);
#if WinRT
            dynamic result = await _FBClient.GetTaskAsync("me");

            userData = new FacebookUserInfo();
            userData.Id = result.id;
            userData.FirstName = result.first_name;
            userData.LastName = result.last_name;
            userData.Email = result.email;
            userData.Gender = result.gender;
#endif
#if WINDOWS_PHONE
            _FBClient.GetCompleted+= (o, e) =>
            {
                
                var result = (IDictionary<string, object>)e.GetResultData();
                userData = new FacebookUserInfo();
                userData.Id = result["id"].ToString();
                userData.FirstName = result["first_name"].ToString();
                userData.LastName = result["last_name"].ToString();
                userData.Email = result["email"].ToString();
                userData.Gender = result["gender"].ToString();
                
            };
            _FBClient.GetAsync("me");
#endif

            //get friends data
            /*result = await _FBClient.GetTaskAsync("me/friends");
            friendsData = new List<FacebookUserInfo>();
            foreach (dynamic friend in result.data)
            {
                var friendData = new FacebookUserInfo();
                friendData.Id = friend.id;
                friendData.FirstName = friend.name;
                friendsData.Add(friendData);
            }*/

                return true;
        }

        public void Logout()
        {
            FacebookSessionClient.Logout();
        }
                      
        public async void PublishOnWall(string facebookid, string message)
        {
            /*
            if (_FBClient == null || string.IsNullOrEmpty(AccessToken))
            {
                //TODO redirect to fbconnect??
                throw new Exception("Not connected to facebook");
            }
            dynamic parameters = new ExpandoObject();
            parameters.access_token = _FBClient.AccessToken;
            parameters.message = message;
            //parameters.tags = "1329346965";
            //parameters.place = "123";
            //parameters.name = "Test";
            //var res = await _FBClient.PostTaskAsync(string.Format("{0}/feed",facebookid), parameters);
            var res = await _FBClient.PostTaskAsync(string.Format("me/feed"), parameters);*/
            
        }
        #endregion

        #region properties

        public string FacebookId { get; set; }

        public string AccessToken
        {
            get
            {
                return RoamingSettings.AccessToken;
            }
            set
            {
                RoamingSettings.AccessToken = value;
            }
        }
        public string UserName
        {
            get
            {
                if (userData != null)
                    return string.Format("{0} {1}", userData.FirstName, userData.LastName);
                return string.Empty;
            }
        }

        public bool IsLogged
        {
            get
            { return (userData != null && friendsData!=null  ); }
        }

        public FacebookUserInfo FacebookUser
        {
            get
            {
                return userData;
            }
        }

        public List<FacebookUserInfo> FacebookFriends
        {
            get
            {
                return friendsData;
            }
        }

#endregion

    }

    

}
