Imports KniffelNet.KniffelScoreLabel.KniffelLabelType
Imports System.Windows.Input
Imports System.Windows.Media

Partial Public Class KniffelScorePanel
    Inherits UserControl
    Public Event ScoreApplyed(ByVal type As KniffelScoreLabel.KniffelLabelType, ByVal value As Integer)
    Public Event TotalCalculated(ByVal name As String, ByVal value As String)
    Public Sub New()
        InitializeComponent()
        Renew()
    End Sub
    Public Sub Renew()
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            kl.Renew()
            AddHandler kl.ValueChanged, AddressOf CalculateTotal
            AddHandler kl.MouseEnter, AddressOf klMouseEnter
            AddHandler kl.MouseLeave, AddressOf klMouseLeave
            AddHandler kl.MouseLeftButtonDown, AddressOf klMouseClick
        Next
    End Sub

    Private _orientation As System.Windows.Controls.Orientation
    Public Property Orientation As System.Windows.Controls.Orientation
        Get
            Return _orientation
        End Get
        Set(ByVal value As System.Windows.Controls.Orientation)
            _orientation = value
            LayoutRoot.Orientation = value
            spScoreLabels.Orientation = value
            For Each kl As KniffelScoreLabel In spScoreLabels.Children
                kl.LayoutRoot.Orientation = value
            Next
        End Set
    End Property
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
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            If Not kl.HaveValue Then

                Select Case kl.Type
                    Case kltNum1, kltNum2, kltNum3, kltNum4, kltNum5, kltNum6
                        kl.txtLabel.Text = dp.YhatzeeNumberScore(kl.Type).ToString
                    Case kltNum3R, kltNum4R
                        kl.txtLabel.Text = dp.YhatzeeeOfAKindScore(kl.Type - 4).ToString
                    Case kltNumFH
                        kl.txtLabel.Text = dp.YhatzeeeFullHouseScore.ToString
                    Case kltNumSS
                        kl.txtLabel.Text = dp.YhatzeeeSmallStraightScore.ToString
                    Case kltNumLS
                        kl.txtLabel.Text = dp.YhatzeeeLargeStraightScore.ToString
                    Case kltNumK
                        kl.txtLabel.Text = dp.YhatzeeeFiveOfAKindScore.ToString
                    Case kltNumC
                        kl.txtLabel.Text = dp.YhatzeeeChanceScore.ToString
                End Select

            End If


        Next
    End Sub
    Private Sub klMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)
        Dim kl As KniffelScoreLabel = CType(sender, KniffelScoreLabel)
        'ShowPossibleValue(kl)
        If (Not kl.HaveValue) AndAlso (Not kl.txtLabel.Text = "") Then
            Cursor = Cursors.Hand
            kl.spValueBackground.Background = New SolidColorBrush(Colors.Orange)
        End If
    End Sub
    Private Sub klMouseClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)
        Dim kl As KniffelScoreLabel = CType(sender, KniffelScoreLabel)
        If (Not kl.txtLabel.Text = "") AndAlso (Not kl.HaveValue) Then
            kl.ApplyValue()

            Cursor = Cursors.Arrow

            HidePossibleValue()
            RaiseEvent ScoreApplyed(kl.Type, kl.Value)
            
        End If
    End Sub
    Private Sub klMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)
        Dim kl As KniffelScoreLabel = CType(sender, KniffelScoreLabel)
        If (Not kl.HaveValue) Then
            Cursor = Cursors.Arrow
            kl.spValueBackground.Background = New SolidColorBrush(Colors.Transparent)
        End If
    End Sub

   
    Private Sub CalculateTotal()
        Dim r As Integer = 0
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            r += kl.Value
        Next
        Total.Text = r.ToString
        RaiseEvent TotalCalculated(PlayerName.Text, r.ToString)
    End Sub
    Public Sub HidePossibleValue()
        For Each kl As KniffelScoreLabel In spScoreLabels.Children
            If Not kl.HaveValue Then
                kl.txtLabel.Text = ""
            End If
        Next
    End Sub

End Class
