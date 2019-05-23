using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using System.Web.Security;
using System.Diagnostics;
using HesterConsultants.Properties;
using HesterConsultants.admin;

namespace HesterConsultants.employees
{
    // to do - interface for file service
    //  - GetFile()
    //  - SaveFile() 
    //  - SaveNewVersion() etc
    //  - plug in provider in web.config
    public partial class GetFile : System.Web.UI.Page
    {
        private Employee curEmployee;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curEmployee = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (curEmployee == null)
                return;

            //this.Response.Cache.SetCacheability(HttpCacheability.NoCache); //messes up ie
            this.Response.ContentType = "application/octet-stream";

            string qs = this.Request.QueryString["fileId"];
            if (!String.IsNullOrEmpty(qs))
            {
                int fileId = Convert.ToInt32(qs);
                TransmitJobFile(fileId);
            }
            else // try url
            {
                string url = this.Request.QueryString["fileUrl"];
                if (!String.IsNullOrEmpty(url))
                {
                    // get filename (end of url)
                    int pos = url.LastIndexOf("/");
                    string filename = url.Substring(pos);
                    TransmitFileFromUrl(url, filename);
                }
            }
        }

        private void TransmitJobFile(int fileId)
        {
            JobFile jobFile = JobFile.JobFileFromId(fileId);

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

        private void TransmitFileFromUrl(string url, string filename)
        {
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            this.Response.TransmitFile(this.Server.MapPath(url));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}