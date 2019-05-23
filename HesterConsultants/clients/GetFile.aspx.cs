using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using System.Web.Security;
using System.Diagnostics;
using HesterConsultants.AppCode;
using HesterConsultants.Properties;

namespace HesterConsultants.clients
{
    public partial class GetFile : System.Web.UI.Page
    {
        private int fileId;
        private JobFile jobFile;
        private Client curClient;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            ClientUtils.RedirectNewOrUnauthenticatedClient();
            curClient = ClientUtils.GetClientFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (curClient == null)
                return;

            //this.Response.Cache.SetCacheability(HttpCacheability.NoCache); // messes up ie
            this.Response.ContentType = "application/octet-stream";

            string qs = this.Request.QueryString["fileId"];

            if (!String.IsNullOrEmpty(qs))
                fileId = Convert.ToInt32(qs);
            else
                return;

            GetSelectedFile();

            if (jobFile == null)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to retrieve file."));

            if (CheckSelectedFileIsForClient())
            {
                if (jobFile.Job.IsArchived)
                    throw new Exception(SiteUtils.ExceptionMessageForCustomer("File is archived or removed per file retention policy."));
                else
                    TransmitFile();
            }
            else
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("File does not show requesting client as owner."));
        }

        private void GetSelectedFile()
        {
            //jobFile = CacheLayer.JobFileFromId(fileId);
            //if (jobFile == null)
            jobFile = JobFile.JobFileFromId(fileId);

            //Debug.WriteLine("GetSelectedFile(): " + jobFile.Name);
        }

        private bool CheckSelectedFileIsForClient()
        {
            //Client jobClient = null;
            bool ret = false;

            HesterConsultants.AppCode.Entities.Job job =
                HesterConsultants.AppCode.Entities.Job.JobFromId(jobFile.JobId);

            if (job != null)
            {
                //jobClient = HesterConsultants.AppCode.CacheLayer.ClientFromId(job.ClientId);
                ret = job.ClientId == curClient.ClientId; 
            }

            return ret;
        }

        private void TransmitFile()
        {
            if (jobFile != null)
            {
                if (!jobFile.Job.IsArchived)
                {
                    this.Response.AppendHeader("Content-Disposition", "attachment; filename=" + jobFile.Name);
                    this.Response.TransmitFile(this.Server.MapPath(jobFile.Path));
                }
                else
                    throw new Exception(SiteUtils.ExceptionMessageForCustomer("File is archived or removed per file retention policy."));
            }
            //this.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}