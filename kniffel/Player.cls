VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Player"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
'local variable(s) to hold property value(s)
Private mvarNickName As String 'local copy
Private mvarPassword As String 'local copy
Private mvarValue1 As Byte 'local copy
Private mvarValue2 As Byte 'local copy
Private mvarValue3 As Byte 'local copy
Private mvarValue4 As Byte 'local copy
Private mvarValue5 As Byte 'local copy
Private mvarIsComp As Boolean 'local copy
Public Property Let IsComp(ByVal vData As Boolean)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.IsComp = 5
    mvarIsComp = vData
End Property


Public Property Get IsComp() As Boolean
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.IsComp
    IsComp = mvarIsComp
End Property



Public Property Let Value5(ByVal vData As Byte)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Value5 = 5
    mvarValue5 = vData
End Property


Public Property Get Value5() As Byte
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Value5
    Value5 = mvarValue5
End Property



Public Property Let Value4(ByVal vData As Byte)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Value4 = 5
    mvarValue4 = vData
End Property


Public Property Get Value4() As Byte
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Value4
    Value4 = mvarValue4
End Property



Public Property Let Value3(ByVal vData As Byte)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Value3 = 5
    mvarValue3 = vData
End Property


Public Property Get Value3() As Byte
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Value3
    Value3 = mvarValue3
End Property



Public Property Let Value2(ByVal vData As Byte)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Value2 = 5
    mvarValue2 = vData
End Property


Public Property Get Value2() As Byte
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Value2
    Value2 = mvarValue2
End Property



Public Property Let Value1(ByVal vData As Byte)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Value1 = 5
    mvarValue1 = vData
End Property


Public Property Get Value1() As Byte
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Value1
    Value1 = mvarValue1
End Property



Public Property Let Password(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Password = 5
    mvarPassword = vData
End Property


Public Property Get Password() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Password
    Password = mvarPassword
End Property



Public Property Let NickName(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.NickName = 5
    mvarNickName = vData
End Property


Public Property Get NickName() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.NickName
    NickName = mvarNickName
End Property



