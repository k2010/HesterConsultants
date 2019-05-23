<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.admin.Default" Trace="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Administration</title>
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
                <h1>Administration</h1>

                <%-- open/recent jobs ----------------------------------------------------%>
                <asp:Panel runat="server" ID="pnlOpenRecentJobs">
                    <h2>Current Work Requests</h2>
                    <asp:ListView runat="server" 
                        ID="lvJobs" 
                        DataSourceID="odsOpenRecentJobs"
                        ItemPlaceholderID="phOpenRecentJobs" onsorting="lvJobs_Sorting">
                        
                        <LayoutTemplate>
                            <table class="openJobs">
                                <thead>
                                    <th><asp:LinkButton runat="server" ID="hlJobNo" CommandName="Sort" CommandArgument="JobId" title="Click to Sort">Job&nbsp;No.</asp:LinkButton></th>
                                    <th><asp:LinkButton runat="server" ID="hlDateDue" CommandName="Sort" CommandArgument="DateDue" title="Click to Sort">Due</asp:LinkButton></th>
                                    <th><asp:LinkButton runat="server" ID="hlClient" CommandName="Sort" CommandArgument="Client.LastName" title="Click to Sort">Client</asp:LinkButton></th>
                                    <th>Job Type</th>
                                    <th>Instructions</th><%-- use css ellipsis thing --%>
                                    <th><asp:LinkButton runat="server" ID="LinkButton1" CommandName="Sort" CommandArgument="JobStatus.Name" title="Click to Sort">Status</asp:LinkButton></th>
                                    <th></th><%-- cancel, modify, send note, etc. (can't modify while in progress) --%>
                                </thead>
                                <tbody runat="server" ID="tbOpenRecentJobs"><asp:PlaceHolder runat="server" ID="phOpenRecentJobs" /></tbody>
                            </table>
                        </LayoutTemplate>
                        
                        <ItemTemplate>
                            <tr class='<%# Eval("JobStatus.Name").ToString().Replace(" ", "").Insert(0, "jobStatus") %>'>
                                <td><%# Eval("JobId") %></td>
                                <td><%# HesterConsultants.admin.AdminUtils.StandardDateFormat(this.DateForEmployee((DateTime)Eval("DateDue")), true) %></td>
                                <td title='<%# Eval("Client.FirstName") + " " + Eval("Client.LastName") %>'><%# Eval("Client.LastName") %></td>
                                <td><%# Eval("JobType.Name") %></td>
                                <td><%# this.EllipsisText(Eval("Instructions").ToString()) %></td>
                                <td><%# Eval("JobStatus.Name") %></td>
                                <td><asp:HyperLink runat="server" ID="hlInspectJob" NavigateUrl='<%# Eval("JobId", "/admin/JobAdmin.aspx?jobId={0}") %>'>Inspect</asp:HyperLink>
                                <br />
                                <asp:HyperLink runat="server" ID="hlWorkOnJob" NavigateUrl='<%# Eval("JobId", "/employees/Work.aspx?jobId={0}") %>'>Work</asp:HyperLink>
                                <br />
                                <asp:HyperLink runat="server" ID="hlClientAlert" NavigateUrl='<%# Eval("JobId", "/admin/ClientMessage.aspx?jobId={0}") %>'>Message</asp:HyperLink></td>
                            </tr>
                        </ItemTemplate>

                        <EmptyDataTemplate>
                                <div style="margin-bottom: 20px;">(No current work requests.)</div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </asp:Panel>
            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

<asp:ObjectDataSource 
    runat="server" 
    ID="odsOpenRecentJobs"
    DataObjectTypeName="HesterConsultants.AppCode.Entities.Job"
    TypeName="HesterConsultants.AppCode.CacheLayer"
    SelectMethod="RecentJobsForAdmin"
    SortParameterName="sortExpression">
</asp:ObjectDataSource>

</body>
</html>
