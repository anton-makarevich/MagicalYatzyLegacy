<%@ Page Language="vb" AutoEventWireup="false" CodeFile="VKontakte.aspx.vb" Inherits="VkontaktePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>KniffelOnline</title>
    <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }
    </style>
    <script src="http://vkontakte.ru/js/api/xd_connection.js?2" type="text/javascript"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</head>
<body>
<script src="http://vkontakte.ru/js/api/openapi.js" type="text/javascript"></script>
<script type="text/javascript">
    VK.init({
        apiId: 2121738
    });
</script>

<p>Извините, приложение временно отключено из-за проблем с сервером. Мы работаем, чтобы исправить эти проблемы. 
Пока можно поиграть <a target="_blank" href="http://apps.microsoft.com/windows/app/sanet-dice-poker/5b0f9106-65a8-49ca-b1f0-641c54a7e3ef">в порт для Windows 8</a>.
 Все результаты игр сохранены <a target="_blank" href="http://sanet.by/kniffel">и по-прежнему доступны в книге рекордов</a>.</p>
   <%-- <form id="form1" runat="server" style="height:100%">
    <div id="silverlightControlHost">
        <object id="kniffelsl" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <%
		      Dim orgSourceValue As String = "KniffelOnline.xap"
Dim param As String
If (System.Diagnostics.Debugger.IsAttached) Then
param = "<param name=""source"" value=""" + orgSourceValue + """ />"
Else

Dim xappath As String = HttpContext.Current.Server.MapPath("") + "\" + orgSourceValue
Dim xapCreationDate As DateTime = System.IO.File.GetLastWriteTime(xappath)
param = "<param name=""source"" value=""" + orgSourceValue + "?ignore=" + xapCreationDate.ToString() + """ />"
End If

Response.Write(param)

%>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="4.0.50826.0" />
		  <param name="autoUpgrade" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object></div>
    </form>--%>
</body>
</html>