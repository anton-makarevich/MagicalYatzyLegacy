VERSION 5.00
Begin VB.Form frmQP 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Игроки"
   ClientHeight    =   5400
   ClientLeft      =   45
   ClientTop       =   315
   ClientWidth     =   3360
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5400
   ScaleWidth      =   3360
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cbOK 
      Caption         =   "OK"
      Height          =   375
      Left            =   960
      TabIndex        =   2
      Top             =   4920
      Width           =   1095
   End
   Begin VB.CommandButton cbCancel 
      Caption         =   "Отмена"
      Height          =   375
      Left            =   2160
      TabIndex        =   1
      Top             =   4920
      Width           =   1095
   End
   Begin VB.Frame frQP 
      Height          =   4815
      Left            =   120
      TabIndex        =   0
      Top             =   0
      Width           =   3135
      Begin VB.TextBox txtNP 
         Enabled         =   0   'False
         Height          =   285
         Index           =   9
         Left            =   360
         TabIndex        =   34
         Text            =   "АсТон"
         Top             =   4440
         Width           =   1935
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Enabled         =   0   'False
         Height          =   255
         Index           =   9
         Left            =   2520
         TabIndex        =   33
         Top             =   4440
         Width           =   255
      End
      Begin VB.TextBox txtNP 
         Enabled         =   0   'False
         Height          =   285
         Index           =   8
         Left            =   360
         TabIndex        =   31
         Text            =   "Раджа"
         Top             =   4080
         Width           =   1935
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Enabled         =   0   'False
         Height          =   255
         Index           =   8
         Left            =   2520
         TabIndex        =   30
         Top             =   4080
         Value           =   1  'Отмечено
         Width           =   255
      End
      Begin VB.TextBox txtNP 
         Enabled         =   0   'False
         Height          =   285
         Index           =   7
         Left            =   360
         TabIndex        =   28
         Text            =   "баба"
         Top             =   3720
         Width           =   1935
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Enabled         =   0   'False
         Height          =   255
         Index           =   7
         Left            =   2520
         TabIndex        =   27
         Top             =   3720
         Value           =   1  'Отмечено
         Width           =   255
      End
      Begin VB.TextBox txtNP 
         Enabled         =   0   'False
         Height          =   285
         Index           =   6
         Left            =   360
         TabIndex        =   25
         Text            =   "Вера"
         Top             =   3360
         Width           =   1935
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Enabled         =   0   'False
         Height          =   255
         Index           =   6
         Left            =   2520
         TabIndex        =   24
         Top             =   3360
         Value           =   1  'Отмечено
         Width           =   255
      End
      Begin VB.TextBox txtNP 
         Enabled         =   0   'False
         Height          =   285
         Index           =   5
         Left            =   360
         TabIndex        =   22
         Text            =   "Вова"
         Top             =   3000
         Width           =   1935
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Enabled         =   0   'False
         Height          =   255
         Index           =   5
         Left            =   2520
         TabIndex        =   21
         Top             =   3000
         Value           =   1  'Отмечено
         Width           =   255
      End
      Begin VB.TextBox txtNP 
         Enabled         =   0   'False
         Height          =   285
         Index           =   4
         Left            =   360
         TabIndex        =   19
         Text            =   "Саша"
         Top             =   2640
         Width           =   1935
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Enabled         =   0   'False
         Height          =   255
         Index           =   4
         Left            =   2520
         TabIndex        =   18
         Top             =   2640
         Value           =   1  'Отмечено
         Width           =   255
      End
      Begin VB.TextBox txtNP 
         Enabled         =   0   'False
         Height          =   285
         Index           =   3
         Left            =   360
         TabIndex        =   16
         Text            =   "Ancia"
         Top             =   2280
         Width           =   1935
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Enabled         =   0   'False
         Height          =   255
         Index           =   3
         Left            =   2520
         TabIndex        =   15
         Top             =   2280
         Value           =   1  'Отмечено
         Width           =   255
      End
      Begin VB.TextBox txtNP 
         Height          =   285
         Index           =   2
         Left            =   360
         TabIndex        =   13
         Text            =   "ju_pi"
         Top             =   1920
         Width           =   1935
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Height          =   255
         Index           =   2
         Left            =   2520
         TabIndex        =   12
         Top             =   1920
         Value           =   1  'Отмечено
         Width           =   255
      End
      Begin VB.CheckBox chHuman 
         Caption         =   "Check1"
         Height          =   255
         Index           =   1
         Left            =   2520
         TabIndex        =   10
         Top             =   1560
         Value           =   1  'Отмечено
         Width           =   255
      End
      Begin VB.TextBox txtNP 
         Height          =   285
         Index           =   1
         Left            =   360
         TabIndex        =   9
         Text            =   "LW"
         Top             =   1560
         Width           =   1935
      End
      Begin VB.TextBox txtQP 
         Alignment       =   2  'Центровка
         BeginProperty Font 
            Name            =   "Arial"
            Size            =   15.75
            Charset         =   204
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   480
         Left            =   2520
         Locked          =   -1  'True
         TabIndex        =   5
         Text            =   "2"
         Top             =   480
         Width           =   375
      End
      Begin VB.CommandButton cbLess 
         Caption         =   "-"
         Height          =   255
         Left            =   2520
         TabIndex        =   4
         Top             =   960
         Width           =   375
      End
      Begin VB.CommandButton cbMore 
         Caption         =   "+"
         Height          =   255
         Left            =   2520
         TabIndex        =   3
         Top             =   240
         Width           =   375
      End
      Begin VB.Label Label4 
         Caption         =   "9"
         Height          =   255
         Index           =   8
         Left            =   120
         TabIndex        =   35
         Top             =   4440
         Width           =   135
      End
      Begin VB.Label Label4 
         Caption         =   "8"
         Height          =   255
         Index           =   7
         Left            =   120
         TabIndex        =   32
         Top             =   4080
         Width           =   135
      End
      Begin VB.Label Label4 
         Caption         =   "7"
         Height          =   255
         Index           =   6
         Left            =   120
         TabIndex        =   29
         Top             =   3720
         Width           =   135
      End
      Begin VB.Label Label4 
         Caption         =   "6"
         Height          =   255
         Index           =   5
         Left            =   120
         TabIndex        =   26
         Top             =   3360
         Width           =   135
      End
      Begin VB.Label Label4 
         Caption         =   "5"
         Height          =   255
         Index           =   4
         Left            =   120
         TabIndex        =   23
         Top             =   3000
         Width           =   135
      End
      Begin VB.Label Label4 
         Caption         =   "4"
         Height          =   255
         Index           =   3
         Left            =   120
         TabIndex        =   20
         Top             =   2640
         Width           =   135
      End
      Begin VB.Label Label4 
         Caption         =   "3"
         Height          =   255
         Index           =   2
         Left            =   120
         TabIndex        =   17
         Top             =   2280
         Width           =   135
      End
      Begin VB.Label Label4 
         Caption         =   "2"
         Height          =   255
         Index           =   1
         Left            =   120
         TabIndex        =   14
         Top             =   1920
         Width           =   135
      End
      Begin VB.Label Label4 
         Caption         =   "1"
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   11
         Top             =   1560
         Width           =   135
      End
      Begin VB.Label Label3 
         Caption         =   "Человек"
         Height          =   255
         Left            =   2280
         TabIndex        =   8
         Top             =   1320
         Width           =   735
      End
      Begin VB.Label Label2 
         Caption         =   "Имя"
         Height          =   255
         Left            =   360
         TabIndex        =   7
         Top             =   1320
         Width           =   1215
      End
      Begin VB.Label Label1 
         Caption         =   "Введите количество  игроков?"
         Height          =   255
         Left            =   120
         TabIndex        =   6
         Top             =   600
         Width           =   2655
      End
   End
End
Attribute VB_Name = "frmQP"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cbLess_Click()
    If IsNumeric(txtQP.Text) Then
        If CSng(txtQP.Text) > 2 Then
            txtNP(txtQP.Text).Enabled = False
            chHuman(txtQP.Text).Enabled = False
            txtQP.Text = CStr(CSng(txtQP.Text) - 1)
        End If
    End If
End Sub

Private Sub cbMore_Click()
    If IsNumeric(txtQP.Text) Then
        If CSng(txtQP.Text) < 9 Then
            txtQP.Text = CStr(CSng(txtQP.Text) + 1)
            txtNP(txtQP.Text).Enabled = True
            chHuman(txtQP.Text).Enabled = True
        End If
    End If
End Sub

Private Sub cbOK_Click()
    Me.Hide
    frmMain.CurGame.NumbPlayers = CByte(txtQP.Text)
    frmMain.MoveIt
End Sub

'Private Sub txtQP_Change()
'    frmMain.CurGame.NumbPlayers = CByte(txtQP.Text)
    'frmMain.CurGame.Hod
'End Sub
