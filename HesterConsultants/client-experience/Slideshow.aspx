<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Slideshow.aspx.cs" Inherits="HesterConsultants.client_experience.Slideshow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=HesterConsultants.Properties.Settings.Default.CompanyName %></title>
    <link rel="stylesheet" type="text/css" href="/style/hc.css" media="Screen" />
    <link rel="stylesheet" type="text/css" href="/style/hc-mobile.css" media="handheld" />
    <link  rel="stylesheet" type="text/css" href="/style/hc-small-device.css" media="only screen and (max-device-width: 480px)" />
    <!--[if lt IE 8]>
    <link rel="stylesheet" type="text/css" href="/style/hc-ie7.css" />
    <![endif]-->
    <link rel="stylesheet" type="text/css" href="/style/twilight/twilight.css" />
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<style type="text/css">
        div#mainbody
        {
            width: 100%;
        }
        #caption-wrapper
        {
            width: 100%;
            margin-top: 20px;
            text-align: center;
        }
        .caption-span
        {
            border-top: solid 1px #666666;
        }
        .ui-effects-transfer
        {
            border: solid 1px #0080ff;
        }
    </style>
    <script type="text/javascript" src="/scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui-1.8.11.custom.min.js"></script>
    <script type="text/javascript" src="/scripts/twilight/twilight.min.js"></script>

    <% = SiteUtils.CheckForClientValsScript(this) %>

    <script type="text/javascript">
        var captions = ["Easy to use New Work Request form",
                        "Options available for conversion job",
                        "Easy to use dropdown calendar for due date",
                        "Client home page showing newly completed jobs at top",
                        "Job pickup page, with notes, files, our work history, and other job info"];
    </script>
    
    <script type="text/javascript">
        $(document).ready(function ()
        {
            // init:
            $(".arrow-left").hide();
            $("#caption").html("<span class=\"caption-span\">" + captions[0] + "</span>");
            //$(".twilight-box").effect("transfer", { to: "#caption" });

            $('.twilight-show').twilight(
            {
                outOpacity: .4,
                overOpacity: .6,
                slideCompleteCallback: function (slideNumber, direction)
                {
                    var captionNum = (slideNumber >= 0 ? slideNumber % 5
                                        : 5 + (slideNumber % 5));

                    if (captionNum == 4)
                        $(".arrow-right").hide();
                    else
                        $(".arrow-right").show();

                    if (captionNum == 0)
                        $(".arrow-left").hide();
                    else
                        $(".arrow-left").show();

                    $("#caption").html("<span class=\"caption-span\">" + captions[captionNum] + "</span>");
                }
            });
        });
    </script>
</head>
<body>
    <div id="container">
    <form id="form1" runat="server">
    <hc:HcHeader ID="header1" runat="server" />

    <div id="mainbody">
        <h1><%=Settings.Default.CompanyNameShort  %> - Client Experience Slideshow</h1>
        <p>Click the arrows on the sides of the image to navigate between slides. | <a href="./">Watch Videos</a></p>
        <div class="twilight-wrapper">
	        <div class="slide-wrapper">
		        <div class="twilight-show">
			        <div class="twilight-box">
				        <div><img src="./images/new-work-request.png" width="726" height="531" alt="" /></div>
				        <div><img src="./images/conversion-options.png" width="726" height="531" alt="" /></div>
				        <div><img src="./images/calendar.png" width="726" height="531" alt="" /></div>
				        <div><img src="./images/client-home.png" width="726" height="531" alt="" /></div>
				        <div><img src="./images/job-pickup.png" width="726" height="531" alt="" /></div>
			        </div>
			        <div class="arrow-left"><img src="./images/arrow-left.png" alt="" /></div>
			        <div class="arrow-right"><img src="./images/arrow-right.png" alt="" /></div>
		        </div>
	        </div>
            <div id="caption-wrapper"><div id="caption"></div></div>
        </div>
    </div>

        
    <hc:HcFooter ID="HcFooter1" runat="server" />
    </form>
</div>
</body>
</html>
