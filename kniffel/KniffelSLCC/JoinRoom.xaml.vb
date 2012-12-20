Imports System.IO.IsolatedStorage
Imports System.IO
Partial Public Class JoinRoom
    Inherits ChildWindow
    Private _rules As KniffelScoreLabel.KniffelRules
    Public Property Rules() As KniffelScoreLabel.KniffelRules
        Get
            Return _rules
        End Get
        Set(ByVal value As KniffelScoreLabel.KniffelRules)
            _rules = value
            cbRules.SelectedItem = KniffelScoreLabel.RuleToString(_rules)
        End Set
    End Property
    Public Property DieStyle As Kniffel.dpStyle

    Public Sub New()
        InitializeComponent()
        cbRules.Items.Clear()
        cbRules.Items.Add(KniffelScoreLabel.RuleToString(KniffelScoreLabel.KniffelRules.krBaby))
        cbRules.Items.Add(KniffelScoreLabel.RuleToString(KniffelScoreLabel.KniffelRules.krSimple))
        cbRules.Items.Add(KniffelScoreLabel.RuleToString(KniffelScoreLabel.KniffelRules.krStandart))
        cbRules.Items.Add(KniffelScoreLabel.RuleToString(KniffelScoreLabel.KniffelRules.krExtended))


    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        DialogResult = False
    End Sub

    Private Sub btnJoin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Rules = KniffelScoreLabel.RuleFromString(cbRules.SelectedItem)
        If rb0.IsChecked Then DieStyle = dpStyle.dpsClassic
        If rb1.IsChecked Then DieStyle = dpStyle.dpsBrutalRed
        If rb2.IsChecked Then DieStyle = dpStyle.dpsBlue
        DialogResult = True
    End Sub

    Private Sub KeyDownHandler(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Key = Key.Enter Then
            btnJoin_Click(cbRules, Nothing)
        End If
    End Sub

    Private Sub cbRules_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cbRules.SelectionChanged
        Rules = KniffelScoreLabel.RuleFromString(cbRules.SelectedItem)
        tbRulesDesc.Text = KniffelScoreLabel.RuleToShortDescriptionString(Rules)
    End Sub
End Class
