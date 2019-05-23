using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;

namespace HesterConsultants.admin.reports
{
    public partial class Hits : System.Web.UI.Page
    {
        private int pageId = 5;
        protected int sessionId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.header1.PageInfo = PageInfo.ReportsRelative;

            HesterConsultants.AppCode.HitHandler hit1 = new HesterConsultants.AppCode.HitHandler(this, this.pageId);
            hit1.HandlePage();

            if (this.Request.QueryString["sessionId"] != null
                    && this.Request.QueryString["sessionId"] != String.Empty)
                sessionId = Convert.ToInt32(this.Request.QueryString["sessionId"]);
        }
    }
}
