using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.Properties;
using HesterConsultants.AppCode.Entities;

namespace HesterConsultants.clients
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            ClientUtils.RedirectNewOrUnauthenticatedClient();
            Client curClient = ClientUtils.GetClientFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Redirect(Settings.Default.ClientHomeUrl, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}