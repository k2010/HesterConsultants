using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.Properties;

namespace HesterConsultants.clients.controls
{
    /// <summary>
    /// Always call this control "ccp1".
    /// </summary>
    public partial class ClientControlPanel : System.Web.UI.UserControl
    {
        public TextBox SearchBox
        { get { return this.txtSearch; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ccp loading");
            this.btnSearch.PostBackUrl = Settings.Default.ClientJobSearchUrl;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.txtSearch.Attributes.Add("autocorrect", "on");
            this.txtSearch.Attributes.Add("autocapitalize", "off");
            //this.txtSearch.Attributes.Add("spellcheck", "off");
        }
    }
}