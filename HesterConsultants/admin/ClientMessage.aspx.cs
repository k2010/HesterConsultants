using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;
using HesterConsultants.Properties;
using HesterConsultants.AppCode.Entities;

namespace HesterConsultants.admin
{
    public partial class ClientMessage : System.Web.UI.Page
    {
        private Employee curAdmin;
        private Job curJob;
        private Emailer emailer;
        protected string strJobId;
        private bool success;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (curAdmin == null)
                return;

            GetJob();

            if (emailer == null)
                emailer = new Emailer(Settings.Default.SmtpHost, Settings.Default.EmailStyleTag);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetControls();
        }

        private void GetJob()
        {
            string qs = this.Request.QueryString["jobId"];
            if (!String.IsNullOrEmpty(qs))
            {
                strJobId = qs;
                int jobId = Convert.ToInt32(qs);
                curJob = CacheLayer.JobFromId(jobId);
            }
        }

        private void SetControls()
        {
            if (!this.IsPostBack)
                LoadMessageTypes();

            if (!success)
            {
                pnlEmailForm.Visible = true;
                pnlSuccess.Visible = false;
                SetJobFields();
                this.txtEmail.Text = curJob.Client.Email;
            }
            else
            {
                pnlSuccess.Controls.Clear();
                pnlSuccess.Controls.Add(new LiteralControl("Message has been sent. <a href=\"./JobAdmin.aspx?jobId=" + curJob.JobId.ToString() + "\">Return to job</a>"));
                pnlSuccess.Visible = true;
                pnlEmailForm.Visible = false;
            }

            // iOs attrs for inputs
            SiteUtils.AddTextBoxAttributes(this, "autocorrect", "off", true);
            SiteUtils.AddTextBoxAttributes(this, "autocapitalize", "off", true);
        }

        private void SetJobFields()
        {
            // other fields
            this.lblHeadingJobNumber.Text = curJob.JobId.ToString();

            this.lblStatus.Text = curJob.JobStatus.Name;

            DateTime dateSubmitted = AdminUtils.EmployeeDate(curJob.DateSubmitted, curAdmin);
            this.lblDateSubmitted.Text = dateSubmitted.ToString("M/d/yyyy h:mm tt");

            DateTime dateDue = AdminUtils.EmployeeDate(curJob.DateDue, curAdmin);
            this.lblDateDue.Text = dateDue.ToString("M/d/yyyy h:mm tt");

            if (curJob.JobStatus.IsAClosedStatus())
            {
                DateTime dateCompleted = AdminUtils.EmployeeDate(curJob.DateCompleted, curAdmin);
                this.lblStatus.Text += " at " + dateCompleted.ToString("M/d/yyyy h:mm tt");
            }

            this.lblBillingRef.Text = this.Server.HtmlEncode(curJob.BillingReference);

            this.lblJobType.Text = curJob.JobType.Name;

            this.phInstructions.Controls.Add(
                new LiteralControl(SiteUtils.SurroundTextBlocksWithHtmlTags(curJob.Instructions, "div", null)));
        }

        private void LoadMessageTypes()
        {
            this.ddMessageTypes.Items.Add(new ListItem("Select:", "-1"));

            foreach (int k in Enum.GetValues(typeof(AdminUtils.ClientMessageType)))
            {
                ListItem item = new ListItem(Enum.GetName(typeof(AdminUtils.ClientMessageType), k), k.ToString());
                this.ddMessageTypes.Items.Add(item);
            }
        }

        private void SendMessage()
        {
            Emailer emailer = new Emailer(Settings.Default.SmtpHost, Settings.Default.EmailStyleTag);

            string messageHtml = SiteUtils.SurroundTextBlocksWithHtmlTags(this.txtMessage.Text, "div", null, false); // false enables us to put html in the message
            success = emailer.SendClientMessage(curJob.Client, curAdmin.Email, this.txtSubject.Text.Trim(), messageHtml);

            //if (!success)
            //    throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to send email."));
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
    }
}