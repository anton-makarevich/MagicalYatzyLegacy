Imports System.IO.IsolatedStorage
Imports System.IO
Imports System.Windows.Browser


Partial Public Class JoinSession
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

        
        AddHandler Loaded, AddressOf MainPage_Loaded
    End Sub
    

    Private Sub MainPage_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Using store As IsolatedStorageFile = _
                IsolatedStorageFile.GetUserStoreForSite()
            If store.FileExists("ks.txt") Then
                Using stream As IsolatedStorageFileStream = store.OpenFile("ks.txt", FileMode.Open)
                    Dim reader As New StreamReader(stream)
                    Dim strData As String = reader.ReadLine
                    If strData = "1" Then
                        cbRememberME.IsChecked = True
                        txtUser.Text = reader.ReadLine
                        txtPassword.Password = reader.ReadLine
                    End If
                    reader.Close()
                End Using
            End If
        End Using
        

    End Sub
    
    Private Sub KeyDownHandler(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Key = Key.Enter Then
            btnJoin_Click(txtUser, Nothing)
        End If
    End Sub
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        DialogResult = False
    End Sub

    Private Sub btnJoin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)

        If txtUser.Text = "" Then Exit Sub
        Username = txtUser.Text '.Trim()
        Password = txtPassword.Password '.Trim()

        'save to isolated storage
        Using store As IsolatedStorageFile = _
IsolatedStorageFile.GetUserStoreForSite()
            If store.FileExists("ks.txt") Then store.DeleteFile("ks.txt")
            If cbRememberME.IsChecked Then
                Using stream As IsolatedStorageFileStream = store.CreateFile("ks.txt")
                    Dim writer As New StreamWriter(stream)
                    writer.WriteLine("1")
                    writer.WriteLine(txtUser.Text)
                    writer.WriteLine(txtPassword.Password)
                    writer.Close()
                End Using
            End If
        End Using

        DialogResult = True
    End Sub
    
End Class
