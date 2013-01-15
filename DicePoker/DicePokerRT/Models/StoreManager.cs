using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace Sanet.Kniffel.Models
{
    public static class StoreManager
    {
        static LicenseInformation licenseInformation= CurrentApp.LicenseInformation;
        //static bool _adfree=true;
        static bool isProductAvailable(string product)
        {
            //return _adfree;
            return (licenseInformation.ProductLicenses[product].IsActive);

            
        }
        static async Task BuyLicense(string product)
        {
            if (!isProductAvailable(product))
            {
                
                try
                {

                    //await CurrentAppSimulator.RequestProductPurchaseAsync(thisfolder, false);

                    await CurrentApp.RequestProductPurchaseAsync(product, false);

                }
                catch (Exception Ex)
                {
                    var s = Ex.Message;
                }
            }
            else
            {
                // The customer already owns this feature.
            }
            
        }

        static public bool IsAdVisible()
        {
            return !isProductAvailable("AdFree");
        }

        static public bool IsStylesAvailable()
        {
             return isProductAvailable("AdFree");
        }

        static async public Task RemoveAd()
        {
            //_adfree = !_adfree;
            await BuyLicense("AdFree");
        }
    }
}
