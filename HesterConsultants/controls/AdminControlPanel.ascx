<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminControlPanel.ascx.cs" Inherits="HesterConsultants.controls.AdminControlPanel" %>

<asp:Panel runat="server" ID="adminControlPanel" CssClass="adminControlPanel clientControlPanel">
    <h3>Admin Tools</h3>
    <div class="text"><a href="<%=Settings.Default.AdminHomeUrl %>">Home</a></div>
    <div class="text"><a href="<%=Settings.Default.AdminReportsUrl %>">Reports</a></div>
    <div class="clientControlPanelHr"></div>
    <div class="text">Search Jobs<br />(job #s, instructions, files):</div>
    <div style="margin-top: 5px; line-height: 1.6em;"><asp:TextBox runat="server" ID="txtSearch" CssClass="searchText" /><br /><asp:Button runat="server" ID="btnSearch" Text="Search" PostBackUrl="" CausesValidation="false" CssClass="searchButton" /></div><%-- postbackurl set in code --%>
    <div class="clientControlPanelHr"></div>
    <div class="text"><a href="/admin/Cache.aspx">Cache</a></div>
    <div class="text"><a href="/admin/Clients.aspx">Clients</a></div>
    <div class="text"><a href="/admin/AuthUsers.aspx">Users</a></div>
    <div class="text"><a href="/admin/Lists.aspx">Lists</asp:Hyperlink></div>
    <div class="text"><a href="/admin/FileManagement.aspx">File Management<br />(take old files off server)</a></div>
    <div class="text"><a href="/admin/Utilities.aspx">Utilities</a></div>
    <div class="clientControlPanelHr"></div>
    <div class="text"><a href="<%=Settings.Default.LogoutUrl %>">Log Out</a></div>
</asp:Panel>
