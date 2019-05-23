using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace HesterConsultants.AppCode.Entities
{
    public class JobStatusChange
    {
        public int JobStatusChangeId { get; set; }
        public Job Job { get; set; }
        public int JobId { get; set; }
        public JobStatus JobStatus { get; set; }
        public int JobStatusId { get; set; }
        public DateTime Date { get; set; }

        // not necessary - we load status changes with the job
        //public static List<JobStatusChange> RecentJobStatusChanges(DateTime asOfDate)
        //{
        //    DataTable dtRecentChanges = ClientData.Current.RecentJobStatusChangesDataTable(asOfDate);
        //    List<JobStatusChange> recentChanges = null;

        //    if (dtRecentChanges.Rows.Count > 0)
        //    {
        //        recentChanges = new List<JobStatusChange>();
        //        foreach (DataRow drStatusChange in dtRecentChanges.Rows)
        //        {
        //            JobStatusChange change = new JobStatusChange();
        //            SetFieldsFromDataRow(change, drStatusChange);
        //            recentChanges.Add(change);
        //        }
        //    }

        //    return recentChanges;
        //}

        private void SetFieldsFromDataRow(DataRow drStatusChange)
        {
            this.JobStatusChangeId = Convert.ToInt32(drStatusChange["JobStatusChangeId"]);
            this.JobId = Convert.ToInt32(drStatusChange["JobId"]);
            this.JobStatusId = Convert.ToInt32(drStatusChange["JobStatusId"]);
            this.Date = Convert.ToDateTime(drStatusChange["DateOfChange"]);
        }

        public static List<JobStatusChange> JobStatusChangesForJob(Job job)
        {
            DataTable dtStatusChangesForJob = ClientData.Current.JobStatusChangesForJobDataTable(job.JobId);
            List<JobStatusChange> changes = new List<JobStatusChange>();

            foreach (DataRow drChange in dtStatusChangesForJob.Rows)
            {
                JobStatusChange change = new JobStatusChange();
                change.SetFieldsFromDataRow(drChange);

                change.Job = job;
                change.JobStatus = JobStatus.JobStatusFromId(change.JobStatusId);
            
                //if (job.JobStatusChanges == null)
                //    job.JobStatusChanges = new List<JobStatusChange>();

                //job.JobStatusChanges.Add(change);

                changes.Add(change);
            }

            return changes;
        }

        public static JobStatusChange InsertJobStatusChange(Job job, JobStatus jobStatus, DateTime dateOfChange)
        {
            JobStatusChange change = null;

            int changeId = ClientData.Current.InsertJobStatusChange(job.JobId, jobStatus.JobStatusId, dateOfChange);

            if (changeId == 0)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add job status change to database."));

            change = new JobStatusChange();
            change.JobStatusChangeId = changeId;
            change.Job = job;
            change.JobId = job.JobId;
            change.JobStatus = jobStatus;
            change.JobStatusId = jobStatus.JobStatusId;
            change.Date = dateOfChange;

            if (job.JobStatusChanges == null)
                job.JobStatusChanges = new List<JobStatusChange>();

            job.JobStatusChanges.Add(change);

            return change;
        }
    }
}