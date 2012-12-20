Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.Text


Namespace Kniffel.Web
    Public Class KniffelGameServiceFactory
        Inherits DuplexServiceFactory(Of KniffelGameService)
    End Class

    <AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
    Public Class KniffelGameService
        Inherits DuplexService
        'Private syncRoot As New Object()

        Private sessionConnections As New List(Of KniffelPlayerInfo)
        Private sessionGames As New List(Of KniffelGameInfo)

        Private ValueSeparator As String = "?.$V!,?"
        Private PlayerSeparator As String = "?.$P!,?"
        Private rand As New Random

        ' Private dbProvider As New KniffelGameOleDbProvider

        ' When a user attempts to connect to game, we check to see if the user name exists in our list.  If no, we add the
        ' joining user's information to the session object, and send a message to the host to inform of the connected user.
        ' The joining user will also receieve a message telling whether the connection succeeded or not.
        Protected Overrides Sub JoinSession(ByVal sessionid As String, ByVal joinMessage As JoinSessionMessage)
BeginJoin:
            Dim jssm As New JoinSessionServerMessage()
            Dim sci As KniffelPlayerInfo = sessionConnections.Where(Function(x) x.Name = joinMessage.Username).FirstOrDefault()

            If sci Is Nothing Then
                Dim kpn As New KniffelPlayerInfo
                kpn.Name = joinMessage.Username
                kpn.Password = joinMessage.UserPass
                kpn.SessionId = sessionid
                kpn.GameId = 0
                sessionConnections.Add(kpn)
                jssm.Name = kpn.Name
                jssm.Password = kpn.Password
                jssm.ID = kpn.SessionId
                jssm.EName = Encrypt(jssm.Name)
                jssm.EPassword = Encrypt(jssm.Password)
                Dim AllPl As String = AllPlayers()
                Dim AllGm As String = AllGames()
                SyncLock syncRoot
                    PushToAllClients(New ClientConnectedMessage() With {.Username = joinMessage.Username, .AllPlayers = AllPl, .AllGames = AllGm})
                End SyncLock
                'dbProvider.InsertPlayer(kpn.Name, kpn.Password, kpn.SessionId, kpn.GameId)
                'CheckActivePlayersInDb()
            Else
                'пробуем отправить мессэдж
                'если не проходит клиента отключает
                'чтобы проверить еще раз смотрим если такой клиент в активной сессии
                'если есть выходим, если нет подключаем этого
                PushMessageToClient(sci.SessionId, New ChatMessage() With {.Message = "Попытка подключения"})
                Dim sci2 As KniffelPlayerInfo = sessionConnections.Where(Function(x) x.Name = joinMessage.Username).FirstOrDefault()
                If sci2 Is Nothing Then GoTo BeginJoin
                If sci2.Password = joinMessage.UserPass Then
                    Dim crm As New ConnectGameRoomMessage
                    crm.GameId = 0
                    crm.Username = sci2.Name
                    ConnectGame(crm)
                    Dim strOldsesiion As String = sci2.SessionId
                    sci2.SessionId = sessionid
                    jssm.Name = sci.Name
                    jssm.Password = sci.Password
                    jssm.ID = sci.SessionId
                    jssm.EName = Encrypt(jssm.Name)
                    jssm.EPassword = Encrypt(jssm.Password)
                    Dim AllPl As String = AllPlayers()
                    Dim AllGm As String = AllGames()
                    ClientDisconnected(strOldsesiion)
                    PushToAllClients(New ClientConnectedMessage() With {.Username = joinMessage.Username, .AllPlayers = AllPl, .AllGames = AllGm})

                Else
                    jssm.Failed = True
                End If



            End If
            PushMessageToClient(OperationContext.Current.Channel.SessionId, jssm)
            If jssm.Failed Then MyBase.ClientDisconnected(sessionid)
        End Sub
        Private Function AllPlayers(Optional ByVal GameId As Integer = 0) As String
            Dim sbAllPlayers As New StringBuilder

            SyncLock syncRoot
                For i As Integer = 0 To sessionConnections.Count - 1
                    Dim kpc As KniffelPlayerInfo = sessionConnections(i)
                    sbAllPlayers.Append(kpc.Name)
                    sbAllPlayers.Append(ValueSeparator)
                    sbAllPlayers.Append(kpc.GameId.ToString)
                    sbAllPlayers.Append(ValueSeparator)
                    sbAllPlayers.Append(kpc.SessionId)
                    If Not i = sessionConnections.Count - 1 Then sbAllPlayers.Append(PlayerSeparator)
                Next
            End SyncLock
            Return sbAllPlayers.ToString
        End Function
        Private Function AllGames() As String
            Dim sbAllPlayers As New StringBuilder

            SyncLock syncRoot
                For i As Integer = 0 To sessionGames.Count - 1
                    Dim kpc As KniffelGameInfo = sessionGames(i)
                    sbAllPlayers.Append(kpc.GameId)
                    sbAllPlayers.Append(ValueSeparator)
                    sbAllPlayers.Append(kpc.IsPlaying.ToString)
                    sbAllPlayers.Append(ValueSeparator)
                    sbAllPlayers.Append(kpc.Rules)
                    sbAllPlayers.Append(ValueSeparator)
                    sbAllPlayers.Append(kpc.Move)
                    If Not i = sessionConnections.Count - 1 Then sbAllPlayers.Append(PlayerSeparator)
                Next
            End SyncLock
            Return sbAllPlayers.ToString
        End Function
        ' A generic method that allows a host or the connected user to send a method to the other (whether it's file or chat related).
        ' The method looks at the sender's internal session id, searchs for an active session with that ID.  If found it sends the
        ' message to the other user.
        Private Sub SendMessage(ByVal msg As ChatMessage)
            If msg.ToID = String.Empty Then
                SyncLock syncRoot
                    For Each kpi As KniffelPlayerInfo In sessionConnections
                        'If kpi.GameId = 0 OrElse kpi.GameId = msg.GameID Then
                        PushMessageToClient(kpi.SessionId, msg)
                        'End If
                    Next
                End SyncLock

            Else
                PushMessageToClient(msg.ToID, msg)
            End If



        End Sub

        Private Function Encrypt(ByVal StringToEncrypt As String) As String
            Dim sbEncr As New StringBuilder
            Dim dblCountLength As Double
            Dim intRandomNumber As Short
            Dim strCurrentChar As String
            Dim intAscCurrentChar As Short
            Dim intInverseAsc As Short
            Dim intAddNinetyNine As Short
            Dim dblMultiRandom As Double
            Dim dblWithRandom As Double
            Dim intCountPower As Short
            Dim intPower As Short
            Dim strConvertToBase As String

            Const intLowerBounds As Short = 10
            Const intUpperBounds As Short = 28

            For dblCountLength = 1 To Len(StringToEncrypt)
                Randomize()
                intRandomNumber = Int((intUpperBounds - intLowerBounds + 1) * Rnd() + intLowerBounds)
                strCurrentChar = Mid(StringToEncrypt, dblCountLength, 1)
                intAscCurrentChar = Asc(strCurrentChar)
                intInverseAsc = 256 - intAscCurrentChar
                intAddNinetyNine = intInverseAsc + 99
                dblMultiRandom = intAddNinetyNine * intRandomNumber
                dblWithRandom = CDbl(Mid(CStr(dblMultiRandom), 1, 2) & intRandomNumber & Mid(CStr(dblMultiRandom), 3, 2))
                For intCountPower = 0 To 5
                    If dblWithRandom / (93 ^ intCountPower) >= 1 Then
                        intPower = intCountPower
                    End If
                Next intCountPower
                strConvertToBase = ""
                For intCountPower = intPower To 0 Step -1
                    strConvertToBase = strConvertToBase & Chr(Int(dblWithRandom / (93 ^ intCountPower)) + 33)
                    dblWithRandom = dblWithRandom Mod 93 ^ intCountPower
                Next intCountPower
                sbEncr.Append(Len(strConvertToBase))
                sbEncr.Append(strConvertToBase)
            Next dblCountLength
            Return sbEncr.ToString
        End Function

        'Private Sub CheckActivePlayersInDb()
        '    Dim al As New List(Of KniffelPlayerInfo)
        '    dbProvider.LoadAllPlayers(al)
        '    For Each kp As KniffelPlayerInfo In al
        '        Dim strName As String = kp.Name
        '        Dim kp2 As KniffelPlayerInfo = sessionConnections.Where(Function(x) x.Name = strName).FirstOrDefault()
        '        If kp2 Is Nothing Then dbProvider.DeletePlayer(strName)
        '    Next
        'End Sub

        ' method to handle messages received.  
        Protected Overrides Sub OnMessage(ByVal sessionId As String, ByVal data As DuplexMessage)
            'TypeOf data Is JoinSessionMessage Then
            'JoinSession(sessionId, TryCast(data, JoinSessionMessage))
            'ElseIf

            If TypeOf data Is CreateGameRoomMessage Then
                CreateGame(TryCast(data, CreateGameRoomMessage))
            ElseIf TypeOf data Is ConnectGameRoomMessage Then
                ConnectGame(TryCast(data, ConnectGameRoomMessage))
            ElseIf TypeOf data Is PlayerIsReadyMessage Then
                StartGame(TryCast(data, PlayerIsReadyMessage))
            ElseIf TypeOf data Is EncrytValueMessage Then
                TryCast(data, EncrytValueMessage).Value = Encrypt(TryCast(data, EncrytValueMessage).Value)
                PushMessageToClient(sessionId, data)
            ElseIf TypeOf data Is ChatMessage Then
                SendMessage(TryCast(data, ChatMessage))
            ElseIf TypeOf data Is DieFixedMessage Then
                FixDie(TryCast(data, DieFixedMessage))
            ElseIf TypeOf data Is DoRollMessage Then
                ReportRoll(TryCast(data, DoRollMessage))
            ElseIf TypeOf data Is ApplyScoreMessage Then
                ApplyScore(TryCast(data, ApplyScoreMessage))
            End If
        End Sub

        Private Sub FixDie(ByVal msg As DieFixedMessage)
            SyncLock syncRoot
                For Each kpi As KniffelPlayerInfo In sessionConnections
                    If Not kpi.GameId = msg.GameId Then Continue For
                    If kpi.Name = msg.Username Then Continue For
                    PushMessageToClient(kpi.SessionId, msg)
                Next
            End SyncLock
        End Sub
        Private Sub ReportRoll(ByVal msg As DoRollMessage)
            SyncLock syncRoot
                ReDim msg.Value(4)
                'В цикл для нормальной игры, за циклом - только книффеля))

                For i As Integer = 0 To 4
                    Dim ii As Integer = rand.Next(1, 7)

                    msg.Value(i) = ii
                Next


                For Each kpi As KniffelPlayerInfo In sessionConnections
                    If Not kpi.GameId = msg.GameId Then Continue For
                    'If kpi.Name = msg.Username Then Continue For
                    PushMessageToClient(kpi.SessionId, msg)

                Next
            End SyncLock
        End Sub
        Private Sub ApplyScore(ByVal msg As ApplyScoreMessage)
            Dim game As KniffelGameInfo = sessionGames.Where(Function(x) x.GameId = msg.GameId).FirstOrDefault()
            If Not game Is Nothing Then
                If game.MovingPlayerName = msg.Username Then

                    SyncLock syncRoot
                        For Each kpi As KniffelPlayerInfo In sessionConnections
                            If Not kpi.GameId = msg.GameId Then Continue For
                            If kpi.Name = msg.Username Then

                                kpi.Move += 1
                                Continue For
                            End If

                            PushMessageToClient(kpi.SessionId, msg)
                        Next
                    End SyncLock
                    DoMove(msg.GameId, msg.Move)
                End If
            End If
        End Sub

        Private Sub StartGame(ByVal msg As PlayerIsReadyMessage)
            Dim game As KniffelGameInfo = sessionGames.Where(Function(x) x.GameId = msg.GameId).FirstOrDefault()
            If Not game Is Nothing Then
                If game.IsPlaying Then Exit Sub
            End If
            Dim bAllReady As Boolean = True
            SyncLock syncRoot
                For Each kpi As KniffelPlayerInfo In sessionConnections
                    If Not kpi.GameId = msg.GameId Then Continue For
                    If kpi.Name = msg.Username Then
                        kpi.Move = 1

                    End If
                    If Not kpi.Move = 1 Then bAllReady = False
                    PushMessageToClient(kpi.SessionId, msg)
                Next
            End SyncLock

            If bAllReady Then
                UpdateGameStatus(msg.GameId, True)
                DoMove(msg.GameId, 1)
            End If
        End Sub
        Private Sub UpdateGameStatus(ByVal gameid As Integer, ByVal IsPlaying As Boolean)
            Dim game As KniffelGameInfo = sessionGames.Where(Function(x) x.GameId = gameid).FirstOrDefault()
            If Not game Is Nothing Then
                game.IsPlaying = IsPlaying
                Dim msg As New GameStatusMessage
                msg.GameId = gameid
                msg.IsPlaying = IsPlaying
                
                PushToAllClients(msg)
                
            End If
        End Sub

        Private Sub DoMove(ByVal GameId As Integer, ByVal move As Integer)

            Dim CurrentPlayer As KniffelPlayerInfo = sessionConnections.Where(Function(x) x.Move = move And x.GameId = GameId).FirstOrDefault()
            If CurrentPlayer Is Nothing Then
                NextMove(GameId, move)
                Exit Sub
            End If
            Dim game As KniffelGameInfo = sessionGames.Where(Function(x) x.GameId = GameId).FirstOrDefault()
            If Not game Is Nothing Then
                game.MovingPlayerName = CurrentPlayer.Name
            End If

            Dim msg As New DoMoveMessage
            msg.GameId = GameId
            msg.Username = CurrentPlayer.Name
            msg.Move = CurrentPlayer.Move
            SyncLock syncRoot
                For Each kpi As KniffelPlayerInfo In sessionConnections
                    If Not (kpi.GameId = GameId Or kpi.GameId = 0) Then Continue For
                    PushMessageToClient(kpi.SessionId, msg)
                Next
            End SyncLock
        End Sub
        Private Sub NextMove(ByVal GameId As Integer, ByVal move As Integer)
            Dim game As KniffelGameInfo = sessionGames.Where(Function(x) x.GameId = GameId).FirstOrDefault()
            game.Move = move
            If Not game Is Nothing Then
                If move = game.MaxMove Then
                    Dim msg As New GameOverMessage
                    msg.GameId = GameId
                    SyncLock syncRoot
                        For Each kpi As KniffelPlayerInfo In sessionConnections
                            If Not kpi.GameId = GameId Then Continue For
                            PushMessageToClient(kpi.SessionId, msg)
                        Next
                    End SyncLock
                    UpdateGameStatus(GameId, False)
                Else
                    Dim m As Integer = move + 1
                    DoMove(GameId, m)
                End If
            End If
        End Sub
        Private Sub CreateGame(ByVal msg As CreateGameRoomMessage)
            Dim game As New KniffelGameInfo
            game.GameId = GetFreeGameId()
            game.Rules = msg.Rules
            game.DieStyle = msg.DieStyle
            sessionGames.Add(game)
            Dim msg2 As New ConnectGameRoomMessage
            msg2.Username = msg.Username
            msg2.GameId = game.GameId
            msg2.Rules = msg.Rules
            ConnectGame(msg2)
        End Sub
        Private Sub ConnectGame(ByVal msg As ConnectGameRoomMessage)

            Select Case msg.Rules
                Case 0
                    msg.GameTable = Encrypt("scores")
                Case 1
                    msg.GameTable = Encrypt("scorese")
                Case 2
                    msg.GameTable = Encrypt("scoresb")
                Case 3
                    msg.GameTable = Encrypt("scoress")
            End Select
            Dim game As KniffelGameInfo = sessionGames.Where(Function(x) x.GameId = msg.GameId).FirstOrDefault()
            If game Is Nothing Then
                msg.DieStyle = 2
            Else
                msg.DieStyle = game.DieStyle
            End If
            game = sessionGames.Where(Function(x) x.GameId = msg.OldGameId).FirstOrDefault()
            SyncLock (syncRoot)
                For Each kpc As KniffelPlayerInfo In sessionConnections
                    If kpc.Name = msg.Username Then kpc.GameId = msg.GameId
                    PushMessageToClient(kpc.SessionId, msg)
                Next
                'если игрок вышел из-за стола в начале игры - обновляем статус игры, чтобы его больше не ждали
                If msg.GameId = 0 Then
                    'Dim game As KniffelGameInfo = sessionGames.Where(Function(x) x.GameId = msg.OldGameId).FirstOrDefault()
                    If Not game Is Nothing Then
                        If Not game.IsPlaying Then
                            Dim bAllReady As Boolean = True

                            For Each kpi As KniffelPlayerInfo In sessionConnections
                                If Not kpi.GameId = msg.OldGameId Then Continue For
                                If Not kpi.Move = 1 Then bAllReady = False
                            Next
                            If bAllReady Then
                                UpdateGameStatus(msg.OldGameId, True)
                                DoMove(msg.OldGameId, 1)
                            End If
                        End If
                    End If
                End If
            End SyncLock
        End Sub
        Private Function GetFreeGameId() As Integer
            Dim i As Integer = 0

            Do
                i += 1
            Loop While IsRoomExist(i)

            Return i
        End Function
        Private Function IsRoomExist(ByVal id As Integer) As Boolean
            Dim bToRemove As KniffelGameInfo
            Dim bRes As Boolean = False
RemoveRoom:
            If Not bToRemove Is Nothing Then sessionGames.Remove(bToRemove)
            SyncLock syncRoot
                For Each b As KniffelGameInfo In sessionGames
                    b.PlayersNumber = GetPlayersInRoom(b.GameId)
                    If b.PlayersNumber = 0 Then
                        bToRemove = b
                        GoTo RemoveRoom
                    End If

                    If b.GameId = id Then bRes = True
                Next
            End SyncLock
            Return bRes


        End Function
        Private Function GetPlayersInRoom(ByVal id As Integer) As Integer
            Dim i As Integer = 0
            'SyncLock syncRoot
            For Each kp As KniffelPlayerInfo In sessionConnections
                If kp.GameId = id Then i += 1
            Next
            'End SyncLock
            Return i
        End Function
        ' When a client disconnects, if it's a host we remove the session and notify the other user.  When it's not the host, we send
        ' a message to the host, but the session remains.
        
        Protected Overrides Sub OnDisconnected(ByVal Id As String)

            Dim sci As KniffelPlayerInfo = sessionConnections.Where(Function(x) (x.SessionId = Id)).FirstOrDefault()
            If Not sci Is Nothing Then
                'If Not sci.GameId = 0 Then
                '    If sci.IsMyMove Then
                '        Dim asm As New ApplyScoreMessage
                '        asm.GameId = sci.GameId
                '        asm.Move = sci.Move
                '        asm.Username = sci.Name
                '        asm.ScoreType = 21
                '        asm.ScoreValue = 0
                '        ApplyScore(asm)
                '    End If
                '    Dim crm As New ConnectGameRoomMessage
                '    crm.Username = sci.Name
                '    crm.GameId = 0
                '    crm.OldGameId = sci.GameId
                '    ConnectGame(crm)
                'End If
                sessionConnections.Remove(sci)
                PushToAllClients(New ClientDisconnectedMessage() With {.Username = sci.Name})
                'dbProvider.DeletePlayer(sci.Name)
                'CheckActivePlayersInDb()
            End If
        End Sub

    End Class

    Public Class KniffelPlayerInfo
        Public Property SessionId As String
        Public Property Name As String
        Public Property Password As String
        Public Property GameId As Integer
        Public Property Move As Integer

    End Class
    Public Class KniffelGameInfo
        Public Property GameId As Integer
        Public Property IsPlaying As Boolean
        Public Property Password As String
        Public Property PlayersNumber As Integer
        Public Property Rules As Integer
        Public Property Move As Integer
        Public Property DieStyle As Integer
        Public Property MovingPlayerName As String
        Public ReadOnly Property MaxMove As Integer
            Get
                Select Case Rules
                    Case 2
                        Return 7
                    Case 1, 0, 3

                        Return 13
                End Select
            End Get
        End Property
    End Class
End Namespace