
Partial Public Class AboutPage
    Inherits ChildWindow

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        

    End Sub

    Public Property Version As String
        Set(ByVal value As String)
            lVersion.Content = value
        End Set
        Get
            Return lVersion.Content.ToString
        End Get
    End Property
    Private Sub btnJoin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
       
        DialogResult = True
    End Sub

    Private Sub KeyDownHandler(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Key = Key.Enter Then
            btnJoin_Click(lTitle, Nothing)
        End If
    End Sub
    
End Class
