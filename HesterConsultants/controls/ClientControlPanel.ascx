<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientControlPanel.ascx.cs" Inherits="HesterConsultants.clients.controls.ClientControlPanel" %>

<asp:Panel runat="server" ID="clientControlPanel" CssClass="clientControlPanel">
    <h3>Client Tools</h3>
    <div class="text"><a href="<%=Settings.Default.ClientHomeUrl %>">Home</a></div>
    <div class="text"><a href="<%=Settings.Default.NewJobUrl %>">New Work Request</a></div>
    <div class="clientControlPanelHr"></div>
    <div class="text">Search Jobs<br />(job #s, instructions, files):</div>
    <div style="margin-top: 5px; line-height: 1.6em;"><asp:TextBox runat="server" ID="txtSearch" CssClass="searchText" /><br /><asp:Button runat="server" ID="btnSearch" Text="Search" PostBackUrl="" CausesValidation="false" CssClass="searchButton" /></div><%-- postbackurl set in code --%>
    <div class="clientControlPanelHr"></div>
    <div class="text"><a href="<%=Settings.Default.ClientAccountUrl %>">My Account</a></div>
    <div class="text"><a href="<%=Settings.Default.ClientProfileUrl %>">My Profile</a></div>
    <div class="text"><a href="<%=Settings.Default.ChangePasswordUrl %>">Change Password</a></div>
    <div class="text"><a href="<%=Settings.Default.ContactUrl %>">Feedback</a></div>
    <div class="clientControlPanelHr"></div>
    <div class="text"><a href="<%=Settings.Default.LogoutUrl %>">Log Out</a></div>

    <div class="marginTop10px">
        <span id="siteseal">
            <script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=6UDaCOHTMsySu0hrL2cCrlljpt7pxjo70cN114YfIrLKi88POV7ms"></script>
        </span>
    </div>
</asp:Panel>

