using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Localization;
using Android.Content;
using Sanet.Kniffel.Utils;

namespace DiceRollerXF.Droid
{
    [Activity(Label = "Sanet Dice", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        static MainActivity _instance;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _instance = this;
            
            LocalizerExtensions.Initialize(new ResourceModel(new string[] { "en", "ru", "de" }));
            global::Xamarin.Forms.Forms.Init(this, bundle);
            DialogsHelper.Init(new Acr.UserDialogs.UserDialogsImpl(GetActivity));
            DicePanel.DeviceScale= Resources.DisplayMetrics.Density;
            LoadApplication(new App());
        }

        Activity GetActivity()
        {
            return this;
        }

        public static MainActivity Instance
        {
            get
            {
                return _instance;
            }
        }

        public void RateApp(string app)
        {
            var uri = Android.Net.Uri.Parse("market://details?id="+app);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
        public void NavigateToUrl(string url)
        {
            var uri = Android.Net.Uri.Parse(url);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                var email = new Intent(Intent.ActionSend);
                email.PutExtra(Android.Content.Intent.ExtraEmail,
                    new string[] { to });

                email.PutExtra(Intent.ExtraSubject, "Sanet Dice (Android)");


                email.SetType("message/rfc822");

                StartActivity(email);
            }
            catch { }
        }

        public void ShowMyApps(string author)
        {
            var uri = Android.Net.Uri.Parse("market://search?q=pub:" + author);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
    }
}

