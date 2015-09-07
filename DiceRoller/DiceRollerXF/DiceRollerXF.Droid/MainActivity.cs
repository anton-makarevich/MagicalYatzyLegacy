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
using Android.Hardware;
using DiceRollerXF.Models;

namespace DiceRollerXF.Droid
{
    [Activity(Label = "Sanet Dice", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, Android.Hardware.ISensorEventListener
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

            // Register this as a listener with the underlying service.
            var sensorManager = GetSystemService(SensorService) as Android.Hardware.SensorManager;
            var sensor = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Accelerometer);
            sensorManager.RegisterListener(this, sensor, Android.Hardware.SensorDelay.Game);
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
        #region ShakeDetection
        bool hasUpdated = false;
        DateTime lastUpdate;
        float last_x = 0.0f;
        float last_y = 0.0f;
        float last_z = 0.0f;

        const int ShakeDetectionTimeLapse = 250;
        const double ShakeThreshold = 800;
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == Android.Hardware.SensorType.Accelerometer)
            {
                float x = e.Values[0];
                float y = e.Values[1];
                float z = e.Values[2];

                DateTime curTime = System.DateTime.Now;
                if (hasUpdated == false)
                {
                    hasUpdated = true;
                    lastUpdate = curTime;
                    last_x = x;
                    last_y = y;
                    last_z = z;
                }
                else
                {
                    if ((curTime - lastUpdate).TotalMilliseconds > ShakeDetectionTimeLapse)
                    {
                        float diffTime = (float)(curTime - lastUpdate).TotalMilliseconds;
                        lastUpdate = curTime;
                        float total = x + y + z - last_x - last_y - last_z;
                        float speed = Math.Abs(total) / diffTime * 10000;

                        if (speed > ShakeThreshold)
                        {
                            MotionHelper.ShakeNotify();
                        }

                        last_x = x;
                        last_y = y;
                        last_z = z;
                    }
                }
            }
        }
        #endregion
    }
}

