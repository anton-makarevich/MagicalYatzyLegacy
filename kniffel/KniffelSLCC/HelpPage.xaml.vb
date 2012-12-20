
Partial Public Class HelpPage
    Inherits ChildWindow

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()


    End Sub

    Private Sub bAbout_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bAbout.Click

        Dim fAbout As New AboutPage()
        Try
            fAbout.Version = "1.4.5.11053 beta" 'System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        fAbout.Show()
        'bAbout_MouseLeave(sender, e)
    End Sub

    


    Private Sub btnJoin_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub bThanksto_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bThanksto.Click
        Dim fThanks As New ThanksPage()
        fThanks.Show()
    End Sub
End Class
