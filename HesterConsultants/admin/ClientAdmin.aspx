<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientAdmin.aspx.cs" Inherits="HesterConsultants.admin.ClientAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Settings.Default.CompanyName %> - Administer Clients</title>
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
                reset passwords, look up jobs<br />
                http://www.asp.net/security/tutorials/unlocking-and-approving-user-accounts-cs

                <asp:Panel runat="server" ID="pnlClient">
                    <table>
                        <tr>
                            <td class="tableLabelRightAligned">Name:</td>
                            <td><asp:Label runat="server" ID="lblName" /></td>
                        </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Email:</td>
                            <td><asp:Placeholder runat="server" ID="phEmail" /></td>
                        </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Phone:</td>
                            <td><asp:Label runat="server" ID="lblPhone" /></td>
                        </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Address:</td>
                            <td><asp:Label runat="server" ID="lblAddress" /></td>
                        </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Time Zone:</td>
                            <td><asp:Label runat="server" ID="lblTimeZone" /></td>
                        </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Open Invoices:</td>
                            <td><asp:PlaceHolder runat="server" ID="phInvoices" /></td>
                        </tr>
                        <tr>
                            <td class="tableLabelRightAligned">Tools:</td>
                            <td>
                                <asp:LinkButton runat="server" ID="hlInvoice" Text="Create Invoice" 
                                    onclick="hlInvoice_Click" /><br />
                                <asp:LinkButton runat="server" ID="hlLogin" Text="Log In As" /><br />
                                <asp:LinkButton runat="server" ID="hlLockOut" Text="Lock Out" OnClientClick="return confirm('Are you sure you want to lock out this user?');" /><br />
                                <asp:LinkButton runat="server" ID="hlDeleteUser" Text="Delete User" 
                                    OnClientClick="return confirm('Are you sure you want to delete this user?');" /> <asp:PlaceHolder runat="server" ID="phResetPwNote" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

</body>
</html>
