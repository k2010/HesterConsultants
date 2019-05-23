using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.IO;
using HesterConsultants.Properties;

namespace HesterConsultants.AppCode.Entities
{
    public class JobFile 
    {
        public int JobFileId { get; set; }
        internal int JobId { get; set; }
        public Job Job { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsReturnedFile { get; set; }
        public bool IsSubmittedFile { get; set; }

        public enum JobFileType
        {
            Submitted,
            Returned,
            Working
        }

        // methods
        public static JobFile JobFileFromId(int jobFileId)
        {
            Debug.WriteLine("JobFile.JobFileFromId()");

            DataRow drJobFile = ClientData.Current.JobFileFromIdDataRow(jobFileId);
            if (drJobFile != null)
            {
                JobFile jobFile = new JobFile();
                jobFile.SetFieldsFromDataRow(drJobFile);

                // get job
                jobFile.Job = Job.JobFromId(jobFile.JobId);

                return jobFile;
            }

            return null;
        }

        public static List<JobFile> JobFilesForJob(Job job)
        {
            Debug.WriteLine("JobFile.JobFilesForJob(Job " + job.JobId.ToString() + ")");

            DataTable dtJobFilesForJob = ClientData.Current.JobFilesForJobDataTable(job.JobId);
            List<JobFile> jobFiles = new List<JobFile>();

            foreach (DataRow drJobFile in dtJobFilesForJob.Rows)
            {
                JobFile jobFile = new JobFile();
                jobFile.SetFieldsFromDataRow(drJobFile);

                jobFile.Job = job;

                //if (jobFile.IsReturnedFile)
                //{
                //    if (job.ReturnedFiles == null)
                //        job.ReturnedFiles = new List<JobFile>();
                //    job.ReturnedFiles.Add(jobFile);
                //}
                //else if (jobFile.IsSubmittedFile)
                //{
                //    if (job.SubmittedFiles == null)
                //        job.SubmittedFiles = new List<JobFile>();
                //    job.SubmittedFiles.Add(jobFile);
                //}
                //else 
                //{
                //    if (job.WorkingFiles == null)
                //        job.WorkingFiles = new List<JobFile>();
                //    job.WorkingFiles.Add(jobFile);
                //}

                jobFiles.Add(jobFile);
            }

            return jobFiles;
        }

        private void SetFieldsFromDataRow(DataRow drJobFile)
        {
            this.JobFileId = (int)drJobFile["JobFileId"];
            this.JobId = (int)drJobFile["JobId"];
            this.Name = drJobFile["JobFileName"].ToString();
            this.Path = drJobFile["JobFilePath"].ToString();
            this.IsReturnedFile = (bool)drJobFile["IsReturnFile"];
            this.IsSubmittedFile = (bool)drJobFile["IsSubmittedFile"];
        }

        /// <summary>
        /// Inserts JobFile and relates it to Job. Returns the JobFile or null.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="isReturnedFile"></param>
        /// <param name="isSubmittedFile"></param>
        /// <returns></returns>
        public static JobFile InsertJobFile(Job job, string filename, string path, bool isReturnedFile, bool isSubmittedFile)
        {
            JobFile jobFile = null;

            int jobFileId = ClientData.Current.InsertJobFile(job.JobId, filename, path, isReturnedFile, isSubmittedFile);

            if (jobFileId == 0)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add job file to database."));

            jobFile = new JobFile();
            jobFile.JobFileId = jobFileId;
            jobFile.Job = job;
            jobFile.JobId = job.JobId;
            jobFile.Name = filename;
            jobFile.Path = path;
            jobFile.IsReturnedFile = isReturnedFile;
            jobFile.IsSubmittedFile = isSubmittedFile;

            if (jobFile.IsReturnedFile)
            {
                if (job.ReturnedFiles == null)
                    job.ReturnedFiles = new List<JobFile>();

                job.ReturnedFiles.Add(jobFile);
            }
            else if (jobFile.IsSubmittedFile)
            {
                if (job.SubmittedFiles == null)
                    job.SubmittedFiles = new List<JobFile>();

                job.SubmittedFiles.Add(jobFile);
            }
            else
            {
                if (job.WorkingFiles == null)
                    job.WorkingFiles = new List<JobFile>();

                job.WorkingFiles.Add(jobFile);
            }

            return jobFile;
        }

        public bool Delete()
        {
            bool ret = ClientData.Current.DeleteJobFile(this.JobFileId);
            if (ret)
            {
                string physicalPath = HttpContext.Current.Server.MapPath(this.Path);
                string trashPath = HttpContext.Current.Server.MapPath(Settings.Default.TrashFolder);
                string safeName = SiteUtils.ConflictFreeFilename(trashPath, this.Name, 2);
                string fileInTrashPath = trashPath + @"\" + safeName;

                try
                {
                    FileInfo fi = new FileInfo(physicalPath);
                    fi.MoveTo(fileInTrashPath);
                }
                catch
                {
                    throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to delete file."));
                }
            }

            return ret;
        }

        // operators
        public override bool Equals(object obj)
        {
            try
            {
                return this.JobFileId == ((JobFile)obj).JobFileId;
            }
            catch 
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.JobFileId.GetHashCode();
        }
    }
}