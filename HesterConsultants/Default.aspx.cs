using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;
using HesterConsultants.Properties;
using System.Web.Security;
using HesterConsultants.AppCode.Entities;
using System.Diagnostics;

namespace HesterConsultants
{
    public partial class _Default : System.Web.UI.Page
    {
        private int pageId = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            //debug
            //CacheLayer.ClearCache();
            
            header1.PageInfo = PageInfo.HomeRelative;

            HesterConsultants.AppCode.HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();

            SetControls();
        }

        protected int DoSlide()
        {
            int ret = 0;

            if (this.Request.Cookies[Settings.Default.SiteCookieName] != null
                &&
                this.Request.Cookies[Settings.Default.SiteCookieName][Settings.Default.SplashCookieName] != null)
            {
                DateTime lastSplashDate = DateTime.MinValue;
                bool goodDate = DateTime.TryParse(this.Request.Cookies[Settings.Default.SiteCookieName][Settings.Default.SplashCookieName], out lastSplashDate);
                Debug.WriteLine("last splash: " + lastSplashDate.ToString());

                if (goodDate)
                {
                    if (DateTime.UtcNow.AddHours(-6.0) > lastSplashDate)
                    {
                        ret = 1;
                        // reset the cookie
                        this.Response.Cookies[Settings.Default.SiteCookieName][Settings.Default.SplashCookieName] = DateTime.UtcNow.ToString();
                    }
                    else // less than 6 hours ago
                        ret = 0;
                }
                else // bad date
                {
                    ret = 1;
                    // set cookie
                    this.Response.Cookies[Settings.Default.SiteCookieName][Settings.Default.SplashCookieName] = DateTime.UtcNow.ToString();
                }
            }
            else // no cookie
            {
                ret = 1;
                // set cookie
                this.Response.Cookies[Settings.Default.SiteCookieName][Settings.Default.SplashCookieName] = DateTime.UtcNow.ToString();
            }

            return ret;
        }

        //protected string UserNameTextBox()
        //{
        //    TextBox username = Login1.FindControl("UserName") as TextBox;
        //    if (username != null && username.Visible)
        //        return username.ClientID;
        //    else
        //        return String.Empty;
        //}

        //protected string PwTextBox()
        //{
        //    TextBox pw = Login1.FindControl("Password") as TextBox;
        //    if (pw != null && pw.Visible)
        //        return pw.ClientID;
        //    else
        //        return String.Empty;
        //}

        protected void RedirectToContact(object sender, EventArgs e)
        {
            this.Response.Redirect(Settings.Default.ContactUrl, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void SetControls()
        {
            Client curClient = this.Session[Global.SESSION_CLIENT] as Client;
            if (curClient != null)
                this.pnlLogin.Visible = false;
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            // check for new client that logged in without token
            // need token to verify email
            //string userName = Login1.UserName;
            //MembershipUser authUser = Membership.GetUser(userName);
            //Guid authUserId = (Guid)authUser.ProviderUserKey;

            //if (Roles.IsUserInRole(userName, Settings.Default.RoleUnauthenticatedClient))
            //{
            //    try
            //    {
            //        Guid guid = new Guid(strToken);
            //        bool ret = ClientData.Current.VerifyNewClientToken(authUserId, new Guid(strToken)); // sets auth flag in db if good
            //        Debug.WriteLine("verified token: " + ret.ToString());
            //    }
            //    catch (ArgumentNullException aex)
            //    {
            //        ClientData.Current.LogErrorAndSendAlert(aex, userName + " tried to log on without a token.");
            //    }
            //    catch (FormatException fex)
            //    {
            //        ClientData.Current.LogErrorAndSendAlert(fex, userName + " tried to log with token in bad format.");
            //    }
            //}                
        }
    }
}
