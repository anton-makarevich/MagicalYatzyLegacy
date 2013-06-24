
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Sanet.Kniffel.Models
{
    public static class StoreManager
    {
        
        public static bool IsProductAvailable(string product)
        {
            return false;
        }
        public static bool BuyLicense(string product)
        {
           
            return true;          
        }

        static public bool IsAdVisible()
        {
            return IsProductAvailable("AdFree");
        }

        static public bool IsStylesAvailable()
        {
             return !IsProductAvailable("AdFree");
        }

        static public void RemoveAd()
        {
            //_adfree = !_adfree;
             BuyLicense("AdFree");
        }

        #region Trial
       
        private static bool _isTrial = true;
        public static bool IsTrial
        {
            get
            {
                return _isTrial;
            }
        }
        /// <summary>
        /// Check the current license information for this application
        /// </summary>
        public static void CheckTrial()
        {
            return;
        }

        #endregion

    }
}
