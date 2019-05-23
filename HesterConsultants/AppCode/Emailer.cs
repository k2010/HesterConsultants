using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using HesterConsultants.AppCode.Entities;
using System.Text;
using HesterConsultants.Properties;
using System.Collections;
using System.Web.Security;

namespace HesterConsultants.AppCode
{
    public class Emailer
    {
        public string SmtpHost { get; set; }
        public string StyleTag { get; set; } // note - it's html-encoded in config file, must decode

        public Emailer(string smtpHost, string styleTag)
        {
            this.SmtpHost = smtpHost;
            this.StyleTag = styleTag;
        }

        public bool SendMessage(MailAddress from, MailAddress replyTo, MailAddressCollection tos, MailAddressCollection ccs, MailAddressCollection bccs, string subject, string body, bool isHtml)
        {
            bool ret = false;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = from;
            foreach (MailAddress addr in tos)
                msg.To.Add(addr);
            if (ccs != null)
            {
                foreach (MailAddress addr in ccs)
                    msg.CC.Add(addr);
            }
            if (bccs != null)
            {
                foreach (MailAddress addr in bccs)
                    msg.Bcc.Add(addr);
            }
            msg.Subject = subject;
            msg.IsBodyHtml = isHtml;
            msg.Body = body;

            SmtpClient client = new SmtpClient(this.SmtpHost);
            client.Credentials = new System.Net.NetworkCredential(Settings.Default.EmailUser, Settings.Default.EmailPw);
            //try
            //{
                client.Send(msg);
                ret = true;
            //}
            //catch (Exception e)
            //{
            //    ClientData.Current.LogErrorAndSendAlert(e);
            //    //SiteUtils.LogError("**Couldn't send email** " + e.Message,
            //    //    DateTime.Now.ToUniversalTime());
            //    return false;
            //}

            return ret;
        }

        public bool SendNewClientWelcomeEmail(string newClientEmail, string strToken)
        {
            MailAddress from = new MailAddress(Settings.Default.CustomerServiceEmail);
            MailAddressCollection tos = new MailAddressCollection();
            tos.Add(new MailAddress(newClientEmail));
            string subject = "Welcome to " + Settings.Default.CompanyDomain;

            string message = "<div>Thank you for registering at " + Settings.Default.CompanyDomain + ".</div>"
                + "<div>Please click the link below (or copy and paste it into your browser's address bar with no line breaks or spaces) to verify this email address and complete your registration.</div>"
                + "<div style=\"white-space: nowrap;\"><a href=\"" + Settings.Default.CompanyUrlFull + Settings.Default.ClientLoginUrl + "?t=" + strToken + "\">"
                + Settings.Default.CompanyUrlFull 
                + Settings.Default.ClientLoginUrl + "...</a></div>"
                + "<div>(If you did not use this email address to register, then someone else used your email address. You may inform us of this situation, if you wish, in a reply to this message. However, the person will not be able to use our site without visiting the link above.)</div>";

            string body = StandardHtmlFormattedMessage("Welcome to " + Settings.Default.CompanyDomain, message, true);

            return SendMessage(from, null, tos, null, null, subject, body, true);
        }

        public bool SendNewClientAlertEmail(string newUserEmail)
        {
            // to principal
            MailAddress from = new MailAddress(Settings.Default.CustomerServiceEmail);
            MailAddressCollection tos = new MailAddressCollection();
            tos.Add(new MailAddress(Settings.Default.CompanyContactEmail));
            string subject = "New Client Signup";
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>Client: " + newUserEmail + "</div>");
            sb.Append("<div><a href=\"");
            sb.Append(Settings.Default.CompanyUrlFull + FormsAuthentication.LoginUrl);
            sb.Append("\">Log In</a></div>");

            string message = sb.ToString();
            string body = StandardHtmlFormattedMessage("New Client Alert", message, false);

            return SendMessage(from, null, tos, null, null, subject, body, true);
        }

        public bool SendReceiptAcknowledgment(Job job)
        {
            // send to client
            MailAddress from = new MailAddress(Settings.Default.CustomerServiceEmail);
            MailAddressCollection tos = new MailAddressCollection();
            tos.Add(new MailAddress(job.Client.Email));
            string subject = "Acknowledgment of Receipt - Work Request No. " + job.JobId.ToString();

            StringBuilder sbMessage = new StringBuilder();

            //sbMessage.Append("<h1>Acknowledgment of Receipt</h1>");
            sbMessage.Append("<div>" + Settings.Default.CompanyNameShort + " has received your Work Request No. ");
            sbMessage.Append(job.JobId.ToString());
            sbMessage.Append(". We will review the request and contact you with our estimate for the job.</div>");

            string message = sbMessage.ToString();
            string body = StandardHtmlFormattedMessage("Acknowledgment of Receipt of Work Request", message, true);

            return SendMessage(from, null, tos, null, null, subject, body, true);
        }

        public bool SendReceiptAlert(Job job)
        {
            // to us
            MailAddress from = new MailAddress(Settings.Default.CustomerServiceEmail);
            MailAddressCollection tos = new MailAddressCollection();
            tos.Add(new MailAddress(Settings.Default.CompanyContactEmail));
            string subject = "New Work Request - No. " + job.JobId.ToString();
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>Job No. " + job.JobId.ToString() + "</div>");
            sb.Append("<div>Client: " + job.Client.FirstName + " " + job.Client.LastName + "</div>");
            sb.Append("<div><a href=\"");
            sb.Append(Settings.Default.CompanyUrlFull + FormsAuthentication.LoginUrl);
            sb.Append("\">Log In</a></div>");

            string message = sb.ToString();
            string body = StandardHtmlFormattedMessage("New Work Request Alert", message, false);

            return SendMessage(from, null, tos, null, null, subject, body, true);
        }

        public bool SendArrivalEmail(string remoteAddr, string requestedUrl)
        {
            MailAddress from = new MailAddress(Settings.Default.CustomerServiceEmail);
            MailAddressCollection tos = new MailAddressCollection();
            tos.Add(new MailAddress(Settings.Default.CompanyContactEmail));
            string subject = "Arrival @ " + Settings.Default.CompanyDomain;

            StringBuilder sbMessage = new StringBuilder();

            sbMessage.Append("<div>");
            sbMessage.Append(requestedUrl + " &#150; ");
            sbMessage.Append("<a href = \"" + SiteUtils.WHOIS_LOOKUP_URL);
            sbMessage.Append(remoteAddr + "\"");
            sbMessage.Append(" target = \"whoisWindow\">");
            sbMessage.Append(remoteAddr);
            sbMessage.Append("</a>");
            sbMessage.Append("</p>");

            string message = sbMessage.ToString();
            string body = StandardHtmlFormattedMessage("Arrival @ " + Settings.Default.CompanyDomain, message, false);

            return SendMessage(from, null, tos, null, null, subject, body, true);
        }

        public bool SendContactMessageAlert(string senderName, DateTime date, string messageUrl)
        {
            MailAddress from = new MailAddress(Settings.Default.CustomerServiceEmail, "Customer Service (for " + senderName + ")");
            MailAddressCollection tos = new MailAddressCollection();
            tos.Add(new MailAddress(Settings.Default.CompanyContactEmail));
            string subject = "New contact message";
            StringBuilder sbMessage = new StringBuilder();

            string message = "<div>Read message at " + messageUrl + ".</div>";
            string body = StandardHtmlFormattedMessage("New contact message", message, false);

            return SendMessage(from, null, tos, null, null, subject, body, true);
        }

        public bool SendClientMessage(Client client, string senderEmail, string subject, string messageHtml)
        {
            MailAddress from = new MailAddress(senderEmail);
            MailAddressCollection tos = new MailAddressCollection();
            tos.Add(client.Email);

            // bcc me
            MailAddressCollection bccs = new MailAddressCollection();
            bccs.Add(new MailAddress(Settings.Default.CompanyContactEmail));

            // use subject for title
            string title = HttpContext.Current.Server.HtmlEncode(subject);

            string body = StandardHtmlFormattedMessage(title, messageHtml, true);

            return SendMessage(from, null, tos, null, bccs, subject, body, true);
        }

        /// <summary>
        /// Supplies HTML open and close, including the &lt;body&gt; tags. Pass title and the HTML to go between the &lt;body&gt; tags.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="messageHtml"></param>
        /// <param name="toClient">Whether this message is going to a client.</param>
        /// <returns></returns>
        private string StandardHtmlFormattedMessage(string title, string messageHtml, bool toClient)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><title>");
            sb.Append(title);
            sb.Append("</title>");
            sb.Append(HttpContext.Current.Server.HtmlDecode(this.StyleTag));
            sb.Append("</head><body>");

            if (Settings.Default.UseDumbOutlookFormatting)
                sb.Append("<font face=\"" + Settings.Default.DefaultEmailFontList + "\">");
            
            sb.Append(messageHtml);

            if (toClient)
            {
                sb.Append("<div>Thank you,<br />"
                    + Settings.Default.CompanyName
                    + "<br /><a href=\"" + Settings.Default.CompanyUrlFull
                    + "\">" + Settings.Default.CompanyUrlShort + "</a>"
                    + "<br />" + Settings.Default.CompanyContactPhone
                    + "</div>");
            }

            if (Settings.Default.UseDumbOutlookFormatting)
                sb.Append("</font>");

            sb.Append("</body></html>");

            return sb.ToString();
        }

        public static string EmailConfidentialityMessage()
        {
            string message = "<div>This email may contain material that is confidential, privileged and/or attorney work product for the sole use of the intended recipient. Any review, reliance or distribution by others or forwarding without express permission is strictly prohibited. If you are not the intended recipient, please contact the sender and delete all copies of this email and its attachments from your system.</div>";

            message += "<div>" + Settings.Default.CompanyName + "</div>";

            return message;
        }

        public bool SendErrorAlert(Exception ex, string extraInfo)
        {
            HttpServerUtility server = HttpContext.Current.Server;

            MailAddress from = new MailAddress(Settings.Default.CustomerServiceEmail);
            MailAddressCollection tos = new MailAddressCollection();
            tos.Add(new MailAddress(Settings.Default.CompanyContactEmail));
            string title = "Exception Alert";
            string subject = "Exception Alert";
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>" + server.HtmlEncode(ex.Message) + "</div>");
            if (!String.IsNullOrEmpty(extraInfo))
            {
                sb.Append("<div>Extra info:</div>");
                sb.Append("<div>" + SiteUtils.SurroundTextBlocksWithHtmlTags(extraInfo, "div", null) + "</div>");
            }
            sb.Append("<div>Exception data:</div><table>");
            foreach (DictionaryEntry entry in ex.Data)
            {
                sb.Append("<tr>");

                sb.Append("<td style=\"border-bottom: solid 1px #cccccc;\">");
                sb.Append(server.HtmlEncode(entry.Key.ToString()));
                sb.Append("</td>");

                sb.Append("<td style=\"border-bottom: solid 1px #cccccc;\">");
                sb.Append(server.HtmlEncode(entry.Value.ToString()));
                sb.Append("<td>");

                sb.Append("</tr>");
            }
            sb.Append("</table>");

            string message = sb.ToString();
            string body = StandardHtmlFormattedMessage(title, message, false);

            return SendMessage(from, null, tos, null, null, subject, body, true);
        }
    }
}