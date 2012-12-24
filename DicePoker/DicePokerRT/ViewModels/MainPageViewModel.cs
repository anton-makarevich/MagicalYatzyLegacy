using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.ViewModels
{
    public class MainPageViewModel:BaseViewModel
    {

#region Constructor
        public MainPageViewModel()
        {
            MenuActions = new List<MainMenuAction>();
            MenuActions.Add(
                new MainMenuAction
                {
                    Label="NewLocalGameAction",
                    MenuAction=new Action(()=>
                        {
                            Utilities.ShowToastNotification("NewGameClicked");
                        })
                });
        }

#endregion
#region Properties


        private List<MainMenuAction> _MenuActions;
        public List<MainMenuAction> MenuActions
        {
            get { return _MenuActions; }
            set
            {
                if (_MenuActions != value)
                {
                    _MenuActions = value;
                    NotifyPropertyChanged("MenuActions");
                }
            }
        }
        
#endregion

    }
}
