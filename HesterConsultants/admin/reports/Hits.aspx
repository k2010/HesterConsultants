<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hits.aspx.cs" Inherits="HesterConsultants.admin.reports.Hits" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Hits Report</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>
</head>
<body>
    <div id="container">
    <form id="form1" runat="server">
    <hc:HcHeader runat="server" ID="header1" />
	<asp:Panel id="reports" runat="server">
		<p class="pageHeading">Hits Detail: Session No. <% = sessionId.ToString() %></p>
	    <asp:GridView ID="gvHits" runat="server" 
        AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1" AutoGenerateEditButton="false" BorderWidth="0"
        AllowPaging="true" AlternatingRowStyle-CssClass="alternatingRow" 
        HeaderStyle-CssClass="headingRow" PageSize="50" CssClass="reports"
        GridLines="None" CellSpacing="-1">
            <Columns>
                <asp:BoundField DataField="PageURL" HeaderText="Page" />

                <asp:TemplateField HeaderText="Start (local)" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStartTimeLocal" Text='<%# HesterConsultants.AppCode.SiteUtils.UtcToClientTime(Eval("HitDate")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="QueryString" HeaderText="Query&nbsp;String" />
                <asp:TemplateField HeaderText="Referrer">
                    <ItemTemplate>
                        <%# Eval("ReferrerUrl") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NextPreviousFirstLast" NextPageText="Next&gt;" PreviousPageText="&lt;Previous" LastPageText="Last&gt;&gt;" FirstPageText="&lt;&lt;First" />
        </asp:GridView>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:HesterConsultants.Properties.Settings.ConnString_Visitors %>" 
            ProviderName="<%$ ConnectionStrings:HesterConsultants.Properties.Settings.ConnString_Visitors.ProviderName %>" 
            SelectCommandType="StoredProcedure" 
            SelectCommand="HitsForSession" OldValuesParameterFormatString="{0}">
            
            <SelectParameters>
                <asp:QueryStringParameter Name="SessionId" Type="Int32" QueryStringField="sessionId" />
            </SelectParameters>
        </asp:SqlDataSource>
	</asp:Panel>
    
    <hc:HcFooter runat="server" ID="footer1" />
    </form>
    </div>
</body>
</html>
