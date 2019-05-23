using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HesterConsultants.services
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            header1.PageInfo = HesterConsultants.AppCode.PageInfo.ServicesRelative;
        }
    }
}
