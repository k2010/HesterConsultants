using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.Properties;
using System.Net.Mail;
using System.Web.Security;

namespace HesterConsultants.login
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private string email;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TextBox txtEmail = (TextBox)this.PasswordRecovery1.UserNameTemplateContainer.FindControl("UserName");
            if (txtEmail != null)
            {
                txtEmail.Attributes.Add("autocorrect", "off");
                txtEmail.Attributes.Add("autocapitalize", "off");
            }
        }

        protected string EmailTextBoxName()
        {
            string ret = String.Empty;

            TextBox txtEmail = (TextBox)this.PasswordRecovery1.UserNameTemplateContainer.FindControl("UserName");

            if (txtEmail != null)
                ret = txtEmail.ClientID;

            return ret;
        }

        protected string EmailSubmitButtonName()
        {
            Button submit = (Button)this.PasswordRecovery1.UserNameTemplateContainer.FindControl("SubmitButton");
            return submit.ClientID;
        }

        protected string AnswerTextBoxName()
        {
            TextBox txtEmail = (TextBox)this.PasswordRecovery1.QuestionTemplateContainer.FindControl("Answer");
            return txtEmail.ClientID;
        }

        protected string AnswerSubmitButtonName()
        {
            Button submit = (Button)this.PasswordRecovery1.QuestionTemplateContainer.FindControl("SubmitButton");
            return submit.ClientID;
        }

        protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
        {
            e.Message.Subject = "Technical Support";
            e.Message.Bcc.Add(new MailAddress(Settings.Default.CompanyContactEmail));
            e.Message.Body += "\nPlease note that you will be required to change your password upon logging in.";
        }

        private void AssignMustChangePasswordRole()
        {
            Roles.AddUserToRole(email, Settings.Default.RoleMustChangePassword);
        }

        protected void PasswordRecovery1_VerifyingAnswer(object sender, LoginCancelEventArgs e)
        {
            PasswordRecovery pwr = (PasswordRecovery)sender;
            email = pwr.UserName;
            if (!Roles.IsUserInRole(email, Settings.Default.RoleMustChangePassword))
                AssignMustChangePasswordRole();
        }
    }
}