using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HesterConsultants.AppCode;
using System.Security.Principal;
using System.Diagnostics;
using HesterConsultants.Properties;

namespace HesterConsultants.login
{
    public partial class Default : System.Web.UI.Page
    {
        private string strToken = String.Empty;
        private bool hasToken = false;
        private string newClientEmail;
        //private bool userAuthenticated;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // debug - test bad token
            //if (String.IsNullOrEmpty(this.Request.QueryString["t"]))
            //    this.Response.Redirect("/login/?t=mofo", false);

            InitLogin();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TextBox username = Login1.FindControl("UserName") as TextBox;
            if (username != null)
            {
                // iOs
                username.Attributes.Add("autocorrect", "off");
                username.Attributes.Add("autocapitalize", "off");
            }
            // username text box email
            // this doesn't work; use js
            //if (username != null)
            //{
            //    username.Attributes["type"] = "email";
            //}
        }

        protected string UserNameTextBox()
        {
            TextBox username = Login1.FindControl("UserName") as TextBox;
            if (username != null && username.Visible)
                return username.ClientID;
            else
                return String.Empty;
        }

        protected string PwTextBox()
        {
            TextBox pw = Login1.FindControl("Password") as TextBox;
            if (pw != null && pw.Visible)
                return pw.ClientID;
            else
                return String.Empty;
        }

        private void InitLogin()
        {
			// 2014
			return;
			// --------------------------------

            // check for problems before showing form

            // see if there is a token (for new clients from email)
            hasToken = RequestHasToken();

            if (!this.IsPostBack)
            {
                // redirect if already logged on 
                if (UserIsAuthenticated())
                {
                    // send token so can verify in Handler ?? 
                    this.Response.Redirect(FormsAuthentication.DefaultUrl, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                // check for bad token 
                // 1. header (redirected from handler)
                //if (this.Request.Headers["badToken"] == "true")
                //    ShowBadTokenErrorAndLogOut();

                // 2. query string
                if (hasToken)
                {
                    if (MatchingEmailExists())
                    {
                        Debug.WriteLine("matching email exists");
                        FillInEmail();
                    }
                    else
                        // nobody has that token,
                        // don't allow login
                        ShowBadTokenErrorAndLogOut();
                }
            }
        }

        private bool UserIsAuthenticated()
        {
            IPrincipal principal = this.User;
            bool ret = 
                principal != null
                && principal.Identity != null
                && principal.Identity.IsAuthenticated;

            return ret;
        }

        private bool RequestHasToken()
        {
            strToken = this.Request.QueryString["t"];

            return !String.IsNullOrEmpty(strToken);
        }

        private void ShowBadTokenErrorAndLogOut()
        {
            // show only the new client, you got the wrong token message
            this.pnlLogin.Visible = false;
            this.pnlNewAccountLink.Visible = false;
            this.pnlNewClientErrorMessage.Visible = true;

            FormsAuthentication.SignOut();
        }

        private void LogBadLoginAttempt()
        {
            // to do
        }

        private bool MatchingEmailExists()
        {
            try
            {
                newClientEmail = ClientData.Current.GetNewClientEmailFromToken(new Guid(strToken));
            }
            catch (Exception ex)
            {
                // to do - log exception
                ClientData.Current.LogErrorAndSendAlert(ex);
                return false;
            }

            return !String.IsNullOrEmpty(newClientEmail);
        }

        private void FillInEmail()
        {
            TextBox txtUserName = (TextBox)Login1.FindControl("UserName");
            txtUserName.Text = newClientEmail;
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            // check for new client that logged in without token
            // need token to verify email
            string userName = Login1.UserName;
            MembershipUser authUser = Membership.GetUser(userName);
            Guid authUserId = (Guid)authUser.ProviderUserKey;

            if (Roles.IsUserInRole(userName, Settings.Default.RoleUnauthenticatedClient))
            {
                try
                {
                    Guid guid = new Guid(strToken);
                    bool ret = ClientData.Current.VerifyNewClientToken(authUserId, new Guid(strToken)); // sets auth flag in db if good
                    Debug.WriteLine("verified token: " + ret.ToString());
                }
                catch (ArgumentNullException aex)
                {
                    ClientData.Current.LogErrorAndSendAlert(aex, userName + " tried to log on without a token.");
                }
                catch (FormatException fex)
                {
                    ClientData.Current.LogErrorAndSendAlert(fex, userName + " tried to log with token in bad format.");
                }
                

                //if (!String.IsNullOrEmpty(strToken))
                //{
                //}
            }

            // don't skip the handler
            //this.Server.Transfer(FormsAuthentication.DefaultUrl, true);
        }
    }
}