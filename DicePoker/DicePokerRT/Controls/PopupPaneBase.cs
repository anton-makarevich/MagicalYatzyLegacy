using Sanet.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

//Base for every popup control we use
namespace Sanet.Controls
{
    public class PopupPaneBase : BasePage, INotifyPropertyChanged
    {
        public PopupPaneBase()
        {
            var bounds = Window.Current.Bounds;
            this.Width = bounds.Width;
            this.Height = bounds.Height;
        }

        #region GeneralPopup


        private bool _IsOk = false;
        public bool IsOk
        {
            get { return _IsOk; }
            set
            {
                if (_IsOk != value)
                {
                    _IsOk = value;
                }
            }
        }


        /// <summary>
        /// got current popup, the simpliest is to get parent but that not working in some cases
        /// so more reliable to set popup to tag opject
        /// </summary>
        protected Popup parentPopup
        {
            get
            {
                return this.Tag as Popup;

            }
        }
        /// <summary>
        /// tapped on the "fogged" part
        /// </summary>
        protected void Cancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Cancel();
        }
        /// <summary>
        /// X button clicked
        /// </summary>
        protected void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        virtual protected void Cancel()
        {
            IsOk = false;
            parentPopup.IsOpen = false;
        }
        /// <summary>
        /// For visibility of progress ring 
        /// </summary>
        bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}
