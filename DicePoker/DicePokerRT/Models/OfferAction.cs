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
    public class OfferAction:MainMenuAction
    {
       
        #region Properties

        public override string Label
        {
            get
            {
                return string.Format("{0} {1} {2}", Amount, "ArtifactsForLabel".Localize(),Cost);
            }
            set { }
        }

        private string _Cost;
        public string Cost
        {
            get { return _Cost; }
            set
            {
                if (_Cost != value)
                {
                    _Cost = value;
                    NotifyPropertyChanged("Cost");
                }
            }
        }
        
        private string _Amount;
        public string Amount
        {
            get 
            {
                return _Amount;
            }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        
        private string _Discount;
        public string Discount
        {
            get { return _Discount; }
            set
            {
                if (_Discount != value)
                {
                    _Discount = value;
                    NotifyPropertyChanged("Discount");
                    NotifyPropertyChanged("HasDiscount");
                }
            }
        }

        public bool HasDiscount
        {
            get
            {
                return !string.IsNullOrEmpty(Discount);
            }
        }
        
        private string _OfferID;
        public string OfferID
        {
            get { return _OfferID; }
            set
            {
                if (_OfferID != value)
                {
                    _OfferID = value;
                    NotifyPropertyChanged("OfferID");
                    NotifyPropertyChanged("IsAvailable");
                }
            }
        }

        
        public bool IsAvailable
        {
            get
            {
                if (!string.IsNullOrEmpty(OfferID))
                    return !StoreManager.IsProductAvailable(OfferID);
                return false;
            }
            
        }


        #endregion

        public void  RefreshIsAvailable()
        {
            NotifyPropertyChanged("IsAvailable");
        }
    }
}
