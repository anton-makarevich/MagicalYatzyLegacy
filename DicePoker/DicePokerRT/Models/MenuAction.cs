using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    public class MainMenuAction:BaseViewModel
    {
        #region Fields
        public Action MenuAction;
        #endregion
        #region Properties
        
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
                }
            }
        }

        #endregion
    }
}
