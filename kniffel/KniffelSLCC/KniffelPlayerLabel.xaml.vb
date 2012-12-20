Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows

Partial Public Class KniffelPlayerLabel
    Inherits UserControl
    Private ValueIsSet As Boolean
    Public Event ValueChanged()

    Public Sub New()
        InitializeComponent()

    End Sub
   
End Class
