using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using HesterConsultants.AppCode.Entities;

namespace HesterConsultants.clients
{
    /// <summary>
    /// Ajax for returning a time zone id, given a state name as query string
    /// </summary>
    public partial class TimeZoneHelper : System.Web.UI.Page
    {
        private string state;

        private string eastern = "Eastern Standard Time";
        private string central = "Central Standard Time";
        private string mountain = "Mountain Standard Time";
        private string pacific = "Pacific Standard Time";
        private string arizona = "US Mountain Standard Time";
        private string alaska = "Alaskan Standard Time";
        private string hawaii = "Hawaiian Standard Time";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.ContentType = "text/plain";

            state = this.Request.QueryString["state"];
            if (String.IsNullOrEmpty(state))
            {
                ReturnText(String.Empty);
                return;
            }

            // unencode
            state = this.Server.UrlDecode(state);

            Dictionary<string, string> stateTzs = new Dictionary<string, string>();
 
            stateTzs.Add("alabama", central);
            stateTzs.Add("alaska", alaska);
            stateTzs.Add("arizona", arizona);
            stateTzs.Add("arkansas", central);
            stateTzs.Add("california", pacific);
            stateTzs.Add("colorado", mountain);
            stateTzs.Add("connecticut", eastern);
            stateTzs.Add("delaware", eastern);
            stateTzs.Add("florida", eastern);
            stateTzs.Add("georgia", eastern);
            stateTzs.Add("hawaii", hawaii);
            //stateTzs.Add("idaho", );
            stateTzs.Add("illinois", central);
            //stateTzs.Add("indiana", );
            stateTzs.Add("iowa", central);
            stateTzs.Add("kansas", central);
            //stateTzs.Add("kentucky", );
            stateTzs.Add("louisiana", central);
            stateTzs.Add("maine", eastern);
            stateTzs.Add("maryland", eastern);
            stateTzs.Add("massachusetts", eastern);
            stateTzs.Add("michigan", eastern);
            stateTzs.Add("minnesota", central);
            stateTzs.Add("mississippi", central);
            stateTzs.Add("missouri", central);
            stateTzs.Add("montana", mountain);
            //stateTzs.Add("nebraska", );
            stateTzs.Add("nevada", pacific);
            stateTzs.Add("new hampshire", eastern);
            stateTzs.Add("new jersey", eastern);
            stateTzs.Add("new mexico", mountain);
            stateTzs.Add("new york", eastern);
            stateTzs.Add("north carolina", eastern);
            //stateTzs.Add("north dakota", );
            stateTzs.Add("ohio", eastern);
            stateTzs.Add("oklahoma", central);
            stateTzs.Add("oregon", pacific);
            stateTzs.Add("pennsylvania", eastern);
            stateTzs.Add("rhode island", eastern);
            stateTzs.Add("south carolina", eastern);
            //stateTzs.Add("south dakota", );
            //stateTzs.Add("tennessee", );
            stateTzs.Add("texas", central);
            stateTzs.Add("utah", mountain);
            stateTzs.Add("vermont", eastern);
            stateTzs.Add("virginia", eastern);
            stateTzs.Add("washington", pacific);
            stateTzs.Add("west virginia", eastern);
            stateTzs.Add("wisconsin", central);
            stateTzs.Add("wyoming", mountain);
            stateTzs.Add("washington d.c.", eastern);
            stateTzs.Add("washington dc", eastern);
            stateTzs.Add("washington, d.c.", eastern);
            stateTzs.Add("washington, dc", eastern);
            stateTzs.Add("district of columbia", eastern);
            stateTzs.Add("al", central);
            stateTzs.Add("ak", alaska);
            stateTzs.Add("az", arizona);
            stateTzs.Add("ar", central);
            stateTzs.Add("ca", pacific);
            stateTzs.Add("co", mountain);
            stateTzs.Add("ct", eastern);
            stateTzs.Add("de", eastern);
            stateTzs.Add("fl", eastern);
            stateTzs.Add("ga", eastern);
            stateTzs.Add("hi", hawaii);
            //stateTzs.Add("id", );
            stateTzs.Add("il", central);
            //stateTzs.Add("in", );
            stateTzs.Add("ia", central);
            stateTzs.Add("ks", central);
            //stateTzs.Add("ky", );
            stateTzs.Add("la", central);
            stateTzs.Add("me", eastern);
            stateTzs.Add("md", eastern);
            stateTzs.Add("ma", eastern);
            stateTzs.Add("mi", eastern);
            stateTzs.Add("mn", central);
            stateTzs.Add("ms", central);
            stateTzs.Add("mo", central);
            stateTzs.Add("mt", mountain);
            //stateTzs.Add("ne", );
            stateTzs.Add("nv", pacific);
            stateTzs.Add("nh", eastern);
            stateTzs.Add("nj", eastern);
            stateTzs.Add("nm", mountain);
            stateTzs.Add("ny", eastern);
            stateTzs.Add("nc", eastern);
            //stateTzs.Add("nd", );
            stateTzs.Add("oh", eastern);
            stateTzs.Add("ok", central);
            stateTzs.Add("or", pacific);
            stateTzs.Add("pa", eastern);
            stateTzs.Add("ri", eastern);
            stateTzs.Add("sc", eastern);
            //stateTzs.Add("sd", );
            //stateTzs.Add("tn", );
            stateTzs.Add("tx", central);
            stateTzs.Add("ut", mountain);
            stateTzs.Add("vt", eastern);
            stateTzs.Add("va", eastern);
            stateTzs.Add("wa", pacific);
            stateTzs.Add("wv", eastern);
            stateTzs.Add("wi", central);
            stateTzs.Add("wy", mountain);
            stateTzs.Add("dc", eastern);

            if (stateTzs.ContainsKey(state))
                ReturnText(stateTzs[state]);
            else
                ReturnText(String.Empty);
        }

        private void ReturnText(string text)
        {
            this.Response.Clear();
            this.Response.Write(text);
            //this.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}