using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using Date = System.DateTime;


namespace CTBTeam {
	public partial class ToeTruck : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
		}

		protected void downloadToeTruck(object sender, EventArgs e) {
			Response.ContentType = "Application/THEFILETYPE";
			Response.AppendHeader("Content-Disposition", "attachment; filename=ToeTruck.THEFILETYPE");
			Response.TransmitFile(Server.MapPath("~/ToeTruck.THEFILETYPE"));
			Response.End();
		}
	}
}