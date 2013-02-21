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
        static public bool IsProductAvailable(string product)
        {
            //return true;//_adfree;
            return (licenseInformation.ProductLicenses[product].IsActive);

            
        }
        static public async Task BuyLicense(string product)
        {
            if (!IsProductAvailable(product))
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
            return !IsProductAvailable("AdFree");
        }

        static public bool IsStylesAvailable()
        {
             return IsProductAvailable("AdFree");
        }

        static async public Task RemoveAd()
        {
            //_adfree = !_adfree;
            await BuyLicense("AdFree");
        }
    }
}
