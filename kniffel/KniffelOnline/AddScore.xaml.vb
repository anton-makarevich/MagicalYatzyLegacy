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

End Class
