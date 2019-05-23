using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using System.Text;
using System.Diagnostics;

namespace HesterConsultants.admin
{
    public partial class ClientMessageTemplates : System.Web.UI.Page
    {
        private Employee curAdmin;

        protected void Page_PreInit(object sender, EventArgs e)
        {
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.ContentType = "application/json";

            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
            if (curAdmin == null)
                RespondWithError();
            else
                GetResponse();
        }

        private void GetResponse()
        {
            string strJobId = this.Request.QueryString["jobId"];
            string strMessageTypeInt = this.Request.QueryString["messageTypeId"];

            if (String.IsNullOrEmpty(strJobId) || String.IsNullOrEmpty(strMessageTypeInt))
                return;

            int intMessageType = Convert.ToInt32(strMessageTypeInt);
            AdminUtils.ClientMessageType messageType = (AdminUtils.ClientMessageType)intMessageType;

            Job job = CacheLayer.JobFromId(Convert.ToInt32(strJobId));

            string subject = AdminUtils.GetClientAlertSubject(messageType, job);
            string message = AdminUtils.GetClientAlertMessage(messageType, job);

            StringBuilder json = new StringBuilder();

            // note - this assumes the returned values from AdminUtils won't have double quotes
            json.Append("[{");
            json.Append("\"subject\": ");
            json.Append("\"" + SiteUtils.EncodeJsString(subject) + "\", ");
            json.Append("\"message\": ");
            json.Append("\"" + SiteUtils.EncodeJsString(message) + "\"");
            json.Append("}]");

            this.Response.Clear();
            Debug.WriteLine(json.ToString());
            this.Response.Write(json.ToString());
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
            // if no matches, return json with empty strings for props,
            // to prevent error (because jquery ajax expects good json)
            //if (matchingCompanies.Count == 0)
            //    json.Append("{\"id\": \"\", \"label\": \"\"}");
