using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using System.Web.Security;
using HesterConsultants.admin;

namespace HesterConsultants.employees
{
    public partial class Default : System.Web.UI.Page
    {
        private Employee curEmployee;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curEmployee = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // get logged on ee
            if (curEmployee == null)
                return;

            SetControls();
        }

        private void SetControls()
        {
        }

        //protected string DateFormat(DateTime dt)
        //{
        //    return dt.ToString("M/d/yyyy" + "<br />") + dt.ToString("h:mm tt");
        //}

        protected DateTime AdminDate(DateTime utcDate)
        {
            return AdminUtils.EmployeeDate(utcDate, curEmployee);
        }
    }
}