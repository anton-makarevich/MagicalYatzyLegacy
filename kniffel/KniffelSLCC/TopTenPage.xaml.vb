Imports Kniffel
Partial Public Class TopTenPage
    Inherits ChildWindow

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
       
        
    End Sub
    'Private Sub ReorderColumns(ByVal sender As System.Object, ByVal e As System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs) Handles dgBaby.AutoGeneratingColumn, dgFull.AutoGeneratingColumn, dgSimple.AutoGeneratingColumn, dgStandard.AutoGeneratingColumn
    '    Dim dg As DataGrid = TryCast(sender, DataGrid)
    '    'If TryCast(dg.Tag, String).Length = 0 Then dg.Tag = "0"
    '    If TryCast(dg.Tag, String).Length > 5 Then Exit Sub
    '    dg.Tag = dg.Tag & "1"
    '    Try
    '        Select Case TryCast(e.Column.Header, String)

    '            Case "Place"
    '                e.Column.Header = "Место"
    '                e.Column.DisplayIndex = 0
    '            Case "Player"
    '                e.Column.Header = "Имя игрока"
    '                e.Column.DisplayIndex = 0
    '            Case "PicUrl"
    '                e.Column.Visibility = Windows.Visibility.Collapsed
    '            Case "HighScore"
    '                e.Column.Header = "Лучший результат"
    '                e.Column.DisplayIndex = 2
    '            Case "Total"
    '                e.Column.Header = "Всего очков"
    '                e.Column.DisplayIndex = 4
    '            Case "Games"
    '                e.Column.Header = "Всего игр"
    '                e.Column.DisplayIndex = 3
    '        End Select
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    End Try

    'End Sub


    Private Sub btnJoin_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        DialogResult = True

    End Sub
End Class
