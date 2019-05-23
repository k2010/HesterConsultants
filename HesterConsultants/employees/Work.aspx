<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Work.aspx.cs" Inherits="HesterConsultants.employees.Work" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Work on Job</title>
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
                //alert($("#hlStopWork").length);

                if ($("#hlStopWork").length > 0)
                {
                    $("#hlStopWork").click(function (e)
                    {
                        e.preventDefault();

                        $("#<%=this.pnlStop.ClientID %>").slideDown(1000);

                        $.ajax(
                        {
                            url: "./GetEmployeeDateTime.aspx",
                            success: function (data, textStatus, jqXHR)
                            {
                                $("#<%=this.txtTimeStopped.ClientID %>").val(data);
                                var startDate = new Date("<%=this.txtTimeStarted.Text %>");
                                var stopDate = new Date($("#<%=this.txtTimeStopped.ClientID %>").val());
                                var difference = stopDate.getTime() - startDate.getTime();
                                var minutes = Math.round(difference / (1000 * 60));
                                $("#<%=this.txtHoursWorked.ClientID %>").val(hesterConsultants.roundToDecimals(minutes / 60, 1));
                            },
                            error: function (jqXHR, textStatus, errorThrown)
                            {
                                alert("Ajax error: " + textStatus + "\n\nTry again!");
                            }
                        });
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
                <h1>Job No. <asp:Label runat="server" ID="lblHeadingJobNumber" /> - Work</h1>
                <asp:Panel runat="server" ID="pnlViewJob" CssClass="text"></asp:Panel>
                <asp:Panel runat="server" ID="pnlJobInfo">
                    <%-- return files --%>
                    <asp:Panel runat="server" ID="pnlReturnFiles">
                        <fieldset>
                            <legend>Files to be Returned to Client</legend>
                            <asp:Placeholder runat="server" ID="phReturnFiles" />
                        </fieldset>
                    </asp:Panel>
                
                    <%-- working files --%>
                    <asp:Panel runat="server" ID="pnlWorkingFiles" Visible="false">
                        <fieldset>
                            <legend>Working Files</legend>
                            <asp:Placeholder runat="server" ID="phWorkingFiles" />
                        </fieldset>
                    </asp:Panel>

                    <%-- submitted files --%>
                    <asp:Panel runat="server" ID="pnlSubmittedFiles">
                        <fieldset>
                            <legend>Client Submitted Files</legend>
                            <asp:Placeholder runat="server" ID="phSubmittedFiles" />
                        </fieldset>
                    </asp:Panel>

                    <h2>Job Details</h2>
                    <table>
                        <tr>
                            <td class="tableLabelRightAligned">Client:</td>
                            <td><asp:Label runat="server" ID="lblClient" /></td>
                        </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Status:</td>
                            <td><asp:Label runat="server" ID="lblStatus" /><%-- e.g., "Completed 3/23/2011 3:00 pm Pacific --%></td>
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
                            <td class="tableLabelRightAligned">Job Type:</td>
                            <td><asp:Label runat="server" ID="lblJobType" /></td>
                        </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Instructions:</td>
                            <td><asp:Placeholder runat="server" ID="phInstructions" /></td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlStart">
                    <fieldset>
                        <legend>Start Work</legend>
                        <table>
                            <tr>
                                <td class="tableLabelRightAligned">Notes:</td>
                                <td><asp:TextBox runat="server" ID="txtSegmentNotesForStart" TextMode="MultiLine" Rows="3" Columns="50" /></td>
                            </tr>
                        </table>
                        <div class="marginTop20px"><asp:Button runat="server" ID="btnStart" Text="Start" 
                            onclick="btnStart_Click" /></div>
                    </fieldset>
                    
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlStop" style="display: none;">
                    <fieldset>
                        <legend>Stop Work</legend>
                        <table>
                            <tr>
                                <td class="tableLabelRightAligned">Started:</td>
                                <td><asp:TextBox runat="server" ID="txtTimeStarted" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Stopped:</td>
                                <td><asp:TextBox runat="server" ID="txtTimeStopped" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Hours worked:</td>
                                <td><asp:TextBox runat="server" ID="txtHoursWorked" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">New status:</td>
                                <td><asp:DropDownList runat="server" ID="ddStatuses" DataSourceID="odsJobStatuses" DataTextField="Name" DataValueField="JobStatusId" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Notes:</td>
                                <td><asp:TextBox runat="server" ID="txtSegmentNotesForStop" TextMode="MultiLine" Rows="3" Columns="50" /></td>
                            </tr>
                        </table>

                        <fieldset>
                            <legend>Add any new files:</legend>
                            <div id="returnFileInputsContainer">
                                Files to return to client:<br />
                                <div><asp:FileUpload runat="server" id="returnFile1" /></div>
                            </div>
                        
                            <div id="workingFileInputsContainer">
                                Working files:<br />
                                <div><asp:FileUpload runat="server" id="workingFile1" /></div>
                            </div>

                            <div id="submittedFileInputsContainer">
                                Client submitted files:<br />
                                <div><asp:FileUpload runat="server" id="submittedFile1" /></div>
                            </div>
                        </fieldset>
                        <div class="marginTop20px right"><asp:Button runat="server" ID="btnStop" 
                                Text="Stop" onclick="btnStop_Click" /></div>
                    </fieldset>
                </asp:Panel>

                    <asp:Panel runat="server" ID="pnlSegments">
                        <h2>Previous/Current Work</h2>
                        <asp:PlaceHolder runat="server" ID="phSegments" />
                    </asp:Panel>

<%--                    <asp:ListView runat="server" ID="lvSegments" DataSourceID="odsSegments">
                        <LayoutTemplate>
                            <table>
                                <thead>
                                    <th>Employee</th>
                                    <th>Start</th>
                                    <th>Stop</th>
                                    <th>Notes</th>
                                    <th>Resulting Status</th>
                                </thead>
                                <tbody runat="server" ID="tbOpenRecentJobs"><asp:PlaceHolder runat="server" ID="phOpenRecentJobs" /></tbody>
                            </table>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Employee.FirstName") + " " + Eval("Employee.LastName") %></td>
                                <td><%# HesterConsultants.admin.AdminUtils.StandardDateFormat((DateTime)Eval("StartDate")) %></td>
                                <td><%# HesterConsultants.admin.AdminUtils.StandardDateFormat((DateTime)Eval("EndDate")) %></td>
                                <td><%# Eval("Notes") %></td>
                            </tr>
                            
                        </ItemTemplate>

                        <EmptyDataTemplate>
                            <div>[No work segments.]</div>
                        </EmptyDataTemplate>
                    </asp:ListView>
--%>                

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


<%--    <asp:ObjectDataSource runat="server" ID="odsSegments" 
        DataObjectTypeName="HesterConsultants.AppCode.Entities.JobSegment" 
        TypeName="HesterConsultants.AppCode.Entities.JobSegment" 
        SelectMethod="SegmentsForJob" onselecting="odsSegments_Selecting">
    
        <SelectParameters>
            <asp:QueryStringParameter Name="job" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
--%></body>
</html>
