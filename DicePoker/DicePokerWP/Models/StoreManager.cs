using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sanet.Kniffel.Models
{
    public static class StoreManager
    {
        
        static bool isProductAvailable(string product)
        {
            return true;//_adfree;
            //return (licenseInformation.ProductLicenses[product].IsActive);

            
        }
        static void BuyLicense(string product)
        {
            
            
        }

        static public bool IsAdVisible()
        {
            return !isProductAvailable("AdFree");
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
    }
}
