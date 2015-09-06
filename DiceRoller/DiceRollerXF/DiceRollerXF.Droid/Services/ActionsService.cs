using DiceRollerXF.Droid;
using Sanet.Kniffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(ActionsService))]
namespace Sanet.Kniffel.Services
{
    public class ActionsService : IActionsService
    {
        public void NavigateToUrl(string url)
        {
            MainActivity.Instance.NavigateToUrl(url);
        }

        public void RateApp(string appName)
        {
            MainActivity.Instance.RateApp(appName);
        }

        public void SendEmail(string to, string topic, string body)
        {
            MainActivity.Instance.SendEmail(to, topic, body);
        }

        public void ShowOtherApps(string parametr)
        {
            MainActivity.Instance.ShowMyApps(parametr);
        }
    }
}