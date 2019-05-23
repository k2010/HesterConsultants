using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;

namespace HesterConsultants.client_experience
{
    public partial class Default : System.Web.UI.Page
    {
        private int pageId = 18;

        protected void Page_Load(object sender, EventArgs e)
        {
            HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();
        }
    }
}