<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="dbtest.aspx.vb" Inherits="_dbtest" Title="Книффель online - многопользовательская сетевая онлайн игра в кости" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="page">
		<div id="page-bgtop">
			<div id="page-bgbtm">
            <%--Right Panel--%>
				<div id="sidebar1" class="sidebar">
					<ul>
						<li>
							<h2>Игра</h2>
							<ul>
								<li><a href="playkniffel.aspx">Книффель!</a></li>
								<li><a href="http://sanet.by/kniffel.aspx">Книга рекордов Книффеля!</a></li>
							</ul>
                            
						</li>
                        <li>
							<h2>Сейчас в игре</h2>
							<div id="sidebardata">
            <asp:Panel ID="AllItemsPanel" runat="server">
               
                <asp:ObjectDataSource ID="ObjectDataSourceOnlinePlayers" runat="server" SelectMethod="GetItem"
                    TypeName="Kniffel.Web.KniffelOnlinePlayerProvider">
                    
                 </asp:ObjectDataSource>
                <asp:GridView ID="GridViewOnlinePlayers" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSourceOnlinePlayers"
                    AllowPaging="True" PageSize="5" OnRowCreated="GridViewOnlinePlayers_RowCreated" BorderWidth="0px"
                    DataKeyNames="Name" Width="249px">
                    <Columns>
                        <asp:TemplateField >
                            <ItemTemplate>
                            
                                <%# Eval("Name")%>
                                                                
                            </ItemTemplate>
                            
                        </asp:TemplateField>
                        
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                </asp:GridView>
            </asp:Panel>
                            
                            </div>
						</li>
						<li>
							<h2>Разное</h2>
							<ul>
								<li><a href="http://sanet.by">Разработка программного обеспечения и web сайтов</a></li>
								
							</ul>
						</li>
					</ul>
				</div>
				<!-- start content -->
				<div id="content">
					<div class="post">
						<h1 class="title">Добро пожаловать</h1>
						
						<div class="entry">
							<p><strong>Книффель Online</strong> - это бесплатная многопользовательская сетевая игра в кости.</p>
							<p>В настоящее время можно <a href="playkniffel.aspx">играть в Книффель</a> (он же покер на костях, он же Yhatzeee) по правилам, доступным по <a href="http://sanet.by/help.aspx?docid=Kniffel">данной ссылке</a>. Игровой процесс происходит прямо в браузере - вам не нужно скачивать и устанавливать никаких программ за исключение Silverlight плагина, если он еще не установлен.</p>
							<p>Играть в книффель можно и в оффлайне. Для этого нужно <a href="http://sanet.by/freewares.aspx?docid=Kniffel">скачать и установить версию игры для Windows</a>. Правда многопользовательская игра в этом случае возможна только на одном компьютере. Зато можно сразиться с искуcственным "интеллектом"))</p>
                            <p>В будущем на этот сайт планируется добавить многопользовательские игры в кости и по другим правилам, так что заходите почаще!</p>
                            </div>
					</div>
					
				</div>
				<!-- end content -->
				<!-- start sidebars -->
				<!-- end sidebars -->
				<div style="clear: both;">&nbsp;</div>
			</div>
        </div>
	</div>

   
</asp:Content>
