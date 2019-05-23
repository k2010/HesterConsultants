using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.Properties;
using HesterConsultants.admin;
using HesterConsultants.AppCode;
using System.Text;
using HesterConsultants.controls;

namespace HesterConsultants.employees
{
    public partial class JobSearchAdmin : System.Web.UI.Page
    {
        private Employee curEmployee;
        private List<string> searchList;
        private string highlightTag = Settings.Default.SearchHighlightHtmlTag;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curEmployee = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (curEmployee == null)
                return;

            GetData();
            this.lvFoundJobs.DataBind(); // need this b/c of viewstate
        }

        private void GetData()
        {
            this.odsFoundJobs.SelectParameters.Clear();
        }

        protected DateTime DateForEmployee(DateTime dt)
        {
            return AdminUtils.EmployeeDate(dt, curEmployee);
        }

        protected string JobNumber(HesterConsultants.AppCode.Entities.Job job)
        {

            string s = SiteUtils.HighlightTermsInTextBlocks(job.JobId.ToString(), searchList, highlightTag, 5);

            return s;
        }

        protected string EllipsisText(string text)
        {
            //return SiteUtils.SurroundTextBlocksWithHtmlTags(SiteUtils.TruncatedTextWithIndicator(text, 50), "div", new { title = text });

            return SiteUtils.SurroundTextBlocksWithHtmlTags(SiteUtils.HighlightTermsInTextBlocks(text, searchList, highlightTag, 20), "div", null, false);
        }

        protected string FileList(HesterConsultants.AppCode.Entities.Job job)
        {
            StringBuilder sb = new StringBuilder();
            if (job.ReturnedFiles != null && job.SubmittedFiles != null)
                sb.Append("<div style=\"border-top: dashed 1px #666666; line-height: normal;\">&nbsp;</span>");

            if (job.ReturnedFiles != null)
            {
                sb.Append("Returned files:<br />");
                foreach (JobFile file in job.ReturnedFiles)
                    sb.Append(SiteUtils.HighlightTermsInTextBlocks(file.Name, searchList, highlightTag, 5) + "<br />");
            }

            if (job.SubmittedFiles != null)
            {
                sb.Append("Submitted files:<br />");
                foreach (JobFile file in job.SubmittedFiles)
                    sb.Append(SiteUtils.HighlightTermsInTextBlocks(file.Name, searchList, Settings.Default.SearchHighlightHtmlTag, 5) + "<br />");
            }

            return sb.ToString();
        }

        // to do - why is this firing twice?
        protected void odsFoundJobs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            AdminControlPanel acp = null;
            if (this.PreviousPage != null)
            {
                acp = (AdminControlPanel)this.PreviousPage.FindControl("acp1");
                AdminControlPanel newAcp = (AdminControlPanel)this.FindControl("acp1");
                newAcp.SearchBox.Text = acp.SearchBox.Text;
            }
            else
                acp = (AdminControlPanel)this.FindControl("acp1");

            TextBox searchBox = acp.SearchBox;
            string searchTerms = searchBox.Text.Trim();

            searchList = searchTerms.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // remove stop words
            searchList = AdminUtils.RemoveStopWords(searchList).ToList();

            e.InputParameters.Add("client", new Client() { ClientId = 0 });
            e.InputParameters.Add("searchTerms", searchList);
            e.InputParameters.Add("isAdmin", true);
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