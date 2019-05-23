<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthUsers.aspx.cs" Inherits="HesterConsultants.admin.AuthUsers" %>

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
                <h1>Administer Authenticated Users</h1>

                <asp:Panel runat="server" ID="pnlUsers">
                    <h2>Logged On Users</h2>
                    <asp:PlaceHolder runat="server" ID="phCurUsers" />
                </asp:Panel>

                <asp:ListView runat="server" ID="lvAuthUsers" DataSourceID="odsAuthUsers" 
                    ItemPlaceholderID="tbAuthUsers" DataKeyNames="UserName" 
                    onitemcommand="lvAuthUsers_ItemCommand" 
                    oniteminserting="lvAuthUsers_ItemInserting"><%-- use UserName as key -- delete method set up that way --%>
                        <LayoutTemplate>
                            <table class="openJobs">
                                <thead>
                                    <th>User Name</th>
                                    <th>Online</th>
                                    <th>Locked out</th>
                                    <th></th><%-- manage --%>
                                </thead>
                                <tbody runat="server" ID="tbAuthUsers"><asp:PlaceHolder runat="server" ID="phOpenRecentJobs" /></tbody>
                            </table>
                            <p><asp:LinkButton runat="server" ID="hlAddUser" Text="Add user" CommandName="Add" /></p>
                        </LayoutTemplate>
                        
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("MembershipUser.UserName") %></td>
                                <td><asp:CheckBox runat="server" ID="chkOnline" Checked='<%# Eval("MembershipUser.IsOnline") %>' /></td>
                                <td><asp:CheckBox runat="server" ID="chkLockedOut" Checked='<%# Eval("MembershipUser.IsLockedOut") %>' /></td>

                                <td><asp:HyperLink runat="server" ID ="hlManageUser" NavigateUrl='<%# Eval("UserName", "/admin/AuthUserManage.aspx?userName={0}") %>' Text="Manage" />
                                <br />
                                <asp:LinkButton runat="server" ID="hlDeleteUser" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this user?');" /></td>
                            </tr>
                        </ItemTemplate>

                        <InsertItemTemplate>
                            <tr>
                                <td>
                                    <table class="">
                                        <tr>
                                            <td>User Email: </td>
                                            <td><asp:TextBox runat="server" ID="txtUserName" /></td>
                                        </tr>
                                        <tr>
                                            <td>Password: 
                                            <td><asp:TextBox runat="server" ID="txtPw" /></td></td>
                                        </tr>
                                        <tr>
                                            <td>Question: </td>
                                            <td><asp:TextBox runat="server" ID="txtSecQuestion" /></td>
                                        </tr>
                                        <tr>
                                            <td>Answer: </td>
                                            <td><asp:TextBox runat="server" ID="txtSecAnswer" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td><asp:Button runat="server" ID="btnSaveUser" CommandName="Insert" Text="Save" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </InsertItemTemplate>

                </asp:ListView>
            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsAuthUsers" 
        DataObjectTypeName="HesterConsultants.AppCode.Entities.AuthenticationUser" 
        TypeName="HesterConsultants.AppCode.Entities.AuthenticationUser" 
        SelectMethod="AllAuthUsers" 
        DeleteMethod="DeleteMembershipUser">
    </asp:ObjectDataSource>
</body>
</html>
