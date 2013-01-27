using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Sanet.Models
{
    public static class InternetCheker
    {
        /// <summary>
        /// Check for internet connection
        /// </summary>
        /// <returns></returns>
        public static bool IsInternetAvailable()
        {
            ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (InternetConnectionProfile != null)
            {
                NetworkConnectivityLevel networkConnectivityLevel = InternetConnectionProfile.GetNetworkConnectivityLevel();
                if (networkConnectivityLevel == NetworkConnectivityLevel.InternetAccess)
                    return true;
            }
            return false;
        }
    }
}
