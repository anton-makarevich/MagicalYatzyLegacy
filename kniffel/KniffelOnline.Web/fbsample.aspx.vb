Option Strict Off
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Diagnostics.CodeAnalysis
Imports System.Diagnostics.Contracts
Imports System.Linq
Imports System.Web
Imports Facebook
Imports Facebook.Web

Partial Class fbsampleclass
    Inherits CanvasPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not authorizer.IsAuthorized()) Then
            Dim params As New Collections.Generic.Dictionary(Of String, Object)
            params.Add("req_perms", "publish_stream")
            params.Add("next", "http://kniffel.sanet.by/fbsample.aspx")
            Dim authurl As Uri = authorizer.GetLoginUrl(params)
            CanvasRedirect(authurl.ToString())

        Else

            LoggedIn()
        End If

    End Sub
    Private Sub LoggedIn()
        'lblNamee.Text = fbApp.AccessToken
        'Dim myInfo As Object = fbApp.Get("me")
        'lblName.Text 
        'pnlHello.Visible = True
        'lblNamee.Text = myInfo.name
    End Sub
End Class
