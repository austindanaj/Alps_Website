using System;


namespace CTBTeam {
	public partial class ttc823jdnajs3 : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (Session["toetruck"] == null)
				redirectSafely("~/ToeTruck");
		}

		protected void downloadToeTruck(object sender, EventArgs e) {
			bool q1Correct, q2Correct, q3Correct, q4Correct;

			q1Correct = q1.Text.Equals("24");
			q2Correct = q2.Text.Equals("-1/12");
			q3Correct = q3.Text.ToLower().Equals("complement");
			q4Correct = q4.Text.ToLower().Equals("o(n)");

			if (!(q1Correct && q2Correct && q3Correct && q4Correct))
				return;

			Response.ContentType = "Application/png";
			Response.AppendHeader("Content-Disposition", "attachment; filename=ToeTruck.png");
			Response.TransmitFile(Server.MapPath("~/Images/ToeTruck.png"));
			Response.End();
		}
	}
}