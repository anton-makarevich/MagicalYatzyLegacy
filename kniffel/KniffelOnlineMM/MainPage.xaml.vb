
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Browser
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Input
Imports System.Text
Imports KniffelNet.KniffelGameService
Imports Kniffel
Imports System.Windows.Threading
Imports Oogsoft.MyMir.SilverlightAPI
Imports Oogsoft.MyMir.SilverlightAPI.Users
'Imports MailRuWrapper
'Imports MailRuWrapper.Client
Partial Public Class MainPage
    Inherits UserControl
    Implements INotifyPropertyChanged
    Public fileDuplexService As DuplexServiceClient

    Private binding As New CustomBinding(New PollingDuplexBindingElement(), New BinaryMessageEncodingBindingElement(), New HttpTransportBindingElement())

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    'timer for ping
    Dim tPing As New DispatcherTimer

    'timer for mailru
    Dim tmm As New DispatcherTimer

    ' UI vars
    Private privateIsInSession As Boolean = False

    'Player
    Dim CurrentPlayer As New KniffelPlayerSL

    'Vkontakte variables
    Private objectID As String = "kniffelsl" ' ID of silverlight object in HTML page markup
    Private appID As Integer = 2121738 ' Your application ID
    Private secret As String = "JuaJ8mf5V7" ' Your application secret
    Private iuid As Double = 0 ' User ID
    Private uids(0) As Long

    ''moymir variables

    'Private _wrapper As ClientWrapper
    'Private muids(1) As String

    Public Property IsConnected() As Boolean
        Get
            Return privateIsInSession
        End Get
        Set(ByVal value As Boolean)
            If value = False Then
                CurrentPlayer.Name = String.Empty
                CurrentPlayer.Password = String.Empty
                CurrentPlayer.ID = String.Empty
            End If
            privateIsInSession = value
        End Set
    End Property
    Private Sub PingService()
        Dim cm As New ChatMessage
        cm.FromID = CurrentPlayer.ID
        cm.ToID = CurrentPlayer.ID
        cm.Message = "АУ! Не спим"
        SendToServiceAsync(New SendToService(cm))
        cpMain.AddMsgToListbox("АУ! Не спим")
    End Sub
    Private Sub SendToServiceAsync(ByVal request As SendToService)


        Try
            If fileDuplexService.State = CommunicationState.Closed Or fileDuplexService.State = CommunicationState.Faulted Then ReloadDuplex()
            fileDuplexService.SendToServiceAsync(request)
            tPing.Stop()
            tPing.Interval = TimeSpan.FromMinutes(8)
            tPing.Start()
        Catch ex As Exception
            cpMain.AddMsgToListbox(ex.Message)
        End Try
    End Sub

    Private Sub ReloadDuplex()
        fileDuplexService = New DuplexServiceClient(binding, New EndpointAddress("http://kniffel.sanet.by/KniffelGameService.svc"))
        ' fileDuplexService = New DuplexServiceClient(binding, New EndpointAddress("http://localhost:9797/KniffelGameService.svc"))
        AddHandler fileDuplexService.SendToClientReceived, AddressOf FileDuplexServiceSendToClientReceived
        AddHandler fileDuplexService.SendToServiceCompleted, AddressOf FileDuplexServiceSendToServiceCompleted
    End Sub

    Private Sub bAbout_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bAbout.MouseLeftButtonDown

        Dim fAbout As New AboutPage()
        Try
            fAbout.Version = "1.1.8.11028 beta" 'System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        fAbout.Show()
        bAbout_MouseLeave(sender, e)
    End Sub
    Private Sub bAbout_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bAbout.MouseEnter
        bAbout.Opacity = 1
    End Sub

    Private Sub bAbout_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bAbout.MouseLeave
        bAbout.Opacity = 0.6
    End Sub
    Public Sub New()
        InitializeComponent()
        ReloadDuplex()
        

        AddHandler cpMain.btnChatMsg.Click, AddressOf btnChatMsg_Click

        'AddHandler bConnect.Click, AddressOf MainPage_Loaded

        AddHandler dAddScore.Closed, AddressOf AddScore
        AddHandler cpMain.txtChatMsg.KeyDown, AddressOf txtChatMsg_KeyDown

        AddHandler DicePanelSL1.DieFrozen, AddressOf OnDieFrozen

        tPing.Interval = TimeSpan.FromMinutes(1)
        AddHandler tPing.Tick, AddressOf PingService

        tPing.Interval = TimeSpan.FromSeconds(3)
        AddHandler tmm.Tick, AddressOf LoadMM

        UIState = UIStates.uiDisconnected
        LayoutRoot.DataContext = Me
    End Sub

    Private Sub OnDieFrozen(ByVal fixed As Boolean, ByVal value As Integer)
        If Not UIState = UIStates.uiPlay Then Exit Sub
        Dim msg As New DieFixedMessage
        msg.GameId = CurrentPlayer.GameID
        msg.Username = CurrentPlayer.Name
        msg.Fixed = fixed
        msg.Value = value
        SendToServiceAsync(New SendToService(msg))
    End Sub
    Private Sub ClientDisconnected(ByVal cdm As ClientDisconnectedMessage)
        cpMain.AddMsgToListbox(cdm.Username & " вышел из игры")
        ' Dim lbi As lbiKniffelPlayer
        For Each ksp As KniffelScorePanel In spScoreTables.Children
            If ksp.PlayerName = cdm.Username Then
                spScoreTables.Children.Remove(ksp)
                Dim crm As New ConnectGameRoomMessage
                crm.Username = cdm.Username
                crm.GameId = 0
                Me.SendToServiceAsync(New SendToService(crm))
                Exit For
            End If

        Next
        For Each lbi2 As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If lbi2.PlayerName = cdm.Username Then
                cpMain.lbPlayers.Items.Remove(lbi2)
                Exit For
            End If

        Next

        UpdateRooms()
    End Sub
    Private Sub LoadPlayers(ByVal AllPlayers As String)
        cpMain.lbPlayers.Items.Clear()
        Dim ta() As String = Split(AllPlayers, "?.$P!,?")
        For Each str1 As String In ta
            Dim ta2() As String = Split(str1, "?.$V!,?")
            Dim lbi As New lbiKniffelPlayer
            lbi.PlayerName = ta2(0)
            lbi.gameid = ta2(1)
            lbi.ID = ta2(2)

            cpMain.lbPlayers.Items.Add(lbi)
        Next
    End Sub
    Private Sub UpdatePlayersGameStatus(ByVal allgames As String)
        Dim ta() As String = Split(allgames, "?.$P!,?")
        For Each str1 As String In ta
            Dim ta2() As String = Split(str1, "?.$V!,?")
            For Each lbi As lbiKniffelPlayer In cpMain.lbPlayers.Items
                If lbi.GameID = ta2(0) Then
                    lbi.IsPlaying = CBool(ta2(1))
                    lbi.Rules = CInt(ta2(2))
                End If
            Next


           
        Next
    End Sub

    Private Sub UpdateRooms()

        For Each kp As lbiKniffelPlayer In cpMain.lbPlayers.Items

            If Not IsRoomExist(kp.GameID) Then
                If kp.GameID = "0" Then Continue For
                Dim b As New KniffelGameRoom
                b.ID = kp.GameID
                Dim strRules As KniffelScoreLabel.KniffelRules
                b.PlayersNumber = GetPlayersInRoom(kp.GameID, strRules)
                b.Status = KniffelGameRoomStatus.kgrWaiting
                b.Rules = strRules
                AddHandler b.Click, AddressOf ConnectToGame
                spRooms.Children.Add(b)
            End If
        Next
    End Sub

    Private Function IsRoomExist(ByVal id As String) As Boolean
        Dim bToRemove As KniffelGameRoom
        Dim bRes As Boolean = False
RemoveRoom:
        If Not bToRemove Is Nothing Then spRooms.Children.Remove(bToRemove)
        For Each b As KniffelGameRoom In spRooms.Children
            b.PlayersNumber = GetPlayersInRoom(b.ID, Nothing)
            b.Status = GetRoomStatus(b.ID)
            If b.PlayersNumber = 0 Then
                bToRemove = b
                GoTo RemoveRoom
            End If

            If b.ID.ToString = id Then bRes = True
        Next
        Return bRes


    End Function
    Private Function GetPlayersInRoom(ByVal id As String, ByRef rules As KniffelScoreLabel.KniffelRules) As Integer
        Dim i As Integer = 0
        For Each kp As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If kp.GameID = id Then
                i += 1
                rules = kp.Rules
            End If

        Next
        Return i
    End Function
    Private Function GetRoomStatus(ByVal id As String) As KniffelGameRoomStatus

        For Each kp As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If Not kp.GameID = id Then Continue For
            If kp.IsPlaying Then Return KniffelGameRoomStatus.kgrPlaying
        Next
        Return KniffelGameRoomStatus.kgrWaiting
    End Function

    Private Sub FileDuplexServiceSendToClientReceived(ByVal sender As Object, ByVal e As SendToClientReceivedEventArgs)
        If e.Error Is Nothing Then
            Cursor = Cursors.Arrow
            If TypeOf e.request.msg Is ClientConnectedMessage Then
                Dim msg As ClientConnectedMessage = CType(e.request.msg, ClientConnectedMessage)
                cpMain.AddMsgToListbox(msg.Username & " подключился к игре")

                LoadPlayers(msg.AllPlayers)
                UpdatePlayersGameStatus(msg.AllGames)
                UpdateRooms()
            ElseIf TypeOf e.request.msg Is EncrytValueMessage Then
                ER = TryCast(e.request.msg, EncrytValueMessage).Value
            ElseIf TypeOf e.request.msg Is ConnectGameRoomMessage Then
                RoomChanged(CType(e.request.msg, ConnectGameRoomMessage))
            ElseIf TypeOf e.request.msg Is PlayerIsReadyMessage Then
                PlayerReady(CType(e.request.msg, PlayerIsReadyMessage))
            ElseIf TypeOf e.request.msg Is DieFixedMessage Then
                FixDie(CType(e.request.msg, DieFixedMessage))
            ElseIf TypeOf e.request.msg Is JoinSessionServerMessage Then
                Dim jssm As JoinSessionServerMessage = TryCast(e.request.msg, JoinSessionServerMessage)
                IsConnected = Not jssm.Failed
                If jssm.Failed Then
                    cpMain.AddMsgToListbox("Подключение не удалось. Возможно игрок с таким именем уже подключен")
                    Join()
                    Return
                Else
                    CurrentPlayer.Name = jssm.Name
                    CurrentPlayer.Password = jssm.Password
                    CurrentPlayer.ID = jssm.ID
                    CurrentPlayer.cn = jssm.EName
                    CurrentPlayer.cp = jssm.EPassword
                    UIState = UIStates.uiConnected
                End If


            ElseIf TypeOf e.request.msg Is ChatMessage Then
                Dim msg As ChatMessage = CType(e.request.msg, ChatMessage)

                If Not msg.FromID = CurrentPlayer.ID Then
                    Dim sendername As String = GetPlayerNameById(msg.FromID)
                    Dim strPrivate As String = ""
                    If msg.ToID = CurrentPlayer.ID Then strPrivate = " (приватно)"
                    cpMain.AddMsgToListbox(sendername & strPrivate & ": " & msg.Message)
                End If

            ElseIf TypeOf e.request.msg Is ClientDisconnectedMessage Then
                ClientDisconnected(CType(e.request.msg, ClientDisconnectedMessage))
            ElseIf TypeOf e.request.msg Is DoMoveMessage Then
                Dim dmm As DoMoveMessage = CType(e.request.msg, DoMoveMessage)
                If dmm.GameId = CurrentPlayer.GameID Then DoNewMove(dmm)
                If CurrentPlayer.GameID = 0 Then UpdateRoomMove(dmm.GameId, dmm.Move)
            ElseIf TypeOf e.request.msg Is DoRollMessage Then
                DoNewRoll(CType(e.request.msg, DoRollMessage))
            ElseIf TypeOf e.request.msg Is ApplyScoreMessage Then
                ApplyScore(CType(e.request.msg, ApplyScoreMessage))
            ElseIf TypeOf e.request.msg Is GameOverMessage Then
                GameOver(CType(e.request.msg, GameOverMessage).GameId)
            ElseIf TypeOf e.request.msg Is GameStatusMessage Then
                CheckGameStatus(CType(e.request.msg, GameStatusMessage))
            ElseIf TypeOf e.request.msg Is GeneralError Then
                cpMain.AddMsgToListbox((CType(e.request.msg, GeneralError)).ErrorMessage)
            End If
        Else
            cpMain.AddMsgToListbox("При получении команды с сервера возникла ошибка: " & e.Error.Message)
        End If
    End Sub
    Private Sub UpdateRoomMove(ByVal roomid As Integer, ByVal move As Integer)
        For Each kgr As KniffelGameRoom In spRooms.Children
            If kgr.ID = roomid.ToString Then kgr.Move = move
        Next
    End Sub

    Private Sub CheckGameStatus(ByVal msg As GameStatusMessage)
        For Each lbi As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If lbi.GameID = msg.GameId.ToString Then lbi.IsPlaying = msg.IsPlaying
        Next
        UpdateRooms()
    End Sub
    Private Sub GameOver(ByVal GameId As Integer)
        If GameId = CurrentPlayer.GameID Then
            CurrentPlayer.IsMoving = False
            DisbleRollButton()
            Button1.Content = "Новая игра"
            Button1.Visibility = Visibility.Visible
            Button2.Visibility = Visibility.Collapsed
            If (Not CurrentPlayer.Name = "") Then 'And CurrentPlayer.Rules = KniffelScoreLabel.KniffelRules.krStandart
                For Each kp As KniffelScorePanel In spScoreTables.Children
                    If kp.PlayerName = CurrentPlayer.Name Then
                        dAddScore.SetProperties(kp.Total)
                        Exit For
                    End If
                Next
                dAddScore.Show()


            End If

        End If
        'For Each kp As KniffelGameRoom In spRooms.Children
        '    If kp.ID = GameId.ToString Then kp.Status = KniffelGameRoomStatus.kgrWaiting
        'Next
    End Sub
    Private Sub DoNewMove(ByVal msg As DoMoveMessage)
        iMove = msg.Move
        tbTotal.Text = "ходит: " & msg.Username
        Button1.Content = "Идет игра..."
        If msg.Username = CurrentPlayer.Name Then
            EnableRollButton()
        Else
            RollButtonNotMy()
        End If
        For Each kp As KniffelGameRoom In spRooms.Children
            If kp.ID = msg.GameId.ToString Then kp.Move = msg.Move
        Next
        'For Each lbi
    End Sub
    Private Sub DoNewRoll(ByVal msg As DoRollMessage)

        Dim ai As New List(Of Integer)
        For i As Integer = 0 To 4
            ai.Add(msg.Value(i))
        Next
        DicePanelSL1.RollDice(ai)
    End Sub
    Private Sub ApplyScore(ByVal msg As ApplyScoreMessage)
        For Each kp As KniffelScorePanel In spScoreTables.Children
            If kp.PlayerName = msg.Username Then
                kp.ApplyScore(msg.ScoreType, msg.ScoreValue, msg.HaveBonus)
            End If
        Next
        DicePanelSL1.ClearFreeze()
    End Sub
    Private Sub FixDie(ByVal msg As DieFixedMessage)
        For Each d As Kniffel.DieSL In DicePanelSL1.aDice
            If d.Result = msg.Value And d.Frozen = Not msg.Fixed Then
                d.Frozen = msg.Fixed
                d.DrawDie()
                'AddMsgToListbox(msg.Username & " зафиксировал кость " & msg.Value)
                Exit Sub
            End If

        Next
    End Sub
    Private Sub PlayerReady(ByVal msg As PlayerIsReadyMessage)
        cpMain.AddMsgToListbox(msg.Username & " готов к игре")
        
    End Sub
    Private Sub RoomChanged(ByVal msg As ConnectGameRoomMessage)
        If msg.GameId = 0 Then
            Dim strOldRoom As String = "0"
            If msg.Username = CurrentPlayer.Name Then
                strOldRoom = CurrentPlayer.GameID.ToString
                CurrentPlayer.GameID = 0
                spScoreTables.Children.Clear()
            Else

                For Each kgp1 As lbiKniffelPlayer In cpMain.lbPlayers.Items
                    If kgp1.PlayerName = msg.Username Then
                        strOldRoom = kgp1.GameID
                        Exit For
                    End If
                Next
                If strOldRoom = CurrentPlayer.GameID.ToString AndAlso (Not strOldRoom = "0") Then
                    For Each st As KniffelScorePanel In spScoreTables.Children
                        If st.PlayerName = msg.Username Then
                            spScoreTables.Children.Remove(st)
                            Exit For
                        End If
                    Next
                End If
            End If
            If Not strOldRoom = "0" Then cpMain.AddMsgToListbox(msg.Username & " отошел от стола " & strOldRoom)
            For Each kgp As lbiKniffelPlayer In cpMain.lbPlayers.Items
                If kgp.PlayerName = msg.Username Then
                    kgp.IsPlaying = False
                End If


            Next
        Else
            cpMain.AddMsgToListbox(msg.Username & " присел к столу № " & msg.GameId)
            If msg.Username = CurrentPlayer.Name Then
                iMove = 1
                UIState = UIStates.uiPlay
                CurrentPlayer.GameID = msg.GameId
                CurrentPlayer.Rules = msg.Rules
                CurrentPlayer.cr = msg.GameTable
                DicePanelSL1.ClickToFreeze = False

                For Each kgp As lbiKniffelPlayer In cpMain.lbPlayers.Items
                    If kgp.GameID = msg.GameId.ToString Then
                        If Not IsPlayerInTable(kgp.PlayerName) Then
                            Dim st As New KniffelScorePanel(CurrentPlayer.Rules)
                            st.PlayerName = kgp.PlayerName
                            st.View = KniffelScoreLabel.KniffelLabelView.klvOnlyScore
                            AddHandler st.ScoreApplyed, AddressOf ScoreApplyed
                            AddHandler st.TotalCalculated, AddressOf EncodeResult
                            spScoreTables.Children.Add(st)
                        End If
                    End If
                Next
            Else
                If msg.GameId = CurrentPlayer.GameID Then
                    If Not IsPlayerInTable(msg.Username) Then
                        Dim st As New KniffelScorePanel(CurrentPlayer.Rules)
                        st.PlayerName = msg.Username
                        st.View = KniffelScoreLabel.KniffelLabelView.klvOnlyScore
                        AddHandler st.ScoreApplyed, AddressOf ScoreApplyed
                        spScoreTables.Children.Add(st)
                    End If
                End If

                End If

        End If
        For Each kgp As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If kgp.PlayerName = msg.Username Then
                kgp.GameID = msg.GameId.ToString
                kgp.Rules = msg.Rules

            End If


        Next
        UpdateRooms()
    End Sub
    Private Function IsPlayerInTable(ByVal name As String) As Boolean
        For Each cp As KniffelScorePanel In spScoreTables.Children
            If cp.PlayerName = name Then Return True
        Next
        Return False
    End Function
    Enum UIStates
        uiDisconnected = 0
        uiConnected = 1
        uiPlay = 3
    End Enum
    Private _uistate As UIStates
    Private Property UIState As UIStates
        Set(ByVal value As UIStates)
            tbName.Width = 600 - iHelp.ActualWidth - bAbout.ActualWidth - iPlayer.ActualWidth - tbName.Margin.Left - tbName.Margin.Right
            spScoreTables.Children.Clear()
            Select Case value
                Case UIStates.uiConnected
                    tbName.Text = CurrentPlayer.Name '"Подключен как " &

                    '.Visibility = Visibility.Collapsed
                    'bDisconnect.Visibility = Visibility.Visible
                    spСhat.Visibility = Visibility.Visible
                    spPlayMenu.Visibility = Visibility.Visible
                    spPlaySingle.Visibility = Visibility.Collapsed
                    spPlayPanel.Visibility = Visibility.Collapsed

                    bCreateRoom.Visibility = Windows.Visibility.Visible
                    bExit.Visibility = Windows.Visibility.Collapsed
                    tbName.Width -= 48
                Case UIStates.uiDisconnected
                    tbName.Text = "" 'Вход не выполнен
                    'bConnect.Visibility = Visibility.Visible
                    'bDisconnect.Visibility = Visibility.Collapsed
                    spСhat.Visibility = Visibility.Collapsed
                    spPlayMenu.Visibility = Visibility.Collapsed
                    spPlaySingle.Visibility = Visibility.Collapsed
                    spPlayPanel.Visibility = Visibility.Collapsed
                    bCreateRoom.Visibility = Windows.Visibility.Collapsed
                    bExit.Visibility = Windows.Visibility.Collapsed

                Case UIStates.uiPlay
                    tbName.Text = CurrentPlayer.Name '"Подключен как " & 
                    'bConnect.Visibility = Visibility.Collapsed
                    'bDisconnect.Visibility = Visibility.Visible
                    spСhat.Visibility = Visibility.Visible
                    spPlayMenu.Visibility = Visibility.Collapsed
                    spPlayPanel.Visibility = Visibility.Visible
                    spPlaySingle.Visibility = Visibility.Visible
                    Dim st As New KniffelScorePanel(CurrentPlayer.Rules)
                    st.PlayerName = CurrentPlayer.Name
                    AddHandler st.ScoreApplyed, AddressOf ScoreApplyed
                    AddHandler st.TotalCalculated, AddressOf EncodeResult
                    spScoreTables.Children.Add(st)
                    Button1.Content = "Готов играть"
                    Button1.Visibility = Windows.Visibility.Visible
                    Button2.Visibility = Windows.Visibility.Collapsed
                    RollButtonNotMy()
                    bCreateRoom.Visibility = Windows.Visibility.Collapsed
                    bExit.Visibility = Windows.Visibility.Visible
                    tbName.Width -= 48
            End Select

            _uistate = value
            'RaiseEvent UIStateUpdated()
        End Set
        Get
            Return _uistate
        End Get
    End Property
    'Public Event UIStateUpdated()
    'Private Sub OnUIUpdated() Handles Me.UIStateUpdated

    'End Sub
    Public Function GetPlayerNameById(ByVal Id As String) As String
        For Each lbi As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If lbi.ID = Id Then Return lbi.PlayerName
        Next
        Return "Неизвестный"
    End Function
    Private Sub FileDuplexServiceSendToServiceCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If e.Error Is Nothing Then
            If e.UserState Is Nothing Then
                Return
            End If

        Else
            cpMain.AddMsgToListbox("При отправке команды на сервер возникла ошибка: " & e.Error.Message)
        End If
    End Sub

    Private Sub SessionJoinClosed(ByVal sender As Object, ByVal e As EventArgs)
        Dim sessionJoin As JoinSession = CType(sender, JoinSession)
        If sessionJoin.DialogResult = True Then
            Dim jsm As New JoinSessionMessage()
            jsm.UserPass = sessionJoin.Password
            jsm.Username = sessionJoin.Username
            
            'If fileDuplexService.State = CommunicationState.Closed Or fileDuplexService.State = CommunicationState.Faulted Then ReloadDuplex()
            SendToServiceAsync(New SendToService(jsm))
            Cursor = Cursors.Wait
            CheckDefRoom()
        End If
        
        
    End Sub

    Private Sub Join()
        If Cursor Is Cursors.Wait Then Exit Sub
        If IsConnected Then Exit Sub
        Dim sessionJoin As New JoinSession()
        AddHandler sessionJoin.Closed, AddressOf SessionJoinClosed
        sessionJoin.Show()
    End Sub
    Private Sub btnChatMsg_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If cpMain.txtChatMsg.Text.Trim() = String.Empty Then
            Return
        End If

        Dim cm As New ChatMessage()
        cm.Message = cpMain.txtChatMsg.Text
        Dim strFromTo As String = CurrentPlayer.Name
        If cpMain.lbPlayers.SelectedItems.Count > 0 Then
            Dim kpi As lbiKniffelPlayer = TryCast(cpMain.lbPlayers.SelectedItem, lbiKniffelPlayer)
            cm.ToID = kpi.ID
            strFromTo = strFromTo & " для " & kpi.PlayerName
        End If
        cm.FromID = CurrentPlayer.ID
        cm.GameID = CurrentPlayer.GameID
        cpMain.AddMsgToListbox(strFromTo & ": " & cpMain.txtChatMsg.Text)
        SendToServiceAsync(New SendToService(cm))
        cpMain.txtChatMsg.Text = ""

    End Sub

    Public Sub btnDisconnect_Click()

        If CurrentPlayer.IsMoving Then
            FalseMove()
            CurrentPlayer.IsMoving = False
        End If
        SendToServiceAsync(New SendToService(New DisconnectMessage))

        UIState = UIStates.uiDisconnected
        IsConnected = False

    End Sub

    Private Sub txtChatMsg_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = Key.Enter Then
            btnChatMsg_Click(Me, Nothing)
        End If
    End Sub

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Private Sub MainPage_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        
           
        'Join()

        tmm.Start()

    End Sub
    Private Sub CheckDefRoom()

        Dim strqs As String = ""
        Try
            strqs = HtmlPage.Document.QueryString("rules")
        Catch ex As Exception

        End Try

        Select Case strqs
            Case "baby"
                CurrentPlayer.Rules = KniffelScoreLabel.KniffelRules.krSimple
            Case "full"
                CurrentPlayer.Rules = KniffelScoreLabel.KniffelRules.krExtended
            Case Else
                CurrentPlayer.Rules = KniffelScoreLabel.KniffelRules.krStandart
        End Select

    End Sub
    '#Region "ВКонтакте"
    '    Private Sub LoadVk()
    '        AddHandler Vk.Instance.VkInitSucceeded, AddressOf Instance_VkInitSucceeded
    '        AddHandler Vk.Instance.VkInitFailed, AddressOf Instance_VkInitFailed

    '        Vk.Instance.Initialize(objectID, appID, secret, iuid, False) ' запуск API в тестовом режиме (true|false) -  ID текущего пользователя (при запуске из IFrame ВКонтакте можно импользовать Vk.Instance.Parameters.ViewerID) -  secret вашего приложения (можете найти/установить его в настройках приложения) -  ID вашего приложения (можете найти его в настройках приложения) -  это ID вашего <object/> тэга в HTML разметке


    '        AddHandler Vk.Instance.External.ApplicationAdded, AddressOf ApplicationAdded
    '        AddHandler Vk.Instance.External.SettingsChanged, AddressOf VKSettingsChanged
    '        AddHandler Vk.Instance.Api.DirectRequestCallback, AddressOf Api_DirectRequestCallback

    '        uids(0) = iuid

    '        Vk.Instance.Api.IsAppUser(New ApiCallback(Of String)(AddressOf IsAppUserCallback))

    '    End Sub
    '    Private Sub GetProfilePhotoCallback(ByVal response As Response(Of VkApi.DataTypes.Profile()))
    '        Dim strPhotoUri As String = response.ResponseData(0).photo
    '        Try
    '            iPlayer.Source = New BitmapImage(New Uri(strPhotoUri))
    '            iPlayer.Width = 50
    '            iPlayer.Height = 50
    '        Catch ex As Exception
    '            cpMain.AddMsgToListbox("Не удалось загрузить фото по адресу " & strPhotoUri & _
    '                                   ex.Message)
    '        End Try
    '    End Sub
    '    Private Sub GetProfileNameCallback(ByVal response As Response(Of VkApi.DataTypes.Profile()))

    '        Dim jsm As New JoinSessionMessage()
    '        jsm.Username = response.ResponseData(0).first_name & " " & response.ResponseData(0).last_name
    '        jsm.UserPass = CStr(response.ResponseData(0).uid)

    '        SendToServiceAsync(New SendToService(jsm))
    '        Cursor = Cursors.Wait
    '        CheckDefRoom()
    '    End Sub
    '    Private Sub ApplicationAdded(ByVal sender As Object, ByVal e As EventArgs)
    '        CheckForSettings()
    '    End Sub

    '    Private Sub IsAppUserCallback(ByVal response As Response(Of String))

    '        If response.ResponseData = "1" Then
    '            CheckForSettings()
    '        Else
    '            Vk.Instance.External.ShowInstallBox()
    '        End If
    '    End Sub
    '    Private Sub CheckForSettings()
    '        Vk.Instance.Api.GetUserSettings(New ApiCallback(Of VkApi.DataTypes.UserSettings)(AddressOf Me.GetUserSettingsCallback))


    '    End Sub
    '    Private Sub GetUserSettingsCallback(ByVal response As Response(Of VkApi.DataTypes.UserSettings))
    '        If response.ResponseData.ToString.Contains(VkApi.DataTypes.UserSettings.MenuLinkAccess.ToString) Then ' _
    '            'And response.ResponseData.ToString.Contains(VkApi.DataTypes.UserSettings.NotificationsAccess.ToString) _
    '            'And response.ResponseData.ToString.Contains(VkApi.DataTypes.UserSettings.FriendsAccess.ToString) Then

    '            If UIState = UIStates.uiDisconnected Then
    '                Vk.Instance.Api.GetProfiles(uids, VkApi.DataTypes.ProfileFields.photo, New ApiCallback(Of VkApi.DataTypes.Profile())(AddressOf Me.GetProfilePhotoCallback))
    '                Vk.Instance.Api.GetProfiles(uids, VkApi.DataTypes.ProfileFields.first_name, New ApiCallback(Of VkApi.DataTypes.Profile())(AddressOf Me.GetProfileNameCallback))

    '            End If
    '        Else
    '            MessageBox.Show("Игре Книффель онлайн необходим доступ к вашим друзьям," & vbCrLf & _
    '                           "а также возможности устонавливать ссылку в левом меню и присылать вам уведомления" & vbCrLf & _
    '                           "Пожалуйста измените соответствующие настройки приложения.")
    '            Vk.Instance.External.ShowSettingsBox()
    '        End If

    '    End Sub
    '    Private Sub VKSettingsChanged(ByVal st As VkApi.DataTypes.UserSettings)
    '        CheckForSettings()
    '    End Sub
    '    Public Property VKPhotoUri As String

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    Private Sub Instance_VkInitSucceeded(ByVal sender As Object, ByVal e As EventArgs)
    '        'успешная инициализация
    '    End Sub

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    Private Sub Instance_VkInitFailed(ByVal sender As Object, ByVal e As VkInitFailedEventArgs)
    '        Join()
    '    End Sub

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    Private Sub Api_DirectRequestCallback(ByVal requestID As Long, ByVal jsonData As String)
    '        'Me.WriteLine(requestID.ToString())
    '        'Me.WriteLine(jsonData)
    '    End Sub
    '#End Region
    '#Region "MoiMir"
    Public Sub LoadMM()
        tmm.Stop()

        Try

            Dim strUsers() As String
            strUsers = New String() {HtmlPage.Document.QueryString("vid")}
            Dim mmusers = SocialFactory.Create(Of IUsers)()
            Dim str As String
            'mmusers.GetInfo(strUsers, New CollectionResultDelegate(Of User)(AddressOf GetProfileNameCallback), Nothing)
            mmusers.GetInfo(strUsers, Sub(users As IEnumerable(Of User), ex As Exception) str = users.GetEnumerator.Current.FirstName, Nothing)
            MessageBox.Show(str)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub GetProfileNameCallback(ByVal response As IEnumerable(Of User), ByVal ex2 As Exception)
        MessageBox.Show("cb")
        MessageBox.Show(ex2.Message)
        Try
            Dim us As User = response.GetEnumerator.Current
            MessageBox.Show(us.FirstName)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    '        _wrapper = New ClientWrapper("2b5bef209d9201d7b1e9198ddd508a9c", HtmlPage.Document.QueryString)
    '        AddHandler _wrapper.Log, AddressOf WrapperLog
    '        muids(0) = _wrapper.VID
    '        '
    '        _wrapper.IsPreValidationEnabled = True

    '        'Try
    '        '    _wrapper.Users.BeginGetInfo(New AsyncCallback(AddressOf OnUserGetInfoCallBack), _wrapper, muids)

    '        'Catch ex As Exception
    '        '    MessageBox.Show(ex.Message)
    '        'End Try

    '        Dim myWebRequest As Net.WebRequest
    '        Dim sb As New StringBuilder
    '        sb.Append("http://www.appsmail.ru/platform/api?method=users.getInfo&app_id=")
    '        sb.Append(_wrapper.AppID)
    '        sb.Append("&session_key=")
    '        sb.Append(_wrapper.SessionKey)
    '        sb.Append("&sig=")
    '        sb.Append(HtmlPage.Document.QueryString("sig"))
    '        sb.Append("&uids=")
    '        sb.Append(_wrapper.VID)
    '        MessageBox.Show(sb.ToString)
    '        Try
    '            myWebRequest = Net.WebRequest.Create(sb.ToString)
    '            myWebRequest.BeginGetRequestStream(New AsyncCallback(AddressOf OnEndGerRequestStream), myWebRequest)
    '        Catch ex As Exception
    '            MessageBox.Show(ex.Message)
    '        End Try

    '        MessageBox.Show("send")

    '    End Sub
    '    Private Sub OnEndGerRequestStream(ByVal ar As IAsyncResult)
    '        MessageBox.Show("CallBAck")
    '        Dim wr As Net.WebRequest = TryCast(ar, Net.WebRequest)
    '        Dim sr As StreamReader = New StreamReader(wr.EndGetRequestStream(ar))
    '        MessageBox.Show(sr.ReadToEnd)
    '    End Sub
    '    Private Sub OnUserGetInfoCallBack(ByVal ar As IAsyncResult)
    '        MessageBox.Show("CallBAck")
    '        Dim _w As ClientWrapper = TryCast(ar.AsyncState, ClientWrapper)
    '        Dim users As MailRuWrapper.IUser() = _w.Users.EndGetInfo(ar)
    '        MessageBox.Show("after list")
    '        Dim user As IUser
    '        If users Is Nothing Then
    '            MessageBox.Show("user nothing")
    '            Join()
    '        Else
    '            Try
    '                user = users(0)
    '                MessageBox.Show(user.FirstName)
    '                Dim jsm As New JoinSessionMessage()
    '                jsm.Username = user.FirstName & " " & user.LastName
    '                jsm.UserPass = user.Uid

    '                SendToServiceAsync(New SendToService(jsm))
    '                Cursor = Cursors.Wait
    '                CheckDefRoom()
    '            Catch ex As Exception
    '                MessageBox.Show(ex.Message)
    '                Join()
    '            End Try

    '        End If
    '    End Sub
    '    Private Sub WrapperLog(ByVal obj As String)
    '        cpMain.AddMsgToListbox(obj)
    '    End Sub


    '#End Region
#Region "Game"
    '''PlaySingleGame

    Private IsRolling As Boolean
    Private iRoll As Integer
    Private IsPlaying As Boolean
    Private fmove As Integer
    Private dAddScore As New AddScorePage
    Private Property iMove As Integer
        Get
            Return fmove
        End Get
        Set(ByVal value As Integer)
            fmove = value
            tbMove.Text = "ход: " & value
        End Set
    End Property
    'encrypted Result
    Private ER As String
    Private Sub AddScore()
        If dAddScore.DialogResult Then

            Dim ks As New KniffelPutScore.KniffelServiceSoapClient
            'Dim address As New EndpointAddress("http://localhost:1069/screenagent/KniffelService.asmx")
            Dim address As New EndpointAddress("http://sanet.by/KniffelService.asmx")
            ' Use the new address with the proxy object.

            ks.Endpoint.Address = address
            ks.PutScoreIntoTableAsync(CurrentPlayer.cn, CurrentPlayer.cp, ER, CurrentPlayer.cr)

        End If
    End Sub
    Private Sub EndOfRoll()
        IsRolling = False

        If CurrentPlayer.IsMoving Then
            For Each kp As KniffelScorePanel In spScoreTables.Children
                If kp.PlayerName = CurrentPlayer.Name Then kp.ShowPossibleValue(DicePanelSL1, iRoll)
            Next
            If iRoll < 4 Then
                EnableRollButton()
            Else

                DisbleRollButton()
            End If
        End If

    End Sub
    Private Sub BeginOfRoll()
        IsRolling = True
        For Each kp As KniffelScorePanel In spScoreTables.Children
            If kp.PlayerName = CurrentPlayer.Name Then kp.HidePossibleValue()
        Next
    End Sub
    Public Sub EncodeResult(ByVal name As String, ByVal value As String)
        'If iMove = 13 Then

        If name = CurrentPlayer.Name Then
            Dim em As New EncrytValueMessage
            em.Value = value
            SendToServiceAsync(New SendToService(em))
        End If

        'End If
    End Sub
    Private Sub ScoreApplyed(ByVal type As KniffelScoreLabel.KniffelLabelType, ByVal value As Integer, ByVal havebonus As Boolean)
        DicePanelSL1.ClearFreeze()

        iRoll = 1

        If UIState = UIStates.uiPlay Then

            Dim msg As New ApplyScoreMessage
            msg.GameId = CurrentPlayer.GameID
            msg.Move = iMove
            msg.Username = CurrentPlayer.Name
            msg.ScoreType = CInt(type)
            msg.ScoreValue = value
            msg.HaveBonus = havebonus
            SendToServiceAsync(New SendToService(msg))
        End If

    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If iRoll = 1 Then
            DicePanelSL1.ClearFreeze()
            DicePanelSL1.ClickToFreeze = True
        End If

        If DicePanelSL1.AllDiceFrozen Then Exit Sub

        Dim msg As New DoRollMessage

        msg.GameId = CurrentPlayer.GameID
        Me.SendToServiceAsync(New SendToService(msg))

        iRoll += 1
        Button2.Content = "Бросаем..."
        Button2.IsEnabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        DicePanelSL1.DieAngle = 1
        DicePanelSL1.TreeDScaleCoef = 0.38
        DicePanelSL1.NumDice = 5
        DicePanelSL1.RollDelay = 15
        'DicePanelSL1.ClickToFreeze = True
        DicePanelSL1.MaxRollLoop = 40
        'DicePanelSL1.PlaySound = True


        AddHandler DicePanelSL1.EndRoll, AddressOf EndOfRoll
        AddHandler DicePanelSL1.BeginRoll, AddressOf BeginOfRoll
        For Each KP As KniffelScorePanel In spScoreTables.Children
            KP.Renew()
        Next
        iMove = 1
        iRoll = 1

        Dim msg As New PlayerIsReadyMessage
        msg.GameId = CurrentPlayer.GameID
        msg.Username = CurrentPlayer.Name
        Me.SendToServiceAsync(New SendToService(msg))
        'If CurrentPlayer.Name = msg.Username Then
        Button2.Content = "Ждем других..."
        Button1.Visibility = Visibility.Collapsed
        Button2.Visibility = Visibility.Visible
        'End If
    End Sub
    Private Sub EnableRollButton()

        Button2.Content = "Бросок " & iRoll
        Button2.IsEnabled = True
        CurrentPlayer.IsMoving = True

    End Sub
    Private Sub DisbleRollButton()

        Button2.Content = "Конец хода "
        Button2.IsEnabled = False
        DicePanelSL1.ClickToFreeze = False
    End Sub
    Private Sub RollButtonNotMy()

        Button2.Content = "Ждем других"
        Button2.IsEnabled = False
        DicePanelSL1.ClickToFreeze = False
        CurrentPlayer.IsMoving = False
    End Sub
#End Region

    Private Sub bExit_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bExit.MouseEnter
        bExit.Opacity = 1
    End Sub

    Private Sub bExit_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bExit.MouseLeave
        bExit.Opacity = 0.6
    End Sub
    Private Sub bExit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bExit.MouseLeftButtonDown
        Dim crm As New ConnectGameRoomMessage
        crm.Username = CurrentPlayer.Name
        crm.GameId = 0
        crm.OldGameId = CurrentPlayer.GameID
        Me.SendToServiceAsync(New SendToService(crm))
        If CurrentPlayer.IsMoving Then
            FalseMove()
            CurrentPlayer.IsMoving = False
        End If
        UIState = UIStates.uiConnected
        bExit.Opacity = 0.6
    End Sub
    Private Sub FalseMove()
        Dim msg As New ApplyScoreMessage
        msg.GameId = CurrentPlayer.GameID
        msg.Move = iMove
        msg.Username = CurrentPlayer.Name
        msg.ScoreType = 21
        msg.ScoreValue = 0
        Me.SendToServiceAsync(New SendToService(msg))
    End Sub
    Private Sub ConnectToGame(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        Dim crm As New ConnectGameRoomMessage
        crm.Username = CurrentPlayer.Name
        crm.Rules = TryCast(sender, KniffelGameRoom).Rules
        crm.GameId = CInt(TryCast(sender, KniffelGameRoom).ID)
        CurrentPlayer.Rules = crm.Rules
        Me.SendToServiceAsync(New SendToService(crm))
    End Sub

    Private Sub bCreateRoom_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bCreateRoom.MouseEnter
        bCreateRoom.Opacity = 1
    End Sub

    Private Sub bCreateRoom_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bCreateRoom.MouseLeave
        bCreateRoom.Opacity = 0.6
    End Sub

    Private Sub bCreateRoom_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bCreateRoom.MouseLeftButtonDown
        Dim roomJoin As New JoinRoom()
        AddHandler roomJoin.Closed, AddressOf CreateRoomClosed
        roomJoin.Rules = CurrentPlayer.Rules
        roomJoin.Show()
        bCreateRoom.Opacity = 0.6
    End Sub
    Private Sub CreateRoomClosed(ByVal sender As Object, ByVal e As EventArgs)
        Dim roomJoin As JoinRoom = CType(sender, JoinRoom)
        If roomJoin.DialogResult = True Then
            Dim crm As New CreateGameRoomMessage
            crm.Username = CurrentPlayer.Name
            CurrentPlayer.Rules = roomJoin.Rules
            crm.Rules = roomJoin.Rules
            Me.SendToServiceAsync(New SendToService(crm))
        End If
    End Sub




    'Private Sub popUp_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles popUp.MouseLeftButtonDown
    '    popUp.IsOpen = False
    'End Sub

    Private Sub iHelp_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles iHelp.MouseEnter
        iHelp.Opacity = 1
    End Sub

    Private Sub iHelp_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles iHelp.MouseLeave
        iHelp.Opacity = 0.6
    End Sub

    Private Sub iHelp_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles iHelp.MouseLeftButtonDown
        Dim fHelp As New HelpPage()
        fHelp.Show()
        iHelp_MouseLeave(sender, e)
    End Sub


End Class





