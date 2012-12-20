
Public Class lbiKniffelPlayer
    Inherits Sanet.lbiGamePlayer
    Public Property IsPlaying As Boolean
    Public Property Rules As KniffelScoreLabel.KniffelRules
    Public Property Move As Integer
End Class

Public Class KniffelPlayerSL
    Public Property Name As String
    Public Property cp As String
    Public Property cn As String
    Public Property cr As String
    Public Property Password As String
    Public Property ID As String
    Public Property GameID As Integer
    Public Property IsMoving As Boolean
    Public Property Rules As KniffelScoreLabel.KniffelRules = KniffelScoreLabel.KniffelRules.krStandart
    Public Property GamePlatform As KniffelGamePlatform
    Public Property PicUrl As String
    Public Property IsWallPublishAllowed As Boolean = False
    Public Property LastResult As String
    Public ReadOnly Property RuleName As String
        Get
            Return KniffelScoreLabel.RuleToString(Rules)
        End Get
    End Property
End Class

Public Class KniffelGameRoom
    Inherits Expander
    Public bEnter As Button
    Public spRoot As StackPanel
    Public wpPlayers As WrapPanel
    Public Event EnterClicked(ByVal sender As Object, ByVal e As RoutedEventArgs)
    Private Sub RaiseEnterClicked(ByVal sender As Object, ByVal e As RoutedEventArgs)
        RaiseEvent EnterClicked(Me, e)
    End Sub
    Public Sub New()
        Foreground = New SolidColorBrush(Colors.White)
        bEnter = New Button
        bEnter.Width = 150
        spRoot = New StackPanel
        spRoot.Orientation = Orientation.Horizontal
        AddHandler bEnter.Click, AddressOf RaiseEnterClicked
        Content = spRoot
        spRoot.Children.Add(bEnter)
        wpPlayers = New WrapPanel
        wpPlayers.MaxWidth = 450
        spRoot.Children.Add(wpPlayers)
    End Sub
    Private _id As String
    Public Event RoomContentUpdated()
    Public Property ID As String
        Set(ByVal value As String)
            _id = value
            SetContent()
            CheckIsEnabled()
        End Set
        Get
            Return _id
        End Get
    End Property
    Private _move As Integer
    Public Property Move As Integer
        Get
            Return _move
        End Get
        Set(ByVal value As Integer)
            _move = value
            If _status = KniffelGameRoomStatus.kgrPlaying Then
                If Not _move = 0 Then strStatus = "идет игра, ход " & _move & "/" & MaxMove
                SetContent()
            End If
        End Set
    End Property
    Private _maxmove As Integer
    Public ReadOnly Property MaxMove As Integer
        Get
            Return _maxmove
        End Get
    End Property
    Private _status As KniffelGameRoomStatus
    Public Property Status As KniffelGameRoomStatus
        Set(ByVal value As KniffelGameRoomStatus)
            _status = value
            If value = KniffelGameRoomStatus.kgrPlaying Then
                strStatus = "идет игра, ход " & _Move & "/" & MaxMove
            Else
                strStatus = "ожидание"
            End If
            SetContent()
            CheckIsEnabled()
        End Set
        Get
            Return _status
        End Get
    End Property
    Private _playersnumber As Integer
    Public Property PlayersNumber As Integer
        Set(ByVal value As Integer)
            _playersnumber = value
            SetContent()
            CheckIsEnabled()
        End Set
        Get
            Return _playersnumber
        End Get
    End Property
    Private strStatus As String
    Private Sub SetContent()
        Header = "Cтол № " & ID & ", игроков: " & PlayersNumber & "/6, правила: " & KniffelScoreLabel.RuleToString(Rules) & ", статус: " & strStatus
        RaiseEvent RoomContentUpdated()
    End Sub
    Private Sub CheckIsEnabled()
        If PlayersNumber = 6 OrElse Status = KniffelGameRoomStatus.kgrPlaying Then
            If PlayersNumber = 6 Then
                bEnter.Content = "Нет мест"
            Else
                bEnter.Content = "Идет игра"
            End If
            bEnter.IsEnabled = False
        Else
            bEnter.Content = "Сесть за стол"
            bEnter.IsEnabled = True
        End If

    End Sub
    Private _rules As KniffelScoreLabel.KniffelRules
    Public Property Rules As KniffelScoreLabel.KniffelRules
        Set(ByVal value As KniffelScoreLabel.KniffelRules)
            _rules = value
            Select Case value
                Case KniffelScoreLabel.KniffelRules.krExtended, KniffelScoreLabel.KniffelRules.krStandart, KniffelScoreLabel.KniffelRules.krSimple
                    _maxmove = 13
                Case KniffelScoreLabel.KniffelRules.krBaby
                    _maxmove = 7
            End Select
            SetContent()
        End Set
        Get
            Return _rules
        End Get
    End Property
End Class
Public Enum KniffelGamePlatform
    kgpSanet = 0
    kgpVkontakte = 1
    kgpMoiMir = 2
End Enum
Public Enum KniffelGameRoomStatus
    kgrWaiting = 0
    kgrPlaying = 1

End Enum

''' <summary>
''' A simple text control that truncates the text to ellipses when there
''' is insufficient room to display all of the text.
''' </summary>
Public Class DynamicTextBlock
    Inherits ContentControl
#Region "Text (DependencyProperty)"

    ''' <summary>
    ''' Gets or sets the Text DependencyProperty. This is the text that will be displayed.
    ''' </summary>
    ''' 
    Public Property Text() As String
        Get
            Return DirectCast(GetValue(TextProperty), String)
        End Get
        Set(ByVal value As String)

            SetValue(TextProperty, value)

        End Set
    End Property
    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(DynamicTextBlock), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf OnTextChanged)))

    Private Shared Sub OnTextChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DynamicTextBlock).OnTextChanged(e)

    End Sub
    Public Event TextChanged()
    Protected Overridable Sub OnTextChanged(ByVal e As DependencyPropertyChangedEventArgs)
        Me.InvalidateMeasure()
    End Sub

#End Region

#Region "TextWrapping (DependencyProperty)"

    ''' <summary>
    ''' Gets or sets the TextWrapping property. This corresponds to TextBlock.TextWrapping.
    ''' </summary>
    Public Property TextWrapping() As TextWrapping
        Get
            Return DirectCast(GetValue(TextWrappingProperty), TextWrapping)
        End Get
        Set(ByVal value As TextWrapping)
            SetValue(TextWrappingProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TextWrappingProperty As DependencyProperty = DependencyProperty.Register("TextWrapping", GetType(TextWrapping), GetType(DynamicTextBlock), New PropertyMetadata(TextWrapping.NoWrap, New PropertyChangedCallback(AddressOf OnTextWrappingChanged)))

    Private Shared Sub OnTextWrappingChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DynamicTextBlock).OnTextWrappingChanged(e)
    End Sub

    Protected Overridable Sub OnTextWrappingChanged(ByVal e As DependencyPropertyChangedEventArgs)
        Me.textBlock.TextWrapping = DirectCast(e.NewValue, TextWrapping)
        Me.InvalidateMeasure()
    End Sub

#End Region

#Region "LineHeight (DependencyProperty)"

    ''' <summary>
    ''' Gets or sets the LineHeight property. This property corresponds to TextBlock.LineHeight;
    ''' </summary>
    Public Property LineHeight() As Double
        Get
            Return CDbl(GetValue(LineHeightProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(LineHeightProperty, value)
        End Set
    End Property

    Public Shared ReadOnly LineHeightProperty As DependencyProperty = DependencyProperty.Register("LineHeight", GetType(Double), GetType(DynamicTextBlock), New PropertyMetadata(0.0, New PropertyChangedCallback(AddressOf OnLineHeightChanged)))

    Private Shared Sub OnLineHeightChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DynamicTextBlock).OnLineHeightChanged(e)
    End Sub

    Protected Overridable Sub OnLineHeightChanged(ByVal e As DependencyPropertyChangedEventArgs)
        textBlock.LineHeight = LineHeight
        Me.InvalidateMeasure()
    End Sub

#End Region

#Region "LineStackingStrategy (DependencyProperty)"

    ''' <summary>
    ''' Gets or sets the LineStackingStrategy DependencyProperty. This corresponds to TextBlock.LineStackingStrategy.
    ''' </summary>
    Public Property LineStackingStrategy() As LineStackingStrategy
        Get
            Return DirectCast(GetValue(LineStackingStrategyProperty), LineStackingStrategy)
        End Get
        Set(ByVal value As LineStackingStrategy)
            SetValue(LineStackingStrategyProperty, value)
        End Set
    End Property
    Public Shared ReadOnly LineStackingStrategyProperty As DependencyProperty = DependencyProperty.Register("LineStackingStrategy", GetType(LineStackingStrategy), GetType(DynamicTextBlock), New PropertyMetadata(LineStackingStrategy.BlockLineHeight, New PropertyChangedCallback(AddressOf OnLineStackingStrategyChanged)))

    Private Shared Sub OnLineStackingStrategyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DynamicTextBlock).OnLineStackingStrategyChanged(e)
    End Sub

    Protected Overridable Sub OnLineStackingStrategyChanged(ByVal e As DependencyPropertyChangedEventArgs)
        Me.textBlock.LineStackingStrategy = DirectCast(e.NewValue, LineStackingStrategy)
        Me.InvalidateMeasure()
    End Sub

#End Region

    ''' <summary>
    ''' A TextBlock that gets set as the control's content and is ultiately the control 
    ''' that displays our text
    ''' </summary>
    Public textBlock As TextBlock

    ''' <summary>
    ''' Initializes a new instance of the DynamicTextBlock class
    ''' </summary>
    Public Sub New()
        ' create our textBlock and initialize
        Me.textBlock = New TextBlock()
        Me.Content = Me.textBlock
    End Sub

    ''' <summary>
    ''' Handles the measure part of the measure and arrange layout process. During this process
    ''' we measure the textBlock that we've created as content with increasingly smaller amounts
    ''' of text until we find text that fits.
    ''' </summary>
    ''' <param name="availableSize">the available size</param>
    ''' <returns>the base implementation of Measure</returns>

    Protected Overrides Function MeasureOverride(ByVal availableSize As Size) As Size
        ' just to make the code easier to read
        Dim wrapping As Boolean = Me.TextWrapping = TextWrapping.Wrap

        Dim unboundSize As Size = If(wrapping, New Size(availableSize.Width, Double.PositiveInfinity), New Size(Double.PositiveInfinity, availableSize.Height))
        Dim reducedText As String = Me.Text

        ' set the text and measure it to see if it fits without alteration
        Me.textBlock.Text = reducedText
        Dim textSize As Size = MyBase.MeasureOverride(unboundSize)

        While If(wrapping, textSize.Height > availableSize.Height, textSize.Width > availableSize.Width)
            Dim prevLength As Integer = reducedText.Length
            reducedText = Me.ReduceText(reducedText)

            If reducedText.Length = prevLength Then
                Exit While
            End If

            Me.textBlock.Text = reducedText & "..."

            textSize = MyBase.MeasureOverride(unboundSize)
        End While
        RaiseEvent TextChanged()
        Return MyBase.MeasureOverride(availableSize)
    End Function

    ''' <summary>
    ''' Reduces the length of the text. Derived classes can override this to use different techniques 
    ''' for reducing the text length.
    ''' </summary>
    ''' <param name="text">the original text</param>
    ''' <returns>the reduced length text</returns>
    Protected Overridable Function ReduceText(ByVal text As String) As String
        Return text.Substring(0, text.Length - 1)
    End Function
End Class
