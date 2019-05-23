using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode.Entities;
using HesterConsultants.AppCode;

namespace HesterConsultants.admin
{
    public partial class Utilities : System.Web.UI.Page
    {
        private Employee curAdmin;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            curAdmin = AdminUtils.GetEmployeeFromSessionOrLogOut();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            for (int k = 1; k <= 20; k++)
                ddNumJobs.Items.Add(k.ToString());

            for (int k = 0; k < 5; k++)
                ddNumDocuments.Items.Add(k.ToString());
        }

        private void SetControls()
        {
            ddNumJobs.SelectedValue = "5";
            ddNumDocuments.SelectedValue = "2";
        }

        protected void btnInsertJobs_Click(object sender, EventArgs e)
        {
            int numJobs = Convert.ToInt32(ddNumJobs.SelectedValue);
            int numDocuments = Convert.ToInt32(ddNumDocuments.SelectedValue);

            ClientData.Current.InsertDummyJobs(numJobs, numDocuments);
        }
    }
}