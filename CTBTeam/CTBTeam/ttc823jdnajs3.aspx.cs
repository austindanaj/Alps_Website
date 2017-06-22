using System;


namespace CTBTeam {
	public partial class ttc823jdnajs3 : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (Session["toetruck"] == null)
				redirectSafely("~/ToeTruck");
		}

		protected void downloadToeTruck(object sender, EventArgs e) {
			Response.ContentType = "Application/png";
			Response.AppendHeader("Content-Disposition", "attachment; filename=ToeTruck.png");
			Response.TransmitFile(Server.MapPath("~/Images/ToeTruck.png"));
			Response.End();
		}
	}
}