Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input


Partial Public Class SessionJoin
    Inherits ChildWindow
    Private _Password As String
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Private Set(ByVal value As String)
            _Password = value
        End Set
    End Property
    Private privateUsername As String
    Public Property Username() As String
        Get
            Return privateUsername
        End Get
        Private Set(ByVal value As String)
            privateUsername = value
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        DialogResult = False
    End Sub

    Private Sub btnJoin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        
        If txtUser.Text = "" Then Exit Sub
        Username = txtUser.Text '.Trim()
        Password = txtPassword.Password '.Trim()

        DialogResult = True
    End Sub

    Private Sub KeyDownHandler(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Key = Key.Enter Then
            btnJoin_Click(txtUser, Nothing)
        End If
    End Sub
End Class




