using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanet.Models;
using System.Threading.Tasks;

#if WinRT
using Windows.UI.Xaml.Media;
#else
using System.Windows.Media;
#endif

namespace Sanet.Kniffel.Models
{
    public class MainMenuAction:BaseViewModel
    {
        #region Fields
        public Action MenuAction;
        #endregion
        #region Properties
        /// <summary>
        /// Action Id - string suitable to be a key for resource file
        /// </summary>
        private string _Label;
        virtual public string Label
        {
            get
            { 
                return _Label;
            }
            set
            {
                if (_Label != value)
                {
                    _Label = value;
                    NotifyPropertyChanged("Label");
                    NotifyPropertyChanged("LocalizedLabel");
                }
            }
        }
        /// <summary>
        /// Localized string for menu item caption
        /// </summary>
        public string LocalizedLabel
        {
            get
            {
                return _Label.Localize().ToUpper();
            }
        }

        
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                    NotifyPropertyChanged("LocalizedDescription");
                }
            }
        }
        public string LocalizedDescription
        {
            get
            {
                return _Description.Localize();
            }
        }

        private ImageSource _Image;
        public ImageSource Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    NotifyPropertyChanged("Image");
                }
            }
        }
        #endregion
    }
}
