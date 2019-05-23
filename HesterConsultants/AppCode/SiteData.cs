using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Text;

namespace HesterConsultants.AppCode
{
    public sealed class SiteData
    {
        // to do - make this a singleton

        // data layer

        // constants
        // only *db* constants here, not site concept constants
        // (those are in SiteUtils)

        private string visitorsConnectionString;

        // database ids
        public int DEFAULT_VISITOR = 2;
        public int DEFAULT_SESSION = 1;
        public int DEFAULT_BROWSER_ID = 16;
        public int DEFAULT_PLATFORM_ID = 19;
        public int DEFAULT_SCREEN_WIDTH = 0;
        public int DEFAULT_SCREEN_HEIGHT = 0;
        public int DEFAULT_SCREEN_COLOR_DEPTH = 0;

        private SiteData()
        {
            this.visitorsConnectionString = HesterConsultants.Properties.Settings.Default.ConnString_Visitors;
        }

        private class SingletonInstantiator
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static SingletonInstantiator()
            {
            }

            internal static SiteData instance = new SiteData();
        }

        public static SiteData Current
        {
            get
            {
                return SingletonInstantiator.instance;
            }
        }

        private SqlConnection VisitorsConnection()
        {
            return new SqlConnection(this.visitorsConnectionString);
        }

        public int InsertVisitor(System.Web.UI.Page page)
        {
            // return visitor id

            SqlConnection conn = VisitorsConnection();
            SqlCommand cmd = conn.CreateCommand();
            SqlParameter parm;
            int visitorId = DEFAULT_VISITOR;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertVisitor";

            parm = cmd.Parameters.Add("@TempSessionId", SqlDbType.NVarChar, 50);
            if (page.Session != null && page.Session.SessionID != null
                    && page.Session.SessionID != String.Empty)
                parm.Value = page.Session.SessionID.ToString();
            else
                parm.Value = System.DBNull.Value;

            parm = cmd.Parameters.Add("@VisitorId", SqlDbType.Int);
            parm.Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                visitorId = (int)cmd.Parameters["@VisitorId"].Value;
                conn.Close();
            }
            catch (Exception e)
            {
                InsertError(e.Message, DateTime.UtcNow);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return visitorId;
        }

        public int InsertSession(int visitorId, System.Web.UI.Page page, DateTime date)
        {
            // return session id

            SqlConnection conn = VisitorsConnection();
            SqlCommand cmd = conn.CreateCommand();
            SqlParameter parm;
            int sessionId = DEFAULT_SESSION;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertSession";

            // visitor id
            parm = cmd.Parameters.Add("@Vid", SqlDbType.Int);
            parm.Value = visitorId;

            // remote addr
            object addr = page.Session[SiteUtils.SESSION_REMOTE_ADDR];
            parm = cmd.Parameters.Add("@RemoteAddr", SqlDbType.NVarChar, 20);
            if (addr != null && addr.ToString() != String.Empty)
                parm.Value = addr.ToString();
            else
                parm.Value = System.DBNull.Value;

            // remote host
            parm = cmd.Parameters.Add("@RemoteHost", SqlDbType.NVarChar, 80);
            if (page.Request.ServerVariables["REMOTE_HOST"] != null && page.Request.ServerVariables["REMOTE_HOST"] != String.Empty)
                parm.Value = page.Request.ServerVariables["REMOTE_HOST"];
            else
                parm.Value = System.DBNull.Value;

            // remote user
            parm = cmd.Parameters.Add("@RemoteUser", SqlDbType.NVarChar, 20);
            if (page.Request.ServerVariables["REMOTE_USER"] != null && page.Request.ServerVariables["REMOTE_USER"] != String.Empty)
                parm.Value = page.Request.ServerVariables["REMOTE_USER"];
            else
                parm.Value = System.DBNull.Value;

            // client-side vars to be updated later
            // ----------------------------------------
            // browser id
            parm = cmd.Parameters.Add("@BrowserId", SqlDbType.Int);
            parm.Value = DEFAULT_BROWSER_ID; // other

            // platform id
            parm = cmd.Parameters.Add("@PlatformId", SqlDbType.Int);
            parm.Value = DEFAULT_PLATFORM_ID; // other

            //// screen size id
            //parm = cmd.Parameters.Add("@ScreenSizeId", SqlDbType.Int);
            //parm.Value = DEFAULT_SCREEN_SIZE_ID; // other

            //// screen color id
            //parm = cmd.Parameters.Add("@ScreenColorDepthId", SqlDbType.Int);
            //parm.Value = DEFAULT_SCREEN_COLORS_ID; // other

            parm = cmd.Parameters.Add("@ScreenWidth", SqlDbType.Int);
            parm.Value = DEFAULT_SCREEN_WIDTH;

            parm = cmd.Parameters.Add("@ScreenHeight", SqlDbType.Int);
            parm.Value = DEFAULT_SCREEN_HEIGHT;

            parm = cmd.Parameters.Add("@ScreenColorDepth", SqlDbType.Int);
            parm.Value = DEFAULT_SCREEN_COLOR_DEPTH;
            // ----------------------------------------

            // start time
            parm = cmd.Parameters.Add("@StartTime", SqlDbType.DateTime);
            parm.Value = date;

            // user agent
            parm = cmd.Parameters.Add("@UserAgent", SqlDbType.NVarChar, 255);
            if (page.Request.ServerVariables["HTTP_USER_AGENT"] != null
                    && page.Request.ServerVariables["HTTP_USER_AGENT"] != String.Empty)
                parm.Value = page.Request.ServerVariables["HTTP_USER_AGENT"];
            else
                parm.Value = System.DBNull.Value;

            // asp session id
            parm = cmd.Parameters.Add("@AspId", SqlDbType.NVarChar, 30);
            if (page.Session.SessionID != null)
                parm.Value = page.Session.SessionID.ToString();
            else
                parm.Value = System.DBNull.Value;

            // output session id
            parm = cmd.Parameters.Add("@SessionId", SqlDbType.Int);
            parm.Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                sessionId = (int)cmd.Parameters["@SessionId"].Value;
                conn.Close();
            }
            catch (Exception e)
            {
                InsertError(e.Message, DateTime.UtcNow);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return sessionId;
        }

        public int GetMySessionIdFromAspId(System.Web.UI.Page page)
        {
            SqlConnection conn = VisitorsConnection();
            SqlCommand cmd = conn.CreateCommand();
            SqlParameter parm;
            int mySessionId = 0;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetSessionIdFromAspSessionId";
            parm = cmd.Parameters.Add("@AspId", SqlDbType.NVarChar, 50);
            parm.Value = page.Session.SessionID.ToString();

            try
            {
                conn.Open();
                object ret = cmd.ExecuteScalar();

                if (ret != null && ret != System.DBNull.Value)
                    mySessionId = Convert.ToInt32(ret);
                conn.Close();
            }
            catch (Exception ex)
            {
                InsertError(ex.Message, DateTime.UtcNow);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return mySessionId;
        }

        public int InsertHit(int sessionId, int pageId, System.Web.UI.Page page, DateTime date)
        {
            SqlConnection conn = VisitorsConnection();
            SqlCommand cmd = conn.CreateCommand();
            SqlParameter parm;
            int hitId = 0;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertHit";

            parm = cmd.Parameters.Add("@SessionId", SqlDbType.Int);
            parm.Value = sessionId;

            parm = cmd.Parameters.Add("@HitDate", SqlDbType.DateTime);
            parm.Value = date;

            parm = cmd.Parameters.Add("@PageId", SqlDbType.Int);
            parm.Value = pageId;

            parm = cmd.Parameters.Add("@QueryString", SqlDbType.NVarChar, 255);
            if (page.Request.QueryString != null && page.Request.QueryString.ToString() != String.Empty)
            {
                parm.Value = page.Request.QueryString.ToString();
            }
            else
                parm.Value = System.DBNull.Value;

            //parm = cmd.Parameters.Add("@ReferrerUrl", SqlDbType.NVarChar, 255);
            parm = cmd.Parameters.Add("@ReferrerUrl", SqlDbType.NText);
            if (!String.IsNullOrEmpty(page.Request.ServerVariables["HTTP_REFERER"]))
                parm.Value = ParseQFromGoogle(page.Request.ServerVariables["HTTP_REFERER"].ToString());
            else
                parm.Value = System.DBNull.Value;

            parm = cmd.Parameters.Add("@HitId", SqlDbType.Int);
            parm.Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                hitId = (int)cmd.Parameters["@HitId"].Value;
                conn.Close();
            }
            catch (Exception e)
            {
                InsertError(e.Message, DateTime.UtcNow);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return hitId;
        }

		public void LogIgnoredSession(string remoteAddr, string referrer, string reason = "")
		{
			SqlConnection conn; // = VisitorsConnection();
			SqlCommand cmd; // = conn.CreateCommand();
			SqlParameter parm;

			using (conn = VisitorsConnection())
			{
				cmd = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "INSERT INTO IgnoredSessions(HitDate, RemoteAddr, Referrer, Reason) "
					+ "VALUES (@HitDate, @RemoteAddr, @Referrer, @Reason);";

				parm = cmd.Parameters.Add("@HitDate", SqlDbType.DateTime);
				parm.Value = DateTime.UtcNow;
				parm = cmd.Parameters.AddWithValue("@RemoteAddr", remoteAddr);
				parm = cmd.Parameters.AddWithValue("@Referrer", referrer);
				parm = cmd.Parameters.AddWithValue("@Reason", reason);

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
		
		public string ParseQFromGoogle(string referrer)
        {
            if (!referrer.Contains("google.com") || !referrer.Contains("?"))
                return referrer;

            StringBuilder sbReferrer = new StringBuilder();

            try
            {
                string[] keys = referrer.Split(new char[] { '?', '&' });

                int lenBeforeQs = keys[0].Length;

                for (int k = 0; k < keys.Length; k++)
                {
                    if (keys[k].StartsWith("q="))
                        sbReferrer.Insert(lenBeforeQs + 1, keys[k] + "&");
                    else
                    {
                        sbReferrer.Append(keys[k]);
                        if (k == 0)
                            sbReferrer.Append("?");
                        else
                            sbReferrer.Append("&");
                    }
                }
            }

            catch (Exception ex)
            {
                InsertError(ex.Message, DateTime.UtcNow);
            }

            return sbReferrer.ToString();
        }

        public void UpdateSessionClientValues(System.Web.UI.Page page,
            string browserVal, string platformVal,
            string screenWidthVal, string screenHeightVal, 
            string screenColorsVal, string timezoneOffsetVal)
        {
            SqlConnection conn = VisitorsConnection();
            SqlCommand cmd = conn.CreateCommand();
            SqlParameter parm;
            int rows;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "UpdateSessionWithClientValues";

            if (page.Session[SiteUtils.SESSION_SESSION_ID] != null
                    &&
                    page.Session[SiteUtils.SESSION_SESSION_ID].ToString() != String.Empty)
            {
                parm = cmd.Parameters.Add("@Bid", SqlDbType.Int);
                if (browserVal != String.Empty)
                    parm.Value = Convert.ToInt32(browserVal);
                else
                    parm.Value = DEFAULT_BROWSER_ID;

                parm = cmd.Parameters.Add("@Pid", SqlDbType.Int);
                if (platformVal != String.Empty)
                    parm.Value = Convert.ToInt32(platformVal);
                else
                    parm.Value = DEFAULT_PLATFORM_ID;

                parm = cmd.Parameters.Add("@Sw", SqlDbType.Int);
                if (screenWidthVal != String.Empty)
                    parm.Value = Convert.ToInt32(screenWidthVal);
                else
                    parm.Value = DEFAULT_SCREEN_WIDTH;

                parm = cmd.Parameters.Add("@Sh", SqlDbType.Int);
                if (screenHeightVal != String.Empty)
                    parm.Value = Convert.ToInt32(screenHeightVal);
                else
                    parm.Value = DEFAULT_SCREEN_HEIGHT;

                parm = cmd.Parameters.Add("@Sc", SqlDbType.Int);
                if (screenColorsVal != String.Empty)
                    parm.Value = Convert.ToInt32(screenColorsVal);
                else
                    parm.Value = DEFAULT_SCREEN_COLOR_DEPTH;

                parm = cmd.Parameters.Add("@Tzo", SqlDbType.Int);
                if (timezoneOffsetVal != String.Empty)
                    parm.Value = Convert.ToInt32(timezoneOffsetVal);
                else
                    parm.Value = System.DBNull.Value;

                parm = cmd.Parameters.Add("@SessionId", SqlDbType.Int);
                parm.Value = Convert.ToInt32(page.Session[SiteUtils.SESSION_SESSION_ID]);

                try
                {
                    conn.Open();
                    rows = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception e)
                {
                    InsertError(e.Message, DateTime.UtcNow);
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }

        public void InsertError(string description, DateTime date)
        {
            SqlConnection conn = VisitorsConnection();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertError";
            SqlParameter parm;

            parm = cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255);
            parm.Value = description;

            parm = cmd.Parameters.Add("@ErrorDate", SqlDbType.DateTime);
            parm.Value = date;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        //public static bool IgnoredIp(string remoteAddr)
        //{
        //    // to do - this is untenable.
        //    // just test for js
        //    // insert hits and sessions for all,
        //    // client stuff only if js

        //    //string[] tokens;
        //    //char[] delimiter = { '.' };

        //    //if (remoteAddr == "")
        //    //    return false;

        //    //tokens = remoteAddr.Split(delimiter);

        //    //if (tokens.Length != 4)
        //    //    return false;

        //    //// compass 64.246.161 or 165
        //    //if (remoteAddr.StartsWith("64.246.161") ||
        //    //    remoteAddr.StartsWith("64.246.165"))
        //    //    return true;

        //    //// microsoft 65.52 - 65.55
        //    //if (remoteAddr.StartsWith("65.55"))
        //    //    return true;

        //    //// time warner websense 66.194.6.0 - 66.194.6.255
        //    //if (remoteAddr.StartsWith("66.194.6."))
        //    //    return true;

        //    //// google 66.249.64-95
        //    //if (remoteAddr.StartsWith("66.249.") &&
        //    //        Convert.ToInt32(tokens[2]) >= 64 &&
        //    //        Convert.ToInt32(tokens[2]) <= 95)
        //    //    return true;

        //    //// daren healthcare 67.215.230.16 - .23
        //    //if (remoteAddr.StartsWith("67.215") &&
        //    //        Convert.ToInt32(tokens[3]) >= 16 &&
        //    //        Convert.ToInt32(tokens[3]) <= 23)
        //    //    return true;

        //    //// inktomi 68.142.249-251...
        //    //if (remoteAddr.StartsWith("68.142.") &&
        //    //    Convert.ToInt32(tokens[2]) >= 249 &&
        //    //    Convert.ToInt32(tokens[2]) <= 251)
        //    //    return true;

        //    //// inktomi 73.30
        //    //if (remoteAddr.StartsWith("72.30"))
        //    //    return true;

        //    return false;
        //}

        public void UpdateSessionAtSessionEnd(HttpSessionState session)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
            SqlConnection conn = VisitorsConnection();
            SqlCommand cmd = conn.CreateCommand();
            SqlParameter parm;
            int mySessionId = DEFAULT_SESSION;

            //System.Diagnostics.Trace.WriteLine("Session_End starting!!!");

            // check that Session["mySessionId"] exists
            if (session[SiteUtils.SESSION_SESSION_ID] == null)
            {
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetSessionIdFromAspSessionId";
                parm = cmd.Parameters.Add("@AspId", SqlDbType.NVarChar, 50);
                parm.Value = session.SessionID.ToString();

                conn.Open();
                object ret = cmd.ExecuteScalar();
                conn.Close();

                if (ret != null && ret != System.DBNull.Value)
                {
                    mySessionId = Convert.ToInt32(ret);
                }
            }
            else
                mySessionId = Convert.ToInt32(session[SiteUtils.SESSION_SESSION_ID]);

            if (mySessionId == 0) return;

            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SetSessionEndTime";
            parm = cmd.Parameters.Add("@EndTime", SqlDbType.DateTime);
            parm.Value = DateTime.UtcNow;

            parm = cmd.Parameters.Add("@SessionId", SqlDbType.Int);
            parm.Value = mySessionId;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                InsertError(ex.Message, DateTime.UtcNow);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }
    }
}
