Imports System.Math
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports System.ComponentModel


Public Enum dpStyle
    dpsClassic = 0
    dpsBrutalRed = 1
    dpsBlue = 2
End Enum
<ToolboxItem(True)> _
Public Class DicePanelNew
    Inherits System.Windows.Forms.Panel

    Private aDice As ArrayList

    Protected FbStop As Bitmap
    Protected FbxRot As Bitmap
    Protected FbyRot As Bitmap
    Protected FbHalo As Bitmap

    Protected FRand As New Random

    Private bBack As Bitmap        'background bitmap

    Public Event DieBounced()
    Public Event DieFrozen(ByVal bUnFreeze As Boolean)
    Private FStyle As dpStyle = dpStyle.dpsClassic

    Public sDiceSound As System.IO.Stream
    Public Property PlaySound As Boolean

    Public Sub New()
        MyBase.New()

        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        SetBitmaps()
    End Sub
    Private Sub SetBitmaps()
        Me.BackColor = dpBackcolor

        Dim a As Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        sDiceSound = a.GetManifestResourceStream("Kniffel.diceroll.wav")
        'Dim gr As Graphics
        Select Case FStyle
            Case dpStyle.dpsClassic
                FbxRot = New Bitmap(a.GetManifestResourceStream("Kniffel.xrot0.png"))
                FbyRot = New Bitmap(a.GetManifestResourceStream("Kniffel.yrot0.png"))
                FbStop = New Bitmap(a.GetManifestResourceStream("Kniffel.stop0.png"))
                FbHalo = New Bitmap(a.GetManifestResourceStream("Kniffel.halo0.png"))


            Case dpStyle.dpsBrutalRed
                FbxRot = New Bitmap(a.GetManifestResourceStream("Kniffel.xrot1.png"))
                FbyRot = New Bitmap(a.GetManifestResourceStream("Kniffel.yrot1.png"))
                FbStop = New Bitmap(a.GetManifestResourceStream("Kniffel.stop1.png"))
                FbHalo = New Bitmap(a.GetManifestResourceStream("Kniffel.halo1.png"))

            Case dpStyle.dpsBlue
                FbxRot = New Bitmap(a.GetManifestResourceStream("Kniffel.xrot2.png"))
                FbyRot = New Bitmap(a.GetManifestResourceStream("Kniffel.yrot2.png"))
                FbStop = New Bitmap(a.GetManifestResourceStream("Kniffel.stop2.png"))
                FbHalo = New Bitmap(a.GetManifestResourceStream("Kniffel.halo2.png"))

        End Select
        'If Not FStyle = dpStyle.dpsBrutalRed Then
        'Dim xc, yc As Integer
        'FbHalo = New Bitmap(432, 432, PixelFormat.Format16bppRgb565)
        'For xc = 1 To 430
        '    For yc = 1 To 430
        '        FbHalo.SetPixel(xc, yc, FbStop.GetPixel(xc, yc))
        '    Next
        'Next
        'For xc = 1 To 430 Step 3
        '    For yc = 1 To 430 Step 2
        '        FbHalo.SetPixel(xc + 1, yc, Color.Black)
        '        FbHalo.SetPixel(xc, yc, Color.Black)
        '    Next
        'Next
        ''End If
        'FbxRot.MakeTransparent(Color.Black)
        'FbyRot.MakeTransparent(Color.Black)
        'FbStop.MakeTransparent(Color.Black)
        ''new
        'FbHalo.MakeTransparent(Color.Black)
    End Sub
    Private dpBackcolor As Color = Color.Green
    Public Property Style() As dpStyle
        Get
            Return FStyle
        End Get
        Set(ByVal value As dpStyle)
            FStyle = value
            Select Case FStyle
                Case dpStyle.dpsClassic
                    dpBackcolor = Color.Green
                Case dpStyle.dpsBrutalRed
                    dpBackcolor = Color.Black
                Case dpStyle.dpsBlue
                    dpBackcolor = Color.FromArgb(100, Color.Beige)
            End Select
            SetBitmaps()
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
    Public Overloads Sub Dispose()
        FbxRot.Dispose()
        FbyRot.Dispose()
        FbStop.Dispose()
        bBack.Dispose()
        MyBase.Dispose()
    End Sub

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

            'regen dice, but only if done once before, or else dbl init
            If DiceGenerated() Then
                Dim d As Die
                GenerateDice()
                Clear()
                For Each d In aDice
                    d.DrawDie(bBack)
                Next
                Me.Invalidate()
            End If
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

        Dim d As Die
        Dim dOld As Die
        Dim bDone As Boolean
        Dim iTry As Integer

        aDice = New ArrayList

        Do While aDice.Count < NumDice
            d = New Die(Me)
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
        Loop

    End Sub

    <Description("Summed Result of All the Dice"), Category("Dice")> _
    ReadOnly Property Result() As Integer
        Get
            Dim d As Die
            Dim i As Integer = 0

            For Each d In aDice
                i += d.Result
            Next
            Return i
        End Get
    End Property
    'Public ReadOnly Property AllDiceFrozen() As Boolean
    '    Get
    '        Dim d As Die
    '        Dim r As Boolean

    '        r = True
    '        For Each d In aDice
    '            r = d.Frozen

    '        Next

    '        Return r
    '    End Get
    'End Property
    'new, changed visibility
    Public ReadOnly Property AllDiceStopped() As Boolean
        Get
            Dim d As Die
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

    Public Sub RollDice(Optional ByRef aResults As ArrayList = Nothing)

        Dim d As Die


        'don't roll if all frozen
        If AllDiceFrozen() Then Exit Sub

        For i As Integer = 0 To aDice.Count - 1
            Dim iResult As Integer = 0
            Try
                iResult = aResults(i)
            Catch ex As Exception

            End Try
            d = aDice(i)
            d.InitializeRoll(iResult)
        Next

        Do
            Clear()
            For Each d In aDice
                d.UpdateDiePosition()
                d.DrawDie(bBack)
            Next
            Threading.Thread.Sleep(RollDelay)
            HandleCollisions()
            Me.Invalidate()
            Application.DoEvents()
        Loop Until AllDiceStopped

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

        Dim di As Die
        Dim dj As Die
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

    Public Sub SetupBackgroundAndDice()
        MakeBackgroundBitmap()

        Dim d As Die
        If Not DiceGenerated() Then
            GenerateDice()
        End If
        For Each d In aDice
            d.DrawDie(bBack)
        Next

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        'happens in design mode
        If bBack Is Nothing Then
            Call SetupBackgroundAndDice()
        End If
        e.Graphics.DrawImageUnscaled(bBack, 0, 0)
    End Sub

    Protected Overrides Sub OnResize(ByVal eventargs As System.EventArgs)
        MyBase.OnResize(eventargs)
        Call SetupBackgroundAndDice()
    End Sub

    'new
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim d As Die
        Dim bFound As Boolean = False

        If e.Button = MouseButtons.Left Then
            If ClickToFreeze Then

                For Each d In aDice
                    If d.ClickedOn(e.X, e.Y) Then
                        d.Frozen = Not d.Frozen
                        bFound = True
                        Exit For
                    End If
                Next

                If bFound Then
                    Clear()
                    For Each d In aDice
                        d.DrawDie(bBack)
                    Next
                    Invalidate()
                End If

                Exit Sub    'don't run mybase if clicktofrezze
            End If
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Private Sub MakeBackgroundBitmap()
        If Not bBack Is Nothing Then bBack.Dispose()
        bBack = New Bitmap(Me.Width, Me.Height, PixelFormat.Format32bppPArgb)
        Clear()
    End Sub

    Private Sub Clear()

        Dim gr As Graphics

        gr = Graphics.FromImage(bBack)

        Try
            gr.Clear(dpBackcolor) '(Color.Black)
        Finally
            gr.Dispose()
        End Try
    End Sub

    Private Function DiceGenerated() As Boolean
        Return Not (aDice Is Nothing)
    End Function

    Protected Sub OnDieBounced()
        RaiseEvent DieBounced()
    End Sub

    Protected Sub OnDieFrozen(ByVal bUnFreeze As Boolean)
        RaiseEvent DieFrozen(bUnFreeze)
    End Sub

    'new
    'don't roll if all frozen
    Public Function AllDiceFrozen() As Boolean

        Dim d As Die
        If ClickToFreeze Then

            For Each d In aDice
                If Not d.Frozen Then
                    Return False
                End If
            Next
            Return True

        End If

    End Function

    'new
    Public Sub ClearFreeze()
        Dim d As Die

        If ClickToFreeze Then

            For Each d In aDice
                If d.Frozen Then
                    d.Frozen = False
                End If
            Next
        End If

    End Sub

    'the score for the numeric 1-6 categories in Y
    Public Function YhatzeeNumberScore(ByVal iNum As Integer) As Integer

        Dim d As Die
        Dim iTot As Integer = 0

        For Each d In aDice
            If d.Result = iNum Then
                iTot += iNum
            End If
        Next
        Return iTot

    End Function

    Public Function YhatzeeeOfAKindScore(ByVal NumofAKind As Integer) As Integer

        Dim d As Die
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

    End Function

    Public Function YhatzeeeFiveOfAKindScore() As Integer

        Const SCORE As Integer = 50

        Dim d As Die
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

    End Function

    Public Function YhatzeeeChanceScore() As Integer
        Return Me.Result
    End Function

    Public Sub KniffelTreeInRow(ByRef Fixed As Boolean, Optional ByVal n As Integer = 3)
        Dim Fr() As Boolean = {False, False, False, False, False, False, False}
        Dim d As Die
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
        Dim d As Die
        For Each d In aDice
            If d.Result = index Then
                d.Frozen = True
            End If
        Next
    End Sub
    Public Function YhatzeeeSmallStraightScore(Optional ByVal ToFix As Boolean = False, Optional ByRef Fixed As Boolean = False, Optional ByVal n As Integer = 3) As Integer
        Dim Fr() As Boolean = {False, False, False, False, False, False, False}
        Const SCORE As Integer = 30
        Dim d As Die
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
    End Function

    Public Function YhatzeeeLargeStraightScore() As Integer

        Const SCORE As Integer = 40
        Dim d As Die
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

    End Function

    Public Function YhatzeeeFullHouseScore() As Integer

        Const SCORE As Integer = 25
        Dim d As Die
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

    End Function

    Private Class Die

        Private Const MAXMOVE As Integer = 5
        Private Enum DieStatus
            dsStopped = 0
            dsRolling = 1
            dsLanding = 2
        End Enum

        Private FRollLoop As Integer

        Private h As Integer = 72
        Private w As Integer = 72
        Private FxPos As Integer
        Private FyPos As Integer

        Private dxDir As Integer
        Private dyDir As Integer         'направление движения

        Private FPanel As DicePanelNew
        Private FStatus As DieStatus = DieStatus.dsLanding

        Private isound As Integer

        Public Sub New(ByVal pn As DicePanelNew)
            MyBase.New()
            FPanel = pn
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


        Private FResult As Integer       'результат кости 1-6
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
                FxPos = Value

                If FxPos < 0 Then
                    FxPos = 0
                    Call BounceX()
                End If
                If FxPos > FPanel.Width - w Then
                    FxPos = FPanel.Width - w
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
                If FyPos > FPanel.Height - h Then
                    FyPos = FPanel.Height - h
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
                FPanel.OnDieFrozen(FFrozen)
            End Set
        End Property

        Public Sub InitializeLocation()
            Try
                xPos = FPanel.FRand.Next(0, FPanel.Width - w)
                yPos = FPanel.FRand.Next(0, FPanel.Height - h)
            Catch oEX As Exception
                xPos = 0
                yPos = 0
            End Try
        End Sub

        Public Sub UpdateDiePosition()

            Select Case pStatus
                Case DieStatus.dsLanding

                    Frame -= 1
                Case DieStatus.dsRolling
                    'увеличиваем либо уменьшаем в зависимости отнаправления
                    Frame += (1 * Sign(dyDir))
                    'play sound
                    If FPanel.PlaySound Then
                        If isound >= 8 Then
                            isound = 1

                            Try
                                My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\diceroll.wav", AudioPlayMode.Background) 'FPanel.sDiceSound, AudioPlayMode.Background)
                            Catch ex As Exception

                            End Try

                        Else
                            isound += 1
                        End If
                    End If


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

        Public Sub DrawDie(ByVal bDest As Bitmap)

            Dim gr As Graphics
            Dim b As Bitmap

            Dim x1 As Integer = (Frame Mod 6) * w
            Dim x As Integer
            Dim y As Integer = (Frame \ 6) * h


            'select the correct bitmap based on what the die is doing, and what direction it's going
            If pStatus = DieStatus.dsRolling Then
                x = x1
                'check quandrant rolling towards based on sign of xdir*ydir
                If (dxDir * dyDir) > 0 Then
                    b = FPanel.FbyRot
                Else
                    b = FPanel.FbxRot
                End If
            Else
                x = 72 * FPanel.DieAngle
                b = FPanel.FbStop
            End If
            Dim r As New System.Drawing.Rectangle(x, y, w, h)
            gr = Graphics.FromImage(bDest)
            Try
                
                If Frozen Then
                    gr.DrawImage(FPanel.FbHalo, xPos, yPos, r, GraphicsUnit.Pixel)
                Else
                    gr.DrawImage(b, xPos, yPos, r, GraphicsUnit.Pixel)
                End If
                'End If



                If FPanel.DebugDrawMode Then
                    Dim p As New Pen(Color.Yellow)
                    Dim xc, yc As Single

                    xc = xPos + (w \ 2)
                    yc = yPos + (h \ 2)

                    gr.DrawRectangle(p, Me.Rect)
                    gr.DrawLine(p, xc, yc, xc + Sign(dxDir) * (w \ 2), yc + Sign(dyDir) * (h \ 2))
                End If
            Finally
                gr.Dispose()
            End Try

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

        ReadOnly Property Rect() As Rectangle
            Get
                Return New Rectangle(xPos, yPos, w, h)
            End Get
        End Property

        Public Function Overlapping(ByVal d As Die) As Boolean
            Return d.Rect.IntersectsWith(Me.Rect)
        End Function

        Public Sub HandleCollision(ByVal d As Die)
            If Me.Overlapping(d) Then
                If Abs(d.yPos - Me.yPos) <= Abs(d.xPos - Me.xPos) Then
                    HandleBounceX(d)
                Else
                    HandleBounceY(d)
                End If
            End If
        End Sub

        Private Sub HandleBounceX(ByVal d As Die)

            Dim dLeft As Die
            Dim dRight As Die

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

        Private Sub HandleBounceY(ByVal d As Die)

            Dim dTop As Die
            Dim dBot As Die

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
            Return Me.Rect.Contains(x, y)
        End Function

    End Class

End Class
