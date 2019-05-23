<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobAdmin.aspx.cs" Inherits="HesterConsultants.admin.JobAdmin" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Administer Job</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <link rel="stylesheet" type="text/css" href="/style/hcblue/jquery-ui-1.8.11.custom.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->

    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui-1.8.11.custom.min.js"></script>
    <script type="text/javascript" src="/scripts/hcJq.js"></script>
    <script type="text/javascript" src="/scripts/hc.js"></script>
    <script type="text/javascript">
        $(document).ready(
            function ()
            {
                hesterConsultants.setupClientPanel();

                $("#<%=this.ddStatuses.ClientID %>").change(function ()
                {
                    var id = $(this).val();
                    $.ajax(
                    {
                        url: "./GetJobStatusIsClosed.aspx",
                        data: "jobStatusId=" + id,
                        success: function (data, textStatus, jqXHR)
                        {
                            if (data == "true")
                            {
                                //alert("going to ee/time");
                                $.ajax(
                                {
                                    url: "/employees/GetEmployeeDateTime.aspx",
                                    success: function (data, textStatus, jqXHR)
                                    {
                                        $("#<%=this.txtDateCompleted.ClientID %>").val(data);
                                    }
                                });
                            }
                        }
                    });

                });

                if ($("#<%=this.pnlUpdated.ClientID %>").get(0))
                {
                    $("#<%=this.pnlUpdated.ClientID %>").animate(
                    {
                        backgroundColor: "#0080ff",
                        color: "#ffffff",
                        duration: 2000
                    });
                }
            });
    </script>
</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
        <hc:HcHeader runat="server" ID="header1" />
        <hca:AdminControlPanel runat="server" ID="acp1" />
        <div id="mainbodyClients">
            <h1>Job No. <asp:Label runat="server" ID="lblHeadingJobNumber"></asp:Label></h1>
            
            <asp:Panel runat="server" ID="pnlSendMessage" CssClass="text"></asp:Panel>
            <asp:Panel runat="server" ID="pnlViewSegments" CssClass="text marginBottom20px"></asp:Panel>

            <asp:Panel runat="server" ID="pnlUpdated" CssClass="text marginBottom20px" style="padding-left: 5px;" Visible="false">Updated</asp:Panel>
            
            <div><asp:Button runat="server" ID="btnUpdateJob" Text="Update"
                            onclick="btnUpdateJob_Click" /></div>

            <table>
                <tr>
                    <td><%-- left side --%>
                        <table>
                            <tr>
                                <td class="tableLabelRightAligned">Client:</td>
                                <td><asp:Label runat="server" ID="lblClient" /></td>                
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Status:</td>
                                <td><asp:DropDownList runat="server" ID="ddStatuses" DataSourceID="odsJobStatuses" DataTextField="Name" DataValueField="JobStatusId" /><asp:PlaceHolder runat="server" ID="phCompletedDate" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Completed:</td>
                                <td><asp:TextBox runat="server" ID="txtDateCompleted" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Due:</td>
                                <td><asp:TextBox runat="server" ID="txtDateDue" /><asp:PlaceHolder runat="server" ID="phOrigDateDue" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Submitted:</td>
                                <td><asp:Label runat="server" ID="lblDateSubmitted" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Billing Reference:</td>
                                <td><asp:TextBox runat="server" ID="txtBillingRef" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Job Type:</td>
                                <td><asp:DropDownList runat="server" ID="ddJobTypes" DataSourceID="odsJobTypes" DataTextField="Name" DataValueField="JobTypeId" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Estimate:</td>
                                <td><asp:TextBox runat="server" ID="txtEstimate" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Final Charge:</td>
                                <td><asp:TextBox runat="server" ID="txtFinalCharge" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Taxes:</td>
                                <td><asp:TextBox runat="server" ID="txtTaxes" /></td>
                            </tr>
                        </table>
</td>
                    <td style="padding-left: 5px;">
                        <table>
                            <tr>
                                <td class="tableLabelRightAligned">Convert to:</td>
                                <td><asp:TextBox runat="server" ID="txtToApplication" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Formatted:</td>
                                <td><asp:CheckBox runat="server" ID="chkFormatted" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Proofed:</td>
                                <td><asp:CheckBox runat="server" ID="chkProofed" /></td>
                            </tr>
                        </table>
                        <div>Instructions:</div>
                        <asp:TextBox runat="server" ID="txtInstructions" TextMode="MultiLine" Rows="7" Columns="37" />
                        <div>Delivery Notes:</div>
                        <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" Rows="3" Columns="37" />
                        <div><asp:CheckBox runat="server" ID="chkArchive" Text="Archive" /></div>
                    </td>
                </tr>
            </table>
            

            <%-- return files --%>
            <asp:Panel runat="server" ID="pnlReturnFiles">
                <fieldset>
                    <legend>Return Files for Client</legend>
                    <asp:Placeholder runat="server" ID="phReturnFiles" />
                    <div id="returnedFileInputsContainer">
                        <div><asp:FileUpload runat="server" id="returnFile1" /></div>
                        <div><asp:FileUpload runat="server" id="returnFile2" /></div>
                        <div><asp:FileUpload runat="server" id="returnFile3" /></div>
                    </div>
                </fieldset>
            </asp:Panel>

            <%-- working files --%>
            <asp:Panel runat="server" ID="pnlWorkingFiles">
                <fieldset>
                    <legend>Working Files</legend>
                    <asp:Placeholder runat="server" ID="phWorkingFiles" />
                    <div id="workingFileInputsContainer">
                        <div><asp:FileUpload runat="server" id="workingFile1" /></div>
                        <div><asp:FileUpload runat="server" id="workingFile2" /></div>
                        <div><asp:FileUpload runat="server" id="workingFile3" /></div>
                    </div>
                </fieldset>
            </asp:Panel>
                
            <%-- submitted files --%>
            <asp:Panel runat="server" ID="pnlSubmittedFiles">
                <fieldset>
                    <legend>Client Submitted Files</legend>
                    <asp:Placeholder runat="server" ID="phSubmittedFiles" />
                    <div id="submittedFileInputsContainer">
                        <div><asp:FileUpload runat="server" id="submittedFile1" /></div>
                        <div><asp:FileUpload runat="server" id="submittedFile2" /></div>
                        <div><asp:FileUpload runat="server" id="submittedFile3" /></div>
                    </div>
                </fieldset>
            </asp:Panel>
    
        </div>
        <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

    <asp:ObjectDataSource
        runat="server"
        ID="odsJobStatuses"
        DataObjectTypeName="HesterConsultants.AppCode.Entities.JobStatus" 
        TypeName="HesterConsultants.AppCode.Entities.JobStatus" 
        SelectMethod="AllJobStatuses">
    </asp:ObjectDataSource>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsJobTypes" 
        DataObjectTypeName="HesterConsultants.AppCode.Entities.JobType" 
        TypeName="HesterConsultants.AppCode.Entities.JobType" 
        SelectMethod="AllJobTypes">
    </asp:ObjectDataSource>

</body>
</html>
