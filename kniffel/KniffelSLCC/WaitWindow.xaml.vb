Imports System.IO.IsolatedStorage
Imports System.IO
Imports System.Windows.Browser
Imports System.Windows.Threading
Imports System.Windows.Media.Imaging

Partial Public Class WaitWindow
    Inherits ChildWindow

    Dim tm As New DispatcherTimer
    Public Sub New()
        InitializeComponent()
        tm.Interval = TimeSpan.FromMilliseconds(400)
        AddHandler tm.Tick, AddressOf UpdateLabel
        tm.Start()
    End Sub
    Private Sub UpdateLabel()
        Dim img As New Image
        Dim strS As String = 0
        Dim r As New Random
        Dim i As Integer = r.Next(0, 3)
        Select Case i
            Case 0
                strS = "xrot."
            Case 1
                strS = "yrot"
            Case 2
                strS = "stop."
        End Select
        i = r.Next(0, 35)
        img.Source = New BitmapImage(New Uri("/images/2/" & strS & i.ToString & ".png", UriKind.Relative))
        img.Width = 25
        img.Height = 25
        spLoading.Children.Add(img)
    End Sub

    
End Class
