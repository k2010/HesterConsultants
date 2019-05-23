using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Diagnostics;
using HesterConsultants.Properties;

namespace HesterConsultants.AppCode
{
    public class SessionUpdater
    {
        // this updates session field values only if 
        // the query string values
        // are not all the default values)

        System.Web.UI.Page page;
        private bool nonDefaultValues = false; // at least one browser val not default
        private bool sessionIdExists = false;
        private bool browserValExists = false;
        private bool platformValExists = false;
        //private bool screenSizeValExists = false;
        private bool screenWidthValExists = false;
        private bool screenHeightValExists = false;
        private bool screenColorsValExists = false;
        private bool timezoneOffsetValExists = false;
        private string sessionIdVal;
        private string browserVal;
        private string platformVal;
        //private string screenSizeVal;
        private string screenWidthVal;
        private string screenHeightVal;
        private string screenColorsVal;
        private string timezoneOffsetVal;

        public SessionUpdater(System.Web.UI.Page page)
        {
            this.page = page;
        }

        public void RunUpdate()
        {
            //Debug.WriteLine("RunUpdate()");

            InitVals();
            if (nonDefaultValues)
            {
                UpdateSessionRecord();
                SetUpEmailAlert();
            }
        }

        private void InitVals()
        {
            sessionIdExists = page.Session[SiteUtils.SESSION_SESSION_ID] != null
                && (sessionIdVal = page.Session[SiteUtils.SESSION_SESSION_ID].ToString()) != String.Empty;
            browserValExists = page.Request.QueryString[SiteUtils.QS_BROWSER] != null
                && (browserVal = page.Request.QueryString[SiteUtils.QS_BROWSER]) != String.Empty;
            platformValExists = page.Request.QueryString[SiteUtils.QS_PLATFORM] != null
                && (platformVal = page.Request.QueryString[SiteUtils.QS_PLATFORM]) != String.Empty;
            screenWidthValExists = page.Request.QueryString[SiteUtils.QS_SCREEN_WIDTH] != null
                && (screenWidthVal = page.Request.QueryString[SiteUtils.QS_SCREEN_WIDTH]) != String.Empty;
            screenHeightValExists = page.Request.QueryString[SiteUtils.QS_SCREEN_HEIGHT] != null
                && (screenHeightVal = page.Request.QueryString[SiteUtils.QS_SCREEN_HEIGHT]) != String.Empty;
            screenColorsValExists = page.Request.QueryString[SiteUtils.QS_COLOR_DEPTH] != null
                && (screenColorsVal = page.Request.QueryString[SiteUtils.QS_COLOR_DEPTH]) != String.Empty;
            timezoneOffsetValExists = page.Request.QueryString[SiteUtils.QS_TIMEZONE_OFFSET] != null
                && (timezoneOffsetVal = page.Request.QueryString[SiteUtils.QS_TIMEZONE_OFFSET]) != String.Empty;

            nonDefaultValues = sessionIdExists && browserValExists && platformValExists
                && screenWidthValExists && screenHeightValExists
                && screenColorsValExists && timezoneOffsetValExists
                && (Convert.ToInt32(browserVal) != HesterConsultants.AppCode.SiteData.Current.DEFAULT_BROWSER_ID
                    || Convert.ToInt32(platformVal) != HesterConsultants.AppCode.SiteData.Current.DEFAULT_PLATFORM_ID
                    || Convert.ToInt32(screenWidthVal) != HesterConsultants.AppCode.SiteData.Current.DEFAULT_SCREEN_WIDTH
                    || Convert.ToInt32(screenHeightVal) != HesterConsultants.AppCode.SiteData.Current.DEFAULT_SCREEN_HEIGHT
                    || Convert.ToInt32(screenColorsVal) != HesterConsultants.AppCode.SiteData.Current.DEFAULT_SCREEN_COLOR_DEPTH);
        }

        private void UpdateSessionRecord()
        {
            //Debug.WriteLine("UpdateSessionRecord()");
            SiteData.Current.UpdateSessionClientValues(page, browserVal,
                platformVal, screenWidthVal, screenHeightVal, screenColorsVal, timezoneOffsetVal);
        }

        private void SetUpEmailAlert()
        {
            //Debug.WriteLine("SetUpEmailAlert()");
            // call email sender if we have
            // remoteAddr and firstRequestedUrl
            // and it's not a local session

            string remoteAddr = String.Empty;
            string requestedUrl = String.Empty;

            if (page.Session[SiteUtils.SESSION_REMOTE_ADDR] != null
                    &&
                    page.Session[SiteUtils.SESSION_FIRST_URL] != null)
            {
                remoteAddr = page.Session[SiteUtils.SESSION_REMOTE_ADDR].ToString();
                requestedUrl = page.Session[SiteUtils.SESSION_FIRST_URL].ToString();
            }

            if (page.Session[SiteUtils.SESSION_LOCAL] != null
                    &&
                    (bool)page.Session[SiteUtils.SESSION_LOCAL] != true)
            {
                // send email goes here
                new Emailer(Settings.Default.SmtpHost, Settings.Default.EmailStyleTag).SendArrivalEmail(remoteAddr, requestedUrl);
            }
            // debug: put it here to test, then return to above
        }
    }
}
