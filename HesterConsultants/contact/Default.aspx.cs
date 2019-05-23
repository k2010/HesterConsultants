using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;
using HesterConsultants.AppCode;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Web.Security;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.Properties;

namespace HesterConsultants.contact
{
    // create html file for message, put in uploads folder,
    // instead of sending email.
    // send company contact an alert email instead, pointing to message.

    public partial class Default : System.Web.UI.Page
    {
        private int pageId = 7;
        private string senderName;
        private string senderEmail;
        private string senderNameAndEmail; // name + <email>
        private string subject;
        private string message;
        private bool success = false;
        private string messageFilename;
        private string messageFolderName;
        private string messageFolderFullPath;
        private string messageFolderUrl;
        private ArrayList attachmentNames; // = new ArrayList();
        private ArrayList attachmentUrls;
        private DateTime nowLocal;
        private Client curClient;
        private Emailer emailer;
        private int threadId = 0;
        private int messageId = 0;
        private DateTime nowUtc;

        protected void Page_Load(object sender, EventArgs e)
        {
            // to do - make sure these are all correct throughout project:
            this.header1.PageInfo = PageInfo.ContactRelative;

            HesterConsultants.AppCode.HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();

            // check for logged in client
            if (User.Identity.IsAuthenticated)
            {
                curClient = this.Session[Global.SESSION_CLIENT] as Client;
                if (curClient != null)
                {
                    this.ccp1.Visible = true;

                    // redirect if must change pw
                    if (Roles.IsUserInRole(Settings.Default.RoleMustChangePassword))
                    {
                        this.Response.Redirect(Settings.Default.ChangePasswordUrl, false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }

            if (!this.IsPostBack)
            {
                if (curClient != null)
                {
                    this.txtName.Text = curClient.FirstName + " " + curClient.LastName;
                    this.txtEmail.Text = curClient.Email;
                }

                // job id in query string
                string jobId = this.Request.QueryString["jobId"];
                if (!String.IsNullOrEmpty(jobId))
                    this.txtSubject.Text = "Job No. " + jobId;
            }
        }

        protected string MainBodyOrMainBodyClients()
        {
            return (curClient != null) ? "mainbodyClients" : "mainbody";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // after message sending
            if (this.IsPostBack)
            {
                if (success)
                {
                    this.pnlContactTableContainer.Style.Add("display", "none");
                    this.pnlContactSendAnother.Style.Add("display", "block");

                    lblIntro.Text = "Thanks! Your message has been sent.";
                    //lblIntro.CssClass = "alertText";

                    senderName = senderEmail = senderNameAndEmail = subject = message = String.Empty;
                    txtMessage.Text = String.Empty;
                }
                else
                    this.pnlContact.Visible = false; // see exception handler
            }
        }

        private void ProcessAttachments()
        {
			// 2014 
			return;
			// ---------------------------------

            // six file inputs - check all six
            // create directory with sessionid // to do - use client id
            // within directory, ensure no filename conflicts
            // if conflict, append -01, -02 etc.
            
            // save each file
            for (int k = 1; k <= 3; k++)
            {
                FileUpload fu = (FileUpload)this.FindControl("file" + k.ToString());
                if (fu.HasFile)
                {
                    // safe
                    string filename = HesterConsultants.AppCode.SiteUtils.WindowsSafeFilename(fu.FileName);

                    // conflict free
                    filename = HesterConsultants.AppCode.SiteUtils.ConflictFreeFilename(messageFolderFullPath, filename, Settings.Default.FilenameAppendedDigitsMax);

                    //Debug.WriteLine(messageFolderFullPath + @"\" + filename);
                    try
                    {
                        fu.SaveAs(messageFolderFullPath + @"\" + filename);
                    }
                    catch
                    {
                        throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to save attachments to server."));
                    }

                    if (attachmentNames == null)
                        attachmentNames = new ArrayList();
                    if (attachmentUrls == null)
                        attachmentUrls = new ArrayList();

                    attachmentNames.Add(filename);
                    attachmentUrls.Add(messageFolderUrl + "/" + filename);
                }
            }
        }

        private void GetMessageFolder()
        {
            if (curClient != null)
                messageFolderName = "client." + curClient.ClientId.ToString();
            else
                messageFolderName = HesterConsultants.AppCode.SiteUtils.WindowsSafeFilename(this.Session.SessionID);

            messageFolderUrl = Settings.Default.ContactUploadsPath + "/" + messageFolderName;
            messageFolderFullPath = this.Server.MapPath(messageFolderUrl);
            //messageFolderFullPath = this.Server.MapPath(Settings.Default.ContactUploadsPath + @"\" + messageFolderName);
            //try
            //{
                Directory.CreateDirectory(messageFolderFullPath);
            //}
            //catch
            //{
                //ClientData.Current.LogErrorAndSendAlert(ex);
                //throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to create folder to store message."));
            //}
        }

        private void SaveCustomerMessage()
        {
            if (senderName != String.Empty)
                messageFilename = HesterConsultants.AppCode.SiteUtils.WindowsSafeFilename(senderName + "_");

            messageFilename += nowLocal.Year.ToString() + "_" + nowLocal.Month.ToString("00") + "_" + nowLocal.Day.ToString("00") + "_"
                + nowLocal.Hour.ToString("00") + "_" + nowLocal.Minute.ToString("00") + "_" + nowLocal.Second.ToString("00") + "_"
                + "message.html";

            StreamWriter swMessage = new StreamWriter(messageFolderFullPath + @"\" + messageFilename);

            swMessage.Write(HtmlMessage());

            swMessage.Close();
        }

        private void SendMessage()
        {
            string messageUrl = Settings.Default.CompanyUrlFull + messageFolderUrl + "/" + messageFilename;
            bool ret = emailer.SendContactMessageAlert(senderName, nowLocal, messageUrl);
            if (!ret)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to send message."));
            else
                success = true;
        }

        private string HtmlMessage()
        {
            StringBuilder sb = new StringBuilder();
            //string n = Environment.NewLine;

            sb.Append("<html><head><title>" + Settings.Default.CompanyName + " - New Contact Message</title>");
            sb.Append(Settings.Default.EmailStyleTag + "</head><body>");

            // header table
            sb.Append("<div>");
            sb.Append("<table>");

            // from
            sb.Append("<tr>");
            sb.Append("<td>From: </td>");
            sb.Append("<td>" + senderNameAndEmail + "</td>");
            sb.Append("</tr>");

            // date
            sb.Append("<tr>");
            sb.Append("<td>Date: </td>");
            sb.Append("<td>" + nowLocal.ToString() + "</td>");
            sb.Append("</tr>");

            // subject
            sb.Append("<tr>");
            sb.Append("<td>Subject: </td>");
            sb.Append("<td>" + subject + "</td>");
            sb.Append("</tr>");

            sb.Append("</table>");
            sb.Append("</div>");

            message = "<div>" + this.Server.HtmlEncode(txtMessage.Text).Trim().Replace(Environment.NewLine, "</div><div>") + "</div>";
            sb.Append(message);

            if (attachmentNames != null)
            {
                sb.Append("<div>Attachments:</div>");

                for (int k = 0; k < attachmentNames.Count; k++)
                {
                    sb.Append("<div><a href=\"" + Settings.Default.CompanyUrlFull + "/admin/GetFile.aspx?fileUrl=" + attachmentUrls[k] + "\">" + attachmentNames[k] + "</a></div>");
                }
            }

            sb.Append("</body></html>");

            return sb.ToString();
        }

        private void LogMessage()
        {
            threadId = ClientData.Current.InsertMessageThread(subject, nowUtc);

            if (threadId != 0)
                messageId = ClientData.Current.InsertMessage(senderName, senderEmail, nowUtc, subject, txtMessage.Text.Trim(), threadId);

            int attachmentId = 0;
            if (messageId != 0)
            {
                if (attachmentNames != null)
                    foreach (string filename in attachmentNames)
                        attachmentId = ClientData.Current.InsertMessageAttachment(filename, messageId);
            }
        }

        private void ProcessMessage()
        {
			// 2014
			return;
			// ------------------------------------

            nowUtc = DateTime.UtcNow;
            nowLocal = HesterConsultants.AppCode.SiteUtils.UtcToCompanyTime(DateTime.UtcNow);

            senderName = this.Server.HtmlEncode(txtName.Text).Trim();
            senderEmail = this.Server.HtmlEncode(txtEmail.Text.Trim());

            if (!(senderName.Equals(String.Empty)))
                senderNameAndEmail = senderName + " ";

            senderNameAndEmail += "&lt;" + senderEmail + "&gt;";

            subject = this.Server.HtmlEncode(this.txtSubject.Text);

            emailer = new Emailer(Settings.Default.SmtpHost, Settings.Default.EmailStyleTag);
            GetMessageFolder();
            ProcessAttachments();
            SaveCustomerMessage();
            SendMessage();
            LogMessage();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            ProcessMessage();
        }
    }
}
