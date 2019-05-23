using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using System.Web.Security;
using HesterConsultants.AppCode;
using HesterConsultants.admin;
using HesterConsultants.Properties;
using System.Text;
using System.Diagnostics;

namespace HesterConsultants.clients
{
    public partial class Job : System.Web.UI.Page
    {
        // to do - how to mark as picked up? 
        //  - automatically, or
        //  - make client do something affirmative 

        private Client curClient;
        //private MembershipUser authUser;
        private HesterConsultants.AppCode.Entities.Job curJob;
        private int pageId = 12;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            ClientUtils.RedirectNewOrUnauthenticatedClient();
            curClient = ClientUtils.GetClientFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (curClient == null)
                return;

            GetJob();
            if (curJob == null)
                return;

            // if it's not this client's job
            // && not admin user, don't show anything
            CheckJobIsForClient();
            UpdatePickedUpJob();
            SetControls();

            HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }

        private void GetJob()
        {
            string qs = this.Request.QueryString["jobId"];
            int jobId = 0;

            if (!String.IsNullOrEmpty(qs))
                jobId = Convert.ToInt32(qs);

            //else
            //    jobId = (int?)this.Session[Global.SESSION_JOB_ID];

            if (jobId != 0)
                curJob = CacheLayer.JobFromId((int)jobId);
        }

        private void CheckJobIsForClient()
        {
            if (!curJob.Client.Equals(curClient)
                    &&
                    !User.IsInRole(Settings.Default.RoleAdmin))
                curJob = null;
        }

        private void UpdatePickedUpJob()
        {
            if (curJob != null
                &&
                curJob.JobStatus.Equals(JobStatus.Completed)
                &&
                !curJob.PickedUp)
            {
                try
                {
                    if (ClientData.Current.UpdateJobPickedUp(true, curJob.JobId))
                        curJob.PickedUp = true;
                }
                catch 
                {
                    // nothing for now // catch it in data layer and log/notify
                }
            }
        }

        private void SetControls()
        {
            // nothing if bad job
            if (curJob == null)
            {
                this.pnlBadJob.Visible = true;
                return;
            }

            // page title
            this.Title = Settings.Default.CompanyName + " - Job No. " + curJob.JobId.ToString();

            // link to contact
            this.hlContact.NavigateUrl = Settings.Default.ContactUrl + "?jobId=" + curJob.JobId.ToString();

            // delivery notes
            if (!curJob.JobStatus.IsAClosedStatus())
                this.pnlDeliveryNotesContainer.Visible = false;
            else
            {
                this.pnlDeliveryNotesContainer.Visible = true;
                if (!String.IsNullOrEmpty(curJob.DeliveryNotes))
                    phDeliveryNotes.Controls.Add(new LiteralControl(SiteUtils.SurroundTextBlocksWithHtmlTags(curJob.DeliveryNotes, "div", null)));
                else
                    phDeliveryNotes.Controls.Add(new LiteralControl("<div>[No notes.]</div>"));
            }

            // fees
            if (curJob.JobStatus.IsAClosedStatus())
            {
                phFees.Controls.Add(new LiteralControl("<div>Fees: " + curJob.FinalCharge.ToString("c") + "</div>"));
                if (curJob.Taxes > 0)
                {
                    phFees.Controls.Add(new LiteralControl("<div>Taxes: " + curJob.Taxes.ToString("c") + "</div>"));
                    phFees.Controls.Add(new LiteralControl("<div>Total: " + (curJob.FinalCharge + curJob.Taxes).ToString("c") + "</div>"));
                }

                pnlFees.Visible = true;
            }

            // returned files
            if (curJob.ReturnedFiles != null && curJob.ReturnedFiles.Count > 0)
            {
                 foreach (JobFile returnedFile in curJob.ReturnedFiles)
                    phReturnedFiles.Controls.Add(FileDiv(returnedFile));

                pnlReturnFiles.Visible = true;
            }

            // submitted files
            if (curJob.SubmittedFiles != null && curJob.SubmittedFiles.Count > 0)
                foreach (JobFile submittedFile in curJob.SubmittedFiles)
                    phSubmittedFiles.Controls.Add(FileDiv(submittedFile));

            else // no submitted files
                phSubmittedFiles.Controls.Add(new LiteralControl("<div>[No files submitted.]</div>"));

            // segments
            if (curJob.JobSegments != null && curJob.JobSegments.Count > 0)
            {
                this.phJobSegments.Controls.Add(new LiteralControl(SegmentsTable()));
                pnlJobSegments.Visible = true;
            }

            // other fields
            this.lblHeadingJobNumber.Text = curJob.JobId.ToString();

            // status
            this.lblStatus.Text = curJob.JobStatus.Name;
            string jobStatusCss = "jobStatus" + curJob.JobStatus.Name.NoSpaces();
            //this.pnlStatusLeft.CssClass = jobStatusCss;
            this.tdStatus.Attributes.Add("class", jobStatusCss);

            //this.lblStatusHeading.Text = curJob.JobStatus.Name;
            //this.pnlStatus.CssClass = jobStatusCss;

            // date submitted
            DateTime dateSubmitted = ClientUtils.DateForClient(curClient, curJob.DateSubmitted);
            this.lblDateSubmitted.Text = dateSubmitted.ToString("M/d/yyyy h:mm tt");

            // date due
            DateTime dateDue = ClientUtils.DateForClient(curClient, curJob.DateDue);
            this.lblDateDue.Text = dateDue.ToString("M/d/yyyy h:mm tt");

            // date completed
            if (curJob.JobStatus.IsAClosedStatus())
            {
                DateTime dateCompleted = ClientUtils.DateForClient(curClient, curJob.DateCompleted);
                this.lblStatus.Text += " at " + dateCompleted.ToString("M/d/yyyy h:mm tt");
            }

            // billing ref
            this.lblBillingRef.Text = this.Server.HtmlEncode(curJob.BillingReference);

            // job type
            this.lblJobType.Text = curJob.JobType.Name;

            // options
            string options = "N/A";
            if (curJob.JobType.Equals(JobType.Conversion) || curJob.JobType.Equals(JobType.Editing))
            {
                options = curJob.ToApplication;
                if (!String.IsNullOrEmpty(options))
                    options += "/";
                options += (curJob.Formatted ? "Formatted/" : "Unformatted/");
                options += (curJob.Proofread ? "Proofed" : "Not proofed");
            }
            this.lblOptions.Text = options;

            // instructions
            this.phInstructions.Controls.Add(
                new LiteralControl(SiteUtils.SurroundTextBlocksWithHtmlTags(curJob.Instructions, "div", null)));
        }

        private Panel FileDiv(JobFile file)
        {
            Panel pnlFile = new Panel();

            if (!curJob.IsArchived)
            {
                // just url with qs
                LiteralControl hlFile = new LiteralControl(AdminUtils.LinkToGetFilePage_Image(file, "clients") + "&nbsp;" + AdminUtils.LinkToGetFilePage_Text(file, "clients"));

                pnlFile.Controls.Add(hlFile);
            }

            else // archived - no link
            {
                LiteralControl fileWithImg = new LiteralControl(AdminUtils.FileIcon() + "&nbsp;" + file.Name + " (archived)");

                pnlFile.Controls.Add(fileWithImg);
            }

            return pnlFile;
        }

        private string SegmentsTable()
        {
            StringBuilder sbSegments = new StringBuilder();
            sbSegments.Append("<table class=\"clientSegmentsTable\">");
            sbSegments.Append("<tr>");
            sbSegments.Append("<th>Staff</th>");
            sbSegments.Append("<th>Date</th>");
            sbSegments.Append("<th class=\"right\">Hours</th>");
            sbSegments.Append("</tr>");

            // copy segments to new list of segments in ***Client's time***
            List<JobSegment> clientSegments = new List<JobSegment>();
            foreach (JobSegment segment in curJob.JobSegments)
            {
                JobSegment clientSegment = segment.ShallowCopy();
                clientSegment.StartDate = ClientUtils.DateForClient(curClient, segment.StartDate);
                clientSegment.EndDate = ClientUtils.DateForClient(curClient, segment.EndDate);
                clientSegments.Add(clientSegment);
                //Debug.WriteLine("for me (real segment): " + TimeZoneInfo.ConvertTimeFromUtc(segment.StartDate, TimeZoneInfo.Local).ToString());
                //Debug.WriteLine("for client (copy): " + clientSegment.StartDate.ToString());
            }

            // group segments by date
            var segmentGroups = clientSegments
                .GroupBy(cs => new { cs.Employee, cs.StartDate.Date }) // grouping on ee and startdate
                .Select(gs => new { Employee = gs.Key.Employee, Date = gs.Key.Date, TotalMinutes = gs.Sum(cs => cs.MinutesWorked) })                      // getting sum of minutes 
                .ToList();

            decimal totalHoursForCompare = 0m;
            foreach (var segmentGroup in segmentGroups)
            {
                sbSegments.Append("<tr>");

                string eeName = segmentGroup.Employee.FirstName.Substring(0, 1) + ". "
                    + segmentGroup.Employee.LastName;
                sbSegments.Append("<td>" + eeName + "</td>");

                // already did this:
                //DateTime startDate = ClientUtils.DateForClient(curClient, segmentGroup.Date);

                // to do - not this way. aggregate hours by day
                string strDate = segmentGroup.Date.ToString("M/d/yyyy");
                sbSegments.Append("<td>" + strDate + "</td>");

                int minutes = segmentGroup.TotalMinutes;
                decimal hours = decimal.Round((decimal)minutes / 60m, 1);

                // keep running total of displayed 
                totalHoursForCompare += hours;

                sbSegments.Append("<td class=\"right\">" + hours.ToString("#,##0.0") + "</td>");
                sbSegments.Append("</tr>");
            }

            sbSegments.Append("</table>");

            int totalMinutes = curJob.JobSegments.Sum(s => s.MinutesWorked);
            decimal totalHoursForSegment = decimal.Round((decimal)totalMinutes / 60m, 1);

            sbSegments.Append("<div class=\"marginTop20px\">Total hours: " + totalHoursForSegment.ToString("#,##0.0") + "</div>");

            // note if they don't add up
            if (totalHoursForCompare != totalHoursForSegment)
                sbSegments.Append("<div><span class=\"asterisk\">*</span> Total hours may not exactly equal the sum of the hours shown in the history table, due to rounding. Our time records are kept in minutes, and converted to hours in this display for your convenience.</div>");

            return sbSegments.ToString();
        }
    }
}