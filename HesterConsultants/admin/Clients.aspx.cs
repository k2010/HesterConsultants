using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;

namespace HesterConsultants.admin
{
    public partial class Clients : System.Web.UI.Page
    {
        private Employee curAdmin;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void lvClients_Sorting(object sender, ListViewSortEventArgs e)
        {
            string amountDueSort = "sortedOnAmountDue";
            string numJobsSort = "numJobs";

            // make first call on Amount Due, # Jobs descending
            if (e.SortExpression == "AmountDue")
            {
                if (ViewState[amountDueSort] == null || !(bool)ViewState[amountDueSort])
                {
                    e.SortDirection = SortDirection.Descending;
                    this.ViewState[amountDueSort] = true;
                }
            }
            else if (e.SortExpression == "NumberOfJobs")
            {
                if (ViewState[numJobsSort] == null || !(bool)ViewState[numJobsSort])
                {
                    e.SortDirection = SortDirection.Descending;
                    this.ViewState[numJobsSort] = true;
                }
            }
        }
    }
}