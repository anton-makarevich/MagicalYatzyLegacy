<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PlayKniffel.aspx.vb" Inherits="PlayKniffel" Title="Книффель online - многопользовательская сетевая онлайн игра в кости" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="page">
		<div id="page-bgtop">
			<div id="page-bgbtm">
            <%--Right Panel--%>
				<div id="sidebar1" class="sidebar">
					<ul>
                    <li>
							<h2>Microsoft Silverlight</h2>
                            <img class="left" src="images/sllogo.png" alt="Microsoft Silverlight" style="border-style:none"/>
                            <p>Игра Kniffel Online работает на платформе Microsoft Silverlight</p>
							<br/>
                            <ul>
								<li><a target="_blank" href = "http://www.microsoft.com/rus/silverlight/">Что такое Silverlight</a></li>
								<li><a target="_blank" href = "http://www.microsoft.com/getsilverlight/Get-Started/Install/Default.aspx">Скачать Silverlight</a></li>
							</ul>
						</li>
						<li>
							<h2>Kniffel Online</h2>
							<ul>
								<li><a target="_blank" href = "http://sanet.by/help.aspx?docid=Kniffel">Правила</a></li>
                                <li><a target="_blank" href = "http://sanet.by/kniffel.aspx">Книга рекордов Книффеля</a></li>
                                <li><a target="_blank" href = "http://forum.sanet.by">Форум техподдержки</a></li>
                                
							</ul>
                             <p>На форуме - решение проблем с игрой, а также все последние новости и изменения</p>
						</li>
						<li>
							<h2>Рассказать друзьям</h2>
							<!-- AddThis Button BEGIN -->
<div class="addthis_toolbox addthis_default_style addthis_32x32_style">
<a class="addthis_button_twitter" title="Добавить в Twitter"></a>
<a class="addthis_button_facebook" title="Добавить в Facebook"></a>
<a class="addthis_button_vk" title="Добавить в Вконтакте"></a>
<a class="addthis_button_mymailru" title="Добавить в Мой Мир"></a>
<a class="addthis_button_email" title="Отправить по электронной почте"></a>
<a class="addthis_button_print" title="Распечатать"></a>
<a class="addthis_button_compact"></a>
</div>
<script type="text/javascript">    var addthis_config = { "data_track_clickback": true };</script>
<script type="text/javascript">    var addthis_config = { "ui_language": "ru" };</script>
<script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=sanetby"></script>
<!-- AddThis Button END -->
                            
						</li>
					</ul>
				</div>
				<!-- start content -->
				<div id="content">
					 <object  id="kniffelsl" data="data:application/x-silverlight-2," type="application/x-silverlight-2" 
                width="660px " height="720px">
		  <%
		      Dim orgSourceValue As String = "ClientBin/KniffelOnline.xap"
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
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Скачать Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object>
					
				</div>
				<!-- end content -->
				<!-- start sidebars -->
				<!-- end sidebars -->
				<div style="clear: both;">&nbsp;</div>
			</div>
        </div>
	</div>

   
</asp:Content>
