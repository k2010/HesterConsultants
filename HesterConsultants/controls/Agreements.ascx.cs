using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HesterConsultants.controls
{
    public partial class Agreements : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public WebControl Tos
        {
            get
            {
                return this.pnlTos;
            }
        }
    }
}