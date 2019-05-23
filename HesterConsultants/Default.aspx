<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=HesterConsultants.Properties.Settings.Default.CompanyName %></title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" media="Screen" />
    <link rel="stylesheet" type="text/css" href="/style/hc-mobile.css" media="handheld" />
    <link  rel="stylesheet" type="text/css" href="/style/hc-small-device.css" media="only screen and (max-device-width: 480px)" />
	<link rel="stylesheet" type="text/css" href="/style/hcblue/jquery-ui-1.8.11.custom.css" />
	<!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->

	<style type="text/css">
        .ui-dialog-titlebar-close
        {
            /* hide the close button on the dialogs */
            display: none;
        }
	</style>
	
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui-1.8.11.custom.min.js"></script>

    <% = SiteUtils.CheckForClientValsScript(this) %>

    <script type="text/javascript">
    	$(document).ready(function ()
    	{
    		$("#pnlAlert").dialog(
                    {
                    	autoOpen: true,
                    	title: "<%=Settings.Default.CompanyName %>",
                    	width: 500,
                    	height: 250,
                    	modal: true,
                    	closeOnEscape: false,
                    	buttons:
                        {
                        	"OK": function ()
                        	{
                        		$(this).dialog("close");
                        	}
                        }
                    });

    		//$("#pnlAlert").dialog("open");
    	});
//            $(".homeSplash").mouseover(function ()
//            {
//                $(this).find("a").css("color", "#f37d01");
//                $(this).css("cursor", "hand");
//            });

//            $(".homeSplash").mouseout(function ()
//            {
//                $(this).find("a").css("color", "#0080ff");
//                $(this).css("cursor", "pointer");
//            });

//            $(".homeSplash").click(function (e)
//            {
//                e.preventDefault();
//                window.location = $(this).find("a").attr("href");
//            });
//        });

	</script>
</head>
<body>
    <div id="container">
    <form id="form1" runat="server">
    <hc:HcHeader ID="header1" runat="server" />

   <%-- <div id="adPanelRight">
        <div class="adHeading">Sponsors:</div>
        <!-- ad unit -->
        <script type="text/javascript"><!--
            google_ad_client = "pub-2996570680838598";
            /* 300x250, created 5/19/10 */
            google_ad_slot = "0489344119";
            google_ad_width = 300;
            google_ad_height = 250;
        // -->
        </script> 
        <script type="text/javascript"
        src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
        </script> 
    </div>--%>
    
    <div id="mainbody">
        <%-- <h1>Welcome to <%=Settings.Default.CompanyName %></h1> --%>
        <asp:Panel runat="server" ID="pnlLogin">
            <table>
                <tr>
                    <td style="border-right: solid 1px #cccccc; padding-right: 30px; padding-bottom: 20px;">
                        <h1 style="margin-bottom: 10px;"><a href="/login/" style="text-decoration: none;">Client Login</a></h1>
                        <div class="indent20px">
                            <a href="/login/"><img src="/style/hcblue/images/right-arrow.png" width="12" height="12" alt="Login" style="vertical-align: middle;" /></a>&nbsp;&nbsp;<a href="/login/">Secure Login</a>
                        </div>
                                
                        <%--<asp:Login ID="Login1" 
                            runat="server" 
                            CssClass="loginTable" 
                            TitleText="" 
                            UserNameLabelText="Email:" 
                            LabelStyle-CssClass="tableLabelRightAligned" 
                            RememberMeText="Remember email next time" 
                            DestinationPageUrl="/login/Handler.aspx" 
                            LoginButtonText="Login">
                        </asp:Login>--%>
                    </td>
                    <td style="padding-left: 30px;">
                        <h1 style="margin-bottom: 10px;"><a href="/NewClient/" style="text-decoration: none;">New Users</a></h1>
                        <div class="indent20px">
                            <a href="/NewClient/"><img src="/style/hcblue/images/right-arrow.png" width="12" height="12" alt="Create new account" style="vertical-align: middle;" /></a>&nbsp;&nbsp;<a href="/NewClient/">Create Your Free Account</a>
                        </div>
                                
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- <table id="tblLogin">
            <tr>
                <td style="padding-right: 10px;"><div class="bigLogin"><span class="innerBigLogin" ><a href="/login/">Client Login</a></span></div></td>
                <td><div class="bigLogin"><span class="innerBigLogin"><a href="/NewClient/">New Client Signup</a></span></div></td>
            </tr>
        </table> --%>
        
        
        <%--<div class="homeSplash">
        <h1>Hester Consultants<br />Secure Online Data Processing</h1>
        <div>PDF Conversions: Convert PDF to Word, Excel, etc.</div>
        <div>Low, per-page rates.</div>
        <div>Guaranteed accuracy.</div>
        <div><a href="/services/">Click here for rates</a></div>
        

        </div>--%>
		<div class="news">
			<p><span class="dateline">October 13, 2012</span> – View our <a href="/client-experience/">Client Experience Videos and Slideshow</a> and see how easy it is to get your legal support work done through our online service!</p>
		</div>
        <%--<div class="news">
            <h1>News</h1>
            <p><span class="dateline">May 24, 2011</span> – View our <a href="/client-experience/">Client Experience Videos and Slideshow</a> and see how easy it is to get your legal support work done through our online service!</p>
            <p><span class="dateline">May 5, 2011</span> – <%=Settings.Default.CompanyNameShort %> is pleased to 
                announce our new secure online data processing services. We offer per-page 
                pricing for common data processing tasks such as converting PDF files to MS 
                Office formats. All client information and file transfers are protected by 
                industry standard transport layer security.</p>
        
            <p>We think you will enjoy our secure online client experience. View our <a href="/services/">Services</a> page for pricing info, and <a href="/NewClient">sign up</a> for your account today!</p>
            <p><span class="dateline">January 6, 2011</span> &#150; Our <a href="<%=Settings.Default.ContactUrl %>">Contact</a> page now offers secure file uploading. Send messages and attachments to us with SSL-guaranteed security!</p>
            <p><span class="dateline">September 17, 2010</span> &#150; New video demos of our solutions: <a href="/demos/">Click here!</a></p>
        </div>--%>
        
        <h1>What We Do</h1>
        <p><%=Settings.Default.CompanyNameShort %> specializes in application development and support services, web and database development, and document analysis. We also provide fast, accurate and secure data processing services. We have extensive experience in the legal field.</p>
        <p>We have strong backgrounds as both programmers and legal professionals. Solutions from our professionals have been put in place at some of the world&#39;s largest law firms.</p>
            
            <p>Our services include:</p>
            <ul>
                <li>Secure online data processing</li>
                <li>Microsoft Office development and support</li>
                <li>Website development</li>
                <li>Windows application development</li>
            </ul>
            <p>Plus, we are flexible enough to take on specialized projects of many other types.</p>
            <p><a href="/contact/">Contact us</a> regarding your project needs.</p>
            <%--<p><asp:Button runat="server" ID="btnContact" OnClick="RedirectToContact" Text="&nbsp;Contact us regarding your project needs.&nbsp;" /></p>--%>
    </div>

    <div id="certs">
        <div class="address">
        <p><%=Settings.Default.CompanyName %><%--<br />
        <%=Server.HtmlDecode(Settings.Default.CompanyAddress) %>--%></p>
        
        <p><span class="nameTitle"><%=Settings.Default.CompanyContactName %><br />
        <%=Settings.Default.CompanyContactTitle %></span><br />
        <a href="mailto:<%=Settings.Default.CompanyContactEmail %>"><%=Settings.Default.CompanyContactEmail %></a><br />
        <%=Settings.Default.CompanyContactPhone %><%-- | <a href="mailto:5125600435@txt.att.net">Text via Email</a>--%></p>
        </div>
        <span class="noWrap"><img src="/images/mcad_120.png" width="120" height="56" style="vertical-align: middle;" alt="Microsoft Certified Application Developer" />&nbsp;&nbsp;&nbsp;&nbsp;<img src="/images/mos-master-2007.png" width="242" height="72" style="vertical-align: middle;" alt="Microsoft Office Master - Office 2007" />&nbsp;&nbsp;&nbsp;&nbsp;<img src="/images/mos-master-2003.png" width="369" height="72" style="vertical-align: middle;" alt="Microsoft Office Master - Office 2003" /></span>
        </div>
        
        <%--<div id="adPanelBottom">
            <script type="text/javascript"><!--
                google_ad_client = "pub-2996570680838598";
                /* 728x15, created 5/19/10 */
                google_ad_slot = "6040745460";
                google_ad_width = 728;
                google_ad_height = 15;
                // -->
            </script>
            <script type="text/javascript"
                src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>
        </div>--%>

		<div id="pnlAlert">
			<p><%=Settings.Default.CompanyNameShort %> is not accepting new assignments at this time. The website remains active for demonstration purposes only.</p>
			<p>For more information, contact <a href="mailto:<%=Settings.Default.CompanyContactEmail %>"><%=Settings.Default.CompanyContactEmail %></a>.</p>
		</div>
    <hc:HcFooter ID="HcFooter1" runat="server" />
    </form>
</div>
	<script src="/scripts/googleAnalytics.js"></script>
</body>
</html>
