<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.admin.reports.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Site Reports</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>
</head>
<body>
    <form id="form1" runat="server">
    <hc:HcHeader runat="server" ID="header1" />
    <hc:HcFooter runat="server" ID="footer1" />
    </form>
</body>
</html>
