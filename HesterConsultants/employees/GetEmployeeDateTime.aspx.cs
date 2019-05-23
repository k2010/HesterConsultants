using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.admin;
using System.Diagnostics;

namespace HesterConsultants.employees
{
    /// <summary>
    /// Call in ajax to get current time in Employee's time zone.
    /// </summary>
    public partial class GetEmployeeDateTime : System.Web.UI.Page
    {
        private Employee curEmployee;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curEmployee = AdminUtils.GetEmployeeFromSessionOrLogOut();
            Debug.WriteLine("curEmployee: " + curEmployee.LastName);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.ContentType = "text/plain";

            if (curEmployee == null)
                RespondWithError();
            else
                GetResponse();
        }

        private void GetResponse()
        {
            this.Response.Clear();
            this.Response.Write(AdminUtils.StandardDateFormat(AdminUtils.EmployeeDate(DateTime.UtcNow, curEmployee), false));
            //this.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void RespondWithError()
        {
            this.Response.Clear();
            this.Response.Write("Error");
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}