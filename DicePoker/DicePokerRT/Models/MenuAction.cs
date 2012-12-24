using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanet.Models;
using System.Threading.Tasks;

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
        public string Label
        {
            get { return _Label; }
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
        #endregion
    }
}
