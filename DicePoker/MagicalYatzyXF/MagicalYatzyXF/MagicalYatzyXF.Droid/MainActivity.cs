using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MagicalYatzyXF.Droid
{
    [Activity(Label = "MagicalYatzyXF", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        static MainActivity _instance;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _instance = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            App.DeviceScale = Resources.DisplayMetrics.Density;
            LoadApplication(new App());
        }

        public static MainActivity Instance { get { return _instance; } }
    }
}

