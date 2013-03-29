using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace Sanet.Kniffel.Server
{
    public class ClientInfo
    {
        public ServerClientLobby ClientLobby { get; set; }
        public WebSocket Socket { get; set; }
    }

    

    public class ClientRequestHandler : IHttpHandler
    {
        /// <summary>
        /// I want to simulate disconnection
        /// </summary>
        System.Timers.Timer _disconnectTimer;
        /// <summary>
        /// dummy port
        /// </summary>
        public static ServerLobby ServerLobby = new ServerLobby(1000);
        private static List<ClientInfo> clientList = new List<ClientInfo>();

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {//server
            if (context.IsWebSocketRequest)
            {
                
                context.AcceptWebSocketRequest(new Func<AspNetWebSocketContext, Task>(MyWebSocket));
                      
            }
            else
            {
                if (context.Request.QueryString.Count == 2 && context.Request.QueryString[0].ToLower() == "tileupdate")
                {
                    
                        return;
                    
                }
                context.Response.Output.Write("dfq le . cb pbc? fq rfyn fylthcn'yl...  http://sanet.by");
            }

        }

        public async Task MyWebSocket(AspNetWebSocketContext context)
        {
            //sim suddenly disconnection
            if (_disconnectTimer == null)
            {
                _disconnectTimer = new System.Timers.Timer { Interval = 51 * 1000 };
                _disconnectTimer.Elapsed += _disconnectTimer_Elapsed;
                //_disconnectTimer.Start();
            }

            string playerId = context.QueryString["playerId"];
            string version = context.QueryString["version"];
            if (playerId == null) playerId = string.Empty;

            
            string reconnect = context.QueryString["reconnect"];
            if (reconnect == null) reconnect = "0";
          
            //tried retry logic but it caused other issues. 
                try
                {
                    WebSocket socket = context.WebSocket;
                    ServerClientLobby clientLobby = null;

                    if (!string.IsNullOrEmpty(playerId))
                    {//check whether we have current instance of players
                        
                        if (reconnect != "1" || !ServerClientLobby.playerToServerClientLobbyMapping.TryGetValue(playerId, out clientLobby))
                        {
                            clientLobby = new ServerClientLobby(ServerLobby, playerId);
                            ServerClientLobby.playerToServerClientLobbyMapping[playerId] =  clientLobby;
                            LogManager.Log("ClientRequestHandler.MyWebSocket", "websocket connect request from {0} ), created new clientLobby", playerId);
                        }
                        else
                        {
                            //Anton: not sure wheather commented part with reconnection check is needed after disposing of old GameServer, maybe not
                            //if (!string.IsNullOrEmpty(isreconnect) && isreconnect == "1")
                            LogManager.Log("ClientRequestHandler.MyWebSocket", "websocket connect request from {0}), used reconnect mode and existing clientLobby", playerId);
                        }
                        

                    }
                    else 
                    {
                        LogManager.Log("ClientRequestHandler.MyWebSocket", "websocket connect request with empty playerId");
                    }

                    //this is not adding bots or winform clients to the list
                    if (clientLobby == null)
                    {
                        clientLobby = new ServerClientLobby(ServerLobby, String.Empty);
                    }

                    //this will reset websocket
                    clientLobby.WebSocket = socket;
                    await clientLobby.StartInAspMode();

                    LogManager.Log("ClientRequestHandler.MyWebSocket", "StartInAspMode returned for player:{0}", playerId);

                }
                catch (Exception ex)
                {
                    LogManager.Log(LogLevel.ErrorHigh, "ClientRequestHandler.MyWebSocket", "websocket connect request from {0} ) threw exception:{1}", playerId, ex);
                }


        }
        //force client disconnection for testing of client disconnection
        void _disconnectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //
//            
            //if (clientList.Count > 0)
            //{
                //var client = clientList[0];
                var client = ServerClientLobby.playerToServerClientLobbyMapping.Values./*Where(f => f.ClientLobby.PlayerName=="Anton").*/FirstOrDefault();
                if (client != null && client.IsConnected && client.WebSocket != null)
                {
                    client.WebSocket.Abort();

                }
            //}
        }

        
    }
}
   
    
    