using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;

namespace HesterConsultants.about
{
    public partial class Default : System.Web.UI.Page
    {
        private int pageId = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            header1.PageInfo = PageInfo.AboutRelative;

            HesterConsultants.AppCode.HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }
    }
}
