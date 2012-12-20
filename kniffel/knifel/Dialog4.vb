Imports System.Windows.Forms

Public Class frmStat
    Dim Statgrid(1000, 3) As String
    Private Sub OK_Button_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    'Private Sub frmStat_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'AxMSFlexGrid1.Row = 0
    '    'AxMSFlexGrid1.Col = 0
    '    'AxMSFlexGrid1.Text = "Игрок"
    '    'AxMSFlexGrid1.Col = 1
    '    'AxMSFlexGrid1.Text = "Игр"
    '    'AxMSFlexGrid1.Col = 2
    '    'AxMSFlexGrid1.Text = "Очков"
    '    'AxMSFlexGrid1.set_ColWidth(3, 1200)
    '    'AxMSFlexGrid1.Text = "Средний балл"
    'End Sub
    Public Sub LoadStat()
        Dim i, j, n, m, g, t As Integer
        Dim con As Boolean
        Dim ta() As String
        ListBox1.Items.Clear()
        j = frmMain.Chemps.aP.Count - 1
        n = 0
        ta = Split(frmMain.Chemps.aP(0), vbTab)
        Statgrid(0, 0) = ta(1)
        Statgrid(0, 1) = "1"
        Statgrid(0, 2) = ta(0)
        Statgrid(0, 3) = ta(0)
        For i = 1 To j
            con = False
            ta = Split(frmMain.Chemps.aP(i), vbTab)
            For m = 0 To n

                If Statgrid(m, 0) = ta(1) Then
                    con = True
                    Statgrid(m, 1) = CStr(CInt(Statgrid(m, 1)) + 1)
                    g = CInt(Statgrid(m, 1))
                    Statgrid(m, 2) = CStr(CInt(Statgrid(m, 2)) + CInt(ta(0)))
                    t = CInt(Statgrid(m, 2))
                    Statgrid(m, 3) = CStr(Math.Round(t / g, 0))
                    Exit For
                End If
            Next m
            If Not con Then
                n += 1
                Statgrid(n, 0) = ta(1)
                Statgrid(n, 1) = "1"
                Statgrid(n, 2) = ta(0)
                Statgrid(n, 3) = ta(0)
            End If
        Next i
        For i = 0 To n
            If Statgrid(i, 0).Length < 15 Then
                For j = 0 To (15 - Statgrid(i, 0).Length)
                    Statgrid(i, 0) = Statgrid(i, 0) & " "
                Next
            End If

            ListBox1.Items.Add(Statgrid(i, 0) & vbTab & Statgrid(i, 1) & vbTab & Statgrid(i, 2) & vbTab & Statgrid(i, 3))
        Next
    End Sub

   
End Class
