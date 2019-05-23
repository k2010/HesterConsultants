<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="HesterConsultants.clients.Account" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Client Account</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/hcJq.js"></script>
    <script type="text/javascript" src="/scripts/hc.js"></script>
    <script type="text/javascript">
        $(document).ready(
            function ()
            {
                hesterConsultants.setupClientPanel();
            });
    </script>
    <% = SiteUtils.CheckForClientValsScript(this) %>
</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <hcc:ClientControlPanel runat="server" ID="ccp1" />
            <div id="mainbodyClients">

                <h1>Client Account - <asp:Label runat="server" ID="lblClientName" /></h1>

                <h2>Currently Due</h2>
                <asp:PlaceHolder runat="server" ID="phInvoices" />

                <%-- current invoice --------------------------------%>
<%--                <asp:Panel runat="server" ID="pnlCurrentInvoice">
                    <h2>Current Invoice</h2>
                    <asp:ListView runat="server" 
                        ID="lvCurrentInvoice" 
                        DataSourceID="odsCurrentInvoice"
                        ItemPlaceholderID="phCurrentInvoice">
                        
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="phCurrentInvoice" />
                        </LayoutTemplate>
                        
                        <ItemTemplate>
                            <div><%# Eval("InvoiceId") %></div>
                            <div><%# Eval("AmountDue") %></div>
                            <div><%# Eval("DateDue") %></div>
                        </ItemTemplate>

                        <EmptyDataTemplate>
                            <div>[No current invoice.]</div>
                        </EmptyDataTemplate>
                    </asp:ListView>

                </asp:Panel>--%>

                <asp:PlaceHolder runat="server" ID="phNote" />
                                
            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsCurrentInvoice"
        DataObjectTypeName="HesterConsultants.AppCode.Entities.Invoice"
        TypeName="HesterConsultants.AppCode.Entities.Invoices"
        SelectMethod="CurrentInvoiceForClient" 
        onselecting="odsCurrentInvoice_Selecting" >

        <SelectParameters>
            <asp:Parameter Name="client" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>


</body>
</html>
