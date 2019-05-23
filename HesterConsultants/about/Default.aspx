<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.about.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - About Us</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" media="Screen" />
    <link rel="stylesheet" type="text/css" href="/style/hc-mobile.css" media="handheld" />
    <link  rel="stylesheet" type="text/css" href="/style/hc-small-device.css" media="only screen and (max-device-width: 480px)" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>
</head>
<body>
    <div id="container">
    <form id="form1" runat="server">
        
        <hc:HcHeader ID="header1" runat="server" />
            <%--<div id="adPanelRight">
            <div class="adHeading">Sponsors:</div>
            <!-- ad unit -->
            <script type="text/javascript"><!--
                google_ad_client = "pub-2996570680838598";
                /* 300x250, created 5/19/10 */
                google_ad_slot = "0489344119";
                google_ad_width = 300;
                google_ad_height = 250;
            //-->
            </script>
            <script type="text/javascript"
            src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>
        </div>--%>
        <div id="mainbody">
            <hcc:AboutThePrincipal runat="server" id="about1" />
        </div>
        <hc:HcFooter ID="HcFooter1" runat="server" />
    </form>
</div>
	<script src="/scripts/googleAnalytics.js"></script>
</body>
</html>
