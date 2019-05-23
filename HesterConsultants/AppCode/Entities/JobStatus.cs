using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using HesterConsultants.Properties;

namespace HesterConsultants.AppCode.Entities
{
    public class JobStatus
    {
        public int JobStatusId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Lookup class. Keep a static list of all statuses,
        /// so that looking up by id can be done without 
        /// going to database.
        /// </summary>
        private static List<JobStatus> allStatuses;

        public JobStatus()
        {
        }

        public JobStatus(int id, string name)
        {
            this.JobStatusId = id;
            this.Name = name;
        }

        public static JobStatus Submitted
        {
            get
            {
                return AllJobStatuses()
                    .Where(js => js.JobStatusId == Settings.Default.JobStatusSubmittedId)
                    .DefaultIfEmpty(JobStatus.TechnicalDifficulty)
                    .First();
            }
        }

        public static JobStatus Queued
        {
            get
            {
                return AllJobStatuses()
                    .Where(js => js.JobStatusId == Settings.Default.JobStatusQueuedId)
                    .DefaultIfEmpty(JobStatus.TechnicalDifficulty)
                    .First();
            }
        }

        public static JobStatus InProgress
        {
            get
            {
                return AllJobStatuses()
                    .Where(js => js.JobStatusId == Settings.Default.JobStatusInProgressId)
                    .DefaultIfEmpty(JobStatus.TechnicalDifficulty)
                    .First();
            }
        }

        public static JobStatus InReview
        {
            get
            {
                return AllJobStatuses()
                    .Where(js => js.JobStatusId == Settings.Default.JobStatusInReviewId)
                    .DefaultIfEmpty(JobStatus.TechnicalDifficulty)
                    .First();
            }
        }

        public static JobStatus PendingApproval
        {
            get
            {
                return AllJobStatuses()
                    .Where(js => js.JobStatusId == Settings.Default.JobStatusPendingApprovalId)
                    .DefaultIfEmpty(JobStatus.TechnicalDifficulty)
                    .First();
            }
        }

        public static JobStatus Completed
        {
            get 
            {
                return AllJobStatuses()
                    .Where(js => js.JobStatusId == Settings.Default.JobStatusCompletedId)
                    .DefaultIfEmpty(JobStatus.TechnicalDifficulty)
                    .First();
            }
        }

        public static JobStatus Canceled
        {
            get
            {
                return AllJobStatuses()
                    .Where(js => js.JobStatusId == Settings.Default.JobStatusCanceledId)
                    .DefaultIfEmpty(JobStatus.TechnicalDifficulty)
                    .First();
            }
        }

        public static JobStatus Declined
        {
            get
            {
                return AllJobStatuses()
                    .Where(js => js.JobStatusId == Settings.Default.JobStatusDeclinedId)
                    .DefaultIfEmpty(JobStatus.TechnicalDifficulty)
                    .First();
            }
        }

        // debug
        public static JobStatus TechnicalDifficulty
        {
            get
            {
                return new JobStatus(0, "Technical Difficulty");
            }
        }

        // methods
        public static List<JobStatus> AllJobStatuses()
        {
            if (allStatuses == null)
                RefreshAllStatuses();

            return allStatuses;
        }

        private static void RefreshAllStatuses()
        {
            // load static list
            DataTable dtStatuses = ClientData.Current.AllJobStatusesDataTable();
            allStatuses = new List<JobStatus>();

            foreach (DataRow drStatus in dtStatuses.Rows)
            {
                JobStatus jobStatus = new JobStatus();
                jobStatus.JobStatusId = (int)drStatus["JobStatusId"];
                jobStatus.Name = drStatus["JobStatusName"].ToString();

                allStatuses.Add(jobStatus);
            }
        }

        public static JobStatus JobStatusFromId(int id)
        {
            return AllJobStatuses().Where(s => s.JobStatusId == id).FirstOrDefault();
        }

        public bool IsAClosedStatus()
        {
            return this.Equals(JobStatus.Canceled)
                || this.Equals(JobStatus.Completed)
                || this.Equals(JobStatus.Declined);
        }

        // operators
        //public static bool operator ==(JobStatus a, JobStatus b)
        //{
        //    try
        //    {
        //        return a.JobStatusId == b.JobStatusId;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public static bool operator !=(JobStatus a, JobStatus b)
        //{
        //    try
        //    {
        //        return a.JobStatusId != b.JobStatusId;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public override bool Equals(object obj)
        {
            try
            {
                return this.JobStatusId == ((JobStatus)obj).JobStatusId;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.JobStatusId.GetHashCode();
        }
    }
}