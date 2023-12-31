﻿Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Runtime.Serialization

Namespace Kniffel.Web

    ' User initiating to join an existing session
    <DataContract()> _
    Public Class JoinSessionMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
                Public UserPass As String
    End Class

    
    ' Lets host know that user has disconnected
    <DataContract()> _
    Public Class ClientDisconnectedMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
    End Class


    ' Message to the host, informing them of a client connection
    <DataContract()> _
    Public Class ClientConnectedMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public AllPlayers As String
        <DataMember()> _
        Public AllGames As String
    End Class
    ' Message to other clients, informing them of a client enter room
    <DataContract()> _
    Public Class ClientEnterRoomServerMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public RoomId As Integer
        <DataMember()> _
        Public AllPlayers As String

    End Class
    ' Message to server, informing them of a client enter room
    <DataContract()> _
    Public Class ClientEnterRoomMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public RoomId As Integer
        
    End Class
    ' Message back to the user trying to join, generated by the server
    <DataContract()> _
    Public Class JoinSessionServerMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Failed As Boolean
        <DataMember()> _
        Public Name As String
        <DataMember()> _
        Public EName As String
        <DataMember()> _
        Public Password As String
        <DataMember()> _
        Public EPassword As String
        <DataMember()> _
        Public ID As String
    End Class

    'message to encrypt
    <DataContract()> _
    Public Class EncrytValueMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Value As String
       
    End Class
    'message to create a game room
    <DataContract()> _
    Public Class CreateGameRoomMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public Rules As Integer
        <DataMember()> _
        Public DieStyle As Integer
    End Class
    'message to connect to a game room
    <DataContract()> _
    Public Class ConnectGameRoomMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public GameId As Integer
        <DataMember()> _
        Public OldGameId As Integer
        <DataMember()> _
        Public Rules As Integer
        <DataMember()> _
        Public DieStyle As Integer
        <DataMember()> _
        Public GameTable As String
    End Class
    'message to report player is ready
    <DataContract()> _
    Public Class PlayerIsReadyMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public GameId As Integer
    End Class
    'message to change die fixed state
    <DataContract()> _
    Public Class DieFixedMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public GameId As Integer
        <DataMember()> _
        Public Value As Integer
        <DataMember()> _
        Public Fixed As Boolean
    End Class
    'message to change die fixed state
    <DataContract()> _
    Public Class DoMoveMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public GameId As Integer
        <DataMember()> _
        Public Move As Integer

    End Class
    'message to report of rolling
    <DataContract()> _
    Public Class DoRollMessage
        Inherits DuplexMessage
        '<DataMember()> _
        'Public Username As String
        <DataMember()> _
        Public GameId As Integer
        <DataMember()> _
        Public Value(4) As Integer
    End Class
    'message to applyscore
    <DataContract()> _
    Public Class ApplyScoreMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Username As String
        <DataMember()> _
        Public GameId As Integer
        <DataMember()> _
        Public ScoreType As Integer
        <DataMember()> _
        Public ScoreValue As Integer
        <DataMember()> _
        Public HaveBonus As Boolean
        <DataMember()> _
        Public Move As Integer
    End Class
    'message to applyscore
    <DataContract()> _
    Public Class GameOverMessage
        Inherits DuplexMessage
        <DataMember()> _
                Public GameId As Integer
    End Class
    'message to report that gamestatus is changed
    <DataContract()> _
    Public Class GameStatusMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public GameId As Integer
        <DataMember()> _
        Public IsPlaying As Boolean
    End Class
    ' Generic chat message
    <DataContract()> _
    Public Class ChatMessage
        Inherits DuplexMessage
        <DataMember()> _
        Public Message As String
        <DataMember()> _
        Public ToID As String
        <DataMember()> _
        Public FromID As String
        <DataMember()> _
        Public GameID As Integer
    End Class

    <DataContract()> _
    Public Class GeneralError
        Inherits DuplexMessage
        <DataMember()> _
        Public ErrorMessage As String
    End Class

End Namespace
