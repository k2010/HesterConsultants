using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;

namespace HesterConsultants.admin.reports
{
    public partial class Default : System.Web.UI.Page
    {
        private int pageId = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            HesterConsultants.AppCode.HitHandler hit1 = new HesterConsultants.AppCode.HitHandler(this, this.pageId);
            hit1.HandlePage();

            this.Response.Redirect("./Sessions.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
