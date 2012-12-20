Imports System.Text
Partial Public Class ChatPanel
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
        tbTo.Foreground = New System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)
    End Sub
    Public Sub AddMsgToListbox(ByVal message As String)
        Dim lbi As New ListBoxItem
        'lbi.MaxWidth = cpMain.lbMessages.Width
        Dim sp As New StackPanel
        sp.Orientation = Orientation.Horizontal
        'Dim im As New Image
        'im.Source = New BitmapImage(New Uri("Images/0/stop.3.png", UriKind.Relative))
        'sp.Children.Add(im)
        Dim tb As New TextBlock
        tb.TextWrapping = Windows.TextWrapping.Wrap
        tb.Text = MakeMultilineMessage(message)
        sp.Children.Add(tb)
        lbi.Content = sp 'MakeMultilineMessage(message)
        lbMessages.Items.Insert(0, lbi)
        lbMessages.SelectedItem = lbi
        lbMessages.ScrollIntoView(lbi)
    End Sub
    Private Function MakeMultilineMessage(ByVal message As String) As String
        Dim sb As New StringBuilder
        Dim iLine As Integer = 0
        Dim ta() As String = Split(message, " ")
        For i As Integer = 0 To ta.Length - 1
            sb.Append(ta(i))
            iLine += ta(i).Length
            If Not i = ta.Length - 1 Then
                If iLine + ta(i + 1).Length < 55 Then
                    sb.Append(" ")
                Else
                    sb.Append(vbCrLf)
                    iLine = 0
                End If
            End If
        Next
        Return sb.ToString
    End Function
    Private Sub lbPlayers_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles lbPlayers.SelectionChanged
        If e.AddedItems.Count > 0 Then
            tbTo.Text = "для " & TryCast(e.AddedItems(0), lbiGamePlayer).PlayerName
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

