using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HesterConsultants.controls
{
    /// <summary>
    /// A simple dropdown that includes every hour and half hour
    /// </summary>
    public partial class HalfHourDropDown : System.Web.UI.UserControl
    {
        public string SelectedValue
        {
            get
            {
                return ddHalfHours.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ListItem[] liHalfHours = 
            {
                new ListItem("Midnight", "12:00 am"),
                new ListItem("12:30 am", "12:30 am"),
                new ListItem("1:00 am", "1:00 am"),
                new ListItem("1:30 am", "1:30 am"),
                new ListItem("2:00 am", "2:00 am"),
                new ListItem("2:30 am", "2:30 am"),
                new ListItem("3:00 am", "3:00 am"),
                new ListItem("3:30 am", "3:30 am"),
                new ListItem("4:00 am", "4:00 am"),
                new ListItem("4:30 am", "4:30 am"),
                new ListItem("5:00 am", "5:00 am"),
                new ListItem("5:30 am", "5:30 am"),
                new ListItem("6:00 am", "6:00 am"),
                new ListItem("6:30 am", "6:30 am"),
                new ListItem("7:00 am", "7:00 am"),
                new ListItem("7:30 am", "7:30 am"),
                new ListItem("8:00 am", "8:00 am"),
                new ListItem("8:30 am", "8:30 am"),
                new ListItem("9:00 am", "9:00 am"),
                new ListItem("9:30 am", "9:30 am"),
                new ListItem("10:00 am", "10:00 am"),
                new ListItem("10:30 am", "10:30 am"),
                new ListItem("11:00 am", "11:00 am"),
                new ListItem("11:30 am", "11:30 am"),
                new ListItem("Noon", "12:00 pm"),
                new ListItem("12:30 pm", "12:30 pm"),
                new ListItem("1:00 pm", "1:00 pm"),
                new ListItem("1:30 pm", "1:30 pm"),
                new ListItem("2:00 pm", "2:00 pm"),
                new ListItem("2:30 pm", "2:30 pm"),
                new ListItem("3:00 pm", "3:00 pm"),
                new ListItem("3:30 pm", "3:30 pm"),
                new ListItem("4:00 pm", "4:00 pm"),
                new ListItem("4:30 pm", "4:30 pm"),
                new ListItem("5:00 pm", "5:00 pm"),
                new ListItem("5:30 pm", "5:30 pm"),
                new ListItem("6:00 pm", "6:00 pm"),
                new ListItem("6:30 pm", "6:30 pm"),
                new ListItem("7:00 pm", "7:00 pm"),
                new ListItem("7:30 pm", "7:30 pm"),
                new ListItem("8:00 pm", "8:00 pm"),
                new ListItem("8:30 pm", "8:30 pm"),
                new ListItem("9:00 pm", "9:00 pm"),
                new ListItem("9:30 pm", "9:30 pm"),
                new ListItem("10:00 pm", "10:00 pm"),
                new ListItem("10:30 pm", "10:30 pm"),
                new ListItem("11:00 pm", "11:00 pm"),
                new ListItem("11:30 pm", "11:30 pm")
            };

            this.ddHalfHours.Items.AddRange(liHalfHours);

            // default time 
            if (!this.IsPostBack)
                this.ddHalfHours.SelectedValue = "9:00 am";
        }
    }
}