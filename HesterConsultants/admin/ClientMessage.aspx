<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientMessage.aspx.cs" Inherits="HesterConsultants.admin.ClientMessage" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Send Client Message</title>
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

                $("#<%=this.ddMessageTypes.ClientID %>").change(function ()
                {
                    $.ajax(
                    {
                        url: "./ClientMessageTemplates.aspx",
                        data:
                        {
                            jobId: "<%=this.strJobId %>",
                            messageTypeId: $("#<%=this.ddMessageTypes.ClientID %>").val()
                        },
                        success: function (data, textStatus, jqXHR)
                        {
                            $("#<%=this.txtSubject.ClientID %>").val(data[0].subject);
                            $("#<%=this.txtMessage.ClientID %>").val(data[0].message);
                        }
                    });
                });
            });
    </script>
</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <hca:AdminControlPanel runat="server" ID="acp1" />
            <div id="mainbodyClients">
                <h1>Send Client Message - Job No. <asp:Label runat="server" ID="lblHeadingJobNumber" /></h1>

                <asp:Panel runat="server" ID="pnlEmailForm">
                    <div class="text marginBottom20px"><span style="position: relative; left: 82px;">Message type:</span><asp:DropDownList runat="server" ID="ddMessageTypes" 
                            AppendDataBoundItems="true" 
                            style="position: relative; left: 85px;" /></div>

                    <div class="pnlBtnSendMessage"><asp:Button ID="btnSend" runat="server" CssClass="bigButton" Text="Send" onclick="btnSend_Click"></asp:Button></div>

	                <table id="tblContact"><!---->
		                <tr>
			                <td style="text-align: right; white-space: nowrap;">To:</td>
			                <td><asp:TextBox id="txtEmail" runat="server" Width="350"></asp:TextBox></td>
		                </tr>
		                <tr>
			                <td class="tableLabelRightAligned">Subject:</td>
			                <td><asp:TextBox id="txtSubject" runat="server" Width="350"></asp:TextBox></td>
		                </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Message:</td>
                            <td><asp:TextBox id="txtMessage" runat="server" TextMode="MultiLine" Width="350" Rows="10" /></td>
                        </tr>
                    </table>

                    <fieldset>
                        <legend>Job Info</legend>
                        <table>
                            <tr>
                                <td class="tableLabelRightAligned">Status:</td>
                                <td><asp:Label runat="server" ID="lblStatus" /></td>
                            </tr>
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
                </asp:Panel>

                <%-- success note --%>
                <asp:Panel runat="server" ID="pnlSuccess" Visible="false"></asp:Panel>

            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>
</body>
</html>
