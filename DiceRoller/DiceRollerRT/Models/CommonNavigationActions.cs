
using Sanet.Kniffel.DiceRoller;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sanet.Kniffel.Models
{
    public static  class CommonNavigationActions
    {
        public static Action NavigateToMainPage
        {
            get
            {
                return new Action(()=>
                    {
                        ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
                });
            }
            
        }
        public static Action NavigateToSanetDicePoker
        {
            get
            {
                return new Action(async () =>
                {
                    await Launcher.LaunchUriAsync(new Uri("http://apps.microsoft.com/windows/app/sanet-dice-poker/5b0f9106-65a8-49ca-b1f0-641c54a7e3ef"));
                });
            }
        }
    }
}
