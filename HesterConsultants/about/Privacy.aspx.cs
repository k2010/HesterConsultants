using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;

namespace HesterConsultants.about
{
    public partial class Privacy : System.Web.UI.Page
    {
        private int pageId;

        protected void Page_Load(object sender, EventArgs e)
        {
            pageId = 16;
            HitHandler hit1 = new HitHandler(this, pageId);
            hit1.HandlePage();
        }
    }
}