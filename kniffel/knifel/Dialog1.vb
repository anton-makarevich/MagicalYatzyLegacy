Imports System.Windows.Forms

Public Class frmPlayers

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim strPass As String = String.Empty
        If frmMain.PlayerExist(TextBox1.Text, strPass) Then
            If ComboBox1.SelectedIndex = 1 Then
                MsgBox("Имя " & TextBox1.Text & " зарезервировано игроком-человеком")
                Exit Sub
            Else
                If Not tbPass1.Text = strPass And ComboBox1.SelectedIndex = 0 Then
                    MsgBox("Пароль для игрока " & TextBox1.Text & " введен не верно")
                    Exit Sub
                End If
            End If
        Else
            If ComboBox1.SelectedIndex = 0 AndAlso tbPass1.Text = "" Then
                If MsgBox("Для ведения индивидуальной статистики и защиты результатов игры, рекомендуется ввести пароль для игрока " & TextBox1.Text & "." & vbCrLf & "Вы хотите добавить пароль?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then Exit Sub
            End If
        End If
        If frmMain.PlayerExist(TextBox2.Text, strPass) Then
            If ComboBox2.SelectedIndex = 1 Then
                MsgBox("Имя " & TextBox2.Text & " зарезервировано игроком-человеком")
                Exit Sub
            Else
                If Not tbPass2.Text = strPass And ComboBox2.SelectedIndex = 0 Then
                    MsgBox("Пароль для игрока " & TextBox2.Text & " введен не верно")
                    Exit Sub
                End If
            End If
        Else
            If ComboBox2.SelectedIndex = 0 AndAlso tbPass2.Text = "" Then
                If MsgBox("Для ведения индивидуальной статистики и защиты результатов игры, рекомендуется ввести пароль для игрока " & TextBox2.Text & "." & vbCrLf & "Вы хотите добавить пароль?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then Exit Sub
            End If
        End If
        If frmMain.PlayerExist(TextBox3.Text, strPass) Then
            If ComboBox3.SelectedIndex = 1 Then
                MsgBox("Имя " & TextBox3.Text & " зарезервировано игроком-человеком")
                Exit Sub
            Else
                If Not tbPass3.Text = strPass And ComboBox3.SelectedIndex = 0 Then
                    MsgBox("Пароль для игрока " & TextBox3.Text & " введен не верно")
                    Exit Sub
                End If
            End If
        Else
            If ComboBox3.SelectedIndex = 0 AndAlso tbPass3.Text = "" Then
                If MsgBox("Для ведения индивидуальной статистики и защиты результатов игры, рекомендуется ввести пароль для игрока " & TextBox3.Text & "." & vbCrLf & "Вы хотите добавить пароль?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then Exit Sub
            End If
        End If
        If frmMain.PlayerExist(TextBox4.Text, strPass) Then
            If ComboBox4.SelectedIndex = 1 Then
                MsgBox("Имя " & TextBox4.Text & " зарезервировано игроком-человеком")
                Exit Sub
            Else
                If Not tbPass4.Text = strPass And ComboBox4.SelectedIndex = 0 Then
                    MsgBox("Пароль для игрока " & TextBox4.Text & " введен не верно")
                    Exit Sub
                End If
            End If
        Else
            If ComboBox4.SelectedIndex = 0 AndAlso tbPass4.Text = "" Then
                If MsgBox("Для ведения индивидуальной статистики и защиты результатов игры, рекомендуется ввести пароль для игрока " & TextBox4.Text & "." & vbCrLf & "Вы хотите добавить пароль?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then Exit Sub
            End If
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub frmPlayers_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'значения списков по умолчанию
        'Me.ComboBox1.SelectedIndex = 0
        'Me.ComboBox2.SelectedIndex = 1
        'Me.ComboBox3.SelectedIndex = 1
        'Me.ComboBox4.SelectedIndex = 1
        Dim iSavedPlayesCount = frmMain.Chemps.aP.Count
        If iSavedPlayesCount = 0 Then
            If ComboBox1.SelectedIndex = 0 Then
                Dim ta() As String = Split(My.User.Name, "\")
                TextBox1.Text = ta(ta.Length - 1)
            End If
        Else
            For i As Integer = 0 To iSavedPlayesCount - 1
                Dim ta() As String = Split(frmMain.Chemps.aP(i), vbTab)
                If TextBox1.AutoCompleteCustomSource.Contains(ta(1)) Then Continue For
                TextBox1.AutoCompleteCustomSource.Add(ta(1))
                TextBox2.AutoCompleteCustomSource.Add(ta(1))
                TextBox3.AutoCompleteCustomSource.Add(ta(1))
                TextBox4.AutoCompleteCustomSource.Add(ta(1))
            Next
        End If

        CheckPasses()
    End Sub
    Private Sub CheckPasses() Handles ComboBox1.SelectedIndexChanged, ComboBox2.SelectedIndexChanged, ComboBox3.SelectedIndexChanged, ComboBox4.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then
            tbPass1.Enabled = True
        Else
            tbPass1.Enabled = False
        End If
        If ComboBox2.SelectedIndex = 0 Then
            tbPass2.Enabled = True
        Else
            tbPass2.Enabled = False
        End If
        If ComboBox3.SelectedIndex = 0 Then
            tbPass3.Enabled = True
        Else
            tbPass3.Enabled = False
        End If
        If ComboBox4.SelectedIndex = 0 Then
            tbPass4.Enabled = True
        Else
            tbPass4.Enabled = False
        End If
    End Sub
    

    Private Sub NamesValidated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.Validated, TextBox2.Validated, TextBox3.Validated, TextBox4.Validated
        Dim t As TextBox = CType(sender, TextBox)
        If t.Name.Contains("tbPass") Then Exit Sub
        If t.Text = "" Then
            MsgBox("Имя игрока не может быть пустым")
            t.Focus()
            Exit Sub
        End If
        Dim o As Control
        For Each o In Me.TableLayoutPanel2.Controls
            If TypeOf o Is TextBox Then
                With CType(o, TextBox)
                    If o.Name.Contains("tbPass") Then Continue For
                    If Not o.Name = t.Name Then

                        If t.Text = o.Text Then
                            MsgBox("Имена игроков должны отличаться")
                            t.Focus()
                        End If
                    End If
                End With

            End If
        Next
        
    End Sub
    
End Class
