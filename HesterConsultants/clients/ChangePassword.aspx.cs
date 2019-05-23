using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HesterConsultants.Properties;
using HesterConsultants.AppCode;
using HesterConsultants.AppCode.Entities;

namespace HesterConsultants.clients
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private Client curClient;
        private string email;
        private int pageId = 10;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //ClientUtils.RedirectNewOrUnauthenticatedClient(); // causes redirect loop
            curClient = ClientUtils.GetClientFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }

        protected string OrigPwTextBoxName()
        {
            string ret = String.Empty;
            TextBox txtOrigPw = this.changePw.ChangePasswordTemplateContainer.FindControl("CurrentPassword") as TextBox;

            if (txtOrigPw != null)
                ret = txtOrigPw.ClientID;

            return ret;
        }

        protected string ConfirmNewPwTextBoxName()
        {
            string ret = String.Empty;
            TextBox txtConfirmNewPw = this.changePw.ChangePasswordTemplateContainer.FindControl("ConfirmNewPassword") as TextBox;

            if (txtConfirmNewPw != null)
                ret = txtConfirmNewPw.ClientID;

            return ret;
        }

        protected string SubmitButtonName()
        {
            string ret = String.Empty;
            Button btnSubmit = this.changePw.ChangePasswordTemplateContainer.FindControl("ChangePasswordPushButton") as Button;

            if (btnSubmit != null)
                ret = btnSubmit.ClientID;

            return ret;
        }

        protected string ContinueButtonName()
        {
            string ret = String.Empty;
            Button btnContinue = this.changePw.SuccessTemplateContainer.FindControl("ContinuePushButton") as Button;

            if (btnContinue != null)
                ret = btnContinue.ClientID;

            return ret;
        }


        private void RemoveMustChangeRole()
        {
            if (Roles.IsUserInRole(Settings.Default.RoleMustChangePassword))
            {
                email = Membership.GetUser().UserName;
                Roles.RemoveUserFromRole(email, Settings.Default.RoleMustChangePassword);
            }
        }

        protected void changePw_ChangedPassword(object sender, EventArgs e)
        {
            RemoveMustChangeRole();
        }
    }
}