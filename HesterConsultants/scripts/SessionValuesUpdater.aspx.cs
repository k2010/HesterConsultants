using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HesterConsultants.scripts
{
    public partial class SessionValuesUpdater : System.Web.UI.Page
    {
        // this calls AppCode.SessionUpdater, 
        // which updates fields in the session record
        // that relate to the browser client.
        // this is called by ajax from client script.

        //protected System.Data.OleDb.OleDbConnection conn;
        private string callingScript;

        private void Page_Load(object sender, System.EventArgs e)
        {
            this.Response.ContentType = "application/x-javascript";

            HesterConsultants.AppCode.SessionUpdater updater = new HesterConsultants.AppCode.SessionUpdater(this);
            updater.RunUpdate();

            callingScript = this.Request.QueryString[HesterConsultants.AppCode.SiteUtils.QS_CALLING_SCRIPT] != null ? this.Request.QueryString[HesterConsultants.AppCode.SiteUtils.QS_CALLING_SCRIPT] : "___";
            
            Response.Clear();
            // write only a js comment
            //Response.Write("alert(" + this.Request.Url + ");");
            Response.Write("// " + callingScript + ".RanUpdate = true;\n");
            HesterConsultants.AppCode.SiteUtils.SetWroteClientValsFlag(this);
        }
    }
}
