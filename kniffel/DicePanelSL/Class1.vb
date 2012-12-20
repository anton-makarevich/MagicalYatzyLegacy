Imports System.ComponentModel
Imports System.Math
Imports System.Windows.Media.Imaging
Imports System.Windows.Resources
Imports System.Windows.Threading

Public Enum dpStyle
    dpsClassic = 0
    dpsBrutalRed = 1
    dpsBlue = 2
End Enum
Public Class DicePanelSL
    Inherits Canvas


    Public FRand As New Random
    Public Property PlaySound As Boolean = False

    Public Event DieBounced()
    Public Event DieFrozen(ByVal fixed As Boolean, ByVal Value As Integer)
    Private FStyle As dpStyle = dpStyle.dpsClassic
    Public aDice As New List(Of DieSL)
    Private sbLoop As Storyboard

    Public Event EndRoll()
    Public Event BeginRoll()

    Public Property TreeDScale As Boolean = True
    Public Property TreeDScaleCoef As Double = 0.6
    'cashed images
    Public DieFrameImages As New Dictionary(Of String, BitmapSource)
    Const strImageRoot As String = "/Images/"
    Public strStyle As String
    Public Event StartLoading()
    Public Event StopLoading()
    Public Sub New()
        'InitializeComponent()
        Style = dpStyle.dpsBlue
        

    End Sub

    Private Sub LoadFrameImages(ByVal rot As String)
        For i As Integer = 0 To 35
            Dim sPath As String = strImageRoot & strStyle & rot & i.ToString & ".png"
            If Not DieFrameImages.ContainsKey(sPath) Then
                Dim ur As New Uri(sPath, UriKind.Relative)
                Dim img As BitmapSource = New BitmapImage(ur)
                DieFrameImages.Add(sPath, img)
            End If
        Next
    End Sub

    Private dpBackcolor As Color = Colors.LightGray
    Public Overloads Property Style() As dpStyle
        Get
            Return FStyle
        End Get
        Set(ByVal value As dpStyle)
            'If FStyle = value And DieFrameImages.Values.Count > 0 Then Exit Property
            FStyle = value

            Select Case FStyle
                Case dpStyle.dpsClassic
                    DieAngle = 0
                    'dpBackcolor = Colors.Green
                    strStyle = "0/"
                Case dpStyle.dpsBrutalRed
                    DieAngle = 1
                    'dpBackcolor = Colors.Black
                    strStyle = "1/"
                Case dpStyle.dpsBlue
                    DieAngle = 1
                    'dpBackcolor = Colors.Transparent 'Color.FromArgb(100, )
                    strStyle = "2/"
            End Select
            'loading image frames
            LoadFrameImages("xrot.")
            LoadFrameImages("yrot.")
            LoadFrameImages("stop.")

            'precashing image frames
            For Each b As BitmapSource In DieFrameImages.Values
                Dim img As New Image
                img.Source = b
                MyBase.Children.Add(img)
            Next
            MyBase.Children.Clear()

            dpBackcolor = Colors.Transparent
            Me.Background = New SolidColorBrush(dpBackcolor) 'bckim '

        End Set
    End Property
    Private FDieAngle As Integer = 0
    Public Property DieAngle() As Integer
        Get
            Return FDieAngle
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            If value > 5 Then value = 5
            FDieAngle = value
        End Set
    End Property
    Private FMaxRollLoop As Integer = 75
    Public Property MaxRollLoop() As Integer
        Get
            Return FMaxRollLoop
        End Get
        Set(ByVal value As Integer)
            If value < 50 Then value = 50
            If value > 150 Then value = 150
            FMaxRollLoop = value
        End Set
    End Property


    Private FNumDice As Integer = 2

    <Description("Number of Dice in the Panel"), _
        Category("Dice"), _
        DefaultValue(2)> _
    Property NumDice() As Integer
        Get
            Return FNumDice
        End Get
        Set(ByVal Value As Integer)
            FNumDice = Value

            GenerateDice()

        End Set
    End Property

    Private FDebugDrawMode As Boolean = False

    <Description("Draws a Box Around the Die for Collision Debugging"), _
    Category("Dice"), DefaultValue(False)> _
    Property DebugDrawMode() As Boolean
        Get
            Return FDebugDrawMode
        End Get
        Set(ByVal Value As Boolean)
            FDebugDrawMode = Value
        End Set
    End Property

    'new
    Private FClickToFreeze As Boolean

    'new
    <Description("Allows user to click dice to lock their movement"), _
    Category("Dice"), DefaultValue(False)> _
    Property ClickToFreeze() As Boolean
        Get
            Return FClickToFreeze
        End Get
        Set(ByVal Value As Boolean)
            FClickToFreeze = Value
        End Set
    End Property

    Private Sub GenerateDice()

        Dim d As DieSL
        Dim dOld As DieSL
        Dim bDone As Boolean
        Dim iTry As Integer

        aDice = New List(Of DieSL)
        Me.Children.Clear()
        'MC.Children.Clear()

        Do While aDice.Count < NumDice
            d = New DieSL(Me)
            iTry = 0

            Do
                iTry += 1
                bDone = True
                d.InitializeLocation()
                For Each dOld In aDice
                    If d.Overlapping(dOld) Then
                        bDone = False
                    End If
                Next
            Loop Until bDone Or iTry > 1000

            aDice.Add(d)
            d.DrawDie()
            Me.Children.Add(d.PNG)
            'Me.Children.Add(d.mSound)

        Loop

    End Sub

    <Description("Summed Result of All the Dice"), Category("Dice")> _
    ReadOnly Property Result() As Integer
        Get
            Dim d As DieSL
            Dim i As Integer = 0

            For Each d In aDice
                i += d.Result
            Next
            Return i
        End Get
    End Property

    'new, changed visibility
    Public ReadOnly Property AllDiceStopped() As Boolean
        Get
            Dim d As DieSL
            Dim r As Boolean

            r = True
            For Each d In aDice
                If d.IsRolling Then
                    r = False
                End If
            Next

            Return r
        End Get
    End Property

    Public Sub RollDice(Optional ByRef aResults As List(Of Integer) = Nothing)

        Dim d As DieSL


        'don't roll if all frozen
        If AllDiceFrozen() Then Exit Sub
        RaiseEvent BeginRoll()
        Dim j As Integer = 0
        For i As Integer = 0 To aDice.Count - 1

            Dim iResult As Integer = 0
            d = aDice(i)
            If d.Frozen Then Continue For
            If Not aResults Is Nothing Then
                If aResults.Count = aDice.Count Then
                    iResult = aResults(j)
                    j += 1
                End If

            End If

            d.InitializeRoll(iResult)

        Next
        sbLoop = New Storyboard
        sbLoop.Duration = TimeSpan.FromMilliseconds(RollDelay)


        AddHandler sbLoop.Completed, AddressOf loop_Completed

        ' Start playing the Storyboard loop

        BeginLoop()

    End Sub
    Private Sub BeginLoop()

        For Each d In aDice
            If PlaySound Then
                If d.iSound >= 8 Then
                    d.PlaySound()
                    d.iSound = 1
                Else
                    d.iSound += 1
                End If
            End If
        Next
        sbLoop.Begin()
    End Sub
    Private Sub loop_Completed(ByVal sender As Object, ByVal e As EventArgs)
        For Each d In aDice
            d.UpdateDiePosition()
            d.DrawDie()
            If d.IsPlaying Then d.StopSound()
        Next
        System.Threading.Thread.Sleep(RollDelay)
        HandleCollisions()
        'Me.Invalidate()
        'System.Windows.Forms.Application.DoEvents()
        If Not AllDiceStopped Then
            BeginLoop()
        Else
            RaiseEvent EndRoll()
            sbLoop = Nothing
        End If


    End Sub
    Private _RollDelay As Integer
    Public Property RollDelay As Integer
        Get
            Return _RollDelay
        End Get
        Set(ByVal value As Integer)
            _RollDelay = value
        End Set
    End Property
    Private Sub HandleCollisions()

        Dim di As DieSL
        Dim dj As DieSL
        Dim i As Integer
        Dim j As Integer

        If NumDice = 1 Then Exit Sub

        'can't use foreach loops here, want to start j loop index AFTER first loop
        For i = 0 To aDice.Count - 2
            For j = i + 1 To aDice.Count - 1
                di = aDice.Item(i)
                dj = aDice.Item(j)
                di.HandleCollision(dj)
            Next
        Next

    End Sub

    Private Sub canvasBackground_SizeChanged(ByVal sender As Object, ByVal e As SizeChangedEventArgs)
        Dim rect As New RectangleGeometry()
        rect.Rect = New Rect(0, 0, Me.ActualWidth, Me.ActualHeight)
        Me.Clip = rect


    End Sub

    'new
    Public Sub DieClicked(ByVal sender As Object, ByVal e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown


        If ClickToFreeze Then
            Dim pointClicked As Point = e.GetPosition(Me)

            For Each d As DieSL In aDice
                If d.ClickedOn(pointClicked.X, pointClicked.Y) Then
                    d.Frozen = Not d.Frozen
                    d.DrawDie()
                    RaiseEvent DieFrozen(d.Frozen, d.Result)
                    Exit For
                End If
            Next






        End If
    End Sub

    Private Function DiceGenerated() As Boolean
        Return Not (aDice Is Nothing)
    End Function

    Public Sub OnDieBounced()
        RaiseEvent DieBounced()
    End Sub



    'new
    'don't roll if all frozen
    Public Function AllDiceFrozen() As Boolean

        Dim d As DieSL


        For Each d In aDice
            If Not d.Frozen Then
                Return False
            End If
        Next
        Return True


    End Function

    'new
    Public Sub ClearFreeze()
        Dim d As DieSL
        'If ClickToFreeze Then
        For Each d In aDice
            If d.Frozen Then
                d.Frozen = False
                d.DrawDie()
            End If
        Next
        'End If

    End Sub

    'the score for the numeric 1-6 categories in Y
    Public Function YhatzeeNumberScore(ByVal iNum As Integer) As Integer

        Dim d As DieSL
        Dim iTot As Integer = 0

        For Each d In aDice
            If d.Result = iNum Then
                iTot += iNum
            End If
        Next
        Return iTot

    End Function

    Public Function YhatzeeeOfAKindScore(ByVal NumofAKind As Integer) As Integer

        Dim d As DieSL
        Dim iOccur(6) As Integer
        Dim i As Integer

        For Each d In aDice
            iOccur(d.Result) += 1
        Next

        For i = 0 To 6
            If iOccur(i) >= NumofAKind Then
                Return Me.Result
            End If
        Next
        Return 0
    End Function

    Public Function YhatzeeeFiveOfAKindScore() As Integer

        Const SCORE As Integer = 50

        Dim d As DieSL
        Dim iOccur(6) As Integer
        Dim i As Integer

        For Each d In aDice
            iOccur(d.Result) += 1
        Next

        For i = 0 To 6
            If iOccur(i) >= 5 Then
                Return SCORE
            End If
        Next
        Return 0
    End Function

    Public Function YhatzeeeChanceScore() As Integer
        Return Me.Result
    End Function

    Public Sub KniffelTreeInRow(ByRef Fixed As Boolean, Optional ByVal n As Integer = 3)
        Dim Fr() As Boolean = {False, False, False, False, False, False, False}
        Dim d As DieSL
        Dim iOccur(6) As Integer
        Dim MinNum As Integer = 0
        Dim i As Integer
        For Each d In aDice
            iOccur(d.Result) += 1
        Next

        If iOccur(1) >= 1 And iOccur(2) >= 1 And iOccur(3) >= 1 Then
            MinNum = 1

        End If

        If iOccur(2) >= 1 And iOccur(3) >= 1 And iOccur(4) >= 1 Then
            MinNum = 2

        End If

        If iOccur(3) >= 1 And iOccur(4) >= 1 And iOccur(5) >= 1 Then
            MinNum = 3

        End If
        If iOccur(4) >= 1 And iOccur(5) >= 1 And iOccur(6) >= 1 Then
            MinNum = 4

        End If
        If Not MinNum = 0 Then
            Fixed = True
            For i = MinNum To MinNum + n
                For Each d In aDice
                    If d.Result = i And i < 7 Then
                        If Not Fr(i) Then
                            d.Frozen = True
                            Fr(i) = True
                        End If
                    End If
                Next
            Next
        End If

    End Sub
    Public Sub FixDice(ByVal index As Integer)
        Dim d As DieSL
        For Each d In aDice
            If d.Result = index Then
                d.Frozen = True
            End If
        Next
    End Sub
    Public Function YhatzeeeSmallStraightScore(Optional ByVal ToFix As Boolean = False, Optional ByRef Fixed As Boolean = False, Optional ByVal n As Integer = 3) As Integer
        Dim Fr() As Boolean = {False, False, False, False, False, False, False}
        Const SCORE As Integer = 30
        Dim d As DieSL
        Dim iOccur(6) As Integer
        Dim MinNum As Integer = 0
        Dim i As Integer

        For Each d In aDice
            iOccur(d.Result) += 1
        Next

        If iOccur(1) >= 1 And iOccur(2) >= 1 And iOccur(3) >= 1 And iOccur(4) >= 1 Then
            MinNum = 1
        End If

        If iOccur(2) >= 1 And iOccur(3) >= 1 And iOccur(4) >= 1 And iOccur(5) >= 1 Then
            MinNum = 2
        End If

        If iOccur(3) >= 1 And iOccur(4) >= 1 And iOccur(5) >= 1 And iOccur(6) >= 1 Then
            MinNum = 3

        End If
        If Not MinNum = 0 Then
            If ToFix Then
                Fixed = True
                For i = MinNum To MinNum + n
                    For Each d In aDice
                        If d.Result = i And i < 7 Then

                            If Not Fr(i) Then
                                d.Frozen = True
                                Fr(i) = True

                            End If
                        End If
                    Next
                Next
            End If
            Return SCORE
        End If
        Return 0
    End Function

    Public Function YhatzeeeLargeStraightScore() As Integer

        Const SCORE As Integer = 40
        Dim d As DieSL
        Dim iOccur(6) As Integer

        For Each d In aDice
            iOccur(d.Result) += 1
        Next

        If iOccur(1) = 1 And iOccur(2) = 1 And iOccur(3) = 1 And iOccur(4) = 1 And iOccur(5) = 1 Then
            Return SCORE
        End If

        If iOccur(2) = 1 And iOccur(3) = 1 And iOccur(4) = 1 And iOccur(5) = 1 And iOccur(6) = 1 Then
            Return SCORE
        End If
        Return 0
    End Function

    Public Function YhatzeeeFullHouseScore() As Integer

        Const SCORE As Integer = 25
        Dim d As DieSL
        Dim iOccur(6) As Integer
        Dim i As Integer
        Dim bPair As Boolean
        Dim bTrip As Boolean


        For Each d In aDice
            iOccur(d.Result) += 1
        Next

        For i = 0 To 6
            If iOccur(i) = 2 Then
                bPair = True
            ElseIf iOccur(i) = 3 Then
                bTrip = True
            End If
        Next

        If bPair And bTrip Then
            Return SCORE
        End If
        Return 0
    End Function



End Class
Public Class DieSL
    Inherits UserControl

    Private Const MAXMOVE As Integer = 5
    Public mSound As MediaElement

    Private Enum DieStatus
        dsStopped = 0
        dsRolling = 1
        dsLanding = 2
    End Enum
    Private _png As Image
    Public Property PNG As Image
        Get
            Return _png
        End Get
        Set(ByVal value As Image)
            _png = value
        End Set
    End Property
    Private FRollLoop As Integer

    Private h As Integer = 72
    Private w As Integer = 72
    Private FxPos As Integer
    Private FyPos As Integer

    Private dxDir As Integer
    Private dyDir As Integer         'направление движения

    Private FPanel As DicePanelSL
    Private FStatus As DieStatus = DieStatus.dsLanding

    Private strstyle As String
    Private strrot As String
    Private strframe As String


    Private Function GetFramePic() As BitmapSource
        'If bA.Length = 0 Then Return Nothing
        Dim sPath As String = strstyle & strrot & Frame.ToString & ".png"
        
        Return FPanel.DieFrameImages(sPath)

    End Function
    Public Sub New(ByVal pn As DicePanelSL)
        MyBase.New()
        PNG = New Image
        'Style = pn.Style
        FPanel = pn
        'mSound.AutoPlay = False
        strstyle = "/Images/" & FPanel.strStyle
    End Sub
    Public IsPlaying As Boolean
    Public Sub PlaySound()
        mSound = New MediaElement
        Dim sri As StreamResourceInfo = Application.GetResourceStream(New Uri("/KiniffelOnline;component/diceroll1.mp3", UriKind.RelativeOrAbsolute))
        mSound.SetSource(sri.Stream)
        IsPlaying = True
    End Sub
    Public Sub StopSound()
        IsPlaying = False
        mSound.Stop()
        mSound = Nothing
    End Sub

    Private FFrame As Integer
    Private Property Frame() As Integer
        Get
            Return FFrame
        End Get
        Set(ByVal Value As Integer)
            FFrame = Value

            If FFrame < 0 Then FFrame += 36
            If FFrame > 35 Then FFrame -= 36
        End Set
    End Property


    Private FResult As Integer = 1     'результат кости 1-6
    Property Result() As Integer
        Get
            Return FResult
        End Get
        Set(ByVal Value As Integer)
            If Value < 1 Or Value > 6 Then
                Throw New Exception("Неправильное значение кости")
            Else
                FResult = Value
            End If
        End Set
    End Property

    Private Property xPos() As Integer
        Get
            Return FxPos
        End Get
        Set(ByVal Value As Integer)
            Dim ks As Integer
            'псевдо перспектива - сужение стола на верху
            'If FPanel.TreeDScale Then
            '    Dim ks1 = FPanel.TreeDScaleCoef / 2 * FPanel.ActualWidth
            '    Dim M As Integer = FPanel.ActualHeight - w
            '    ks = (M - yPos) * ks1 / M
            '    ' MessageBox.Show(FPanel.TreeDScaleCoef & ",  " & ks1)
            'End If

            FxPos = Value

            If FxPos < 0 + ks Then
                FxPos = 0 + ks
                Call BounceX()
            End If
            If FxPos > FPanel.ActualWidth - w - ks Then
                FxPos = FPanel.ActualWidth - w - ks
                Call BounceX()
            End If
        End Set
    End Property

    Private Property yPos() As Integer
        Get
            Return FyPos
        End Get
        Set(ByVal Value As Integer)
            FyPos = Value

            If FyPos < 0 Then
                FyPos = 0
                Call BounceY()
            End If
            If FyPos > FPanel.ActualHeight - h Then
                FyPos = FPanel.ActualHeight - h
                Call BounceY()
            End If
        End Set
    End Property


    Private FFrozen As Boolean
    Property Frozen() As Boolean
        Get
            Return FFrozen
        End Get
        Set(ByVal Value As Boolean)
            FFrozen = Value

        End Set
    End Property

    Public Sub InitializeLocation()
        Try
            xPos = FPanel.FRand.Next(0, FPanel.ActualWidth - w)
            yPos = FPanel.FRand.Next(0, FPanel.ActualHeight - h)
        Catch oEX As Exception
            xPos = 0
            yPos = 0
        End Try
        UpdatePngPosition()
    End Sub
    Public iSound As Integer
    Public Sub UpdateDiePosition()

        Select Case pStatus
            Case DieStatus.dsLanding

                Frame -= 1
            Case DieStatus.dsRolling
                'увеличиваем либо уменьшаем в зависимости отнаправления
                Frame += (1 * Sign(dyDir))
                'mSound.Stop() 'Position = TimeSpan.Zero
                

            Case DieStatus.dsStopped
                Exit Sub
        End Select


        xPos += dxDir
        yPos += dyDir

        FRollLoop += 1

        Select Case pStatus
            Case DieStatus.dsRolling
                'если достигнуто максимальное число вращений останавливаемся
                If FRollLoop > FPanel.MaxRollLoop And FPanel.FRand.Next(1, 100) < 10 Then
                    pStatus = DieStatus.dsLanding
                    FRollLoop = 0

                    Frame = Result * 6
                End If

            Case DieStatus.dsLanding

                If FRollLoop > (5 - FPanel.DieAngle) Then
                    pStatus = DieStatus.dsStopped
                End If
        End Select
        UpdatePngPosition()
    End Sub
    Private Sub UpdatePngPosition()
        PNG.SetValue(Canvas.LeftProperty, CDbl(xPos))
        PNG.SetValue(Canvas.TopProperty, CDbl(yPos))
        
    End Sub
    'new, done for directional changing, collision but
    Private Property pStatus() As DieStatus
        Get
            Return FStatus
        End Get
        Set(ByVal Value As DieStatus)
            FStatus = Value
            If Value = DieStatus.dsStopped Then
                dxDir = 0       'stop direction
                dyDir = 0

            End If
        End Set
    End Property

    Public Sub InitializeRoll(Optional ByVal iResult As Integer = 0)
        If iResult < 0 Or iResult > 6 Then iResult = 0
        'new
        If Not Frozen Then
            'скорость и направление начального движения
            Do
                dxDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1)
            Loop Until Abs(dxDir) > 2
            Do
                dyDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1)
            Loop Until Abs(dyDir) > 2
            If iResult = 0 Then
                Result = FPanel.FRand.Next(1, 7)      'decide what the result will be
            Else
                Result = iResult
            End If
            FRollLoop = 0
            pStatus = DieStatus.dsRolling
        Else
            pStatus = DieStatus.dsStopped
        End If
    End Sub
    'Private _style As dpStyle = dpStyle.dpsClassic
    'Public Property Style As dpStyle
    '    Get
    '        Return _style
    '    End Get
    '    Set(ByVal value As dpStyle)
    '        _style = value
    '    End Set
    'End Property
    Public Sub DrawDie()
        'RESCALE
        If FPanel.TreeDScale Then
            Dim M As Integer = FPanel.ActualHeight - 72
            Dim k As Double = Me.yPos / M * FPanel.TreeDScaleCoef + (1 - FPanel.TreeDScaleCoef)
            Me.h = 72 * k
            Me.w = 72 * k
            Dim st As New ScaleTransform
            st.ScaleX = k
            st.ScaleY = k
            st.CenterX = PNG.Width / 2
            st.CenterY = PNG.Height / 2
            PNG.RenderTransform = st
        End If
        'select the correct bitmap based on what the die is doing, and what direction it's going
        If pStatus = DieStatus.dsRolling Then
            PNG.Opacity = 1
            If (dxDir * dyDir) > 0 Then
                strrot = "yrot."
                PNG.Source = GetFramePic()

            Else
                strrot = "xrot."
                PNG.Source = GetFramePic()
            End If
        Else

            Frame = (Result - 1) * 6 + FPanel.DieAngle
            strrot = "stop."
            PNG.Source = GetFramePic()
            If Frozen Then
                'strrot = "stop." '"halo."
                PNG.Opacity = 0.5
            Else
                PNG.Opacity = 1
            End If
        End If







    End Sub

    ReadOnly Property IsNotRolling() As Boolean
        Get
            Return pStatus = DieStatus.dsStopped
        End Get
    End Property

    ReadOnly Property IsRolling() As Boolean
        Get
            Return Not IsNotRolling
        End Get
    End Property

    ReadOnly Property Rect() As Rect
        Get
            Return New Rect(xPos, yPos, w, h)
        End Get
    End Property

    Public Function Overlapping(ByVal d As DieSL) As Boolean
        'Return d.Rect.Interse(Me.Rect)

        Dim rect1 As Rect = Me.Rect
        rect1.Intersect(d.Rect)
        Return Not rect1 = Rect.Empty
    End Function

    Public Sub HandleCollision(ByVal d As DieSL)
        If Me.Overlapping(d) Then
            If Abs(d.yPos - Me.yPos) <= Abs(d.xPos - Me.xPos) Then
                HandleBounceX(d)
            Else
                HandleBounceY(d)
            End If
        End If
    End Sub

    Private Sub HandleBounceX(ByVal d As DieSL)

        Dim dLeft As DieSL
        Dim dRight As DieSL

        If Me.xPos < d.xPos Then
            dLeft = Me
            dRight = d
        Else
            dLeft = d
            dRight = Me
        End If

        'moving toward each other
        If dLeft.dxDir >= 0 And dRight.dxDir <= 0 Then
            Me.BounceX()
            d.BounceX()
            Exit Sub
        End If

        'moving right, left one caught up to right one
        If dLeft.dxDir > 0 And dRight.dxDir >= 0 Then
            dLeft.BounceX()
            Exit Sub
        End If

        'moving left, right one caught up to left one
        If dLeft.dxDir <= 0 And dRight.dxDir < 0 Then
            dRight.BounceX()
        End If

    End Sub

    Private Sub HandleBounceY(ByVal d As DieSL)

        Dim dTop As DieSL
        Dim dBot As DieSL

        If Me.yPos < d.yPos Then
            dTop = Me
            dBot = d
        Else
            dTop = d
            dBot = Me
        End If

        If dTop.dyDir >= 0 And dBot.dyDir <= 0 Then
            Me.BounceY()
            d.BounceY()
            Exit Sub
        End If

        'moving down, top one caught up to bottom one
        If dTop.dyDir > 0 And dBot.dyDir >= 0 Then
            dTop.BounceY()
            Exit Sub
        End If

        'moving left, bottom one caught up to top one
        If dTop.dyDir <= 0 And dBot.dyDir < 0 Then
            dBot.BounceY()
        End If

    End Sub

    Private Sub BounceX()
        dxDir = -dxDir

        'no sound if not moving
        If pStatus <> DieStatus.dsStopped Then
            FPanel.OnDieBounced()
        End If
    End Sub

    Private Sub BounceY()
        dyDir = -dyDir

        'no sound if not moving
        If pStatus <> DieStatus.dsStopped Then
            FPanel.OnDieBounced()
        End If
    End Sub

    'new
    Public Function ClickedOn(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return Me.Rect.Contains(New Point(x, y))
    End Function

End Class
