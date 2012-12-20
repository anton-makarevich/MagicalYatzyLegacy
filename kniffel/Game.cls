VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Game"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Private mvarNumbPlayers As Byte 'local copy
Private mvarNamePlayers As Collection 'local copy


Public Property Set NamePlayers(ByVal vData As Collection)
'used when assigning an Object to the property, on the left side of a Set statement.
'Syntax: Set x.NamePlayers = Form1
    Set mvarNamePlayers = vData
End Property


Public Property Get NamePlayers() As Collection
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.NamePlayers
    Set NamePlayers = mvarNamePlayers
End Property



Public Property Let NumbPlayers(ByVal vData As Byte)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.NumbPlayers = 5
    mvarNumbPlayers = vData
End Property


Public Property Get NumbPlayers() As Byte
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.NumbPlayers
    NumbPlayers = mvarNumbPlayers
End Property



