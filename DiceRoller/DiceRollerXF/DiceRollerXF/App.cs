using Sanet.Kniffel.XF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DiceRollerXF
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new RollerPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static string Version
        {
            get
            {
                return "1.1.0.0 (XF)";
            }
        }
    }
}
