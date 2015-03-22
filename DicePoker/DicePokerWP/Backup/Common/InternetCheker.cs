using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
            return (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType !=
 Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None);
        }
    }
}
