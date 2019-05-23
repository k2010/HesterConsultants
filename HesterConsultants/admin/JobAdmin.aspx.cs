using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using System.Web.Security;
using System.IO;
using HesterConsultants.Properties;
using System.Diagnostics;

namespace HesterConsultants.admin
{
    public partial class JobAdmin : System.Web.UI.Page
    {
        private Job curJob;
        private int jobId;
        private Employee curAdmin;
        private bool filesChanged;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (curAdmin == null)
                return;

            GetJob(); // need to get job before events
            SetFileLists(); // need to set delete buttons before events b/c they *cause* events
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (this.IsPostBack)
                GetJob(); // need to get reworked Job after events

            if (curJob != null) // might be posting back from search box
                SetControls(); // now show the rest
        }

        private void GetJob()
        {
            // don't care if job is in cache or not

            string qs = this.Request.QueryString["jobId"];
            if (!String.IsNullOrEmpty(qs))
            {
                jobId = Convert.ToInt32(qs);
                curJob = CacheLayer.JobFromId(jobId);
            }
        }

        private void SetFileLists()
        {
            // clear first b/c this may get called twice
            phReturnFiles.Controls.Clear();
            phWorkingFiles.Controls.Clear();
            phSubmittedFiles.Controls.Clear();

            // returned files
            if (curJob.ReturnedFiles != null)
            {
                foreach (JobFile returnedFile in curJob.ReturnedFiles.ToList())
                    phReturnFiles.Controls.Add(FileDiv(returnedFile));
            }

            // working files
            if (curJob.WorkingFiles != null)
            {
                foreach (JobFile workingFile in curJob.WorkingFiles.ToList())
                    phWorkingFiles.Controls.Add(FileDiv(workingFile));
            }

            // submitted files
            if (curJob.SubmittedFiles != null)
            {
                foreach (JobFile submittedFile in curJob.SubmittedFiles.ToList())
                    phSubmittedFiles.Controls.Add(FileDiv(submittedFile));
            }
            else // no submitted files
                phSubmittedFiles.Controls.Add(new LiteralControl("<div>[No files submitted.]</div>"));
        }

        private void SetControls()
        {
            // link to send message
            this.pnlSendMessage.Controls.Add(new LiteralControl("<a href=\"./ClientMessage.aspx?jobId=" + curJob.JobId.ToString() + "\">Send client a message</a>"));
            this.pnlViewSegments.Controls.Add(new LiteralControl("<a href=\"/employees/Work.aspx?jobId=" + curJob.JobId.ToString() + "\">View work</a>"));

            // files
            if (filesChanged)
                SetFileLists(); // we called this earlier on page_load, so only do it again if 
                                // added or deleted a file.

            // job no.
            this.lblHeadingJobNumber.Text = curJob.JobId.ToString();

            // client
            this.lblClient.Text = this.Server.HtmlEncode(curJob.Client.FirstName + " " + curJob.Client.LastName);

            // status
            this.ddStatuses.SelectedValue = curJob.JobStatus.JobStatusId.ToString();

            // job type
            this.ddJobTypes.SelectedValue = curJob.JobType.JobTypeId.ToString();

            // job type options
            this.txtToApplication.Text = curJob.ToApplication;
            this.chkFormatted.Checked = curJob.Formatted;
            this.chkProofed.Checked = curJob.Proofread;

            // date submitted
            DateTime dateSubmittedInAdminZone = AdminUtils.EmployeeDate(curJob.DateSubmitted, curAdmin);
            this.lblDateSubmitted.Text = dateSubmittedInAdminZone.ToString("M/d/yyyy h:mm tt");

            // billing
            this.txtBillingRef.Text = curJob.BillingReference;

            // date due
            DateTime dateDueInAdminZone = AdminUtils.EmployeeDate(curJob.DateDue, curAdmin);
            this.txtDateDue.Text = dateDueInAdminZone.ToString("M/d/yyyy h:mm tt");

            // original date due
            DateTime originalDateDueInAdminZone = AdminUtils.EmployeeDate(curJob.OriginalDateDue, curAdmin);
            if (originalDateDueInAdminZone != dateDueInAdminZone)
                phOrigDateDue.Controls.Add(new LiteralControl("<br />Original Date Due: " + originalDateDueInAdminZone.ToString("M/d/yyyy h:mm tt")));

            // date completed
            if (curJob.DateCompleted > DateTime.MinValue)
            {
                DateTime dateCompleted = AdminUtils.EmployeeDate(curJob.DateCompleted, curAdmin);
                this.txtDateCompleted.Text = dateCompleted.ToString("M/d/yyyy h:mm tt");
                //this.phCompletedDate.Controls.Add(new LiteralControl(" at " + dateCompleted.ToString("M/d/yyyy h:mm tt")));
            }

            // estimate
            this.txtEstimate.Text = curJob.Estimate.ToString("c");

            // final charge
            this.txtFinalCharge.Text = curJob.FinalCharge.ToString("c");

            // taxes
            this.txtTaxes.Text = curJob.Taxes.ToString("c");

            // instructions
            this.txtInstructions.Text = curJob.Instructions;
            
            // delivery notes
            this.txtNotes.Text = curJob.DeliveryNotes;

            // archived
            this.chkArchive.Checked = curJob.IsArchived;

            // iOs attrs
            SiteUtils.AddTextBoxAttributes(this, "autocorrect", "off", true);
            SiteUtils.AddTextBoxAttributes(this, "autocapitalize", "off", true);
        }

        private void SaveFiles()
        {
            filesChanged = true;

            // get or create directory

            // virtual jobfiles root
            string jobFilesRootVirtual = Settings.Default.JobFilesRoot;
            if (!jobFilesRootVirtual.EndsWith(@"/"))
                jobFilesRootVirtual += @"/";

            // get local path
            string jobFilesRootLocal = this.Server.MapPath(jobFilesRootVirtual);

            // file upload controls
            for (int j = 0; j < 2; j++)
            {
                JobFile.JobFileType fileType;
                string whichFileControl; 
                string whichDirectory; 

                if (j == 0)
                {
                    fileType = JobFile.JobFileType.Returned;
                    whichFileControl = "returnFile";
                    whichDirectory = "out";
                }
                else if (j == 1)
                {
                    fileType = JobFile.JobFileType.Working;
                    whichFileControl = "workingFile";
                    whichDirectory = "working";
                }
                else
                {
                    fileType = JobFile.JobFileType.Submitted;
                    whichFileControl = "submittedFile";
                    whichDirectory = "in";
                }


                for (int k = 1; k <= 3; k++)
                {
                    FileUpload fu = (FileUpload)this.FindControl(whichFileControl + k.ToString());
                    if (fu.HasFile)
                    {
                        DirectoryInfo fileFolder = Directory.CreateDirectory(jobFilesRootLocal + @"\"  + jobId.ToString() + @"\" + whichDirectory);

                        string filename = HesterConsultants.AppCode.SiteUtils.WindowsSafeFilename(fu.FileName);

                        // adjust filename if necessary
                        filename = HesterConsultants.AppCode.SiteUtils.ConflictFreeFilename(fileFolder.FullName, filename, Settings.Default.FilenameAppendedDigitsMax);

                        string virtualPath = jobFilesRootVirtual + jobId.ToString() + @"/" + whichDirectory + @"/" + filename;

                        // save file
                        try
                        {
                            //Debug.WriteLine(messageFolderFullPath + @"\" + filename);
                            fu.SaveAs(fileFolder.FullName + @"\" + filename);
                        }
                        catch (Exception ex)
                        {
                            ClientData.Current.LogErrorAndSendAlert(ex);
                            throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to save file."));
                        }

                        // update db & objects
                        JobFile jobFile = JobFile.InsertJobFile(curJob, filename, virtualPath, (fileType == JobFile.JobFileType.Returned), (fileType == JobFile.JobFileType.Submitted));

                        if (jobFile == null)
                            throw new Exception(SiteUtils.ExceptionMessageForCustomer("Job file is null."));
                    }
                }
            }
        }

        private void UpdateJob()
        {
            Job newJob = curJob.ShallowCopy(); // copy to new Job in case db update fails
                                               // (we don't want to update the cached job in that case)

            // check fields

            // job type
            int selectedJobTypeId = Convert.ToInt32(this.ddJobTypes.SelectedValue);
            JobType jobType = JobType.JobTypeFromId(selectedJobTypeId);

            // new job status
            int newJobStatusId = Convert.ToInt32(this.ddStatuses.SelectedValue);
            JobStatus newJobStatus = JobStatus.JobStatusFromId(newJobStatusId);
            bool statusChanged = !curJob.JobStatus.Equals(newJobStatus);

            // date due
            DateTime dateDue = Convert.ToDateTime(this.txtDateDue.Text);
            // convert to utc
            dateDue = AdminUtils.UtcOfEmployeeDate(dateDue, curAdmin);
            newJob.DateDue = dateDue;

            // billing ref
            newJob.BillingReference = txtBillingRef.Text;

            // job type
            newJob.JobType = jobType;
            
            // job type options
            newJob.ToApplication = this.txtToApplication.Text.Trim();
            newJob.Formatted = this.chkFormatted.Checked;
            newJob.Proofread = this.chkProofed.Checked;

            // date completed
            object objDateCompleted = null;
            if (!String.IsNullOrEmpty(this.txtDateCompleted.Text.Trim()))
            {
                DateTime dateCompleted = Convert.ToDateTime(this.txtDateCompleted.Text);
                // to utc
                dateCompleted = AdminUtils.UtcOfEmployeeDate(dateCompleted, curAdmin);

                newJob.DateCompleted = dateCompleted;
                objDateCompleted = dateCompleted;
            }
            else
                newJob.DateCompleted = DateTime.MinValue;

            // instructions
            newJob.Instructions = this.txtInstructions.Text.Trim();

            // estimate
            newJob.Estimate = Convert.ToDecimal(this.txtEstimate.Text.Replace("$", ""));

            // final charge
            newJob.FinalCharge = Convert.ToDecimal(this.txtFinalCharge.Text.Replace("$", ""));

            // taxes
            newJob.Taxes = Convert.ToDecimal(this.txtTaxes.Text.Replace("$", ""));

            // delivery notes
            newJob.DeliveryNotes = txtNotes.Text.Trim();

            // archive
            newJob.IsArchived = chkArchive.Checked;

            // to do - newJob.PickedUp - no, done in /clients/job.aspx, right ??

            // date for status change
            DateTime nowUtc = DateTime.UtcNow;

            bool ret = ClientData.Current.UpdateJob(newJob.BillingReference, newJob.JobType.JobTypeId, newJob.ToApplication, newJob.Formatted, newJob.Proofread, newJob.DateDue, objDateCompleted, newJob.Instructions, newJob.Estimate, newJob.FinalCharge, newJob.Taxes, newJob.DeliveryNotes, newJob.IsArchived, newJob.JobId);

            if (!ret)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to update job in database."));

            // changed status
            if (statusChanged)
            {
                //int jobStatusChangeId = ClientData.Current.InsertJobStatusChange(newJob.JobId, newJobStatusId, nowUtc);
                JobStatusChange change = JobStatusChange.InsertJobStatusChange(newJob, newJobStatus, nowUtc);
                if (change == null)
                    throw new Exception(SiteUtils.ExceptionMessageForCustomer("Job status change is null."));
            }

            // update cached job
            List<Job> jobsInCache = CacheLayer.RecentJobs();
            int index = jobsInCache.IndexOf(curJob);
            if (index != -1)
            {
                jobsInCache.Remove(curJob);
                jobsInCache.Insert(index, newJob);
            }

            // replace global ref
            curJob = newJob;

            pnlUpdated.Visible = true;
        }

        private int JobIndexInCache(Job job)
        {
            return CacheLayer.RecentJobs().IndexOf(job);
        }

        protected void btnUpdateJob_Click(object sender, EventArgs e)
        {
            SaveFiles();
            UpdateJob();
            //this.Response.Redirect(Settings.Default.AdminHomeUrl);
        }

        private Panel FileDiv(JobFile file)
        {
            Panel pnlFile = new Panel();

            if (!curJob.IsArchived)
            {
                // just url with qs
                LiteralControl hlFile = new LiteralControl(AdminUtils.LinkToGetFilePage_Image(file, "employees")
                    + "&nbsp;" + AdminUtils.LinkToGetFilePage_Text(file, "employees") + " | " );

                LinkButton hlDeleteFile = new LinkButton();
                //hlDeleteFile.OnClientClick = "return confirm('Are you sure you want to delete this file?');";
                hlDeleteFile.Command += new CommandEventHandler(hlDeleteFile_Command);
                hlDeleteFile.CommandArgument = file.JobFileId.ToString();
                hlDeleteFile.Text = "Delete";

                pnlFile.Controls.Add(hlFile);
                pnlFile.Controls.Add(hlDeleteFile);
            }

            else // archived - no link
            {
                LiteralControl fileWithImg = new LiteralControl(AdminUtils.FileIcon() + "&nbsp;" + file.Name + " (archived)");

                pnlFile.Controls.Add(fileWithImg);
            }

            return pnlFile;
        }

        private void DeleteFile(int fileId)
        {
            filesChanged = true;

            JobFile file = JobFile.JobFileFromId(fileId);
            
            // remove reference from Job
            Job job = CacheLayer.JobFromId(file.JobId);

            if (file.IsReturnedFile)
                job.ReturnedFiles.Remove(file);
            else if (file.IsSubmittedFile)
                job.SubmittedFiles.Remove(file);
            else
                job.WorkingFiles.Remove(file);

            // delete file
            file.Delete();
        }

        protected void hlDeleteFile_Command(object sender, CommandEventArgs e)
        {
            int fileId = 0;
            bool ret = int.TryParse(e.CommandArgument.ToString(), out fileId);

            if (fileId != 0)
                DeleteFile(fileId);
        }

        //private void RedirectToFile(int fileId)
        //{
        //    this.Response.Redirect("./GetFile.aspx?fileId=" + fileId.ToString(), false);
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        //protected void hlFileWithImage_Click(object sender, CommandEventArgs e)
        //{
        //    Debug.WriteLine(e.CommandArgument.ToString());
        //    int? fileId = Convert.ToInt32(e.CommandArgument);
        //    if (fileId != null)
        //        RedirectToFile((int)fileId);
        //}
    }
}