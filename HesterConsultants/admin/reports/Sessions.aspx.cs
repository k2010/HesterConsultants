using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;
using System.Data;

namespace HesterConsultants.admin.reports
{
    public partial class Sessions : System.Web.UI.Page
    {
        private int pageId = 4;
        private string visitorId = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            // debug
            //object addr = this.Session[HesterConsultants.AppCode.SiteUtils.SESSION_REMOTE_ADDR];
            //if (addr != null)
            //    this.lblSessionRemoteAddr.Text = "addr: " + addr;
            //else
            //    this.lblSessionRemoteAddr.Text = "addr: null!";

            this.header1.PageInfo = PageInfo.ReportsRelative;

            HesterConsultants.AppCode.HitHandler hit1 = new HesterConsultants.AppCode.HitHandler(this, this.pageId);
            hit1.HandlePage();

            if (this.Request.QueryString["visitorId"] != null
                    && this.Request.QueryString["visitorId"] != String.Empty)
                visitorId = this.Request.QueryString["visitorId"];

            if (visitorId != String.Empty)
                SqlDataSource1.SelectCommand = "SessionsForVisitor";
            else
            {
                SqlDataSource1.SelectCommand = "GetAllSessions";
                SqlDataSource1.SelectParameters.Clear();
            }
        }

        protected string WhoisUrl(object objIp)
        {
            return SiteUtils.WHOIS_LOOKUP_URL + objIp.ToString();
        }

        protected string ScreenSize(object objW, object objH)
        {
            int w;
            int h;

            if ((w = Convert.ToInt32(objW)) == 0
                    || (h = Convert.ToInt32(objH)) == 0)
                return "–";
            else
                return w.ToString() + "×" + h.ToString();
        }
    }
}
