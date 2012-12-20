Imports System.IO
Imports System.Web.UI.WebControls

Partial Class _Default
    Inherits System.Web.UI.Page
    Protected Sub GridViewOnlinePlayers_RowCreated(ByVal [source] As Object, ByVal e As GridViewRowEventArgs)
        If e.Row Is Nothing Then
            Return
        End If
        ' display if 'Visible' = true
        If e.Row.RowType = DataControlRowType.DataRow And Not (e.Row.DataItem Is Nothing) Then
            'Dim visible As Boolean = CBool(DataBinder.Eval(e.Row.DataItem, "Visible"))
            e.Row.Visible = True
        End If

        ' display image if a url is specifid

    End Sub
End Class

