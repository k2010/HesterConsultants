using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HesterConsultants.AppCode.Entities;
using System.Web.Security;
using HesterConsultants.Properties;
using System.Diagnostics;

namespace HesterConsultants.admin
{
    public static class AdminUtils
    {
        public enum ClientMessageType
        {
            Estimate = 0,
            JobDeclined = 1,
            JobCanceled = 2,
            JobCompleted = 3,
            General = 4
        }

        //public static Client GetCurrentAdmin()
        //{
        //    HttpContext context = HttpContext.Current;

        //    Client curAdmin = context.Session[Global.SESSION_CLIENT] as Client;
        //    if (curAdmin == null)
        //    {
        //        FormsAuthentication.SignOut();
        //        context.Response.Redirect(Settings.Default.ClientLoginUrl, true);
        //        return null;
        //    }

        //    return curAdmin;
        //}

        /// <summary>
        /// Gets current Employee from Session. If null, signs out and redirects to login page.
        /// </summary>
        /// <returns></returns>
        public static Employee GetEmployeeFromSessionOrLogOut()
        {
            HttpContext context = HttpContext.Current;
            Employee curEmployee = context.Session[Global.SESSION_EMPLOYEE] as Employee;

            if (curEmployee == null)
            {
                FormsAuthentication.SignOut();
                context.Response.Redirect(Settings.Default.ClientLoginUrl, false);
                context.ApplicationInstance.CompleteRequest();
            }

            return curEmployee;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="whichPath">Either "employees" or "clients".</param>
        /// <returns></returns>
        public static string LinkToGetFilePage_Text(JobFile file, string whichPath)
        {
            return OpenLinkToGetFilePage(file, whichPath) + file.Name + "</a>";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="whichPath">Either "employees" or "clients".</param>
        /// <returns></returns>
        public static string LinkToGetFilePage_Image(JobFile file, string whichPath)
        {
            return OpenLinkToGetFilePage(file, whichPath) + FileIcon() + "</a>";
        }

        public static string FileIcon()
        {
            return "<span style=\"vertical-align: bottom;\"><img src=\"/images/file_trans.png\" width=\"24\" height=\"27\" alt=\"\" />" + "</span>";
        }

        //private static string OpenLinkToGetFilePage(JobFile file)
        //{
        //    // with rel. path, this can be used for either /admin/GetFile.aspx or /clients/GetFile.aspx
        //    return "<a class=\"fileLink\" href=\"./GetFile.aspx?fileId=" + file.JobFileId.ToString() + "\">";

        //}

        private static string OpenLinkToGetFilePage(JobFile file, string folder)
        {
            string s = "<a class=\"fileLink\" href=\"/" + folder + "/GetFile.aspx"
                + "?fileId=" + file.JobFileId.ToString()
                + "\">";

            return s;
        }

        public static string StandardDateFormat(DateTime dt, bool includeLineBreak)
        {
            if (dt.Equals(DateTime.MinValue))
                return String.Empty;

            string dateFormat = dt.ToString("M/d/yyyy");
            
            if (includeLineBreak)
                dateFormat += "<br />";
            else
                dateFormat += " ";

            dateFormat += dt.ToString("h:mm tt");

            return dateFormat;
        }

        public static DateTime EmployeeDate(DateTime utcDate, Employee ee)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, TimeZoneInfo.FindSystemTimeZoneById(ee.TimeZoneId));
        }

        public static DateTime UtcOfEmployeeDate(DateTime eeDate, Employee ee)
        {
            return TimeZoneInfo.ConvertTimeToUtc(eeDate, TimeZoneInfo.FindSystemTimeZoneById(ee.TimeZoneId));
        }

        public static string GetClientAlertSubject(ClientMessageType alertType, Job job)
        {
            string subject = String.Empty;

            switch (alertType)
            {
                case ClientMessageType.Estimate:
                    subject = "Estimate for Work Request - No. " + job.JobId.ToString();
                    break;

                case ClientMessageType.JobCanceled:
                    subject = "Notice of Job Cancellation - Job No. " + job.JobId.ToString();
                    break;

                case ClientMessageType.JobCompleted:
                    subject = "Notice of Job Completion - Job No. " + job.JobId.ToString();
                    break;

                case ClientMessageType.JobDeclined:
                    subject = "Unable to Accept Work Request - No. " + job.JobId.ToString();
                    break;

                case ClientMessageType.General:
                    subject = "Notice Regarding Job No. " + job.JobId.ToString();
                    break;
            }

            return subject;
        }

        public static string GetClientAlertTitle(ClientMessageType alertType, Job job)
        {
            string title = String.Empty;

            switch (alertType)
            {
                case ClientMessageType.Estimate:
                    title = "Estimate for Work Request";
                    break;

                case ClientMessageType.JobCanceled:
                    title = "Notice of Job Cancellation";
                    break;

                case ClientMessageType.JobCompleted:
                    title = "Notice of Job Completion";
                    break;

                case ClientMessageType.JobDeclined:
                    title = "Unable to Accept Work Request";
                    break;

                case ClientMessageType.General:
                    title = "Notice Regarding Your Job";
                    break;
            }

            return title;
        }

        public static string GetClientAlertMessage(ClientMessageType alertType, Job job)
        {
            string message = String.Empty;
            string n = Environment.NewLine;

            switch (alertType)
            {
                case ClientMessageType.Estimate:
                    message += Settings.Default.CompanyName + " is pleased to offer the following estimate for Work Request No. " + job.JobId.ToString() + "." + n;
                    message += "Our estimated total fees for this Work Request are: " + job.Estimate.ToString("c") + "." + n;
                    message += "Please reply to this message to accept, decline, or discuss our estimate.";
                    break;

                case ClientMessageType.JobCanceled:
                    message += "As requested, we have stopped work on Job No. " + job.JobId.ToString() + "." + n;
                    message += "Please reply to this message if you have any questions.";
                    break;

                case ClientMessageType.JobCompleted:
                    message += "Job No. " + job.JobId.ToString() + " has been completed." + n;
                    message += "Please log in to " + Settings.Default.CompanyUrlFull + " to pick up this job.";
                    break;

                case ClientMessageType.JobDeclined:
                    message += Settings.Default.CompanyName + " is sorry to inform you that we are unable to accept Work Request No. " + job.JobId.ToString() + "." + n;
                    message += "Please reply to this message if you have any questions.";
                    break;

                case ClientMessageType.General:
                    message += "Notice regarding Job No. " + job.JobId.ToString() + ":";
                    break;
            }

            return message;
        }

        /// <summary>
        /// Removes stop words defined in config file, and also removes single characters.
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static IList<string> RemoveStopWords(IList<String> terms)
        {
            string[] stopWords = Settings.Default.JobSearchStopWordList.Split(new string[] { ", ", "," }, StringSplitOptions.RemoveEmptyEntries);

            // remove stopwords, single letters
            for(int k = terms.Count - 1; k >= 0; k--)
                if (stopWords.Contains(terms[k].ToLower()) || terms[k].Length < 2)
                {
                    Debug.WriteLine("removing " + terms[k]);
                    terms.RemoveAt(k);
                }

            return terms;
        }
    }
}