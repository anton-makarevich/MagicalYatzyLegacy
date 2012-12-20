VERSION 5.00
Begin VB.Form frmMain 
   BackColor       =   &H00008080&
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Книфель"
   ClientHeight    =   7650
   ClientLeft      =   150
   ClientTop       =   840
   ClientWidth     =   10905
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7650
   ScaleWidth      =   10905
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cbContinue 
      Caption         =   "Бросок"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   48
         Charset         =   204
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1575
      Left            =   2880
      TabIndex        =   6
      Top             =   600
      Visible         =   0   'False
      Width           =   7215
   End
   Begin VB.Label CPN 
      BackColor       =   &H000080FF&
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   24
         Charset         =   204
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1095
      Left            =   240
      TabIndex        =   5
      Top             =   840
      Width           =   1815
   End
   Begin VB.Label kubik 
      BackColor       =   &H8000000D&
      BorderStyle     =   1  'Фиксировано один
      Caption         =   "1"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   72
         Charset         =   204
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1575
      Index           =   4
      Left            =   8640
      TabIndex        =   4
      Top             =   600
      Width           =   1455
   End
   Begin VB.Label kubik 
      BackColor       =   &H8000000D&
      BorderStyle     =   1  'Фиксировано один
      Caption         =   "1"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   72
         Charset         =   204
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1575
      Index           =   3
      Left            =   7200
      TabIndex        =   3
      Top             =   600
      Width           =   1455
   End
   Begin VB.Label kubik 
      BackColor       =   &H8000000D&
      BorderStyle     =   1  'Фиксировано один
      Caption         =   "1"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   72
         Charset         =   204
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1575
      Index           =   2
      Left            =   5760
      TabIndex        =   2
      Top             =   600
      Width           =   1455
   End
   Begin VB.Label kubik 
      BackColor       =   &H8000000D&
      BorderStyle     =   1  'Фиксировано один
      Caption         =   "1"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   72
         Charset         =   204
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1575
      Index           =   1
      Left            =   4320
      TabIndex        =   1
      Top             =   600
      Width           =   1455
   End
   Begin VB.Label kubik 
      BackColor       =   &H8000000D&
      BorderStyle     =   1  'Фиксировано один
      Caption         =   "1"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   72
         Charset         =   204
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1575
      Index           =   0
      Left            =   2880
      TabIndex        =   0
      Top             =   600
      Width           =   1455
   End
   Begin VB.Menu mnuGame 
      Caption         =   "Игра"
      Begin VB.Menu mnuNew 
         Caption         =   "Новая"
      End
      Begin VB.Menu mnuSeparator1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "Выход"
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Public CurGame As New Game
Dim CurPlayer(1 To 9) As New Player

Public EndGame As Boolean

Dim i As Byte
Dim j As Byte

Dim StopGame As Boolean

Public Sub MoveIt()
    EndGame = False
    For i = 1 To CurGame.NumbPlayers
        CurPlayer(i).NickName = frmQP.txtNP(i).Text
        CurPlayer(i).IsComp = frmQP.chHuman(i).Value
    Next i
    Do Until EndGame = True
        Hod
    Loop
End Sub

Private Sub cbContinue_Click()
    StopGame = False
    cbContinue.Visible = False
End Sub

Private Sub mnuExit_Click()
    End
End Sub

Private Sub mnuNew_Click()
    
    frmQP.Show vbModal, Me
End Sub
Public Sub Hod()
    Dim i As Byte
    For i = 1 To CurGame.NumbPlayers
            
        CPN.Caption = CurPlayer(i).NickName
        
        If CurPlayer(i).IsComp = False Then
            cbContinue.Visible = True
            Do While StopGame = True
                DoEvents
            Loop
        Else
        
        End If
        CurPlayer(i).Value1 = Brosok
        kubik(0).Caption = Brosok
        
        CurPlayer(i).Value2 = Brosok
        kubik(1).Caption = Brosok
        
        CurPlayer(i).Value3 = Brosok
        kubik(2).Caption = Brosok
        
        CurPlayer(i).Value4 = Brosok
        kubik(3).Caption = Brosok
        
        CurPlayer(i).Value5 = Brosok
        kubik(4).Caption = Brosok
        
        StopGame = True
        'Do While StopGame = True
        '    DoEvents
        'Loop
    Next i
End Sub
Public Function Brosok() As Byte
    Randomize
    Brosok = Int(Rnd * 6) + 1
End Function
