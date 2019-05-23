<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Job.aspx.cs" Inherits="HesterConsultants.clients.Job" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/hcJq.js"></script>
    <script type="text/javascript" src="/scripts/hc.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>
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
            <hcc:ClientControlPanel runat="server" ID="ccp1" />
            <div id="mainbodyClients">
                <div class="pnlJobFormContainer">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 150px; vertical-align: middle;"><h1>Job No. <asp:Label runat="server" ID="lblHeadingJobNumber"></asp:Label></h1></td>
                            <td runat="server" id="tdStatus" style="padding-left: 10px; vertical-align: middle;"><asp:Panel runat="server" ID="pnlStatus" style="padding-left: 10px;"><h1 style="font-weight: normal; font-size: 1.3em; color: #666666;">&nbsp;Status: <asp:Label runat="server" ID="lblStatus" /></h1></asp:Panel></td>
                        </tr>
                    </table>

                    <%-- bad job --%>
                    <asp:Panel runat="server" ID="pnlBadJob" Visible="false" CssClass="errorMessage">
                        Sorry, the job number given does not appear to match any of your jobs.
                    </asp:Panel>

                    <%-- delivery notes --%>
                    <asp:Panel runat="server" ID="pnlDeliveryNotesContainer">
                        <fieldset>
                            <legend>Delivery Notes</legend>
                            <asp:Placeholder runat="server" ID="phDeliveryNotes" />
                        </fieldset>
                    </asp:Panel>

                    <%-- return files --%>
                    <asp:Panel runat="server" ID="pnlReturnFiles" Visible="false">
                        <fieldset>
                            <legend>Returned Files</legend>
                            <asp:Placeholder runat="server" ID="phReturnedFiles" />
                        </fieldset>
                    </asp:Panel>

                    <%-- fees --%>
                    <asp:Panel runat="server" ID="pnlFees" Visible="false">
                        <fieldset>
                            <legend>Fees</legend>
                            <asp:PlaceHolder runat="server" ID="phFees" />
                        </fieldset>
                    </asp:Panel>
                
                    <%-- work history --%>
                    <asp:Panel runat="server" ID="pnlJobSegments" Visible="false">
                        <fieldset>
                            <legend>Work History</legend>
                            <asp:PlaceHolder runat="server" ID="phJobSegments" />
                        </fieldset>
                    </asp:Panel>
                
                    <fieldset>
                        <legend>Job Info</legend>
                        <table>
                            <tr>
                                <td class="tableLabelRightAligned">Due:</td>
                                <td><asp:Label runat="server" ID="lblDateDue" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Submitted:</td>
                                <td><asp:Label runat="server" ID="lblDateSubmitted" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Billing reference:</td>
                                <td><asp:Label runat="server" ID="lblBillingRef" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Job type:</td>
                                <td><asp:Label runat="server" ID="lblJobType" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Options:</td>
                                <td><asp:Label runat="server" ID="lblOptions" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Instructions:</td>
                                <td><asp:Placeholder runat="server" ID="phInstructions" /></td>
                            </tr>
                        </table>

                        <%-- submitted files --%>
                        <asp:Panel runat="server" ID="pnlSubmittedFiles">
                            <fieldset>
                                <legend>Submitted Files</legend>
                                <asp:Placeholder runat="server" ID="phSubmittedFiles" />
                            </fieldset>
                        </asp:Panel>
                    </fieldset>

                    <asp:Panel runat="server" ID="pnlMakeChanges" class="marginTop30px"><p>If you would like to make changes or submit feedback regarding this job, please use our <asp:HyperLink runat="server" ID="hlContact">contact form</asp:HyperLink>.</p></asp:Panel>
            </div>
            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

<%--<asp:ObjectDataSource 
    runat="server" 
    ID="odsReturnFiles" 
    DataObjectTypeName="HesterConsultants.AppCode.Entities.JobFile" 
    TypeName="HesterConsultants.AppCode.Entities.JobFile" 
    SelectMethod="ReturnFilesForJob" onselecting="odsReturnFiles_Selecting" >
    
    <SelectParameters>
        <asp:Parameter Name="job" Type="Object" />
    </SelectParameters>
</asp:ObjectDataSource>--%>

</body>
</html>
