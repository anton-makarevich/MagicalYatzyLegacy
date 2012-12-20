Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Description

Namespace Kniffel.Web
    ''' <summary>
    ''' Derive from this class to create a duplex Service Factory to use in an .svc file
    ''' </summary>
    ''' <typeparam name="T">The Duplex Service type (typically derived from DuplexService)</typeparam>
    Public MustInherit Class DuplexServiceFactory(Of T As {IUniversalDuplexContract, New})
        Inherits ServiceHostFactoryBase
        Private serviceInstance As New T()

        ''' <summary>
        ''' This method is called by WCF when it needs to construct the service.
        ''' Typically this should not be overridden further.
        ''' </summary>
        Public Overrides Function CreateServiceHost(ByVal constructorString As String, ByVal baseAddresses() As Uri) As ServiceHostBase
            Dim service As New ServiceHost(serviceInstance, baseAddresses)
            Dim binding As New CustomBinding(New PollingDuplexBindingElement(), New BinaryMessageEncodingBindingElement(), New HttpTransportBindingElement())

            service.Description.Behaviors.Add(New ServiceMetadataBehavior())
            service.AddServiceEndpoint(GetType(IUniversalDuplexContract), binding, "")
            service.AddServiceEndpoint(GetType(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex")
            Return service
        End Function
    End Class

    ''' <summary>
    ''' Derive your own Duplex service from this class
    ''' </summary>
    <ServiceBehavior(InstanceContextMode:=InstanceContextMode.Single)> _
    Public MustInherit Class DuplexService
        Implements IUniversalDuplexContract
        Friend syncRoot As New Object()
        Private clients As New Dictionary(Of String, IUniversalDuplexCallbackContract)()

        ''' <summary>
        ''' This will be called when a new client is connected
        ''' </summary>
        ''' <param name="sessionId">Session ID of the newly-connected client</param>
        Protected Overridable Sub OnConnected(ByVal sessionId As String)
        End Sub

        ''' <summary>
        ''' This will be called when a client is disconnected
        ''' </summary>
        ''' <param name="sessionId">Session ID of the newly-disconnected client</param>
        Protected Overridable Sub OnDisconnected(ByVal sessionId As String)
        End Sub
        Protected Overridable Sub JoinSession(ByVal sessionid As String, ByVal joinMessage As JoinSessionMessage)

        End Sub

        ''' <summary>
        ''' This will be called when a message is received from a client
        ''' </summary>
        ''' <param name="sessionId">Session ID of the client sending the message</param>
        ''' <param name="message">The message that was received</param>
        Protected Overridable Sub OnMessage(ByVal sessionId As String, ByVal message As DuplexMessage)
        End Sub

        ''' <summary>
        ''' Pushes a message to all connected clients
        ''' </summary>
        ''' <param name="message">The message to push</param>
        Protected Sub PushToAllClients(ByVal message As DuplexMessage)
            SyncLock syncRoot
                Dim CompletedKeys As New List(Of String)
BeginOnceAgain:
                Try
                    For Each session As String In clients.Keys
                        If Not CompletedKeys.Contains(session) Then
                            PushMessageToClient(session, message)
                            CompletedKeys.Add(session)
                        End If
                    Next session
                Catch ex As Exception
                    GoTo BeginOnceAgain
                End Try

            End SyncLock
        End Sub

        ''' <summary>
        ''' Pushes a message to one specific client
        ''' </summary>
        ''' <param name="clientSessionId">Session ID of the client that should receive the message</param>
        ''' <param name="message">The message to push</param>
        Protected Sub PushMessageToClient(ByVal clientSessionId As String, ByVal message As DuplexMessage)
            If Not clients.ContainsKey(clientSessionId) Then
                ClientDisconnected(clientSessionId)
                Exit Sub
            End If
            Try
                Dim ch As IUniversalDuplexCallbackContract = clients(clientSessionId)

                Dim iar As IAsyncResult = ch.BeginSendToClient(message, New AsyncCallback(AddressOf OnPushMessageComplete), New PushMessageState(ch, clientSessionId))
                If iar.CompletedSynchronously Then
                    CompletePushMessage(iar)
                End If
                'If Not iar.IsCompleted Then
                '    ClientDisconnected(clientSessionId)
                'End If
            Catch ex As Exception
                ClientDisconnected(clientSessionId)
            End Try

        End Sub

        Private Sub OnPushMessageComplete(ByVal iar As IAsyncResult)
            If iar.CompletedSynchronously Then
                Return
            Else
                CompletePushMessage(iar)
            End If
        End Sub

        Private Sub CompletePushMessage(ByVal iar As IAsyncResult)
            Dim ch As IUniversalDuplexCallbackContract = (CType(iar.AsyncState, PushMessageState)).ch
            Try
                ch.EndSendToClient(iar)
            Catch ex As Exception
                'Any error while pushing out a message to a client
                'will be treated as if that client has disconnected
                System.Diagnostics.Debug.WriteLine(ex)
                ClientDisconnected((CType(iar.AsyncState, PushMessageState)).sessionId)
            End Try
        End Sub


        Private Sub SendToService(ByVal msg As DuplexMessage) Implements IUniversalDuplexContract.SendToService
            'We get here when we receive a message from a client

            Dim ch As IUniversalDuplexCallbackContract = OperationContext.Current.GetCallbackChannel(Of IUniversalDuplexCallbackContract)()
            Dim session As String = OperationContext.Current.Channel.SessionId

            'Any message from a client we haven't seen before causes the new client to be added to our list
            '(Basically, treated as a "Connect" message)
            'SyncLock syncRoot
            '    If Not clients.ContainsKey(session) Then
            '        clients.Add(session, ch)
            '        AddHandler OperationContext.Current.Channel.Closing, AddressOf Channel_Closing
            '        AddHandler OperationContext.Current.Channel.Faulted, AddressOf Channel_Faulted
            '        OnConnected(session)
            '    End If
            'End SyncLock

            'If it's a Disconnect message, treat as disconnection
            If TypeOf msg Is DisconnectMessage Then
                ClientDisconnected(session)
                'Otherwise, if it's a payload-carrying message (and not just a simple "Connect"), process it
            ElseIf TypeOf msg Is JoinSessionMessage Then
                SyncLock syncRoot
                    If Not clients.ContainsKey(session) Then
                        clients.Add(session, ch)
                        AddHandler OperationContext.Current.Channel.Closing, AddressOf Channel_Closing
                        AddHandler OperationContext.Current.Channel.Faulted, AddressOf Channel_Faulted
                        OnConnected(session)
                    End If
                End SyncLock
                JoinSession(session, TryCast(msg, JoinSessionMessage))
            ElseIf Not (TypeOf msg Is ConnectMessage) Then
                If clients.ContainsKey(session) Then OnMessage(session, msg)
            End If
        End Sub

        Private Sub Channel_Closing(ByVal sender As Object, ByVal e As EventArgs)
            Dim channel As IContextChannel = CType(sender, IContextChannel)
            ClientDisconnected(channel.SessionId)
        End Sub

        Private Sub Channel_Faulted(ByVal sender As Object, ByVal e As EventArgs)
            Dim channel As IContextChannel = CType(sender, IContextChannel)
            ClientDisconnected(channel.SessionId)
        End Sub

        Public Sub ClientDisconnected(ByVal sessionId As String)
            SyncLock syncRoot
                If clients.ContainsKey(sessionId) Then
                    clients.Remove(sessionId)
                End If
            End SyncLock
            Try
                OnDisconnected(sessionId)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex)
            End Try
        End Sub

        'Helper class for tracking both a channel and its session ID together
        Private Class PushMessageState
            Friend ch As IUniversalDuplexCallbackContract
            Friend sessionId As String
            Friend Sub New(ByVal channel As IUniversalDuplexCallbackContract, ByVal session As String)
                ch = channel
                sessionId = session
            End Sub
        End Class
    End Class


    ''' <summary>
    ''' "Regular" part of Duplex contract:  From Silverlight to the Service
    ''' </summary>
    <ServiceContract(Name:="DuplexService", CallbackContract:=GetType(IUniversalDuplexCallbackContract))> _
    Public Interface IUniversalDuplexContract
        <OperationContract(IsOneWay:=True)> _
        Sub SendToService(ByVal msg As DuplexMessage)

    End Interface
    ''' <summary>
    ''' "Callback" part of Duplex contract: From the Service to Silverlight
    ''' </summary>
    <ServiceContract()> _
    Public Interface IUniversalDuplexCallbackContract
        '[OperationContract(IsOneWay = true)]
        'void SendToClient(DuplexMessage msg);

        <OperationContract(IsOneWay:=True, AsyncPattern:=True)> _
        Function BeginSendToClient(ByVal msg As DuplexMessage, ByVal acb As AsyncCallback, ByVal state As Object) As IAsyncResult
        Sub EndSendToClient(ByVal iar As IAsyncResult)


    End Interface

    ''' <summary>
    ''' Standard "Connect" message - clients may use this message to connect, when no other payload is required
    ''' </summary>
    <DataContract(Namespace:="http://samples.microsoft.com/silverlight2/duplex")> _
    Public Class ConnectMessage
        Inherits DuplexMessage
    End Class

    ''' <summary>
    ''' Standard "Disconnect" message - clients must use this message to disconnect
    ''' </summary>
    <DataContract(Namespace:="http://samples.microsoft.com/silverlight2/duplex")> _
    Public Class DisconnectMessage
        Inherits DuplexMessage
    End Class

    ''' <summary>
    ''' Base message class. Please add [KnownType] attributes as necessary for every 
    ''' derived message type.
    ''' </summary>
    <DataContract(Namespace:="http://samples.microsoft.com/silverlight2/duplex"), _
    KnownType(GetType(ConnectMessage)), KnownType(GetType(DisconnectMessage)), _
    KnownType(GetType(JoinSessionMessage)), KnownType(GetType(JoinSessionServerMessage)), _
    KnownType(GetType(ChatMessage)), KnownType(GetType(ClientConnectedMessage)), _
    KnownType(GetType(ClientDisconnectedMessage)), KnownType(GetType(EncrytValueMessage)), _
    KnownType(GetType(GeneralError)), KnownType(GetType(ConnectGameRoomMessage)), _
    KnownType(GetType(CreateGameRoomMessage)), KnownType(GetType(PlayerIsReadyMessage)), _
    KnownType(GetType(DieFixedMessage)), KnownType(GetType(DoRollMessage)), _
    KnownType(GetType(DoMoveMessage)), KnownType(GetType(ApplyScoreMessage)), _
    KnownType(GetType(GameOverMessage)), KnownType(GetType(GameStatusMessage))> _
    Public Class DuplexMessage
    End Class

End Namespace
