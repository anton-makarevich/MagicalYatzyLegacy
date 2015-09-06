using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Services
{
    public interface IActionsService
    {
        void NavigateToUrl(string url);
        void SendEmail(string to, string topic, string body);
        void RateApp(string appName);

        void ShowOtherApps(string parametr);
    }
}
