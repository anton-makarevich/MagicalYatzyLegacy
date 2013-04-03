using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WinRT
using Windows.UI.Xaml.Controls;
#else
using System.Windows.Controls;
#endif

namespace Sanet.Models
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BaseViewModel() { }
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }


        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                if (_IsBusy != value)
                {
                    _IsBusy = value;
                    NotifyPropertyChanged("IsBusy");
                }
            }
        }

        //sound
        protected MediaElement _player = new MediaElement();
        public void Dispose()
        {
            _player = null;
        }
    }
}
