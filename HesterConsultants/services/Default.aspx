<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.services.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Services</title>
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

    <div id="mainbody" style="width: auto;">
    <h1>Our Services</h1>
    <div class="servicePanel">
        <div class="servicePanelLeft">
            <div class="servicePanelService">
                <h2 class="serviceHeading">Data Processing</h2>
                <table class="indent40px feeTable">
                    <tr>
                        <td><strong>Service</strong></td>
                        <td class="dollarFee"><strong>Fee*</strong></td>
                    </tr>
                </table>
                <h3 class="serviceHeading indent10px">PDF Conversions to Word or Excel</h3>
                <h4 class="indent20px">1. Formatted<sup class="asterisk">(1)</sup> to Your Template</h4>
                <table class="indent40px feeTable">
                    <tr>
                        <td>Normal<sup class="asterisk">(2)</sup> PDF</td>
                        <td class="dollarFee">$2.50/page</td>
                    </tr>
                    <tr>
                        <td>Image<sup class="asterisk">(2)</sup> PDF</td>
                        <td class="dollarFee">$5.00/page</td>
                    </tr>
                </table>

                <h4 class="indent20px">2. Unformatted<sup class="asterisk" style="font-weight: normal;">(1)</sup></h4>
                <table class="indent40px feeTable">
                    <tr>
                        <td>Normal<sup class="asterisk">(2)</sup> PDF</td>
                        <td class="dollarFee">$1.50/page</td>
                    </tr>
                    <tr>
                        <td>Image<sup class="asterisk">(2)</sup> PDF</td>
                        <td class="dollarFee">$4.00/page</td>
                    </tr>
                </table>
    
                <h4 class="indent20px">PDF Conversions - Other Charges</h4>
                <table class="feeTable indent40px">
                    <tr>
                        <td>Additional charge for financial tables, complex layouts or poor quality image</td>
                        <td class="dollarFee">$2.00/page</td>
                    </tr>
                    <tr>
                        <td>Additional charge for extensive proofreading required (e.g., a long list of numbers)</td>
                        <td class="dollarFee">$2.00/page</td>
                    </tr>
                    <tr>
                        <td>Reduced charge if no proofreading required<sup class="asterisk">(3)</sup></td>
                        <td class="dollarFee">(20%)</td>
                    </tr>
                </table>
            </div>
            <div class="servicePanelService">
                <h3 class="serviceHeading indent10px">Text Editing</h3>
                <table class="indent40px feeTable">
                    <tr>
                        <td>Using your scanned markup</td>
                        <td class="right">$3.00/page of markup</td>
                    </tr>
                </table>
            </div>
            <div class="servicePanelService">
                <h3 class="serviceHeading indent10px">Other Data Processing</h3>
                <p class="indent40px">
                    <a href="<%=Settings.Default.ContactUrl %>">Contact us</a> for an estimate for your project.
                </p>
            </div>
            <div class="servicePanelService marginTop20px">
                <h2 class="serviceHeading">Development, Programming and Technical Consulting</h2>
                <p class="indent20px"><a href="<%=Settings.Default.ContactUrl %>">Contact <%=Settings.Default.CompanyContactName %></a> for an estimate for your project.</p>
            </div>
        </div>

        <table>
            <tr>
                <td>
<%--        <h1>&nbsp;</h1><%-- placeholder --%>

        <h3>Notes</h3>
        <p><sup class="asterisk bold">(1)</sup> "Formatted" means formatted to match a sample document that you provide.</p> 
            <p>"Unformatted" means:</p>
        <ul>
            <li>plain text</li>
            <li>blocks (paragraphs) are preserved</li>
            <li>tables are reduced to paragraphs</li>
            <li>hard numbering, not automatic</li>
        </ul>
        <p>Unformatted is a good choice, e.g., if the purpose of the conversion is merely to run a redline between two PDFs.</p>
        <p><sup class="asterisk bold">(2)</sup> A "Normal" (or, "Formatted Text &amp; Graphics") PDF is created from a computer program (i.e., printed to PDF). An "Image" PDF is created by scanning paper. Normal PDFs are quicker to convert.</p>
        <p><sup class="asterisk bold">(3)</sup> Conversions will be less accurate without proofreading. The software used to convert from PDF to other formats makes use of "best-guess" algorithms in many instances, such as when recognizing text from an image, or when attempting to duplicate a layout.</p>
        <h3>No Surprises</h3>
        <p>We will let you know the <em>exact charge</em> for a PDF conversion or text editing project before starting work.</p>
        <div><a href="/clients/" style="font-size: x-large;">Get started: Client login</a></div>
                </td>
            </tr>
        </table>

        <br style="clear: both;" />
   </div>
    

        <div class="text">* Texas clients add 6.25% (8.25% for Austin clients) sales and use tax for data processing and some other services.</div>
    </div>
    <hc:HcFooter ID="HcFooter1" runat="server" />
    </form>
    </div>
	<script src="/scripts/googleAnalytics.js"></script>
</body>
</html>
