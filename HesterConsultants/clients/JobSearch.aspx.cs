using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using HesterConsultants.clients.controls;
using HesterConsultants.admin;
using System.Text;
using HesterConsultants.Properties;
using System.Diagnostics;

namespace HesterConsultants.clients
{
    public partial class JobSearch : System.Web.UI.Page
    {
        private Client curClient;
        private bool isAdmin;
        private List<string> searchList;
        private string highlightTag = Settings.Default.SearchHighlightHtmlTag;
        private int pageId = 13;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            ClientUtils.RedirectNewOrUnauthenticatedClient();
            curClient = ClientUtils.GetClientFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            isAdmin = User.IsInRole(Settings.Default.RoleAdmin);
            GetData();
            this.lvFoundJobs.DataBind(); // need this b/c of viewstate

            HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }

        private void GetData()
        {
            this.odsFoundJobs.SelectParameters.Clear();
        }

        protected DateTime DateForClient(DateTime dt)
        {
            return ClientUtils.DateForClient(curClient, dt);
        }

        protected string JobNumber(HesterConsultants.AppCode.Entities.Job job)
        {

            string s = SiteUtils.HighlightTermsInTextBlocks(job.JobId.ToString(), searchList, highlightTag, 5);

            return s;
        }

        protected string EllipsisText(string text)
        {
            //return SiteUtils.SurroundTextBlocksWithHtmlTags(SiteUtils.TruncatedTextWithIndicator(text, 50), "div", new { title = text });
            return SiteUtils.SurroundTextBlocksWithHtmlTags(SiteUtils.HighlightTermsInTextBlocks(text, searchList, highlightTag, 30), "div", null, false);
        }

        protected string FileList(HesterConsultants.AppCode.Entities.Job job)
        {
            StringBuilder sb = new StringBuilder();
            if (job.ReturnedFiles != null && job.SubmittedFiles != null)
                sb.Append("<div style=\"border-top: dashed 1px #666666; line-height: normal;\">&nbsp;</span>");

            if (job.ReturnedFiles != null)
            {
                sb.Append("Returned files:<ul>");
                foreach (JobFile file in job.ReturnedFiles)
                {
                    sb.Append("<li>");
                    sb.Append(SiteUtils.HighlightTermsInTextBlocks(file.Name, searchList, highlightTag, 10));
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }

            if (job.SubmittedFiles != null)
            {
                sb.Append("Submitted files:<ul>");
                foreach (JobFile file in job.SubmittedFiles)
                {
                    sb.Append("<li>");
                    sb.Append(SiteUtils.HighlightTermsInTextBlocks(file.Name, searchList, Settings.Default.SearchHighlightHtmlTag, 10));
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }

            return sb.ToString();
        }

        // to do - why is this firing twice?
        protected void odsFoundJobs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            ClientControlPanel ccp = null;
            if (this.PreviousPage != null)
            {
                ccp = (ClientControlPanel)this.PreviousPage.FindControl("ccp1");
                ClientControlPanel newCcp = (ClientControlPanel)this.FindControl("ccp1");
                newCcp.SearchBox.Text = ccp.SearchBox.Text;
            }
            else
                ccp = (ClientControlPanel)this.FindControl("ccp1");

            TextBox searchBox = ccp.SearchBox;
            string searchTerms = searchBox.Text.Trim();

            searchList = searchTerms.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // remove stop words
            searchList = AdminUtils.RemoveStopWords(searchList).ToList();

            e.InputParameters.Add("client", curClient);
            e.InputParameters.Add("searchTerms", searchList);
            e.InputParameters.Add("isAdmin", isAdmin);
        }

        protected void lvFoundJobs_Sorting(object sender, ListViewSortEventArgs e)
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
    }
}