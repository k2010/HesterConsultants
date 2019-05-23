using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;

namespace HesterConsultants.AppCode.Entities
{
    public class JobSegment
    {
        public int JobSegmentId { get; set; }
        public Job Job { get; set; }
        internal int JobId { get; set; }
        public Employee Employee { get; set; }
        internal int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinutesWorked { get; set; }
        public string Notes { get; set; }

        //public static List<JobSegment> SegmentsForJob(Job job)
        //{
        //    DataTable dtSegments = ClientData.Current.JobSegmentsForJobDataTable(job.JobId);

        //    if (dtSegments == null || dtSegments.Rows.Count == 0)
        //        return null;

        //    List<JobSegment> segments = new List<JobSegment>();

        //    foreach (DataRow drSegment in dtSegments.Rows)
        //    {
        //        JobSegment segment = new JobSegment();
        //        segment.Job = job;

        //        segments.Add(segment);
        //    }

        //    return segments;
        //}

        private void SetFieldsFromDataRow(DataRow drSegment)
        {
            this.JobSegmentId = Convert.ToInt32(drSegment["JobSegmentId"]);
            this.JobId = Convert.ToInt32(drSegment["JobId"]);
            this.Employee = Employee.EmployeeFromId(Convert.ToInt32(drSegment["EmployeeId"]));
            this.EmployeeId = Convert.ToInt32(drSegment["EmployeeId"]);
            if (drSegment["MinutesWorked"] != System.DBNull.Value)
                this.MinutesWorked = Convert.ToInt32(drSegment["MinutesWorked"]);
            else
                this.MinutesWorked = 0;
            this.Notes = drSegment["Notes"].ToString();
            this.StartDate = Convert.ToDateTime(drSegment["StartDate"]);
            this.EndDate = (drSegment["EndDate"] == System.DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(drSegment["EndDate"]);
        }

        public static List<JobSegment> JobSegmentsForJob(Job job)
        {
            DataTable dtSegmentsForJob = ClientData.Current.JobSegmentsForJobDataTable(job.JobId);
            List<JobSegment> segments = new List<JobSegment>();

            foreach (DataRow drSegment in dtSegmentsForJob.Rows)
            {
                JobSegment segment = new JobSegment();
                segment.SetFieldsFromDataRow(drSegment);

                segment.Job = job;

                //if (job.JobSegments == null)
                //    job.JobSegments = new List<JobSegment>();

                //job.JobSegments.Add(segment);

                segments.Add(segment);
            }

            return segments;
        }

        public static JobSegment InsertJobSegment(Job job, Employee employee, DateTime startDate, string notes)
        {
            int jobSegmentId = ClientData.Current.InsertJobSegment(job.JobId, employee.EmployeeId, startDate, notes);

            if (jobSegmentId == 0)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to insert job segment into database."));

            JobSegment segment = new JobSegment();
            segment.JobSegmentId = jobSegmentId;
            segment.Job = job;
            segment.JobId = job.JobId;
            segment.Employee = employee;
            segment.EmployeeId = employee.EmployeeId;
            segment.StartDate = startDate;
            segment.Notes = notes;
            segment.MinutesWorked = 0;

            if (job.JobSegments == null)
                job.JobSegments = new List<JobSegment>();

            job.JobSegments.Add(segment);

            return segment;
        }

        public JobSegment UpdateJobSegment(DateTime startDate, DateTime endDate, int minutesWorked, string notes)
        {
            bool ret = ClientData.Current.UpdateJobSegment(startDate, endDate, minutesWorked, notes, this.JobSegmentId);

            if (ret == false)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to update job segment in database."));

            this.StartDate = startDate;
            this.EndDate = endDate;
            this.MinutesWorked = minutesWorked;
            this.Notes = notes;

            return this;
        }

        public JobSegment ShallowCopy()
        {
            return (JobSegment)this.MemberwiseClone();
        }

        // operators
        public override bool Equals(object obj)
        {
            try
            {
                return this.JobSegmentId == ((JobSegment)obj).JobSegmentId;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.JobSegmentId.GetHashCode();
        }
    }
}