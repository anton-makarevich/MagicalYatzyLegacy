Imports Kniffel.KniffelScoreLabel.KniffelLabelType
Imports System.Windows.Input
Imports System.Windows.Media
Imports Kniffel
Partial Public Class KniffelScorePanel
    Inherits UserControl
    Public Event ScoreApplyed(ByVal type As KniffelScoreLabel.KniffelLabelType, ByVal value As Integer, ByVal havebonus As Boolean)
    Public Event TotalCalculated(ByVal name As String, ByVal value As String)
    Public Sub New(ByVal rules As KniffelScoreLabel.KniffelRules)
        InitializeComponent()
        _rules = rules
        Renew()
    End Sub
    Private Sub NameTextChanged() Handles txtPlayerName.TextChanged
        If Not txtPlayerName.textBlock.text = txtPlayerName.Text Then
            tbName.Text = txtPlayerName.Text
            ttName.Visibility = Windows.Visibility.Visible
        Else
            ttName.Visibility = Windows.Visibility.Collapsed
        End If

    End Sub
    Public Sub Renew()
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            Select Case _rules
                Case KniffelScoreLabel.KniffelRules.krBaby
                    kl.Width = 55
                    Select Case kl.Type
                        Case kltNum1, kltNum2, kltNum3, kltNum4, kltNum5, kltNum6, kltNumK
                            kl.Visibility = Visibility.Visible
                        Case Else
                            kl.Visibility = Visibility.Collapsed
                    End Select
                Case KniffelScoreLabel.KniffelRules.krSimple
                    kl.Width = 30
                    kl.Visibility = Visibility.Visible
                    If kl.Type = kltNumB Then kl.Visibility = Windows.Visibility.Collapsed
                Case KniffelScoreLabel.KniffelRules.krExtended, KniffelScoreLabel.KniffelRules.krStandart
                    kl.Width = 29
                    kl.Visibility = Visibility.Visible
            End Select

            kl.Renew()
            AddHandler kl.ValueChanged, AddressOf CalculateTotal
            AddHandler kl.MouseEnter, AddressOf klMouseEnter
            AddHandler kl.MouseLeave, AddressOf klMouseLeave
            AddHandler kl.MouseLeftButtonDown, AddressOf klMouseClick
        Next
        If Rules = KniffelScoreLabel.KniffelRules.krExtended OrElse Rules = KniffelScoreLabel.KniffelRules.krStandart Then
            txtTotal.Width = 44
        Else
            txtTotal.Width = 47
        End If
    End Sub

    Private _orientation As System.Windows.Controls.Orientation

    Private _View As KniffelScoreLabel.KniffelLabelView
    Public Property View As KniffelScoreLabel.KniffelLabelView
        Set(ByVal value As KniffelScoreLabel.KniffelLabelView)
            _View = value
            For Each kl As KniffelScoreLabel In spScoreLabels.Children
                kl.View = value
            Next
        End Set
        Get
            Return _View
        End Get
    End Property
    Public Sub ShowPossibleValue(ByRef dp As Kniffel.DicePanelSL, ByVal roll As Integer)
        If roll = 1 Then Exit Sub
        Dim bHaveKniffel As Boolean = False
        Dim bKniffelClosed As Boolean = False
        If Rules = KniffelScoreLabel.KniffelRules.krExtended Then
            For Each kl As KniffelScoreLabel In spScoreLabels.Children
                If Not kl.Type = kltNumK Then Continue For
                If kl.HaveValue Then
                    If kl.Value = 50 Then bHaveKniffel = True
                    If bHaveKniffel OrElse kl.Value = 0 Then bKniffelClosed = True
                End If
            Next
        End If
        Dim bMoreKniffel As Boolean
        Dim bIsKNiffelJoker As Boolean
        If dp.YhatzeeeFiveOfAKindScore = 50 Then bMoreKniffel = True
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            If kl.Type = kltNumB Then Continue For



            Select Case kl.Type
                Case kltNum1, kltNum2, kltNum3, kltNum4, kltNum5, kltNum6
                    If Not kl.HaveValue Then
                        kl.txtLabel.Text = dp.YhatzeeNumberScore(kl.Type).ToString
                    Else
                        If bMoreKniffel AndAlso bKniffelClosed AndAlso Rules = KniffelScoreLabel.KniffelRules.krExtended Then
                            If Not dp.YhatzeeNumberScore(kl.Type) = 0 Then bIsKNiffelJoker = True
                        End If
                    End If
                Case kltNum3R, kltNum4R
                    If Not kl.HaveValue Then kl.txtLabel.Text = dp.YhatzeeeOfAKindScore(kl.Type - 4).ToString

                Case kltNumFH
                    If Not kl.HaveValue Then
                        kl.txtLabel.Text = dp.YhatzeeeFullHouseScore.ToString
                        If bIsKNiffelJoker Then kl.txtLabel.Text = "25"
                    End If

                Case kltNumSS
                    If Not kl.HaveValue Then
                        kl.txtLabel.Text = dp.YhatzeeeSmallStraightScore.ToString
                        If bIsKNiffelJoker Then kl.txtLabel.Text = "30"
                    End If

                Case kltNumLS
                    If Not kl.HaveValue Then
                        kl.txtLabel.Text = dp.YhatzeeeLargeStraightScore.ToString
                        If bIsKNiffelJoker Then kl.txtLabel.Text = "40"
                    End If
                Case kltNumK
                    If Not kl.HaveValue Then kl.txtLabel.Text = dp.YhatzeeeFiveOfAKindScore.ToString
                Case kltNumC
                    If Not kl.HaveValue Then kl.txtLabel.Text = dp.YhatzeeeChanceScore.ToString

            End Select
            If Not kl.HaveValue AndAlso Rules = KniffelScoreLabel.KniffelRules.krExtended Then
                kl.CanHaveBonus = False

                If bHaveKniffel And bMoreKniffel Then
                    kl.CanHaveBonus = True

                End If

                'End If
            End If


        Next
    End Sub
    Private Sub klMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)
        Dim kl As KniffelScoreLabel = CType(sender, KniffelScoreLabel)
        If kl.Type = kltNumB Then Exit Sub
        If (Not kl.HaveValue) AndAlso (Not kl.txtLabel.Text = "") Then

            Cursor = Cursors.Hand
            kl.spValueBackground.Background = New SolidColorBrush(Colors.Orange)
        End If
    End Sub
    Private Sub klMouseClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)


        Dim kl As KniffelScoreLabel = CType(sender, KniffelScoreLabel)
        If kl.LabelClicked Then Exit Sub
        If kl.Type = kltNumB Then Exit Sub
        If (Not kl.txtLabel.Text = "") AndAlso (Not kl.HaveValue) Then
            kl.ApplyValue()
            
            Cursor = Cursors.Arrow

            HidePossibleValue()

            RaiseEvent ScoreApplyed(kl.Type, kl.Value, kl.CanHaveBonus)

        End If
    End Sub
    Private Sub klMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)
        Dim kl As KniffelScoreLabel = CType(sender, KniffelScoreLabel)
        If kl.Type = kltNumB Then Exit Sub
        If (Not kl.HaveValue) Then
            Cursor = Cursors.Arrow
            kl.spValueBackground.Background = New SolidColorBrush(Colors.Transparent)
        End If
    End Sub


    Private Sub CalculateTotal()
        Dim r As Integer = 0
        Dim i As Integer = 0
        Dim v As Integer = 0
        If Rules = KniffelScoreLabel.KniffelRules.krExtended OrElse Rules = KniffelScoreLabel.KniffelRules.krStandart Then
            For Each kl As KniffelScoreLabel In spScoreLabels.Children
                If kl.Type < 7 Then
                    i += kl.Value
                    If kl.HaveValue Then v += 1
                End If

            Next
            If i > 62 Then
                ApplyScore(14, 35, False)
            Else
                If v = 6 Then ApplyScore(14, 0, False)
            End If
        End If
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            r += kl.Value
            r += kl.Bonus
        Next
        Total = r.ToString
        RaiseEvent TotalCalculated(txtPlayerName.Text, r.ToString)
    End Sub
    Public Sub HidePossibleValue()
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            If Not kl.HaveValue Then
                kl.txtLabel.Text = ""
                kl.CanHaveBonus = False
            End If
        Next
    End Sub
    Public Property PlayerName As String
        Set(ByVal value As String)
            txtPlayerName.Text = value
        End Set
        Get
            Return txtPlayerName.Text
        End Get
    End Property
    Public Property Total As String
        Set(ByVal value As String)
            txtTotal.Text = value
        End Set
        Get
            Return txtTotal.Text
        End Get
    End Property
    Public Sub ApplyScore(ByVal ScoreType As Integer, ByVal ScoreValue As Integer, ByVal havebonus As Boolean)
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            If CInt(kl.Type) = ScoreType Then
                kl.txtLabel.Text = ScoreValue.ToString
                kl.CanHaveBonus = havebonus
                kl.ApplyValue()
            End If
        Next
    End Sub
    Private _rules As KniffelScoreLabel.KniffelRules
    Public ReadOnly Property Rules
        Get
            Return _rules
        End Get
    End Property
End Class