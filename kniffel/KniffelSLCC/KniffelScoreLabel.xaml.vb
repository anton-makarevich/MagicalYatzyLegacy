Imports System.Windows.Media
Imports System.Windows.Media.Imaging
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
        CanHaveBonus = False
        Bonus = 0
        CheckAppearence()
        'ttHelp.Visibility = Windows.Visibility.Collapsed
        RaiseEvent ValueChanged()

    End Sub
    Public Property Bonus As Integer
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
    'Private Sub popUp_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles popUp.MouseLeftButtonDown
    '    popUp.IsOpen = False
    'End Sub
    Private Sub CheckAppearence()
        txtLabel.Foreground = New SolidColorBrush(Colors.Black)
        'txtValue.Foreground = New SolidColorBrush(Colors.Black)
        If ValueIsSet Then
            txtLabel.FontWeight = FontWeights.Bold
            If CanHaveBonus Then
                colorAnim.To = Colors.Yellow
                'spValueBackground.Background = New SolidColorBrush(Colors.Yellow)
            Else
                colorAnim.To = Colors.LightGray
                'spValueBackground.Background = New SolidColorBrush(Colors.LightGray)
            End If
            colorStoryboard.Begin()
            txtLabel.Text = (fvalue).ToString '+ Bonus

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
            If CanHaveBonus Then Bonus = 100


        CheckAppearence()
        RaiseEvent ValueChanged()
        End If
    End Sub
    Public Property Type As KniffelLabelType
        Set(ByVal value As KniffelLabelType)
            ftype = value
            txtValue.Text = TypeToString(value)
            SetupHelpToolTip(value)
        End Set
        Get
            Return ftype
        End Get
    End Property
    Public Shared Function TypeToString(ByVal type As KniffelLabelType) As String
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
            Case KniffelLabelType.kltNumB
                Return "Б"
        End Select
        Return "Ш"
    End Function
    Public Sub SetupHelpToolTip(ByVal type As KniffelLabelType)

        Select Case type
            Case KniffelLabelType.kltNum1
                tbHelpHeader.Text = "Единицы"
                tbHelpBody.Text = "Сумма выпавших костей с номиналом 1"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.9.png", UriKind.Relative))
                
            Case KniffelLabelType.kltNum2
                tbHelpHeader.Text = "Двойки"
                tbHelpBody.Text = "Сумма выпавших костей с номиналом 2"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.9.png", UriKind.Relative))
                
            Case KniffelLabelType.kltNum3
                tbHelpHeader.Text = "Тройки"
                tbHelpBody.Text = "Сумма выпавших костей с номиналом 3"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.15.png", UriKind.Relative))
                
            Case KniffelLabelType.kltNum4
                tbHelpHeader.Text = "Четверки"
                tbHelpBody.Text = "Сумма выпавших костей с номиналом 4"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.21.png", UriKind.Relative))
                '
            Case KniffelLabelType.kltNum5
                tbHelpHeader.Text = "Пятерки"
                tbHelpBody.Text = "Сумма выпавших костей с номиналом 5"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.27.png", UriKind.Relative))
                
            Case KniffelLabelType.kltNum6
                tbHelpHeader.Text = "Шестерки"
                tbHelpBody.Text = "Сумма выпавших костей с номиналом 6"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                
            Case KniffelLabelType.kltNum3R
                tbHelpHeader.Text = "Три одинаковые"
                tbHelpBody.Text = "Сумма всех выпавших костей, если среди них есть 3 одинаковые"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp2.Source = New BitmapImage(New Uri("Images/0/stop.3.png", UriKind.Relative))
                iHelp3.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp4.Source = New BitmapImage(New Uri("Images/0/stop.21.png", UriKind.Relative))
                iHelp5.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
            Case KniffelLabelType.kltNum4R
                tbHelpHeader.Text = "Четыре одинаковые"
                tbHelpBody.Text = "Сумма всех выпавших костей, если среди них есть 4 одинаковые"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.27.png", UriKind.Relative))
                iHelp2.Source = New BitmapImage(New Uri("Images/0/stop.27.png", UriKind.Relative))
                iHelp3.Source = New BitmapImage(New Uri("Images/0/stop.27.png", UriKind.Relative))
                iHelp4.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp5.Source = New BitmapImage(New Uri("Images/0/stop.27.png", UriKind.Relative))
            Case KniffelLabelType.kltNumFH
                tbHelpHeader.Text = "Full House"
                tbHelpBody.Text = "25 очков, если выпало 2 и 3 одинаковые кости"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.15.png", UriKind.Relative))
                iHelp2.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp3.Source = New BitmapImage(New Uri("Images/0/stop.15.png", UriKind.Relative))
                iHelp4.Source = New BitmapImage(New Uri("Images/0/stop.15.png", UriKind.Relative))
                iHelp5.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
            Case KniffelLabelType.kltNumSS
                tbHelpHeader.Text = "Small Street"
                tbHelpBody.Text = "30 очков, если выпало 4 кости подряд"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.15.png", UriKind.Relative))
                iHelp2.Source = New BitmapImage(New Uri("Images/0/stop.21.png", UriKind.Relative))
                iHelp3.Source = New BitmapImage(New Uri("Images/0/stop.27.png", UriKind.Relative))
                iHelp4.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))

            Case KniffelLabelType.kltNumLS
                tbHelpHeader.Text = "Large Street"
                tbHelpBody.Text = "40 очков, если выпало 5 костей подряд"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.9.png", UriKind.Relative))
                iHelp2.Source = New BitmapImage(New Uri("Images/0/stop.15.png", UriKind.Relative))
                iHelp3.Source = New BitmapImage(New Uri("Images/0/stop.21.png", UriKind.Relative))
                iHelp4.Source = New BitmapImage(New Uri("Images/0/stop.27.png", UriKind.Relative))
                iHelp5.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
            Case KniffelLabelType.kltNumK
                tbHelpHeader.Text = "Книффель!"
                tbHelpBody.Text = "50 очков, если выпало 5 одинаковых костей"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp2.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp3.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp4.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp5.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
            Case KniffelLabelType.kltNumC
                tbHelpHeader.Text = "Шанс"
                tbHelpBody.Text = "Сумма всех выпавших костей"
                iHelp.Source = New BitmapImage(New Uri("Images/0/stop.27.png", UriKind.Relative))
                iHelp2.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp3.Source = New BitmapImage(New Uri("Images/0/stop.21.png", UriKind.Relative))
                iHelp4.Source = New BitmapImage(New Uri("Images/0/stop.33.png", UriKind.Relative))
                iHelp5.Source = New BitmapImage(New Uri("Images/0/stop.21.png", UriKind.Relative))
            Case KniffelLabelType.kltNumB
                tbHelpHeader.Text = "Бонус"
                tbHelpBody.Text = "35 очков, если сумма в ячейках с ""1"" по ""6"" 63 и более"
        End Select

    End Sub
    Public Shared Function RuleToString(ByVal rule As KniffelRules) As String
        Select Case rule
            Case KniffelRules.krBaby
                Return "детские"
            Case KniffelRules.krExtended
                Return "расширенные"
            Case KniffelRules.krSimple
                Return "простые"
            Case KniffelRules.krStandart
                Return "стандартные"
        End Select
        Return "стандартные"
    End Function
    Public Shared Function RuleToStringID(ByVal rule As KniffelRules) As String
        Select Case rule
            Case KniffelRules.krBaby
                Return "baby"
            Case KniffelRules.krExtended
                Return "full"
            Case KniffelRules.krSimple
                Return "simple"
            Case KniffelRules.krStandart
                Return "standard"
        End Select
        Return "standard"
    End Function
    Public Shared Function RuleToShortDescriptionString(ByVal rule As KniffelRules) As String
        Select Case rule
            Case KniffelRules.krBaby
                Return "только цифровые комбинации и книффель, 7 ходов"
            Case KniffelRules.krExtended
                Return "все комбинации, бонус в 35 очков за сумму цифровых комбинаций 63 и более, бонус 100 очков за каждый последующий книффель, книффель-джокер, 13 ходов"
            Case KniffelRules.krSimple
                Return "все комбинации, без бонусов, 13 ходов"
            Case KniffelRules.krStandart
                Return "все комбинации, бонус в 35 очков за сумму цифровых комбинаций 63 и более, 13 ходов"
        End Select
        Return "стандартные"
    End Function
    Public Shared Function RuleFromString(ByVal rule As String) As KniffelRules
        Select Case rule
            Case "детские"
                Return KniffelRules.krBaby
            Case "расширенные"
                Return KniffelRules.krExtended
            Case "простые"
                Return KniffelRules.krSimple
            Case "стандартные"
                Return KniffelRules.krStandart

        End Select
        Return KniffelRules.krStandart
    End Function
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
        kltNumB = 14
    End Enum
    Public Enum KniffelLabelView
        klvFull = 0
        klvOnlyScore = 1
    End Enum
    Public Enum KniffelRules
        krStandart = 3
        krExtended = 1
        krSimple = 0
        krBaby = 2
    End Enum
    Private _canhavebonus
    Public Property CanHaveBonus As Boolean
        Set(ByVal value As Boolean)
            _canhavebonus = value
            If value = True Then
                ttBonus.Visibility = Windows.Visibility.Visible
            Else
                ttBonus.Visibility = Windows.Visibility.Collapsed
            End If
        End Set
        Get
            Return _canhavebonus
        End Get
    End Property
    'Private Sub iHelp_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtValue.MouseLeftButtonDown
    '    puHeader.Text = TypeToString(Type)
    '    popUp.IsOpen = True
    'End Sub
    Private isLabel As Boolean
    Public ReadOnly Property LabelClicked As Boolean
        Get
            Return isLabel
        End Get
    End Property
    Private Sub txtLabel_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtLabel.MouseLeftButtonDown
        isLabel = False
    End Sub

    Private Sub txtValue_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtValue.MouseLeftButtonDown
        isLabel = True
    End Sub
End Class
