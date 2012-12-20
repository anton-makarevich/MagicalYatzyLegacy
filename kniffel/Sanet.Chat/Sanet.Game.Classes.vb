Public Class lbiGamePlayer
    Inherits ListBoxItem
    Public Property ID As String
    Public Property PlayerName As String
        Set(ByVal value As String)
            _playername = value
            SetContent()
        End Set
        Get
            Return _playername
        End Get
    End Property
    Private _gameid As String
    Public Property GameID As String
        Set(ByVal value As String)
            _gameid = value
            SetContent()
        End Set
        Get
            Return _gameid
        End Get
    End Property
    Private _playername As String

    Private Sub SetContent()
        If GameID = "0" Then
            Content = PlayerName
        Else
            Content = PlayerName & " (" & GameID & ")"
        End If
    End Sub

End Class
