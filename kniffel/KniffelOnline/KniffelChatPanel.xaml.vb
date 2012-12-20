Partial Public Class KniffelChatPanel
    Inherits UserControl

    Public Sub New 
        InitializeComponent()
        tbTo.Foreground = New System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)
    End Sub

    
    Private Sub lbPlayers_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles lbPlayers.SelectionChanged
        If e.AddedItems.Count > 0 Then
            tbTo.Text = "для " & TryCast(e.AddedItems(0), lbiKniffelPlayer).PlayerName
            btnToAll.IsEnabled = True
        End If
        If TryCast(sender, ListBox).SelectedItems.Count = 0 Then
            btnToAll.IsEnabled = False
            tbTo.Text = "всем"
        End If

    End Sub

    Private Sub btnToAll_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnToAll.Click
        lbPlayers.SelectedIndex = -1
    End Sub
End Class
