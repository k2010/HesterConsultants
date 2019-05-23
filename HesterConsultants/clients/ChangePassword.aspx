<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="HesterConsultants.clients.ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Change Password</title>
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
            hesterConsultants.setupClientPanel();

            if ($("#<%=this.OrigPwTextBoxName() %>").get(0))
            {
                $("#<%=this.OrigPwTextBoxName() %>").get(0).focus();
            }

            if ($("#<%=this.ConfirmNewPwTextBoxName() %>").get(0))
            {
                $("#<%=this.ConfirmNewPwTextBoxName() %>").keypress(function (e)
                {
                    if (e.which == 13)
                        e.preventDefault();
                });

                $("#<%=this.ConfirmNewPwTextBoxName() %>").keyup(function (e)
                {
                    if (e.which == 13)
                    {
                        e.preventDefault();
                        if ($("#<%=this.SubmitButtonName() %>").get(0))
                            $("#<%=this.SubmitButtonName() %>").click();
                    }
                });
            }

            else if ($("#<%=this.ContinueButtonName() %>").get(0))
            {
                $("#<%=this.ContinueButtonName() %>").get(0).focus();
            }
        });
    </script>

</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <hcc:ClientControlPanel runat="server" ID="ccp1" />
            <div id="mainbodyClients" style="width: 90%;">
                <h1>Change Password</h1>

                <asp:Panel runat="server" ID="pnlReset" CssClass="marginBottom20px" style="width: 420px;">
                    <asp:ChangePassword 
                        runat="server" 
                        ID="changePw" 
                        CssClass="loginTable" 
                        TitleTextStyle-HorizontalAlign="Left" 
                        FailureTextStyle-HorizontalAlign="Left" 
                        ChangePasswordTitleText=""
                        LabelStyle-CssClass="tableLabelRightAligned" 
                        ConfirmPasswordCompareErrorMessage="Confirm New Password must match New Password." 
                        SuccessText="" 
                        SuccessTitleText="Your password has been changed." 
                        ContinueDestinationPageUrl="/clients/Home.aspx"
                        CancelDestinationPageUrl="/clients/Home.aspx" 
                        onchangedpassword="changePw_ChangedPassword" style="width: 500px;" >
                    </asp:ChangePassword>
                </asp:Panel>

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
