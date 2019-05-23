// ************************************
// ***** hc_client_vals.js
// ************************************

var hc =
{
    scriptName: 'hc_client_vals',
    br: null,
    pl: null,
    sc: null,
    sw: null,
    sh: null,
    tzo: null,
    UPDATER_SCRIPT: "/scripts/SessionValuesUpdater.aspx",
    ranUpdate: null
};

// ************************************
// ***** Constants for db field ids:
// ************************************

hc.NN2 = 1;
hc.NN3 = 2;
hc.NN4 = 3;
hc.NN6 = 4;
hc.NN7 = 18;
hc.MSIE2 = 5;
hc.MSIE3 = 6;
hc.MSIE4 = 7;
hc.MSIE4_5 = 17;
hc.MSIE5_0 = 8;
hc.MSIE5_5 = 9;
hc.MSIE5_OTHER = 22;
hc.MSIE6 = 10;
hc.MSIE7 = 23;
hc.MSIE8 = 25;
hc.OPERA = 11;
hc.LYNX = 12;
hc.OMNIWEB = 13;
hc.AOL = 14;
hc.MSN = 15;
hc.BROWSER_OTHER = 16;
hc.FIREFOX = 19;
hc.MOZILLA = 20;
hc.SAFARI = 21;
hc.CHROME = 24;

hc.WINDOWS_95 = 1;
hc.WINDOWS_98 = 2;
hc.WINDOWS_ME = 3;
hc.WINDOWS_NT4 = 4;
hc.WINDOWS_2000 = 5;
hc.WINDOWS_XP = 6;
hc.WINDOWS_2003 = 21;
hc.WINDOWS_VISTA = 22;
hc.WINDOWS_7 = 23;
hc.WINDOWS_2008 = 24; // to do - this has same version as vista (6.0), or r2 is same as 7 (6.1)
hc.MAC_OS7 = 7;
hc.MAC_OS8 = 8;
hc.MAC_OS9 = 9;
hc.MAC_OSX = 10;
hc.MAC_UNSPECIFIED = 20;
hc.UNIX = 12;
hc.LINUX = 13;
hc.BEOS = 14;
hc.WINDOWS_CE = 15;
hc.PALMOS = 16;
hc.WINDOWS_3_1 = 18;
hc.PLATFORM_OTHER = 19;

//hc.S_640 = 1;
//hc.S_800 = 2;
//hc.S_1024 = 3;
//hc.S_1152 = 4;
//hc.S_1280_1024 = 5;
//hc.S_1280_960 = 6;
//hc.S_1600 = 7;
//hc.S_OTHER = 8;
//hc.S_1366 = 9;
//hc.S_1400 = 10;
//hc.S_1440 = 11;
//hc.S_1680 = 12;
//hc.S_1920 = 13;
//hc.S_2560_1024 = 14;
//hc.S_2560_1600 = 15;

//hc.COL_8 = 1;
//hc.COL_16 = 2;
//hc.COL_24 = 3;
//hc.COL_32 = 4;
//hc.COL_OTHER = 5;
hc.SCREEN_WIDTH_OTHER = 0;
hc.SCREEN_HEIGHT_OTHER = 0;
hc.SCREEN_COLOR_DEPTH_OTHER = 0;

// ------------- getBrowserData () -------------------------------------------- //
// ---------------------------------------------------------------------------- //
// ---------------------------------------------------------------------------- //

// these global so can be accessed on calling page:
//var br;
//var pl;
//var sc;
//var ss;
//var tzo;

hc.getBrowserData =
function()
{
    // get:
    //  - browser; 			br
    //  - platform; 			pl
    //  - screen size; 		scrSize
    //  - screen color depth; 	scrColor
    //  - time zone offset; 	tzo

    var na;
    var ua;
//    var sw;
//    var sh;

    ua = navigator.userAgent.toLowerCase();
    na = navigator.appName.toLowerCase();

    // browser: -----------------------------
    if (navigator)
    {
        if (na == "microsoft internet explorer") na = "msie";
        na += " " + navigator.appVersion.toLowerCase();

        // shorten:
        if (na.indexOf("msie 8") != -1) this.br = this.MSIE8;
        else if (na.indexOf("msie 7") != -1) this.br = this.MSIE7;
        else if (na.indexOf("msie 6") != -1) this.br = this.MSIE6;
        else if (na.indexOf("msie 5.5") != -1) this.br = this.MSIE5_5;
        else if (na.indexOf("msie 5.0") != -1) this.br = this.MSIE5_0;
        else if (na.indexOf("msie 5") != -1) this.br = this.MSIE5_OTHER;
        else if (na.indexOf("msie 4.5") != -1) this.br = this.MSIE4_5;
        else if (na.indexOf("msie 4") != -1) this.br = this.MSIE4;
        else if (na.indexOf("msie 3") != -1) this.br = this.MSIE3;
        else if (na.indexOf("chrome") != -1) this.br = this.CHROME;
        else if (ua.indexOf("firefox") != -1) this.br = this.FIREFOX;
        else if (ua.indexOf("safari") != -1) this.br = this.SAFARI;
        else if (ua.indexOf("opera") != -1) this.br = this.OPERA; // ua
        else if (ua.indexOf("omniweb") != -1) this.br = this.OMNIWEB;
        else if (ua.indexOf("lynx") != -1) this.br = this.LYNX;
        else if (na.indexOf("aol") != -1) this.br = this.AOL;
        else if (na.indexOf("msn") != -1) this.br = this.MSN;
        else if (ua.indexOf("netscape/7") != -1) this.br = this.NN7;
        else if (na.indexOf("netscape6") != -1 || na.indexOf("netscape 6") != -1
  				|| na.indexOf("netscape 5") != -1) this.br = this.NN6;
        else if (ua.indexOf("gecko") != -1) this.br = this.MOZILLA;
        else if (na.indexOf("netscape 4") != -1) this.br = this.NN4;
        else if (na.indexOf("netscape 3") != -1) this.br = this.NN3;
        else if (na.indexOf("netscape 2") != -1) this.br = this.NN2;
        else this.br = BROWSER_OTHER;

        // platform: -----------------------------
        if (ua.indexOf("nt 5.1") != -1 || ua.indexOf("windows xp") != -1 || ua.indexOf("winxp") != -1)
            this.pl = this.WINDOWS_XP;
        else if (ua.indexOf("nt 6.1") != -1 || ua.indexOf("windows 7") != -1)
            this.pl = this.WINDOWS_7;
        else if (ua.indexOf("nt 6.0") != -1 || ua.indexOf("windows vista") != -1)
            this.pl = this.WINDOWS_VISTA;
        else if (ua.indexOf("nt 5.2") != -1)
            this.pl = this.WINDOWS_2003;
        else if (ua.indexOf("nt 5") != -1 || ua.indexOf("windows 2000") != -1)
            this.pl = this.WINDOWS_2000;
        else if (ua.indexOf("windows nt") != -1 && navigator.platform == "Win32")
            this.pl = this.WINDOWS_NT4;
        else if (ua.indexOf("windows me") != -1 || ua.indexOf("winme") != -1)
            this.pl = this.WINDOWS_ME;
        else if (ua.indexOf("windows 98") != -1 || ua.indexOf("win98") != -1)
            this.pl = this.WINDOWS_98;
        else if (ua.indexOf("windows 95") != -1 || ua.indexOf("win95") != -1)
            this.pl = this.WINDOWS_95;
        else if (ua.indexOf("osx") != -1 || ua.indexOf("os x") != -1)
            this.pl = this.MAC_OSX;
        else if (ua.indexOf("os9") != -1)
            this.pl = this.MAC_OS9;
        else if (ua.indexOf("os8") != -1)
            this.pl = this.MAC_OS8;
        else if (ua.indexOf("mac") != -1)
            this.pl = this.MAC_UNSPECIFIED;
        else if (ua.indexOf("x11") != -1 || ua.indexOf("sunos") != -1)
            this.pl = this.UNIX;
        else if (ua.indexOf("linux") != -1)
            this.pl = this.LINUX;
        else if (ua.indexOf("windows 3") != -1)
            this.pl = this.WINDOWS_3_1;
        else this.pl = this.PLATFORM_OTHER;
    }

    // screen size & color: -----------------------------
    if (!!screen)
    {
        this.sw = screen.width;
        this.sh = screen.height;

        if (!!screen.colorDepth)
            this.sc = screen.colorDepth;
        else
            this.sc = this.SCREEN_COLOR_DEPTH_OTHER;
    }
    else
    {
        this.sw = this.SCREEN_WIDTH_OTHER;
        this.sh = this.SCREEN_HEIGHT_OTHER;
        this.sc = this.SCREEN_COLOR_DEPTH_OTHER;
    }
    //        if (sw == 640 && sh == 480) this.ss = this.S_640;
    //        else if (sw == 800 && sh == 600) this.ss = this.S_800;
    //        else if (sw == 1024 && sh == 768) this.ss = this.S_1024;
    //        else if (sw == 1152 && sh == 864) this.ss = this.S_1152;
    //        else if (sw == 1280 && sh == 1024) this.ss = this.S_1280_1024;
    //        else if (sw == 1280 && sh == 960) this.ss = this.S_1280_960;
    //        else if (sw == 1600 && sh == 1200) this.ss = this.S_1600;
    //        else if (sw == 1366 && sh == 768) this.ss = this.S_1366;
    //        else if (sw == 1400 && sh == 1050) this.ss = this.S_1400;
    //        else if (sw == 1440 && sh == 900) this.ss = this.S_1440;
    //        else if (sw == 1680 && sh == 1050) this.ss = this.S_1680;
    //        else if (sw == 1920 && sh == 1200) this.ss = this.S_1920;
    //        else if (sw == 2560 && sh == 1024) this.ss = this.S_2560_1024;
    //        else if (sw == 2560 && sh == 1600) this.ss = this.S_2560_1600;
    //        else this.ss = this.S_OTHER;

    //        if (screen.pixelDepth) this.sc = screen.pixelDepth;
    //        else if (screen.colorDepth) this.sc = screen.colorDepth;

    //        if (this.sc == 8) this.sc = this.COL_8;
    //        else if (this.sc == 16) this.sc = this.COL_16;
    //        else if (this.sc == 24) this.sc = this.COL_24;
    //        else if (this.sc == 32) this.sc = this.COL_32;
    //        else this.sc = this.COL_OTHER;
    //    }
//    else
//    {
//        this.ss = this.S_OTHER;
//        sc = this.COL_OTHER;
//    }

    // time zone offset: -----------------------------
    // find highest timezoneoffset (the base offset)
    // by looping through months
    // (js timezoneoffsets are always lower than base in dst)
    var highestTzo = -1000;
    var currentTzo = new Date().getTimezoneOffset();
    for (var m = 0; m < 12; m++)
    {
        var today = new Date();
        today.setMonth(m);
        today.setDate(1);
        var curTzo = today.getTimezoneOffset();
        if (curTzo > highestTzo)
            highestTzo = curTzo;
    }
    this.tzo = highestTzo;
}

hc.callUpdater =
function ()
{
    // prototype:
    //    var qs = "?br=" + this.br
    //        + "&pl=" + this.pl
    //        + "&sc=" + this.sc
    //        + "&sw=" + this.sw
    //        + "&sh=" + this.sh
    //        + "&tzo=" + this.tzo
    //        + "&caller=" + this.scriptName;

    // jquery:
    var qs = "br=" + this.br
        + "&pl=" + this.pl
        + "&sc=" + this.sc
        + "&sw=" + this.sw
        + "&sh=" + this.sh
        + "&tzo=" + this.tzo
        + "&caller=" + this.scriptName;

    // relies on jQuery ---- formerly Prototype ---- for Ajax
    //    var Prototype;
    //if (typeof Prototype == 'undefined') 
    //{
    //    throw new Error("Prototype not loaded.");
    if (typeof jQuery == 'undefined')
    {
        //alert("not loaded");
        throw new Error("jQuery not loaded.");
        document.write("<script type=\"text/javascript\" src=\""
            + this.UPDATER_SCRIPT
            + qs
            + "\"></script>");
    }
    else
    {
        // prototype:
        //new Ajax.Request(this.UPDATER_SCRIPT + qs, { method: 'get' });

        // jquery:
        //alert(this.UPDATER_SCRIPT + qs);
        $.ajax({ url: this.UPDATER_SCRIPT, dataType: "script", data: qs });
    }
}

// execute: --------------------------------------------- 
hc.getBrowserData();
hc.callUpdater();

