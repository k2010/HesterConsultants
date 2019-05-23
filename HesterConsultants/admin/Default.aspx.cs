using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;
using HesterConsultants.AppCode.Entities;
using System.Text;
using System.Web.Security;
using System.Diagnostics;

namespace HesterConsultants.admin
{
    public partial class Default : System.Web.UI.Page
    {
        private Employee curAdmin;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // get logged on Admin (aka Employee)
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected DateTime DateForEmployee(DateTime dt)
        {
            if (curAdmin != null)
                return AdminUtils.EmployeeDate(dt, curAdmin);
            else
                return DateTime.MinValue;
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

        // put this in utilities.aspx
        protected void InsertAFewJobs()
        {
            // insert and delete some jobs to crank up the id number
            // also do files
        }

        protected void lvJobs_Sorting(object sender, ListViewSortEventArgs e)
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