using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Web.Security;
using HesterConsultants.AppCode;
using HesterConsultants.Properties;

namespace HesterConsultants.NewClient
{
    public partial class Default : System.Web.UI.Page
    {
        private string userEmail;
        private Guid authUserId;
        private Guid token;
        private Emailer emailer;

        protected void Page_Load(object sender, EventArgs e)
        {
            // hide intro and footnotes after first wizard step
            if (this.IsPostBack)
            {
                this.pnlNewAccountMessage.Visible = false;
                this.pnlFootnotes.Visible = false;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            SiteUtils.AddTextBoxAttributes(this, "autocorrect", "off", true);
            SiteUtils.AddTextBoxAttributes(this, "autocapitalize", "off", true);

            //TextBox txtEmail = this.userWiz1.CreateUserStep.ContentTemplateContainer.FindControl("UserName") as TextBox;
            //if (txtEmail != null)
            //{
            //    txtEmail.Attributes.Add("autocorrect", "off");
            //    txtEmail.Attributes.Add("autocapitalize", "off");
            //}
        }

        protected string PasswordControlId()
        {
            TextBox txtPw = this.userWiz1.CreateUserStep.ContentTemplateContainer.FindControl("Password") as TextBox;
            if (txtPw != null)
                return txtPw.ClientID;
            else
                return String.Empty;
        }

        protected string EmailControlId()
        {
            TextBox txtEmail = this.userWiz1.CreateUserStep.ContentTemplateContainer.FindControl("UserName") as TextBox;
            if (txtEmail != null)
                return txtEmail.ClientID;
            else
                return String.Empty;
        }

        private void SetUserData()
        {
            Debug.WriteLine("called SetUserData()");
            MembershipUser user = Membership.GetUser(userWiz1.UserName);
            
            // copy email to email field
            user.Email = user.UserName;
            
            // get fields
            userEmail = user.Email;
            authUserId = (Guid)user.ProviderUserKey;

            // add unauthenticated client role
            Roles.AddUserToRole(user.UserName, Settings.Default.RoleUnauthenticatedClient);

            Membership.UpdateUser(user);
        }

        protected void userWiz1_ContinueButtonClick(object sender, EventArgs e)
        {
            SetUserData();
            InsertNewClientToken();
            SendWelcomeEmail();
            SendNewClientAlertEmail();
        }

        private void InsertNewClientToken()
        {
            try
            {
                token = ClientData.Current.InsertNewClientToken(authUserId);
            }
            catch (Exception ex)
            {
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to insert new client token in database. " + ex));
            }
        }

        private void SendWelcomeEmail()
        {
            if (emailer == null)
                emailer = new Emailer(Settings.Default.SmtpHost, Settings.Default.EmailStyleTag);

            emailer.SendNewClientWelcomeEmail(userEmail, token.ToString());
        }

        private void SendNewClientAlertEmail()
        {
            if (emailer == null)
                emailer = new Emailer(Settings.Default.SmtpHost, Settings.Default.EmailStyleTag);

            emailer.SendNewClientAlertEmail(userEmail);
        }

        //private void GetAspnetUserInfo()
        //{
        //}
    }
}