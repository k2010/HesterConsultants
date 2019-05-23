using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using System.Text;
using System.Web.Security;
using HesterConsultants.Properties;

namespace HesterConsultants.admin
{
    public partial class Cache : System.Web.UI.Page
    {
        private Employee curAdmin;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // show clients
            List<Client> clients = CacheLayer.AllClients();
            StringBuilder sbClients = new StringBuilder();

            foreach (Client client in clients)
            {
                sbClients.Append("<p>"
                    + client.ClientId
                    + " "
                    + this.Server.HtmlEncode(client.FirstName)
                    + " "
                    + this.Server.HtmlEncode(client.LastName)
                    + " - "
                    + this.Server.HtmlEncode(client.Email)
                    + "</p>");
            }

            this.phClients.Controls.Add(new LiteralControl(sbClients.ToString()));

            // jobs
            List<Job> jobs = CacheLayer.RecentJobs();
            StringBuilder sbJobs = new StringBuilder();

            foreach (Job job in jobs)
            {
                sbJobs.Append("<p>"
                    + job.JobId.ToString()
                    + " - "
                    + SiteUtils.SurroundTextBlocksWithHtmlTags(job.Instructions, "div", null)
                    + " - "
                    + job.JobStatus.Name
                    + "</p>");
            }

            this.phJobs.Controls.Add(new LiteralControl(sbJobs.ToString()));
        }


        protected void btnClearCache_Click(object sender, EventArgs e)
        {
            CacheLayer.ClearCache();
        }
    }
}