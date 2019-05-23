using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;
using System.Web.Security;
using System.Diagnostics;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.Properties;

namespace HesterConsultants.clients
{
    public partial class Profile : System.Web.UI.Page
    {
        // could come to this page as:
        //      - new client (auth user but not Client - redirected - needs to fill in info)
        //          -so don't use ClientUtils.[redirect method]
        //      - regular client (review/change info)

        private MembershipUser user = Membership.GetUser(); // /clients dir not accessible unless authenticated
        private Client curClient = null;
        protected bool newClient = false;
        private int pageId = 11;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // get user name or email
            // new clients have email but no name
            GetClientInfoFromLogin();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (curClient == null)
                return;

            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Debug.WriteLine("postback: " + this.IsPostBack.ToString());
            if (!this.IsPostBack)
                SetControls();

            HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // iOs attrs for inputs
            SiteUtils.AddTextBoxAttributes(this, "autocorrect", "off", true);
            SiteUtils.AddTextBoxAttributes(this, "autocapitalize", "off", true);
        }

        protected string MainBodyOrMainBodyClients()
        {
            return (newClient) ? "mainbody" : "mainbodyClients";
        }

        protected string CurCompanyId()
        {
            if (curClient != null)
                return curClient.Company.CompanyId.ToString();
            else
                return String.Empty;
        }

        private void GetClientInfoFromLogin()
        {
            // check for must change pw
            if (Roles.IsUserInRole(Settings.Default.RoleMustChangePassword))
            {
                this.Response.Redirect(Settings.Default.ChangePasswordUrl, true);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            // new client isn't Client yet
            curClient = CacheLayer.ClientFromAuthUserId((Guid)user.ProviderUserKey); 
            newClient = (curClient == null);

            // show client bar if existing Client
            if (!newClient)
                this.ccp1.Visible = true;
        }

        private void SetControls()
        {
            // fill in for existing clients
            if (curClient != null)
            {
                this.txtFirstname.Text = curClient.FirstName;
                this.txtLastname.Text = curClient.LastName;
                this.txtPhone.Text = curClient.Phone;
                this.txtCompany.Text = curClient.Company.Name;
                this.txtAddress1.Text = curClient.Address.Address1;
                this.txtAddress2.Text = curClient.Address.Address2;
                this.txtCity.Text = curClient.Address.City;
                this.txtState.Text = curClient.Address.State;
                this.txtZip.Text = curClient.Address.PostalCode;
                this.txtCountry.Text = curClient.Address.Country;
                // not using hidden fields at the moment
                //this.hidCompanyId.Value = curClient.CompanyId.ToString();
                //this.hidAddressId.Value = curClient.AddressId.ToString();
                this.ddTimezones.SelectedValue = curClient.TimeZoneId;

                this.lblUser.Text = curClient.FirstName + " " + curClient.LastName;
            }
            else
            {
                this.lblUser.Text = user.UserName;
                this.phNewClientInfo.Controls.Add(
                    new LiteralControl("<p class=\"gentleHelpText\">Please enter your account information below to use our site. Thank you.</p>"));
            }
        }

        private void UpdateClientData()
        {
            string firstName = this.txtFirstname.Text.Trim();
            string lastName = this.txtLastname.Text.Trim();
            Guid authUserId = (Guid)user.ProviderUserKey;
            string phone = this.txtPhone.Text.Trim();
            DateTime nowUniv = DateTime.UtcNow;

            string companyName = this.txtCompany.Text.Trim();
            // use person as company if no company
            if (String.IsNullOrEmpty(companyName))
                companyName = "[" + firstName + " " + lastName + "]";

            string addr1 = this.txtAddress1.Text.Trim();
            string addr2 = this.txtAddress2.Text.Trim();
            string city = this.txtCity.Text.Trim();
            string state = this.txtState.Text.Trim();
            string zip = this.txtZip.Text.Trim();
            string country = this.txtCountry.Text.Trim();
            string timeZone = this.ddTimezones.SelectedValue;

            // determine if new company
            Company company = CacheLayer.CompanyFromName(companyName);

            // insert new company
            if (company == null)
                company = InsertNewCompany(companyName);

            // determine if new address
            Address address = CacheLayer.AddressFromStrings(
                addr1 + addr2 + city + state + zip + country);

            if (address == null)
                address = InsertNewAddress(addr1, addr2, city, state, zip, country, company);

            UpdateClient(firstName, lastName, phone, authUserId, user.UserName, company, address, timeZone); // user.UserName is email address
        }

        private Company InsertNewCompany(string companyName)
        {
            Company newCompany = new Company(companyName);
            CacheLayer.InsertCompany(newCompany); // assigns id, puts in cache and db
            
            return newCompany; // has id now
        }

        private Address InsertNewAddress(string addr1, string addr2, string city, string state, string zip, string country, Company company)
        {
            Address address = new Address(addr1, addr2, city, state, zip, country);
            CacheLayer.InsertAddress(address);

            return address;
        }

        private void UpdateClient(string firstName, string lastName, string phone, Guid authUserId, string email, Company company, Address address, string timeZone)
        {
            if (newClient)
            {
                curClient = new Client(firstName, lastName, phone, authUserId, email, company, address, timeZone);
                CacheLayer.InsertClient(curClient);
                
            }
            else
            {
                curClient = curClient.ShallowCopy();

                curClient.FirstName = firstName;
                curClient.LastName = lastName;
                curClient.Phone = phone;
                curClient.Company = company;
                curClient.Address = address;
                curClient.TimeZoneId = timeZone;

                CacheLayer.UpdateClient(curClient);
            }

            // session
            this.Session[Global.SESSION_CLIENT] = curClient;
        }

        private void UpdateUserFromNewClientToClient()
        {
            // update user role
            if (User.IsInRole(Settings.Default.RoleNewClient))
                Roles.RemoveUserFromRole(user.UserName, Settings.Default.RoleNewClient);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            UpdateClientData();
            UpdateUserFromNewClientToClient();

            this.Response.Redirect(Settings.Default.ClientHomeUrl, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void ddTimezones_Load(object sender, EventArgs e)
        {
            ListItem li = new ListItem("Select:", "0");

            ((DropDownList)sender).Items.Add(li);
        }
    }
}