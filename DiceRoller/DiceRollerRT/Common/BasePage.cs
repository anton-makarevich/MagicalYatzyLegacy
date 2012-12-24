using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sanet.Views
{
    public class BasePage:Page
    {
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the group to be displayed.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Returning to a cached page through navigation shouldn't trigger state loading
           // if (this._pageKey != null) return;
            this.LoadState(e.Parameter, null);
            
        }

        protected virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }
        #region ViewModel
        public void SetViewModel<T>(T viewModel) where T : BaseViewModel
        {
            DataContext = viewModel;
            
        }

        public T GetViewModel<T>() where T : BaseViewModel
        {
            return (T)DataContext;
        }

        #endregion
    }
}
