using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HesterConsultants.AppCode.Entities;
using System.Web.Security;
using HesterConsultants.Properties;

namespace HesterConsultants.clients
{
    public static class ClientUtils
    {
        /// <summary>
        /// Check's auth user's role for client-related roles, and redirects if appropriate.
        /// </summary>
        public static void RedirectNewOrUnauthenticatedClient()
        {
            HttpContext context = HttpContext.Current;
            MembershipUser user = Membership.GetUser();

            if (user == null)
            {
                context.Response.Redirect(FormsAuthentication.LoginUrl, false);
                context.ApplicationInstance.CompleteRequest();
                return;
            }

            else if (Roles.IsUserInRole(Settings.Default.RoleUnauthenticatedClient))
            {
                FormsAuthentication.SignOut();
                context.Response.Redirect(FormsAuthentication.LoginUrl, false);
                context.ApplicationInstance.CompleteRequest();
                return;
            }

            else if (Roles.IsUserInRole(Settings.Default.RoleNewClient))
            {
                context.Response.Redirect(Settings.Default.ClientProfileUrl, false);
                context.ApplicationInstance.CompleteRequest();
            }

            else if (Roles.IsUserInRole(Settings.Default.RoleMustChangePassword))
            {
                context.Response.Redirect(Settings.Default.ChangePasswordUrl, false);
                context.ApplicationInstance.CompleteRequest();
            }
        }

        public static Client GetClientFromSessionOrLogOut()
        {
            HttpContext context = HttpContext.Current;
            Client curClient = context.Session[Global.SESSION_CLIENT] as Client;

            if (curClient == null)
            {
                FormsAuthentication.SignOut();
                context.Response.Redirect(Settings.Default.ClientLoginUrl, false);
                context.ApplicationInstance.CompleteRequest();
            }

            return curClient;
        }

        public static DateTime DateForClient(Client client, DateTime utcDate)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, TimeZoneInfo.FindSystemTimeZoneById(client.TimeZoneId));
        }

        public static DateTime UtcForClientDate(Client client, DateTime clientDate)
        {
            return TimeZoneInfo.ConvertTimeToUtc(clientDate, TimeZoneInfo.FindSystemTimeZoneById(client.TimeZoneId));
        }

        // extension method for job segment
        public static DateTime DateTimeForClient(this JobSegment segment, Client client, DateTime utcDate)
        {
            return DateForClient(client, utcDate);
        }

    }
}