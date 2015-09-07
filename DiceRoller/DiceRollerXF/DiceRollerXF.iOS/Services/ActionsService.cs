using Foundation;
using MessageUI;
using Sanet.Kniffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ActionsService))]
namespace Sanet.Kniffel.Services
{
    public class ActionsService : IActionsService
    {
        public void NavigateToUrl(string url)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
        }

        public void RateApp(string appName)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
                UIApplication.SharedApplication.OpenUrl(new NSUrl("itms-apps://itunes.apple.com/app/id826095442"));
            else
                UIApplication.SharedApplication.OpenUrl(new NSUrl("itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=826095442"));
        }
        
        public void SendEmail(string to, string topic, string body)
        {
            try
            {
                var _emailComposerView = new MFMailComposeViewController();
                _emailComposerView.SetSubject("Sanet Dice (iOS)");
                _emailComposerView.SetToRecipients(new string[] { to });

                _emailComposerView.Finished += (sender, e) =>
                {
                    e.Controller.InvokeOnMainThread(() =>
                    {
                        e.Controller.DismissViewController(true, null);
                    });
                };
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(_emailComposerView, true, null);
            }
            catch { }
        }

        public void ShowOtherApps(string parametr)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/artist/anton+makarevich/id826095445?ign-mpt=uo%3D4"));
        }
    }
}