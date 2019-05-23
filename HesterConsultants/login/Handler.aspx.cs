using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HesterConsultants.AppCode;
//using System.Net;
using System.Diagnostics;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.Properties;
using HesterConsultants.clients;

namespace HesterConsultants.login
{
    public partial class Handler : System.Web.UI.Page
    {
        private MembershipUser authUser;
        private string authUserName;
        private Guid authUserId;

        protected void Page_Load(object sender, EventArgs e)
        {
			// 2014 - cancel
			return;
			//------------------------------

            Debug.WriteLine("Handler.aspx - " + this.Request.Url.ToString());
            // check 
            // 1. authenticated
            if (!User.Identity.IsAuthenticated)
            {
                Debug.WriteLine("not authenticated");
                this.Response.Redirect(FormsAuthentication.LoginUrl, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            // 2. user info
            authUser = Membership.GetUser();
            authUserName = authUser.UserName;
            authUserId = (Guid)authUser.ProviderUserKey;

            // session var
            HttpContext.Current.Session[Global.SESSION_AUTH_USER_ID] = authUserId;

            // 3. roles
            if (!HandleAdmins())
            {
                Debug.WriteLine("not an admin");
                if (!HandleEmployees())
                {
                    Debug.WriteLine("not an employee");
                    HandleClients();
                }
            }

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private bool HandleAdmins()
        {
            Debug.WriteLine("user is admin: " + User.IsInRole(Settings.Default.RoleAdmin).ToString());
            if (User.IsInRole(Settings.Default.RoleAdmin))
            {
                // session vars
                Employee curAdmin = Employee.EmployeeFromAuthUserId(authUserId);
                // admins also clients
                Client curClient = CacheLayer.ClientFromAuthUserId(authUserId);

                this.Session[Global.SESSION_EMPLOYEE] = curAdmin;
                this.Session[Global.SESSION_CLIENT] = curClient;

                string qs;
                string returnUrl = String.Empty;
                if (this.Request.UrlReferrer != null)
                {
                    Debug.WriteLine("referrer to handler: " + this.Request.UrlReferrer);
                    if (!String.IsNullOrEmpty(qs = this.Request.UrlReferrer.Query))
                    {
                        bool hasReturnUrl = qs.StartsWith("?ReturnUrl");
                        Debug.WriteLine("ReturnUrl: " + hasReturnUrl.ToString());
                        if (hasReturnUrl)
                        {
                            string[] qss = qs.Split(new char[] { '=', '&' });
                            returnUrl = this.Server.UrlDecode(qss[1]);
                        }

                        if (!String.IsNullOrEmpty(returnUrl))
                            this.Response.Redirect(returnUrl, false);
                        else
                            this.Response.Redirect(Settings.Default.AdminHomeUrl, false);
                        
                        // CompleteRequest() called above in Page_Load
                    }
                }

                this.Response.Redirect(Settings.Default.AdminHomeUrl, false);
                // CompleteRequest() called above in Page_Load
                return true;
            }

            return false;
        }

        private bool HandleEmployees()
        {
            if (User.IsInRole(Settings.Default.RoleEmployee))
            {
                // session vars

                // ees are also clients
                Employee curEmployee = Employee.EmployeeFromAuthUserId(authUserId);
                Client curClient = CacheLayer.ClientFromAuthUserId(authUserId);

                this.Session[Global.SESSION_EMPLOYEE] = curEmployee;
                this.Session[Global.SESSION_CLIENT] = curClient;

                this.Response.Redirect(Settings.Default.EmployeeHomeUrl, false);
                // CompleteRequest() called above in Page_Load
                return true;
            }

            return false;
        }

        private bool HandleClients()
        {
            // if in Unauthenticated Client role,
            // make sure they used token to log in

            bool unauthenticatedClient = User.IsInRole(Settings.Default.RoleUnauthenticatedClient);
            
            // to do - must change password 
            bool mustChangePw = User.IsInRole(Settings.Default.RoleMustChangePassword);
            
            bool newClient = User.IsInRole(Settings.Default.RoleNewClient);
            bool regularClient = User.IsInRole(Settings.Default.RoleClient);

            if (unauthenticatedClient || newClient || mustChangePw || regularClient)
            {
                // session var
                Client curClient = CacheLayer.ClientFromAuthUserId((Guid)(Membership.GetUser().ProviderUserKey)); 
                    // curClient is null for unauth or new --
                    // no Client obj until new graduates
                    // to regular via EditAccount

                this.Session[Global.SESSION_CLIENT] = curClient;

                if (unauthenticatedClient)
                {
                    if (!CheckNewClientAuthentication())
                    {
                        FormsAuthentication.SignOut();
                        this.Response.Redirect(FormsAuthentication.LoginUrl + "?t=badToken", false);
                    }
                    else
                    {
                        UpgradeUnauthenticatedClientToNewClient();
                        this.Response.Redirect(Settings.Default.ClientProfileUrl, false);
                    }
                }
                else if (newClient)
                    this.Response.Redirect(Settings.Default.ClientProfileUrl, false);
                else if (mustChangePw)
                    this.Response.Redirect(Settings.Default.ChangePasswordUrl, false);
                else // regular client
                {
                    this.Response.Redirect(Settings.Default.ClientHomeUrl, false);
                }

                // CompleteRequest() called above in Page_Load
                return true;
            }

            // not a client
            FormsAuthentication.SignOut();
            this.Response.Redirect(FormsAuthentication.LoginUrl, false);
            // CompleteRequest() called above in Page_Load
            return false;
        }

        private bool CheckNewClientAuthentication()
        {
            bool ret = ClientData.Current.VerifyNewClientIsAuthenticated(authUserId);
            Debug.WriteLine("CheckNewClientAuthentication: " + ret.ToString());

            return ret;
        }

        private void UpgradeUnauthenticatedClientToNewClient()
        {
            Roles.RemoveUserFromRole(authUserName, Settings.Default.RoleUnauthenticatedClient);
            Roles.AddUserToRole(authUserName, Settings.Default.RoleNewClient);
            Roles.AddUserToRole(authUserName, Settings.Default.RoleClient);
        }
    }
}