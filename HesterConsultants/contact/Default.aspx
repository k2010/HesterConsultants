<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.contact.Default" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Contact Us</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" media="Screen" />
    <link rel="stylesheet" type="text/css" href="/style/hc-mobile.css" media="handheld" />
    <link  rel="stylesheet" type="text/css" href="/style/hc-small-device.css" media="only screen and (max-device-width: 480px)" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<style type="text/css">
        #moreFileInputs
        {
            display: none;
        }
    </style>


    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/hcJq.js"></script>
    <script type="text/javascript" src="/scripts/hc.js"></script>

    <% = SiteUtils.CheckForClientValsScript(this) %>
    
    <script type="text/javascript">
    	$(document).ready(function ()
    	{
    		// 2014
    		return;
    		// -----------------------------------

    		hesterConsultants.setupClientPanel();

    		var panelShowing = !($("#<%=this.pnlContactTableContainer.ClientID %>").is(":hidden"));
    		if (panelShowing)
    		{
    			// focus first empty box
    			if ($("#<%=this.txtName.ClientID %>").val() == "")
    				$("#<%=this.txtName.ClientID %>").get(0).focus();
    			else if ($("#<%=this.txtEmail.ClientID %>").val() == "")
    				$("#<%=this.txtEmail.ClientID %>").get(0).focus();
    			else if ($("#<%=this.txtSubject.ClientID %>").val() == "")
    				$("#<%=this.txtSubject.ClientID %>").get(0).focus();
    			else
    				$("#<%=this.txtMessage.ClientID %>").get(0).focus();
    		}

    		$("#btnSendAnother").click(function ()
    		{
    			showFormAgain();
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
    					$(this).text("Need to upload more files? Just come back and use this form again, or consider using a zip utility to combine multiple files.").fadeIn(500);
    				})
    			}); // css("display", "block");
    		}

    		function showFormAgain()
    		{
    			$("#<%=this.pnlContactTableContainer.ClientID %>").css("display", "block");
    			$("#<%=this.pnlContactSendAnother.ClientID %>").css("display", "none");

    			$("#<%=this.txtSubject.ClientID %>").get(0).select();
    		}
    	});
    </script>

</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader ID="header1" runat="server" />
            <hcc:ClientControlPanel runat="server" ID="ccp1" Visible="false" />

            <div id="<%=this.MainBodyOrMainBodyClients() %>">
            <h1>Contact <%=Settings.Default.CompanyNameShort %> <span style="color: Red;">(Disabled)</span></h1>
            <p><asp:Label ID="lblIntro" runat="server">Use this secure form to send us a message (or use the simple email link to our <%=Settings.Default.CompanyContactTitle.ToLower() %> below.)</asp:Label></p>

            <asp:Panel runat="server" ID="pnlContact" Enabled="false">
                <asp:Panel runat="server" ID="pnlContactTableContainer">
                    <div id="contactErrorMessage">&nbsp;</div>
    
                    <div class="pnlBtnSendMessage"><asp:Button ID="btnSend" runat="server" CssClass="bigButton" Text="Send" 
                            onclick="btnSend_Click"></asp:Button></div>
	                <table id="tblContact"><!---->
		                <tr>
			                <td style="text-align: right; white-space: nowrap;">Your name:</td>
			                <td><asp:TextBox id="txtName" runat="server" style="width: 350px;"></asp:TextBox></td>
		                </tr>
		                <tr>
			                <td style="text-align: right; white-space: nowrap;">Your email:</td>
			                <td><asp:TextBox id="txtEmail" runat="server" style="width: 350px;"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="val1" runat="server" ControlToValidate="txtEmail" ValidationExpression="[a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+"  ErrorMessage="<br />Oops. That email address appears to be invalid. Please double check it." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /><asp:RequiredFieldValidator runat="server" ID="val2" ControlToValidate="txtEmail" ErrorMessage="<br />Please include a valid email address." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
		                </tr>
		                <tr>
			                <td class="tableLabelRightAligned">Subject:</td>
			                <td><asp:TextBox id="txtSubject" runat="server" style="width: 350px;"></asp:TextBox></td>
		                </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Message:</td>
                            <td><asp:TextBox id="txtMessage" runat="server" TextMode="MultiLine" style="width: 350px;" Rows="10" /></td>
                        </tr>
                    </table>
                    <div id="pnlContactAttachments">
                        <div id="pnlContactAttachmentsNote" class="text">Attach one or more files:</div>
                        <div id="pnlAttachments">
                            <div id="contactFileInputsContainer">
                                <div><asp:FileUpload Enabled="false" title="Disabled" runat="server" ID="file1" /></div>
                                <div id="moreFileInputs">
                                    <div><asp:FileUpload Enabled="false" title="Disabled" runat="server" id="file2" /></div>
                                    <div><asp:FileUpload Enabled="false" title="Disabled" runat="server" id="file3" /></div>
                                </div>
                            </div>
                            <%--<div id="moreFileInputsMessage" class="text"><a id="moreFileInputsLink" href="">More files...</a></div>--%>
                        </div>
                    </div>

                </asp:Panel><!--contact table container-->
    
                <asp:Panel runat="server" ID="pnlContactSendAnother">
                    <button id="btnSendAnother" type="button">Send another message...</button>
                </asp:Panel>

                <div id="pnlAddressContainer" style="position: relative;">
	                <div class="address">
                        <p><span class="nameTitle"><%=Settings.Default.CompanyContactName %><br />
                        <%=Settings.Default.CompanyContactTitle %></span><br />
                        <a href="mailto:<%=Settings.Default.CompanyContactEmail %>"><%=Settings.Default.CompanyContactEmail %></a><br />
                        <%=Settings.Default.CompanyContactPhone %><%-- | <a href="mailto:<%=Settings.Default.CompanyContactTextEmail %>">Text via Email</a>--%></p>
        
                        <p><%=Settings.Default.CompanyName %><%--<br />
                        <%=Server.HtmlDecode(Settings.Default.CompanyAddress) %>--%></p>

                        <div class="securitySeal">
                            <span id="siteseal">
                                <script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=6UDaCOHTMsySu0hrL2cCrlljpt7pxjo70cN114YfIrLKi88POV7ms"></script>
                            </span>
                        </div>
                    </div>
                </div><!--attachments and address container-->   
	        </asp:Panel>
            </div>
    
            <hc:HcFooter ID="HcFooter1" runat="server" />
        </form>
    </div>
	<script src="/scripts/googleAnalytics.js"></script>
</body>
</html>
