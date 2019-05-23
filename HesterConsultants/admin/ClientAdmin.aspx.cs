using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using System.Web.Security;
using System.Text;

namespace HesterConsultants.admin
{
    public partial class ClientAdmin : System.Web.UI.Page
    {
        private Employee curAdmin;
        private HesterConsultants.AppCode.Entities.Client curClient;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (curAdmin == null)
                return;

            GetData();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // do this after event handlers
            SetControls();
        }

        private void GetData()
        {
            string qs = this.Request.QueryString["clientId"];

            if (String.IsNullOrEmpty(qs))
                return;

            curClient = CacheLayer.ClientFromId(Convert.ToInt32(qs));
        }

        private void SetControls()
        {
            this.lblName.Text = curClient.FirstName + " " + curClient.LastName;
            this.phEmail.Controls.Add(new LiteralControl("<a href=\"mailto:" + curClient.Email + "\">" + curClient.Email + "</a>"));
            this.lblPhone.Text = curClient.Phone;
            this.lblAddress.Text = curClient.Address.AddressBlock("<br />");
            this.lblTimeZone.Text = curClient.TimeZoneId;

            IList<Invoice> openInvoices = new Invoices().UnpaidInvoicesForClient(curClient);
            StringBuilder sbInvoices = new StringBuilder();
            foreach (Invoice invoice in openInvoices)
            {
                sbInvoices.Append("<a href=\"InvoiceAdmin.aspx?invoiceId=");
                sbInvoices.Append(invoice.InvoiceId.ToString());
                sbInvoices.Append("\">");
                sbInvoices.Append(invoice.InvoiceId.ToString());
                sbInvoices.Append("</a>");
                sbInvoices.Append(" - " + invoice.AmountDue.ToString("c"));
            }

            this.phInvoices.Controls.Add(new LiteralControl(sbInvoices.ToString()));

        }

        private void CreateCurrentInvoice()
        {
            Invoice invoice = new Invoices().CreateCurrentInvoice(curClient, curAdmin);
            if (invoice == null)
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to create invoice."));

            //this.Response.Redirect("/admin/InvoiceAdmin.aspx?invoiceId=" + invoice.InvoiceId.ToString(), false);
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void hlInvoice_Click(object sender, EventArgs e)
        {
            CreateCurrentInvoice();
        }
    }
}