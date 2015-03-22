using Microsoft.Phone.Controls;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Models
{
    public static class WPBasePageExtensions
    {
        
        #region ViewModel
        public static void SetViewModel<T>(this PhoneApplicationPage page) where T : BaseViewModel
        {
            page.DataContext = ViewModelProvider.GetViewModel<T>();
            
        }

        public static T GetViewModel<T>(this PhoneApplicationPage page) where T : BaseViewModel
        {
            return (T)page.DataContext;
        }

        #endregion

    }
}
