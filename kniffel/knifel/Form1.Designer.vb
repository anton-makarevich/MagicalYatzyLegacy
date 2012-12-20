<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.mnuMain = New System.Windows.Forms.MenuStrip()
        Me.‘‡ÈÎToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mKniffelOnlain = New System.Windows.Forms.ToolStripMenuItem()
        Me.mPlayKOE = New System.Windows.Forms.ToolStripMenuItem()
        Me.mPlayKOS = New System.Windows.Forms.ToolStripMenuItem()
        Me.mPlayKOB = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mChemps = New System.Windows.Forms.ToolStripMenuItem()
        Me.mLoosers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mStat = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.bInetStat = New System.Windows.Forms.ToolStripMenuItem()
        Me.mHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.œ‡‚ËÎ‡ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.bHelpOnline = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.cbRoll = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.tbcMain = New amCommonControls.TextBoxConsole()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.bSendMessage = New System.Windows.Forms.Button()
        Me.dcPanel = New Kniffel.DicePanelNew()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.mnuMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuMain
        '
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.‘‡ÈÎToolStripMenuItem, Me.StatToolStripMenuItem, Me.mHelp})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(751, 24)
        Me.mnuMain.TabIndex = 1
        Me.mnuMain.Text = "MenuStrip1"
        '
        '‘‡ÈÎToolStripMenuItem
        '
        Me.‘‡ÈÎToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mNew, Me.ToolStripSeparator1, Me.mKniffelOnlain, Me.ToolStripSeparator3, Me.mOptions, Me.ToolStripSeparator5, Me.mExit})
        Me.‘‡ÈÎToolStripMenuItem.Name = "‘‡ÈÎToolStripMenuItem"
        Me.‘‡ÈÎToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.‘‡ÈÎToolStripMenuItem.Text = "»„‡"
        '
        'mNew
        '
        Me.mNew.Name = "mNew"
        Me.mNew.Size = New System.Drawing.Size(247, 22)
        Me.mNew.Text = "ÕÓ‚‡ˇ Ë„‡..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(244, 6)
        '
        'mKniffelOnlain
        '
        Me.mKniffelOnlain.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mPlayKOE, Me.mPlayKOS, Me.mPlayKOB})
        Me.mKniffelOnlain.Name = "mKniffelOnlain"
        Me.mKniffelOnlain.Size = New System.Drawing.Size(247, 22)
        Me.mKniffelOnlain.Text = "—ÂÚÂ‚‡ˇ Ë„‡ ‚  ÌËÙÙÂÎ¸ ÓÌÎ‡ÈÌ"
        '
        'mPlayKOE
        '
        Me.mPlayKOE.Name = "mPlayKOE"
        Me.mPlayKOE.Size = New System.Drawing.Size(204, 22)
        Me.mPlayKOE.Text = "–‡Ò¯ËÂÌÌ˚È  ÌËÙÙÂÎ¸!"
        '
        'mPlayKOS
        '
        Me.mPlayKOS.Name = "mPlayKOS"
        Me.mPlayKOS.Size = New System.Drawing.Size(204, 22)
        Me.mPlayKOS.Text = "—Ú‡Ì‰‡ÚÌ˚È  ÌËÙÙÂÎ¸!"
        '
        'mPlayKOB
        '
        Me.mPlayKOB.Name = "mPlayKOB"
        Me.mPlayKOB.Size = New System.Drawing.Size(204, 22)
        Me.mPlayKOB.Text = "ƒÂÚÒÍËÈ  ÌËÙÙÂÎ¸!"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(244, 6)
        '
        'mOptions
        '
        Me.mOptions.Name = "mOptions"
        Me.mOptions.Size = New System.Drawing.Size(247, 22)
        Me.mOptions.Text = "Õ‡ÒÚÓÈÍË..."
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(244, 6)
        '
        'mExit
        '
        Me.mExit.Name = "mExit"
        Me.mExit.Size = New System.Drawing.Size(247, 22)
        Me.mExit.Text = "¬˚ıÓ‰"
        '
        'StatToolStripMenuItem
        '
        Me.StatToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mChemps, Me.mLoosers, Me.mStat, Me.ToolStripSeparator4, Me.bInetStat})
        Me.StatToolStripMenuItem.Name = "StatToolStripMenuItem"
        Me.StatToolStripMenuItem.Size = New System.Drawing.Size(79, 20)
        Me.StatToolStripMenuItem.Text = "—Ú‡ÚËÒÚËÍ‡"
        '
        'mChemps
        '
        Me.mChemps.Name = "mChemps"
        Me.mChemps.Size = New System.Drawing.Size(148, 22)
        Me.mChemps.Text = "◊ÂÏÔËÓÌ˚...."
        '
        'mLoosers
        '
        Me.mLoosers.Name = "mLoosers"
        Me.mLoosers.Size = New System.Drawing.Size(148, 22)
        Me.mLoosers.Text = "ÕÂÛ‰‡˜ÌËÍË..."
        '
        'mStat
        '
        Me.mStat.Name = "mStat"
        Me.mStat.Size = New System.Drawing.Size(148, 22)
        Me.mStat.Text = "Œ·˘‡ˇ..."
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(145, 6)
        '
        'bInetStat
        '
        Me.bInetStat.Name = "bInetStat"
        Me.bInetStat.Size = New System.Drawing.Size(148, 22)
        Me.bInetStat.Text = "¬ ËÌÚÂÌÂÚ..."
        '
        'mHelp
        '
        Me.mHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.œ‡‚ËÎ‡ToolStripMenuItem, Me.bHelpOnline, Me.ToolStripSeparator2, Me.mAbout})
        Me.mHelp.Name = "mHelp"
        Me.mHelp.Size = New System.Drawing.Size(62, 20)
        Me.mHelp.Text = "—Ô‡‚Í‡"
        '
        'œ‡‚ËÎ‡ToolStripMenuItem
        '
        Me.œ‡‚ËÎ‡ToolStripMenuItem.Name = "œ‡‚ËÎ‡ToolStripMenuItem"
        Me.œ‡‚ËÎ‡ToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.œ‡‚ËÎ‡ToolStripMenuItem.Text = "œ‡‚ËÎ‡..."
        '
        'bHelpOnline
        '
        Me.bHelpOnline.Name = "bHelpOnline"
        Me.bHelpOnline.Size = New System.Drawing.Size(162, 22)
        Me.bHelpOnline.Text = "—Ô‡‚Í‡ Online..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(159, 6)
        '
        'mAbout
        '
        Me.mAbout.Name = "mAbout"
        Me.mAbout.Size = New System.Drawing.Size(162, 22)
        Me.mAbout.Text = "Œ ÔÓ„‡ÏÏÂ..."
        '
        'cbRoll
        '
        Me.cbRoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbRoll.Location = New System.Drawing.Point(564, 440)
        Me.cbRoll.Name = "cbRoll"
        Me.cbRoll.Size = New System.Drawing.Size(72, 23)
        Me.cbRoll.TabIndex = 2
        Me.cbRoll.Text = "¡ÓÒÓÍ 2"
        Me.cbRoll.UseVisualStyleBackColor = True
        Me.cbRoll.Visible = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 3000
        '
        'tbcMain
        '
        Me.tbcMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbcMain.Location = New System.Drawing.Point(0, 524)
        Me.tbcMain.Multiline = True
        Me.tbcMain.Name = "tbcMain"
        Me.tbcMain.Size = New System.Drawing.Size(751, 80)
        Me.tbcMain.TabIndex = 3
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(0, 608)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(670, 20)
        Me.TextBox1.TabIndex = 4
        '
        'bSendMessage
        '
        Me.bSendMessage.Location = New System.Drawing.Point(676, 606)
        Me.bSendMessage.Name = "bSendMessage"
        Me.bSendMessage.Size = New System.Drawing.Size(75, 23)
        Me.bSendMessage.TabIndex = 5
        Me.bSendMessage.Text = "ŒÚÔ‡‚ËÚ¸"
        Me.bSendMessage.UseVisualStyleBackColor = True
        '
        'dcPanel
        '
        Me.dcPanel.BackColor = System.Drawing.Color.Green
        Me.dcPanel.DieAngle = 0
        Me.dcPanel.Location = New System.Drawing.Point(111, 110)
        Me.dcPanel.MaxRollLoop = 75
        Me.dcPanel.Name = "dcPanel"
        Me.dcPanel.NumDice = 5
        Me.dcPanel.PlaySound = False
        Me.dcPanel.RollDelay = 0
        Me.dcPanel.Size = New System.Drawing.Size(525, 318)
        Me.dcPanel.Style = Kniffel.dpStyle.dpsClassic
        Me.dcPanel.TabIndex = 0
        Me.dcPanel.Visible = False
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(129, 456)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(153, 62)
        Me.WebBrowser1.TabIndex = 6
        Me.WebBrowser1.Visible = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(751, 631)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.bSendMessage)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.tbcMain)
        Me.Controls.Add(Me.cbRoll)
        Me.Controls.Add(Me.dcPanel)
        Me.Controls.Add(Me.mnuMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.mnuMain
        Me.Name = "frmMain"
        Me.Text = " ÌËÙÙÂÎ¸"
        Me.mnuMain.ResumeLayout(False)
        Me.mnuMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dcPanel As Kniffel.DicePanelNew
    Friend WithEvents mnuMain As System.Windows.Forms.MenuStrip
    Friend WithEvents ‘‡ÈÎToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cbRoll As System.Windows.Forms.Button
    Friend WithEvents mHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents œ‡‚ËÎ‡ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents mOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mChemps As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mLoosers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mStat As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tbcMain As amCommonControls.TextBoxConsole
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents bSendMessage As System.Windows.Forms.Button
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bInetStat As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bHelpOnline As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mKniffelOnlain As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mPlayKOE As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mPlayKOS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mPlayKOB As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser

End Class
