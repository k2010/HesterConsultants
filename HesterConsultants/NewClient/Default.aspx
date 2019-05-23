<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.NewClient.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Create New Account</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <link rel="stylesheet" type="text/css" href="/style/hcblue/jquery-ui-1.8.11.custom.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui-1.8.11.custom.min.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>

    <script type="text/javascript">
    	$(document).ready(function ()
    	{
    		// 2014
    		return;
    		// ------------------------------

    		var didPwNoteAnim = false;
    		var usernameInput = "<%=this.EmailControlId() %>";
    		if (usernameInput == "")
    			return;

    		var usernameInputJq = $("#" + usernameInput);
    		var usernameInputDom = usernameInputJq.get(0);

    		// make login input type=email
    		if (!($.browser.msie && $.browser.version < 9))
    		{
    			try
    			{
    				// can't use jq to change type; throws exception
    				// so use dom var instead
    				usernameInputDom.type = "email";
    			}
    			catch (e)
    			{
    				// do nothing
    			}
    		}

    		if (!!!($("#<%=this.EmailControlId() %>").get(0)))
    			return;

    		$("#<%=this.EmailControlId() %>").get(0).focus();

    		$("#<%=this.EmailControlId() %>").blur(function ()
    		{
    			if (!didPwNoteAnim)
    			{
    				$("#pnlPasswordNote").animate(
                        { color: "#0080ff" },
                        { duration: 1000,
                        	complete: function ()
                        	{
                        		$("#pnlPasswordNote").animate({ color: "#666666" }, 3000);
                        	}
                        });

    				didPwNoteAnim = true;
    			}
    		});
    	});
    </script>
    
</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <div id="mainbody" style="width: 80%;">
                <h1>Register for Your New Account <span style="color: red;">(Disabled)</span></h1>
                
                <asp:Panel runat="server" ID="pnlNewAccountMessage" style="position: absolute; left: 450px;"><p>Thank you for registering for a new account at <%=Settings.Default.CompanyNameShort %>. Your account is free: we charge only for contracted work.</p> <p>We do not require any financial information from you—just basic contact information, including your billing address.</p></asp:Panel>

                <asp:CreateUserWizard Enabled="false" ID="userWiz1" runat="server" 
                    RequireEmail="false" 
                    LoginCreatedUser="true"
                    CreateUserButtonText="Next" 
                    LabelStyle-CssClass="tableLabelRightAligned" 
                    DuplicateUserNameErrorMessage="That email address is already in use." 
                    QuestionRequiredErrorMessage="Security Question is required." 
                    AnswerRequiredErrorMessage="Security Answer is required." 
                    ContinueButtonText="Next"
                    FinishCompleteButtonText="Finish"
                    ContinueDestinationPageUrl="~/NewClient/ConfirmAccount.aspx" 
                    CssClass="newUserWizardTable" 
                    oncontinuebuttonclick="userWiz1_ContinueButtonClick">
                    <WizardSteps>
                        <asp:CreateUserWizardStep ID="userWizStep1" runat="server">
                            <ContentTemplate>
                                
                                <table>
                                    <tr>
                                        <td class="tableLabelRightAligned"><asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Email address:</asp:Label></td>
                                        <td><asp:TextBox ID="UserName" runat="server"></asp:TextBox><span class="asteriskRequiredField">*</span>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="<br />Email is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" />
                                            <asp:RegularExpressionValidator runat="server" ID="EmailExpressionValidator" ControlToValidate="UserName" ValidationExpression="[a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+" ErrorMessage="<br />Please enter a valid email address." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tableLabelRightAligned"><asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label></td>
                                        <td><asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox><span class="asteriskRequiredField">*</span>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="<br />Password is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><div id="pnlPasswordNote"><span id="lblPasswordNote">Password must be at least 7 characters, with at least 1 non-alphanumeric character:</span></div></td>
                                    </tr>
                                    <tr>
                                        <td class="tableLabelRightAligned"><asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm password:</asp:Label></td>
                                        <td><asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox><span class="asteriskRequiredField">*</span>
                                            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" ErrorMessage="<br />The Password and Confirmation Password must match." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tableLabelRightAligned"><asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">Security question:</asp:Label></td>
                                        <td><asp:TextBox ID="Question" runat="server"></asp:TextBox><span class="asteriskRequiredField">*</span></td>
                                    </tr>
                                    <tr>
                                        <td class="tableLabelRightAligned"><asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Security answer:</asp:Label></td>
                                        <td><asp:TextBox ID="Answer" runat="server"></asp:TextBox><span class="asteriskRequiredField">*</span></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td style="color: red"><asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:CreateUserWizardStep>

                        <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>Thank you. Please click Finish to create your account.</td>
                                    </tr>
                                    <tr>
                                        <td class="tableLabelRightAligned"><br /><asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue" Text="Finish" /></td>
                                    </tr>
                                </table>
                            
                            </ContentTemplate>
                        </asp:CompleteWizardStep>
                    </WizardSteps>
                </asp:CreateUserWizard>
            
                <asp:Panel runat="server" ID="pnlFootnotes" class="footnote" style="width: 450px;">
                    <div><span class="asteriskRequiredField">*</span> indicates a required field.</div>
                
                    
                </asp:Panel>

                <div class="securitySeal marginTop20px">
                    <span id="siteseal">
                        <script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=6UDaCOHTMsySu0hrL2cCrlljpt7pxjo70cN114YfIrLKi88POV7ms"></script>
                    </span>
                </div>
            </div>
            <hc:HcFooter ID="HcFooter1" runat="server" />
        </form>
    </div>
</body>
</html>