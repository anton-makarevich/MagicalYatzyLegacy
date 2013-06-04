using Facebook;
using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Common
{
    public class FacebookInfo
    {
        private const string _AppId = "438894422873149";

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string _ExtendedPermissions = "user_about_me,email,read_stream,publish_stream";

        private readonly FacebookClient _FBClient = new FacebookClient();

        public string AccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(_FBClient.AccessToken))
                    _FBClient.AccessToken = RoamingSettings.AccessToken;
                return _FBClient.AccessToken;
            }
            set
            {
                _FBClient.AccessToken = value;
                RoamingSettings.AccessToken = value;
            }
        }

        FacebookUserInfo userData;
        List<FacebookUserInfo> friendsData;
        
        public async Task<bool> CheckFBLogin (Uri resultingUri)
        {
            FacebookOAuthResult oauthResult;
            if (!_FBClient.TryParseOAuthCallbackUrl(resultingUri, out oauthResult))
            {
                return false;
            }

            if (oauthResult.IsSuccess)
            {
                var accessToken = oauthResult.AccessToken;
                await LoginSucceded(accessToken);
                return true;
            }
            else
            {
                // user cancelled
                return false;
            }
        }
        public Uri GetFacebookLoginUrl()
        {
            dynamic parameters = new ExpandoObject();
            parameters.client_id = _AppId;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";
            parameters.response_type = "token";
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrWhiteSpace(_ExtendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters.scope = _ExtendedPermissions;
            }

            return _FBClient.GetLoginUrl(parameters);
        }

        public Uri GetFaceBookLogoutUrl()
        {
            if (_FBClient == null || string.IsNullOrEmpty(AccessToken))
            {
                return null;
            }
            string logoutUri = string.Format("https://www.facebook.com/logout.php?next={0}&access_token={1}", "https://www.facebook.com/connect/login_success.html", AccessToken);
            return new Uri(logoutUri);
        }

        private async Task LoginSucceded(string accessToken)
        {
            AccessToken = accessToken;
            dynamic result = await _FBClient.GetTaskAsync("me");

            userData = new FacebookUserInfo();
            userData.Id=result.id;
            userData.FirstName=result.first_name;
            userData.LastName = result.last_name;
            userData.Email = result.email;
            userData.Gender = result.gender;

            //get friends data
            result =await _FBClient.GetTaskAsync("me/friends");
            friendsData = new List<FacebookUserInfo>();
            foreach (dynamic friend in result.data)
            {
                var friendData = new FacebookUserInfo();
                friendData.Id = friend.id;
                friendData.FirstName = friend.name;
                friendsData.Add(friendData);
            }
            
        }

        public void LogoutSucceded()
        {
            AccessToken = "";
            userData = null;

        }

        public async void PublishOnWall(string facebookid, string message)
        {
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
            var res = await _FBClient.PostTaskAsync(string.Format("me/feed"), parameters);
            
        }
       
#region properties
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
