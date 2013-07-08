using Sanet.Kniffel;
using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.Web;

namespace Sanet.Network.Protocol
{
    public class TCPCommunicator
    {
        public event EventHandler MessageReceived;

        //connection events
        //notify that we lost connection and can't reconnect
        public event Action ConnectionLost;
        //notify connection lost and we try to reconnect
        public event Action Disconnected;
        //notify connection established
        public event Action Connected;
        

        public MessageWebSocket ClientWebSocket;

        protected bool m_IsConnected;

        Uri ServerUri(string id,bool isreconnect)
        {
           
            string serveruri=@"ws://" + Config.GetHostName();
            //@"ws://pksvc45.cloudapp.net/
            string fulluri = serveruri + "app.ashx";
            //var p = RoamingSettings.GetLastPlayer(0).Player;
            fulluri += "?playerId=" + id;

            if (isreconnect)
                fulluri += "&reconnect=1";
            else
                fulluri += "&reconnect=0";
            
            LogManager.Log(LogLevel.Message, "TCPCommRT.ServerUri", "Server uri is {0}", fulluri);
            return new Uri(fulluri);
        }
        

        //Queue<string> unsend = new Queue<string>();

        public TCPCommunicator()
        {
            m_IsConnected = false;
        }

        public virtual bool IsConnected
        {
            get
            {
                return ClientWebSocket!= null;//not sure about this
            }
        }

        //TODO fix this
        public virtual void SetIsConnected()
        {
            // if( m_Socket != null )
            //   m_IsConnected = true;
        }
        string _lastId= string.Empty;
        public async Task<bool> ConnectAsync(string id, bool isreconnect = false)
        {
            //await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            //{
                try
                {
                    LogManager.Log(LogLevel.Message, "TCPCommunicatorRT.ConnectAsync", "Trying to connect, reconnect:{0}", isreconnect);
                    // Make a local copy to avoid races with Closed events.
                    MessageWebSocket webSocket = ClientWebSocket;

                    // Have we connected yet?
                    if (!IsConnected)
                    {
                        _lastId = id;
                        var uri = ServerUri(id,isreconnect);
                        webSocket = new MessageWebSocket();
                        webSocket.Control.MessageType = SocketMessageType.Utf8;
                        // Set up callbacks
                        webSocket.MessageReceived += Receive;
                        webSocket.Closed += webSocket_Closed;
                        
                        await webSocket.ConnectAsync(uri);
                        ClientWebSocket = webSocket; // Only store it after successfully connecting.
                        LogManager.Log(LogLevel.Message, "TCPCommunicatorRT.ConnectAsync", "Connected successfully, reconnect: {0}", isreconnect);

                        if (Connected != null)
                            Connected();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex) // For debugging
                {

                    //WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                    //if (status == WebErrorStatus.PreconditionFailed)
                    //    Utils.ShowMessage(ex.Message);
                    
                    LogManager.Log("TCPCommunicatorRT.ConnectAsync", ex);
                    return false;
                    //throw;
                    
                }
            
        }

        void webSocket_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            LogManager.Log(LogLevel.Message, "TCPCommunicatorRT.webSocket_Closed", "Websocket connection was closed");
                        
            var socket = (MessageWebSocket)sender;
            if (socket != null)
            {
                try
                {
                    
                    socket.Dispose();
                    socket = null;
                    //if (ConnectionLost != null)
                    //    ConnectionLost();
                }
                catch (Exception ex)
                {
                    LogManager.Log("TCPCommunicatorRT.webSocket_Closed", ex);
                }
            }

        }

        private void Receive(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                using (DataReader reader = args.GetDataReader())
                {
                    reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;

                    string read = reader.ReadString(reader.UnconsumedBufferLength);
                    LogManager.Log(LogLevel.Message, "TCPCommunicatorRT.Receive", "data:{0}", read);
              
                    if (MessageReceived != null) MessageReceived(read, null);
                }
            }
            catch (Exception ex) 
            {
                OnReceiveCrashed(ex);

            }
        }
        /// <summary>
        /// Do not use this
        /// </summary>
        /// <returns></returns>
        public virtual bool Connect()
        {

            try
            {
                Task t = ConnectAsync(_lastId);
                t.Wait();

                return true;
            }
            catch (Exception e)
            {
                LogManager.Log(LogLevel.Error, "TCPCommunicatorRT.Connect", "Error on connect: {0}", e);
                CloseConnections();
                return false;
            }
        }

        private void CloseConnections()
        {
            try
            {
                if (ClientWebSocket != null)
                {
                    ClientWebSocket.Close(1000, "Closed due to user request.");
                    ClientWebSocket = null;
                    //notify we disconnected
                    if (Disconnected != null)
                        Disconnected();
                }
                
            }
            catch (Exception ex)
            {
                LogManager.Log("TCPCommunicator.CloseConnections", ex);
            }
        }
        
        protected virtual void Send(string line)
        {
            Task t = SendData(line);
        }
               
        private async Task SendData(string line)
        {
            if (ClientWebSocket != null)
            {
                // Send the data as one complete message.
                try
                {
                    
                    await ClientWebSocket.OutputStream.WriteAsync(getBufferFromString(line));
                    LogManager.Log(LogLevel.Message, "TCPCommunicatorRT.SendData", "data:{0}", line);

                }
                catch (Exception ex)
                {
                    //unsend.Enqueue(line);
                    OnSendCrashed(ex);
                }
            }
            else
            {
             //   unsend.Enqueue(line);
            }
        }
        private IBuffer getBufferFromString(string str)
        { 
            using (var dw = new DataWriter()) 
            {
                dw.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                dw.WriteString(str); 
                return dw.DetachBuffer(); 
            } 
        }

             
        public virtual void OnReceiveCrashed(Exception e)
        {
            connectionError(e);
        }
        public virtual void OnSendCrashed(Exception e)
        {
            connectionError(e);
        }

        async void connectionError(Exception e)
        {
            //return; //uncomment this to disable reconnection
            WebErrorStatus status = WebSocketError.GetStatus(e.GetBaseException().HResult);
            LogManager.Log("TCPCommunicator.ConnectionError", e);
            if (status == WebErrorStatus.CannotConnect
                || status == WebErrorStatus.ConnectionAborted)//add more reasons
            {
                Close();

                int reconnectionCounter = 0;

                int waitTime = 1 * 1000;
                //looping trying to recconect
                while (!IsConnected)
                {
                    try
                    {
                        new System.Threading.ManualResetEvent(false).WaitOne(waitTime);//wait a little
                        //Task.Delay(waitTime).Wait();
                    }
                    catch (Exception ex)
                    {
                        LogManager.Log("TCPRT.waitInReconnect", ex);
                    }
                    await ConnectAsync(_lastId, true);
                    reconnectionCounter++;
                    if (reconnectionCounter > 3)
                    {
                        //we reached max reconnection attempts -go to lobby
                        if (ConnectionLost != null)
                            ConnectionLost();
                        return;
                    }
                    if (waitTime < 10 * 10000)
                    {
                        waitTime = waitTime * 2;
                    }
                    else
                    {
                        waitTime = 2 * 1000;
                    }
                }
            }
        }
        
        //TODO fix this
        //TODO, can we  recover, that is, try to reconnect. 
        //in this case the game state should not broken. 
        public void Close()
        {
            CloseConnections();
            
        }
    }
}
