<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="HesterConsultants.clients.Profile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Client Profile</title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" />
    <link rel="stylesheet" type="text/css" href="/style/hcblue/jquery-ui-1.8.11.custom.css" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui-1.8.11.custom.min.js"></script>
    <script type="text/javascript" src="/scripts/hcJq.js"></script>
    <script type="text/javascript" src="/scripts/hc.js"></script>
    <% = SiteUtils.CheckForClientValsScript(this) %>

    <script type="text/javascript">
        $(document).ready(
            function ()
            {
                hesterConsultants.setupClientPanel();

                if ($("#<%=this.txtFirstname.ClientID %>").val() == "")
                    $("#<%=this.txtFirstname.ClientID %>").get(0).focus();

                $(function ()
                {
                    setCurCompany("<%=this.CurCompanyId() %>");

                    var unmatchedCoExpressions = new Array();
                    var unmatchedAddrExpressions = new Array();
                    var curState;

                    function setCurCompany(companyId)
                    {
                        $("#<%=hidCompanyId.ClientID %>").val(companyId);
                        //alert("ajax " + companyId);
                        $.ajax(
                        {
                            url: "./CompanyList.aspx",
                            data: { "coId": companyId }
                        });
                        //alert(document.getElementById("hidCompanyId").value);
                    }

                    $("#<%=txtCompany.ClientID %>").autocomplete(
                    {
                        //source: "CompanyList.aspx",
                        source: function (request, response)
                        {
                            $.ajax(
                            {
                                url: "./CompanyList.aspx",
                                dataType: "json",
                                data: { term: request.term },
                                beforeSend: function (jqXHR, settings)
                                {
                                    // check if search term begins with unmatched expr,
                                    // cancel request if it does
                                    var curCoText = request.term;
                                    for (var k = 0; k < unmatchedCoExpressions.length; k++)
                                    {
                                        // if text in text box matches,
                                        // cancel search
                                        if (curCoText.length >= unmatchedCoExpressions[k].length
                                                &&
                                                curCoText.toLowerCase().indexOf(unmatchedCoExpressions[k], 0) == 0)
                                        {
                                            //alert(curCoText + " matches " + unmatchedCoExpressions[k] + " - canceling ajax");
                                            // stop the spinning
                                            $("#<%=txtCompany.ClientID %>").removeClass("ui-autocomplete-loading");

                                            return false; // don't send if already unmatched
                                        }
                                    }

                                    // bring back the spinning
                                    $("#<%=txtCompany.ClientID %>").addClass("ui-autocomplete-loading");
                                    return true;
                                },
                                success: function (data, textStatus, jqXHR)
                                {
                                    // if no result (companylist.aspx returns [{"id": "", "label": ""}])
                                    if (!!data && !!data[0])
                                    {
                                        // add term to unmatched companies
                                        if (data[0].id == "")
                                        {
                                            unmatchedCoExpressions.push(request.term.toLowerCase());
                                            // stop the spinning
                                            $("#<%=txtCompany.ClientID %>").removeClass("ui-autocomplete-loading");
                                        }

                                        response(data);
                                    }
                                    else // data null
                                    // stop the spinning
                                        $("#<%=txtCompany.ClientID %>").removeClass("ui-autocomplete-loading");

                                },
                                complete: function ()
                                {
                                    //alert("ajax complete");
                                    $("#<%=txtCompany.ClientID %>").removeClass("ui-autocomplete-loading");
                                },
                                error: function ()
                                {
                                    $("#<%=txtCompany.ClientID %>").removeClass("ui-autocomplete-loading");
                                    //alert("ajax error");
                                }
                            })
                        },
                        minLength: 1,
                        change: function (event, ui)
                        {
                            if (!!ui && !!ui.item && !!ui.item.id)
                                setCurCompany(ui.item.id ? ui.item.id : "0");
                        }
                    }); // end of company autocomplete

                    $("#<%=txtAddress1.ClientID %>").autocomplete(
                    {
                        //source: "AddressList.aspx",
                        source: function (request, response)
                        {
                            $.ajax(
                            {
                                url: "./AddressList.aspx",
                                dataType: "json",
                                data: { term: request.term },
                                beforeSend: function (jqXHR, settings)
                                {
                                    // check if search term matches unmatched companies,
                                    // cancel request if it does
                                    var curAddrText = request.term;
                                    for (var k = 0; k < unmatchedAddrExpressions.length; k++)
                                    {
                                        // if text in text box matches an unmatched co,
                                        // cancel search
                                        if (curAddrText.length >= unmatchedAddrExpressions[k].length
                                                &&
                                                curAddrText.toLowerCase().indexOf(unmatchedAddrExpressions[k], 0) == 0)
                                        {
                                            //alert(curAddrText + " matches " + unmatchedAddrExpressions[k] + " - canceling ajax");
                                            // stop the spinning
                                            $("#<%=txtAddress1.ClientID %>").removeClass("ui-autocomplete-loading");

                                            return false; // don't send if already unmatched
                                        }
                                    }

                                    // bring back the spinning
                                    $("#<%=txtAddress1.ClientID %>").addClass("ui-autocomplete-loading");
                                    return true;
                                },
                                success: function (data, textStatus, jqXHR)
                                {
                                    // if no result (companylist.aspx returns [{"id": "", "label": ""}])
                                    if (!!data && !!data[0])
                                    {
                                        // add term to unmatched companies
                                        if (data[0].id == "")
                                        {
                                            unmatchedAddrExpressions.push(request.term.toLowerCase());
                                            // stop the spinning
                                            $("#<%=txtAddress1.ClientID %>").removeClass("ui-autocomplete-loading");
                                        }

                                        response(data);
                                    }
                                },
                                complete: function ()
                                {
                                    //alert("ajax complete");
                                    $("#<%=txtAddress1.ClientID %>").removeClass("ui-autocomplete-loading");
                                },
                                error: function ()
                                {
                                    $("#<%=txtAddress1.ClientID %>").removeClass("ui-autocomplete-loading");
                                    alert("ajax error");
                                }
                            })
                        },
                        minLength: 1,
                        select: function (event, ui)
                        {
                            if (!!ui && !!ui.item)
                                setAddress(ui.item);
                        }
                    }); // end of address autocomplete

                    function setAddress(addrObj)
                    {
                        $("#<%=txtAddress2.ClientID %>").val(addrObj.address2);
                        $("#<%=txtCity.ClientID %>").val(addrObj.city);
                        $("#<%=txtState.ClientID %>").val(addrObj.state);
                        $("#<%=txtZip.ClientID %>").val(addrObj.postalCode);
                        $("#<%=txtCountry.ClientID %>").val(addrObj.country);
                        // call blur to get the time zone
                        $("#<%=txtState.ClientID %>").blur();
                    }

                    $("#<%=txtState.ClientID %>").focus(function ()
                    {
                        curState = $(this).val();
                    });

                    $("#<%=txtState.ClientID %>").blur(function ()
                    {
                        if ($(this).val() == curState)
                            return;

                        // set tz
                        $.ajax(
                        {
                            url: "./TimeZoneHelper.aspx",
                            dataType: "text",
                            data: { "state": $("#<%=txtState.ClientID %>").val().toLowerCase() },
                            success: function (ret)
                            {
                                try
                                {
                                    $("#<%=ddTimezones.ClientID %>").val(ret);
                                }
                                catch (e)
                                {
                                    // do nothing
                                }
                            }
                        });

                        curState = $(this).val();
                    });
                });
            });
    </script>

</head>
<body>
    <div id="container">
        <form id="form1" runat="server">
            <hc:HcHeader runat="server" ID="header1" />
            <hcc:ClientControlPanel runat="server" ID="ccp1" Visible="false" />
            <div id="<%=this.MainBodyOrMainBodyClients() %>">
    
                <h1>Client Profile - <asp:Label runat="server" ID="lblUser" /></h1>
               
                <%-- if new client, instructions --%>
                <asp:PlaceHolder runat="server" ID="phNewClientInfo"></asp:PlaceHolder>
                
                <table>
                    <tr>
                        <td class="tableLabelRightAligned" style="width: 120px;">First name:</td>
                        <td><asp:TextBox runat="server" ID="txtFirstname"></asp:TextBox><span class="asteriskRequiredField">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="reqFirstName" ControlToValidate="txtFirstname" ErrorMessage="<br />First name is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                    </tr>
                    <tr>
                        <td class="tableLabelRightAligned">Last name:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLastname"></asp:TextBox><span class="asteriskRequiredField">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="reqLastName" ControlToValidate="txtLastname" ErrorMessage="<br />Last name is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                    </tr>
                    <tr>
                        <td class="tableLabelRightAligned">Phone:</td>
                        <td><asp:TextBox runat="server" ID="txtPhone"></asp:TextBox><span class="asteriskRequiredField">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="reqPhone" ControlToValidate="txtPhone" ErrorMessage="<br />Phone number is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                    </tr>
                    <tr>
                        <td class="tableLabelRightAligned">Company:</td>
                        <td><asp:TextBox runat="server" ID="txtCompany"></asp:TextBox><asp:HiddenField runat="server" ID="hidCompanyId" Value="0" /><%-- place co id here --%></td>
                    </tr>
                    <tr>
                        <td class="tableLabelRightAligned">Billing address:</td>
                        <td><asp:TextBox runat="server" ID="txtAddress1"></asp:TextBox><span class="asteriskRequiredField">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="reqAddress1" ControlToValidate="txtAddress1" ErrorMessage="<br />Address is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /><asp:HiddenField runat="server" ID="hidAddressId" /><%-- place addr id here --%></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:TextBox runat="server" ID="txtAddress2"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tableLabelRightAligned">City:</td>
                        <td><asp:TextBox runat="server" ID="txtCity"></asp:TextBox><span class="asteriskRequiredField">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="reqCity" ControlToValidate="txtCity" ErrorMessage="<br />City is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                    </tr>
                    <tr>
                        <td class="tableLabelRightAligned">State/Province:</td>
                        <td><asp:TextBox runat="server" ID="txtState"></asp:TextBox><span class="asteriskRequiredField">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="reqState" ControlToValidate="txtState" ErrorMessage="<br />State/Province is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                    </tr>
                    <tr>
                        <td class="tableLabelRightAligned">Zip/Postal code:</td>
                        <td><asp:TextBox runat="server" ID="txtZip"></asp:TextBox><span class="asteriskRequiredField">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="reqZip" ControlToValidate="txtZip" ErrorMessage="Zip/Postal code is required." SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" /></td>
                    </tr>
                    <tr>
                        <td class="tableLabelRightAligned">Country:</td>
                        <td><asp:TextBox runat="server" ID="txtCountry">USA</asp:TextBox></td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tableLabelRightAligned" style="width: 120px;">Time zone:</td>
                        <td><asp:DropDownList runat="server" ID="ddTimezones" DataSourceID="odsTimeZones" DataTextField="DisplayName" DataValueField="Id" AppendDataBoundItems="true" onload="ddTimezones_Load"></asp:DropDownList><span class="asteriskRequiredField">*</span></td><%-- to do - js to show what time it is in selected zone--%>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="right" style="padding-top: 20px;"><asp:Button runat="server" 
                                ID="btnSubmit" Text="Update" onclick="btnSubmit_Click" /></td>
                    </tr>
                </table>
                
                <div><asp:ValidationSummary runat="server" ID="sum1" ShowMessageBox="false" ShowSummary="true" DisplayMode="BulletList" /></div>

                <asp:Panel runat="server" ID="pnlFootnotes" class="text footnote"><span class="asteriskRequiredField">*</span> indicates a required field.</asp:Panel>

            </div>
            <hc:HcFooter runat="server" ID="footer1" />
        </form>
    </div>

    <asp:ObjectDataSource 
        runat="server" 
        ID="odsTimeZones" 
        TypeName="HesterConsultants.AppCode.SiteUtils" 
        SelectMethod="AllSystemTimeZones"
        DataObjectTypeName="System.TimeZoneInfo">
    </asp:ObjectDataSource>
</body>
</html>
