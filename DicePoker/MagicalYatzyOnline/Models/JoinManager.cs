
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

        public static async Task<bool> JoinTable(int gameid, Rules rule)
        {


            

            if (ViewModelProvider.HasViewModel<PlayGameViewModel>())
            {
                var vm = ViewModelProvider.GetViewModel<PlayGameViewModel>();
                vm.Dispose();
                vm = null;
                
            }
            
            var gui = ViewModelProvider.GetNewViewModel<PlayGameViewModel>();
            var p = ViewModelProvider.GetViewModel<NewOnlineGameViewModel>().SelectedPlayer;

            if (!WSServer.IsConnected)
            {
                await WSServer.ConnectAsync(p.Player.ID);
                //WSServer.RefreshUserInfo(dataProvider.CurrentUserInfo.DisplayName);
            }

            
            
            KniffelGameClient game = WSServer.JoinTable(gameid,rule,p.Player,gui);
            if (game != null)
            {
                game.StartGame();
                ((Frame)Window.Current.Content).Navigate(typeof(GamePage));
                CurrentTable = game;
                //game.JoinGame(p.Player);
                return true;
            }
            else
            {
                SmartDispatcher.BeginInvoke(() =>
                    {
                        Utilities.ShowMessage("Sorry, we can't join right now. Please try again later.");
                        
                    });
                return false;
            }
        }
        public static async Task<bool> JoinTable()
        {
            return await JoinTable(CurrentTable.GameId, CurrentTable.Rules.Rule);
        }

        public static void Disconnect()
        {
            if (CurrentTable == null)
                return;
            CurrentTable.Disconnect();
            WSServer.Close();
        }

        }
}

