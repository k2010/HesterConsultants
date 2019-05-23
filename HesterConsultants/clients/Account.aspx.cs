using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using System.Text;
using HesterConsultants.admin;
using HesterConsultants.AppCode;
using HesterConsultants.Properties;

namespace HesterConsultants.clients
{
    public partial class Account : System.Web.UI.Page
    {
        private Client curClient;
        //private List<BalanceDue> balances;
        private List<Invoice> unpaidInvoices;
        private int pageId = 17;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            ClientUtils.RedirectNewOrUnauthenticatedClient();
            curClient = ClientUtils.GetClientFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.odsCurrentInvoice.SelectParameters.Clear();

            GetAccountData();
            SetControls();

            HitHandler hit1 = new HitHandler(this, pageId);
            hit1.HandlePage();
       }

        private void GetAccountData()
        {
            //balances = curClient.GetBalances();
            unpaidInvoices = new Invoices().UnpaidInvoicesForClient(curClient).ToList();
        }

        private void SetControls()
        {
            this.lblClientName.Text = curClient.FirstName + " " + curClient.LastName;
            this.phInvoices.Controls.Add(new LiteralControl(BalancesTable()));

            // note
            if (unpaidInvoices.Count > 0)
            {
                // to do - decouple paypal
                phNote.Controls.Add(new LiteralControl("<p>Please check your email for your complete invoice.</p>"
                    + "<p>Payment may be made by check or money order to:</p>"
                    + "<p>" + Settings.Default.CompanyName 
                    + "</br />"
                    + Settings.Default.CompanyAddress
                    + "</p>"
                    + "<p><em>or</em></p>"
                    + "<p>"
                    + "By using <a href=\"https://www.paypal.com/\">PayPal</a>, sending payment to: account "
                    + "<strong>" + Settings.Default.CompanyContactEmail + "</strong>"
                    + " or mobile "
                    + "<strong>" + Settings.Default.CompanyContactPhone + "</strong>"
                    + ".</p>"));
                    
            }
        }

        private string BalancesTable()
        {
            StringBuilder sb = new StringBuilder();
            string n = Environment.NewLine;

            sb.Append("<table style=\"width: 500px;\">" + n);
            sb.Append("<tr>" + n);
            sb.Append("<th>Invoice(s)</th>" + n);
            //sb.Append("<th>Due</th>" + n);
            sb.Append("<th class=\"right\">Amount</th>" + n);
            sb.Append("</tr>" + n);

            // invoices are ordered from latest
            int k = 0;
            decimal total = 0m;
            foreach (Invoice invoice in unpaidInvoices)
            {
                string cssClass = String.Empty;
                if (k > 1)
                    cssClass = "pastDueCritical";
                else if (k == 1)
                    cssClass = "pastDueModerate";

                sb.Append("<tr class=\"" + cssClass + "\">" + n);
                sb.Append("<td>" + invoice.InvoiceId.ToString() + "</td>" + n);

                //sb.Append("<td>" + invoice.DateDue.ToString("MMMM d, yyyy") + "</td>" + n);
                sb.Append("<td class=\"right\">" + invoice.AmountDue.ToString("c") + "</td>" + n);

                sb.Append("</tr>" + n);

                total += invoice.AmountDue;
                k++;
            }

            sb.Append("<tr class=\"totalRow\">" + n);
            sb.Append("<td><strong>Total</strong></td>" + n);
            sb.Append("<td class=\"right\"><strong>" + total.ToString("c") + "</strong></td>" + n);
            sb.Append("</tr>" + n);

            sb.Append("</table>" + n);

            return sb.ToString();
        }

        protected void odsCurrentInvoice_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters.Add("client", curClient);
        }
    }
}