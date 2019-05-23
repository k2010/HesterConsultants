<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobSearch.aspx.cs" Inherits="HesterConsultants.clients.JobSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Search Jobs</title>
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

                $(".bigLogin").click(function ()
                {
                    window.location = $(this).find("a").attr("href");
                    return false;
                });

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

                <h1>Search Results</h1>
                                
                <%-- search results ----------------------------------------------------%>
                <asp:Panel runat="server" ID="pnlFoundJobs">
                    <%--<h2>Jobs/Work Requests</h2>--%>
                    <asp:ListView runat="server" 
                        ID="lvFoundJobs" 
                        DataSourceID="odsFoundJobs"
                        ItemPlaceholderID="phFoundJobs" onsorting="lvFoundJobs_Sorting">
                        
                        <LayoutTemplate>
                            <table class="openJobs">
                                <thead>
                                    <th style="width: 10%;"><asp:LinkButton runat="server" ID="hlJobNo" CommandName="Sort" CommandArgument="JobId" title="Click to Sort">Job&nbsp;No.</asp:LinkButton></th>
                                    <th style="width: 10%;"><asp:LinkButton runat="server" ID="hlDateDue" CommandName="Sort" CommandArgument="DateDue" title="Click to Sort">Due</asp:LinkButton></th>
                                    <th style="width: 10%;">Job Type</th>
                                    <th style="width: 44%;">Instructions/Files</th><%-- use css ellipsis thing --%>
                                    <th style="width: 13%;"><asp:LinkButton runat="server" ID="hlStatus" CommandName="Sort" CommandArgument="JobStatus.JobStatusId" title="Click to Sort">Status</asp:LinkButton></th>
                                    <th style="width: 13%;"></th><%-- cancel, modify, send note, etc. (can't modify while in progress) --%>
                                </thead>
                                <tbody runat="server" ID="tbFoundJobs"><asp:PlaceHolder runat="server" ID="phFoundJobs" /></tbody>
                            </table>
                            <p class="marginTop30px">If you would like to make changes to any jobs, please use our <a href="<%=Settings.Default.ContactUrl %>">contact form</a>.</p>
                        </LayoutTemplate>
                        
                        <ItemTemplate>
                            <tr class='<%# Eval("JobStatus.Name").ToString().Replace(" ", "").Insert(0, "jobStatus") %>'>
                                <td><%# this.JobNumber((HesterConsultants.AppCode.Entities.Job)GetDataItem()) %></td>
                                <td><%# HesterConsultants.admin.AdminUtils.StandardDateFormat(this.DateForClient((DateTime)Eval("DateDue")), true) %></td>
                                <td><%# Eval("JobType.Name") %></td>
                                <td><%# this.EllipsisText(Eval("Instructions").ToString()) + this.FileList((HesterConsultants.AppCode.Entities.Job)GetDataItem())%></td>
                                <td><%# Eval("JobStatus.Name") %></td>
                                <td><asp:HyperLink runat="server" ID="hlJob" NavigateUrl='<%# Eval("JobId", "./Job.aspx?jobId={0}") %>' Text="Inspect" /></td>
                            </tr>
                        </ItemTemplate>

                        <EmptyDataTemplate>
                                <div style="margin-bottom: 20px;">(No matching jobs.)</div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </asp:Panel>
            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsFoundJobs"
        DataObjectTypeName="HesterConsultants.AppCode.Entities.Job"
        TypeName="HesterConsultants.AppCode.Entities.Job"
        SelectMethod="JobsFromSearch"
        SortParameterName="sortExpression" 
        onselecting="odsFoundJobs_Selecting">

    </asp:ObjectDataSource>

</body>
</html>
