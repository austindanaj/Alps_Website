using System;


namespace CTBTeam {
	public partial class ttc823jdnajs3 : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (Session["toetruck"] == null)
				redirectSafely("~/ToeTruck");
		}

		protected void download(object sender, EventArgs e) {
			string filename = sender.Equals(btnToeTruck) ? "ToeTruck.png" : "FrankCadi.png" ;
			Response.ContentType = "Application/png";
			Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
			Response.TransmitFile(Server.MapPath("~/Images/" + filename));
			Response.End();
		}
	}
}