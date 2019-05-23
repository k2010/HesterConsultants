<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.temp09132012.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>
		<%=HesterConsultants.Properties.Settings.Default.CompanyName %></title>
	<link rel="stylesheet" type="text/css" href="/style/hc.css" media="Screen" />
	<link rel="stylesheet" type="text/css" href="/style/hc-mobile.css" media="handheld" />
	<link rel="stylesheet" type="text/css" href="/style/hc-small-device.css" media="only screen and (max-device-width: 480px)" />
	<!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />
	<%--<script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
	<script type="text/javascript" src="/scripts/jquery-ui-1.8.11.custom.min.js"></script>--%>

</head>
<body>
	<div id="container">
		<form id="form1" runat="server">
			<hc:HcHeader ID="header1" runat="server" />
			<div id="mainbody">
				<div style="margin-top: 20px; padding: 20px;">
					Please click this link to download the installer files: <br />
					<a id="download" href="./GetFile.aspx">Download</a>
				</div>
				<asp:Panel runat="server" id="debug"></asp:Panel>
			</div>
			<hc:HcFooter ID="HcFooter1" runat="server" />
		</form>
	</div>
</body>
</html>
