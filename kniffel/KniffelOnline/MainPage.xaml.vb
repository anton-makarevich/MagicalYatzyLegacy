
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
Imports VkApi
Imports VkApi.DataTypes
Imports MailRuWrapper
Imports MailRuWrapper.Client
'Imports RxCore2
Partial Public Class MainPage
    Inherits UserControl
    Implements INotifyPropertyChanged
    Public fileDuplexService As DuplexServiceClient

    Private binding As New CustomBinding(New PollingDuplexBindingElement(), New BinaryMessageEncodingBindingElement(), New HttpTransportBindingElement())

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    'timer for ping
    Dim tPing As New dispatcherTimer

    ' UI vars
    Private privateIsInSession As Boolean = False

    'Player
    Dim CurrentPlayer As New KniffelPlayerSL

    'Vkontakte variables
    Private objectID As String = "kniffelsl" ' ID of silverlight object in HTML page markup
    Private appID As Integer = 2121738 ' Your application ID
    Private secret As String = "JuaJ8mf5V7" ' Your application secret
    Private iuid As Long = 0 ' User ID
    Private uids() As Long

    ''moymir variables
    Private _wrapper As ClientWrapper
    Private muids() As String

    'ShowWindoe
    Private pageWW As WaitWindow

    'KniffelScoreService
    Private ks As New KniffelPutScore.KniffelServiceSoapClient
    Private pageTT As TopTenPage

    'variable to count tries of command
    Private itry As Integer = 0
    Private dmLastMessage As DuplexMessage
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


TryAgain:
        itry += 1
        dmLastMessage = request.msg
        Try
            If fileDuplexService.State = CommunicationState.Closed Or fileDuplexService.State = CommunicationState.Faulted Then ReloadDuplex()
            fileDuplexService.SendToServiceAsync(request)
            tPing.Stop()
            tPing.Interval = TimeSpan.FromMinutes(8)
            tPing.Start()
        Catch ex As Exception
            If itry < 4 Then GoTo TryAgain
            cpMain.AddMsgToListbox(ex.Message)
        End Try
    End Sub

    Private Sub ReloadDuplex()
        fileDuplexService = New DuplexServiceClient(binding, New EndpointAddress("http://kniffel.sanet.by/KniffelGameService.svc"))
        'fileDuplexService = New DuplexServiceClient(binding, New EndpointAddress("http://localhost:9797/KniffelGameService.svc"))
        AddHandler fileDuplexService.SendToClientReceived, AddressOf FileDuplexServiceSendToClientReceived
        AddHandler fileDuplexService.SendToServiceCompleted, AddressOf FileDuplexServiceSendToServiceCompleted
    End Sub

    Private Sub UpdateStatus()
        If Not CurrentPlayer.GameID = 0 Then Exit Sub
        Dim sb As New StringBuilder
        If cpMain.lbPlayers.Items.Count < 2 Then
            tbWhoPlay.Text = "К сожалению, в настоящее время кроме вас никого в игре нет."
            Exit Sub
        Else
            sb.Append("Количество игроков, подключенных к игре :")
            sb.Append(cpMain.lbPlayers.Items.Count)
            sb.Append(". ")
        End If
        If spRooms.Children.Count = 0 Then
            sb.Append("Игровых столов нет.")
        Else
            Dim a As New List(Of String)
            Dim n As New List(Of String)
            For Each kgr As KniffelGameRoom In spRooms.Children
                If kgr.Status = KniffelGameRoomStatus.kgrPlaying Then
                    n.Add(kgr.ID)
                Else
                    a.Add(kgr.ID)
                End If
            Next
            If Not a.Count = 0 Then
                sb.Append(vbCrLf)
                If a.Count = 1 Then
                    sb.Append("За столом ")
                    sb.Append(a(0))
                Else
                    sb.Append("За столами ")
                    For i = 0 To a.Count - 1

                        sb.Append(a(i))
                        If Not i = a.Count - 1 Then sb.Append(", ")
                    Next
                End If
                sb.Append(" ждут игроков - присоеденяйтесь, нажав на кнопку соответствующего стола ниже. ")
            End If
            If Not n.Count = 0 Then
                sb.Append(vbCrLf)
                If n.Count = 1 Then
                    sb.Append("За столом ")
                    sb.Append(n(0))
                Else
                    sb.Append("За столами ")
                    For i = 0 To n.Count - 1

                        sb.Append(n(i))
                        If Not i = n.Count - 1 Then sb.Append(", ")
                    Next
                End If
                sb.Append(" идет игра. Дождитесь окончания партии, чтобы присоединиться.")
            End If

        End If
        tbWhoPlay.Text = sb.ToString
    End Sub

    Public Sub New()
        InitializeComponent()
        ReloadDuplex()


        AddHandler cpMain.btnChatMsg.Click, AddressOf btnChatMsg_Click
        AddHandler DicePanelSL1.StartLoading, AddressOf ShowWaitWindow
        AddHandler DicePanelSL1.StopLoading, AddressOf HideWaitWindow
        'AddHandler bConnect.Click, AddressOf MainPage_Loaded

        AddHandler dAddScore.Closed, AddressOf AddScore
        AddHandler cpMain.txtChatMsg.KeyDown, AddressOf txtChatMsg_KeyDown

        AddHandler DicePanelSL1.DieFrozen, AddressOf OnDieFrozen

        tPing.Interval = TimeSpan.FromMinutes(1)
        AddHandler tPing.Tick, AddressOf PingService

        UIState = UIStates.uiDisconnected
        LayoutRoot.DataContext = Me


        'Dim address As New EndpointAddress("http://localhost:1098/KniffelService.asmx")
        Dim address As New EndpointAddress("http://sanet.by/KniffelService.asmx")
        ks.Endpoint.Address = address
        AddHandler ks.GetTopPlayersCompleted, AddressOf GetTopPlayersCallBack
        AddHandler ks.GetChempionCompleted, AddressOf GetChempionCallBack
    End Sub
    'Private Sub ResizeMyHTML()
    '    Dim element As HtmlElement
    '    element = HtmlPage.Document.GetElementById("kniffelsl")
    '    element.SetStyleAttribute("width", Me.Width & "px")
    '    element.SetStyleAttribute("height", Me.Height & "px")
    'End Sub
    Private Sub GetChempionCallBack(ByVal sender As Object, ByVal e As KniffelPutScore.GetChempionCompletedEventArgs)
        If e.Error Is Nothing Then
            tbRoomInfo.Text = "Стол №" & CurrentPlayer.GameID & ", правила: " & KniffelScoreLabel.RuleToString(CurrentPlayer.Rules) & ", лучший результат: " & e.Score & ", принадлежит " & e.Name
        Else
            tbRoomInfo.Text = ""
            cpMain.AddMsgToListbox(e.Error.Message)
        End If
    End Sub
    Private Sub ShowWaitWindow()
        pageWW = New WaitWindow
        pageWW.Show()
    End Sub
    Private Sub HideWaitWindow()
        pageWW.Close()
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
                If DirectCast(tbTotal.Tag, String) = cdm.Username Then
                    FalseMove(cdm.Username)

                End If
                SendReady(cdm.Username)
                Exit For
            End If

        Next
        For Each lbi2 As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If lbi2.PlayerName = cdm.Username Then

                cpMain.lbPlayers.Items.Remove(lbi2)
                UpdateStatus()
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
            lbi.GameID = ta2(1)
            lbi.ID = ta2(2)

            cpMain.lbPlayers.Items.Add(lbi)

        Next
        UpdateStatus()
    End Sub
    Private Sub UpdatePlayersGameStatus(ByVal allgames As String)
        Dim ta() As String = Split(allgames, "?.$P!,?")
        For Each str1 As String In ta
            Dim ta2() As String = Split(str1, "?.$V!,?")
            For Each lbi As lbiKniffelPlayer In cpMain.lbPlayers.Items
                If lbi.GameID = ta2(0) Then
                    lbi.IsPlaying = CBool(ta2(1))
                    lbi.Rules = CInt(ta2(2))
                    lbi.Move = CInt(ta2(3))
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
                b.PlayersNumber = GetPlayersInRoom(b, strRules)
                b.Status = KniffelGameRoomStatus.kgrWaiting
                b.Rules = strRules
                b.Move = kp.Move + 1
                AddHandler b.EnterClicked, AddressOf ConnectToGame
                AddHandler b.RoomContentUpdated, AddressOf UpdateStatus
                spRooms.Children.Add(b)
            End If
        Next
        UpdateStatus()
    End Sub

    Private Function IsRoomExist(ByVal id As String) As Boolean
        Dim bToRemove As KniffelGameRoom
        Dim bRes As Boolean = False
RemoveRoom:
        If Not bToRemove Is Nothing Then spRooms.Children.Remove(bToRemove)
        For Each b As KniffelGameRoom In spRooms.Children
            b.PlayersNumber = GetPlayersInRoom(b, Nothing)
            b.Status = GetRoomStatus(b.ID)
            If b.PlayersNumber = 0 Then
                bToRemove = b
                GoTo RemoveRoom
            End If

            If b.ID.ToString = id Then bRes = True
        Next
        Return bRes


    End Function
    Private Function GetPlayersInRoom(ByRef room As KniffelGameRoom, ByRef rules As KniffelScoreLabel.KniffelRules) As Integer
        Dim i As Integer = 0
        room.wpPlayers.Children.Clear()
        For Each kp As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If kp.GameID = room.ID Then
                Dim pl As New KniffelPlayerLabel
                pl.tbName.Text = kp.PlayerName
                room.wpPlayers.Children.Add(pl)
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
                    Try
                        HideWaitWindow()
                    Catch ex As Exception

                    End Try
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
            cpMain.AddMsgToListbox("При получении команды с сервера возникла ошибка. " & _
                                   vbCrLf & "Пожалуйста сообщите об этом в службу поддержки: " & _
                                    vbCrLf & e.Error.Message)
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
#Region "Game message responses"
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
                        Dim bSWPCB As Boolean = False
                        If CurrentPlayer.GamePlatform = KniffelGamePlatform.kgpVkontakte Or CurrentPlayer.GamePlatform = KniffelGamePlatform.kgpMoiMir Then
                            If CurrentPlayer.Name = "Антон Макаревич" Then bSWPCB = True
                        End If

                        If bSWPCB Then

                            dAddScore.cbWallPublish.Visibility = Windows.Visibility.Visible

                            dAddScore.cbWallPublish.Content = "Опубликовать на стене"
                            dAddScore.cbWallPublish.IsEnabled = True

                        Else
                            dAddScore.cbWallPublish.Visibility = Windows.Visibility.Collapsed
                        End If
                        dAddScore.SetProperties(kp.Total)
                        CurrentPlayer.LastResult = kp.Total
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
        tbTotal.Tag = msg.Username
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
        Cursor = Cursors.Arrow
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
                ShowWaitWindow()
                DicePanelSL1.Style = msg.DieStyle
                HideWaitWindow()
                iMove = 1
                UIState = UIStates.uiPlay
                CurrentPlayer.GameID = msg.GameId
                CurrentPlayer.Rules = msg.Rules
                CurrentPlayer.cr = msg.GameTable
                DicePanelSL1.ClickToFreeze = False
                Button1.Content = "Готов играть"
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
#End Region
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
            tbName.Width = 600 - iHelp.ActualWidth - bInviteFriends.ActualWidth - bTopTen.ActualWidth - iPlayer.ActualWidth - tbName.Margin.Left - tbName.Margin.Right
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
                    spStatusPanel.Visibility = Windows.Visibility.Visible
                    spRoomInfoPanel.Visibility = Visibility.Collapsed
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
                    spStatusPanel.Visibility = Visibility.Collapsed
                    bCreateRoom.Visibility = Windows.Visibility.Collapsed
                    bExit.Visibility = Windows.Visibility.Collapsed
                    spRoomInfoPanel.Visibility = Visibility.Collapsed
                Case UIStates.uiPlay
                    tbName.Text = CurrentPlayer.Name '"Подключен как " & 
                    'bConnect.Visibility = Visibility.Collapsed
                    'bDisconnect.Visibility = Visibility.Visible
                    spСhat.Visibility = Visibility.Visible
                    spPlayMenu.Visibility = Visibility.Collapsed
                    spPlayPanel.Visibility = Visibility.Visible
                    spPlaySingle.Visibility = Visibility.Visible
                    spStatusPanel.Visibility = Visibility.Collapsed
                    spRoomInfoPanel.Visibility = Visibility.Visible
                    Dim strN = "", strS As String = ""
                    ks.GetChempionAsync(KniffelScoreLabel.RuleToStringID(CurrentPlayer.Rules), strN, strS)
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

    Public Function GetPlayerNameById(ByVal Id As String) As String
        For Each lbi As lbiKniffelPlayer In cpMain.lbPlayers.Items
            If lbi.ID = Id Then Return lbi.PlayerName
        Next
        Return "Неизвестный"
    End Function
    Private Sub FileDuplexServiceSendToServiceCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If e.Error Is Nothing Then
            itry = 0
            If e.UserState Is Nothing Then
                Return
            End If

        Else
            If itry < 10 Then
                cpMain.AddMsgToListbox("При отправке команды на сервер возникла ошибка: " & _
                                                                  vbCrLf & e.Error.Message & _
                                                                 vbCrLf & "Пробуем еще раз...")
                SendToServiceAsync(New SendToService(dmLastMessage))
            Else
                itry = 0
                cpMain.AddMsgToListbox("При отправке команды на сервер возникла ошибка: " & _
                                                                   vbCrLf & e.Error.Message)
            End If

        End If
    End Sub

    Private Sub SessionJoinClosed(ByVal sender As Object, ByVal e As EventArgs)
        Dim sessionJoin As JoinSession = CType(sender, JoinSession)
        If sessionJoin.DialogResult = True Then
            Dim jsm As New JoinSessionMessage()
            jsm.UserPass = sessionJoin.Password
            jsm.Username = sessionJoin.Username
            CurrentPlayer.GamePlatform = KniffelGamePlatform.kgpSanet
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

        If Not CurrentPlayer.GameID = 0 Then bExit_Click(bExit, Nothing)

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

        Try
            iuid = CInt(HtmlPage.Document.QueryString("viewer_id"))
        Catch ex As Exception

        End Try

        If iuid = 0 Then
            Dim strvid As String = ""
            Try
                strvid = HtmlPage.Document.QueryString("vid")
            Catch ex As Exception

            End Try

            If strvid = "" Then
                Join()
            Else
                LoadMM()
            End If
        Else
            LoadVk()
        End If

    End Sub
    Private Sub CheckDefRoom()

        Dim strqs As String = ""
        Try
            strqs = HtmlPage.Document.QueryString("rules")
        Catch ex As Exception

        End Try

        Select Case strqs
            Case "baby"
                CurrentPlayer.Rules = KniffelScoreLabel.KniffelRules.krBaby
            Case "full"
                CurrentPlayer.Rules = KniffelScoreLabel.KniffelRules.krExtended
            Case "simple"
                CurrentPlayer.Rules = KniffelScoreLabel.KniffelRules.krSimple
            Case Else
                CurrentPlayer.Rules = KniffelScoreLabel.KniffelRules.krStandart
        End Select
        Try
            strqs = HtmlPage.Document.QueryString("style")
        Catch ex As Exception

        End Try

        Select Case strqs
            Case "0"
                DicePanelSL1.Style = dpStyle.dpsClassic
            Case "1"
                DicePanelSL1.Style = dpStyle.dpsBrutalRed
            Case Else
                DicePanelSL1.Style = dpStyle.dpsBlue
        End Select
    End Sub
#Region "LeaveRoomButton"
    Private Sub bExit_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bExit.MouseEnter
        bExit.Opacity = 1
    End Sub
    Private Sub bExit_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bExit.MouseLeave
        bExit.Opacity = 0.6
    End Sub
    Private Sub bExit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bExit.MouseLeftButtonDown
        If CurrentPlayer.IsMoving Then
            FalseMove(CurrentPlayer.Name)
            CurrentPlayer.IsMoving = False
        End If
        Dim crm As New ConnectGameRoomMessage
        crm.Username = CurrentPlayer.Name
        crm.GameId = 0
        crm.OldGameId = CurrentPlayer.GameID
        Me.SendToServiceAsync(New SendToService(crm))

        UIState = UIStates.uiConnected
        bExit.Opacity = 0.6
    End Sub
    Private Sub FalseMove(ByVal playername As String)
        Dim msg As New ApplyScoreMessage
        msg.GameId = CurrentPlayer.GameID
        msg.Move = iMove
        msg.Username = playername
        msg.ScoreType = 21
        msg.ScoreValue = 0
        Me.SendToServiceAsync(New SendToService(msg))
    End Sub
#End Region
#Region "InviteFriendsButon"
    Private Sub bInviteFriends_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bInviteFriends.MouseEnter
        bInviteFriends.Opacity = 1
    End Sub

    Private Sub bInviteFriends_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bInviteFriends.MouseLeave
        bInviteFriends.Opacity = 0.6
    End Sub
    Private Sub bInviteFriends_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bInviteFriends.MouseLeftButtonDown
        If CurrentPlayer.GamePlatform = KniffelGamePlatform.kgpVkontakte Then
            Vk.Instance.External.ShowInviteBox()
        End If
        bInviteFriends.Opacity = 0.6
    End Sub
#End Region
    Private Sub ConnectToGame(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Cursor Is Cursors.Wait Then Exit Sub
        Cursor = Cursors.Wait
        Dim crm As New ConnectGameRoomMessage
        crm.Username = CurrentPlayer.Name
        crm.Rules = TryCast(sender, KniffelGameRoom).Rules
        crm.GameId = CInt(TryCast(sender, KniffelGameRoom).ID)
        CurrentPlayer.Rules = crm.Rules
        Me.SendToServiceAsync(New SendToService(crm))
    End Sub
#Region "CreateRoom Button"
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
            'ShowWaitWindow()
            'DicePanelSL1.Style = roomJoin.DieStyle
            crm.DieStyle = roomJoin.DieStyle
            'HideWaitWindow()
            Me.SendToServiceAsync(New SendToService(crm))
        End If
    End Sub
#End Region
#Region "HelpButton"
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
#End Region
#Region "ВКонтакте"
    Private Sub LoadVk()
        ShowWaitWindow()
        AddHandler Vk.Instance.VkInitSucceeded, AddressOf Instance_VkInitSucceeded
        AddHandler Vk.Instance.VkInitFailed, AddressOf Instance_VkInitFailed

        Vk.Instance.Initialize(objectID, appID, secret, iuid, False) ' запуск API в тестовом режиме (true|false) -  ID текущего пользователя (при запуске из IFrame ВКонтакте можно импользовать Vk.Instance.Parameters.ViewerID) -  secret вашего приложения (можете найти/установить его в настройках приложения) -  ID вашего приложения (можете найти его в настройках приложения) -  это ID вашего <object/> тэга в HTML разметке

        AddHandler Vk.Instance.External.ApplicationAdded, AddressOf ApplicationAdded
        AddHandler Vk.Instance.External.SettingsChanged, AddressOf VKSettingsChanged
        AddHandler Vk.Instance.Api.DirectRequestCallback, AddressOf Api_DirectRequestCallback

        uids = New Long() {iuid}

        Vk.Instance.Api.IsAppUser(New ApiCallback(Of String)(AddressOf IsAppUserCallback))


    End Sub
    Private Sub GetProfilePhotoCallback(ByVal response As Response(Of VkApi.DataTypes.Profile()))
        'MessageBox.Show("ph")
        CurrentPlayer.PicUrl = response.ResponseData(0).photo
        Try

            iPlayer.Source = New BitmapImage(New Uri(CurrentPlayer.PicUrl))
            iPlayer.Width = 50
            iPlayer.Height = 50
        Catch ex As Exception
            cpMain.AddMsgToListbox("Не удалось загрузить фото по адресу " & CurrentPlayer.PicUrl & _
                                   ex.Message)
        End Try
    End Sub
    Private Sub GetProfileNameCallback(ByVal response As Response(Of VkApi.DataTypes.Profile()))
        Dim jsm As New JoinSessionMessage()
        jsm.Username = response.ResponseData(0).first_name & " " & response.ResponseData(0).last_name
        jsm.UserPass = CStr(response.ResponseData(0).uid)
        CurrentPlayer.GamePlatform = KniffelGamePlatform.kgpVkontakte
        SendToServiceAsync(New SendToService(jsm))
        bInviteFriends.Visibility = Windows.Visibility.Visible
        Cursor = Cursors.Wait
        CheckDefRoom()
        CheckForSettings()
    End Sub
    Private Sub ApplicationAdded(ByVal sender As Object, ByVal e As EventArgs)
        CheckForSettings()
    End Sub

    Private Sub IsAppUserCallback(ByVal response As Response(Of String))
        'MessageBox.Show("cs")
        If response.ResponseData = "1" Then

            Vk.Instance.Api.GetProfiles(uids, VkApi.DataTypes.ProfileFields.photo, New ApiCallback(Of VkApi.DataTypes.Profile())(AddressOf Me.GetProfilePhotoCallback))
            Vk.Instance.Api.GetProfiles(uids, VkApi.DataTypes.ProfileFields.first_name, New ApiCallback(Of VkApi.DataTypes.Profile())(AddressOf Me.GetProfileNameCallback))

        Else
            Vk.Instance.External.ShowInstallBox()
        End If
    End Sub
    Private Sub CheckForSettings()
        'MessageBox.Show("cfs")
        Vk.Instance.Api.GetUserSettings(New ApiCallback(Of VkApi.DataTypes.UserSettings)(AddressOf Me.GetUserSettingsCallback))

    End Sub
    Private Sub GetUserSettingsCallback(ByVal response As Response(Of VkApi.DataTypes.UserSettings))
        If response.ResponseData.ToString.Contains(VkApi.DataTypes.UserSettings.WallPublicationAccess.ToString) Then
            CurrentPlayer.IsWallPublishAllowed = True
        Else
            CurrentPlayer.IsWallPublishAllowed = False
        End If
    End Sub
    Private Sub VKSettingsChanged(ByVal st As VkApi.DataTypes.UserSettings)
        CheckForSettings()
    End Sub
    Public Property VKPhotoUri As String

    Private Sub Instance_VkInitSucceeded(ByVal sender As Object, ByVal e As EventArgs)
        'успешная инициализация
        ' MessageBox.Show("initsu")
    End Sub

    Private Sub Instance_VkInitFailed(ByVal sender As Object, ByVal e As VkInitFailedEventArgs)
        Join()
    End Sub

    
    Private Sub Api_DirectRequestCallback(ByVal requestID As Long, ByVal jsonData As String)
        cpMain.AddMsgToListbox(requestID.ToString())
        cpMain.AddMsgToListbox(jsonData)
    End Sub
#End Region
#Region "MoiMir"
    Private Sub LoadMM()
        ShowWaitWindow()
        Dim num As Integer
        _wrapper = New ClientWrapper("2b5bef209d9201d7b1e9198ddd508a9c", HtmlPage.Document.QueryString)
        Dim queryString As Dictionary(Of String, String) = DirectCast(HtmlPage.Document.QueryString, Dictionary(Of String, String))
        If (queryString.ContainsKey("is_app_user") AndAlso Integer.TryParse(queryString.Item("is_app_user"), num)) Then
            AddHandler _wrapper.Log, AddressOf WrapperLog
            GetUserProfile()
        Else
            MessageBox.Show("Чтобы продолжить, необходимо установить приложение себе на страницу")
        End If


    End Sub
    Public Sub GetUserProfile()
        Try
            _wrapper.BeginGetSelfInfo(Sub(async As IAsyncResult)
                                          Me.Dispatcher.BeginInvoke(New RxCore2.Procedure(Of IAsyncResult)(AddressOf Me.OnGotSelfInfo), New Object() {async})
                                      End Sub)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub OnGotSelfInfo(ByVal result As IAsyncResult)

        Try
            Dim user As IUser = _wrapper.EndGetSelfInfo(result)

            Try
                CurrentPlayer.PicUrl = user.Pic.AbsolutePath
                iPlayer.Source = New BitmapImage(user.Pic)
                iPlayer.Width = 50
                iPlayer.Height = 50
            Catch ex As Exception
                cpMain.AddMsgToListbox("Не удалось загрузить фото по адресу " & _
                                       ex.Message)
            End Try
            Dim jsm As New JoinSessionMessage()
            jsm.Username = user.FirstName & " " & user.LastName
            jsm.UserPass = "mm" & user.Uid
            CurrentPlayer.GamePlatform = KniffelGamePlatform.kgpMoiMir
            SendToServiceAsync(New SendToService(jsm))
            Cursor = Cursors.Wait
            CheckDefRoom()
        Catch exception As Exception
            MessageBox.Show(exception.Message)
        End Try

    End Sub
    Private Sub OnWallPublish(ByVal result As IAsyncResult)

        Try
            If Not _wrapper.Stream.EndPublish(result) Then

            End If


        Catch exception As Exception
            cpMain.AddMsgToListbox(exception.Message)
        End Try

    End Sub
    Private GotUserProfile As RxCore2.Procedure(Of User)
    Private Sub WrapperLog(ByVal obj As String)
        cpMain.AddMsgToListbox(obj)
    End Sub


#End Region
#Region "Game"
    '''PlaySingleGame

    Private IsRolling As Boolean
    Private iRoll As Integer
    Private IsPlaying As Boolean
    Private fmove As Integer
    Private dAddScore As New AddScorePage
    'encrypted Result
    Private ER As String
    Private Property iMove As Integer
        Get
            Return fmove
        End Get
        Set(ByVal value As Integer)
            fmove = value
            tbMove.Text = "ход: " & value
        End Set
    End Property
    Private Sub SendReady(ByVal playername As String)
        Dim msg As New PlayerIsReadyMessage
        msg.GameId = CurrentPlayer.GameID
        msg.Username = playername
        Me.SendToServiceAsync(New SendToService(msg))
    End Sub
    Private Sub AddScore()
        If dAddScore.DialogResult Then


            ' Use the new address with the proxy object.


            If CurrentPlayer.PicUrl = "" Then CurrentPlayer.PicUrl = "na"
            ks.PutScoreIntoTableWithPicAsync(CurrentPlayer.cn, CurrentPlayer.cp, ER, CurrentPlayer.cr, CurrentPlayer.PicUrl)
            If dAddScore.cbWallPublish.IsChecked = True Then

                Try
                    Select Case CurrentPlayer.GamePlatform
                        Case KniffelGamePlatform.kgpVkontakte
                            cpMain.AddMsgToListbox("Попытка писать на стене в контакте")
                            Dim strWallMessageVK As String = "Мой результат в Книффеле! - " & CurrentPlayer.LastResult & " очков! (" & KniffelScoreLabel.RuleToString(CurrentPlayer.Rules) & " правила)"
                            Vk.Instance.Api.DirectRequest("wall.post", New VkApi.DataTypes.Parameter() {New Parameter("owner_id", HtmlPage.Document.QueryString("viewer_id")), New Parameter("message", strWallMessageVK)})
                        Case KniffelGamePlatform.kgpMoiMir
                            cpMain.AddMsgToListbox("Попытка писать на стене в моем мире")
                            Dim strWallMessageMM As String = "Мой результат в [url=invite]Книффеле![/url] - " & CurrentPlayer.LastResult & " очков! (" & KniffelScoreLabel.RuleToString(CurrentPlayer.Rules) & " правила)"
                            _wrapper.Stream.BeginPublish(Sub(async As IAsyncResult)
                                                             Me.Dispatcher.BeginInvoke(New RxCore2.Procedure(Of IAsyncResult)(AddressOf Me.OnWallPublish), New Object() {async})
                                                         End Sub, strWallMessageMM, "Мой результат в Книффель!")
                    End Select

                Catch ex As Exception

                End Try
            End If
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

        SendReady(CurrentPlayer.Name)
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

#Region "Rating"

    Private Sub bTopTen_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bTopTen.MouseEnter
        bTopTen.Opacity = 1
    End Sub

    Private Sub bTopTen_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles bTopTen.MouseLeave
        bTopTen.Opacity = 0.6
    End Sub

    Private Sub bTopTen_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles bTopTen.MouseLeftButtonDown
        pageTT = New TopTenPage
        ks.GetTopPlayersAsync("baby", New System.Collections.ObjectModel.ObservableCollection(Of KniffelPutScore.KniffelScore), "baby")
        ks.GetTopPlayersAsync("simple", New System.Collections.ObjectModel.ObservableCollection(Of KniffelPutScore.KniffelScore), "simple")
        ks.GetTopPlayersAsync("standard", New System.Collections.ObjectModel.ObservableCollection(Of KniffelPutScore.KniffelScore), "standard")
        ks.GetTopPlayersAsync("full", New System.Collections.ObjectModel.ObservableCollection(Of KniffelPutScore.KniffelScore), "full")
        bTopTen_MouseLeave(sender, e)
        pageTT.Show()

    End Sub
    Private Sub GetTopPlayersCallBack(ByVal seder As Object, ByVal e As KniffelPutScore.GetTopPlayersCompletedEventArgs)
        If e.Error Is Nothing Then
            Select Case TryCast(e.UserState, String)
                Case "full"
                    pageTT.dgFull.ItemsSource = e.Players
                Case "baby"
                    pageTT.dgBaby.ItemsSource = e.Players
                Case "standard"
                    pageTT.dgStandard.ItemsSource = e.Players
                Case "simple"
                    pageTT.dgSimple.ItemsSource = e.Players
            End Select
        End If
    End Sub
#End Region
End Class





