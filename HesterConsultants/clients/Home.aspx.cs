using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;
using HesterConsultants.AppCode.Entities;
using System.Web.Security;
using System.Data;
using System.Diagnostics;
using HesterConsultants.Properties;

namespace HesterConsultants.clients
{
    public partial class Home : System.Web.UI.Page
    {
        private Client curClient;
        private int numNewlyCompletedJobs = 0;
        private int pageId = 9;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            ClientUtils.RedirectNewOrUnauthenticatedClient();
            curClient = ClientUtils.GetClientFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.header1.PageInfo = PageInfo.ClientsRelative;

            if (curClient == null)
                return;

            GetData();

            HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            if (numNewlyCompletedJobs == 0)
                pnlNewlyCompletedJobs.Visible = false;
        }

        private void GetData()
        {
            this.odsNewlyCompletedJobs.SelectParameters.Clear();
            this.odsOpenRecentJobs.SelectParameters.Clear();
        }

        protected DateTime DateForClient(DateTime dt)
        {
            return ClientUtils.DateForClient(curClient, dt);
        }

        protected string EllipsisText(string text)
        {
            int numCharsToShow = 50;
            object attributes = null;

            string truncatedText = SiteUtils.TruncatedTextWithIndicator(text, numCharsToShow);
            string divs = SiteUtils.SurroundTextBlocksWithHtmlTags(truncatedText, "div", null, true);
            if (text.Length > numCharsToShow)
                attributes = new { title = this.Server.HtmlEncode(text) };    

            return SiteUtils.ContainerHtml("div", divs, attributes);
        }

        //protected string DateFormat(DateTime dt)
        //{
        //    return dt.ToString("M/d/yyyy") + "<br />" + dt.ToString("h:mm tt");
        //}

        private void AddClientParameter(ObjectDataSourceSelectingEventArgs e)
        {
            if (!e.InputParameters.Contains("client")) // to do - does this fix exception 
                                                       // "key has already been added"?
                e.InputParameters.Add("client", curClient);
        }

        protected void odsOpenRecentJobs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            AddClientParameter(e);
        }

        protected void odsNewlyCompletedJobs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            AddClientParameter(e);
        }

        protected void odsNewlyCompletedJobs_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            List<HesterConsultants.AppCode.Entities.Job> newlyCompletedJobs = 
                e.ReturnValue as List<HesterConsultants.AppCode.Entities.Job>;
            if (newlyCompletedJobs != null)
                numNewlyCompletedJobs = newlyCompletedJobs.Count;
        }

        protected void lvOpenRecentJobs_Sorting(object sender, ListViewSortEventArgs e)
        {
            string jobIdSort = "sortedOnJobId";

            // make first call on JobId descending
            if (e.SortExpression == "JobId")
            {
                if (ViewState[jobIdSort] == null || !(bool)ViewState[jobIdSort])
                {
                    e.SortDirection = SortDirection.Descending;
                    this.ViewState[jobIdSort] = true;
                }
            }
        }

        //private void SetCurrentJobAndRedirect(int jobId)
        //{
        //    // set session var
        //    this.Session[Global.SESSION_JOB_ID] = Convert.ToInt32(jobId);
        //    this.Response.Redirect("./Job.aspx", false);
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        //protected void hlJob_Click(object sender, CommandEventArgs e)
        //{
        //    Debug.WriteLine(e.CommandArgument.ToString());
        //    int? jobId = Convert.ToInt32(e.CommandArgument);
        //    if (jobId != null)
        //        SetCurrentJobAndRedirect((int)jobId);
        //}
    }
}