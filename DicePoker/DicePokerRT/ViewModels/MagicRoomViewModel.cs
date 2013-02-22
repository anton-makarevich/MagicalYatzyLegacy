
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
        public string MagicArtifactsDescription
        {
            get
            {
                return "MagicArtifactsDescription".Localize();

            }
        }
        public string HowToGetDescription
        {
            get
            {
                return "HowToGetDescription".Localize();

            }
        }

        public string PurchaseDescription
        {
            get
            {
                return "PurchaseDescription".Localize();

            }
        }
        public string MagicRollLabel
        {
            get
            {
                return "MagicRollLabel".Localize()+".";

            }
        }
        public string ForthRollLabel
        {
            get
            {
                return "ForthRollLabel".Localize() + ".";

            }
        }
        public string ManualSetLabel
        {
            get
            {
                return "ManualSetLabel".Localize() + ".";

            }
        }
        public string MagicRollDescription
        {
            get
            {
                return "MagicRollDescription".Localize();

            }
        }
        public string ManualSetDescription
        {
            get
            {
                return "ManualSetDescription".Localize();

            }
        }
        public string RerollDescription
        {
            get
            {
                return "RerollDescription".Localize();

            }
        }
        public string FirstTimeBonusDescription
        {
            get
            {
                return "FirstTimeBonusDescription".Localize();

            }
        }
        public string FirstTimeBonusLabel
        {
            get
            {
                return "FirstTimeBonusLabel".Localize() + ".";

            }
        }
        public string ResultBonusDescription
        {
            get
            {
                return "ResultBonusDescription".Localize();

            }
        }
        public string ResultBonusLabel
        {
            get
            {
                return "ResultBonusLabel".Localize() + ".";

            }
        }
        public string PurchaseMiniLabel
        {
            get
            {
                return "PurchaseMiniLabel".Localize();

            }
        }
        public string PurchaseMiniDescription
        {
            get
            {
                return "PurchaseMiniDescription".Localize();

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
                        if (await StoreManager.BuyLicense("WizardTools30"))
                        {
                            RoamingSettings.SetMagicRollsCount(CurrentPlayer, RoamingSettings.GetMagicRollsCount(CurrentPlayer) + 30);
                            RoamingSettings.SetManualSetsCount(CurrentPlayer, RoamingSettings.GetManualSetsCount(CurrentPlayer) + 30);
                            RoamingSettings.SetForthRollsCount(CurrentPlayer, RoamingSettings.GetForthRollsCount(CurrentPlayer) + 30);
                            var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                            var res = await ks.AddPlayersMagicsAsync(CurrentPlayer.Name, CurrentPlayer.Password.Encrypt(33), "30".Encrypt(33), "30".Encrypt(33), "30".Encrypt(33));
                            if (!res.Body.AddPlayersMagicsResult)
                            {
                                Utilities.ShowMessage("PurchaseErrorMessage".Localize(), "ErrorLabel".Localize());
                                LogManager.Log(LogLevel.Error, "Purchase30", "Can't send info to server for user {0}", CurrentPlayer.Name);
                            }

                        }
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
                       if (await StoreManager.BuyLicense("WizardTools50"))
                       {
                           RoamingSettings.SetMagicRollsCount(CurrentPlayer, RoamingSettings.GetMagicRollsCount(CurrentPlayer) + 50);
                           RoamingSettings.SetManualSetsCount(CurrentPlayer, RoamingSettings.GetManualSetsCount(CurrentPlayer) + 50);
                           RoamingSettings.SetForthRollsCount(CurrentPlayer, RoamingSettings.GetForthRollsCount(CurrentPlayer) + 50);
                           var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                           var res = await ks.AddPlayersMagicsAsync(CurrentPlayer.Name, CurrentPlayer.Password.Encrypt(33), "50".Encrypt(33), "50".Encrypt(33), "50".Encrypt(33));
                           if (!res.Body.AddPlayersMagicsResult)
                           {
                               Utilities.ShowMessage("PurchaseErrorMessage".Localize(), "ErrorLabel".Localize());
                               LogManager.Log(LogLevel.Error, "Purchase50", "Can't send info to server for user {0}", CurrentPlayer.Name);
                           }

                       }
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
                       if (await StoreManager.BuyLicense("WizardTools100"))
                       {
                           RoamingSettings.SetMagicRollsCount(CurrentPlayer, RoamingSettings.GetMagicRollsCount(CurrentPlayer) + 100);
                           RoamingSettings.SetManualSetsCount(CurrentPlayer, RoamingSettings.GetManualSetsCount(CurrentPlayer) + 100);
                           RoamingSettings.SetForthRollsCount(CurrentPlayer, RoamingSettings.GetForthRollsCount(CurrentPlayer) + 100);
                           var ks = new DicePokerRT.KniffelLeaderBoardService.KniffelServiceSoapClient();
                           var res= await ks.AddPlayersMagicsAsync(CurrentPlayer.Name, CurrentPlayer.Password.Encrypt(33), "100".Encrypt(33), "100".Encrypt(33), "100".Encrypt(33));
                           if (!res.Body.AddPlayersMagicsResult)
                           {
                               Utilities.ShowMessage("PurchaseErrorMessage".Localize(), "ErrorLabel".Localize());
                               LogManager.Log(LogLevel.Error, "Purchase100", "Can't send info to server for user {0}", CurrentPlayer.Name);
                           }
                           
                       }
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
