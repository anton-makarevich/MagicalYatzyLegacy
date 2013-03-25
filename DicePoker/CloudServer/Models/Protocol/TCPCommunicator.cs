
using System.IO;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Net.WebSockets;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections;
using Sanet.Models;



namespace Sanet.Network.Protocol
{
    /// <summary>
    /// class used by server & winform client
    /// Win8 client uses different file
    ///server reconnection implementation: As soon as the exception is thrown in recieve or send, resources are cleaned up
    /// </summary>
    public class TCPCommunicator
    {
        //used only by server
        private WebSocket _webSocket;
        private CancellationTokenSource _cancellationTokenSrc;
        public WebSocket WebSocket
        {
            get { return _webSocket; }
            set 
            {
                Task t = null;
                try
                {

                     //TODO comment for testing
                    if (_webSocket != null && _webSocket.State == WebSocketState.Open)
                    {
                        if (_cancellationTokenSrc != null)
                        {
                            _cancellationTokenSrc.Cancel();
                        }

                        t = _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing as client thinks there is no more connection", CancellationToken.None);
                        
                        //let the websocket close existing connection & continue
                        //dispose is not really required to be called

                    }
                }
                    //websockect already disposed exception
                    //null ref exception
                catch { }

                _webSocket = value;
                _cancellationTokenSrc = new CancellationTokenSource();
                //not really need to wait
              //  t = SendAllQueuedMessages();
            }
        }

        //used only by client
        public ClientWebSocket ClientWebSocket { get; set; }
 
        protected bool m_IsConnected;

        //only used by winform client
        //Uri ServerUri = new Uri(@"ws://localhost/app.ashx");
#if !DEBUG
        //http://kpkr.cloudapp.net/
        Uri ServerUri = new Uri(@"ws://kpkr.cloudapp.net/app.ashx");
#endif

#if DEBUG
        Uri ServerUri = new Uri(@"ws://localhost:8643/app.ashx");
#endif
        public TCPCommunicator()
        {
            m_IsConnected = false;
            messagesToSend = Queue.Synchronized(new Queue());
        }

        public virtual bool IsConnected
        {
            get
            {
                try
                {
                    if (ClientWebSocket != null)
                    {
                        return ClientWebSocket.State == WebSocketState.Open;
                    }
                    if (WebSocket != null)
                    {
                        return WebSocket.State == WebSocketState.Open;
                    }
                }
                 //can throw null ref
                    //dispose object exception
                catch { }

                return false;

            }
        }
        
        //TODO fix this
        public virtual void SetIsConnected()
        {
           // if( m_Socket != null )
             //   m_IsConnected = true;
        }

        public async Task ConnectAsync()
        {

            ClientWebSocket = new ClientWebSocket();
            _cancellationTokenSrc = new CancellationTokenSource();
            await ClientWebSocket.ConnectAsync(ServerUri, _cancellationTokenSrc.Token);
        }

        public virtual bool Connect()
        {
            
            try
            {
                Task t = ConnectAsync();
                t.Wait();

                return true;
            }
            catch (Exception e)
            {
                LogManager.Log(LogLevel.Error, "TCPCommunicator.Connect", "Error on connect: {0}", e);
                //CleanupConnections();
                return false;
            }
        }

        private AsyncLock _asyncLock = new AsyncLock();
        private async Task CloseConnections()
        {
            Console.WriteLine("TCpCommunicator.CloseConnections, ...");
            if (WebSocket != null || ClientWebSocket != null)
            {
                using (await _asyncLock.LockAsync())
                {
                    WebSocket socket = _webSocket;
                    _webSocket = null;

                    ClientWebSocket clientSocket = ClientWebSocket;
                    ClientWebSocket = null;
                    try
                    {
                        if (socket != null && socket.State == WebSocketState.Open)
                        {
                            _cancellationTokenSrc.Cancel();
                            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
                            //   WebSocket.Dispose(); throws exception later 
                        }
                        else if (clientSocket != null && clientSocket.State == WebSocketState.Open)
                        {
                            _cancellationTokenSrc.Cancel();
                            await clientSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing client", CancellationToken.None);
                            // ClientWebSocket.Dispose(); //throws exception later
                        }
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                    finally
                    {
                       
                    }
                }
            }
        }

        private async Task<string> GetDataString()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult result = null;

            if (WebSocket != null)
            {
                // Asynchronously wait for a message to arrive from a client
                result =
                    await WebSocket.ReceiveAsync(buffer, _cancellationTokenSrc.Token);
            }
            else
            {
                result = await ClientWebSocket.ReceiveAsync(buffer, _cancellationTokenSrc.Token);
            }

            if (result != null)
            {
                string userMessage = Encoding.UTF8.GetString(buffer.Array, 0,
                            result.Count);
                return userMessage;
            }

            return null;
        }

        /// <summary>
        /// hitesh: call this when a new req is recieved
        /// used by server & winform client
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<string> Receive() 
        {
            try
            {
                string line = await GetDataString();
                LogManager.Log(LogLevel.MessageVeryLow, "TCPCommunicator.Recieve", "data:{0}", line);
                //dataString.Wait();
                //string line =dataString.Result;
                //return line;
                return line;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> SendData(string line)
        {

            if ((WebSocket != null && WebSocket.State == WebSocketState.Open)
                || (ClientWebSocket != null && ClientWebSocket.State == WebSocketState.Open))
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

                buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(line));

                if (WebSocket != null)
                {
                    // Asynchronously send a message to the client
                    await WebSocket.SendAsync(buffer, WebSocketMessageType.Text,
                            true, _cancellationTokenSrc.Token);
                    LogManager.Log(LogLevel.MessageVeryLow, "TCPCommunicator.SendData.WebSocket", "data:{0}", line);
              
                    return true;
                }
                else
                {
                    await ClientWebSocket.SendAsync(buffer, WebSocketMessageType.Text,
                            true, _cancellationTokenSrc.Token);
                    LogManager.Log(LogLevel.MessageVeryLow, "TCPCommunicator.SendData.ClientWebSocket", "data:{0}", line);
                    return true;
                }
            }

            return false;
        }

        private Queue messagesToSend;
        private Queue ThreasSafeQueue { get { return messagesToSend; } }
        private ReaderWriterLockSlim slimLockForSendingQueuedMessages = new ReaderWriterLockSlim();
       

        protected virtual void Send(string line)
        {
            try
            {
                Task t = null;
                //let's be optimistic & only add in queue if exception occurs
                // potential race condition when socket state changes from open to something else & it calls
                if (messagesToSend.Count >= 1 || !IsConnected)
                {
                 //   messagesToSend.Enqueue(line);
                  //  t = SendAllQueuedMessages();
                    //return;
                }

                if (IsConnected)
                {
                    t = SendData(line);
                }
            }
            catch (OperationCanceledException) //normal expected
            { }
            catch (Exception e)
            {
                //CleanupConnections(); not required as recieve should also crash,
                OnSendCrashed(e);
            }
        }

        private int isRunning = 0;

       
        public async Task StartInAspMode()
        {
            if (Interlocked.CompareExchange(ref isRunning, 1, 0) == 0)
            {
                try
                {
                    await Run();
                }
                catch (OperationCanceledException) //expcted when task is cancelled
                { }
                finally
                {
                    Interlocked.Exchange(ref isRunning, 0);
                }
            }
        }

        private async void RunCallByThread()
        {
            try
            {
                await Run();
            }
            catch (OperationCanceledException) //expected when task is cancelled
            { }
        }

        
        /// <summary>
        /// this powers running of a client & server both
        /// on server if exception is thrown or when connections gets aborted unexpectedly this method exits
        /// but later StartInAspMode again fires this method
        /// </summary>
        protected virtual async Task Run()
        {
            LogManager.Log(LogLevel.Message, "TCPCommunicator.Run()", "Starting to listen");
            while ((WebSocket != null && WebSocket.State == WebSocketState.Open)
                || (ClientWebSocket != null && ClientWebSocket.State == WebSocketState.Open))
            {
                try
                {
                    string result = await Receive();
                    if (result == null)
                    {
                        return;
                    }

                }
                catch (OperationCanceledException) //normal expected
                { }
                catch (Exception e)
                {
                    CloseConnections();

                    OnReceiveCrashed(e);
                }
            }
            
        }

        public virtual void OnReceiveCrashed(Exception e)
        {
            LogManager.Log(LogLevel.Error, "TCPCommunicator.OnReceiveCrashed", "{0}: {1}", e.GetType(), e.Message);
            LogManager.Log(LogLevel.ErrorLow, "TCPCommunicator.OnReceiveCrashed", e.StackTrace); 
        }

        public virtual void OnSendCrashed(Exception e)
        {
            LogManager.Log(LogLevel.Error, "TCPCommunicator.OnSendCrashed", "{0}: {1}", e.GetType(), e.Message);
            LogManager.Log(LogLevel.ErrorLow, "TCPCommunicator.OnSendCrashed", e.StackTrace); 
        }

        //used by clients
        public void Start()
        {
            //StartInAspMode();
            new Thread(new ThreadStart(RunCallByThread)).Start();
        }

        //TODO fix this
        //TODO, can we recover, that is, try to reconnect. 
        //in this case the game state should not broken. 
        public async Task Close()
        {
            //remove user from clientServer
            //isnt required as server will send close call
            await CloseConnections();
        }
    }
}
