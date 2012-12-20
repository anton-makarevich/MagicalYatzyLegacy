<%@ Page Language="vb" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="cbDefault1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml">
<head runat="server">
    <title>Kniffel Online</title>
</head>
<body>

<script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php"
type="text/javascript"></script>

<form id="form1" runat="server">
    <!-- start content -->


<div id="content">
					 <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" 
                width="660px " height="640px">
		  <param name="source" value="KniffelOnline.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="3.0.40818.0" />
		  <param name="autoUpgrade" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40818.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object>
					
</div>
    </form>

<script type="text/javascript">
FB.init("063bb70bda07b7db1492d3c59cb5260e", "xd_receiver.htm");
</script>
<script type="text/javascript">
FB.login(function(response) );

</script>
</body>
</html>
