using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using HesterConsultants.Properties;

namespace HesterConsultants.AppCode
{
    public class HitHandler
    {
        private System.Web.UI.Page page;
        private int pageId;
        private int visitorId;
        private int mySessionId = 0;
        private int hitId = 0;
        //private string aspSessionId = "";
        //private string js = "";

        public HitHandler(System.Web.UI.Page page, int pageId)
        {
            this.page = page;
            this.pageId = pageId;
            //aspSessionId = page.Session.SessionID;
        }

        //public string Js
        //{
        //    get
        //    {
        //        return js;
        //    }
        //    set
        //    {
        //        js = value;
        //    }
        //}

        private void InsertSession()
        {
            //Debug.WriteLine("InsertSession()");
            mySessionId = SiteData.Current.InsertSession(visitorId, page, DateTime.UtcNow);

            page.Session[SiteUtils.SESSION_SESSION_ID] = mySessionId;
        }

        private void InsertVisitor()
        {
            //Debug.WriteLine("InsertVisitor()");
            visitorId = SiteData.Current.InsertVisitor(page);

            page.Session[SiteUtils.SESSION_VISITOR_ID] = visitorId;
            page.Response.Cookies[Settings.Default.SiteCookieName][Settings.Default.VisitorCookieName]
                = visitorId.ToString();
        }

        private void HandleVisitor()
        {
            //Debug.WriteLine("HandleVisitor()");
            string siteCookie = Settings.Default.SiteCookieName;
            string visitorCookie = Settings.Default.VisitorCookieName;

            // see if visitor has vid cookie
            if (page.Request.Cookies != null && page.Request.Cookies[siteCookie] != null && page.Request.Cookies[siteCookie][visitorCookie] != null)
                visitorId = Convert.ToInt32(page.Request.Cookies[siteCookie][visitorCookie]);
            else
                InsertVisitor();
        }

        private void InsertHit()
        {
            //Debug.WriteLine("InsertHit()");
            hitId = SiteData.Current.InsertHit(mySessionId, pageId, page, DateTime.UtcNow);
        }

        public void HandlePage() // this is the entry point - each page calls this
        {
			// 2014- cancel for now
			//return;
			// -------------------------

			object dontLog = HttpContext.Current.Session[SiteUtils.SESSION_DONT_LOG];
			if (dontLog == null || (bool)dontLog == false)
			{
				//Debug.WriteLine("HandlePage()");
				if (!SessionExists())
				{
					HandleVisitor();
					InsertSession();
				}
				else
				{
					// if there is a session, check that client vars
					// are updated
					// 4/1/10 comment out:
					//if (this.Page.Request.QueryString["tz"] != null)
					//    UpdateClientValues();
				}

				InsertHit();
			}
        }

        private bool SessionExists()
        {
            // this should generally return false
            // on first page hit, true otherwise.
            // if session was inserted, there should be 
            // either Session["mySessionId"] or at least 
            // an Asp SessionId in the db.
            // if user has cookies disabled,
            // there may be no mySessionId but there is 
            // an Asp SessionId - use that session ??????
            if (page.Session[SiteUtils.SESSION_SESSION_ID] != null)
            {
                mySessionId = Convert.ToInt32(page.Session[SiteUtils.SESSION_SESSION_ID]);
                return true;
            }
            else
            {
                mySessionId = SiteData.Current.GetMySessionIdFromAspId(page);
                // gets mySessionId if there is a row for this session
                return mySessionId != 0;
            }
        }
    }
}
