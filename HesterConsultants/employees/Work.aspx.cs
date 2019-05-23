using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using HesterConsultants.admin;
using System.Text;
using System.IO;
using HesterConsultants.Properties;

namespace HesterConsultants.employees
{
    public partial class Work : System.Web.UI.Page
    {
        // to do - have ees use .WorkingFiles, not .ReturnedFiles.
        // that way, we leverage the safe filename thing and never 
        // overwrite anything, even if ee keeps working on the same file.

        private Job curJob;
        private int jobId;
        private Employee curEmployee; 
        private bool isWorkingNow = false;
        private JobSegment curSegment;
        private List<JobSegment> curSegments;
        private bool filesChanged = false;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curEmployee = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (curEmployee == null)
                return;

            //eeTimeZone = TimeZoneInfo.FindSystemTimeZoneById(curEmployee.TimeZoneId);
            //eeOffset = (int) Math.Round(eeTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes);

            GetJob();
            GetJobSegments();
            SetFileLists();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                GetJob(); // status may be updated after segment insert/update
                GetJobSegments(); // get segments again after insert/update event handler
            }

            SetControls();
        }

        private void GetJob()
        {
            string qs = this.Request.QueryString["jobId"];
            if (!String.IsNullOrEmpty(qs))
            {
                jobId = Convert.ToInt32(qs);
                curJob = CacheLayer.JobFromId(jobId);
            }
        }

        private void GetJobSegments()
        {
            // to do - these will be added when someone starts/stops
            // so, no need to go to JobSegment
            //curSegments = JobSegment.SegmentsForJob(curJob);
            if (curJob.JobSegments != null)
                curSegments = curJob.JobSegments.ToList();

            if (curSegments != null)
            {
                curSegments = curSegments.OrderByDescending(s => s.StartDate).ToList();

                curSegment = curSegments.Where(s => s.Employee.Equals(curEmployee)
                                                &&
                                                s.EndDate == DateTime.MinValue)
                                                .FirstOrDefault();
            }
            isWorkingNow = curSegment != null;
        }

        private void SetControls()
        {
            // link to job
            this.pnlViewJob.Controls.Add(new LiteralControl("<a href=\"/admin/JobAdmin.aspx?jobId=" + curJob.JobId.ToString() + "\">View job details</a>"));

            // files
            if (filesChanged)
                SetFileLists(); // we called this earlier on page_load, so only do it again if 
                                // added or deleted a file.

            // segments
            if (curSegments != null)
                phSegments.Controls.Add(new LiteralControl(SegmentsTable()));

            // job no.
            this.lblHeadingJobNumber.Text = curJob.JobId.ToString();

            // client
            this.lblClient.Text = this.Server.HtmlEncode(curJob.Client.FirstName + " " + curJob.Client.LastName);

            // status
            this.lblStatus.Text = curJob.JobStatus.Name;

            // date submitted
            DateTime dateSubmitted = AdminUtils.EmployeeDate(curJob.DateSubmitted, curEmployee);                   this.lblDateSubmitted.Text = dateSubmitted.ToString("M/d/yyyy h:mm tt");

            // date due
            DateTime dateDue = AdminUtils.EmployeeDate(curJob.DateDue, curEmployee);
            this.lblDateDue.Text = dateDue.ToString("M/d/yyyy h:mm tt");

            // completed "at _______"
            if (curJob.JobStatus.IsAClosedStatus())
            {
                DateTime dateCompleted = AdminUtils.EmployeeDate(curJob.DateCompleted, curEmployee);                   this.lblStatus.Text += " at " + dateCompleted.ToString("M/d/yyyy h:mm tt");
            }

            // job type
            this.lblJobType.Text = curJob.JobType.Name;

            // instructions
            this.phInstructions.Controls.Add(
                new LiteralControl(SiteUtils.SurroundTextBlocksWithHtmlTags(curJob.Instructions, "div", null)));

            // buttons
            if (isWorkingNow)
            {
                pnlStart.Visible = false;
                DateTime startTimeForEe = AdminUtils.EmployeeDate(curSegment.StartDate, curEmployee);                  this.txtTimeStarted.Text = AdminUtils.StandardDateFormat(startTimeForEe, false);
            }
            else
                pnlStart.Visible = true;

            // job status dropdown
            this.ddStatuses.SelectedValue = curJob.JobStatus.JobStatusId.ToString();
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

        private Panel FileDiv(JobFile file)
        {
            Panel pnlFile = new Panel();

            if (!curJob.IsArchived)
            {
                // just url with qs
                LiteralControl hlFile = new LiteralControl(AdminUtils.LinkToGetFilePage_Image(file, "employees") + "&nbsp;" + AdminUtils.LinkToGetFilePage_Text(file, "employees"));

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
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class=\"openJobs\">");
            sb.Append("<tr>");
            sb.Append("<th>Employee</th>");
            sb.Append("<th>Start</th>");
            sb.Append("<th>Stop</th>");
            sb.Append("<th>Notes</th>");
            sb.Append("<th></th>");
            sb.Append("</tr>");

            foreach (JobSegment segment in curSegments)
            {
                string segmentEmployee = segment.Employee.FirstName.Substring(0, 1) + ". "
                    + segment.Employee.LastName;
                DateTime segmentStart = AdminUtils.EmployeeDate(segment.StartDate, curEmployee);

                bool isOpenSegment = segment.EndDate == DateTime.MinValue;
                DateTime segmentStop = DateTime.MinValue;
                if (!isOpenSegment)
                    segmentStop = AdminUtils.EmployeeDate(segment.EndDate, curEmployee);

                sb.Append("<tr");
                if (isOpenSegment)
                    sb.Append(" class=\"openJobSegment\"");
                sb.Append(">");

                sb.Append("<td>" + segmentEmployee + "</td>");
                sb.Append("<td>" + AdminUtils.StandardDateFormat(segmentStart, true) + "</td>");
                sb.Append("<td>");
                if (!isOpenSegment)
                    sb.Append(AdminUtils.StandardDateFormat(segmentStop, true));
                sb.Append("</td>");

                sb.Append("<td>" + SiteUtils.SurroundTextBlocksWithHtmlTags(segment.Notes, "div", null) + "</td>");

                sb.Append("<td>");
                if (isOpenSegment && segment.Employee.Equals(curEmployee))
                    sb.Append("<a href=\"\" id=\"hlStopWork\">Stop</a>");
                sb.Append("</td>");

                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        private void InsertSegment()
        {
            DateTime nowUtc = DateTime.UtcNow;
            string notes = this.txtSegmentNotesForStart.Text.Trim();

            // insert job segment
            JobSegment segment = JobSegment.InsertJobSegment(curJob, curEmployee, nowUtc, notes);
            if (segment == null)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add job segment to database."));

            // insert job status change
            JobStatusChange change = JobStatusChange.InsertJobStatusChange(curJob, JobStatus.InProgress, nowUtc);
            if (change == null)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add job status change."));
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


                FileUpload fu = (FileUpload)this.FindControl(whichFileControl + "1");
                if (fu.HasFile)
                {
                    DirectoryInfo fileFolder = Directory.CreateDirectory(jobFilesRootLocal + @"\" + jobId.ToString() + @"\" + whichDirectory);

                    string filename = HesterConsultants.AppCode.SiteUtils.WindowsSafeFilename(fu.FileName);

                    // adjust filename if necessary
                    filename = HesterConsultants.AppCode.SiteUtils.ConflictFreeFilename(fileFolder.FullName, filename, Settings.Default.FilenameAppendedDigitsMax);

                    string virtualPath = jobFilesRootVirtual + jobId.ToString() + @"/" + whichDirectory + @"/" + filename;

                    // save file
                    try
                    {
                        fu.SaveAs(fileFolder.FullName + @"\" + filename);
                    }
                    catch (Exception ex)
                    {
                        ClientData.Current.LogErrorAndSendAlert(ex);
                        throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to save file."));
                    }

                    JobFile jobFile = JobFile.InsertJobFile(curJob, filename, virtualPath, (fileType == JobFile.JobFileType.Returned), (fileType == JobFile.JobFileType.Submitted));

                    if (jobFile == null)
                        throw new Exception(SiteUtils.ExceptionMessageForCustomer("Job file is null."));
                }
            }
        }

        private void UpdateSegment()
        {
            if (curSegment == null)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Current segment is null."));

            DateTime startTime = DateTime.Parse(this.txtTimeStarted.Text);
            startTime = AdminUtils.UtcOfEmployeeDate(startTime, curEmployee);

            DateTime stopTime = DateTime.Parse(this.txtTimeStopped.Text);
            stopTime = AdminUtils.UtcOfEmployeeDate(stopTime, curEmployee);

            int minutesWorked = Convert.ToInt32(Convert.ToDecimal(this.txtHoursWorked.Text) * 60m);
                //(int) Math.Round(nowUtc.Subtract(curSegment.StartDate).TotalMinutes);

            // make new segment, so we can test for null -- necessary ??
            JobSegment newSegment = curSegment.UpdateJobSegment(startTime, stopTime, minutesWorked, this.txtSegmentNotesForStop.Text.Trim());

            if (newSegment == null)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to update job segment."));

            // assign successful result
            curSegment = newSegment;

            // update job status
            JobStatus newStatus = JobStatus.JobStatusFromId(Convert.ToInt32(this.ddStatuses.SelectedValue));

            bool statusChanged = !newStatus.Equals(curJob.JobStatus);

            if (statusChanged)
            {
                JobStatusChange change = JobStatusChange.InsertJobStatusChange(curJob, newStatus, DateTime.UtcNow);
                if (change == null)
                    throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add job status change."));

                if (newStatus.IsAClosedStatus())
                {
                    // update the job to insert completion date
                    Job newJob = curJob.ShallowCopy();

                    DateTime dateCompleted = Convert.ToDateTime(this.txtTimeStopped.Text);
                    dateCompleted = AdminUtils.UtcOfEmployeeDate(dateCompleted, curEmployee);

                    newJob.DateCompleted = dateCompleted;

                    bool ret = ClientData.Current.UpdateJob(newJob.BillingReference, newJob.JobType.JobTypeId, newJob.ToApplication, newJob.Formatted, newJob.Proofread, newJob.DateDue, newJob.DateCompleted, newJob.Instructions, newJob.Estimate, newJob.FinalCharge, newJob.Taxes, newJob.DeliveryNotes, newJob.IsArchived, newJob.JobId);

                    if (!ret)
                        throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to update job in database."));

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
                }
            }
        }

        //private void AddJobToOdsSegmentsParameters(ObjectDataSourceSelectingEventArgs e)
        //{
        //    e.InputParameters.Add("job", curJob);
        //}

        //protected void odsSegments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        //{
        //    AddJobToOdsSegmentsParameters(e);
        //}

        protected void btnStart_Click(object sender, EventArgs e)
        {
            InsertSegment();
        }

        protected void btnStop_Click(object sender, EventArgs e)
        {
            UpdateSegment();
            SaveFiles();
        }
    }
}