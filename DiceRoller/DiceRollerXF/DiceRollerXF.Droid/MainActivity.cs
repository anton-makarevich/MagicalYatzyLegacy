using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Sanet.Kniffel.DicePanel;

namespace DiceRollerXF.Droid
{
    [Activity(Label = "DiceRollerXF", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        static MainActivity _instance;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            _instance = this;
            DicePanel.DeviceScale= Resources.DisplayMetrics.Density;
            LoadApplication(new App());
        }

        public static MainActivity Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}

