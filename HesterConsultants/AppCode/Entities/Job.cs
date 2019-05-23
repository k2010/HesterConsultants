using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using HesterConsultants.admin;
using System.Diagnostics;

namespace HesterConsultants.AppCode.Entities
{
    public class Job
    {
        public int JobId { get; set; }
        public Client Client { get; set; }
        internal int ClientId { get; set; }
        public string BillingReference { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime DateDue { get; set; }
        public DateTime OriginalDateDue { get; set; }
        public DateTime DateCompleted { get; set; }
        internal int JobTypeId { get; set; }
        public JobType JobType { get; set; }
        public string ToApplication { get; set; }
        public bool Formatted { get; set; }
        public bool Proofread { get; set; }
        public string Instructions { get; set; }
        public IList<JobSegment> JobSegments { get; set; }
        public IList<JobFile> SubmittedFiles { get; set; }
        public IList<JobFile> ReturnedFiles { get; set; }
        public IList<JobFile> WorkingFiles { get; set; }
        public IList<JobStatusChange> JobStatusChanges { get; set; }
        public decimal Estimate { get; set; }
        public decimal FinalCharge { get; set; }
        public decimal Taxes { get; set; }
        public bool PickedUp { get; set; }
        public string DeliveryNotes { get; set; }
        public bool IsArchived { get; set; }
        public Invoice Invoice { get; set; }
        internal int InvoiceId { get; set; }


        // ??:
        //public AlertMethod AlertMethod { get; set; }
        //public bool AgreementSigned { get; set; }

        /// <summary>
        /// Calculated from most recent JobStatusChange
        /// </summary>
        public JobStatus JobStatus
        {
            get
            {
                JobStatusChange change = null;
                if (this.JobStatusChanges != null)
                    change = (JobStatusChange)(this.JobStatusChanges.OrderByDescending(jsc => jsc.Date).FirstOrDefault());
                if (change != null)
                    return change.JobStatus;
                else
                    return JobStatus.TechnicalDifficulty;
            }
        }

        // constructors
        public Job()
        {
        }

        // methods

        public Job ShallowCopy()
        {
            // make a copy of curJob
            return (Job)this.MemberwiseClone();
        }

        public static Job JobFromId(int jobId)
        {
            Job job = null;
            DataRow drJob = ClientData.Current.JobFromIdDataRow(jobId);

            if (drJob != null)
            {
                job = new Job();
                job.SetFieldsFromDataRow(drJob);
            }

            return job;
        }

        public static IList<Job> OpenAndRecentJobs(DateTime asOfDate)
        {
            Debug.WriteLine("Job.OpenAndRecentJobs()");

            DataTable dtOpenRecentJobs = ClientData.Current.RecentJobsDataTable(asOfDate);
            List<Job> jobs = new List<Job>();

            foreach (DataRow drJob in dtOpenRecentJobs.Rows)
            {
                Job job = new Job();
                job.SetFieldsFromDataRow(drJob);
                jobs.Add(job);
            }

            return jobs;
        }

        public static IList<Job> JobsForInvoice(Invoice invoice)
        {
            Debug.WriteLine("Job.JobsForInvoice()");

            DataTable dtJobsForInvoice = ClientData.Current.JobsFromInvoiceIdDataTable(invoice.InvoiceId);
            List<Job> jobs = new List<Job>();

            foreach (DataRow drJob in dtJobsForInvoice.Rows)
            {
                Job job = new Job();
                job.SetFieldsFromDataRow(drJob);

                job.Invoice = invoice;
                job.Client = invoice.Client;
                job.JobType = JobType.JobTypeFromId(job.JobTypeId);

                jobs.Add(job);
            }

            return jobs;
        }

        public static IList<Job> UninvoicedJobsForClient(Client client)
        {
            Debug.WriteLine("Job.UninvoicedJobsForClient()");

            DataTable dtJobsForInvoice = ClientData.Current.UninvoicedJobsForClientDataTable(client.ClientId);
            List<Job> jobs = new List<Job>();

            foreach (DataRow drJob in dtJobsForInvoice.Rows)
            {
                Job job = new Job();
                job.SetFieldsFromDataRow(drJob);

                job.Client = client;
                job.JobType = JobType.JobTypeFromId(job.JobTypeId);

                // only add if job is finished
                if (job.JobStatus.IsAClosedStatus())
                    jobs.Add(job);
            }

            return jobs;
        }

        public static IList<Job> JobsFromSearch(Client client, List<string> searchTerms, bool isAdmin, string sortExpression = "")
        {
            Debug.WriteLine("Job.JobsFromSearch()");

            if (searchTerms.Count == 0)
                return null;

            DataTable dtJobsFromSearch = ClientData.Current.JobsFromSearchTermDataTable(client.ClientId, searchTerms, isAdmin);
            List<Job> jobs = new List<Job>();

            foreach (DataRow drJob in dtJobsFromSearch.Rows)
            {
                Job job = new Job();
                job.SetFieldsFromDataRow(drJob);

                // need to do parent objs here
                job.Client = client;
                job.JobType = JobType.JobTypeFromId(job.JobTypeId);
                jobs.Add(job);
            }

            // remove dupes (caused by join query)
            jobs = jobs.Distinct().ToList();

            if (!String.IsNullOrEmpty(sortExpression))
            {
                ObjectSortField osf;
                if (sortExpression.ToUpper().EndsWith(" DESC"))
                    osf = new ObjectSortField(sortExpression.Substring(0, sortExpression.Length - 5), ObjectSortField.SortOrders.Descending);
                else
                    osf = new ObjectSortField(sortExpression, ObjectSortField.SortOrders.Ascending);
                jobs.Sort(new ObjectComparer<Job>(new ObjectSortField[] { osf }));
            }

            return jobs;
        }

        public bool SetInvoice(Invoice invoice)
        {
            return ClientData.Current.UpdateJobSetInvoice(this.JobId, invoice.InvoiceId);
        }

        private void SetFieldsFromDataRow(DataRow drJob)
        {
            object temp;

            this.JobId = (int)drJob["JobId"];
            this.ClientId = (int)drJob["ClientId"];
            this.BillingReference = drJob["BillingReference"].ToString();
            //this.JobStatusId = (int)drJob["JobStatusId"];
            this.JobTypeId = (int)drJob["JobTypeId"];
            this.ToApplication = drJob["ToApplication"].ToString();
            this.Formatted = (bool)drJob["Formatted"];
            this.Proofread = (bool)drJob["Proofread"];
            this.DateDue = Convert.ToDateTime(drJob["DateDue"]);
            this.OriginalDateDue = Convert.ToDateTime(drJob["OriginalDateDue"]);
            this.DateSubmitted = Convert.ToDateTime(drJob["DateSubmitted"]);
            if ((temp = drJob["DateCompleted"]) != DBNull.Value)
                this.DateCompleted = Convert.ToDateTime(temp);
            else
                this.DateCompleted = DateTime.MinValue;
            this.Instructions = drJob["Instructions"].ToString();
            this.Estimate = (decimal)drJob["Estimate"];
            this.FinalCharge = (decimal)drJob["FinalCharge"];
            this.Taxes = (decimal)drJob["Taxes"];
            this.PickedUp = (bool)drJob["PickedUp"];
            this.DeliveryNotes = drJob["DeliveryNotes"].ToString();
            this.IsArchived = (bool)drJob["IsArchived"];
            if ((temp = drJob["InvoiceId"]) != DBNull.Value)
                this.InvoiceId = (int)temp;
            else
                this.InvoiceId = 0;

            // get jobtype - no circular calls
            this.JobType = JobType.JobTypeFromId(this.JobTypeId);

            // don't need parent objects (Client, Invoice) at this point
            // that would cause circular calls
            // set those later, depending on what is calling

            // get child objects
            // that are relevant only in the context of a Job.
            this.JobStatusChanges = JobStatusChange.JobStatusChangesForJob(this);
            this.JobSegments = JobSegment.JobSegmentsForJob(this);

            //JobFile.JobFilesForJob(this);
            List<JobFile> files = JobFile.JobFilesForJob(this);
            foreach (JobFile file in files)
            {
                if (file.IsReturnedFile)
                {
                    if (this.ReturnedFiles == null)
                        this.ReturnedFiles = new List<JobFile>();
                    this.ReturnedFiles.Add(file);
                }
                else if (file.IsSubmittedFile)
                {
                    if (this.SubmittedFiles == null)
                        this.SubmittedFiles = new List<JobFile>();
                    this.SubmittedFiles.Add(file);
                }
                else // working
                {
                    if (this.WorkingFiles == null)
                        this.WorkingFiles = new List<JobFile>();
                    this.WorkingFiles.Add(file);
                }
            }
        }

        // operators
        public override bool Equals(object obj)
        {
            try
            {
                return this.JobId == ((Job)obj).JobId;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.JobId.GetHashCode();
        }
    }
}