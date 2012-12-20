<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmResults
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmResults))
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.colScore = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colPlayer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.cbPlayAgain = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.Location = New System.Drawing.Point(258, 222)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 1
        Me.OK_Button.Text = "OK"
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.CheckBoxes = True
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colScore, Me.colPlayer})
        Me.ListView1.Location = New System.Drawing.Point(12, 12)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(313, 126)
        Me.ListView1.TabIndex = 2
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'colScore
        '
        Me.colScore.Text = "Очки"
        Me.colScore.Width = 120
        '
        'colPlayer
        '
        Me.colPlayer.Text = "Игрок"
        Me.colPlayer.Width = 160
        '
        'LinkLabel1
        '
        Me.LinkLabel1.LinkArea = New System.Windows.Forms.LinkArea(66, 23)
        Me.LinkLabel1.Location = New System.Drawing.Point(12, 141)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(313, 41)
        Me.LinkLabel1.TabIndex = 3
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Отметьте флажки  игроков, результаты которых Вы хотите добавить в книгу рекордов " & _
            "Книффеля. Нельзя отметить результаты компьютера"
        Me.LinkLabel1.UseCompatibleTextRendering = True
        '
        'cbPlayAgain
        '
        Me.cbPlayAgain.AutoSize = True
        Me.cbPlayAgain.Checked = True
        Me.cbPlayAgain.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbPlayAgain.Location = New System.Drawing.Point(12, 192)
        Me.cbPlayAgain.Name = "cbPlayAgain"
        Me.cbPlayAgain.Size = New System.Drawing.Size(103, 17)
        Me.cbPlayAgain.TabIndex = 4
        Me.cbPlayAgain.Text = "Еще партейку?"
        Me.cbPlayAgain.UseVisualStyleBackColor = True
        '
        'frmResults
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(337, 257)
        Me.ControlBox = False
        Me.Controls.Add(Me.cbPlayAgain)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.OK_Button)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmResults"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Результаты"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents colScore As System.Windows.Forms.ColumnHeader
    Friend WithEvents colPlayer As System.Windows.Forms.ColumnHeader
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents cbPlayAgain As System.Windows.Forms.CheckBox

End Class
