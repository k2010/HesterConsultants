using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;

namespace HesterConsultants.admin
{
    public partial class GetJobStatusIsClosed : System.Web.UI.Page
    {
        private Employee curEmployee;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curEmployee = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.ContentType = "text/plain";

            if (curEmployee == null)
                RespondWithError();
            else
                GetResponse();
        }

        private void GetResponse()
        {
            string ret = "false";
            string qs = this.Request.QueryString["jobStatusId"];

            if (String.IsNullOrEmpty(qs))
            {
                this.Response.Clear();
                this.Response.Write("false");
                //this.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            int jobStausId = Convert.ToInt32(qs);
            JobStatus jobStatus = JobStatus.JobStatusFromId(jobStausId);

            if (jobStatus.IsAClosedStatus())
                ret = "true";

            this.Response.Clear();
            this.Response.Write(ret);
            //this.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void RespondWithError()
        {
            this.Response.Clear();
            this.Response.Write("Error");
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}