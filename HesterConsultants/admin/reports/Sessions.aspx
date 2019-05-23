<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sessions.aspx.cs" Inherits="HesterConsultants.admin.reports.Sessions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Sessions Report</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>
</head>
<body>
    <div id="container">
    <form id="form1" runat="server">
	<hc:HcHeader runat="server" id="header1" />
	<asp:Panel id="reports" runat="server">
		<p class="pageHeading">Sessions Report:</p>
	    <asp:GridView ID="gvSessions" runat="server" 
        AutoGenerateColumns="False" DataKeyNames="SessionID" 
        DataSourceID="SqlDataSource1" AutoGenerateEditButton="false" BorderWidth="0"
        AllowPaging="true" AlternatingRowStyle-CssClass="alternatingRow" 
        HeaderStyle-CssClass="headingRow" PageSize="25" CssClass="reports" 
            GridLines="None" CellSpacing="-1">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="SessionID" DataNavigateUrlFormatString="./hits.aspx?sessionId={0}" DataTextField="SessionID" HeaderText="Session" ItemStyle-HorizontalAlign="Right" />
                <asp:HyperLinkField DataNavigateUrlFields="VisitorID" DataNavigateUrlFormatString="./sessions.aspx?visitorId={0}" DataTextField="VisitorID" HeaderText="Visitor" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="IP">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlWhois" NavigateUrl='<%# this.WhoisUrl(Eval("IP")) %>' Text='<%# Eval("IP") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Browser" HeaderText="Browser" />
                <asp:BoundField DataField="Platform" HeaderText="Platform" />
                <asp:TemplateField HeaderText="Screen" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblScreenSize" Text='<%# this.ScreenSize(Eval("ScreenWidth"), Eval("ScreenHeight")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Color" HeaderText="Color" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="Start (local)" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStartTimeLocal" Text='<%# HesterConsultants.AppCode.SiteUtils.UtcToClientTime(Eval("Start")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End (local)" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEndTimeLocal" Text='<%# HesterConsultants.AppCode.SiteUtils.UtcToClientTime(Eval("End")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Zone" HeaderText="Zone" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="UserAgent" HeaderText="UserAgent" />
            </Columns>
            <PagerSettings Mode="NextPreviousFirstLast" NextPageText="Next" PreviousPageText="Previous" LastPageText="Last" FirstPageText="First" />
        </asp:GridView>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:HesterConsultants.Properties.Settings.ConnString_Visitors %>" 
            ProviderName="<%$ ConnectionStrings:HesterConsultants.Properties.Settings.ConnString_Visitors.ProviderName %>" 
            SelectCommandType="StoredProcedure" OldValuesParameterFormatString="{0}" >
            
            <SelectParameters>
                <asp:QueryStringParameter Name="VisitorId" Type="Int32" QueryStringField="visitorid" />
            </SelectParameters>
        </asp:SqlDataSource>

	</asp:Panel>
	<hc:HcFooter runat="server" ID="footer1" />
    </form>
    </div>
</body>
</html>
