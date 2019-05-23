using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;
using System.Text;
using System.Diagnostics;

namespace HesterConsultants.clients
{
    public partial class CompanyList : System.Web.UI.Page
    {
        private string companyId;
        private string term;

        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("in CompanyList.aspx");

            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.ContentType = "application/json";

            // clear any temp company from Session
            this.Session[Global.SESSION_COMPANY] = null;

            // do different things based on query string
            ProcessQs();
        }

        private void ProcessQs()
        {
            string[] keys = this.Request.QueryString.AllKeys;
            foreach (string key in keys)
                Debug.WriteLine("key: " + key + "\nvalue: " + this.Request.QueryString[key]);

            companyId = this.Request.QueryString["coId"];
            term = this.Request.QueryString["term"];

            if (!String.IsNullOrEmpty(companyId))
            {
                Debug.WriteLine("company id: " + companyId.ToString());
                SetCurrentCompany();
            }
            else if (!String.IsNullOrEmpty(term))
                GetMatchingCompanies();
            else
                WriteEmptyJson();
        }

        private void SetCurrentCompany()
        {
            // set current company in session,
            // so that addresses can be filtered 
            // for the address autocomplete
            this.Session[Global.SESSION_COMPANY] =
                CacheLayer.CompanyFromId(Convert.ToInt32(companyId));

            Debug.WriteLine(((Company)this.Session[Global.SESSION_COMPANY]).Name + " is in Session");
        }

        private void GetMatchingCompanies()
        {
            List<Company> allCompanies = CacheLayer.AllCompanies();
            List<Company> matchingCompanies = allCompanies.Where(c => c.Name.ToLower().StartsWith(term.ToLower())).ToList();

            Debug.WriteLine("matching companies:");
            foreach (Company co in matchingCompanies)
                Debug.WriteLine(" - " + co.Name);

            StringBuilder json = new StringBuilder();

            json.Append("[");
            
            // if no matches, return json with empty strings for props,
            // to prevent error (because jquery ajax expects good json)
            if (matchingCompanies.Count == 0)
            {
                WriteEmptyJson();
                return;
            }

            else
            {
                foreach (Company company in matchingCompanies)
                {
                    json.Append("{");
                    json.Append("\"id\": \"");
                    json.Append(company.CompanyId.ToString());
                    json.Append("\",");
                    json.Append("\"label\": \"");
                    json.Append(company.Name);
                    json.Append("\"},");

                }
                // remove last comma
                json.Remove(json.Length - 1, 1);
            }

            json.Append("]");

            Debug.WriteLine("CompanyList.aspx return: " + json.ToString());
            
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