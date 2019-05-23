using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using HesterConsultants.AppCode;
using System.Diagnostics;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.Properties;

namespace HesterConsultants
{
    public class Global : System.Web.HttpApplication
    {
        // to do - put all this stuff in the config file

        // site-specific contants
        // 
        //public static string COMPANY_DOMAIN = "hesterconsultants.com";
        //public static string COMPANY_URL_SHORT = "www.hesterconsultants.com";
        //public static string COMPANY_URL_FULL = "http://www.hesterconsultants.com";
        //public static string COMPANY_URL_FULL_SECURE = "https://www.hesterconsultants.com";
        //public static string COMPANY_NAME = "Hester Consultants LLC";
        //public static string COMPANY_NAME_SHORT = "Hester Consultants";
        //public static string COMPANY_SLOGAN = "Data | Automation | Advice";
        //public static int COMPANY_START_YEAR = 2010;
        //public static string SITE_COOKIE = "hcv1"; // root node for all cookies
        //public static string VISITOR_COOKIE = "vid";

        // pages
        //public static string CLIENT_LOGIN_PAGE = "/login/";
        //public static string CLIENT_HOME_PAGE = "/clients/Home.aspx";
        //public static string CLIENT_ACCOUNT_PAGE = "/clients/EditAccount.aspx";
        ////public static string NEW_CLIENT_FORM_PAGE = "/clients/";
        //public static string ADMIN_HOME_PAGE = "/admin/";
        //public static string EMPLOYEE_HOME_PAGE = "/employees/";

        // machines
        //public static string[] LOCAL_SERVER_NAMES = { "mopac", "rr2222" };
        //public static bool LOCAL_SERVER = LOCAL_SERVER_NAMES.Contains<string>(HttpContext.Current.Server.MachineName.ToLower()); // to do - send this to SiteUtils

        // mail
        //public static string CUSTOMER_SERVICE_EMAIL = 
        //    LOCAL_SERVER ? "postmaster@kenhester.com" : "postmaster@hesterconsultants.com";
        //public static string CUSTOMER_CONTACT_EMAIL = "ken.hester@hesterconsultants.com";
        //public static string CLIENT_EMAIL = "ken@kenhester.com"; // to do - use ^^ company contact instead
        //public static string SMTP_HOST = 
        //    LOCAL_SERVER ? "MOPAC" : "relay-hosting.secureserver.net";
        //public static TimeZoneInfo CLIENT_TIME_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        //public static string CLIENT_TIME_ZONE_SHORT_NAME = "CT";

        // phone
        //public static string PHONE_MAIN = "512.560.0435";
        //public static string PHONE_TECH_SUPPORT = "512.560.0435";

        //public string SITE_ROOT; // set in setuplocalorremotesession()

        // ip addresses
        //public static string MY_ISP_IP = "99.88.243.50"; // to do - check from time to time
        //public static string MY_LOCAL_IP = "192.168";
        //public static string MY_LOCALHOST_IP = "127.0.0.1";

        // paths
        //public static string UPLOADS_PATH_ATTACHMENTS = "/admin/uploads/mail";

        // cached list keys
        public static string CACHE_ALL_AUTHUSERS = "allAuthUsers";
        public static string CACHE_ALL_CLIENTS = "allClients";
        public static string CACHE_ALL_COMPANIES = "allCompanies";
        public static string CACHE_ALL_ADDRESSES = "allAddresses";
        public static string CACHE_ALL_JOB_TYPES = "allJobTypes";
        public static string CACHE_ALL_JOB_STATUSES = "allJobStatuses";
        public static string CACHE_RECENT_JOBS = "recentJobs";
        public static string CACHE_RECENT_JOB_STATUS_CHANGES = "recentJobStatusChanges";
        public static string CACHE_ALL_EMPLOYEES = "allEmployees";

        // roles
        //public static string ROLE_ADMIN = "Admin";
        //public static string ROLE_EMPLOYEE = "Employee";
        //public static string ROLE_CLIENT = "Client"; // full client access
        //public static string ROLE_NEW_CLIENT = "NewClient"; // authenticated via token; needs to enter info
        //public static string ROLE_UNAUTHENTICATED_CLIENT = "UnauthenticatedClient"; // hasn't used token to authenticate yet

        // session keys
        public static string SESSION_CONTACT_MESSAGES_FOLDER = "uploadsFolder";
        public static string SESSION_AUTH_USER_ID = "aspnetUserId";
        public static string SESSION_CLIENT = "client";
        public static string SESSION_COMPANY = "company";
        public static string SESSION_EMPLOYEE = "employee";
        //public static string SESSION_JOB_ID = "currentJobId";
        //public static string SESSION_FILE_ID = "currentFileId";
        //public static string SESSION_CLIENT_ID = "clientId";
        //public static string SESSION_CLIENT_NAME = "clientName";

        // business concepts
        //public static int MAX_LENGTH_OF_FILENAME_DIGIT_APPENDS = 2;
        //public static string JOB_STATUS_NAME_COMPLETED = "Completed";
        //public static int RECENT_JOBS_NUMBER_OF_DAYS = 7;

        //public enum LoginLevels
        //{
        //    PublicLevel,
        //    AdminLevel
        //}
        
        public static PageInfo[] pages_public = { PageInfo.HomeRelative, PageInfo.ServicesRelative, PageInfo.DemosRelative, PageInfo.AboutRelative, PageInfo.ContactRelative, PageInfo.ClientsRelative };
        public static PageInfo[] pages_admin = { PageInfo.HomeRelative, PageInfo.ServicesRelative, PageInfo.DemosRelative, PageInfo.AboutRelative, PageInfo.ContactRelative, PageInfo.StatsRelative, PageInfo.ClientsRelative, PageInfo.AdminRelative };
        public static PageInfo[] pages_local = { PageInfo.HomeRelative, PageInfo.ServicesRelative, PageInfo.DemosRelative, PageInfo.AboutRelative, PageInfo.ContactRelative, PageInfo.ClientsRelative, PageInfo.AdminRelative };

        //public static string ContactUrl()
        //{
        //    // kah 2011-01-07 - this is not needed anymore, thanks to SecureWebPages httpmodule
        //    return LOCAL_SERVER ? PageInfo.ContactRelative.Url : PageInfo.ContactSecure.Url;
        //}

        protected void Application_Start(object sender, EventArgs e)
        {
            // to do - load up the cache

        }

        protected void Session_Start(object sender, EventArgs e)
        {
			//Debug.AutoFlush = true;
            SetUpSiteCookie();
            SetUpLocalOrRemoteSession();
            //HandleAuthenticatedUser();
        }

        //private void HandleAuthenticatedUser()
        //{
        //    // to do - *done* put this in handler.aspx

        //    // user may be authenticated, but not in Session[client]
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        MembershipUser user = Membership.GetUser();

        //        if (User.IsInRole(Settings.Default.RoleClient))
        //        {
        //            Client client = CacheLayer.ClientFromAuthUserId((Guid)user.ProviderUserKey);
        //            if (client == null)
        //            {
        //                FormsAuthentication.SignOut();
        //                this.Response.Redirect(FormsAuthentication.LoginUrl);
        //            }
        //            else
        //            {
        //                Session[Global.SESSION_CLIENT] = client;
        //                Session[Global.SESSION_AUTH_USER_ID] = user.ProviderUserKey;
        //            }
        //        }
        //    }
        //}

        private void SetUpLocalOrRemoteSession()
        {
            string remoteAddr = String.Empty;
			bool ignoreIp = false;
			bool ignoreRef = false;
			string ignoredReferrer = String.Empty;
			bool local = SiteUtils.IsLocalSession(ref remoteAddr);

            //this.Session["ignoreSession"] = false; ??

			// debug --------------------------------------------------------------------------
			//local = false;
			// --------------------------------------------------------------------------------

			if (local)
			{
				Debug.Print("******** local *********");
				Session.Timeout = 120;
			}
			else
			{
				ignoreRef = SiteUtils.IgnoreReferrer(ref ignoredReferrer);
				// debug
				//ignored = false;
				if (ignoreRef)
				{
					Debug.WriteLine("ignored: " + ignoredReferrer);
					Response.Clear();
					Response.End();
					SiteData.Current.LogIgnoredSession(remoteAddr, ignoredReferrer, "Ignored referrer");
				}
				else
				{
					ignoreIp = SiteUtils.IgnoreIpAddress();
					// debug -------------------------------------------------------
					//ignoreIp = false;
					// -------------------------------------------------------------
				}
			}

			if (this.Request.ServerVariables["URL"] != null)
                this.Session[SiteUtils.SESSION_FIRST_URL] = this.Request.ServerVariables["URL"].ToString();

			//if (this.Request.ServerVariables["REMOTE_ADDR"] != null)
			//{
			//    remoteAddr = this.Request.ServerVariables["REMOTE_ADDR"].ToString();
			//    this.Session[SiteUtils.SESSION_REMOTE_ADDR] = remoteAddr;
			//    //System.Diagnostics.Debug.Print("remote addr: " + remoteAddr);

			//    if (remoteAddr == Settings.Default.DevIspIpAddress)
			//        local = true;
			//    else if (remoteAddr.StartsWith("::1"))
			//        local = true;
			//    else if (remoteAddr.StartsWith(Settings.Default.DevLanIpAddressLeft))
			//        local = true;
			//    else if (remoteAddr == Settings.Default.DevLocalhostIpAddress)
			//        local = true;

			//    //Debug.WriteLine("remote addr: " + this.Request.ServerVariables["REMOTE_ADDR"]);
			//    //Debug.WriteLine("Local: " + local.ToString());
                
			//    // debug
			//    //local = false;
			//}

            if (local)
            {
                Session[SiteUtils.SESSION_LOCAL] = true;
                //Session[SiteUtils.SESSION_LOGIN_LEVEL] = LoginLevels.AdminLevel;
                Session.Timeout = 240;
            }
            else
            {
                Session[SiteUtils.SESSION_LOCAL] = false;
                //Session[SiteUtils.SESSION_LOGIN_LEVEL] = LoginLevels.PublicLevel;
                Session.Timeout = 20;
            }

			// trace:
			//Session["ignorereferrer"] = ignoreRef;
			//Session["ignoreip"] = ignoreIp;

			if (local || ignoreRef || ignoreIp)
			{
				Session[SiteUtils.SESSION_DONT_LOG] = true;
				Session[SiteUtils.SESSION_DONT_SEND_ALERT] = true;
			}
		}

        private void SetUpSiteCookie()
        {
            DateTime now = DateTime.Now;

            HttpCookie siteCookie = this.Request.Cookies[Settings.Default.SiteCookieName]; // this.Request.Cookies.Get(SITE_COOKIE);
            if (siteCookie == null)
            {
                siteCookie = new HttpCookie(Settings.Default.SiteCookieName);
                siteCookie.Path = "/";
            }

            siteCookie.Expires = DateTime.MaxValue; // now.AddYears(1); // one year for site cookie

            this.Response.Cookies.Add(siteCookie);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // debug
            //if (System.Diagnostics.Debugger.IsAttached && User == null)
            //{
            //    FormsAuthentication.SetAuthCookie("y2kenny@kenhester.com", false);
            //}
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            SiteData.Current.UpdateSessionAtSessionEnd(this.Session);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}