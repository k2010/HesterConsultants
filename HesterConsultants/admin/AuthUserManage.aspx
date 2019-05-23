<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthUserManage.aspx.cs" Inherits="HesterConsultants.admin.AuthUserManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Administer Authenticated Users</title>
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
                <h1>Administer Authenticated User</h1>
                <table>
                    <tr>
                        <td>User name:</td>
                        <td><asp:Label runat="server" ID="lblUsername" /></td>
                    </tr>
                    <tr>
                        <td>Roles:</td>
                        <td><asp:PlaceHolder runat="server" ID="phRoles" /></td>
                    </tr>
                    <tr>
                        <td>Status:</td>
                        <td><asp:CheckBox runat="server" ID="chkLockedOut" Text="Locked out" /> (can only unlock; can't lock)</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="right"><asp:Button runat="server" ID="btnUpdateUser" Text="Update" 
                                onclick="btnUpdateUser_Click" /></td>
                    </tr>
                </table>

            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>
</body>
</html>
