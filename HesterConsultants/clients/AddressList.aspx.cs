using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using System.Diagnostics;
using System.Text;

namespace HesterConsultants.clients
{
    /// <summary>
    /// Ajax to return addresses for a given company.
    /// </summary>
    public partial class AddressList : System.Web.UI.Page
    {
        private string term;
        private Company curCompany;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.ContentType = "application/json";

            // if no company in Session, respond with empty
            if ((curCompany = this.Session[Global.SESSION_COMPANY] as Company) == null)
            {
                WriteEmptyJson();
                return;
            }

            // check term qs
            term = this.Request.QueryString["term"];
            if (String.IsNullOrEmpty(term))
            {
                WriteEmptyJson();
                return;
            }

            GetInitialAddresses();
        }

        private void GetInitialAddresses()
        {
            List<Address> allAddresses = CacheLayer.AllAddresses();
            List<Client> allClients = CacheLayer.AllClients();

            Debug.WriteLine("cur company: " + curCompany.Name);

            List<Client> companyClients = allClients.Where(c => c.Company.Equals(curCompany)).ToList();
            Debug.WriteLine("companyClients null: " + (companyClients == null).ToString());
            Debug.WriteLine("companyClients empty: " + (companyClients.Count == 0).ToString());
            Debug.WriteLine("company clients:");
            foreach (Client client in companyClients)
                Debug.WriteLine(client.LastName);

            // addresses that match clients of the company
            List<Address> matchingAddresses = companyClients.Select(c => c.Address).ToList();

            // now, match the qs
            matchingAddresses = matchingAddresses.Where(a => a.Address1.ToLower().StartsWith(term.ToLower())).ToList();

            Debug.WriteLine("matching addresses:");
            foreach (Address address in matchingAddresses)
                Debug.WriteLine(address.Address1);

            StringBuilder json = new StringBuilder();

            json.Append("[");

            if (matchingAddresses.Count == 0)
                // return empty json
                WriteEmptyJson();

            else
            {
                foreach (Address address in matchingAddresses)
                {
                    json.Append("{");
                    json.Append("\"id\": \"");
                    json.Append(address.AddressId.ToString());
                    json.Append("\",");
                    json.Append("\"label\": \"");
                    json.Append(address.Address1);
                    json.Append("\",");
                    json.Append("\"address1\": \"");
                    json.Append(address.Address1);
                    json.Append("\",");
                    json.Append("\"address2\": \"");
                    if (address.Address2 != null)
                        json.Append(address.Address2);
                    json.Append("\",");
                    json.Append("\"city\": \"");
                    json.Append(address.City);
                    json.Append("\",");
                    json.Append("\"state\": \"");
                    json.Append(address.State);
                    json.Append("\",");
                    json.Append("\"postalCode\": \"");
                    json.Append(address.PostalCode);
                    json.Append("\",");
                    json.Append("\"country\": \"");
                    if (address.Country != null)
                        json.Append(address.Country);
                    json.Append("\"");
                    json.Append("},");
                }

                // remove last comma
                json.Remove(json.Length - 1, 1);
            }

            json.Append("]");

            Debug.WriteLine(json.ToString());

            this.Response.Clear();
            this.Response.Write(json.ToString());
            //this.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void WriteEmptyJson()
        {
            this.Response.Clear();
            this.Response.Write("{\"id\": \"\", \"label\": \"\"}");
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}