using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using HesterConsultants.Properties;

namespace HesterConsultants.AppCode
{
    public class HcFooter : System.Web.UI.WebControls.WebControl
    {
        private string CopyrightMessage()
        {
            StringBuilder sbMsg = new StringBuilder();
            int curYear = DateTime.Now.Year;

            sbMsg.Append("<div id=\"footer\">");
            sbMsg.Append("<div id=\"copyright\">");
            sbMsg.Append("Copyright ");
            sbMsg.Append(Settings.Default.CompanyStartYear.ToString() + " ");
            if (curYear != Settings.Default.CompanyStartYear)
                sbMsg.Append("- " + curYear.ToString() + " ");
            sbMsg.Append(Settings.Default.CompanyName);

            sbMsg.Append(" | " + PrivacyLink());
            sbMsg.Append("</div>");
            sbMsg.Append("</div>");

            return sbMsg.ToString();
        }

        private string PrivacyLink()
        {
            return "<a href=\"/about/Privacy.aspx\">Privacy</a>";
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write(CopyrightMessage());
        }
    }
}
