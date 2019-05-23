using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.Properties;
using HesterConsultants.AppCode;

namespace HesterConsultants.admin
{
    public partial class AuthUsers : System.Web.UI.Page
    {
        private Employee curAdmin;
        private List<MembershipUser> curUsers;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (curAdmin == null)
                return;

            GetLoggedOnUsers();
            SetControls();
        }

        private void GetLoggedOnUsers()
        {
            curUsers = new List<MembershipUser>();
            foreach (MembershipUser user in Membership.GetAllUsers())
                if (user.IsOnline)
                    curUsers.Add(user);
        }

        private void SetControls()
        {
            // users
            if (curUsers.Count == 0)
                phCurUsers.Controls.Add(new LiteralControl("[None]"));

            else
            {
                foreach (MembershipUser user in curUsers)
                {
                    phCurUsers.Controls.Add(new LiteralControl("<div>" + user.UserName + " - since " + user.LastLoginDate.ToString("M/d h:mm tt") + "</div>"));
                    
                }
            }
        }

        protected void lvAuthUsers_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Add"))
            {
                ListView lv = (ListView)sender;
                lv.InsertItemPosition = InsertItemPosition.LastItem;
            }
        }

        protected void lvAuthUsers_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            // insert a MembershipUser here;
            // no need to go to wrapper class
            ListViewItem item = e.Item;
            TextBox txtUserName = item.FindControl("txtUserName") as TextBox;
            TextBox txtPw = item.FindControl("txtPw") as TextBox;
            TextBox txtQ = item.FindControl("txtSecQuestion") as TextBox;
            TextBox txtA = item.FindControl("txtSecAnswer") as TextBox;

            string userName = txtUserName.Text.Trim();
            string pw = txtPw.Text.Trim();
            string q = txtQ.Text.Trim();
            string a = txtA.Text.Trim();
            MembershipCreateStatus status = MembershipCreateStatus.UserRejected;

            // create user
            MembershipUser newUser = Membership.CreateUser(userName, pw, userName, q, a, true, out status);
            if (status != MembershipCreateStatus.Success)
                throw new Exception("CreateUser status: " + status.ToString());

            // put them in client role
            Roles.AddUserToRole(userName, Settings.Default.RoleClient);

            // insert into client db
            Address address = CacheLayer.AddressFromId(Settings.Default.DefaultAddressId);
            Company company = CacheLayer.CompanyFromId(Settings.Default.DefaultCompanyId);

            Guid guid = (Guid)(newUser.ProviderUserKey);

            Client client = new Client("[First]", "[Last]", "[Phone]", guid, newUser.UserName, company, address, Settings.Default.CompanyTimeZoneId);

            CacheLayer.InsertClient(client);

            // no need to go to wrapper
            e.Cancel = true;

            this.Response.Redirect("/admin/AuthUserManage.aspx?userName=" + userName, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}