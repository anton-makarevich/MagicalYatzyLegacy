Imports System.Windows.Forms
Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Public Class frmChemps

    Public Sub PaintChemps()
        Dim y As Integer = 20
        Dim i, j As Integer
        Dim lbA As Label
        Dim f As Font
        Dim ta() As String
        f = New Font("Ariel", 12, FontStyle.Bold Or FontStyle.Italic, GraphicsUnit.Point)
        If Me.Text = "Чемпионы" Then
            frmMain.Chemps.Sort(True)
        Else
            frmMain.Chemps.Sort(False)
        End If
        If frmMain.Chemps.aP.Count - 1 > 9 Then
            j = 9
        Else
            j = frmMain.Chemps.aP.Count - 1
        End If
        For i = 0 To j
            lbA = New Label
            With lbA
                .BackColor = Color.Transparent
                .ForeColor = frmMain.FntColor
                .Font = f
                .Location = New Point(20, y)
                .Size = New Size(400, 23)
                .TextAlign = ContentAlignment.MiddleCenter
                ta = Split(frmMain.Chemps.aP(i), vbTab)
                .Text = CStr(i + 1) & ".  " & ta(1) & " - " & ta(0)

            End With
            Me.Controls.Add(lbA)
            y += 23
        Next
    End Sub

    Private Sub Dialog3_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim b As LinearGradientBrush

        b = New LinearGradientBrush(ClientRectangle, _
            frmMain.BckColor1, frmMain.BckColor2, LinearGradientMode.ForwardDiagonal)

        e.Graphics.FillRectangle(b, ClientRectangle)
    End Sub

    Private Sub OK_Button_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class
