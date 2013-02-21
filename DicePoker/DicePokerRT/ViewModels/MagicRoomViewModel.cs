
using DicePokerRT.KniffelLeaderBoardService;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;

namespace Sanet.Kniffel.ViewModels
{
    public class MagicRoomViewModel : AdBasedViewModel
    {
        #region Constructor
        public MagicRoomViewModel()
        {
            CreateCommands();
            FillOffers();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Page title
        /// </summary>
        public string Title
        {
            get
            {
                var title="MagicRoomLabel".Localize();
                if (CurrentPlayer != null)
                    title += " (" + CurrentPlayer.Name+")";
                return title;
            }
        }

        
        private Player _CurrentPlayer;
        public Player CurrentPlayer
        {
            get { return _CurrentPlayer; }
            set
            {
                if (_CurrentPlayer != value)
                {
                    _CurrentPlayer = value;
                    NotifyPropertyChanged("CurrentPlayer");
                    NotifyPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Page title
        /// </summary>
        public string ArtifactsLabel
        {
            get
            {
                return  "ArtifactsLabel".Localize();
                
            }
        }

        public string HowGetLabel
        {
            get
            {
                return "HowGetLabel".Localize();

            }
        }

        public string PurchaseLabel
        {
            get
            {
                return "PurchaseLabel".Localize();

            }
        }


        private ObservableCollection<OfferAction> _Offers;
        public ObservableCollection<OfferAction> Offers
        {
            get { return _Offers; }
            set
            {
                if (_Offers != value)
                {
                    _Offers = value;
                    NotifyPropertyChanged("Offers");
                }
            }
        }


        #endregion

        #region Methods

        private void FillOffers()
        {
            Offers= new ObservableCollection<OfferAction>();
            Offers.Add(
                new OfferAction
                {
                    MenuAction = new Action(async () =>
                    {
                        await StoreManager.BuyLicense("WizardTools30");
                        RefreshOffers();
                    }),
                    Description = "Offer1Description",
                    OfferID = "WizardTools30",
                    Amount="90",
                   Cost="Cost1Label".Localize()
                    
                });
            Offers.Add(
               new OfferAction
               {
                   MenuAction = new Action(async () =>
                   {
                       await StoreManager.BuyLicense("WizardTools50");
                       RefreshOffers();
                   }),
                   Description = "Offer2Description",
                   OfferID = "WizardTools50",
                   Amount = "150",
                   Cost = "Cost2Label".Localize(),
                   Discount="-20%"

               });
            Offers.Add(
               new OfferAction
               {
                   MenuAction = new Action(async () =>
                   {
                       await StoreManager.BuyLicense("WizardTools100");
                       RefreshOffers();
                   }),
                   Description = "Offer3Description",
                   OfferID = "WizardTools100",
                   Amount = "300",
                   Cost = "Cost3Label".Localize(),
                   Discount = "-50%"

               });
            NotifyPropertyChanged("Offers");
        }

        void RefreshOffers()
        {
            foreach (OfferAction offer in Offers)
            {
                offer.RefreshIsAvailable();
            }
            NotifyPropertyChanged("Offers");
        }

        #endregion

        #region Commands
        
        protected void CreateCommands()
        {
            
        }



        #endregion


    }
}
