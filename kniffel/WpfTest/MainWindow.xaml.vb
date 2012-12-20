Imports System.Resources

Class MainWindow
   
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim r As New Random
        DicePanelWPF1.Style = r.Next(0, 3)
        DicePanelWPF1.RollDelay = r.Next(20, 200)
        DicePanelWPF1.NumDice = 5
        DicePanelWPF1.ClickToFreeze = True
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        DicePanelWPF1.RollDice()
        TextBox1.Text = DicePanelWPF1.Result
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        DicePanelwpf1.Style = 0
        DicePanelWPF1.NumDice = 1
        DicePanelWPF1.ClickToFreeze = True
        DicePanelWPF1.GenAllFrames()
    End Sub
End Class
