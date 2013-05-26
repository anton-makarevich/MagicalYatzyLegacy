using Microsoft.Phone.Tasks;
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
        
        static bool isProductAvailable(string product)
        {
            return _isTrial;
        }
        static void BuyLicense(string product)
        {
            MarketplaceDetailTask task = new MarketplaceDetailTask();
            //task.ContentType = MarketplaceContentType.Applications;
            task.Show();
            CheckTrial();
                        
        }

        static public bool IsAdVisible()
        {
            return isProductAvailable("AdFree");
        }

        static public bool IsStylesAvailable()
        {
             return isProductAvailable("AdFree");
        }

        static public void RemoveAd()
        {
            //_adfree = !_adfree;
             BuyLicense("AdFree");
        }

        #region Trial
        private static Microsoft.Phone.Marketplace.LicenseInformation _licenseInfo = new Microsoft.Phone.Marketplace.LicenseInformation();
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

#if DEBUG
            _isTrial = true;

#else
            _isTrial = _licenseInfo.IsTrial();
#endif
        }

        private static bool askedAboutTrial = false;
        /// <summary>
        /// Ask user how he wants to use in debug
        /// </summary>
        public static void CheckTrialDebug()
        {
            if (!askedAboutTrial)
            {
                // When debugging, we want to simulate a trial mode experience. The following conditional allows us to set the _isTrial 
                // property to simulate trial mode being on or off. 
#if DEBUG
                string message = "Press 'OK', to launch app in Trial simulation mode. Press 'Cancel' to launch the full version.";
                if (MessageBox.Show(message, "Trial Debug",
                     MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    _isTrial = true;
                }
                else
                {
                    _isTrial = false;
                }
#else
            _isTrial = _licenseInfo.IsTrial();
#endif
                askedAboutTrial = true;
            }
        }
        #endregion

    }
}
