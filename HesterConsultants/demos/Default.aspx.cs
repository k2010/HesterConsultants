using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;

namespace HesterConsultants.demos
{
    public partial class Default : System.Web.UI.Page
    {
        private int pageId = 6;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.header1.PageInfo = PageInfo.DemosRelative;

            HesterConsultants.AppCode.HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }
    }
}
