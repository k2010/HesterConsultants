using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HesterConsultants.AppCode;

namespace HesterConsultants.temp09132012
{
	public partial class GetFile : System.Web.UI.Page
	{
		private string filename = "TxCDCInstallerFiles.zip";

		protected void Page_Load(object sender, EventArgs e)
		{
			this.Response.ContentType = "application/octet-stream";
			this.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
			this.Response.TransmitFile(this.Server.MapPath("./" + filename));
			HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

	}
}