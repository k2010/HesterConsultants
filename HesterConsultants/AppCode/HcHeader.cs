using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using System.Web.UI;
using System.Text;
using System.Web.Security;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.Properties;
using HesterConsultants.AppCode;

namespace HesterConsultants.AppCode
{
    [
        AspNetHostingPermission(SecurityAction.Demand,
            Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
            Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        ToolboxData("<{0}:HcHeader runat=\"server\"> </{0}:HcHeader>")
    ]
    public class HcHeader : System.Web.UI.WebControls.WebControl
    {
        private PageInfo pageInfo;
        private const string SPACER = "&nbsp;|&nbsp;";

        public PageInfo PageInfo
        {
            get
            {
                return pageInfo;
            }
            set
            {
                pageInfo = value;
            }
        }

        private string Header()
        {
            StringBuilder sbHeader = new StringBuilder();

            sbHeader.Append("<div id=\"header\">");
            sbHeader.Append(Logo());
            sbHeader.Append(TopNav());
            sbHeader.Append("</div>");

            return sbHeader.ToString();
        }

        private string Logo()
        {
            StringBuilder sbLogo = new StringBuilder();
            bool notOnHomePage = this.pageInfo == null || this.pageInfo.PageId != PageInfo.HomeRelative.PageId;

            sbLogo.Append("<div id=\"logoContainer\">");
            sbLogo.Append("<span id=\"logo\">");
            if (notOnHomePage)
                sbLogo.Append("<a href=\"/\">");
            sbLogo.Append(Settings.Default.CompanyName);
            if (notOnHomePage)
                sbLogo.Append("</a>");
            sbLogo.Append("</span>");
            sbLogo.Append("<span id=\"slogan\"");
            //if (withEffect)
            //{
            //    if (this.Page.Request.Cookies["hcv1"] != null
            //            &&
            //        this.Page.Request.Cookies["hcv1"]["allowFade"] != null)
            //    {
            //        if (this.Page.Request.Cookies["hcv1"] == null
            //                ||
            //                this.Page.Request.Cookies["hcv1"]["sloganFader"] == null)
            //            sbLogo.Append(" style=\"color: #00aaee;\"");
            //    }
            //}
            sbLogo.Append(">");
            sbLogo.Append(Settings.Default.CompanySlogan);
            sbLogo.Append("</span>");
            sbLogo.Append("</div>");

            return sbLogo.ToString();
        }

        private string TopNav()
        {
            StringBuilder sbNav = new StringBuilder();
            PageInfo[] pages;
            HttpContext context = HttpContext.Current;

            //if (current.Session[SiteUtils.SESSION_LOGIN_LEVEL] != null
            //        && (Global.LoginLevels)current.Session[SiteUtils.SESSION_LOGIN_LEVEL] ==
            //        Global.LoginLevels.AdminLevel)
            //    pages = Global.pages_admin;
            //else

            if (context.Session[SiteUtils.SESSION_LOCAL] != null
                    &&
                    Convert.ToBoolean(context.Session[SiteUtils.SESSION_LOCAL]) == true)
            {
                if (SiteUtils.DevServer())
                    pages = Global.pages_local;
                else if (context.Session[SiteUtils.SESSION_LOCAL] != null
                        &&
                        Convert.ToBoolean(context.Session[SiteUtils.SESSION_LOCAL]) == true)
                    pages = Global.pages_admin;
                else
                    pages = Global.pages_public;
            }
            else
                pages = Global.pages_public;

                    
            // debug
            pages = Global.pages_public;

            sbNav.Append("<div id=\"topNav\">");

            // login
            sbNav.Append(LoginStatusSection());

            // links
            foreach (PageInfo page in pages)
                sbNav.Append(NavLink(page) + SPACER);

            //sbNav.Append(ClientLink());

            sbNav.Remove(sbNav.Length - SPACER.Length, SPACER.Length);

            sbNav.Append("</div>");

            return sbNav.ToString();
        }

        private string NavLink(PageInfo page)
        {
            if (this.pageInfo != null && page.PageId == this.pageInfo.PageId)
                return "<span class=\"activeNavLink\">" + this.pageInfo.PageName + "</span>";

            else
                return "<a href=\"" + page.Url + "\">"
                    + page.PageName + "</a>";
        }

        private string LoginStatusSection()
        {
            StringBuilder sbLogin = new StringBuilder();
            Client curClient = null;
            string clientName = String.Empty;

            sbLogin.Append("<div class=\"loginTop\">");

            if (this.Page.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.Session[Global.SESSION_CLIENT] != null)
                {
                    curClient = HttpContext.Current.Session[Global.SESSION_CLIENT] as Client;
                    if (curClient != null)
                        clientName = curClient.FirstName + " " + curClient.LastName;
                    else
                        clientName = Membership.GetUser().UserName;
                    sbLogin.Append(clientName + " - <a href=\"/clients/Logout.aspx\">Log Out</a>");
                }
            }
            else
                sbLogin.Append("<a href=\"/login/\">Login</a>");

            sbLogin.Append("</div>");

            return sbLogin.ToString();
        }

        //private string ClientLink()
        //{
        //    return "<a href=\"" + PageInfo.ClientsRelative.Url + "\" class=\"clientsLink\">" + PageInfo.ClientsRelative.PageName + "</a>";
        //}

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write(Header());
        }
    }
}
