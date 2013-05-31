
using DicePokerRT;
using DicePokerRT.KniffelLeaderBoardService;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Protocol;
using Sanet.Kniffel.WebApi;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Sanet.Kniffel.ViewModels
{
    public class FacebookViewModel: AdBasedViewModel
    {
               
   
        #region Constructor
        public FacebookViewModel()
            :base()
        {
            CreateCommands();
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
                return Messages.NEW_ONLINE_GAME_START.Localize();
            }
        }
        
        

        #endregion

        #region Methods

        
        #endregion

        #region Commands
       
        //public RelayCommand StartCommand { get; set; }
        
        protected void CreateCommands()
        {
            
            //StartCommand = new RelayCommand(o => StartGame(), () => true);
        }



        #endregion


    }
}
