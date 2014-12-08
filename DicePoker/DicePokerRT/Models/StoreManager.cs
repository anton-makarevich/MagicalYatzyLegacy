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
            //return true;
            //return product == "WizardTools50";
            return (licenseInformation.ProductLicenses[product].IsActive);

            
        }
        static public async Task<bool> BuyLicense(string product)
        {
            if (!IsProductAvailable(product))
            {
                
                try
                {

                    //await CurrentAppSimulator.RequestProductPurchaseAsync(thisfolder, false);

                    var cert = await CurrentApp.RequestProductPurchaseAsync(product, true);
                    if (!string.IsNullOrEmpty(cert))
                    {
                        LogManager.Log(LogLevel.Message, "StoreManager.BuyLicense", "product '{0}' is purchased, your receipt is {1}", product, cert);
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    LogManager.Log("StoreManager.BuyLicense",ex);
                    return false;
                }
            }
            else
            {
                LogManager.Log(LogLevel.Message, "StoreManager.BuyLicense", "product '{0}' is already purchased", product);
                return false;
            }
            
        }

        static public bool IsAdVisible()
        {
            //return false;
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
