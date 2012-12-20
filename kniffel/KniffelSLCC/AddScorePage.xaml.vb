Partial Public Class AddScorePage
    Inherits ChildWindow
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub bOk_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bOk.Click
        Me.DialogResult = True
    End Sub

    Private Sub bCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bCancel.Click
        Me.DialogResult = False
    End Sub
    Public Sub SetProperties(ByVal Scores As String)
        lMessage.Content = "Игра закончена, Ваш счет " & Scores

        cbWallPublish.IsChecked = False

        Title = "Результат"
        bOk.Content = "Да"
        bCancel.Content = "Нет"
        Label1.Content = "Добавить в книгу рекордов?"

    End Sub
End Class
