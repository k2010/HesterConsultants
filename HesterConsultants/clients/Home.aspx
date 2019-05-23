<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="HesterConsultants.clients.Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Client Home</title>
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

                $(".bigLogin").mouseover(function ()
                {
                    $(this).css("background-image", "url('/style/hcblue/images/hc-blue-button-bg.png')");
                });

                $(".bigLogin").mouseout(function ()
                {
                    $(this).css("background-image", "url('/style/hcblue/images/hc-gray-button-bg.png')");
                });

                $(".bigLogin").click(function (e)
                {
                    e.preventDefault();
                    window.location = $(this).find("a").attr("href");
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

                <h1>Client Home</h1>
                <%-- newly completed jobs to be picked up --------------------------------%>
                <asp:Panel runat="server" ID="pnlNewlyCompletedJobs">
                    <h2>Newly Completed Work Requests</h2>
                    <asp:ListView runat="server" 
                        ID="lvNewlyCompletedJobs" 
                        DataSourceID="odsNewlyCompletedJobs"
                        ItemPlaceholderID="phNewlyCompletedJobs">
                        
                        <LayoutTemplate>
                            <table class="openJobs">
                                <thead>
                                    <th style="width: 10%;">Job&nbsp;No.</th>
                                    <th style="width: 10%;">Due</th>
                                    <th style="width: 10%;">Job Type</th>
                                    <th style="width: 56%;">Instructions</th>
                                    <th style="width: 14%;"></th><%-- cancel, modify, send note, etc. (can't modify while in progress) --%>
                                </thead>
                                <tbody runat="server" ID="tbNewlyCompletedJobs"><asp:PlaceHolder runat="server" ID="phNewlyCompletedJobs" /></tbody>
                            </table>
                        </LayoutTemplate>
                        
                        <ItemTemplate>
                            <tr class='<%# Eval("JobStatus.Name").ToString().Replace(" ", "").Insert(0, "jobStatus") %>'>
                                <td ><%# Eval("JobId") %></td>
                                <td><%# HesterConsultants.admin.AdminUtils.StandardDateFormat(this.DateForClient((DateTime)Eval("DateDue")), true) %></td>
                                <td><%# Eval("JobType.Name") %></td>
                                <td><%# this.EllipsisText(Eval("Instructions").ToString()) %></td>
                                <td><asp:HyperLink runat="server" ID="hlJob" NavigateUrl='<%# Eval("JobId", "./Job.aspx?jobId={0}") %>' Text="Pick up" /></td>
                            </tr>
                            
                            
                        </ItemTemplate>
                    </asp:ListView>

                </asp:Panel>
                                
                <%-- open/recent jobs ----------------------------------------------------%>
                <asp:Panel runat="server" ID="pnlOpenRecentJobs">
                    <h2>Current/Recent Work Requests</h2>
                    <asp:ListView runat="server" 
                        ID="lvOpenRecentJobs" 
                        DataSourceID="odsOpenRecentJobs"
                        ItemPlaceholderID="phOpenRecentJobs" onsorting="lvOpenRecentJobs_Sorting">
                        
                        <LayoutTemplate>
                            <table class="openJobs">
                                <thead>
                                    <th style="width: 10%;"><asp:LinkButton runat="server" ID="hlJobNo" CommandName="Sort" CommandArgument="JobId" title="Click to Sort">Job&nbsp;No.</asp:LinkButton></th>
                                    <th style="width: 10%;"><asp:LinkButton runat="server" ID="hlDateDue" CommandName="Sort" CommandArgument="DateDue" title="Click to Sort">Due</asp:LinkButton></th>
                                    <th style="width: 16%;">Job Type</th>
                                    <th style="width: 38%;">Instructions</th>
                                    <th style="width: 13%;"><asp:LinkButton runat="server" ID="hlStatus" CommandName="Sort" CommandArgument="JobStatus.JobStatusId" title="Click to Sort">Status</asp:LinkButton></th>
                                    <th style="width: 13%;"></th><%-- cancel, modify, send note, etc. (can't modify while in progress) --%>
                                </thead>
                                <tbody runat="server" ID="tbOpenRecentJobs"><asp:PlaceHolder runat="server" ID="phOpenRecentJobs" /></tbody>
                            </table>
                            <div class="text marginTop30px">If you would like to make changes to any jobs, please use our <a href="<%=Settings.Default.ContactUrl %>">contact form</a>.</div>
                        </LayoutTemplate>
                        
                        <ItemTemplate>
                            <tr class='<%# Eval("JobStatus.Name").ToString().Replace(" ", "").Insert(0, "jobStatus") %>'>
                                <td><%# Eval("JobId") %></td>
                                <td><%# HesterConsultants.admin.AdminUtils.StandardDateFormat(this.DateForClient((DateTime)Eval("DateDue")), true) %></td>
                                <td><%# Eval("JobType.Name") %></td>
                                <td><%# this.EllipsisText(Eval("Instructions").ToString()) %></td>
                                <td><%# Eval("JobStatus.Name") %></td>
                                <td><asp:HyperLink runat="server" ID="hlJob" NavigateUrl='<%# Eval("JobId", "./Job.aspx?jobId={0}") %>' Text="Inspect" /></td>
                            </tr>
                        </ItemTemplate>

                        <EmptyDataTemplate>
                                <div style="margin-bottom: 20px;">(No current work requests.)</div>
                                <div class="bigLogin"><span class="innerBigLogin"><a href="<%=Settings.Default.NewJobUrl %>">New Work Request</a></span></div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </asp:Panel>

            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsNewlyCompletedJobs"
        DataObjectTypeName="HesterConsultants.AppCode.Entities.Job"
        TypeName="HesterConsultants.AppCode.CacheLayer"
        SelectMethod="NewlyCompletedJobsForClient" 
        onselected="odsNewlyCompletedJobs_Selected" 
        onselecting="odsNewlyCompletedJobs_Selecting">

        <SelectParameters>
            <asp:Parameter Name="client" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsOpenRecentJobs"
        DataObjectTypeName="HesterConsultants.AppCode.Entities.Job"
        TypeName="HesterConsultants.AppCode.CacheLayer"
        SelectMethod="RecentJobsForClient"
        SortParameterName="sortExpression"  
        onselecting="odsOpenRecentJobs_Selecting">

        <SelectParameters>
            <asp:Parameter Name="client" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>

</body>
</html>

