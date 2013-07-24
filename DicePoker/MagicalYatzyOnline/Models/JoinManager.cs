#if WinRT
using DicePokerRT;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif
using Sanet.Kniffel.Protocol;
using Sanet.Kniffel.Protocol.Commands.Game;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sanet.Kniffel.Models
{
    public static class JoinManager
    {
        
        //websocket 
        public static LobbyTCPClient WSServer= new LobbyTCPClient();

        public static void Init()
        {
            WSServer.Disconnected += WSServer_Disconnected;
        }

        static void WSServer_Disconnected()
        {
#if WINDOWS_PHONE
            //await JoinTable();
#endif
        }

        public async static void Reconnect()
        {
            if (!ViewModelProvider.HasViewModel<NewOnlineGameViewModel>() || !ViewModelProvider.GetViewModel<PlayGameViewModel>().IsOnlineGame)
                return;
            var p = ViewModelProvider.GetViewModel<NewOnlineGameViewModel>().SelectedPlayer;
            if (p==null)
                return;
            WSServer.Close();
            await WSServer.ConnectAsync(p.Player.ID, true);
            //Disconnect();
            //await JoinTable();
            
                //Utilities.ShowMessage("Disconnected");
                CurrentTable.Send(new PlayerPingCommand(CurrentTable.MyName));
                CurrentTable.Send(new TableInfoNeededCommand(CurrentTable.MyName));
                //CommonNavigationActions.NavigateToNewOnlineGamePage();
            
        }

        public static void Deactivate()
        {
            if (!ViewModelProvider.HasViewModel<NewOnlineGameViewModel>() || !ViewModelProvider.GetViewModel<PlayGameViewModel>().IsOnlineGame)
                return;
            var p = ViewModelProvider.GetViewModel<NewOnlineGameViewModel>().SelectedPlayer;
            if (p == null)
                return;
            
            CurrentTable.Send(new PlayerDeactivatedCommand(CurrentTable.MyName));
            
            //CommonNavigationActions.NavigateToNewOnlineGamePage();

        }

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
                var isOk =await WSServer.ConnectAsync(p.Player.ID);
                if (!isOk)
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        Utilities.ShowMessage("CantJoinMessage".Localize());

                    });
                    return false;
                }
            }

            
            
            KniffelGameClient game = WSServer.JoinTable(gameid,rule,p.Player,gui);
            if (game != null)
            {
                game.StartGame();

                CommonNavigationActions.NavigateToGamePage();

                CurrentTable = game;
                //game.JoinGame(p.Player);
                return true;
            }
            else
            {
                SmartDispatcher.BeginInvoke(() =>
                    {
                        Utilities.ShowMessage("CantJoinMessage".Localize());
                        
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

