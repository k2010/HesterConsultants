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
    public class JobType
    {
        public int JobTypeId { get; set; }
        public string Name { get; set; }

        private static List<JobType> allJobTypes;

        public JobType()
        {
        }

        public JobType(int id, string name)
        {
            this.JobTypeId = id;
            this.Name = name;
        }

        public static JobType Development
        {
            get
            {
                return AllJobTypes().Where(jt => jt.JobTypeId == Settings.Default.JobTypeDevelopmentId)
                    .DefaultIfEmpty(JobType.Other)
                    .First();
            }
        }

        public static JobType Consulting
        {
            get
            {
                return AllJobTypes().Where(jt => jt.JobTypeId == Settings.Default.JobTypeConsultingId)
                    .DefaultIfEmpty(JobType.Other)
                    .First();
            }
        }

        public static JobType Conversion
        {
            get
            {
                return AllJobTypes().Where(jt => jt.JobTypeId == Settings.Default.JobTypeConversionId)
                    .DefaultIfEmpty(JobType.Other)
                    .First();
            }
        }

        public static JobType Editing
        {
            get
            {
                return AllJobTypes().Where(jt => jt.JobTypeId == Settings.Default.JobTypeEditingId)
                    .DefaultIfEmpty(JobType.Other)
                    .First();
            }
        }

        public static JobType Other
        {
            get
            {
                return new JobType(Settings.Default.JobTypeOtherId, "Other");
            }
        }

        // methods
        public static List<JobType> AllJobTypes()
        {
            if (allJobTypes == null)
                RefreshAllJobTypes();

            return allJobTypes;
        }

        public static JobType JobTypeFromId(int id)
        {
            return AllJobTypes().FirstOrDefault(jt => jt.JobTypeId == id);
        }

        public static int MinJobTypeId()
        {
            return AllJobTypes().Min(jt => jt.JobTypeId);
        }

        public static int MaxJobTypeId()
        {
            return AllJobTypes().Max(jt => jt.JobTypeId);
        }

        private static void RefreshAllJobTypes()
        {
            DataTable dtJobTypes = ClientData.Current.AllJobTypesDataTable();
            allJobTypes = new List<JobType>();

            foreach (DataRow drJobType in dtJobTypes.Rows)
            {
                JobType jobType = new JobType();
                jobType.JobTypeId = (int)drJobType["JobTypeId"];
                jobType.Name = drJobType["JobTypeName"].ToString();

                allJobTypes.Add(jobType);
            }
        }

        // operators
        public override bool Equals(object obj)
        {
            try
            {
                return this.JobTypeId == ((JobType)obj).JobTypeId;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.JobTypeId.GetHashCode();
        }
    }
}