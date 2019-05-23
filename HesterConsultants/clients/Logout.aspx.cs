using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HesterConsultants.AppCode;
using HesterConsultants.Properties;

namespace HesterConsultants.clients
{
    public partial class Logout : System.Web.UI.Page
    {
        private int pageId = 14;

        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            this.Session[Global.SESSION_CLIENT] = null;

            this.Response.Redirect("/", false);

            HitHandler hit1 = new HitHandler(this, this.pageId);
            hit1.HandlePage();

            //this.Session.Abandon();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}