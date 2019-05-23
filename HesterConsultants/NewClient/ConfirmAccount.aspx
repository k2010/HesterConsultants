<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmAccount.aspx.cs" Inherits="HesterConsultants.NewClient.ConfirmAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - New Account</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>
</head>
<body>
    <form id="form1" runat="server">
        <hc:HcHeader runat="server" ID="header1" />
        <%--<div id="mainbody">
            <h1>Check Your Email</h1>
            <p>Thank you for registering. To verify your email address, we have sent a message to that address.</p> 
            <p>Please check your email and click the link to log in.</p>
            <div class="gentleHelpText">
                <p><strong>Important:</strong> To ensure you get our emails, please add the following to your email contacts:</p>
                <ul>
                    <li><%=Settings.Default.CustomerServiceEmail %></li>
                    <li><%=Settings.Default.CompanyContactEmail %></li>
                </ul>

                <p><strong>Tip:</strong> Add the entire domain <%=Settings.Default.CompanyDomain %>, or "*@<%=Settings.Default.CompanyDomain %>" if your email program supports that.</p>

            </div>
            <p>You have been logged out for the time being.</p>

            <div class="securitySeal">
                <span id="siteseal">
                    <script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=6UDaCOHTMsySu0hrL2cCrlljpt7pxjo70cN114YfIrLKi88POV7ms"></script>
                </span>
            </div>
        </div>--%>
        <hc:HcFooter runat="server" ID="footer1" />
    </form>
</body>
</html>
