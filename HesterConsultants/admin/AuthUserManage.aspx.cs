using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using System.Web.Security;
using System.Diagnostics;
using HesterConsultants.Properties;
using HesterConsultants.AppCode;

namespace HesterConsultants.admin
{
    public partial class AuthUserManage : System.Web.UI.Page
    {
        private Employee curAdmin;
        private MembershipUser user = null;
        private List<CheckBox> rolesCheckBoxes;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            GetAdmin();
            if (curAdmin == null)
                return;

            // need to do this before page_load,
            // because dynamically adding controls
            // see http://www.4guysfromrolla.com/articles/092904-1.aspx
            GetUserData();
            InitControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("");
            ShowControlState(this, 0);
            //ShowCheckBoxes();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (user != null)
                SetControls();
        }

        private void GetAdmin()
        {
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        private void GetUserData()
        {
            string qs = this.Request.QueryString["userName"];
            if (!String.IsNullOrEmpty(qs))
                user = Membership.GetUser(qs);
        }

        private void InitControls()
        {
            InitRolesCheckBoxes();
        }

        private void SetControls()
        {
            this.lblUsername.Text = user.UserName;
            
            // show roles
            foreach (CheckBox box in rolesCheckBoxes)
                box.Checked = Roles.IsUserInRole(user.UserName, box.Text);
        }

        private void InitRolesCheckBoxes()
        {
            string[] allRoles = Roles.GetAllRoles();
            rolesCheckBoxes = new List<CheckBox>();
            LiteralControl lineBreak = null;

            foreach (string roleName in allRoles)
            {
                CheckBox chkRole = new CheckBox();
                chkRole.Text = roleName;
                chkRole.ID = "chk" + roleName;
                rolesCheckBoxes.Add(chkRole);

                // add to page
                phRoles.Controls.Add(chkRole);
                lineBreak = new LiteralControl("<br />");
                phRoles.Controls.Add(lineBreak);
            }

            // last line break
            phRoles.Controls.Remove(lineBreak);
            phRoles.Controls.Add(new LiteralControl("<div class=\"caution\">Caution: Do not add Client without also adding New Client.</div>"));
        }

        private void ShowInitialRolesForUser()
        {
            // only if not postback
            if (!this.IsPostBack)
            {
                foreach (CheckBox chkRole in rolesCheckBoxes)
                    chkRole.Checked = Roles.IsUserInRole(user.UserName, chkRole.Text);
            }
        }

        private void UpdateUser()
        {
            //bool newClient = !Roles.IsUserInRole(user.UserName, Settings.Default.RoleClient);
            bool newEmployee = !Roles.IsUserInRole(user.UserName, Settings.Default.RoleEmployee);

            // unlock
            if (user.IsLockedOut && !chkLockedOut.Checked)
                user.UnlockUser();

            bool ret = false;
            // roles 
            // checkboxes are here because added in Init, not Load
            // (before reading viewstate)
            foreach (CheckBox box in rolesCheckBoxes)
            {
                string roleName = box.Text;

                if (box.Checked) // add role if not in already
                {
                    if (!Roles.IsUserInRole(user.UserName, roleName))
                        Roles.AddUserToRole(user.UserName, roleName);

                    // temporary way of adding new employee
                    if (newEmployee && roleName == Settings.Default.RoleEmployee)
                    {
                        Address address = CacheLayer.AddressFromId(Settings.Default.DefaultAddressId);
                        Employee employee = new Employee("[First]", "[Last]", "[Title]", address, "[Phone]", DateTime.UtcNow, (Guid)(user.ProviderUserKey), Settings.Default.CompanyTimeZoneId);

                        ret = Employee.InsertEmployee(employee);
                    }
                }
                else // remove role if in
                {
                    if (Roles.IsUserInRole(user.UserName, roleName))
                        Roles.RemoveUserFromRole(user.UserName, roleName);
                }
            }
        }

        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            UpdateUser();
        }

        private void ShowControlState(Control c, int depth)
        {
            for (int k = 0; k < depth; k++)
            {
                Debug.Write(" ");
            }

            if (c is CheckBox)
            {
                Debug.Write(depth.ToString() + " ");
                Debug.WriteLine(" - checkbox - " + c.GetType().ToString() + " - " + c.ID);
            }

            if (c.Controls.Count > 0)
            {
                depth++;
                foreach (Control c2 in c.Controls)
                    ShowControlState(c2, depth);
            }
        }

        private void ShowCheckBoxes()
        {
            
        }
    }
}