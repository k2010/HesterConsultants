<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="HesterConsultants.login.ResetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Reset Password</title>
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
        $(document).ready(function ()
        {
            var emailControl = "<%=this.EmailTextBoxName() %>";
            if (emailControl == "")
                return;

            var emailJq = $("#" + emailControl);
            var emailDom = emailJq.get(0);

            if (emailDom)
            {
                if (!($.browser.msie && $.browser.version < 9))
                {
                    try
                    {
                        emailDom.type = "email";
                    }
                    catch (e)
                    {
                    }
                }

                emailDom.focus();

                emailJq.keypress(function (e)
                {
                    if (e.which == 13)
                        e.preventDefault();
                });

                emailJq.keyup(function (e)
                {
                    if (e.which == 13)
                    {
                        e.preventDefault();
                        if ($("#<%=this.EmailSubmitButtonName() %>").get(0))
                            $("#<%=this.EmailSubmitButtonName() %>").click();
                    }
                });
            }

            else if ($("#<%=this.AnswerTextBoxName() %>").get(0))
            {
                $("#<%=this.AnswerTextBoxName() %>").get(0).focus();

                $("#<%=this.AnswerTextBoxName() %>").keypress(function (e)
                {
                    if (e.which == 13)
                        e.preventDefault();
                });

                $("#<%=this.AnswerTextBoxName() %>").keyup(function (e)
                {
                    if (e.which == 13)
                    {
                        e.preventDefault();
                        if ($("#<%=this.AnswerSubmitButtonName() %>").get(0))
                            $("#<%=this.AnswerSubmitButtonName() %>").click();
                    }
                });
            }
        });
    </script>

</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <div id="mainbody" style="width: 90%;">
                <h1>Reset Password</h1>

               <asp:Panel runat="server" ID="pnlReset" style="width: 450px;">
                   <asp:PasswordRecovery Enabled="false"
                       ID="PasswordRecovery1" 
                       runat="server" 
                       CssClass="loginTable" 
                       TitleTextStyle-HorizontalAlign="Left" 
                       UserNameTitleText="Forgot your password?" 
                       UserNameInstructionText="Please enter your email address." 
                       InstructionTextStyle-HorizontalAlign="Left" 
                       LabelStyle-CssClass="tableLabelRightAligned" 
                       UserNameLabelText="Email: " 
                       QuestionInstructionText="Answer your security question to receive your password." 
                       onsendingmail="PasswordRecovery1_SendingMail" 
                       onverifyinganswer="PasswordRecovery1_VerifyingAnswer" >
                   </asp:PasswordRecovery>
                </asp:Panel>

                <p class="footnote marginBottom20px">Please note: We will generate a new password and email it to you. For security reasons, when you log in with your new password, you will be required to change the password.</p>
                
                <div class="securitySeal">
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
