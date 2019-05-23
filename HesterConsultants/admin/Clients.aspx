<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clients.aspx.cs" Inherits="HesterConsultants.admin.Clients" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Administer Clients</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <link rel="stylesheet" type="text/css" href="/style/hcblue/jquery-ui-1.8.11.custom.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->

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
</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <hca:AdminControlPanel runat="server" ID="acp1" />
            <div id="mainbodyClients">
                <h1>Clients</h1>
                <asp:Panel runat="server" ID="pnlClients">
                    <asp:ListView runat="server" ID="lvClients" DataSourceID="odsClientExes" 
                        ItemPlaceholderID="phClients" onsorting="lvClients_Sorting">
                        <LayoutTemplate>
                            <table class="openJobs">
                                <thead>
                                    <tr>
                                        <th><asp:LinkButton runat="server" ID="hlName" CommandName="Sort" CommandArgument="Client.LastName" title="Click to Sort">Name</asp:LinkButton></th>
                                        <th><asp:LinkButton runat="server" ID="hlAmount" CommandName="Sort" CommandArgument="AmountDue" title="Click to Sort">Amount Due</asp:LinkButton></th>
                                        <th><asp:LinkButton runat="server" ID="hlNumJobs" CommandName="Sort" CommandArgument="NumberOfJobs" title="Click to Sort"># of Jobs</asp:LinkButton></th>
                                        <th>Manage</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="phClients" />
                                </tbody>
                            </table>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Client.FirstName") + " " + Eval("Client.LastName") %></td>
                                <td><%# ((decimal)Eval("AmountDue")).ToString("c") %></td>
                                <td><%# Eval("NumberOfJobs") %></td>
                                <td><asp:HyperLink runat="server" ID="hlManageClient" NavigateUrl='<%# Eval("Client.ClientId", "/admin/ClientAdmin.aspx?clientId={0}") %>' Text="Manage" /></td>
                            </tr>
                        </ItemTemplate>
            
                    </asp:ListView>
                </asp:Panel>
            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsClientExes" 
        TypeName="HesterConsultants.AppCode.Entities.ClientEx" 
        DataObjectTypeName="HesterConsultants.AppCode.Entities.ClientEx" 
        SelectMethod="AllClientExes"
        SortParameterName="sortExpression">
    </asp:ObjectDataSource>
</body>
</html>
