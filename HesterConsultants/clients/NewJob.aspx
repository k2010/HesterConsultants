<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewJob.aspx.cs" Inherits="HesterConsultants.clients.NewJob" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Submit New Work Request</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <link rel="stylesheet" type="text/css" href="/style/hcblue/jquery-ui-1.8.11.custom.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<style type="text/css">
        .ui-dialog-titlebar-close
        {
            /* hide the close button on the dialogs */
            display: none;
        }
        
        #pnlConversionEditingOptions
        {
            display: none;
            background-color: #ffffff;
        }
        
        .conversionOptions
        {
            display: none;
            float: left;
            margin-right: 10px;
        }
        
        .proofingOptions
        {
            display: none;
            float: left;
            margin-right: 20px;
        }
        
        #moreFileInputs
        {
            display: none;
        }
    </style>

    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui-1.8.11.custom.min.js"></script>
    <script type="text/javascript" src="/scripts/hcJq.js"></script>
    <script type="text/javascript" src="/scripts/hc.js"></script>

    <script type="text/javascript">
        $(document).ready(function ()
            {
                hesterConsultants.setupClientPanel();

                $(function ()
                {
                	$("#pnlAlert").dialog(
                    {
                    	autoOpen: true,
                    	title: "<%=Settings.Default.CompanyName %>",
                    	width: 500,
                    	height: 250,
                    	modal: true,
                    	closeOnEscape: false,
                    	buttons:
                        {
                        	"OK": function ()
                        	{
                        		$(this).dialog("close");
                        	}
                        }
                    });

//                    $("#pnlTos").dialog(
//                    {
//                        autoOpen: false,
//                        title: "<%=Settings.Default.CompanyName %> Terms of Service",
//                        width: 600,
//                        height: 400,
//                        modal: true,
//                        closeOnEscape: false,
//                        buttons:
//                        {
//                            "Agree": function ()
//                            {
//                                $(this).dialog("close");
//                            },
//                            "Disagree": function ()
//                            {
//                                window.history.back();
//                            }
//                        },
//                        close: function ()
//                        {
//                            $("#<%=txtBillingRef.ClientID %>").get(0).focus();
//                        }
//                    });
                });

                // show dialog if not a postback
                //if (!!!<%=this.Postback0Or1() %>)
                //    $("#pnlTos").dialog("open");

                $(function ()
                {
                    $("#txtDateDue").datepicker(
                    {
                        dateFormat: "m/d/yy",
                        showOn: "both",
                        buttonImage: "/style/images/calendar.png",
                        buttonImageOnly: true,
                        buttonText: "Click to show calendar"
                    });
                });

                $("#<%=this.ddJobTypes.ClientID %>").change(function ()
                {
                    var convert = "<%=HesterConsultants.Properties.Settings.Default.JobTypeConversionId %>";
                    var edit = "<%=HesterConsultants.Properties.Settings.Default.JobTypeEditingId %>";
                    var jobTypeVal = $("#<%=this.ddJobTypes.ClientID %>").val();

                    var speed = ($.browser.msie ? 100 : 1000) // ugly ie fade - make it go fast

                    if (jobTypeVal == convert || jobTypeVal == edit)
                    {
                        // show panel
                        $("#pnlConversionEditingOptions").fadeIn(speed);

                        // always show proofing options
                        $("#pnlConversionEditingOptions").find(".proofingOptions").fadeIn(speed);

                        // show conversion options
                        if (jobTypeVal == convert)
                        {
                            $("#optionsLegend").text("Conversion Options");
                            $("#pnlConversionEditingOptions").find(".conversionOptions").fadeIn(speed);
                        }
                        else
                        {
                            $("#optionsLegend").text("Editing Options");
                            $("#pnlConversionEditingOptions").find(".conversionOptions").fadeOut(speed);
                        }
                    }
                    else
                        $("#pnlConversionEditingOptions").fadeOut(speed);
                });

                $("#<%=this.rbApplications.ClientID %> input").click(function ()
                {
                    var idx = $("#<%=this.rbApplications.ClientID %> input").index(this);
                    if (idx < 2)
                    {
                        $("#<%=this.rbVersions.ClientID %>").removeAttr("disabled");
                        $("#<%=this.rbVersions.ClientID %>").find("*").removeAttr("disabled");
                    }
                    else
                    {
                        $("#<%=this.rbVersions.ClientID %>").attr("disabled", "disabled");
                        $("#<%=this.rbVersions.ClientID %>").find("*").attr("disabled", "disabled");
                    }
                });

                $("#<%=this.pnlHelp.ClientID %>").dialog(
                {
                    autoOpen: false,
                    width: 600,
                    title: "Job Options Help",
                    buttons:
                    {
                        "OK": function ()
                        {
                            $(this).dialog("close");
                        }
                    }
                });

                $("#hlOptionsHelp").click(function (e)
                {
                    e.preventDefault();
                    $("#<%=this.pnlHelp.ClientID %>").dialog("open");
                });

                $("#moreFileInputsLink").click(function (e)
                {
                    e.preventDefault();
                    showMoreFileInputs();
                });

                function showMoreFileInputs()
                {
                    $("#moreFileInputs").slideDown(1000, function ()
                    {
                        $("#moreFileInputsMessage").fadeOut(500, function ()
                        {
                            $("#moreFileInputsTip").html('Need to upload more files? Use our secure <a href="<%=Settings.Default.ContactUrl %>">contact form</a> after submitting this job, or consider using a zip utility to combine multiple files.').fadeIn(500);
                        })
                    }); 
                }

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
                <h1>Submit New Work Request</h1>

                <asp:Panel runat="server" ID="pnlNewJob">

                    <asp:Panel runat="server" ID="pnlJobFormContainer" CssClass="pnlJobFormContainer">
                    <fieldset>
                        <legend>Details</legend>
                        <table>
                            <tr>
                                <td class="tableLabelRightAligned" style="width: 150px;">Your billing reference:</td>
                                <td><asp:TextBox runat="server" ID="txtBillingRef"></asp:TextBox>&nbsp;<span class="asteriskRequiredField">*</span><asp:RequiredFieldValidator runat="server" ID="reqBilling" ControlToValidate="txtBillingRef" ErrorMessage="Billing reference is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Job Type:</td>
                                <td><asp:DropDownList runat="server" ID="ddJobTypes" DataSourceID="odsJobTypes" DataTextField="Name" DataValueField="JobTypeId" AppendDataBoundItems="true" onload="ddJobTypes_Load" />&nbsp;<span class="asteriskRequiredField">*</span><asp:RangeValidator runat="server" ID="rngJobType" ControlToValidate="ddJobTypes" ErrorMessage="Please select a job type." MinimumValue="1" MaximumValue="8" SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                            </tr>
                            <tr id="trConversionEditingOptions">
                                <td></td>
                                <td>
                                    <div id="pnlConversionEditingOptions">
                                        <fieldset>
                                            <legend id="optionsLegend" style="font-weight: normal;"></legend>
                                                <div class="conversionOptions">
                                                    <div style="border-bottom: solid 1px #666666;">Convert to</div>
                                                    <asp:RadioButtonList runat="server" ID="rbApplications">
                                                        <asp:ListItem Text="Word" Value="Word" />
                                                        <asp:ListItem Text="Excel" Value="Excel" />
                                                        <asp:ListItem Text="Other" Value="Other" />
                                                    </asp:RadioButtonList>
                                            
                                                </div>

                                                <div class="conversionOptions">
                                                    <div style="border-bottom: solid 1px #666666;">Version</div>
                                                    <asp:RadioButtonList runat="server" ID="rbVersions" Enabled="false">
                                                        <asp:ListItem Text="2007-10" Value="2007" />
                                                        <asp:ListItem Text="97-2003" Value="2003" />
                                                    </asp:RadioButtonList>
                                                </div>

                                                <div class="conversionOptions">
                                                    <div style="border-bottom: solid 1px #666666;">Formatting</div>
                                                    <asp:RadioButtonList runat="server" ID="rbFormatted">
                                                        <asp:ListItem Text="Formatted" Value="yes" Selected="True" />
                                                        <asp:ListItem Text="Unformatted" Value="no" />
                                                    </asp:RadioButtonList>
                                                </div>
                                            
                                                <div class="proofingOptions">
                                                                                                                                                           <div style="border-bottom: solid 1px #666666;">Proofing</div>
                                                    <asp:RadioButtonList runat="server" ID="rbProof">
                                                    <asp:ListItem Text="Yes" Value="yes" Selected="True" />
                                                    <asp:ListItem Text="No" Value="no" />
                                                </asp:RadioButtonList>
                                                </div>

                                                <div><a href="" id="hlOptionsHelp">Help</a></div>
                                        </fieldset>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Due:</td>
                                <td><asp:TextBox runat="server" ID="txtDateDue" style="margin-right: 3px;" />&nbsp;<hcc:HalfHourDropDown runat="server" id="ddHalfHours" />&nbsp;<span class="asteriskRequiredField">*</span></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><asp:PlaceHolder runat="server" ID="phDateRangeValidator" /><asp:RequiredFieldValidator runat="server" ID="reqDate" ControlToValidate="txtDateDue" ErrorMessage="Please enter a due date.<br />" SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" />Your times are displayed in <asp:Label runat="server" ID="lblUserTimeZone" CssClass="gentleHelpText" />. (<a href="<%=Settings.Default.ClientProfileUrl %>">change</a>).</td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned">Additional instructions:</td>
                                <td><asp:TextBox runat="server" ID="txtInstructions" TextMode="MultiLine" Rows="6" style="width: 90%;" /></td>
                            </tr>
                            <tr>
                                <td class="tableLabelRightAligned" style="white-space: normal;">Upload any <br />necessary files:</td>
                                <td><%-- submitted files --%>
                                    <table>
                                        <tr>
                                            <td>
                                                <div id="submittedFileInputsContainer">
                                                    <div><asp:FileUpload runat="server" id="file1" /></div>
                                                    <div><asp:FileUpload runat="server" id="file2" /></div>
                                                    <div><asp:FileUpload runat="server" id="file3" /></div>
                                                    <div id="moreFileInputs">
                                                        <div><asp:FileUpload runat="server" id="file4" /></div>
                                                        <div><asp:FileUpload runat="server" id="file5" /></div>
                                                        <div><asp:FileUpload runat="server" id="file6" /></div>
                                                    </div>
                                                </div>
                                                <div id="moreFileInputsMessage"><a id="moreFileInputsLink" href="">More files...</a></div>
                                            </td>
                                            <td style="padding-left: 5px;">
                                                <ul class="gentleHelpText" style="margin: 0;">
                                                    <li>Include a sample document if we need to follow a template.</li>
                                                    <li>Large files may take a while to upload.</li>
                                                </ul> </td>
                                        </tr>
                                    </table>
                                    <div id="moreFileInputsTip"></div>
                                </td><%-- end of submitted files td --%>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="padding-top: 5px; border-top: solid 1px #cccccc;">
                                    <table style="width: 90%;">
                                        <tr>
                                            <td style="vertical-align: middle; padding-right: 5px;"><asp:Button runat="server" ID="btnSubmit" CssClass="bigButton" Text="Submit" onclick="btnSubmit_Click" /></td>
                                            <td class="gentleHelpText" style="vertical-align: middle;"><b>Important:</b> Do not click Submit unless you have read and agreed to the Terms of Service, which should have been displayed in a popup dialog, and are repeated below. Thank you.
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlSubmissionInfo" CssClass="marginTop20px">
                            <strong>Please note:</strong> 
                            <ul>
                                <li>When you submit a job, your job will remain in the "Submitted" status until we review it and confirm that we can undertake it.</li>
                                <li>We will email you an estimate of the fees for the job.</li>
                                <li>If you approve of the estimate, please reply to our email. At that point, we can begin work.</li>
                            </ul>
                
                        </asp:Panel>


                    </fieldset>
                    </asp:Panel>

                    <asp:ValidationSummary runat="server" ID="valSum1" ShowMessageBox="false" ShowSummary="true" HeaderText="The following errors occurred:" />

                </asp:Panel>

                <asp:Panel runat="server" id="pnlTos">
                    <hcc:Agreements runat="server" ID="pnlTosInner" />
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlTosRedundant" style="width: 90%;">
                    <hcc:Agreements runat="server" ID="pnlTosRedundantInner" />
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlHelp">
                    <hcc:ConversionEditingOptionsHelp runat="server" ID="ceoh1" />
                </asp:Panel>

				<div id="pnlAlert">
					<p>
						<%=Settings.Default.CompanyNameShort %>
						is not accepting new assignments at this time. The website remains active for demonstration purposes only.</p>
					<p>
						For more information, contact <a href="mailto:<%=Settings.Default.CompanyContactEmail %>">
							<%=Settings.Default.CompanyContactEmail %></a>.</p>
				</div>
			</div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsJobTypes" 
        DataObjectTypeName="HesterConsultants.AppCode.Entities.JobType" 
        TypeName="HesterConsultants.AppCode.Entities.JobType" 
        SelectMethod="AllJobTypes">
    </asp:ObjectDataSource>

</body>
</html>
