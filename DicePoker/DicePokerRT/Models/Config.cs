using Sanet.Kniffel.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel
{
    public static class Config
    {
        public static string GetHostName()
        {
#if !DEBUG
       // http://....
            return "yatzy.cloudapp.net/";
#else
            return "localhost:57584/";
#endif
        }

        public static ClientType GetClientType()
        { 
#if WinRT
            return ClientType.WinRT;
#endif
#if WP7
            return ClientType.WP7;
#endif
#if WP8
           return ClientType.WP8;
#endif
            return ClientType.VK;
        }

        public static int GetAdduplexId()
        {
#if WP8
            return 43963;//Wp8
#endif
#if WP7
            return 41731;//wp7
#endif
#if WinRT
            return 30603;//Win8
#endif

            return 0;

        }

    }
}
