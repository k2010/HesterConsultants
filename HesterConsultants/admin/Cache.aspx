<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cache.aspx.cs" Inherits="HesterConsultants.admin.Cache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Administer Cache</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <link rel="stylesheet" type="text/css" href="/style/hcblue/jquery-ui-1.8.11.custom.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->

    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/hcJq.js"></script>
    <script type="text/javascript" src="/scripts/hc.js"></script>
    <script type="text/javascript">
        $(document).ready(
            function ()
            {
                hesterConsultants.setupClientPanel();
            });
    </script>
</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <hca:AdminControlPanel runat="server" ID="acp1" />
            <div id="mainbodyClients">
                <h1>Cache</h1>

                <asp:Panel runat="server" ID="pnlCache">
                    <asp:Button runat="server" ID="btnClearCache" Text="Clear Cache" 
                            onclick="btnClearCache_Click" />
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlDetails">
                    <h3>Clients</h3>
                    <asp:PlaceHolder runat="server" ID="phClients"></asp:PlaceHolder>
    
                    <h3>Jobs</h3>
                    <asp:PlaceHolder runat="server" ID="phJobs"></asp:PlaceHolder>
                </asp:Panel>
            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

<asp:ObjectDataSource 
    runat="server" 
    ID="odsOpenRecentJobs"
    DataObjectTypeName="HesterConsultants.AppCode.Entities.Job"
    TypeName="HesterConsultants.AppCode.CacheLayer"
    SelectMethod="RecentJobsForAdmin">
</asp:ObjectDataSource>

</body>
</html>
