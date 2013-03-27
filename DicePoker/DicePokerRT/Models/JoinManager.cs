
using DicePokerRT;
using Sanet.Kniffel.Protocol;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sanet.Kniffel.Models
{
    public static class JoinManager
    {
        
        //websocket 
        public static LobbyTCPClient WSServer= new LobbyTCPClient();


        public static KniffelGameClient CurrentTable;

        public static async Task<bool> JoinTable(KniffelGameClient tbl)
        {


            

            if (ViewModelProvider.HasViewModel<PlayGameViewModel>())
            {
                var vm = ViewModelProvider.GetViewModel<PlayGameViewModel>();
                vm.Dispose();
                vm = null;
                
            }

            if (!WSServer.IsConnected)
            {
                await WSServer.ConnectAsync();
                //WSServer.RefreshUserInfo(dataProvider.CurrentUserInfo.DisplayName);
            }

            var gui = ViewModelProvider.GetNewViewModel<PlayGameViewModel>();
            
            KniffelGameClient game = WSServer.JoinTable(tbl.GameId,  gui);
            if (game != null)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(GamePage), gui);
                CurrentTable = tbl;
                return true;
            }
            else
            {
                Utilities.ShowMessage("Sorry, we can't join this table right now. Please try again later.");
                return false;
            }
            }
        public static async Task<bool> JoinTable()
        {
            return await JoinTable(CurrentTable);
        }
           
        }
}

