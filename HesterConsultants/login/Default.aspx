<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.login.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Client Login</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>

    <script type="text/javascript">
    	$(document).ready(function ()
    	{
    		// 2014
    		return;
    		// ------------------------

    		var usernameInput = "<%=this.UserNameTextBox() %>";
    		if (usernameInput == "")
    			return;

    		var usernameInputJq = $("#" + usernameInput);
    		var usernameInputDom = usernameInputJq.get(0);

    		var pwInput = "<%=this.PwTextBox() %>";

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

    		// focus on username or pw
    		if (usernameInputJq.val() == "")
    		{
    			if (usernameInput != "")
    				usernameInputDom.focus();
    		}
    		else
    		{
    			if (pwInput != "")
    				$("#" + pwInput).get(0).focus();
    		}
    	});
    </script>
</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <div id="mainbody" style="width: 90%;">
               
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%;">
                            <h1>Client Login <span style="color: Red;">(Disabled)</span></h1>
                            <asp:Panel runat="server" ID="pnlLogin">
                                <asp:Login Enabled="false" TitleText="Disabled" ID="Login1" 
                                    runat="server" 
                                    CssClass="loginTable" 
                                    UserNameLabelText="Email:" 
                                    LabelStyle-CssClass="tableLabelRightAligned" 
                                    RememberMeText="Remember email next time" 
                                    LoginButtonText="Login" 
                                    onloggedin="Login1_LoggedIn">
                                </asp:Login>

                            </asp:Panel>

                            <asp:Panel runat="server" ID="pnlNewClientErrorMessage" Visible="false" class="errorMessage">
                                <p>Sorry, there was a problem logging you in.</p>
                                <p><b>New clients:</b> Please double-check your welcome email to ensure that you have the entire link entered in your browser's address bar, with no spaces or line breaks.</p>
                                <p>If you are still unsuccessful, please call us at <%= Settings.Default.CompanyPhoneTechSupport %>, or send an email to <a href="mailto:<%= Settings.Default.CustomerServiceEmail %>"><%= Settings.Default.CustomerServiceEmail %></a>.</p>
                            </asp:Panel>

                            <asp:Panel runat="server" ID="pnlHelp" CssClass="marginBottom20px">
                                <%--<p><a href="./ResetPassword.aspx">Forgot password?</a></p>--%>
                            </asp:Panel>
                        </td>
                        <td style="padding-left: 20px; border-left: solid 1px #cccccc;">
                            <asp:Panel runat="server" ID="pnlNewAccountLink">
                                <h1>New Clients</h1>
                                <div class="indent20px"><a href="/NewClient/"><img src="/style/hcblue/images/right-arrow.png" width="12" height="12" alt="Create new account" style="vertical-align: middle;" /></a>&nbsp;&nbsp;<a href="/NewClient/">Create New Free Account</a></div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>


                <div class="securitySeal">
                <span id="siteseal">
                <script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=6UDaCOHTMsySu0hrL2cCrlljpt7pxjo70cN114YfIrLKi88POV7ms"></script>
                </span>
                </div>
            </div>
            <hc:HcFooter ID="HcFooter1" runat="server" />
        </form>
    </div>
	<script src="/scripts/googleAnalytics.js"></script>
</body>
</html>
