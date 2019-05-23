<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.demos.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=Settings.Default.CompanyName %> - Demos</title>
    
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

        <div id="adPanelRight">
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
        <%--<script type="text/javascript"
        src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
        </script>--%>
    </div>

    <div id="mainbody">
    <h1>Demos <span style="color: Red;">(Removed)</span></h1>
    <%--<p>All our demos can be seen on our YouTube channel at <a href="http://www.youtube.com/user/HesterConsultants">http://www.youtube.com/user/HesterConsultants</a>.</p>--%>

    <h2 class="demoHeading">Advanced Sorting in Excel</h2>
    <div class="videoBox" style="width: 480px;">
    <object width="480" height="385"><param name="movie" value="<%--http://www.youtube.com/v/4xwLpdpYzzg?fs=1&amp;hl=en_US--%>" /><param name="allowFullScreen" value="true" /><param name="allowscriptaccess" value="always" /><embed src="<%--http://www.youtube.com/v/4xwLpdpYzzg?fs=1&amp;hl=en_US--%>" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="480" height="385"></embed></object>
    </div>
    <div class="text videoCaption">Sort groups of rows, not individual rows. It's tricky!</div>
    <div class="text">Full size video: http://www.youtube.com/watch?v=... <%--<a href="http://www.youtube.com/watch?v=4xwLpdpYzzg">http://www.youtube.com/watch?v=4xwLpdpYzzg</a>--%></div>

    <h2 class="demoHeading">Advanced Data Manipulation in Excel</h2>
    <div class="videoBox" style="width: 480px;">
    <object width="480" height="385"><param name="movie" value="<%--http://www.youtube.com/v/56PNX12JNAM?fs=1&amp;hl=en_US--%>" /><param name="allowFullScreen" value="true" /><param name="allowscriptaccess" value="always" /><embed src="<%--http://www.youtube.com/v/56PNX12JNAM?fs=1&amp;hl=en_US--%>" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="480" height="385"></embed></object>
    </div>
    <div class="text videoCaption">Instantly split Bates number ranges into separate columns for beginning number and ending number.</div>
    <div class="text">Full size video: http://www.youtube.com/watch?v=... <%--<a href="http://www.youtube.com/watch?v=56PNX12JNAM">http://www.youtube.com/watch?v=56PNX12JNAM</a>--%></div>

    <h2 class="demoHeading">Highlight and Bookmark Words in a PDF File</h2>
    <div class="videoBox" style="width: 480px;">
    <object width="480" height="385"><param name="movie" value="<%--http://www.youtube.com/v/osnhDKZZOXE?fs=1&amp;hl=en_US--%>" /><param name="allowFullScreen" value="true" /><param name="allowscriptaccess" value="always" /><embed src="<%--http://www.youtube.com/v/osnhDKZZOXE?fs=1&amp;hl=en_US--%>" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="480" height="385"></embed></object>
    </div>
    <div class="text videoCaption">Instantly highlight words and phrases in a lengthy PDF file.</div>
    <div class="text">Full size video: http://www.youtube.com/watch?v=... <%--<a href="http://www.youtube.com/watch?v=osnhDKZZOXE">http://www.youtube.com/watch?v=osnhDKZZOXE</a>--%></div>

    <h2 class="demoHeading">Advanced Formatting in Excel</h2>
    <div class="videoBox" style="width: 480px;">
    <object width="480" height="385"><param name="movie" value="<%--http://www.youtube.com/v/68E9nk_wMY4?fs=1&amp;hl=en_US--%>" /><param name="allowFullScreen" value="true" /><param name="allowscriptaccess" value="always" /><embed src="<%--http://www.youtube.com/v/68E9nk_wMY4?fs=1&amp;hl=en_US--%>" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="480" height="385"></embed></object>
    </div>
    <div class="text videoCaption">Highlight individual words in a lengthy spreadsheet.</div>
    <div class="text">Full size video: http://www.youtube.com/watch?v=... <%--<a href="http://www.youtube.com/watch?v=68E9nk_wMY4">http://www.youtube.com/watch?v=68E9nk_wMY4</a>--%></div>
    </div>
    <hc:HcFooter ID="HcFooter1" runat="server" />
    </form>
    </div>
	<script src="/scripts/googleAnalytics.js"></script>
</body>
</html>
