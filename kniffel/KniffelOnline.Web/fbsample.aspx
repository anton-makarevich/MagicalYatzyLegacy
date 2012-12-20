<%@ Page Title="Home Page" Language="VB" AutoEventWireup="false" CodeFile="fbsample.aspx.vb" Inherits="fbsampleclass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
   </head>
<body>
    <form id="Form1" runat="server">
    <div class="page">
        <div class="header">
            <div class="title">
                <h1>
                    My ASP.NET Application
                </h1>
            </div>
            
            
        </div>
        <div class="main">
            
        <h2>
            Hello
            <asp:Label ID="lblNamee" runat="server" />!
        </h2>
   
    
        <a href="fbsample.aspx">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" /><br />
        </a>
    
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="footer">
    </div>
    </form>
</body>
</html>