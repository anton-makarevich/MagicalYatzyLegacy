
Partial Public Class ThanksPage
    Inherits ChildWindow

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()


    End Sub

    
    Private Sub btnJoin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)

        DialogResult = True
    End Sub

    Private Sub KeyDownHandler(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Key = Key.Enter Then
            btnJoin_Click(Me, Nothing)
        End If
    End Sub

End Class
