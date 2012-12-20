Imports Microsoft.VisualBasic
Imports System.Data.OleDb
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports System
Namespace Kniffel.Web
    Public Class KniffelGameOleDbProvider
        Public Const dbConnStr As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\kniffelgame.mdb;Persist Security Info=True;Jet OLEDB:Database Password=12345qwert"

        Public dbConnection As OleDbConnection

        Public Sub New(Optional ByVal strConn As String = "")
            If strConn = "" Then
                dbConnection = New OleDbConnection(dbConnStr)
            Else
                dbConnection = New OleDbConnection(strConn)
            End If

            dbConnection.Open()


        End Sub
        'inserts
        Public Function InsertPlayer(ByVal username As String, ByVal pass As String, ByVal ID As String, ByVal GameID As Integer) As Boolean

            Dim dbCommand As OleDbCommand
            If dbConnection Is Nothing Then
                dbConnection = New OleDbConnection(dbConnStr)
                dbConnection.Open()
            End If
            Dim strSQL As String = "INSERT INTO ActivePlayers VALUES('" & username & "', '" & pass & "', '" & ID & "', " & GameID & ")"
            Try
                'tbConsole.WriteLine(strSQL)
                dbCommand = New OleDbCommand(strSQL, dbConnection)
                dbCommand.ExecuteNonQuery()
            Catch ex As Exception
                Return False
            Finally
                dbCommand = Nothing
            End Try
            Return True
        End Function

        'deletes
        Public Function DeletePlayer(Optional ByVal UserName As String = "ALL") As Boolean
            If dbConnection Is Nothing Then
                dbConnection = New OleDbConnection(dbConnStr)
                dbConnection.Open()
            End If
            Dim dbCommand As OleDbCommand
            Dim strSQL As String
            If UserName = "ALL" Then
                strSQL = "DELETE * FROM ActivePlayers"
            Else
                strSQL = "DELETE FROM ActivePlayers WHERE [user] = '" & UserName & "'"
            End If

            Try
                dbCommand = New OleDbCommand(strSQL, dbConnection)
                dbCommand.ExecuteNonQuery()
                dbCommand.Dispose()
                Return True
            Catch ex As Exception


                Return False

            Finally

                dbCommand = Nothing
            End Try
            Return True
        End Function


        'Loads
        Public Function LoadAllPlayers(ByRef AllScores As List(Of KniffelPlayerInfo), Optional ByVal GameID As Integer = 0) As Boolean
            Dim strSQL As String
            If GameID = 0 Then
                strSQL = "SELECT * FROM ActivePlayers"
            Else
                strSQL = "SELECT * FROM ActivePlayers WHERE [Game] = " & GameID
            End If
            Dim dbAdapter As OleDbDataAdapter = New OleDbDataAdapter(strSQL, dbConnection)
            Dim DataSet As DataSet = New DataSet("players")
            dbAdapter.Fill(DataSet)
            If DataSet.Tables(0).Rows.Count = 0 Then
                DataSet.Dispose()
                DataSet = Nothing
                dbAdapter.Dispose()
                dbAdapter = Nothing
                Return False
            End If
            AllScores = New List(Of KniffelPlayerInfo)
            Dim dbRow As DataRow

            For iRCount As Integer = 0 To DataSet.Tables(0).Rows.Count - 1
                Dim score As New KniffelPlayerInfo
                dbRow = DataSet.Tables(0).Rows(iRCount)
                score.Name = (dbRow("user")).ToString
                score.Password = dbRow("pass").ToString
                score.SessionId = dbRow("ID").ToString
                score.GameId = CInt(dbRow("GameID"))


                AllScores.Add(score)
            Next
            dbRow = Nothing
            DataSet.Dispose()
            DataSet = Nothing
            dbAdapter.Dispose()
            dbAdapter = Nothing
            Return True
        End Function

        'Updates
        Public Function UpdatePlayer(ByVal user As String, ByVal GameID As Integer) As Boolean
            Dim dbCommand As OleDbCommand
            Dim strSQL As String = "UPDATE ActivePlayers SET [Game] = " & GameID & " WHERE user = '" & user & "'"
            Try
                dbCommand = New OleDbCommand(strSQL, dbConnection)
                dbCommand.ExecuteNonQuery()
            Catch ex As Exception
                Return False
            Finally
                dbCommand = Nothing
            End Try
            Return True
        End Function
        Public Sub Close()
            dbConnection.Close()
            dbConnection.Dispose()
        End Sub

    End Class

    Public Class KniffelOnlinePlayerProvider
        Public Function GetItem() As List(Of KniffelPlayerInfo)
            Dim loi As New List(Of KniffelPlayerInfo)

            Dim kdbp As New KniffelGameOleDbProvider
            kdbp.LoadAllPlayers(loi)
            If loi.Count = 0 Then
                Dim kp As New KniffelPlayerInfo
                kp.Name = "Никого нет :("
                loi.Add(kp)
            End If
            'loi.Sort()
            Return loi
        End Function
    End Class
End Namespace