<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HesterConsultants.client_experience.Default" %>

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
	<link rel="shortcut icon" href="/favicons/h-2012-09-08.ico" />

	<style type="text/css">
        div#mainbody
        {
            width: 100%;
        }
        #video-wrapper
        {
            width: 850px;
            margin: auto;
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

    <% = SiteUtils.CheckForClientValsScript(this) %>

    
    <script type="text/javascript">
        $(document).ready(function ()
        {
            var part1Summary = "<ul><li>Part 1 demonstrates our easy to use job submission form.</li> <li>Our form reflects our commitment to user-friendly design.</ul>";
            var part2Summary = "<ul><li>Part 2 demonstrates picking up a completed job.</li> <li>Note that we provide your files and notes up front, as well as our work history and fees.</li></ul>";

            var flashHtml1 = "<object width=\"800\" height=\"600\"> "
                + "<param name=\"movie\" value=\"http://fpdownload.adobe.com/strobe/FlashMediaPlayback.swf\" /> "
                + "<param name=\"flashvars\" value=\"src=http%3A%2F%2Fwww.hesterconsultants.com%2Fclient-experience%2Fvideos%2Fclient-experience-submit-job.flv&controlBarMode=docked&poster=.%2Fvideos%2Fclient-experience-submit-job.png\" /> "
                + "<param name=\"allowFullScreen\" value=\"true\" /> "
                + "<param name=\"allowscriptaccess\" value=\"always\" /> "
                + "<embed src=\"http://fpdownload.adobe.com/strobe/FlashMediaPlayback.swf\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" width=\"800\" height=\"600\" flashvars=\"src=http%3A%2F%2Fwww.hesterconsultants.com%2Fclient-experience%2Fvideos%2Fclient-experience-submit-job.flv&controlBarMode=docked&poster=.%2Fvideos%2Fclient-experience-submit-job.png\"></embed> "
            + "</object>";

            var flashHtml2 = "<object width=\"800\" height=\"600\"> "
                + "<param name=\"movie\" value=\"http://fpdownload.adobe.com/strobe/FlashMediaPlayback.swf\" /> "
                + "<param name=\"flashvars\" value=\"src=http%3A%2F%2Fwww.hesterconsultants.com%2Fclient-experience%2Fvideos%2Fclient-experience-pick-up-job.flv&controlBarMode=docked&poster=.%2Fvideos%2Fclient-experience-pick-up-job.png\" /> "
                + "<param name=\"allowFullScreen\" value=\"true\" /> "
                + "<param name=\"allowscriptaccess\" value=\"always\" /> "
                + "<embed src=\"http://fpdownload.adobe.com/strobe/FlashMediaPlayback.swf\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" width=\"800\" height=\"600\" flashvars=\"src=http%3A%2F%2Fwww.hesterconsultants.com%2Fclient-experience%2Fvideos%2Fclient-experience-pick-up-job.flv&controlBarMode=docked&poster=.%2Fvideos%2Fclient-experience-pick-up-job.png\"></embed> "
            + "</object>";

            $("#summary").html(part1Summary);
            $("#video-wrapper").html(flashHtml1);

            $("#part1-a").click(function (e)
            {
                e.preventDefault();
                $("#part2-text-only").hide();
                $("#part2-a").show();
                $("#part1-a").hide();
                $("#part1-text-only").show();
                $("#summary").html(part1Summary);
                $("#video-wrapper").html(flashHtml1);
            });

            $("#part2-a").click(function (e)
            {
                e.preventDefault();
                $("#part1-text-only").hide();
                $("#part1-a").show();
                $("#part2-a").hide();
                $("#part2-text-only").show();
                $("#summary").html(part2Summary);
                $("#video-wrapper").html(flashHtml2);
            });
        });
    </script>
</head>
<body>
    <div id="container">
    <form id="form1" runat="server">
    <hc:HcHeader ID="header1" runat="server" />

    <div id="mainbody">
        <h1><%=Settings.Default.CompanyNameShort  %> - Client Experience Videos</h1>
        <p>These short videos demonstrate the experience of submitting a job online, and picking up a completed job.</p>

        <p><a id="part1-a" href="" style="display: none;">Part 1 - Job Submission</a><span id="part1-text-only">Part 1 - Job Submission</span> | <a id="part2-a" href="">Part 2 - Job Pickup</a><span id="part2-text-only" style="display: none;">Part 2 - Job Pickup</span> | <a href="./Slideshow.aspx">Still Image Slideshow</a></p>

        <div id="summary" class="marginBottom20px"></div>

        <div id="video-wrapper"></div>
    </div>

        
    <hc:HcFooter ID="HcFooter1" runat="server" />
    </form>
</div>
</body>
</html>
