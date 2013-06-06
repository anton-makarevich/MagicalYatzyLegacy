using Microsoft.Phone.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.Common
{
    public static class WPIdentifyHelpers
    {
        public static string GetWindowsLiveAnonymousID()
        {
            string result = string.Empty;
            object anid;
            if (UserExtendedProperties.TryGetValue("ANID", out anid))
            {
                if (anid != null && anid.ToString().Length >= (32 + 2))
                {
                    result = anid.ToString().Substring(32, 2);
                }
            }

            return result;
        }
    }
}
