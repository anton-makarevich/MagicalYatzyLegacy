
Imports System.Windows.Media
Imports System.Windows
Partial Public Class KniffelScoreLabel
    Inherits UserControl
    Private ValueIsSet As Boolean
    Public Event ValueChanged()
    Public Sub New()
        InitializeComponent()
        Renew()
    End Sub
    Public Sub Renew()
        fvalue = 0
        txtLabel.Text = ""
        ValueIsSet = False
        CheckAppearence()
        RaiseEvent ValueChanged()
    End Sub
    Public Enum KniffelLabelType
        kltNum1 = 1
        kltNum2 = 2
        kltNum3 = 3
        kltNum4 = 4
        kltNum5 = 5
        kltNum6 = 6
        kltNum3R = 7
        kltNum4R = 8
        kltNumFH = 9
        kltNumSS = 10
        kltNumLS = 11
        kltNumK = 12
        kltNumC = 13
    End Enum
    Public Enum KniffelLabelView
        klvFull = 0
        klvOnlyScore = 1
    End Enum
    Private ftype As KniffelLabelType
    Private fvalue As Integer
    Private _view As KniffelLabelView
    Public Property View As KniffelLabelView
        Set(ByVal value As KniffelLabelView)
            _view = value
            Select Case value
                Case KniffelLabelView.klvFull
                    txtValue.Visibility = System.Windows.Visibility.Visible
                Case KniffelLabelView.klvOnlyScore
                    txtValue.Visibility = System.Windows.Visibility.Collapsed
            End Select
            CheckAppearence()
        End Set
        Get
            Return _view
        End Get
    End Property
    Public ReadOnly Property Value As Integer
        Get
            Return fvalue

        End Get

    End Property
    Private Sub CheckAppearence()
        txtLabel.Foreground = New SolidColorBrush(Colors.Black)
        'txtValue.Foreground = New SolidColorBrush(Colors.Black)
        If ValueIsSet Then
            txtLabel.FontWeight = FontWeights.Bold
            spValueBackground.Background = New SolidColorBrush(Colors.LightGray)

        Else
            txtLabel.FontWeight = FontWeights.Light
            spValueBackground.Background = New SolidColorBrush(Colors.Transparent)

        End If

    End Sub
    Public ReadOnly Property HaveValue As Boolean
        Get
            Return ValueIsSet 'Not Value = 0
        End Get
    End Property
    Public Sub ApplyValue()
        If Not HaveValue And (Not txtLabel.Text = "") Then
            ValueIsSet = True
            fvalue = CInt(txtLabel.Text)
            txtLabel.Text = fvalue.ToString
            CheckAppearence()
            RaiseEvent ValueChanged()
        End If
    End Sub
    Public Property Type As KniffelLabelType
        Set(ByVal value As KniffelLabelType)
            ftype = value
            txtValue.Text = TypeToString(value)
        End Set
        Get
            Return ftype
        End Get
    End Property
    Private Function TypeToString(ByVal type As KniffelLabelType) As String
        Select Case type
            Case KniffelLabelType.kltNum1
                Return "1"
            Case KniffelLabelType.kltNum2
                Return "2"
            Case KniffelLabelType.kltNum3
                Return "3"
            Case KniffelLabelType.kltNum4
                Return "4"
            Case KniffelLabelType.kltNum5
                Return "5"
            Case KniffelLabelType.kltNum6
                Return "6"
            Case KniffelLabelType.kltNum3R
                Return "Т"
            Case KniffelLabelType.kltNum4R
                Return "Ч"
            Case KniffelLabelType.kltNumFH
                Return "FH"
            Case KniffelLabelType.kltNumSS
                Return "SS"
            Case KniffelLabelType.kltNumLS
                Return "LS"
            Case KniffelLabelType.kltNumK
                Return "К!"
            Case KniffelLabelType.kltNumC
                Return "Ш"
        End Select
        Return "Ш"
    End Function
End Class
