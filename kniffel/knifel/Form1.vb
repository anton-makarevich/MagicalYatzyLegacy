Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Imports System.Text
Imports System.Net.Sockets
Imports System.IO
Imports System.Threading
Imports System.Net
Imports System.Xml.Serialization
Imports System.Xml


Public Enum YhatzeeScoreType
    ystNumber = 0
    ystKind = 1
    ystFullHouse = 2
    ystSmallStraight = 3
    ystLargeStraight = 4
    ystYhatzee = 5
    ystChance = 6
End Enum
Public Enum KniffelPlayerType
    kpHuman = 0
    kpComp = 1
    kpNet = 2
    kpNone = 3
End Enum
Public Class frmMain

    Dim Player1 As New kniffelPlayer
    Dim Player2 As New kniffelPlayer
    Dim Player3 As New kniffelPlayer
    Dim Player4 As New kniffelPlayer

    Dim GameRunning As Boolean

    Public ActPlayer As kniffelPlayer
    Public iMove As Integer
    Public iPlayer As Integer
    Public ValueRecorded As Boolean
    Public Chemps As New ChempTable
    Public Setts As New ChempTable
    Public BckColor1 As Color = Color.Green
    Public BckColor2 As Color = Color.White
    Public FntColor As Color = Color.Black

    Private ScoreColor As Color = Color.AntiqueWhite

    Public AllPassPlayers As New ArrayList
    'Public Class ClientHandler
    '    Private nsStream As NetworkStream
    '    Public Client As TcpClient
    '    Private ID As String
    '    Public thisUser As kniffelPlayer
    '    Private oByte As Byte()
    '    Private cSend As String
    '    Private sbNetCommand As StringBuilder

    '    Public Sub New(ByVal client As TcpClient, ByVal ID As String)
    '        Me.Client = client
    '        Me.ID = ID
    '    End Sub
    '    Public Event ConsoleUpdate(ByVal sender As Object, ByVal e As amCommonControls.ConsoleUpdaterEventArgs)
    '    'Public Event UsersUpdate(ByVal sender As Object, ByVal e As UsersUpdateEventArgs)


    '    'Public Sub Start()
    '    '    'Dim myConnection As New OleDbConnection(frmMain.dbConnStr)
    '    '    ' Retrieve the network stream.
    '    '    Dim strILine As String

    '    '    nsStream = Client.GetStream()

    '    '    ' Create a BinaryWriter for writing to the stream.
    '    '    'Dim w As New BinaryWriter(Stream)
    '    '    Dim oSer As XmlSerializer
    '    '    ' Create a BinaryReader for reading from the stream.
    '    '    Dim r As New StreamReader(nsStream)
    '    '    thisUser = New saUser
    '    '    Do
    '    '        If Not Client.Connected Then Exit Do
    '    '        'write to stream



    '    '        'read from stream
    '    '        If nsStream.DataAvailable Then
    '    '            Try
    '    '                strILine = r.ReadLine
    '    '            Catch ex As Exception
    '    '                nsStream.Close()
    '    '                Exit Do
    '    '            End Try

    '    '            Select Case strILine
    '    '                '///////Client wants to login
    '    '                Case ClientMessages.RequestLogin
    '    '                    strILine = r.ReadLine
    '    '                    oSer = New XmlSerializer(thisUser.GetType)
    '    '                    thisUser = oSer.Deserialize(New StringReader(strILine))
    '    '                    If Not fMain.DBProvider.IsUserCorrect(thisUser.user, thisUser.pass) = 0 Then
    '    '                        oByte = saCommon.ConvertToByte(ServerMessages.InvalidUser & vbCrLf)
    '    '                        nsStream.Write(oByte, 0, oByte.Length)
    '    '                        Exit Do
    '    '                    End If

    '    '                    If fMain.IsActiveUser(thisUser) Then
    '    '                        oByte = saCommon.ConvertToByte(ServerMessages.AlreadyConnected & vbCrLf)
    '    '                        nsStream.Write(oByte, 0, oByte.Length)
    '    '                        RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasDisconnected))
    '    '                        RaiseEvent ConsoleUpdate(Me, New ConsoleUpdaterEventArgs("Пользователь " & thisUser.user & " уже подключен"))
    '    '                        Exit Do
    '    '                    End If

    '    '                    oByte = saCommon.ConvertToByte(ServerMessages.OK & vbCrLf)
    '    '                    nsStream.Write(oByte, 0, oByte.Length)
    '    '                    '
    '    '                    RaiseEvent ConsoleUpdate(Me, New ConsoleUpdaterEventArgs("Клиент " & thisUser.user & " подключен"))

    '    '                    'fMain.ConnectNodeInvoke(thisUser)
    '    '                    RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasConnected))
    '    '                    '///////////////////////////////////////////////////////////////////////
    '    '                    '
    '    '                    '///client want to exit
    '    '                Case ClientMessages.Disconnect
    '    '                    RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasDisconnected))
    '    '                    Exit Do
    '    '                    '///////Client wants to send settings
    '    '                Case ClientMessages.SendingSettings
    '    '                    strILine = r.ReadLine
    '    '                    Dim NewSettings As New saUserSettings
    '    '                    oSer = New XmlSerializer(NewSettings.GetType)
    '    '                    NewSettings = oSer.Deserialize(New StringReader(strILine))
    '    '                    '
    '    '                    'RaiseEvent ConsoleUpdate(Me, New ConsoleUpdaterEventArgs("Пользователь " & thisUser.FIO(FSLib.Declension.DeclensionCase.Imenit) & strClientVersion & " ) подключен"))

    '    '                    RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasConnected))
    '    '                    RaiseEvent UserSettingsUpdate(Me, New UserSettingsUpdateEventArgs(thisUser.user, NewSettings))

    '    '                    '///////Client wants to send startstopstate
    '    '                Case ClientMessages.SendingStartStopState
    '    '                    strILine = r.ReadLine
    '    '                    Dim NewSSS As New saStartStopButtonsState
    '    '                    oSer = New XmlSerializer(NewSSS.GetType)
    '    '                    NewSSS = oSer.Deserialize(New StringReader(strILine))
    '    '                    '
    '    '                    'RaiseEvent ConsoleUpdate(Me, New ConsoleUpdaterEventArgs("Пользователь " & thisUser.FIO(FSLib.Declension.DeclensionCase.Imenit) & strClientVersion & " ) подключен"))

    '    '                    RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasConnected))
    '    '                    RaiseEvent StartStopUpdate(Me, New StartStopUpdateEventArgs(thisUser.user, NewSSS))
    '    '                    '///////Client wants to send snapshot

    '    '                    '///////Client wants to send snapshot
    '    '                Case ClientMessages.SendingSnapShot
    '    '                    strILine = r.ReadLine
    '    '                    Dim ssh As New saSnapShot
    '    '                    oSer = New XmlSerializer(ssh.GetType)
    '    '                    ssh = oSer.Deserialize(New StringReader(strILine))
    '    '                    ssh.NewItem = True
    '    '                    fMain.DBProvider.InsertImage(ssh)
    '    '                    RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasConnected))
    '    '                    RaiseEvent NewSnapShot(Me, New SnapShotUpdateEventArgs(thisUser.user, ssh))
    '    '                    '
    '    '                    'RaiseEvent ConsoleUpdate(Me, New ConsoleUpdaterEventArgs("Пользователь " & thisUser.FIO(FSLib.Declension.DeclensionCase.Imenit) & strClientVersion & " ) подключен"))
    '    '            End Select
    '    '        End If
    '    '        System.Threading.Thread.Sleep(250)
    '    '        Try
    '    '            oByte = saCommon.ConvertToByte(ServerMessages.OK & vbCrLf)
    '    '            nsStream.Write(oByte, 0, oByte.Length)
    '    '        Catch ex As Exception
    '    '            RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasDisconnected))
    '    '            Exit Do
    '    '        End Try
    '    '    Loop

    '    '    ' Close the connection socket.
    '    '    Client.Close()
    '    '    'RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasDeleted))
    '    '    RaiseEvent ConsoleUpdate(Me, New amCommonControls.ConsoleUpdaterEventArgs("Cоединение c " & thisUser.user & " закрыто"))
    '    'End Sub
    '    Public Sub SendSettings(ByVal MySettings As saUserSettings)
    '        SyncLock nsStream
    '            Try
    '                sbNetCommand = New StringBuilder

    '                sbNetCommand.Append(ClientMessages.SendingSettings)
    '                sbNetCommand.Append(vbCrLf)

    '                cSend = saCommon.toXML(MySettings)

    '                sbNetCommand.Append(cSend)
    '                sbNetCommand.Append(vbCrLf)

    '                cSend = sbNetCommand.ToString
    '                oByte = saCommon.ConvertToByte(cSend)
    '                nsStream.Write(oByte, 0, oByte.Length)
    '                RaiseEvent ConsoleUpdate(Me, New ConsoleUpdaterEventArgs("Настройки направлены клиенту " & thisUser.user))
    '            Catch ex As Exception
    '                Client.Close()
    '                RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasDisconnected))
    '            End Try
    '        End SyncLock
    '    End Sub
    '    Public Sub StartCopy(ByVal sender As Object, ByVal e As EventArgs)
    '        Try
    '            sbNetCommand = New StringBuilder
    '            sbNetCommand.Append(ServerMessages.StartCopy)
    '            sbNetCommand.Append(vbCrLf)
    '            cSend = sbNetCommand.ToString
    '            oByte = saCommon.ConvertToByte(cSend)
    '            nsStream.Write(oByte, 0, oByte.Length)
    '        Catch ex As Exception
    '            Client.Close()
    '            RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasDisconnected))
    '        End Try

    '    End Sub
    '    Public Sub StopCopy(ByVal sender As Object, ByVal e As EventArgs)
    '        Try
    '            sbNetCommand = New StringBuilder
    '            sbNetCommand.Append(ServerMessages.StopCopy)
    '            sbNetCommand.Append(vbCrLf)
    '            cSend = sbNetCommand.ToString
    '            oByte = saCommon.ConvertToByte(cSend)
    '            nsStream.Write(oByte, 0, oByte.Length)
    '        Catch ex As Exception
    '            Client.Close()
    '            RaiseEvent UsersUpdate(Me, New UsersUpdateEventArgs(thisUser, UsersUpdateEventArgs.UserUpdateEvent.jdUserWasDisconnected))
    '        End Try

    '    End Sub
    'End Class
    Private Sub frmMain_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Chemps.Write()
        Setts.Write()
        End
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer
        Setts.FName = "kniffel"
        Setts.Read()
        If Setts.aP.Count < 14 Then
            For i = 0 To 13 - Setts.aP.Count
                Setts.aP.Add("")
            Next
        End If
        If Not Setts.aP.Item(8) = "" Then frmOptions.TrackBar1.Value = CInt(Setts.aP.Item(8))
        If Not Setts.aP.Item(9) = "" Then frmOptions.TrackBar3.Value = CInt(Setts.aP.Item(9))
        If Not Setts.aP.Item(10) = "" Then frmOptions.TrackBar4.Value = CInt(Setts.aP.Item(10))
        If Not Setts.aP.Item(11) = "" Then frmOptions.TrackBar2.Value = CInt(Setts.aP.Item(11))
        If Not Setts.aP.Item(12) = "" Then frmOptions.TrackBar2.Value = CInt(Setts.aP.Item(12))
        If Not Setts.aP.Item(13) = "" Then frmOptions.CheckBox1.Checked = CBool(Setts.aP.Item(13))
        ApplySettings()

        Chemps.FName = "scores"
        Chemps.Read()

        For i = 0 To Chemps.aP.Count - 1
            Dim ta() As String = Split(Chemps.aP(i), vbTab)
            If ta.Length = 3 Then
                If Not ta(2) = "" Then AllPassPlayers.Add(ta(1) & vbTab & ta(2))
            End If
        Next

        Player1.Location = New Point(564, 440)
        Player2.Location = New Point(564, 80)
        Player3.Location = New Point(18, 470)
        Player4.Location = New Point(673, 470)

        Me.Height = 560
        tbcMain.Visible = False
        TextBox1.Visible = False
        bSendMessage.Visible = False
    End Sub
    Public Function PlayerExist(ByVal name As String, ByRef pass As String) As Boolean
        For Each strValue As String In AllPassPlayers
            Dim ta() As String = Split(strValue, vbTab)
            If ta(0) = name Then
                pass = ta(1)
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub frmMain_Paint(ByVal sender As Object, _
    ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint


        Dim b As LinearGradientBrush

        b = New LinearGradientBrush(ClientRectangle, _
            BckColor1, BckColor2, LinearGradientMode.ForwardDiagonal)
        'Color.Red, Color.Black, LinearGradientMode.ForwardDiagonal)

        e.Graphics.FillRectangle(b, ClientRectangle)
    End Sub



    Private Sub mNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mNew.Click
        GameRunning = True

        If Not Setts.aP.Item(0) = "" Then frmPlayers.TextBox1.Text = Setts.aP.Item(0)
        If Not Setts.aP.Item(1) = "" Then frmPlayers.TextBox2.Text = Setts.aP.Item(1)
        If Not Setts.aP.Item(2) = "" Then frmPlayers.TextBox3.Text = Setts.aP.Item(2)
        If Not Setts.aP.Item(3) = "" Then frmPlayers.TextBox4.Text = Setts.aP.Item(3)

        If Not Setts.aP.Item(4) = "" Then
            frmPlayers.ComboBox1.SelectedIndex = Setts.aP.Item(4)
        Else
            frmPlayers.ComboBox1.SelectedIndex = 0
        End If

        If Not Setts.aP.Item(5) = "" Then
            frmPlayers.ComboBox2.SelectedIndex = Setts.aP.Item(5)
        Else
            frmPlayers.ComboBox2.SelectedIndex = 2
        End If

        If Not Setts.aP.Item(6) = "" Then
            frmPlayers.ComboBox3.SelectedIndex = Setts.aP.Item(6)
        Else
            frmPlayers.ComboBox3.SelectedIndex = 2
        End If

        If Not Setts.aP.Item(7) = "" Then
            frmPlayers.ComboBox4.SelectedIndex = Setts.aP.Item(7)
        Else
            frmPlayers.ComboBox4.SelectedIndex = 2
        End If

        frmPlayers.tbPass1.Text = ""
        frmPlayers.tbPass2.Text = ""
        frmPlayers.tbPass3.Text = ""
        frmPlayers.tbPass4.Text = ""

        If frmPlayers.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Setts.aP.Item(0) = frmPlayers.TextBox1.Text
            Setts.aP.Item(1) = frmPlayers.TextBox2.Text
            Setts.aP.Item(2) = frmPlayers.TextBox3.Text
            Setts.aP.Item(3) = frmPlayers.TextBox4.Text

            Setts.aP.Item(4) = CStr(frmPlayers.ComboBox1.SelectedIndex)
            Setts.aP.Item(5) = CStr(frmPlayers.ComboBox2.SelectedIndex)
            Setts.aP.Item(6) = CStr(frmPlayers.ComboBox3.SelectedIndex)
            Setts.aP.Item(7) = CStr(frmPlayers.ComboBox4.SelectedIndex)

            Dim strPass As String = String.Empty
            If frmPlayers.ComboBox1.SelectedIndex = 0 And PlayerExist(frmPlayers.TextBox1.Text, strPass) = False Then AllPassPlayers.Add(frmPlayers.TextBox1.Text & vbTab & frmPlayers.tbPass1.Text)
            If frmPlayers.ComboBox2.SelectedIndex = 0 And PlayerExist(frmPlayers.TextBox2.Text, strPass) = False Then AllPassPlayers.Add(frmPlayers.TextBox2.Text & vbTab & frmPlayers.tbPass2.Text)
            If frmPlayers.ComboBox3.SelectedIndex = 0 And PlayerExist(frmPlayers.TextBox3.Text, strPass) = False Then AllPassPlayers.Add(frmPlayers.TextBox3.Text & vbTab & frmPlayers.tbPass3.Text)
            If frmPlayers.ComboBox4.SelectedIndex = 0 And PlayerExist(frmPlayers.TextBox4.Text, strPass) = False Then AllPassPlayers.Add(frmPlayers.TextBox4.Text & vbTab & frmPlayers.tbPass4.Text)

            Setts.Write()

            

            'имена игроков
            Player1.Name = frmPlayers.TextBox1.Text
            Player2.Name = frmPlayers.TextBox2.Text
            Player3.Name = frmPlayers.TextBox3.Text
            Player4.Name = frmPlayers.TextBox4.Text

            'Пароли игроков
            Player1.Pass = frmPlayers.tbPass1.Text
            Player2.Pass = frmPlayers.tbPass2.Text
            Player3.Pass = frmPlayers.tbPass3.Text
            Player4.Pass = frmPlayers.tbPass4.Text

            'тип игроков
            Select Case frmPlayers.ComboBox1.SelectedItem
                Case "Человек"
                    Player1.Type = KniffelPlayerType.kpHuman
                Case "Компьютер"
                    Player1.Type = KniffelPlayerType.kpComp
                Case "Нет"
                    Player1.Type = KniffelPlayerType.kpNone
            End Select
            Select Case frmPlayers.ComboBox2.SelectedItem
                Case "Человек"
                    Player2.Type = KniffelPlayerType.kpHuman
                Case "Компьютер"
                    Player2.Type = KniffelPlayerType.kpComp
                Case "Нет"
                    Player2.Type = KniffelPlayerType.kpNone
            End Select
            Select Case frmPlayers.ComboBox3.SelectedItem
                Case "Человек"
                    Player3.Type = KniffelPlayerType.kpHuman
                Case "Компьютер"
                    Player3.Type = KniffelPlayerType.kpComp
                Case "Нет"
                    Player3.Type = KniffelPlayerType.kpNone
            End Select
            Select Case frmPlayers.ComboBox4.SelectedItem
                Case "Человек"
                    Player4.Type = KniffelPlayerType.kpHuman
                Case "Компьютер"
                    Player4.Type = KniffelPlayerType.kpComp
                Case "Нет"
                    Player4.Type = KniffelPlayerType.kpNone
            End Select
StartingGame:
            'Обнуление очков
            Player1.Init()
            Player2.Init()
            Player3.Init()
            Player4.Init()

            dcPanel.Enabled = True
            dcPanel.Visible = True
            dcPanel.ClickToFreeze = True
            dcPanel.NumDice = 5
            SetUpBoard()
            RunGame()
            SetDownBoard()
            
            If GameRunning Then
                If ShowResults() Then GoTo StartingGame
                GameRunning = False
            End If

        End If

    End Sub
    Private Sub RunGame()

        For iMove = 1 To 13
            If Not Player1.Type = KniffelPlayerType.kpNone Then
                ActPlayer = Player1
                Me.Text = "Книффель ход " & iMove & ", ходит " & Player1.Name
                Player1.MakeMove()
            End If
            If Not Player2.Type = KniffelPlayerType.kpNone Then
                ActPlayer = Player2
                Me.Text = "Книффель ход " & iMove & ", ходит " & Player2.Name
                Player2.MakeMove()
            End If
            If Not Player3.Type = KniffelPlayerType.kpNone Then
                ActPlayer = Player3
                Me.Text = "Книффель ход " & iMove & ", ходит " & Player3.Name
                Player3.MakeMove()
            End If
            If Not Player4.Type = KniffelPlayerType.kpNone Then
                ActPlayer = Player4
                Me.Text = "Книффель ход " & iMove & ", ходит " & Player4.Name
                ActPlayer.MakeMove()
            End If
        Next iMove

    End Sub
    Private Function ShowResults() As Boolean
        Dim tb(3) As String

        Dim j As Integer

        frmResults.ListView1.Items.Clear()

        j = -1
        Dim iMax As Integer = 0
        Dim strWin As String = String.Empty
        If Not Player1.Total = "" And Not Player1.Type = KniffelPlayerType.kpNone Then
            Dim lvi As New ListViewItem(Player1.Total)
            If Player1.Total > iMax Then
                iMax = Player1.Total
                strWin = Player1.Name
            End If
            If Not Player1.Type = KniffelPlayerType.kpHuman Then
                lvi.ForeColor = Color.DarkSlateGray

                lvi.BackColor = Color.LightGray
            Else
                lvi.Tag = Player1.Pass
                lvi.Checked = True
            End If
            lvi.SubItems.Add(Player1.Name)
            frmResults.ListView1.Items.Add(lvi)
            Chemps.aP.Add(Player1.Total & vbTab & Player1.Name & vbTab & Player1.Pass)
            j += 1
        End If

        If Not Player2.Total = "" Then
            Dim lvi As New ListViewItem(Player2.Total)
            If Player2.Total > iMax Then
                iMax = Player2.Total
                strWin = Player2.Name

            End If
            If Not Player2.Type = KniffelPlayerType.kpHuman Then
                lvi.ForeColor = Color.DarkSlateGray

                lvi.BackColor = Color.LightGray
            Else
                lvi.Tag = Player2.Pass
                lvi.Checked = True
            End If
            lvi.SubItems.Add(Player2.Name)
            frmResults.ListView1.Items.Add(lvi)
            Chemps.aP.Add(Player2.Total & vbTab & Player2.Name & vbTab & Player2.Pass)
            j += 1
        End If

        If Not Player3.Total = "" Then
            Dim lvi As New ListViewItem(Player3.Total)
            If Player3.Total > iMax Then
                iMax = Player3.Total
                strWin = Player3.Name

            End If
            If Not Player3.Type = KniffelPlayerType.kpHuman Then
                lvi.ForeColor = Color.DarkSlateGray
                lvi.BackColor = Color.LightGray
            Else
                lvi.Tag = Player3.Pass
                lvi.Checked = True
            End If
            lvi.SubItems.Add(Player3.Name)
            frmResults.ListView1.Items.Add(lvi)
            Chemps.aP.Add(Player3.Total & vbTab & Player3.Name & vbTab & Player3.Pass)
            j += 1
        End If

        If Not Player4.Total = "" Then
            Dim lvi As New ListViewItem(Player4.Total)
            If Player4.Total > iMax Then
                iMax = Player4.Total
                strWin = Player4.Name
            End If
            If Not Player4.Type = KniffelPlayerType.kpHuman Then
                lvi.ForeColor = Color.DarkSlateGray
                lvi.BackColor = Color.LightGray
            Else
                lvi.Tag = Player4.Pass
                lvi.Checked = True
            End If
            lvi.SubItems.Add(Player4.Name)
            frmResults.ListView1.Items.Add(lvi)
            Chemps.aP.Add(Player4.Total & vbTab & Player4.Name & vbTab & Player4.Pass)
            j += 1
        End If


        frmResults.ListView1.Sorting = SortOrder.Descending
        frmResults.ListView1.Sort()
        'Dim strWinner As String =
        Me.Text = "Книффель. " & " Конец игры - выиграл " & strWin
        Dim bPlayAgain As Boolean = False
        If frmResults.ShowDialog = Windows.Forms.DialogResult.OK Then
            bPlayAgain = frmResults.cbPlayAgain.Checked
            Dim ws As New sanet.by.kniffel.KniffelService
            Try
                ws.Url = "http://sanet.by/KniffelService.asmx"
SendStatToInet:
                For Each lvi As ListViewItem In frmResults.ListView1.Items
                    If lvi.Checked Then ws.PutScore(Encrypt(lvi.SubItems(1).Text), Encrypt(lvi.Tag), Encrypt(lvi.Text))
                    lvi.Checked = False
                Next

            Catch ex As Exception
                If MsgBox("При отправке результатов игры в интернет возникли проблемы." & vbCrLf & "Проверьте наличие интеренет подключения и нажмите ""Ok"", чтобы попробовать еще раз.", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then GoTo SendStatToInet
            Finally
                ws.Dispose()
            End Try

        End If

        Me.Text = "Книффель"
        Return bPlayAgain
    End Function
    Private Sub SetUpBoard()
        DelLabels(Me)
        MakeLabels()
        WebBrowser1.Visible = False
    End Sub
    Private Sub SetDownBoard()
        DelLabels(Me)
        dcPanel.Enabled = False
        dcPanel.Visible = False
        cbRoll.Visible = False
        WebBrowser1.Visible = False
    End Sub
    ''Сетевая игра
    'Private Sub NewLanGame(ByVal sender As System.Object, ByVal e As System.EventArgs) ' Handles mNew.Click
    '    GameRunning = True

    '    If Not Setts.aP.Item(0) = "" Then frmPlayers.TextBox1.Text = Setts.aP.Item(0)
    '    If Not Setts.aP.Item(1) = "" Then frmPlayers.TextBox2.Text = Setts.aP.Item(1)
    '    If Not Setts.aP.Item(2) = "" Then frmPlayers.TextBox3.Text = Setts.aP.Item(2)
    '    If Not Setts.aP.Item(3) = "" Then frmPlayers.TextBox4.Text = Setts.aP.Item(3)

    '    If Not Setts.aP.Item(4) = "" Then
    '        frmPlayers.ComboBox1.SelectedIndex = Setts.aP.Item(4)
    '    Else
    '        frmPlayers.ComboBox1.SelectedIndex = 0
    '    End If

    '    If Not Setts.aP.Item(5) = "" Then
    '        frmPlayers.ComboBox2.SelectedIndex = Setts.aP.Item(5)
    '    Else
    '        frmPlayers.ComboBox2.SelectedIndex = 2
    '    End If

    '    If Not Setts.aP.Item(6) = "" Then
    '        frmPlayers.ComboBox3.SelectedIndex = Setts.aP.Item(6)
    '    Else
    '        frmPlayers.ComboBox3.SelectedIndex = 2
    '    End If

    '    If Not Setts.aP.Item(7) = "" Then
    '        frmPlayers.ComboBox4.SelectedIndex = Setts.aP.Item(7)
    '    Else
    '        frmPlayers.ComboBox4.SelectedIndex = 2
    '    End If

    '    frmPlayers.tbPass1.Text = ""
    '    frmPlayers.tbPass2.Text = ""
    '    frmPlayers.tbPass3.Text = ""
    '    frmPlayers.tbPass4.Text = ""

    '    If frmPlayers.ShowDialog = System.Windows.Forms.DialogResult.OK Then
    '        Setts.aP.Item(0) = frmPlayers.TextBox1.Text
    '        Setts.aP.Item(1) = frmPlayers.TextBox2.Text
    '        Setts.aP.Item(2) = frmPlayers.TextBox3.Text
    '        Setts.aP.Item(3) = frmPlayers.TextBox4.Text

    '        Setts.aP.Item(4) = CStr(frmPlayers.ComboBox1.SelectedIndex)
    '        Setts.aP.Item(5) = CStr(frmPlayers.ComboBox2.SelectedIndex)
    '        Setts.aP.Item(6) = CStr(frmPlayers.ComboBox3.SelectedIndex)
    '        Setts.aP.Item(7) = CStr(frmPlayers.ComboBox4.SelectedIndex)

    '        Dim strPass As String = String.Empty
    '        If frmPlayers.ComboBox1.SelectedIndex = 0 And PlayerExist(frmPlayers.TextBox1.Text, strPass) = False Then AllPassPlayers.Add(frmPlayers.TextBox1.Text & vbTab & frmPlayers.tbPass1.Text)
    '        If frmPlayers.ComboBox2.SelectedIndex = 0 And PlayerExist(frmPlayers.TextBox2.Text, strPass) = False Then AllPassPlayers.Add(frmPlayers.TextBox2.Text & vbTab & frmPlayers.tbPass2.Text)
    '        If frmPlayers.ComboBox3.SelectedIndex = 0 And PlayerExist(frmPlayers.TextBox3.Text, strPass) = False Then AllPassPlayers.Add(frmPlayers.TextBox3.Text & vbTab & frmPlayers.tbPass3.Text)
    '        If frmPlayers.ComboBox4.SelectedIndex = 0 And PlayerExist(frmPlayers.TextBox4.Text, strPass) = False Then AllPassPlayers.Add(frmPlayers.TextBox4.Text & vbTab & frmPlayers.tbPass4.Text)

    '        Setts.Write()

    '        'Обнуление очков
    '        Player1.Init()
    '        Player2.Init()
    '        Player3.Init()
    '        Player4.Init()

    '        'имена игроков
    '        Player1.Name = frmPlayers.TextBox1.Text
    '        Player2.Name = frmPlayers.TextBox2.Text
    '        Player3.Name = frmPlayers.TextBox3.Text
    '        Player4.Name = frmPlayers.TextBox4.Text

    '        'Пароли игроков
    '        Player1.Pass = frmPlayers.tbPass1.Text
    '        Player2.Pass = frmPlayers.tbPass2.Text
    '        Player3.Pass = frmPlayers.tbPass3.Text
    '        Player4.Pass = frmPlayers.tbPass4.Text

    '        'тип игроков
    '        Select Case frmPlayers.ComboBox1.SelectedItem
    '            Case "Человек"
    '                Player1.Type = KniffelPlayerType.kpHuman
    '            Case "Компьютер"
    '                Player1.Type = KniffelPlayerType.kpComp
    '            Case "Нет"
    '                Player1.Type = KniffelPlayerType.kpNone
    '        End Select
    '        Select Case frmPlayers.ComboBox2.SelectedItem
    '            Case "Человек"
    '                Player2.Type = KniffelPlayerType.kpHuman
    '            Case "Компьютер"
    '                Player2.Type = KniffelPlayerType.kpComp
    '            Case "Нет"
    '                Player2.Type = KniffelPlayerType.kpNone
    '        End Select
    '        Select Case frmPlayers.ComboBox3.SelectedItem
    '            Case "Человек"
    '                Player3.Type = KniffelPlayerType.kpHuman
    '            Case "Компьютер"
    '                Player3.Type = KniffelPlayerType.kpComp
    '            Case "Нет"
    '                Player3.Type = KniffelPlayerType.kpNone
    '        End Select
    '        Select Case frmPlayers.ComboBox4.SelectedItem
    '            Case "Человек"
    '                Player4.Type = KniffelPlayerType.kpHuman
    '            Case "Компьютер"
    '                Player4.Type = KniffelPlayerType.kpComp
    '            Case "Нет"
    '                Player4.Type = KniffelPlayerType.kpNone
    '        End Select
    '        dcPanel.Enabled = True
    '        dcPanel.Visible = True
    '        dcPanel.ClickToFreeze = True
    '        dcPanel.NumDice = 5
    '        DelLabels(Me)
    '        MakeLabels()
    '        RunGame()

    '        DelLabels(Me)
    '        dcPanel.Enabled = False
    '        dcPanel.Visible = False
    '        cbRoll.Visible = False
    '        If GameRunning Then
    '            ShowResults()
    '            GameRunning = False
    '        End If

    '    End If

    'End Sub
    'Private Sub RunLanGame()

    '    For iMove = 1 To 13
    '        If Not Player1.Type = KniffelPlayerType.kpNone Then
    '            ActPlayer = Player1
    '            Me.Text = "Книффель ход " & iMove & ", ходит " & Player1.Name
    '            Player1.MakeMove()
    '        End If
    '        If Not Player2.Type = KniffelPlayerType.kpNone Then
    '            ActPlayer = Player2
    '            Me.Text = "Книффель ход " & iMove & ", ходит " & Player2.Name
    '            Player2.MakeMove()
    '        End If
    '        If Not Player3.Type = KniffelPlayerType.kpNone Then
    '            ActPlayer = Player3
    '            Me.Text = "Книффель ход " & iMove & ", ходит " & Player3.Name
    '            Player3.MakeMove()
    '        End If
    '        If Not Player4.Type = KniffelPlayerType.kpNone Then
    '            ActPlayer = Player4
    '            Me.Text = "Книффель ход " & iMove & ", ходит " & Player4.Name
    '            ActPlayer.MakeMove()
    '        End If
    '    Next iMove

    'End Sub
    'Private Sub ShowLanResults()
    '    Dim tb(3) As String

    '    Dim j As Integer

    '    frmResults.ListView1.Items.Clear()

    '    j = -1
    '    Dim iMax As Integer = 0
    '    Dim strWin As String = String.Empty
    '    If Not Player1.Total = "" And Not Player1.Type = KniffelPlayerType.kpNone Then
    '        Dim lvi As New ListViewItem(Player1.Total)
    '        If Player1.Total > iMax Then
    '            iMax = Player1.Total
    '            strWin = Player1.Name
    '        End If
    '        If Not Player1.Type = KniffelPlayerType.kpHuman Then
    '            lvi.ForeColor = Color.DarkSlateGray

    '            lvi.BackColor = Color.LightGray
    '        Else
    '            lvi.Tag = Player1.Pass
    '            lvi.Checked = True
    '        End If
    '        lvi.SubItems.Add(Player1.Name)
    '        frmResults.ListView1.Items.Add(lvi)
    '        Chemps.aP.Add(Player1.Total & vbTab & Player1.Name & vbTab & Player1.Pass)
    '        j += 1
    '    End If

    '    If Not Player2.Total = "" Then
    '        Dim lvi As New ListViewItem(Player2.Total)
    '        If Player2.Total > iMax Then
    '            iMax = Player2.Total
    '            strWin = Player2.Name

    '        End If
    '        If Not Player2.Type = KniffelPlayerType.kpHuman Then
    '            lvi.ForeColor = Color.DarkSlateGray

    '            lvi.BackColor = Color.LightGray
    '        Else
    '            lvi.Tag = Player2.Pass
    '            lvi.Checked = True
    '        End If
    '        lvi.SubItems.Add(Player2.Name)
    '        frmResults.ListView1.Items.Add(lvi)
    '        Chemps.aP.Add(Player2.Total & vbTab & Player2.Name & vbTab & Player2.Pass)
    '        j += 1
    '    End If

    '    If Not Player3.Total = "" Then
    '        Dim lvi As New ListViewItem(Player3.Total)
    '        If Player3.Total > iMax Then
    '            iMax = Player3.Total
    '            strWin = Player3.Name

    '        End If
    '        If Not Player3.Type = KniffelPlayerType.kpHuman Then
    '            lvi.ForeColor = Color.DarkSlateGray
    '            lvi.BackColor = Color.LightGray
    '        Else
    '            lvi.Tag = Player3.Pass
    '            lvi.Checked = True
    '        End If
    '        lvi.SubItems.Add(Player3.Name)
    '        frmResults.ListView1.Items.Add(lvi)
    '        Chemps.aP.Add(Player3.Total & vbTab & Player3.Name & vbTab & Player3.Pass)
    '        j += 1
    '    End If

    '    If Not Player4.Total = "" Then
    '        Dim lvi As New ListViewItem(Player4.Total)
    '        If Player4.Total > iMax Then
    '            iMax = Player4.Total
    '            strWin = Player4.Name
    '        End If
    '        If Not Player4.Type = KniffelPlayerType.kpHuman Then
    '            lvi.ForeColor = Color.DarkSlateGray
    '            lvi.BackColor = Color.LightGray
    '        Else
    '            lvi.Tag = Player4.Pass
    '            lvi.Checked = True
    '        End If
    '        lvi.SubItems.Add(Player4.Name)
    '        frmResults.ListView1.Items.Add(lvi)
    '        Chemps.aP.Add(Player4.Total & vbTab & Player4.Name & vbTab & Player4.Pass)
    '        j += 1
    '    End If


    '    frmResults.ListView1.Sorting = SortOrder.Descending
    '    frmResults.ListView1.Sort()
    '    'Dim strWinner As String =
    '    Me.Text = "Книффель. " & " Конец игры - выиграл " & strWin
    '    If frmResults.ShowDialog = Windows.Forms.DialogResult.OK Then
    '        Try
    '            Dim ws As New sanet.by.kniffel.KniffelService
    '            ws.Url = "http://sanet.by/KniffelService.asmx"
    '            For Each lvi As ListViewItem In frmResults.ListView1.Items
    '                If lvi.Checked Then ws.PutScore(Encrypt(lvi.SubItems(1).Text), Encrypt(lvi.Tag), Encrypt(lvi.Text))
    '            Next
    '        Catch ex As Exception
    '            MsgBox("При отправке результатов игры в интернет возникли проблемы. Проверьте наличие интеренет подключения.")
    '        End Try

    '    End If

    '    Me.Text = "Книффель"

    'End Sub
    Private Sub ShowBrowser(ByVal url As String)
        Dim ws As New sanet.by.kniffel.KniffelService
        Try
            ws.Url = "http://sanet.by/KniffelService.asmx"
SendStatToInet:
            ws.PutScore("", "", "")
            

        Catch ex As Exception
            If MsgBox("Проверьте наличие интеренет подключения и нажмите ""Ok"", чтобы попробовать еще раз.", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then GoTo SendStatToInet
            Exit Sub
        Finally
            ws.Dispose()
        End Try
        SetDownBoard()
        GameRunning = False
        WebBrowser1.Visible = True
        WebBrowser1.Dock = DockStyle.Fill
        WebBrowser1.Navigate(New Uri(url))
    End Sub

    Private Function Encrypt(ByVal StringToEncrypt As String) As String
        Dim sbEncr As New System.Text.StringBuilder
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
    Private Sub DelLabels(ByVal frm As System.Windows.Forms.Form)
        Dim o As Control
        Dim i As Integer

        For i = 0 To 5
            For Each o In frm.Controls
                If TypeOf o Is AAYhatzeeScoreLabel Then
                    frm.Controls.Remove(o)
                End If
            Next
            For Each o In frm.Controls
                If TypeOf o Is Label Then
                    frm.Controls.Remove(o)
                End If
            Next
            For Each o In frm.Controls
                If TypeOf o Is TotalLabel Then
                    frm.Controls.Remove(o)
                End If
            Next
        Next i
    End Sub
    Private Sub MakeLabels()
        Dim lbName As Label
        Dim lbTotal As TotalLabel
        Dim lbA As Label
        Dim lbY As AAYhatzeeScoreLabel
        Dim f As Font
        Dim y As Integer = 105
        Dim i As Integer
        'Шрифт надписей
        f = New Font("Ariel", 12, FontStyle.Bold Or FontStyle.Italic, GraphicsUnit.Point)
        'подписи таблиц
        For i = 1 To 13
            'первый игрок
            If Not Player1.Type = KniffelPlayerType.kpNone Then
                lbA = New Label
                With lbA
                    .BackColor = Color.Transparent
                    .ForeColor = FntColor 'Color.White
                    .Font = f
                    .Location = New Point(y, 500)
                    .Size = New Size(40, 23)
                    .TextAlign = ContentAlignment.TopCenter
                    Select Case i
                        Case 1
                            .Text = "1"
                        Case 2
                            .Text = "2"
                        Case 3
                            .Text = "3"
                        Case 4
                            .Text = "4"
                        Case 5
                            .Text = "5"
                        Case 6
                            .Text = "6"
                        Case 7
                            .Text = "Т"
                        Case 8
                            .Text = "Ч"
                        Case 9
                            .Text = "FH"
                        Case 10
                            .Text = "SS"
                        Case 11
                            .Text = "LS"
                        Case 12
                            .Text = "K!"
                        Case 13
                            .Text = "Ш"
                    End Select
                End With
                Me.Controls.Add(lbA)
            End If
            'второй игрок
            If Not Player2.Type = KniffelPlayerType.kpNone Then
                lbA = New Label
                With lbA
                    .BackColor = Color.Transparent
                    .ForeColor = FntColor 'Color.White
                    .Font = f
                    .Location = New Point(y, 27)
                    .Size = New Size(40, 23)
                    .TextAlign = ContentAlignment.TopCenter
                    Select Case i
                        Case 1
                            .Text = "1"
                        Case 2
                            .Text = "2"
                        Case 3
                            .Text = "3"
                        Case 4
                            .Text = "4"
                        Case 5
                            .Text = "5"
                        Case 6
                            .Text = "6"
                        Case 7
                            .Text = "Т"
                        Case 8
                            .Text = "Ч"
                        Case 9
                            .Text = "FH"
                        Case 10
                            .Text = "SS"
                        Case 11
                            .Text = "LS"
                        Case 12
                            .Text = "K!"
                        Case 13
                            .Text = "Ш"
                    End Select
                End With
                Me.Controls.Add(lbA)
            End If

            'окошки очков
            'первый игрок
            If Not Player1.Type = KniffelPlayerType.kpNone Then
                lbY = New AAYhatzeeScoreLabel
                With lbY
                    .BackColor = ScoreColor
                    .Font = f
                    .Location = New Point(y, 470)
                    .PlayerName = Player1.Name
                    AddHandler lbY.MouseEnter, AddressOf MouseEnterLabel
                    AddHandler lbY.MouseLeave, AddressOf MouseLeaveLabel
                    AddHandler lbY.MouseDown, AddressOf MouseDownLabel

                    Select Case i
                        Case 1, 2, 3, 4, 5, 6
                            .ScoreType = YhatzeeScoreType.ystNumber
                            .ScoreValue = i
                        Case 7
                            .ScoreType = YhatzeeScoreType.ystKind
                            .ScoreValue = 3
                        Case 8
                            .ScoreType = YhatzeeScoreType.ystKind
                            .ScoreValue = 4
                        Case 9
                            .ScoreType = YhatzeeScoreType.ystFullHouse
                        Case 10
                            .ScoreType = YhatzeeScoreType.ystSmallStraight
                        Case 11
                            .ScoreType = YhatzeeScoreType.ystLargeStraight
                        Case 12
                            .ScoreType = YhatzeeScoreType.ystYhatzee
                        Case 13
                            .ScoreType = YhatzeeScoreType.ystChance
                    End Select
                End With
                Me.Controls.Add(lbY)
            End If
            'второй игрок
            If Not Player2.Type = KniffelPlayerType.kpNone Then
                lbY = New AAYhatzeeScoreLabel
                With lbY
                    .BackColor = ScoreColor
                    .Font = f
                    .Location = New Point(y, 50)
                    .PlayerName = Player2.Name
                    AddHandler lbY.MouseEnter, AddressOf MouseEnterLabel
                    AddHandler lbY.MouseLeave, AddressOf MouseLeaveLabel
                    AddHandler lbY.MouseDown, AddressOf MouseDownLabel
                    Select Case i
                        Case 1, 2, 3, 4, 5, 6
                            .ScoreType = YhatzeeScoreType.ystNumber
                            .ScoreValue = i
                        Case 7
                            .ScoreType = YhatzeeScoreType.ystKind
                            .ScoreValue = 3
                        Case 8
                            .ScoreType = YhatzeeScoreType.ystKind
                            .ScoreValue = 4
                        Case 9
                            .ScoreType = YhatzeeScoreType.ystFullHouse
                        Case 10
                            .ScoreType = YhatzeeScoreType.ystSmallStraight
                        Case 11
                            .ScoreType = YhatzeeScoreType.ystLargeStraight
                        Case 12
                            .ScoreType = YhatzeeScoreType.ystYhatzee
                        Case 13
                            .ScoreType = YhatzeeScoreType.ystChance
                    End Select
                End With
                Me.Controls.Add(lbY)
            End If
            y += 42
        Next

        '
        y = 100
        For i = 1 To 13
            'третий игрок
            If Not Player3.Type = KniffelPlayerType.kpNone Then
                lbA = New Label
                With lbA
                    .BackColor = Color.Transparent
                    .ForeColor = FntColor 'Color.White
                    .Font = f
                    .Location = New Point(5, y)
                    .Size = New Size(40, 23)
                    .TextAlign = ContentAlignment.TopCenter
                    Select Case i
                        Case 1
                            .Text = "1"
                        Case 2
                            .Text = "2"
                        Case 3
                            .Text = "3"
                        Case 4
                            .Text = "4"
                        Case 5
                            .Text = "5"
                        Case 6
                            .Text = "6"
                        Case 7
                            .Text = "Т"
                        Case 8
                            .Text = "Ч"
                        Case 9
                            .Text = "FH"
                        Case 10
                            .Text = "SS"
                        Case 11
                            .Text = "LS"
                        Case 12
                            .Text = "K!"
                        Case 13
                            .Text = "Ш"
                    End Select
                End With
                Me.Controls.Add(lbA)
            End If

            '4 игрок
            If Not Player4.Type = KniffelPlayerType.kpNone Then
                lbA = New Label
                With lbA
                    .BackColor = Color.Transparent
                    .ForeColor = FntColor 'Color.White
                    .Font = f
                    .Location = New Point(665, y)
                    .Size = New Size(40, 23)
                    .TextAlign = ContentAlignment.TopCenter
                    Select Case i
                        Case 1
                            .Text = "1"
                        Case 2
                            .Text = "2"
                        Case 3
                            .Text = "3"
                        Case 4
                            .Text = "4"
                        Case 5
                            .Text = "5"
                        Case 6
                            .Text = "6"
                        Case 7
                            .Text = "Т"
                        Case 8
                            .Text = "Ч"
                        Case 9
                            .Text = "FH"
                        Case 10
                            .Text = "SS"
                        Case 11
                            .Text = "LS"
                        Case 12
                            .Text = "K!"
                        Case 13
                            .Text = "Ш"
                    End Select
                End With
                Me.Controls.Add(lbA)
            End If
            'третий игрок
            If Not Player3.Type = KniffelPlayerType.kpNone Then
                lbY = New AAYhatzeeScoreLabel
                With lbY
                    .BackColor = ScoreColor
                    .Font = f
                    .Location = New Point(50, y)
                    .PlayerName = Player3.Name
                    AddHandler lbY.MouseEnter, AddressOf MouseEnterLabel
                    AddHandler lbY.MouseLeave, AddressOf MouseLeaveLabel
                    AddHandler lbY.MouseDown, AddressOf MouseDownLabel
                    Select Case i
                        Case 1, 2, 3, 4, 5, 6
                            .ScoreType = YhatzeeScoreType.ystNumber
                            .ScoreValue = i
                        Case 7
                            .ScoreType = YhatzeeScoreType.ystKind
                            .ScoreValue = 3
                        Case 8
                            .ScoreType = YhatzeeScoreType.ystKind
                            .ScoreValue = 4
                        Case 9
                            .ScoreType = YhatzeeScoreType.ystFullHouse
                        Case 10
                            .ScoreType = YhatzeeScoreType.ystSmallStraight
                        Case 11
                            .ScoreType = YhatzeeScoreType.ystLargeStraight
                        Case 12
                            .ScoreType = YhatzeeScoreType.ystYhatzee
                        Case 13
                            .ScoreType = YhatzeeScoreType.ystChance
                    End Select
                End With
                Me.Controls.Add(lbY)
            End If
            '4й игрок
            If Not Player4.Type = KniffelPlayerType.kpNone Then
                lbY = New AAYhatzeeScoreLabel
                With lbY
                    .BackColor = ScoreColor
                    .Font = f
                    .Location = New Point(705, y)
                    .PlayerName = Player4.Name
                    AddHandler lbY.MouseEnter, AddressOf MouseEnterLabel
                    AddHandler lbY.MouseLeave, AddressOf MouseLeaveLabel
                    AddHandler lbY.MouseDown, AddressOf MouseDownLabel
                    Select Case i
                        Case 1, 2, 3, 4, 5, 6
                            .ScoreType = YhatzeeScoreType.ystNumber
                            .ScoreValue = i
                        Case 7
                            .ScoreType = YhatzeeScoreType.ystKind
                            .ScoreValue = 3
                        Case 8
                            .ScoreType = YhatzeeScoreType.ystKind
                            .ScoreValue = 4
                        Case 9
                            .ScoreType = YhatzeeScoreType.ystFullHouse
                        Case 10
                            .ScoreType = YhatzeeScoreType.ystSmallStraight
                        Case 11
                            .ScoreType = YhatzeeScoreType.ystLargeStraight
                        Case 12
                            .ScoreType = YhatzeeScoreType.ystYhatzee
                        Case 13
                            .ScoreType = YhatzeeScoreType.ystChance
                    End Select
                End With
                Me.Controls.Add(lbY)
            End If
            y += 26
        Next

        'имя и тотал первого игрока
        If Not Player1.Type = KniffelPlayerType.kpNone Then
            lbTotal = New TotalLabel
            With lbTotal
                .Font = f
                .Location = New Point(450, 440)
                .BackColor = ScoreColor 'Color.LightBlue
                .ForeColor = Color.Black
                .Size = New Size(72, 23)
                .TextAlign = ContentAlignment.TopRight
                .BorderStyle = BorderStyle.FixedSingle
                .PlayerName = Player1.Name
            End With
            Me.Controls.Add(lbTotal)

            lbName = New Label
            With lbName
                .BackColor = Color.Transparent
                .ForeColor = FntColor 'Color.White
                .Font = f
                .Location = New Point(300, 440)
                .Size = New Size(150, 23)
                .Text = Player1.Name
            End With
            Me.Controls.Add(lbName)
        End If
        'имя и тотал 2го игрока
        If Not Player2.Type = KniffelPlayerType.kpNone Then
            lbTotal = New TotalLabel
            With lbTotal
                .Font = f
                .Location = New Point(450, 80)
                .BackColor = ScoreColor 'Color.LightBlue
                .ForeColor = Color.Black
                .Size = New Size(72, 23)
                .TextAlign = ContentAlignment.TopRight
                .BorderStyle = BorderStyle.FixedSingle
                .PlayerName = Player2.Name
            End With
            Me.Controls.Add(lbTotal)

            lbName = New Label
            With lbName
                .BackColor = Color.Transparent
                .ForeColor = FntColor 'Color.White
                .Font = f
                .Location = New Point(300, 80)
                .Size = New Size(150, 23)
                .Text = Player2.Name
            End With
            Me.Controls.Add(lbName)
        End If

        'имя и тотал 3го игрока
        If Not Player3.Type = KniffelPlayerType.kpNone Then
            lbTotal = New TotalLabel
            With lbTotal
                .Font = f
                .Location = New Point(50 - 32, 440)
                .BackColor = ScoreColor 'Color.LightBlue
                .ForeColor = Color.Black
                .Size = New Size(72, 23)
                .TextAlign = ContentAlignment.TopRight
                .BorderStyle = BorderStyle.FixedSingle
                .PlayerName = Player3.Name
            End With
            Me.Controls.Add(lbTotal)

            lbName = New Label
            With lbName
                .BackColor = Color.Transparent
                .ForeColor = FntColor 'Color.White
                .Font = f
                .Location = New Point(50 - 32, 50)
                .Size = New Size(150, 23)
                .Text = Player3.Name
            End With
            Me.Controls.Add(lbName)
        End If
        'имя и тотал 4го игрока
        If Not Player4.Type = KniffelPlayerType.kpNone Then
            lbTotal = New TotalLabel
            With lbTotal
                .Font = f
                .Location = New Point(705 - 32, 440)
                .BackColor = ScoreColor 'Color.LightBlue
                .ForeColor = Color.Black
                .Size = New Size(72, 23)
                .TextAlign = ContentAlignment.TopRight
                .BorderStyle = BorderStyle.FixedSingle
                .PlayerName = Player4.Name
            End With
            Me.Controls.Add(lbTotal)

            lbName = New Label
            With lbName
                .BackColor = Color.Transparent
                .ForeColor = FntColor 'Color.White
                .Font = f
                .Location = New Point(705 - 42, 50)
                .Size = New Size(150, 23)
                .Text = Player4.Name
            End With
            Me.Controls.Add(lbName)
        End If
    End Sub
    Public Sub DoTheRoll()
        Cursor = Cursors.WaitCursor
        Try

            dcPanel.RollDice()
        Finally
            cbRoll.Visible = True
            Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub MouseEnterLabel(ByVal sender As Object, ByVal e As System.EventArgs)

        'если вращаются ничнго
        If Not dcPanel.AllDiceStopped Then Exit Sub


        Dim lb As AAYhatzeeScoreLabel = CType(sender, Label)
        With lb
            If Not lb.PlayerName = ActPlayer.Name And ValueRecorded = False Then Exit Sub
            If Not .ScoreAssigned Then

                Select Case .ScoreType
                    Case YhatzeeScoreType.ystNumber
                        .Text = dcPanel.YhatzeeNumberScore(CInt(.ScoreValue))
                    Case YhatzeeScoreType.ystKind
                        .Text = dcPanel.YhatzeeeOfAKindScore(.ScoreValue)
                    Case YhatzeeScoreType.ystFullHouse
                        .Text = dcPanel.YhatzeeeFullHouseScore
                    Case YhatzeeScoreType.ystSmallStraight
                        .Text = dcPanel.YhatzeeeSmallStraightScore
                    Case YhatzeeScoreType.ystLargeStraight
                        .Text = dcPanel.YhatzeeeLargeStraightScore
                    Case YhatzeeScoreType.ystChance
                        .Text = dcPanel.YhatzeeeChanceScore
                    Case YhatzeeScoreType.ystYhatzee
                        .Text = dcPanel.YhatzeeeFiveOfAKindScore
                End Select

            End If
        End With

    End Sub
    Private Sub MouseDownLabel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        'do nothing if we're rolling
        If Not dcPanel.AllDiceStopped Then Exit Sub
        If ValueRecorded = True Then Exit Sub
        Dim lb As AAYhatzeeScoreLabel = CType(sender, Label)
        With lb
            If Not lb.PlayerName = ActPlayer.Name Then Exit Sub
            If Not .ScoreAssigned Then
                .ScoreAssigned = True

                'If CInt(.Text) = 0 Then
                '    oWav.PlaySynchronous("ouch")
                'Else
                '    oWav.PlaySynchronous("applause")
                'End If

                Call CalcTotal()

                dcPanel.ClearFreeze()
                'cbRoll.Text = Далее"
                'cbRoll.Visible = True
                ValueRecorded = True
                Select Case (ActPlayer.Name)
                    Case Player1.Name
                        Player1.EndMove = True
                    Case Player2.Name
                        Player2.EndMove = True
                    Case Player3.Name
                        Player3.EndMove = True
                    Case Player4.Name
                        Player4.EndMove = True
                End Select
                cbRoll.Visible = False
            End If
        End With

    End Sub
    Private Sub MouseLeaveLabel(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim lb As AAYhatzeeScoreLabel = CType(sender, Label)
        With lb
            If Not .ScoreAssigned Then
                .Text = ""
            End If
        End With

    End Sub
    Private Froll As Integer
    Property pRoll() As Integer
        Get
            Return Froll
        End Get
        Set(ByVal Value As Integer)
            If Value < 1 Or Value > 3 Then
                Throw New Exception("Значение броска должно быть от 1 до 3")
            End If
            Froll = Value
            cbRoll.Visible = Froll < 3
            cbRoll.Text = "Бросок " & Froll + 1
        End Set
    End Property
    ReadOnly Property AllBoxesFilled() As Boolean
        Get
            Dim o As Control

            For Each o In Me.Controls
                If TypeOf o Is AAYhatzeeScoreLabel Then
                    With CType(o, AAYhatzeeScoreLabel)
                        If Not .ScoreAssigned Then
                            Return False
                        End If
                    End With
                End If
            Next

            Return True
        End Get
    End Property
    Private Sub CalcTotal()

        Dim o As Control
        Dim iTot As Integer = 0

        For Each o In Me.Controls
            If TypeOf o Is AAYhatzeeScoreLabel Then
                With CType(o, AAYhatzeeScoreLabel)
                    If .ScoreAssigned And .PlayerName = ActPlayer.Name Then
                        iTot += CInt(.Text)
                    End If
                End With
            End If
        Next

        For Each o In Me.Controls
            If TypeOf o Is TotalLabel Then
                With CType(o, TotalLabel)
                    If .PlayerName = ActPlayer.Name Then
                        .Text = iTot
                        Select Case (ActPlayer.Name)
                            Case Player1.Name
                                Player1.Total = iTot
                            Case Player2.Name
                                Player2.Total = iTot
                            Case Player3.Name
                                Player3.Total = iTot
                            Case Player4.Name
                                Player4.Total = iTot
                        End Select
                    End If
                End With
            End If
        Next
    End Sub

    Private Sub cbRoll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRoll.Click
        If Cursor = Cursors.WaitCursor Then Exit Sub

        If ActPlayer.Type = KniffelPlayerType.kpHuman And iMove < 14 Then
            Select Case cbRoll.Text
                Case "Далее"
                    Select Case (ActPlayer.Name)
                        Case Player1.Name
                            Player1.EndMove = True
                        Case Player2.Name
                            Player2.EndMove = True
                        Case Player3.Name
                            Player3.EndMove = True
                        Case Player4.Name
                            Player4.EndMove = True
                    End Select
                    cbRoll.Visible = False
                Case "Бросок 1"
                    dcPanel.ClearFreeze()
                    ValueRecorded = False
                    DoTheRoll()
                    pRoll = 1
                Case "Бросок 2", "Бросок 3"
                    If dcPanel.AllDiceFrozen Then Exit Sub
                    ValueRecorded = False
                    DoTheRoll()
                    pRoll += 1
            End Select
        End If
    End Sub

    Private Sub mExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mExit.Click
        Chemps.Write()
        End
    End Sub

    Private Sub mChemps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mChemps.Click
        DelLabels(frmChemps)
        frmChemps.Text = "Чемпионы"
        frmChemps.PaintChemps()
        frmChemps.ShowDialog()
    End Sub

    Private Sub mAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mAbout.Click
        AboutBox1.ShowDialog()
    End Sub


    Private Sub mLoosers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mLoosers.Click
        DelLabels(frmChemps)
        frmChemps.Text = "Неудачники"
        frmChemps.PaintChemps()
        frmChemps.ShowDialog()
    End Sub

    Private Sub mStat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mStat.Click
        If Chemps.aP.Count = 0 Then
            MsgBox("Статистика пуста")
            Exit Sub
        End If
        Chemps.Sort(True)
        frmStat.LoadStat()
        frmStat.ShowDialog()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.ActPlayer.EndMove = True
        Timer1.Enabled = False
    End Sub

    Private Sub ПравилаToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ПравилаToolStripMenuItem.Click
        Help.ShowHelp(Me, My.Application.Info.DirectoryPath & "\kniffel.chm")
    End Sub

    Private Sub mOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mOptions.Click
        If Cursor = Cursors.WaitCursor Then Exit Sub
        If Not Setts.aP.Item(8) = "" Then frmOptions.TrackBar1.Value = CInt(Setts.aP.Item(8))
        If Not Setts.aP.Item(9) = "" Then frmOptions.TrackBar3.Value = CInt(Setts.aP.Item(9))
        If Not Setts.aP.Item(10) = "" Then frmOptions.TrackBar4.Value = CInt(Setts.aP.Item(10))
        If Not Setts.aP.Item(11) = "" Then frmOptions.TrackBar2.Value = CInt(Setts.aP.Item(11))
        If Not Setts.aP.Item(12) = "" Then frmOptions.TrackBar5.Value = CInt(Setts.aP.Item(12))
        If Not Setts.aP.Item(13) = "" Then frmOptions.CheckBox1.Checked = CBool(Setts.aP.Item(13))
        If frmOptions.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ApplySettings()
        End If

    End Sub
    Private Sub ApplySettings()
        dcPanel.Style = frmOptions.TrackBar1.Value
        'If Not Setts.aP.Item(8) = CStr(frmOptions.TrackBar1.Value) Then
        Setts.aP.Item(8) = CStr(frmOptions.TrackBar1.Value)
        Select Case frmOptions.TrackBar1.Value
            Case 0
                BckColor1 = Color.Green
                BckColor2 = Color.White
                FntColor = Color.Black
                ScoreColor = Color.AntiqueWhite
            Case 1
                BckColor1 = Color.Red
                BckColor2 = Color.Black
                FntColor = Color.White
                ScoreColor = Color.LightBlue
            Case 2
                BckColor1 = Color.LightBlue
                BckColor2 = Color.SandyBrown
                FntColor = Color.Black
                ScoreColor = Color.Beige
        End Select
        'End If
        dcPanel.DieAngle = frmOptions.TrackBar3.Value
        Setts.aP.Item(9) = CStr(frmOptions.TrackBar3.Value)
        dcPanel.MaxRollLoop = frmOptions.TrackBar4.Value * 10
        Setts.aP.Item(10) = CStr(frmOptions.TrackBar4.Value)
        dcPanel.SetupBackgroundAndDice()
        Me.Timer1.Interval = frmOptions.TrackBar2.Value * 1000 + 1
        Setts.aP.Item(11) = CStr(frmOptions.TrackBar2.Value)
        dcPanel.RollDelay = frmOptions.TrackBar5.Value * 10
        Setts.aP.Item(12) = CStr(frmOptions.TrackBar5.Value)
        dcPanel.PlaySound = frmOptions.CheckBox1.Checked
        Setts.aP.Item(13) = frmOptions.CheckBox1.Checked.ToString
        RefreshLabels()
        Me.Refresh()

    End Sub
    Private Sub RefreshLabels()
        Dim o As Control
        Dim i As Integer

        For i = 0 To 5
            For Each o In Me.Controls
                If TypeOf o Is Label Then
                    With CType(o, Label)
                        .ForeColor = FntColor
                    End With
                End If
            Next
            For Each o In Me.Controls
                If TypeOf o Is AAYhatzeeScoreLabel Then
                    With CType(o, AAYhatzeeScoreLabel)
                        .BackColor = ScoreColor
                        .ForeColor = Color.Black
                    End With
                End If
            Next
            For Each o In Me.Controls
                If TypeOf o Is TotalLabel Then
                    With CType(o, TotalLabel)
                        .BackColor = ScoreColor
                        .ForeColor = Color.Black
                    End With
                End If
            Next
        Next i
    End Sub

    Private Sub bInetStat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bInetStat.Click
        'System.Diagnostics.Process.Start("http://sanet.by/kniffel.aspx")
        ShowBrowser("http://sanet.by/kniffel.aspx")
    End Sub

    Private Sub bHelpOnline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bHelpOnline.Click
        System.Diagnostics.Process.Start("http://sanet.by/help.aspx?docid=kniffel")
    End Sub

    Private Sub mKniffelOnlain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mKniffelOnlain.Click
        'System.Diagnostics.Process.Start("http://kniffel.sanet.by")
    End Sub

    Private Sub mPlayKOE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mPlayKOE.Click
        ShowBrowser("http://kniffel.sanet.by/playkniffel.aspx?rules=full")
    End Sub

    Private Sub mPlayKOS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mPlayKOS.Click
        ShowBrowser("http://kniffel.sanet.by/playkniffel.aspx?")
    End Sub

    Private Sub mPlayKOB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mPlayKOB.Click
        ShowBrowser("http://kniffel.sanet.by/playkniffel.aspx?rules=baby")
    End Sub
End Class

Public Class AAYhatzeeScoreLabel
    Inherits Label

    Public Sub New()
        MyBase.New()

        'defaults
        'BackColor = Color.White 'Color.LightBlue
        ForeColor = Color.Black
        Size = New Size(36, 23)
        TextAlign = ContentAlignment.TopCenter
        BorderStyle = BorderStyle.FixedSingle

    End Sub

    Private FScoreType As YhatzeeScoreType
    Property ScoreType() As YhatzeeScoreType
        Get
            Return FScoreType
        End Get
        Set(ByVal Value As YhatzeeScoreType)
            FScoreType = Value
        End Set
    End Property

    Private FScoreValue As Integer
    Property ScoreValue() As Integer
        Get
            Return FScoreValue
        End Get
        Set(ByVal Value As Integer)
            FScoreValue = Value
        End Set
    End Property

    Private FScoreAssigned As Boolean = False
    Property ScoreAssigned() As Boolean
        Get
            Return FScoreAssigned
        End Get
        Set(ByVal Value As Boolean)
            FScoreAssigned = Value
        End Set
    End Property

    Private FPlayerName As String
    Property PlayerName() As String
        Get
            Return FPlayerName
        End Get
        Set(ByVal Value As String)
            FPlayerName = Value
        End Set
    End Property
    

End Class
Public Class kniffelPlayer
    Private FRoll As Integer
    Public Scores(12) As Integer
    Public ScoresMin() = {50, 1, 2, 3, 4, 5, 6, 22, 17, 25, 30, 40, 25}
    Public ScoresMax() = {50, 5, 10, 15, 20, 25, 30, 30, 30, 25, 30, 40, 30}
    '0-kniffel
    '1-1
    '2-2
    '3-3
    '4
    '5
    '6
    '7-nri
    '8-4et
    '9-fh
    '10-ss
    '11-ls
    '12-ch
    
    Private ReadOnly Property AllNumsFilled() As Boolean
        Get
            Dim i As Integer

            For i = 1 To 6
                If scores(i) = -1 Then Return False
            Next
            Return True
        End Get

    End Property
    Public Sub MakeMove()
        If frmMain.iMove > 13 Then Exit Sub
        Select Case FType
            Case KniffelPlayerType.kpHuman
                frmMain.cbRoll.Location = FLocation
                frmMain.cbRoll.Text = "Бросок 1"
                frmMain.cbRoll.Visible = True

                FEndMove = False

                Do Until FEndMove
                    My.Application.DoEvents()
                Loop
            Case KniffelPlayerType.kpComp
                frmMain.Cursor = Cursors.WaitCursor
                frmMain.dcPanel.ClearFreeze()
                FRoll = 1
RollNow:
                FEndMove = False

                frmMain.dcPanel.RollDice()
                frmMain.Timer1.Enabled = True
                Do Until FEndMove
                    My.Application.DoEvents()
                Loop
                frmMain.dcPanel.ClearFreeze()
                If RollMore() And FRoll < 3 Then
                    FRoll += 1
                    AIFixDices()
                    GoTo RollNow
                End If
                AIDecideFill()
                FillTotal()
                frmMain.Cursor = Cursors.Default
        End Select
    End Sub
    Private Sub FillTotal()
        Dim o As Control
        Dim i, iTot As Integer

        For i = 0 To 12
            If Not Scores(i) = -1 Then
                iTot += Scores(i)
            End If
        Next

        For Each o In frmMain.Controls
            If TypeOf o Is TotalLabel Then
                With CType(o, TotalLabel)
                    If .PlayerName = FName Then
                        .Text = iTot
                        Total = iTot
                    End If
                End With
            End If
        Next
    End Sub
    Private Sub FillNumbers(ByVal index As Integer)
        Dim o As Control
        Dim st As YhatzeeScoreType
        For Each o In frmMain.Controls
            If TypeOf o Is AAYhatzeeScoreLabel Then
                With CType(o, AAYhatzeeScoreLabel)
                    Select Case index
                        Case 0
                            st = YhatzeeScoreType.ystYhatzee
                        Case 1, 2, 3, 4, 5, 6
                            st = YhatzeeScoreType.ystNumber
                        Case 7, 8
                            st = YhatzeeScoreType.ystKind
                        Case 9
                            st = YhatzeeScoreType.ystFullHouse
                        Case 10
                            st = YhatzeeScoreType.ystSmallStraight
                        Case 11
                            st = YhatzeeScoreType.ystLargeStraight
                        Case 12
                            st = YhatzeeScoreType.ystChance
                    End Select
                    If Not .ScoreAssigned And .PlayerName = FName And .ScoreType = st Then
                        Select Case st
                            Case YhatzeeScoreType.ystNumber
                                If .ScoreValue = index Then
                                    .Text = Now(index)
                                    .ScoreAssigned = True
                                    Scores(index) = Now(index)
                                    Exit Sub
                                End If
                            Case YhatzeeScoreType.ystKind
                                If .ScoreValue = index - 4 Then
                                    .Text = Now(index)
                                    .ScoreAssigned = True
                                    Scores(index) = Now(index)
                                    Exit Sub
                                End If
                            Case Else
                                .Text = Now(index)
                                .ScoreAssigned = True
                                Scores(index) = Now(index)
                                Exit Sub
                        End Select
                    End If
                End With
            End If
        Next
    End Sub
    Private Sub AIDecideFill()

        Dim i, j As Integer
        Dim n(6) As Integer

        'сколько кубикоков с каждым значением
        For i = 1 To 6
            n(i) = frmMain.dcPanel.YhatzeeNumberScore(i) / i
        Next
        'проверка на книффель
        If Scores(0) = -1 And Now(0) = 50 Then
            FillNumbers(0)
            Exit Sub
        End If
        'проверка на фн
        If Scores(9) = -1 And Now(0) = 25 Then
            FillNumbers(9)
            Exit Sub
        End If

        If Scores(6) = -1 And n(6) = 4 Then
            FillNumbers(6)
            Exit Sub
        End If

        For i = 11 To 9 Step -1
            If Scores(i) = -1 And Now(i) >= ScoresMin(i) Then
                FillNumbers(i)
                Exit Sub
            End If
        Next
        For i = 8 To 7 Step -1
            If Scores(i) = -1 And Now(i) >= (ScoresMin(i) - Math.Round((frmMain.iMove - 1) / 2, 0)) Then
                FillNumbers(i)
                Exit Sub
            End If
        Next

        For j = 5 To 1 Step -1
            For i = 1 To 6 'Step -1
                If Scores(i) = -1 And n(i) >= j Then
                    FillNumbers(i)
                    Exit Sub
                End If
            Next
        Next
        If Scores(12) = -1 And Now(12) >= (ScoresMin(12) - Math.Round((frmMain.iMove - 1) / 2, 0)) Then
            FillNumbers(12)
            Exit Sub
        End If
        For i = 8 To 7 Step -1
            If Scores(i) = -1 And Now(i) > 0 Then
                FillNumbers(i)
                Exit Sub
            End If
        Next
        For i = 1 To 12
            If Scores(i) = -1 Then
                FillNumbers(i)
                Exit Sub
            End If
        Next
        If Scores(0) = -1 Then
            FillNumbers(0)
            Exit Sub
        End If
    End Sub
    Private Function Now(ByVal index As Integer) As Integer
        Select Case index
            Case 0
                Return frmMain.dcPanel.YhatzeeeFiveOfAKindScore
            Case 1, 2, 3, 4, 5, 6
                Return frmMain.dcPanel.YhatzeeNumberScore(index)
            Case 7, 8
                Return frmMain.dcPanel.YhatzeeeOfAKindScore(index - 4)
            Case 9
                Return frmMain.dcPanel.YhatzeeeFullHouseScore
            Case 10
                Return frmMain.dcPanel.YhatzeeeSmallStraightScore
            Case 11
                Return frmMain.dcPanel.YhatzeeeLargeStraightScore
            Case 12
                Return frmMain.dcPanel.YhatzeeeChanceScore
        End Select
    End Function
    Private Sub AIDecideFill()

        Dim i, j As Integer
        Dim n(6) As Integer

        'сколько кубикоков с каждым значением
        For i = 1 To 6
            n(i) = frmMain.dcPanel.YhatzeeNumberScore(i) / i
        Next
        'проверка на книффель
        If Scores(0) = -1 And Now(0) = 50 Then
            FillNumbers(0)
            Exit Sub
        End If
        'проверка на фн
        If Scores(9) = -1 And Now(0) = 25 Then
            FillNumbers(9)
            Exit Sub
        End If

        If Scores(6) = -1 And n(6) = 4 Then
            FillNumbers(6)
            Exit Sub
        End If

        For i = 11 To 9 Step -1
            If Scores(i) = -1 And Now(i) >= ScoresMin(i) Then
                FillNumbers(i)
                Exit Sub
            End If
        Next
        For i = 8 To 7 Step -1
            If Scores(i) = -1 And Now(i) >= (ScoresMin(i) - Math.Round((frmMain.iMove - 1) / 2, 0)) Then
                FillNumbers(i)
                Exit Sub
            End If
        Next

        For j = 5 To 1 Step -1
            For i = 1 To 6 'Step -1
                If Scores(i) = -1 And n(i) >= j Then
                    FillNumbers(i)
                    Exit Sub
                End If
            Next
        Next
        If Scores(12) = -1 And Now(12) >= (ScoresMin(12) - Math.Round((frmMain.iMove - 1) / 2, 0)) Then
            FillNumbers(12)
            Exit Sub
        End If
        For i = 8 To 7 Step -1
            If Scores(i) = -1 And Now(i) > 0 Then
                FillNumbers(i)
                Exit Sub
            End If
        Next
        For i = 1 To 12
            If Scores(i) = -1 Then
                FillNumbers(i)
                Exit Sub
            End If
        Next
        If Scores(0) = -1 Then
            FillNumbers(0)
            Exit Sub
        End If
    End Sub
    Private Sub AIFixDices()
        Dim Fixed As Boolean = False
        Dim i, j, l As Integer
        Dim n(6) As Integer
        Dim sFH As String

        'сколько кубикоков с каждым значением
        For i = 1 To 6
            n(i) = frmMain.dcPanel.YhatzeeNumberScore(i) / i
        Next
        'проверяем на три пятерки и шестерки
        For i = 6 To 5 Step -1
            If Scores(i) = -1 And n(i) > 2 Then
                frmMain.dcPanel.FixDice(i)
                Exit Sub
            End If
        Next

        'проверяем нужны ли подряд и если больше трех отмечаем
        If Scores(11) = -1 Then
            frmMain.dcPanel.YhatzeeeSmallStraightScore(True, Fixed, 4)
            If Fixed Then Exit Sub
            frmMain.dcPanel.KniffelTreeInRow(Fixed, 4)
            If Fixed Then Exit Sub
        End If
        If Scores(10) = -1 Then
            frmMain.dcPanel.YhatzeeeSmallStraightScore(True, Fixed)
            If Fixed Then Exit Sub
            frmMain.dcPanel.KniffelTreeInRow(Fixed)
            If Fixed Then Exit Sub
        End If

        'прверка на FH
        If Scores(9) = -1 Then
            sFH = ""
            For i = 1 To 6
                If n(i) = 2 Then sFH = sFH & CStr(i)
            Next
            If sFH.Length = 2 Then
                For j = 1 To 2
                    l = CInt(Mid(sFH, j, 1))
                    frmMain.dcPanel.FixDice(l)
                Next
                Exit Sub
            End If
        End If
        For j = 5 To 1 Step -1
            For i = 6 To 1 Step -1

                If j > 2 Or AllNumsFilled() Then
                    If Scores(0) = -1 Or Scores(7) = (-1) Or Scores(8) = (-1) Or Scores(i) = -1 Then
                        If n(i) = j Then
                            frmMain.dcPanel.FixDice(i)
                            Exit Sub
                        End If
                    End If
                Else
                    If Scores(i) = -1 Then '(Scores(0) = -1 Or Scores(7) = (-1) Or Scores(8) = (-1)) And
                        If n(i) = j Then
                            frmMain.dcPanel.FixDice(i)
                            Exit Sub
                        End If
                    End If
                End If
            Next
        Next
        If Scores(12) = -1 Then
            For i = 6 To 5 Step -1
                frmMain.dcPanel.FixDice(i)
            Next
        End If
    End Sub
    Private Function RollMore() As Boolean
        If Scores(0) = -1 And Now(0) = 50 Then Return False
        'If Scores(7) = -1 And Now(7) > 25 Then Return False
        'If Scores(8) = -1 And Now(8) > 25 Then Return False
        If Scores(9) = -1 And Now(9) = 25 Then Return False
        If Scores(11) = -1 Then
            If Now(11) = 40 Then Return False
        Else
            If Scores(10) = -1 And Now(10) = 30 Then Return False
        End If
        Return True
    End Function


    Private FName As String
    Property Name() As String
        Get
            Return FName
        End Get
        Set(ByVal Value As String)
            FName = Value
        End Set
    End Property
    Private FPass As String
    Property Pass() As String
        Get
            Return FPass
        End Get
        Set(ByVal Value As String)
            FPass = Value
        End Set
    End Property

    Private FType As KniffelPlayerType
    Property Type() As KniffelPlayerType
        Get
            Return FType
        End Get
        Set(ByVal Value As KniffelPlayerType)
            FType = Value
        End Set
    End Property
    Private FEndMove As Boolean
    Property EndMove() As Boolean
        Get
            Return FEndMove
        End Get
        Set(ByVal Value As Boolean)
            FEndMove = Value
        End Set
    End Property
    Private FLocation As Point
    Property Location() As Point
        Get
            Return FLocation
        End Get
        Set(ByVal Value As Point)
            FLocation = Value
        End Set
    End Property
    Private FTotal As String
    Property Total() As String
        Get
            Return FTotal
        End Get
        Set(ByVal Value As String)
            Select Case Value.Length
                Case 1
                    Value = "00" & Value
                Case 2
                    Value = "0" & Value
            End Select
            FTotal = Value
        End Set
    End Property
    Public Sub Init()
        Dim i As Integer
        FTotal = Nothing
        For i = 0 To 12
            Scores(i) = -1
        Next
        'ReDim ScoresMin=(50, 5, 10, 15, 20, 25, 30, 30, 30, 25, 30, 40)
    End Sub

End Class
Public Class TotalLabel
    Inherits Label
    Private FPlayerName As String
    Property PlayerName() As String
        Get
            Return FPlayerName
        End Get
        Set(ByVal Value As String)
            FPlayerName = Value
        End Set
    End Property
End Class
Public Class ChempTable
    Public aP As New ArrayList

    Public Sub Sort(ByVal inc As Boolean)
        Dim i, j As Integer
        Dim at As New ArrayList

        If aP.Count = 0 Then Exit Sub
        j = aP.Count - 1
        For i = 0 To j
            If Not aP(i) = Nothing Then
                at.Add(aP(i))
            End If
        Next
        aP.Clear()
        at.Sort()
        j = at.Count - 1
        For i = 0 To j
            If inc Then
                aP.Add(at(j - i))
            Else
                aP.Add(at(i))
            End If
        Next
    End Sub
    Private FFName As String
    Public Property FName() As String
        Get
            Return FFName
        End Get
        Set(ByVal value As String)
            FFName = value
        End Set
    End Property
    Public Sub Read()
        Dim SR As System.IO.StreamReader
        Dim strData As String
        Dim i, j As Integer
        Dim FileName As String
        Dim m As Integer = 0
        Dim TA() As String

        FileName = My.Application.Info.DirectoryPath & "\" & FName & ".hsc"

        Dim FI As New System.IO.FileInfo(FileName)
        If FI.Exists Then
            SR = FI.OpenText
            Try
                TA = Split(Decrypt(SR.ReadLine), vbTab)

                If IsNumeric(TA(0)) And TA(1) = My.Computer.Name Then

                    j = CInt(TA(0))
                    For i = 0 To j
                        strData = Decrypt(SR.ReadLine)
                        TA = Split(strData, vbTab)
                        If IsNumeric(TA(0)) Then m = m + (TA(0) - strData.Length) * (i - 500)
                        aP.Add(strData)
                    Next
                    strData = Decrypt(SR.ReadLine)
                    If Not CStr(m) = strData Then
                        SR.Close()
                        FI.Delete()
                        aP.Clear()
                        MsgBox("Файл " & FName & " поврежден")
                        Exit Sub
                    End If
                Else
                    SR.Close()
                    FI.Delete()
                    aP.Clear()
                    MsgBox("Файл " & FName & " поврежден")
                    Exit Sub
                End If
            Catch ex As Exception
                SR.Close()
                FI.Delete()
                aP.Clear()
                MsgBox("Файл " & FName & " поврежден")
                Exit Sub
            End Try

            SR.Close()
        End If
    End Sub
    Public Sub Write()
        Dim SW As System.IO.StreamWriter
        Dim FileName, strdata As String
        Dim i, j As Integer
        Dim m As Integer = 0
        Dim TA() As String

        If aP.Count = 0 Then Exit Sub
        FileName = My.Application.Info.DirectoryPath & "\" & FName & ".hsc"

        Dim FI As New System.IO.FileInfo(FileName)
        If FI.Exists Then FI.Delete()
        SW = FI.CreateText
        
        j = aP.Count - 1

        SW.WriteLine(Encrypt(CStr(j) & vbTab & My.Computer.Name))
        For i = 0 To j
            TA = Split(aP(i), vbTab)
            If IsNumeric(TA(0)) Then
                strdata = aP(i)
                m = m + (TA(0) - strdata.Length) * (i - 500)
            End If
            SW.WriteLine(Encrypt(aP(i)))
        Next i
        SW.WriteLine(Encrypt(m))
        SW.Close()
    End Sub
    Private Function Encrypt(ByVal StringToEncrypt As String) As String

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
        Encrypt = ""
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
            Encrypt = Encrypt & Len(strConvertToBase) & strConvertToBase
        Next dblCountLength

    End Function

    Private Function Decrypt(ByVal StringToDecrypt As String) As String

        Dim dblCountLength As Double
        Dim intLengthChar As Short
        Dim strCurrentChar As String
        Dim dblCurrentChar As Double
        Dim intCountChar As Short
        Dim intRandomSeed As Short
        Dim intBeforeMulti As Short
        Dim intAfterMulti As Short
        Dim intSubNinetyNine As Short
        Dim intInverseAsc As Short

        Decrypt = ""
        For dblCountLength = 1 To StringToDecrypt.Length
            intLengthChar = CShort(Mid(StringToDecrypt, dblCountLength, 1))
            strCurrentChar = Mid(StringToDecrypt, dblCountLength + 1, intLengthChar)
            dblCurrentChar = 0
            For intCountChar = 1 To Len(strCurrentChar)
                dblCurrentChar = dblCurrentChar + (Asc(Mid(strCurrentChar, intCountChar, 1)) - 33) * (93 ^ (Len(strCurrentChar) - intCountChar))
            Next intCountChar
            intRandomSeed = CShort(Mid(CStr(dblCurrentChar), 3, 2))
            intBeforeMulti = CShort(Mid(CStr(dblCurrentChar), 1, 2) & Mid(CStr(dblCurrentChar), 5, 2))
            intAfterMulti = intBeforeMulti / intRandomSeed
            intSubNinetyNine = intAfterMulti - 99
            intInverseAsc = 256 - intSubNinetyNine
            Decrypt = Decrypt & Strings.Chr(intInverseAsc)
            dblCountLength = dblCountLength + intLengthChar

        Next dblCountLength

    End Function
End Class
