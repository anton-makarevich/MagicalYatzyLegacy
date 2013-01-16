
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Media.Imaging;

namespace Sanet.Kniffel.ViewModels
{
    public class AdBasedViewModel:BaseViewModel
    {
        #region Properties

        public bool IsAdVisible
        {
            get
            {
                return true;
            }
        }

        public bool IsStylesAvailable
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Methods

        public void NotifyAdChanged()
        {
            NotifyPropertyChanged("IsAdVisible");
            NotifyPropertyChanged("IsStylesAvailable");
        }

        #endregion

    }
}
