
using System.Threading;
using Android.App;
using Android.OS;

namespace DiceRollerXF.Droid
{
    /// <summary>
    /// Splash activity.
    /// </summary>
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            StartActivity(typeof(MainActivity));
        }
    }
}

