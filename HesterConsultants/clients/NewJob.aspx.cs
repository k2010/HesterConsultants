using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using System.IO;
using System.Diagnostics;
using HesterConsultants.controls;
using System.Web.Security;
using HesterConsultants.Properties;

namespace HesterConsultants.clients
{
    // to do - allow user to modify job if not in progress

    public partial class NewJob : System.Web.UI.Page
    {
        private Client curClient;
        private int minJobTypeId;
        private int maxJobTypeId;
        HesterConsultants.AppCode.Entities.Job newJob;
        private DateTime dateDue;
        private int pageId = 15;

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

            SetControls();

            HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            SiteUtils.AddTextBoxAttributes(this, "autocorrect", "off", true);
            SiteUtils.AddTextBoxAttributes(this, "autocapitalize", "off", true);
        }

        protected string Postback0Or1()
        {
            return (this.IsPostBack ? "1" : "0");
        }

        private void SetControls()
        {
            this.rngJobType.MinimumValue = (minJobTypeId = JobType.MinJobTypeId()).ToString();
            this.rngJobType.MaximumValue = (maxJobTypeId = JobType.MaxJobTypeId()).ToString();

            this.lblUserTimeZone.Text = TimeZoneInfo.FindSystemTimeZoneById(curClient.TimeZoneId).Id;
        }

        private void SaveFiles()
        {
            // create directory
            //HesterConsultants.Properties.Settings settings =
            //    HesterConsultants.Properties.Settings.Default;

            // job files virtual root
            string jobFilesRootVirtual = Settings.Default.JobFilesRoot;
            if (!jobFilesRootVirtual.EndsWith(@"/"))
                jobFilesRootVirtual += @"/";

            // get local path
            string jobFilesRootLocal = this.Server.MapPath(jobFilesRootVirtual);
            if (!jobFilesRootLocal.EndsWith(@"\"))
                jobFilesRootLocal += @"\";

            DirectoryInfo jobInFolder = Directory.CreateDirectory(jobFilesRootLocal + newJob.JobId.ToString() + @"\in"); // "in" for submitted files

            // six file upload controls
            for (int k = 1; k <= 6; k++)
            {
                FileUpload fu = (FileUpload)this.FindControl("file" + k.ToString());
                if (fu.HasFile)
                {
                    string filename = HesterConsultants.AppCode.SiteUtils.WindowsSafeFilename(fu.FileName);

                    // adjust filename if necessary
                    filename = HesterConsultants.AppCode.SiteUtils.ConflictFreeFilename(jobInFolder.FullName, filename, Settings.Default.FilenameAppendedDigitsMax);
                    string virtualPath = jobFilesRootVirtual + newJob.JobId.ToString() + @"/in/" + filename;

                    try
                    {
                        fu.SaveAs(jobInFolder.FullName + @"\" + filename);
                    }
                    catch (Exception ex)
                    {
                        ClientData.Current.LogErrorAndSendAlert(ex);
                        throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to save file."));
                    }

                    //int jobFileId = ClientData.Current.InsertJobFile(newJob.JobId, filename, virtualPath, false, true);
                    
                    //if (jobFileId == 0)
                    //    throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add job file to database."));

                    JobFile jobFile = JobFile.InsertJobFile(newJob, filename, virtualPath, false, true);

                    if (jobFile == null)
                        throw new Exception(SiteUtils.ExceptionMessageForCustomer("Job file is null."));
                }
            }
        }

        private void InsertInitialJobStatus()
        {
            JobStatus submittedStatus = JobStatus.Submitted;
            int submittedStatusId = submittedStatus.JobStatusId;

            //int jobStatusChangeId = ClientData.Current.InsertJobStatusChange(newJob.JobId, submittedStatusId, newJob.DateSubmitted);
            JobStatusChange change = JobStatusChange.InsertJobStatusChange(newJob, JobStatus.Submitted, newJob.DateSubmitted);

            if (change == null)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Job status change is null."));
        }

        //private void EncodeInputValues()
        //{
        //    // allow users to put in angle brackets etc.
        //    // just escape them
        //    // to do - *done* take this out, leave angle brackets in,
        //    // just encode them on output
        //    this.txtDateDue.Text = this.Server.HtmlEncode(this.txtDateDue.Text);
        //    this.txtInstructions.Text = this.Server.HtmlEncode(this.txtInstructions.Text);
        //}

        private void InsertJob()
        {
            // check fields

            // job type
            int selectedJobTypeId = Convert.ToInt32(this.ddJobTypes.SelectedValue);
            if (selectedJobTypeId < minJobTypeId
                    ||
                    selectedJobTypeId > maxJobTypeId)
                selectedJobTypeId = JobType.Other.JobTypeId;

            JobType jobType = JobType.JobTypeFromId(selectedJobTypeId);

            string toApp = String.Empty;
            bool formatted = false;
            bool proofed = false;

            // job type options 
            if (jobType.Equals(JobType.Conversion) || jobType.Equals(JobType.Editing))
            {
                if (jobType.Equals(JobType.Conversion))
                {
                    toApp = this.rbApplications.SelectedValue
                        + " " + this.rbVersions.SelectedValue;

                    formatted = this.rbFormatted.SelectedIndex == 0;
                }

                proofed = this.rbProof.SelectedIndex == 0;
            }

            // date submitted
            DateTime dateSubmitted = DateTime.UtcNow;

            // date due set earlier in btn handler
            //// convert to utc
            dateDue = ClientUtils.UtcForClientDate(curClient, dateDue);

            newJob = new HesterConsultants.AppCode.Entities.Job();
            newJob.Client = curClient;
            newJob.BillingReference = this.txtBillingRef.Text.Trim();
            newJob.JobType = jobType;
            newJob.ToApplication = toApp;
            newJob.Formatted = formatted;
            newJob.Proofread = proofed;
            newJob.DateSubmitted = dateSubmitted;
            newJob.DateDue = newJob.OriginalDateDue = dateDue;
            newJob.Instructions = this.txtInstructions.Text.Trim();
            newJob.Estimate = 0m;
            newJob.FinalCharge = 0m;
            newJob.Taxes = 0m;

            CacheLayer.InsertJob(newJob);
        }

        private void SendMessages()
        {
            Emailer emailer = new Emailer(Settings.Default.SmtpHost, Settings.Default.EmailStyleTag);

            emailer.SendReceiptAcknowledgment(newJob);
            emailer.SendReceiptAlert(newJob);
        }

        private bool DueDateIsValid()
        {
            DateTime clientNow = ClientUtils.DateForClient(curClient, DateTime.UtcNow);
            dateDue = new DateTime();

            try
            {
                dateDue = Convert.ToDateTime(this.txtDateDue.Text + " " + this.ddHalfHours.SelectedValue);
            }
            catch
            {
                return false;
            }

            return dateDue > clientNow && dateDue < clientNow.AddYears(1);
        }

        private void ShowInvalidDueDateMessage()
        {
            this.phDateRangeValidator.Controls.Add(new LiteralControl("<span class=\"errorMessage\">Sorry, that due date looks invalid. Please enter a valid date in the near future, in US date format (m/d/yyyy).<br />Please re-enter any uploaded files. Thank you.</span><br />"));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //EncodeInputValues(); just encode on output
            if (!DueDateIsValid())
                ShowInvalidDueDateMessage();
            else
            {
                InsertJob();
                InsertInitialJobStatus();
                SaveFiles();
                SendMessages();
                this.Response.Redirect("/clients/Job.aspx?jobId=" + newJob.JobId.ToString(), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void ddJobTypes_Load(object sender, EventArgs e)
        {
            ddJobTypes.Items.Add(new ListItem("Select:", "0"));
        }
    }
}