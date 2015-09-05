using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanet.Kniffel.XF.Models
{
    public class Storyboard
    {
        public event EventHandler Completed;

        public TimeSpan Duration { get; set; }

        public void Begin()
        {
            Device.StartTimer(Duration, OnTimer);
        }

        bool OnTimer()
        {
            if (Completed != null)
                Completed(null, null);
            return false;
        }
    }
}
